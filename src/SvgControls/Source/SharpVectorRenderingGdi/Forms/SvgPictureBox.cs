using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;

using SharpVectors.Dom.Css;
using SharpVectors.Dom.Svg;
using SharpVectors.Renderers.Gdi;

namespace SharpVectors.Renderers.Forms
{
    /// <summary>
    /// This represents an <c>SVG</c> picture box control for displaying <c>GDI+</c> rendered <c>SVG</c> images.
    /// </summary>
    /// <remarks>This is similar to the Windows picture box control for displaying an image.</remarks>
    [DefaultProperty(nameof(Source))]
    [DefaultBindingProperty(nameof(Source))]
    [Docking(DockingBehavior.Ask)]
    [Designer(typeof(SvgPictureBoxDesigner))]
    public partial class SvgPictureBox : Control, ISvgControl, ISupportInitialize
    {
        #region Private Fields

        [Flags]
        internal enum WindowStyles : uint
        {
            WS_BORDER           = 0x800000,
            WS_CAPTION          = 0xc00000,
            WS_CHILD            = 0x40000000,
            WS_CLIPCHILDREN     = 0x2000000,
            WS_CLIPSIBLINGS     = 0x4000000,
            WS_DISABLED         = 0x8000000,
            WS_DLGFRAME         = 0x400000,
            WS_GROUP            = 0x20000,
            WS_HSCROLL          = 0x100000,
            WS_MAXIMIZE         = 0x1000000,
            WS_MAXIMIZEBOX      = 0x10000,
            WS_MINIMIZE         = 0x20000000,
            WS_MINIMIZEBOX      = 0x20000,
            WS_OVERLAPPED       = 0x0,
            WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_SIZEFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,
            WS_POPUP            = 0x80000000u,
            WS_POPUPWINDOW      = WS_POPUP | WS_BORDER | WS_SYSMENU,
            WS_SIZEFRAME        = 0x40000,
            WS_SYSMENU          = 0x80000,
            WS_THICKFRAME       = 0x40000,
            WS_TABSTOP          = 0x10000,
            WS_VISIBLE          = 0x10000000,
            WS_VSCROLL          = 0x200000
        }

        [Flags]
        internal enum WindowStylesEx : uint
        {
            WS_EX_TRANSPARENT = 0x00000020,
            WS_EX_TOOLWINDOW  = 0x00000080,
            WS_EX_CLIENTEDGE  = 0x00000200
        }

        private const double BitmapLimit          = 23169*23169d;

        private const string DefaultTitle         = "SharpVectors";

        private const string UserCssFileName      = "user.css";
        private const string UserAgentCssFileName = "useragent.css";

        private const string ValidSVG             = "<svg xmlns=\"http://www.w3.org/2000/svg\"></svg>";

        private int _lastPosX = -1;
        private int _lastPosY = -1;

        private bool _isSvgLoaded;
        private bool _isInitializing;

        private string _svgSource;
        private string _xmlSource;
        private Uri _uriSource;
        private MemoryStream _streamSource;

        private SvgPictureBoxWindow _svgWindow;
        private GdiGraphicsRenderer _svgRenderer;

        private string _appTitle;
        private event EventHandler<SvgAlertArgs> _svgAlerts;
        private event EventHandler<SvgErrorArgs> _svgErrors;

        private Size _savedSize;
        /// <summary>
        ///  Controls how the image is placed within our bounds, or how we are sized to fit said image.
        /// </summary>
        private PictureBoxSizeMode _sizeMode = PictureBoxSizeMode.Normal;
        /// <summary>
        ///  The type of border this control will have.
        /// </summary>
        private BorderStyle _borderStyle = BorderStyle.None;

        #endregion

        #region Constructors and Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgPictureBox"/> class.
        /// </summary>
        public SvgPictureBox()
        {
            InitializeComponent();

            _appTitle  = DefaultTitle;
            _sizeMode  = PictureBoxSizeMode.Normal;
            _savedSize = this.Size;

            SetStyle(ControlStyles.Opaque | ControlStyles.Selectable, false);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
//            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);
//            SetStyle(ControlStyles.ResizeRedraw, true);

            //scriptEngineByMimeType = new TypeDictionary();
            //SetMimeTypeEngineType("application/ecmascript", typeof(JScriptEngine));
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                    components = null;
                }
            }

            if (_svgRenderer != null)
            {
                _svgRenderer.Dispose();
                _svgRenderer = null;
            }

