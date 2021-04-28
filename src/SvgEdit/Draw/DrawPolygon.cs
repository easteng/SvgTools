#region Header

/*  --------------------------------------------------------------------------------------------------------------
 *  I Software Innovations
 *  --------------------------------------------------------------------------------------------------------------
 *  SVG Artieste 2.0
 *  --------------------------------------------------------------------------------------------------------------
 *  File     :       DrawPolygon.cs
 *  Author   :       ajaysbritto@yahoo.com
 *  Date     :       20/03/2010
 *  --------------------------------------------------------------------------------------------------------------
 *  Change Log
 *  --------------------------------------------------------------------------------------------------------------
 *  Author	Comments
 */

#endregion Header

namespace Draw
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Globalization;
    using System.Windows.Forms;

    using SVGLib;

    /// <summary>
    /// Polygon graphic object
    /// </summary>
    public sealed class DrawPolygon : DrawLine
    {
        #region Fields

        private const string Tag = "polyline";
        private readonly ArrayList _pointArray; // list of points
        private Cursor _handleCursor;

        #endregion Fields

        #region Constructors

        public DrawPolygon()
        {
            _pointArray = new ArrayList();

            LoadCursor();
            Initialize();
        }

        public DrawPolygon(float x1, float y1, float x2, float y2)
        {
            _pointArray = new ArrayList {new PointF(x1, y1), new PointF(x2, y2)};

            LoadCursor();
            Initialize();
        }

        public DrawPolygon(PointF[] points)
        {
            _pointArray = new ArrayList();
            for (int i = 0;i<points.Length;i++)
            {
                _pointArray.Add(points[i]);
            }
            LoadCursor();
            Initialize();
        }

        #endregion Constructors

        #region Properties

        public override int HandleCount
        {
            get
            {
                return _pointArray.Count;
            }
        }

        #endregion Properties

        #region Methods

        public static DrawPolygon Create(SvgPolyline svg)
        {
            try
            {
                string s = svg.Points.Trim();
                string[] arr = s.Split(' ');
                var points = new PointF[arr.Length];
                for (int i = 0; i < arr.Length; i++)
                {
                    var arrp = arr[i].Split(',');
                    points[i]=new PointF(ParseSize(arrp[0],Dpi.X),
                        ParseSize(arrp[1],Dpi.Y));
                }
                var dobj = new DrawPolygon(points);
                dobj.SetStyleFromSvg(svg);
                return dobj;
            }
            catch (Exception ex)
            {
                ErrH.Log("DrawPolygon", "Create", ex.ToString(), ErrH._LogPriority.Info);
                return null;
            }
        }

        public void AddPoint(PointF point)
        {
            _pointArray.Add(point);
        }

        public override void Draw(Graphics g)
        {
            float x1 = 0, y1 = 0;     // previous point
            try
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;

                if (Fill != Color.Empty)
                {
                    var arr = new PointF[_pointArray.Count];
                    for (int i = 0; i < _pointArray.Count; i++)
                        arr[i] = (PointF)_pointArray[i];
                    Brush brush = new SolidBrush(Fill);
                    g.FillPolygon(brush,arr);
                }

                var pen = new Pen(Stroke, StrokeWidth);

                IEnumerator enumerator = _pointArray.GetEnumerator();

                if ( enumerator.MoveNext() )
                {
                    x1 = ((PointF)enumerator.Current).X;
                    y1 = ((PointF)enumerator.Current).Y;
                }

                while ( enumerator.MoveNext() )
                {
                    float x2 = ((PointF)enumerator.Current).X;             // current point
                    float y2 = ((PointF)enumerator.Current).Y;             // current point

                    g.DrawLine(pen, x1, y1, x2, y2);

                    x1 = x2;
                    y1 = y2;
                }

                pen.Dispose();
            }
            catch (Exception ex)
            {
                ErrH.Log("DrawPolygon", "Draw", ex.ToString(), ErrH._LogPriority.Info);
            }
        }

        /// <summary>
        /// Get handle point by 1-based number
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <returns></returns>
        public override PointF GetHandle(int handleNumber)
        {
            if ( handleNumber < 1 )
                handleNumber = 1;

            if ( handleNumber > _pointArray.Count )
                handleNumber = _pointArray.Count;

            return ((PointF)_pointArray[handleNumber - 1]);
        }

        public override Cursor GetHandleCursor(int handleNumber)
        {
            return _handleCursor;
        }

        public override string GetXmlStr(SizeF scale)
        {
            //  <polyline style="fill:none; stroke:blue; stroke-width:10"
            //points="50,375 150,375
            string s = "<";
            s += Tag;
            s += GetStrStyle(scale);
            s += " points = "+"\"";
            IEnumerator enumerator = _pointArray.GetEnumerator();
            while ( enumerator.MoveNext() )
            {
                float x = ((PointF)enumerator.Current).X/scale.Width;
                float y = ((PointF)enumerator.Current).Y/scale.Height;
                s += x.ToString(CultureInfo.InvariantCulture)+","+y.ToString(CultureInfo.InvariantCulture);
                s += " ";
            }
            s += "\"";
            s += " />" + "\r\n";
            return s;
        }

        public override void Move(float deltaX, float deltaY)
        {
            int n = _pointArray.Count;

            for ( int i = 0; i < n; i++ )
            {
                var point = new PointF( ((PointF)_pointArray[i]).X + deltaX, ((PointF)_pointArray[i]).Y + deltaY);

                _pointArray[i] = point;
            }

            Invalidate();
        }

        public override void MoveHandleTo(PointF point, int handleNumber)
        {
            if ( handleNumber < 1 )
                handleNumber = 1;

            if ( handleNumber > _pointArray.Count)
                handleNumber = _pointArray.Count;

            _pointArray[handleNumber-1] = point;

            Invalidate();
        }

        public override void Resize(SizeF newscale,SizeF oldscale)
        {
            for (int i = 0; i < _pointArray.Count; i ++)
                _pointArray[i] = RecalcPoint((PointF )_pointArray[i], newscale,oldscale);
            RecalcStrokeWidth(newscale,oldscale);
            Invalidate();
        }

        /// <summary>
        /// Create graphic object used for hit test
        /// </summary>
        protected override void CreateObjects()
        {
            if ( AreaPath != null )
                return;
            try
            {
                // Create closed path which contains all polygon vertexes
                AreaPath = new GraphicsPath();

                float x1 = 0, y1 = 0;     // previous point

                IEnumerator enumerator = _pointArray.GetEnumerator();

                if ( enumerator.MoveNext() )
                {
                    x1 = ((PointF)enumerator.Current).X;
                    y1 = ((PointF)enumerator.Current).Y;
                }

                while ( enumerator.MoveNext() )
                {
                    float x2 = ((PointF)enumerator.Current).X;             // current point
                    float y2 = ((PointF)enumerator.Current).Y;             // current point

                    AreaPath.AddLine(x1, y1, x2, y2);

                    x1 = x2;
                    y1 = y2;
                }

                AreaPath.CloseFigure();

                // Create region from the path
                AreaRegion = new Region(AreaPath);
            }
            catch(Exception ex)
            {
                ErrH.Log("DrawPolygon", "Create", ex.ToString(), ErrH._LogPriority.Info);
            }
        }

        private void LoadCursor()
        {
            _handleCursor = Cursors.SizeAll;
        }

        #endregion Methods

        #region overrides
        public override object Clone()
        {
            // .net does'nt deep copy arraylist. So we will do it
            DrawPolygon copy = new DrawPolygon();
            for (int i = 0; i < _pointArray.Count; i++)
            {
                PointF pointToCopy = (PointF)_pointArray[i];
                copy._pointArray.Add(new PointF(pointToCopy.X, pointToCopy.Y));
            }
            return copy;
        }
        #endregion overrides
    }
}