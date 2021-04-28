﻿using System;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;

using WeifenLuo.WinFormsUI.Docking;

using SharpVectors.Net;

namespace GdiW3cSvgTestSuite
{
    public partial class MainForm : Form
    {
        #region Private Fields

        public const string AppTitle      = "Svg Test Suite";
        public const string AppErrorTitle = "Svg Test Suite - Error";

        private DockPanel _dockPanel;
        private DockingTheme _currentTheme;
        private VS2015LightTheme _vS2015LightTheme;
        private VS2015BlueTheme _vS2015BlueTheme;
        private VS2015DarkTheme _vS2015DarkTheme;

        private TestViewDockPanel _testViewPanel;
        private DebugDockPanel _debugPanel;

        private TestDockPanel _testPanel;
        private AboutTestDockPanel _aboutTestPanel;
        private SvgInputDockPanel _inputPanel;
        private SettingsDockPanel _settingsPanel;
        private ResultsDockPanel _resultsPanel;

        private bool _isTestAvailable;

        private string _testSettingsPath;

        private OptionSettings _optionSettings;

        private ITestPagePanel _selectedPagePanel;

        #endregion

        #region Constructors and Destructor

        public MainForm()
        {
            InitializeComponent();

            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.BackColor     = Color.White;

            var screenBounds  = Screen.PrimaryScreen.WorkingArea;

            int width  = screenBounds.Width;
            int height = screenBounds.Height;
            if (width > 1200)
            {
                this.Width = 1200;
            }
            else
            {
                this.Width = (int)(Math.Min(1200, width) * 0.95);
            }
            this.Height = (int)(Math.Min(1080, height) * 0.90);
            
            _optionSettings = new OptionSettings();
            _testSettingsPath = Path.GetFullPath(Path.Combine("..\\", OptionSettings.SettingsFileName));
            if (!string.IsNullOrWhiteSpace(_testSettingsPath) && File.Exists(_testSettingsPath))
            {
                _optionSettings.Load(_testSettingsPath, this);
            }

            _currentTheme = DockingTheme.LightTheme;

            InitializePanels();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Public Properties

        public IList<SvgTestResult> TestResults
        {
            get {
                if (_testViewPanel != null)
                {
                    return _testViewPanel.TestResults;
                }
                return null;
            }
        }

        public ITestPagePanel ActivePagePanel
        {
            get {
                if (_dockPanel == null || _dockPanel.IsDisposed)
                {
                    return null;
                }

                ITestPagePanel pagePanel = _dockPanel.ActiveDocument as ITestPagePanel;
                if (pagePanel == null || pagePanel.IsDisposed)
                {
                    return null;
                }
                return pagePanel;
            }
        }

        public OptionSettings OptionSettings
        {
            get {
                return _optionSettings;
            }
            set {
                if (value != null)
                {
                    _optionSettings = value;

                    if (_optionSettings.IsLocalSuitePathChanged(_testViewPanel.SuitePath))
                    {
                        if (OptionSettings.IsTestSuiteAvailable(_optionSettings.LocalSuitePath))
                        {
                            _testViewPanel.InitializePath(_optionSettings.LocalSuitePath);
                        }
                    }
                    else if (_isTestAvailable == false)
                    {
                        if (OptionSettings.IsTestSuiteAvailable(_optionSettings.LocalSuitePath))
                        {
                            _testViewPanel.InitializePath(_optionSettings.LocalSuitePath);
                        }
                    }

                    //TODO: Dynamic theme will require saving the windows, closing all and recreating...
                    //this.ApplyTheme(_optionSettings.Theme);
                }
            }
        }

        #endregion

        #region Public Methods

        public void IdleUpdate()
        {
            var dockPanels = new DockPanelContent[]
            {
                _inputPanel,
                _testViewPanel,
                _debugPanel,
                _testPanel,
                _aboutTestPanel,
                _settingsPanel,
                _resultsPanel
            };

            foreach (var dockPanel in dockPanels)
            {
                dockPanel.IdleUpdate();
            }
        }

        #endregion

        #region Private Form Events Handlers

        private void OnFormLoad(object sender, EventArgs e)
        {
            DataSecurityProtocols.Initialize();

            string selectedPath = _optionSettings.LocalSuitePath;

            if (OptionSettings.IsTestSuiteAvailable(selectedPath))
            {
                _isTestAvailable = true;
                if (_testViewPanel != null)
                {
                    _testViewPanel.InitializePath(selectedPath);
                }
            }
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            string backupFile = null;
            if (File.Exists(_testSettingsPath))
            {
                backupFile = Path.ChangeExtension(_testSettingsPath, ".bak");
                try
                {
                    if (File.Exists(backupFile))
                    {
                        File.Delete(backupFile);
                    }
                    File.Move(_testSettingsPath, backupFile);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), AppErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }
            }
            try
            {
                _optionSettings.Save(_testSettingsPath, this);
            }
            catch (Exception ex)
            {
                if (File.Exists(backupFile))
                {
                    File.Move(backupFile, _testSettingsPath);
                }

                MessageBox.Show(ex.ToString(), AppErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (!string.IsNullOrWhiteSpace(backupFile) && File.Exists(backupFile))
            {
                File.Delete(backupFile);
            }

            this.UnInitializePanels();
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void OnFormShown(object sender, EventArgs e)
        {
            if (!_isTestAvailable)
            {
                if (_testViewPanel == null)
                {
                    return;
                }

                PromptDialog dlg = new PromptDialog();
                dlg.Owner = this;
                dlg.OptionSettings = _optionSettings;

                var dialogResult = dlg.ShowDialog();

                if (dialogResult == DialogResult.OK)
                {
                    if (_optionSettings.IsLocalSuitePathChanged(_testViewPanel.SuitePath))
                    {
                        if (OptionSettings.IsTestSuiteAvailable(_optionSettings.LocalSuitePath))
                        {
                            _testViewPanel.InitializePath(_optionSettings.LocalSuitePath);
                        }
                    }
                    else if (_isTestAvailable == false)
                    {
                        if (OptionSettings.IsTestSuiteAvailable(_optionSettings.LocalSuitePath))
                        {
                            _testViewPanel.InitializePath(_optionSettings.LocalSuitePath);
                        }
                    }
                }
            }
        }

        #endregion

        #region Private Methods

        private void ApplyTheme(DockingTheme theme)
        {
            if (_currentTheme != theme)
            {
                _currentTheme = theme;
                var dockPanels = new DockPanelContent[]
                {
                    _testViewPanel,
                    _debugPanel,
                    _testPanel,
                    _aboutTestPanel,
                    _inputPanel,
                    _settingsPanel,
                    _resultsPanel
                };

                ThemeBase currentTheme = _vS2015LightTheme;
                switch (_currentTheme)
                {
                    case DockingTheme.LightTheme:
                        currentTheme = _vS2015LightTheme;
                        break;
                    case DockingTheme.BlueTheme:
                        currentTheme = _vS2015BlueTheme;
                        break;
                    case DockingTheme.DarkTheme:
                        currentTheme = _vS2015DarkTheme;
                        break;
                }

                foreach (var dockPanel in dockPanels)
                {
                    dockPanel.Theme = currentTheme;
                }

                _dockPanel.Theme = currentTheme;
            }
        }

        private void InitializePanels()
        {
            _dockPanel        = new DockPanel();
            _vS2015LightTheme = new VS2015LightTheme();
            _vS2015BlueTheme  = new VS2015BlueTheme();
            _vS2015DarkTheme  = new VS2015DarkTheme();

            _currentTheme     = _optionSettings.Theme;

            ThemeBase currentTheme = _vS2015LightTheme;
            switch (_currentTheme)
            {
                case DockingTheme.LightTheme:
                    currentTheme = _vS2015LightTheme;
                    break;
                case DockingTheme.BlueTheme:
                    currentTheme = _vS2015BlueTheme;
                    break;
                case DockingTheme.DarkTheme:
                    currentTheme = _vS2015DarkTheme;
                    break;
            }

            _dockPanel.Dock                       = DockStyle.Fill;
            _dockPanel.DockBackColor              = Color.White;
            _dockPanel.DockBottomPortion          = 300D;
            _dockPanel.DockLeftPortion            = 350D;
            _dockPanel.DockRightPortion           = 300D;
            _dockPanel.DockTopPortion             = 150D;
            _dockPanel.Font                       = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.World, 0);
            _dockPanel.Location                   = new Point(0, 49);
            _dockPanel.Name                       = "dockPanel";
            _dockPanel.Padding                    = new Padding(6);
            _dockPanel.RightToLeftLayout          = true;
            _dockPanel.ShowAutoHideContentOnHover = true;
            _dockPanel.Size                       = new Size(this.Width - 10, this.Height - 10);
            _dockPanel.TabIndex                   = 0;
            _dockPanel.Theme                      = currentTheme;
            _dockPanel.DocumentStyle              = DocumentStyle.DockingWindow;
            _dockPanel.ShowDocumentIcon           = true;
            _dockPanel.AllowEndUserDocking        = false;
            _dockPanel.AllowEndUserNestedDocking  = false;

            this.Controls.Add(_dockPanel);

            _vS2015LightTheme.Skin.DockPaneStripSkin.TextFont = new Font("Segoe UI", 16F, FontStyle.Regular, GraphicsUnit.World, 0);
            _vS2015BlueTheme.Skin.DockPaneStripSkin.TextFont = new Font("Segoe UI", 16F, FontStyle.Regular, GraphicsUnit.World, 0);
            _vS2015DarkTheme.Skin.DockPaneStripSkin.TextFont = new Font("Segoe UI", 16F, FontStyle.Regular, GraphicsUnit.World, 0);

            _testViewPanel = new TestViewDockPanel();
            _testViewPanel.Text = "W3C SVG Test Suite";
            _testViewPanel.Show(_dockPanel, DockState.DockLeft);

            _debugPanel = new DebugDockPanel();
            _debugPanel.Text = "Debugging";
            _debugPanel.Show(_dockPanel, DockState.DockBottomAutoHide);

            _testPanel = new TestDockPanel();
            _testPanel.Text = "Test";
            _testPanel.Show(_dockPanel, DockState.Document);

            _aboutTestPanel = new AboutTestDockPanel();
            _aboutTestPanel.Text = "About Test";
            _aboutTestPanel.Show(_testPanel.Pane, null);

            _inputPanel = new SvgInputDockPanel();
            _inputPanel.Text = "Svg Input";
            _inputPanel.Show(_testPanel.Pane, null);

            _settingsPanel = new SettingsDockPanel();
            _settingsPanel.Text = "Settings";
            _settingsPanel.Show(_testPanel.Pane, null);

            _resultsPanel = new ResultsDockPanel();
            _resultsPanel.Text = "Test Results";
            _resultsPanel.Show(_testPanel.Pane, null);

            _testViewPanel.Pages.Add(_aboutTestPanel);
            _testViewPanel.Pages.Add(_inputPanel);
            _testViewPanel.Pages.Add(_testPanel);

            var dockPanels = new DockPanelContent[]
            {
                _testViewPanel,
                _debugPanel,
                _testPanel,
                _aboutTestPanel,
                _inputPanel,
                _settingsPanel,
                _resultsPanel
            };

            foreach (var dockPanel in dockPanels)
            {
                dockPanel.InitializePanel(this, _optionSettings, currentTheme);
            }

            _dockPanel.ActiveContentChanged  += OnDockPanelActiveContentChanged;
            _dockPanel.ActiveDocumentChanged += OnDockPanelActiveDocumentChanged;
            _dockPanel.ActivePaneChanged     += OnDockPanelActivePaneChanged;

            _testPanel.Activate();
        }

        private void OnDockPanelActivePaneChanged(object sender, EventArgs e)
        {
        }

        private void OnDockPanelActiveDocumentChanged(object sender, EventArgs e)
        {
            this.OnActivePagePanelChanged(e);
        }

        private void OnDockPanelActiveContentChanged(object sender, EventArgs e)
        {
        }

        private void UnInitializePanels()
        {
            this.WindowState = FormWindowState.Minimized;

            // we don't want to create another instance of tool window, set DockPanel to null
            _debugPanel.DockPanel = null;
            if (_debugPanel.DockHandler != null)
            {
                _debugPanel.DockHandler.Close();
            }
            _testViewPanel.DockPanel = null;
            if (_testViewPanel.DockHandler != null)
            {
                _testViewPanel.DockHandler.Close();
            }

            // Close all other document windows
            foreach (IDockContent document in _dockPanel.DocumentsToArray())
            {
                // IMPORANT: dispose all panes.
                document.DockHandler.DockPanel = null;
                document.DockHandler.Close();
            }

            _aboutTestPanel.DockPanel = null;
            _testPanel.DockPanel      = null;
            _inputPanel.DockPanel     = null;
            _settingsPanel.DockPanel  = null;
            _resultsPanel.DockPanel   = null;

            // IMPORTANT: dispose all float windows.
            foreach (var window in _dockPanel.FloatWindows.ToList())
            {
                window.Dispose();
            }

            Debug.Assert(_dockPanel.Panes.Count == 0);
            Debug.Assert(_dockPanel.Contents.Count == 0);
            Debug.Assert(_dockPanel.FloatWindows.Count == 0);
        }

        internal void OnActivePagePanelChanged(EventArgs e)
        {
            ITestPagePanel newWindow = this.ActivePagePanel;

            if (_selectedPagePanel != null && _selectedPagePanel.IsDisposed == false)
            {
                _selectedPagePanel.OnPageDeselected(EventArgs.Empty);
            }
            _selectedPagePanel = newWindow;
            if (newWindow != null && newWindow.IsDisposed == false)
            {
                newWindow.OnPageSelected(EventArgs.Empty);
            }
        }

        #endregion
    }
}