            base.Dispose(disposing);
        }

        #endregion

        #region Public Events

        /// <summary>
        /// An event that occurs when there is an alert message to be displayed.
        /// </summary>
        /// <remarks>Handle this to display a customized message box or prevent the display of the alert.</remarks>
        public event EventHandler<SvgAlertArgs> Alert
        {
            add { _svgAlerts += value; }
            remove { _svgAlerts -= value; }
        }

        /// <summary>
        /// An event that occurs when there is an error message to be displayed.
        /// </summary>
        /// <remarks>Handle this to display a customized message box or prevent the display of the error.</remarks>
        public event EventHandler<SvgErrorArgs> Error
        {
            add { _svgErrors += value; }
            remove { _svgErrors -= value; }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///  Indicates the border style for the control.
        /// </summary>
        /// <value>An enumeration of the type <see cref="BorderStyle"/> specifying the border style.</value>
        [DefaultValue(BorderStyle.None)]
        [Category(nameof(Appearance))]
        [Description("The border style for this control")]
        public BorderStyle BorderStyle
        {
            get {
                return _borderStyle;
            }
            set {
                if (_borderStyle != value)
                {
                    _borderStyle = value;
                    RecreateHandle();
                    AdjustSize();
                }
            }
        }
        /// <summary>
        /// Gets or sets a value indicating how the rendered image is displayed.
        /// </summary>
        /// <value>An enumeration of the type <see cref="PictureBoxSizeMode"/> specifying the size mode.</value>
        [DefaultValue(PictureBoxSizeMode.Normal)]
        [Category("Behavior")]
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public PictureBoxSizeMode SizeMode
        {
            get {
                return _sizeMode;
            }
            set {
                if (_sizeMode != value)
                {
                    if (value == PictureBoxSizeMode.AutoSize)
                    {
                        this.AutoSize = true;
                        SetStyle(ControlStyles.FixedHeight | ControlStyles.FixedWidth, true);
                    }
                    if (value != PictureBoxSizeMode.AutoSize)
                    {
                        this.AutoSize = false;
                        SetStyle(ControlStyles.FixedHeight | ControlStyles.FixedWidth, false);
                        _savedSize = this.Size;
                    }
                    _sizeMode = value;
                    this.AdjustSize();
                    this.Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the application title. This is used for message dialog titles.
        /// </summary>
        /// <value>A string specifying the application title.</value>
        [DefaultValue(DefaultTitle)]
        [Description("The title of the application, used in displaying error and alert messages.")]
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public string AppTitle
        {
            get {
                return _appTitle;
            }
            set {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _appTitle = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the source URL for the <c>SVG</c> contents.
        /// </summary>
        /// <value>A string specifying the path of the <c>SVG</c> contents.</value>
        [Category("Data")]
        [DefaultValue("")]
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        [Description("The path of the document currently being display in this SvgPictureBox")]
        [EditorAttribute(typeof(SvgFilesUITypeEditor), typeof(UITypeEditor))]
        public string Source
        {
            get {
                return _svgSource;
            }
            set {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    this.Load(value);
                }
                else
                {
                    _svgSource = string.Empty;
                    if (_svgRenderer != null)
                    {
                        _svgRenderer.ClearAll();
                    }
                    if (_svgWindow != null)
                    {
                        _svgWindow.Document = null;
                    }
                    this.Refresh();
                }
            }
        }

        /// <summary>
        /// Gets or sets the source URL for the <c>SVG</c> contents.
        /// </summary>
        /// <value>An <see cref="Uri"/> specifying the path of the <c>SVG</c> contents.</value>
        [Category("Data")]
        [DefaultValue(null)]
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        [Description("The Uri of the document currently being display in this SvgPictureBox")]
        [EditorAttribute(typeof(SvgUrlUITypeEditor), typeof(UITypeEditor))]
        public Uri UriSource
        {
            get {
                return _uriSource;
            }
            set {
                if (value != null)
                {
                    this.Load(value);
                }
                else
                {
                    _uriSource = null;
                    if (_svgRenderer != null)
                    {
                        _svgRenderer.ClearAll();
                    }
                    if (_svgWindow != null)
                    {
                        _svgWindow.Document = null;
                    }
                    this.Refresh();
                }
            }
        }

        /// <summary>
        /// Gets or sets the <c>SVG</c> contents.
        /// </summary>
        /// <value>A string containing the <c>SVG</c> contents.</value>
        [Category("Data")]
        [DefaultValue("")]
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        [Description("The SVG content of the document currently being display in this SvgPictureBox")]
        [EditorAttribute(typeof(SvgTextUITypeEditor), typeof(UITypeEditor))]
        public string XmlSource
        {
            get {
                return _xmlSource;
            }
            set {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    this.LoadXml(value);
                }
                else
                {
                    _xmlSource = string.Empty;
                    if (_svgRenderer != null)
                    {
                        _svgRenderer.ClearAll();
                    }
                    if (_svgWindow != null)
                    {
                        _svgWindow.Document = null;
                    }
                    this.Refresh();
                }
            }
        }

        /// <summary>
        /// Gets the current SVG window implementation or <see cref="ISvgWindow"/> used by this control
        /// </summary>
        /// <value>An SVG window interface, <see cref="ISvgWindow"/>.</value>
        [Category("Data")]
        [Description("The Window Interface connected to the SvgPictureBox")]
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public ISvgWindow Window
        {
            get {
                return _svgWindow;
            }
        }

        /// <summary>
        /// Gets A rectangle that represents the invalidated region of the control.
        /// </summary>
        /// <value>A <see cref="RectangleF"/> that represents the invalidated region.</value>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public RectangleF InvalidRect
        {
            get {
                if (_svgRenderer != null)
                {
                    return GdiConverter.ToRectangle(_svgRenderer.InvalidRect);
                }
                return RectangleF.Empty;
            }
        }

        /// <summary>
        /// Gets or sets the foreground color of the control.
        /// </summary>
        /// <value>The foreground <see cref="System.Drawing.Color"/> of the control. 
        /// The default is the value of the <see cref="DefaultForeColor"/> property.</value>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override Color ForeColor
        {
            get {
                return base.ForeColor;
            }
            set {
                base.ForeColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the font of the text displayed by the control.
        /// </summary>
        /// <value>The <see cref="System.Drawing.Font"/> to apply to the text displayed by the control. 
        /// The default is the value of the <see cref="DefaultFont"/> property.</value>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override Font Font
        {
            get {
                return base.Font;
            }
            set {
                base.Font = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether control's elements are aligned to support locales using right-to-left languages.
        /// </summary>
        /// <value>One of the <see cref="System.Windows.Forms.RightToLeft"/> values.</value>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override RightToLeft RightToLeft
        {
            get {
                return base.RightToLeft;
            }
            set {
                base.RightToLeft = value;
            }
        }

        /// <summary>
        /// Gets or sets the text of this control.
        /// </summary>
        /// <value>The text of the <see cref="SvgPictureBox"/>.</value>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), Bindable(false)]
        public override string Text
        {
            get {
                return base.Text;
            }
            set {
                base.Text = value;
            }
        }

        #endregion

        #region Protected Properties

        /// <summary>
        ///  Deriving classes can override this to configure a default size for their control.
        ///  This is more efficient than setting the size in the control's constructor.
        /// </summary>
        /// <value>The default Size of the control. The default size is 200 pixel square.</value>
        protected override Size DefaultSize
        {
            get {
                return new Size(200, 200);
            }
        }

        /// <summary>
        /// Gets the default Input Method Editor (IME) mode supported by the control.
        /// </summary>
        /// <value>This is one of the <see cref="ImeMode"/> values, and it is set to <see cref="ImeMode.Disable"/>.</value>
        protected override ImeMode DefaultImeMode
        {
            get {
                return ImeMode.Disable;
            }
        }

        /// <summary>
        ///  Returns the parameters needed to create the handle.
        /// </summary>
        protected override CreateParams CreateParams
        {
            get {
                CreateParams cp = base.CreateParams;

                switch (_borderStyle)
                {
                    case BorderStyle.Fixed3D:
                        cp.ExStyle |= (int)WindowStylesEx.WS_EX_CLIENTEDGE;
                        break;
                    case BorderStyle.FixedSingle:
                        cp.Style |= (int)WindowStyles.WS_BORDER;
                        break;
                }

                return cp;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Loads and renders the SVG contents specified by the file path.
        /// </summary>
        /// <param name="svgSource">A <see cref="string"/> specifying the path of the SVG contents.</param>
        public void Load(string svgSource)
        {
            if (string.IsNullOrWhiteSpace(svgSource))
            {
                return;
            }
            if (_isInitializing || this.IsHandleCreated == false)
            {
                return;
            }

            this.Clear();

            Uri uriSource = this.ResolveUri(svgSource);
            if (uriSource != null && uriSource.IsAbsoluteUri)
            {
                if (uriSource.IsFile)
                {
                    _svgSource = uriSource.LocalPath;
                }
                else
                {
                    _svgSource = uriSource.AbsoluteUri;
                }
            }
            if (string.IsNullOrWhiteSpace(_svgSource))
            {
                return;
            }

            this.Load();
        }

        /// <summary>
        /// Loads and renders asynchronously the SVG contents specified by the file path.
        /// </summary>
        /// <param name="svgSource">A <see cref="string"/> specifying the path of the SVG contents.</param>
        public Task<bool> LoadAsync(string svgSource)
        {
            var tcs = new TaskCompletionSource<bool>();

            if (string.IsNullOrWhiteSpace(svgSource))
            {
                tcs.SetResult(false);
                return tcs.Task;
            }

            if (_isInitializing || this.IsHandleCreated == false)
            {
                tcs.SetResult(false);
                return tcs.Task;
            }

            Task.Factory.StartNew(() => {
                try
                {
                    this.Clear();

                    Uri uriSource = this.ResolveUri(svgSource);
                    if (uriSource != null && uriSource.IsAbsoluteUri)
                    {
                        if (uriSource.IsFile)
                        {
                            _svgSource = uriSource.LocalPath;
                        }
                        else
                        {
                            _svgSource = uriSource.AbsoluteUri;
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(_svgSource))
                    {
                        this.Load();

                        tcs.SetResult(_isSvgLoaded);
                    }
                    else
                    {
                        tcs.SetResult(false);
                    }
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                    tcs.SetResult(false);
                }
            });

            return tcs.Task;
        }

        /// <summary>
        /// Loads and renders the SVG contents.
        /// </summary>
        /// <param name="svgSource">A <see cref="string"/> specifying the SVG contents.</param>
        public void LoadXml(string xmlSource)
        {
            if (_isInitializing || this.IsHandleCreated == false)
            {
                return;
            }

            this.Clear();

            if (!string.IsNullOrWhiteSpace(xmlSource))
            {
                xmlSource = xmlSource.Trim();
                if (xmlSource.Length > ValidSVG.Length)
                {
                    _xmlSource = xmlSource;
                }
            }
            if (string.IsNullOrWhiteSpace(_xmlSource))
            {
                return;
            }

            this.Load();
        }

        /// <summary>
        /// Loads and renders the SVG contents specified by the URI.
        /// </summary>
        /// <param name="svgSource">A <see cref="Uri"/> specifying the path of the SVG contents.</param>
        public void Load(Uri svgSource)
        {
            if (_isInitializing || this.IsHandleCreated == false || svgSource == null)
            {
                return;
            }

            this.Clear();

            var uriSource = this.ResolveUri(svgSource);
            if (uriSource != null && uriSource.IsAbsoluteUri)
            {
                _uriSource = uriSource;
            }
            if (_uriSource == null)
            {
                return;
            }

            this.Load();
        }

        /// <summary>
        /// Loads and renders the SVG contents in the specified stream.
        /// </summary>
        /// <param name="streamSource">A stream containing the SVG contents.</param>
        public void Load(Stream streamSource)
        {
            if (_isInitializing || this.IsHandleCreated == false)
            {
                return;
            }

            this.Clear();

            if (streamSource != null)
            {
                // On dispose, the stream is closed so copy it to the memory stream.
                _streamSource = new MemoryStream();
                streamSource.CopyTo(_streamSource);
                // Move the position to the start of the stream
                _streamSource.Seek(0, SeekOrigin.Begin);
            }
            if (_streamSource == null)
            {
                return;
            }

            this.Load();
        }

        /// <summary>
        /// Clears both the SVG document content and the rendered drawings.
        /// </summary>
        public void Unload()
        {
            this.Clear();
        }

        /// <summary>
        ///  Returns a string representation for this control.
        /// </summary>
        public override string ToString()
        {
            string source = _svgSource;
            if (!string.IsNullOrWhiteSpace(_svgSource))
            {
                source = string.Empty;
            }
            StringBuilder builder = new StringBuilder(base.ToString());
            builder.Append(", Source").Append(source);
            builder.Append(", SizeMode: ").Append(_sizeMode.ToString("G"));

            return builder.ToString();
        }

        public override void Refresh()
        {
            base.Refresh();

            if (_svgWindow != null)
            {
                InvalidateAndRender();
            }
            else
            {
                this.Invalidate();
            }
        }

        #endregion

        #region Protected Methods

        protected virtual void Clear()
        {
            try
            {
                _isSvgLoaded = false;

                if (_streamSource != null)
                {
                    _streamSource.Dispose();
                }
                _svgSource    = string.Empty;
                _xmlSource    = string.Empty;
                _uriSource    = null;
                _streamSource = null;

                if (_svgRenderer == null)
                {
                    return;
                }
                _svgRenderer.ClearAll();
                if (_svgWindow != null)
                {
                    _svgWindow.Document = null;
                }

                GC.Collect();
                System.Threading.Thread.Sleep(1);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
            }
        }

        protected virtual void Load()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(_svgSource))
                {
                    // Load the source
                    _svgWindow.Source = _svgSource;
                    // Initialize the style sheets
                    SetupStyleSheets();
                    // Execute all script elements
                    //UnloadEngines();
                    //InitializeEvents();
                    //ExecuteScripts();

                    SvgSvgElement svgEl = (SvgSvgElement)_svgWindow.Document.RootElement;
                    SvgSizeF r = svgEl.GetSize();

                    int winWidth  = (int)svgEl.Width.BaseVal.Value;
                    int winHeight = (int)svgEl.Height.BaseVal.Value;
                    if (!r.Width.Equals(0.0) && !r.Height.Equals(0.0) && (r.Width * 4 * r.Height) < BitmapLimit)
                    {
                        winWidth  = (int)r.Width;
                        winHeight = (int)r.Height;
                    }
                    if ((winWidth * 4 * winHeight) >= BitmapLimit)
                    {
                        winWidth  = this.Width;
                        winHeight = this.Height;
                    }

                    _svgWindow.Resize(winWidth, winHeight);

                    _svgRenderer.InvalidRect = SvgRectF.Empty;

                    this.Render();
                    _isSvgLoaded = true;
                }
                else if (!string.IsNullOrWhiteSpace(_xmlSource) && _xmlSource.Length > ValidSVG.Length)
                {
                    SvgDocument doc = _svgWindow.CreateEmptySvgDocument();
                    doc.LoadXml(_xmlSource);
                    _svgWindow.Document = doc;

                    SetupStyleSheets();

                    SvgSvgElement svgEl = (SvgSvgElement)_svgWindow.Document.RootElement;
                    SvgSizeF r = svgEl.GetSize();

                    int winWidth  = (int)svgEl.Width.BaseVal.Value;
                    int winHeight = (int)svgEl.Height.BaseVal.Value;
                    if (!r.Width.Equals(0.0) && !r.Height.Equals(0.0) && (r.Width * 4 * r.Height) < BitmapLimit)
                    {
                        winWidth  = (int)r.Width;
                        winHeight = (int)r.Height;
                    }
                    if ((winWidth * 4 * winHeight) >= BitmapLimit)
                    {
                        winWidth  = this.Width;
                        winHeight = this.Height;
                    }
                    _svgWindow.Resize(winWidth, winHeight);

                    _svgRenderer.InvalidRect = SvgRectF.Empty;

                    this.Render();
                    _isSvgLoaded = true;
                }
                else if (_uriSource != null)
                {
                    // Load the source
                    _svgWindow.Source = _uriSource.AbsoluteUri;
                    // Initialize the style sheets
                    SetupStyleSheets();
                    // Execute all script elements
                    //UnloadEngines();
                    //InitializeEvents();
                    //ExecuteScripts();

                    SvgSvgElement svgEl = (SvgSvgElement)_svgWindow.Document.RootElement;
                    SvgSizeF r = svgEl.GetSize();

                    int winWidth  = (int)svgEl.Width.BaseVal.Value;
                    int winHeight = (int)svgEl.Height.BaseVal.Value;
                    if (!r.Width.Equals(0.0) && !r.Height.Equals(0.0) && (r.Width * 4 * r.Height) < BitmapLimit)
                    {
                        winWidth  = (int)r.Width;
                        winHeight = (int)r.Height;
                    }
                    if ((winWidth * 4 * winHeight) >= BitmapLimit)
                    {
                        winWidth  = this.Width;
                        winHeight = this.Height;
                    }

                    _svgWindow.Resize(winWidth, winHeight);

                    _svgRenderer.InvalidRect = SvgRectF.Empty;

                    this.Render();
                    _isSvgLoaded = true;
                }
                else if (_streamSource != null)
                {
                    SvgDocument doc = _svgWindow.CreateEmptySvgDocument();
                    doc.Load(_streamSource);
                    _svgWindow.Document = doc;

                    SetupStyleSheets();

                    SvgSvgElement svgEl = (SvgSvgElement)_svgWindow.Document.RootElement;
                    SvgSizeF r = svgEl.GetSize();

                    int winWidth  = (int)svgEl.Width.BaseVal.Value;
                    int winHeight = (int)svgEl.Height.BaseVal.Value;
                    if (!r.Width.Equals(0.0) && !r.Height.Equals(0.0) && (r.Width * 4 * r.Height) < BitmapLimit)
                    {
                        winWidth  = (int)r.Width;
                        winHeight = (int)r.Height;
                    }
                    if ((winWidth * 4 * winHeight) >= BitmapLimit)
                    {
                        winWidth  = this.Width;
                        winHeight = this.Height;
                    }

                    _svgWindow.Resize(winWidth, winHeight);

                    _svgRenderer.InvalidRect = SvgRectF.Empty;

                    this.Render();
                    _isSvgLoaded = true;
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());

                if (this.DesignMode)
                {
                    return;
                }
                var errorArgs = new SvgErrorArgs("An error occurred while loading the document", ex);
                _svgErrors?.Invoke(this, errorArgs);
                if (!errorArgs.Handled)
                {
                    MessageBox.Show(errorArgs.Message + ": " + ex.Message,
                        _appTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        protected virtual void Render()
        {
            if (_svgWindow != null)
            {
                InvalidateAndRender();
            }
        }

        protected virtual void Update(RectangleF rect)
        {
            if (_svgWindow != null)
            {
                InvalidateAndUpdate(rect);
            }
        }

        protected virtual void UpdateGraphics(Graphics graphics)
        {
        }

        protected virtual void Draw(Graphics gr, Rectangle rect)
        {
            //if (this.DesignMode || _svgWindow == null)
            //{
            //    return;
            //}
            try
            {
                if (_svgWindow == null)
                {
                    gr.Clear(this.BackColor);
                    return;
                }
                Bitmap rasterImage = ((GdiGraphicsRenderer)_svgWindow.Renderer).RasterImage;

                if (rasterImage != null)
                {
                    gr.DrawImage(rasterImage, rect);
                }
                else
                {
                    gr.Clear(this.BackColor);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());

                if (this.DesignMode)
                {
                    return;
                }

                var errorArgs = new SvgErrorArgs("An exception occurred while rendering", ex);
                _svgErrors?.Invoke(this, errorArgs);
                if (!errorArgs.Handled)
                {
                    MessageBox.Show(errorArgs.Message + ": " + ex.Message,
                        _appTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        protected virtual void CacheRenderingRegions()
        {
            // Collect the rendering regions for later updates

            //System.Threading.Thread.Sleep(1);
            //SvgDocument doc = (window.Document as SvgDocument);
            //SvgElement root = (doc.RootElement as SvgElement);
            //root.CacheRenderingRegion(renderer);
        }

        /// <summary>
        /// Create an empty SvgDocument and GdiRenderer for this control.  
        /// The empty SvgDocument is returned.  This method is needed only in situations 
        /// where the library user needs to create an SVG DOM tree outside of the usual window Src setting mechanism.
        /// </summary>
        protected virtual SvgDocument CreateEmptySvgDocument()
        {
            if (_svgWindow == null)
            {
                return null;
            }

            SvgDocument svgDocument = _svgWindow.CreateEmptySvgDocument();
            SetupStyleSheets();

            return svgDocument;
        }

        /// <summary>
        /// Loads the default user and agent stylesheets into the current SvgDocument
        /// </summary>
        protected virtual void SetupStyleSheets()
        {
            CssXmlDocument cssDocument = (CssXmlDocument)_svgWindow.Document;
            string appRootPath         = SvgApplicationContext.ExecutableDirectory.FullName;
            FileInfo userAgentCssPath  = new FileInfo(Path.Combine(appRootPath, UserAgentCssFileName));
            FileInfo userCssPath       = new FileInfo(Path.Combine(appRootPath, UserCssFileName));

            if (userAgentCssPath.Exists)
            {
                cssDocument.SetUserAgentStyleSheet((new Uri(userAgentCssPath.FullName)).AbsoluteUri);
            }

            if (userCssPath.Exists)
            {
                cssDocument.SetUserStyleSheet((new Uri(userCssPath.FullName)).AbsoluteUri);
            }
        }

        protected virtual Uri ResolveUri(string svgSource)
        {
            if (string.IsNullOrWhiteSpace(svgSource))
            {
                return null;
            }
            if (File.Exists(svgSource))
            {
                return new Uri(Path.GetFullPath(svgSource));
            }
            try
            {
                Uri uri = new Uri(svgSource);
                if (uri.IsAbsoluteUri)
                {
                    return uri;
                }

                return this.ResolveUri(uri);
            }
            catch (UriFormatException)
            {
                // If relative, get the full path
                string localFile = Path.GetFullPath(svgSource.Replace('/', '\\'));
                if (File.Exists(localFile))
                {
                    return new Uri(localFile);
                }

                var svgPath = svgSource;
                if (svgSource[0] == '\\' || svgSource[0] == '/')
                {
                    svgPath = svgSource.Substring(1);
                }
                svgPath = svgPath.Replace('/', '\\');

                var assembly = System.Reflection.Assembly.GetExecutingAssembly();
                localFile = Path.Combine(Path.GetDirectoryName(assembly.Location), svgPath);
                if (File.Exists(localFile))
                {
                    return new Uri(localFile);
                }
            }
            return null;
        }

        protected virtual Uri ResolveUri(Uri svgSource)
        {
            if (svgSource == null)
            {
                return null;
            }

            if (svgSource.IsAbsoluteUri)
            {
                return svgSource;
            }

            // Try getting a local file in the same directory....
            string svgPath = svgSource.ToString();
            if (svgPath[0] == '\\' || svgPath[0] == '/')
            {
                svgPath = svgPath.Substring(1);
            }
            svgPath = svgPath.Replace('/', '\\');

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            string localFile = Path.Combine(Path.GetDirectoryName(assembly.Location), svgPath);
            if (File.Exists(localFile))
            {
                return new Uri(localFile);
            }

            // Try using the system full path method...
            svgPath = svgSource.ToString();
            svgPath = svgPath.Replace('/', '\\');

            localFile = Path.GetFullPath(svgPath);
            if (File.Exists(localFile))
            {
                return new Uri(localFile);
            }

            return null;
        }

        protected virtual void OnHandleAlert(string message)
        {
            if (this.DesignMode)
            {
                return;
            }
            var alertArgs = new SvgAlertArgs(message);
            _svgAlerts?.Invoke(this, alertArgs);
            if (!alertArgs.Handled)
            {
                MessageBox.Show(alertArgs.Message, _appTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        protected virtual void OnHandleError(string message, Exception exception)
        {
            if (this.DesignMode)
            {
                return;
            }
            var errorArgs = new SvgErrorArgs(message, exception);
            _svgErrors?.Invoke(this, errorArgs);
            if (!errorArgs.Handled)
            {
                throw new SvgErrorException(errorArgs);
            }
        }

        #endregion

        #region Protected Override Methods

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;

            UpdateGraphics(graphics);

            var imageRect = this.ComputeImageRectangle();

            this.Draw(graphics, imageRect);

            // Will fire the Paint event
            base.OnPaint(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (_isSvgLoaded)
            {
                // Worry about clearing the graphics nodes map...
                //System.GC.Collect();
                //System.Threading.Thread.Sleep(1);

//TODO:                (_svgWindow as SvgWindow).Resize(this.Width, this.Height);
                InvalidateAndRender();
            }

            _savedSize = this.Size;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            _savedSize = this.Size;

            _svgRenderer = new GdiGraphicsRenderer();
            _svgWindow   = new SvgPictureBoxWindow(this, _svgRenderer);

            _svgRenderer.OnRender = new RenderEvent(this.OnRender);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (_isSvgLoaded)
            {
                if (_lastPosX == e.X && _lastPosY == e.Y)
                    return;

                _lastPosX = e.X;
                _lastPosY = e.Y;

                if (_svgRenderer != null)
                {
                    _svgRenderer.OnMouseEvent("mousemove", e);
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (_isSvgLoaded)
            {
                if (_svgRenderer != null)
                {
                    _svgRenderer.OnMouseEvent("mousedown", e);
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (_isSvgLoaded)
            {
                if (_svgRenderer != null)
                {
                    _svgRenderer.OnMouseEvent("mouseup", e);
                }
            }
        }

        #endregion

        #region Private Methods

        private void OnRender(SvgRectF updatedRect)
        {
            if (this.InvokeRequired)
            {
                MethodInvoker del = delegate
                {
                    OnRender(updatedRect);
                };
                this.Invoke(del);
                return;
            }
            
            this.Invalidate(new Rectangle((int)updatedRect.X, (int)updatedRect.Y,
                (int)updatedRect.Width, (int)updatedRect.Height));

            // Collect the rendering regions for later updates
            //SvgDocument doc = (window.Document as SvgDocument);
            //SvgElement root = (doc.RootElement as SvgElement);
            //root.CacheRenderingRegion(renderer);
        }

        private void InvalidateAndRender()
        {
            if (_svgWindow == null || _svgWindow.Document == null)
            {
                return;
            }
            try
            {
                if (_svgRenderer != null)
                {
                    SvgSvgElement svgEl = (SvgSvgElement)_svgWindow.Document.RootElement;

                    SvgSizeF r = svgEl.GetSize();

                    int winWidth  = (int)svgEl.Width.BaseVal.Value;
                    int winHeight = (int)svgEl.Height.BaseVal.Value;

                    if (!r.Width.Equals(0.0) && !r.Height.Equals(0.0) && (r.Width* 4 * r.Height) < BitmapLimit)
                    {
                        winWidth  = (int)r.Width;
                        winHeight = (int)r.Height;
                    }
                    if ((winWidth * 4 * winHeight) >= BitmapLimit)
                    {
                        winWidth  = this.Width;
                        winHeight = this.Height;
                    }

                    _svgWindow.Resize(winWidth, winHeight);

                    _svgRenderer.Render(_svgWindow.Document as SvgDocument);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());

                if (this.DesignMode)
                {
                    return;
                }

                var errorArgs = new SvgErrorArgs("An exception occurred while rendering", ex);
                _svgErrors?.Invoke(this, errorArgs);
                if (!errorArgs.Handled)
                {
                    MessageBox.Show(errorArgs.Message + ": " + ex.Message, 
                        _appTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void InvalidateAndUpdate(RectangleF rect)
        {
            if (_svgRenderer != null)
            {
                _svgRenderer.InvalidateRect(SvgConvert.ToRect(rect));
                InvalidateAndRender();
            }
        }

        /// <summary>
        /// If the <see cref="SvgPictureBox"/> has the <see cref="SizeMode"/> property set to 
        /// <see cref="PictureBoxSizeMode.AutoSize"/>, adjust the size to hold the rendered image.
        /// </summary>
        private void AdjustSize()
        {
            if (_sizeMode == PictureBoxSizeMode.AutoSize)
            {
                this.Size = this.PreferredSize;
            }
            else
            {
                this.Size = _savedSize;
            }
        }

        private Rectangle ImageRectangle
        {
            get {
                return ComputeImageRectangle();
            }
        }

        private Image RenderedImage
        {
            get {
                if (_svgRenderer != null)
                {
                    return _svgRenderer.RasterImage;
                }
                if (_svgWindow != null)
                {
                    return ((GdiGraphicsRenderer)_svgWindow.Renderer).RasterImage;
                }
                return null;
            }
        }

        private static Rectangle DeflateRect(Rectangle rect, Padding padding)
        {
            rect.X += padding.Left;
            rect.Y += padding.Top;
            rect.Width -= padding.Horizontal;
            rect.Height -= padding.Vertical;
            return rect;
        }

        private Rectangle ComputeImageRectangle()
        {
            Rectangle imageRect = DeflateRect(this.ClientRectangle, this.Padding);

            var image = this.RenderedImage;

            if (image != null)
            {
                switch (_sizeMode)
                {
                    // The image is placed in the upper-left corner of the SvgPictureBox. 
                    // The image is clipped if it is larger than the SvgPictureBox it is contained in.
                    case PictureBoxSizeMode.Normal:
                    // The SvgPictureBox is sized equal to the size of the image that it contains.
                    case PictureBoxSizeMode.AutoSize:
                        imageRect.Size = image.Size;
                        break;

                    // The image within the SvgPictureBox is stretched or shrunk to fit the size of the SvgPictureBox.
                    case PictureBoxSizeMode.StretchImage:
                        break;

                    // The image is displayed in the center if the SvgPictureBox is larger than the image. 
                    // If the image is larger than the SvgPictureBox, the picture is placed in the center of 
                    // the SvgPictureBox and the outside edges are clipped.
                    case PictureBoxSizeMode.CenterImage:
                        imageRect.X += (imageRect.Width - image.Width) / 2;
                        imageRect.Y += (imageRect.Height - image.Height) / 2;
                        imageRect.Size = image.Size;
                        break;

                    // The size of the image is increased or decreased maintaining the size ratio.
                    case PictureBoxSizeMode.Zoom:
                        Size imageSize = image.Size;
                        Rectangle clientRect = this.ClientRectangle;
                        float ratio = Math.Min((float)clientRect.Width / (float)imageSize.Width, 
                            (float)clientRect.Height / (float)imageSize.Height);
                        imageRect.Width  = (int)(imageSize.Width * ratio);
                        imageRect.Height = (int)(imageSize.Height * ratio);
                        imageRect.X = (clientRect.Width - imageRect.Width) / 2;
                        imageRect.Y = (clientRect.Height - imageRect.Height) / 2;
                        break;
                    // Not expected!
                    default:
                        break;
                }
            }

            return imageRect;
        }
        #endregion

        #region Scripting Methods and Properties

        private void DocumentZoomer(float Zint)
        {

            //String LocalScale = "";
            //if (myXml.LastChild.Attributes["transform"] != null)
            //{

            //    LocalScale = myXml.LastChild.Attributes["transform"].Value;

            //}

            //else
            //{

            //    XmlAttribute scaxml = myXml.CreateAttribute("transform");

            //    scaxml.Value = "scale(1.0)";

            //    myXml.LastChild.Attributes.Append(scaxml);

            //    LocalScale = "scale(1.0)";

            //}

            //string parsIt = LocalScale.Replace("scale(", "").Replace(")", "");

            //string[] parts = parsIt.Split(',');

            //float[] ops = new float[parts.Length];

            //for (int i = 0; i < parts.Length; i++)
            //{


            //    ops[i] = float.Parse(parts[i]) + Zint;


            //    if (ops[i] > 0)

            //        LocalScale = ops[i].ToString() + ",";


            //}

            //LocalScale = LocalScale.Substring(0, LocalScale.Length - 1);

            //LocalScale = "scale(" + LocalScale + ")";

            //myXml.LastChild.Attributes["transform"].Value = LocalScale;

            //SVGPicture.LoadXml(myXml.OuterXml);

        }

        //        private TypeDictionary scriptEngineByMimeType;
        //        private Dictionary<string, ScriptEngine> scriptEngines = new Dictionary<string, ScriptEngine>();

        //        public void SetMimeTypeEngineType(string mimeType, Type engineType)
        //        {
        //            scriptEngineByMimeType[mimeType] = engineType;
        //        }

        //        public ScriptEngine GetScriptEngineByMimeType(string mimeType)
        //        {
        //            ScriptEngine engine = null;

        //            if (mimeType == "")
        //                mimeType = ((ISvgWindow)window).Document.RootElement.GetAttribute("contentScriptType");

        //            if (mimeType == "" || mimeType == "text/ecmascript" || mimeType == "text/javascript" || mimeType == "application/javascript")
        //                mimeType = "application/ecmascript";

        //            if (!scriptEngines.ContainsKey(mimeType))
        //            {
        //                object[] args = new object[] { (window as ISvgWindow) };
        //                engine = (ScriptEngine)scriptEngineByMimeType.CreateInstance(mimeType, args);
        //                scriptEngines.Add(mimeType, engine);
        //                engine.Initialise();
        //            }

        //            if (engine == null)
        //                engine = scriptEngines[mimeType];

        //            return engine;
        //        }


        //        /// <summary>
        //        /// Clears the existing script engine list from any previously running instances
        //        /// </summary>
        //        private void UnloadEngines()
        //        {
        //            // Dispose of all running engines from previous document instances
        //            foreach (string mimeType in scriptEngines.Keys)
        //            {
        //                ScriptEngine engine = scriptEngines[mimeType];
        //                engine.Close();
        //                engine = null;
        //            }
        //            // Clear the list
        //            scriptEngines.Clear();
        //            ClosureEventMonitor.Clear();
        //            ScriptTimerMonitor.Reset();
        //        }

        //        /// <summary>
        //        /// Add event listeners for on* events within the document
        //        /// </summary>
        //        private void InitializeEvents()
        //        {
        //            SvgDocument document = (SvgDocument)window.Document;
        //            document.NamespaceManager.AddNamespace("svg", "http://www.w3.org/2000/svg");

        //            XmlNodeList nodes = document.SelectNodes(@"//*[namespace-uri()='http://www.w3.org/2000/svg']
        //                                                   [local-name()='svg' or
        //                                                    local-name()='g' or
        //                                                    local-name()='defs' or
        //                                                    local-name()='symbol' or
        //                                                    local-name()='use' or
        //                                                    local-name()='switch' or
        //                                                    local-name()='image' or
        //                                                    local-name()='path' or
        //                                                    local-name()='rect' or
        //                                                    local-name()='circle' or
        //                                                    local-name()='ellipse' or
        //                                                    local-name()='line' or
        //                                                    local-name()='polyline' or
        //                                                    local-name()='polygon' or
        //                                                    local-name()='text' or
        //                                                    local-name()='tref' or
        //                                                    local-name()='tspan' or
        //                                                    local-name()='textPath' or
        //                                                    local-name()='altGlyph' or
        //                                                    local-name()='a' or
        //                                                    local-name()='foreignObject']
        //                                                /@*[name()='onfocusin' or
        //                                                    name()='onfocusout' or
        //                                                    name()='onactivate' or
        //                                                    name()='onclick' or
        //                                                    name()='onmousedown' or
        //                                                    name()='onmouseup' or
        //                                                    name()='onmouseover' or
        //                                                    name()='onmousemove' or
        //                                                    name()='onmouseout' or
        //                                                    name()='onload']", document.NamespaceManager);

        //            foreach (XmlNode node in nodes)
        //            {
        //                IAttribute att = (IAttribute)node;
        //                IEventTarget targ = (IEventTarget)att.OwnerElement;
        //                ScriptEventMonitor mon = new ScriptEventMonitor((VsaScriptEngine)GetScriptEngineByMimeType(""), att, window);
        //                string eventName = null;
        //                switch (att.Name)
        //                {
        //                    case "onfocusin":
        //                        eventName = "focusin";
        //                        break;
        //                    case "onfocusout":
        //                        eventName = "focusout";
        //                        break;
        //                    case "onactivate":
        //                        eventName = "activate";
        //                        break;
        //                    case "onclick":
        //                        eventName = "click";
        //                        break;
        //                    case "onmousedown":
        //                        eventName = "mousedown";
        //                        break;
        //                    case "onmouseup":
        //                        eventName = "mouseup";
        //                        break;
        //                    case "onmouseover":
        //                        eventName = "mouseover";
        //                        break;
        //                    case "onmousemove":
        //                        eventName = "mousemove";
        //                        break;
        //                    case "onmouseout":
        //                        eventName = "mouseout";
        //                        break;
        //                    case "onload":
        //                        eventName = "SVGLoad";
        //                        break;
        //                }
        //                targ.AddEventListener(eventName, new EventListener(mon.EventHandler), false);
        //            }
        //        }

        //        /// <summary>
        //        /// Collect the text in all script elements, build engine and execute. 
        //        /// </summary>
        //        private void ExecuteScripts()
        //        {
        //            Dictionary<string, StringBuilder> codeByMimeType = new Dictionary<string, StringBuilder>();
        //            StringBuilder codeBuilder;
        //            SvgDocument document = (SvgDocument)window.Document;

        //            XmlNodeList scripts = document.GetElementsByTagName("script", SvgDocument.SvgNamespace);
        //            StringBuilder code = new StringBuilder();

        //            foreach (XmlElement script in scripts)
        //            {
        //                string type = script.GetAttribute("type");

        //                if (GetScriptEngineByMimeType(type) != null)
        //                {
        //                    // make sure we have a StringBuilder for this MIME type
        //                    if (!codeByMimeType.ContainsKey(type))
        //                        codeByMimeType[type] = new StringBuilder();

        //                    // grab this MIME type's codeBuilder
        //                    codeBuilder = codeByMimeType[type];

        //                    if (script.HasChildNodes)
        //                    {
        //                        // process each child that is text node or a CDATA section
        //                        foreach (XmlNode node in script.ChildNodes)
        //                        {
        //                            if (node.NodeType == XmlNodeType.CDATA || node.NodeType == XmlNodeType.Text)
        //                            {
        //                                codeBuilder.Append(node.Value);
        //                            }
        //                        }
        //                    }

        //                    if (script.HasAttribute("href", "http://www.w3.org/1999/xlink"))
        //                    {
        //                        string href = script.GetAttribute("href", "http://www.w3.org/1999/xlink");
        //                        Uri baseUri = new Uri(((XmlDocument)((ISvgWindow)window).Document).BaseURI);
        //                        Uri hUri = new Uri(baseUri, href);
        //                        ExtendedHttpWebRequestCreator creator = new ExtendedHttpWebRequestCreator();
        //                        ExtendedHttpWebRequest request = (ExtendedHttpWebRequest)creator.Create(hUri);
        //                        ExtendedHttpWebResponse response = (ExtendedHttpWebResponse)request.GetResponse();
        //                        Stream rs = response.GetResponseStream();
        //                        StreamReader sr = new StreamReader(rs);
        //                        codeBuilder.Append(sr.ReadToEnd());
        //                        rs.Close();
        //                    }
        //                }
        //            }

        //            // execute code for all script engines
        //            //foreach (string mimeType in codeByMimeType.Keys)
        //            foreach (KeyValuePair<string, StringBuilder> pair in codeByMimeType)
        //            {
        //                string mimeType = pair.Key;
        //                //codeBuilder = codeByMimeType[mimeType];
        //                codeBuilder = pair.Value;

        //                if (codeBuilder.Length > 0)
        //                {
        //                    ScriptEngine engine = GetScriptEngineByMimeType(mimeType);
        //                    engine.Execute(codeBuilder.ToString());
        //                }
        //            }

        //            // load the root element
        //            ((ISvgWindow)window).Document.RootElement.DispatchEvent(new Event("SVGLoad", false, true));
        //        }

        #endregion

        #region ISupportInitialize Members

        public void BeginInit()
        {
            _isInitializing = true;
        }

        public void EndInit()
        {
            _isInitializing = false;

            if (!_isSvgLoaded)
            {
                this.Load();
            }

            this.Invalidate();
        }

        #endregion

        #region ISvgControl Members

        bool ISvgControl.DesignMode
        {
            get {
                return this.DesignMode;
            }
        }

        void ISvgControl.HandleAlert(string message)
        {
            if (string.IsNullOrWhiteSpace(message) || this.DesignMode)
            {
                return;
            }
            this.OnHandleAlert(message);
        }

        void ISvgControl.HandleError(string message)
        {
            if (string.IsNullOrWhiteSpace(message) || this.DesignMode)
            {
                return;
            }
            this.OnHandleError(message, null);
        }

        void ISvgControl.HandleError(Exception exception)
        {
            if (exception == null || this.DesignMode)
            {
                return;
            }
            this.OnHandleError(null, exception);
        }

        void ISvgControl.HandleError(string message, Exception exception)
        {
            if (string.IsNullOrWhiteSpace(message) || this.DesignMode)
            {
                return;
            }
            this.OnHandleError(message, exception);
        }

        #endregion
    }
}

