using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using BrightIdeasSoftware;

using GDMultiStash.Common;
using GDMultiStash.Common.Objects;

namespace GDMultiStash.Forms
{
    internal partial class MainForm : BaseForm
    {
        private Plexiglass.DockingPlexiglass _plexiglass = null;

        public Main.DecorationCollection Decorations { get; } = new Main.DecorationCollection();
        public Main.StashesPage StashesPage { get; }
        public Main.StashGroupsPage StashGroupsPage { get; }

        public EventHandler<EventArgs> SpaceClick;

        #region pages

        class PageHolder
        {
            public Main.Page Page { get; }
            public Button Button { get; }

            public PageHolder(Main.Page page, Button button)
            {
                Page = page;
                Button = button;
            }
        }

        private int currentPageIndex = -1;
        //private readonly EventHandler<EventArgs> PageChanged;
        private readonly List<PageHolder> pages = new List<PageHolder>();

        private void InitPage(Main.Page page, Button button)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseDownBackColor = Constants.PageButtonBackColorActive;
            button.FlatAppearance.MouseOverBackColor = Constants.PageButtonBackColorActive;
            button.BackColor = Constants.PageButtonBackColor;
            button.ForeColor = Constants.InteractiveForeColor;

            page.BackColor = Constants.PageBackColor;
            page.Dock = DockStyle.Fill;
            page.Visible = false;

            int pageIndex = pages.Count;
            pages.Add(new PageHolder(page, button));
            pagesPaddingPanel.Controls.Add(page);

            button.MouseEnter += delegate { button.ForeColor = Constants.InteractiveForeColorHighlight; };
            button.MouseLeave += delegate { if (pageIndex != currentPageIndex) button.ForeColor = Constants.InteractiveForeColor; };
            button.Click += delegate { ShowPage(pageIndex); };
            button.GotFocus += delegate { ClearFocus(); };
        }

        private void ShowPage(int pageIndex)
        {
            if (currentPageIndex != -1)
            {
                pages[currentPageIndex].Button.BackColor = Constants.PageButtonBackColor;
                pages[currentPageIndex].Button.ForeColor = Constants.InteractiveForeColor;
                pages[currentPageIndex].Page.Visible = false;
            }
            currentPageIndex = pageIndex;
            pages[currentPageIndex].Button.BackColor = Constants.PageButtonBackColorActive;
            pages[currentPageIndex].Button.ForeColor = Constants.InteractiveForeColorHighlight;
            pages[currentPageIndex].Page.Visible = true;
            //PageChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region WndProc 

        Rectangle BorderTop { get { return new Rectangle(0, 0, ClientSize.Width, Constants.WindowResizeBorderSize); } }
        Rectangle BorderLeft { get { return new Rectangle(0, 0, Constants.WindowResizeBorderSize, ClientSize.Height); } }
        Rectangle BorderBottom { get { return new Rectangle(0, ClientSize.Height - Constants.WindowResizeBorderSize, ClientSize.Width, Constants.WindowResizeBorderSize); } }
        Rectangle BorderRight { get { return new Rectangle(ClientSize.Width - Constants.WindowResizeBorderSize, 0, Constants.WindowResizeBorderSize, ClientSize.Height); } }

        protected override CreateParams CreateParams
        {
            get
            {
                // this is required to be able
                // to minimize/restore the window by click
                // on taskbar item
                CreateParams cp = base.CreateParams;
                cp.Style |= Native.WS.MINIMIZEBOX;
                cp.ClassStyle |= Native.CS_DBLCLKS;
                return cp;
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Native.WM_NCLBUTTONDBLCLK) return; // disable doubleclick on titlebar
            base.WndProc(ref m);
            if (m.Msg == Global.WM_SHOWME)
            {
                if (!Visible)
                    Show();
                else
                    MessageBox.Show(Global.L.AlreadyRunningMessage(), "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            else if (m.Msg == 0x84)
            {
                var cursor = PointToClient(Cursor.Position);

                bool tHit = BorderTop.Contains(cursor);
                bool lHit = BorderLeft.Contains(cursor);
                bool rHit = BorderRight.Contains(cursor);
                bool bHit = BorderBottom.Contains(cursor);
                if (bHit && rHit) m.Result = (IntPtr)Native.HT.BOTTOMRIGHT;
                else if (bHit && lHit) m.Result = (IntPtr)Native.HT.BOTTOMLEFT;
                else if (tHit && rHit) m.Result = (IntPtr)Native.HT.TOPRIGHT;
                else if (tHit && lHit) m.Result = (IntPtr)Native.HT.TOPLEFT;
                else if (bHit) m.Result = (IntPtr)Native.HT.BOTTOM;
                else if (rHit) m.Result = (IntPtr)Native.HT.RIGHT;
                else if (lHit) m.Result = (IntPtr)Native.HT.LEFT;
                else if (tHit) m.Result = (IntPtr)Native.HT.TOP;
                else if (cursor.Y < Constants.WindowCaptionDragHeight)
                {
                    m.Result = (IntPtr)Native.HT.CAPTION;
                    return;
                }

            }

        }

        #endregion

        #region buttons

        public delegate Image ButtonImageGetter();

        public void InitializeButton(Button button,
            Color backColor, Color backColorHover, Color backColorPressed,
            Color foreColor, Color foreColorHover,
            ButtonImageGetter imageNormal,
            ButtonImageGetter imageHover,
            Action onClick)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = backColorHover;
            button.FlatAppearance.MouseDownBackColor = backColorPressed;
            button.BackColor = backColor;
            button.ForeColor = foreColor;
            button.Image = imageNormal();
            button.MouseEnter += delegate { button.ForeColor = foreColorHover; button.Image = imageHover(); };
            button.MouseLeave += delegate { button.ForeColor = foreColor; button.Image = imageNormal(); };
            button.Click += delegate { onClick(); };
            button.GotFocus += delegate { ClearFocus(); }; // unfocus, hide ugly border
        }

        public void InitializeToolStripButton(ToolStripMenuItem item,
            Color backColor, Color backColorHover,
            Color foreColor, Color foreColorHover)
        {
            bool hover = false;
            bool opened = false;
            item.MouseEnter += delegate { hover = true; item.ForeColor = foreColorHover; item.BackColor = backColorHover; };
            item.MouseLeave += delegate { hover = false; if (!opened) item.ForeColor = foreColor; item.BackColor = backColor; };
            item.DropDownOpened += delegate { opened = true; item.ForeColor = foreColorHover; };
            item.DropDownClosed += delegate { opened = false; if (!hover) item.ForeColor = foreColor; };
            item.BackColor = backColor;
            item.ForeColor = foreColor;
        }

        #endregion

        //protected override bool ShowWithoutActivation => true;

        public MainForm() : base()
        {
            InitializeComponent();

            DoubleBuffered = true;
            Icon = Properties.Resources.icon32;
            FormBorderStyle = FormBorderStyle.None;
            SetStyle(ControlStyles.ResizeRedraw, true);
            TopMost = false;

            #region Init Buttons

            InitializeButton(captionGameButton,
                Constants.CaptionButtonBackColor, Color.FromArgb(0, 102, 77), Color.FromArgb(0, 135, 102),
                Constants.InteractiveForeColor, Constants.InteractiveForeColorHighlight,
                delegate { return null; },
                delegate { return null; },
                delegate {
                    switch (Global.Windows.StartGame())
                    {
                        case GlobalHandlers.WindowsHandler.GameStartResult.AlreadyRunning:
                            Console.Warning(Global.L.GameAlreadyRunningMessage());
                            break;
                        case GlobalHandlers.WindowsHandler.GameStartResult.Success:
                            break;
                    }
                }
                );

            InitializeButton(captionTrayButton,
                Constants.CaptionButtonBackColor, Constants.CaptionButtonBackColorHover, Constants.CaptionButtonBackColorPressed,
                Constants.InteractiveForeColor, Constants.InteractiveForeColorHighlight,
                delegate { return Properties.Resources.buttonTrayGray; },
                delegate { return Properties.Resources.buttonTrayWhite; },
                delegate { CloseToTray(); }
                );

            InitializeButton(captionMinimizeButton,
                Constants.CaptionButtonBackColor, Constants.CaptionButtonBackColorHover, Constants.CaptionButtonBackColorPressed,
                Constants.InteractiveForeColor, Constants.InteractiveForeColorHighlight,
                delegate { return Properties.Resources.buttonMinimizeGray; },
                delegate { return Properties.Resources.buttonMinimizeWhite; },
                delegate { WindowState = FormWindowState.Minimized; }
                );

            InitializeButton(captionCloseButton,
                Constants.CaptionButtonBackColor, Color.FromArgb(150, 32, 5), Color.FromArgb(207, 49, 12),
                Constants.InteractiveForeColor, Constants.InteractiveForeColorHighlight,
                delegate { return Properties.Resources.buttonCloseGray; },
                delegate { return Properties.Resources.buttonCloseWhite; },
                delegate { Close(); }
                );

            InitializeToolStripButton(captionFileButton,
                Constants.CaptionButtonBackColor, Constants.CaptionButtonBackColorHover,
                Constants.InteractiveForeColor, Constants.InteractiveForeColorHighlight
                );

            InitializeToolStripButton(captionHelpButton,
                Constants.CaptionButtonBackColor, Constants.CaptionButtonBackColorHover,
                Constants.InteractiveForeColor, Constants.InteractiveForeColorHighlight
                );

            captionMenuStrip.Renderer = new Controls.FlatToolStripRenderer();

            #endregion

            BackColor = Constants.FormBackColor;
            formBackgroundPanel.BackColor = Constants.FormBackColor;
            BackgroundImage = Properties.Resources.border;
            BackgroundImageLayout = ImageLayout.Stretch;
            titlePanel.BackgroundImage = Properties.Resources.title;
            titlePanel.BackgroundImageLayout = ImageLayout.Zoom;

            formPaddingPanel.BackColor = Constants.FormBackColor;
            formPaddingPanel.Padding = Constants.FormPadding;

            pagesPaddingPanel.BackColor = Constants.PageBackColor;
            pagesPaddingPanel.Padding = Constants.PagesPadding;

            captionMenuStrip.BackColor = Constants.FormBackColor;

            AllowDrop = false; // used to drag transfer files into the window

            // unselect items by clicking on empty space
            EventHandler spaceClickhandler = new EventHandler((sender, e) => SpaceClick?.Invoke(sender, e));
            formBackgroundPanel.Click += spaceClickhandler;
            formPaddingPanel.Click += spaceClickhandler;
            pagesPaddingPanel.Click += spaceClickhandler;

            StashesPage = new Main.StashesPage(this);
            InitPage(StashesPage, stashesPageButton);

            StashGroupsPage = new Main.StashGroupsPage(this);
            InitPage(StashGroupsPage, stashGroupsPageButton);

            ShowPage(0);
        }

        protected override void Localize(GlobalHandlers.LocalizationHandler.StringsHolder L)
        {
            Text = Global.AppName;

            // form
            captionGameButton.Text = L.StartGameButton();
            captionFileButton.Text = L.FileButton();
            captionHelpButton.Text = L.HelpButton();
            captionImportButton.Text = L.ImportButton();
            captionImportTransferFilesButton.Text = L.TransferFilesButton();
            captionExportButton.Text = L.ExportButton();
            captionExportTransferFilesButton.Text = L.TransferFilesButton();
            captionSettingsButton.Text = L.SettingsButton();
            captionChangelogButton.Text = L.ChangelogButton();
            captionAboutButton.Text = L.AboutButton();
            captionImportCraftingModeButton.Text = L.CraftingModeButton();

            // pages
            StashesPage.Localize();
            stashesPageButton.Text = L.StashesButton();
            StashGroupsPage.Localize();
            stashGroupsPageButton.Text = L.GroupsButton();
        }

        #region public methods

        public void ClearFocus()
        {
            titlePanel.Focus();
        }

        public void CloseToTray()
        {
            Hide();
            _plexiglass.Hide();
        }

        #endregion

        #region form events

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // fix: objectlistview is causing huge lags when form is in background
            _plexiglass = new Plexiglass.DockingPlexiglass(formPaddingPanel)
            {
                Color = Constants.FormBackColor,
                Opacity = 0.50,
                Margin = new Padding(0, 0, 0, 0)
            };
            // only after cfg has been initialized!
            Width = Global.Configuration.Settings.WindowWidth;
            Height = Global.Configuration.Settings.WindowHeight;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (Program.IsQuitting)
            {
                return;
            }
            bool gameOpened = Native.FindWindow("Grim Dawn", null) != IntPtr.Zero;
            if (gameOpened && Global.Configuration.Settings.ConfirmClosing)
            {
                DialogResult result = MessageBox.Show(Global.L.ConfirmClosingMessage(), "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                e.Cancel = (result == DialogResult.Cancel);
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            Program.Quit();
        }

        protected override void OnResizeEnd(EventArgs e)
        {
            base.OnResizeEnd(e);
            Global.Configuration.Settings.WindowWidth = Width;
            Global.Configuration.Settings.WindowHeight = Height;
            Global.Configuration.Save();
        }

        #endregion

        #region controls events

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            Global.Windows.ShowConfigurationWindow();
        }

        private void AboutButton_Click(object sender, EventArgs e)
        {
            Global.Windows.ShowAboutDialog();
        }

        private void ChangelogButton_Click(object sender, EventArgs e)
        {
            Global.Windows.ShowChangelogWindow();
        }

        private void ImportTransferFilesButton_Click(object sender, EventArgs e)
        {
            Global.Windows.ShowImportDialog();
        }

        private void ImportGDSCButton_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog()
            {
                Filter = $"gd_conf|gd_conf.xml",
                Multiselect = false,
            })
            {
                DialogResult result = dialog.ShowDialog();
                if (result != DialogResult.OK) return;

                var importedStashes = new List<StashObject>();
                string stashesPath = System.IO.Path.Combine(new System.IO.FileInfo(dialog.FileName).Directory.FullName, "Stashes");

                var reItem = new System.Text.RegularExpressions.Regex(@"<item\s+([^>]+)>");
                var reID = new System.Text.RegularExpressions.Regex(@"id=""(\d+)""", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                var reName = new System.Text.RegularExpressions.Regex(@"name=""([^""]+)""");
                var reColor = new System.Text.RegularExpressions.Regex(@"tcolor=""0x([a-f0-9]{6})""");
                var matches = reItem.Matches(System.IO.File.ReadAllText(dialog.FileName));
                foreach (System.Text.RegularExpressions.Match match in matches)
                {
                    var inner = match.Groups[1].Value;
                    var mID = reID.Match(inner);
                    var mName = reName.Match(inner);
                    var mColor = reColor.Match(inner);
                    if (!mID.Success) continue;
                    if (!mName.Success) continue;

                    var stashID = mID.Groups[1].Value;
                    string stashFile = System.IO.Directory.GetFiles(System.IO.Path.Combine(stashesPath, stashID))
                        .FirstOrDefault(name => true);
                    if (stashFile == null) continue;

                    if (!TransferFile.FromFile(stashFile, out TransferFile transferFile)) continue;
                    if (transferFile.Expansion == GrimDawnLib.GrimDawnGameExpansion.Unknown) continue;
                    if (transferFile.TotalUsage == 0) continue; // no items inside

                    var stashName = System.Web.HttpUtility.HtmlDecode(mName.Groups[1].Value.Trim());
                    StashObject stash = Global.Stashes.CreateImportStash(stashFile, stashName, transferFile.Expansion, GrimDawnLib.GrimDawnGameMode.Both);
                    if (stash == null) continue;
                    importedStashes.Add(stash);

                    if (mColor.Success)
                        stash.TextColor = $"#{mColor.Groups[1].Value}";
                }

                if (importedStashes.Count != 0)
                {
                    var group = Global.Stashes.CreateStashGroup("GDSC", true);
                    foreach(var s in importedStashes)
                        s.GroupID = group.ID;
                    Global.Configuration.Save();
                    Global.Ingame.InvokeStashesAdded(importedStashes);
                    Global.Ingame.InvokeStashGroupsAdded(group);
                }
            }
        }

        private void CaptionImportCraftingModeButton_Click(object sender, EventArgs e)
        {
            Global.Windows.ShowCraftingModeDialog();
        }

        private void TopMenuExportTransferFilesButton_Click(object sender, EventArgs e)
        {

            // TODO: redundant code in right click context -> export selected stashes

            StashesZipFile zipFile = new StashesZipFile();
            foreach (StashObject selStash in Global.Stashes.GetAllStashes())
                zipFile.AddStash(selStash);

            using (var dialog = new SaveFileDialog()
            {
                Filter = $"{Global.L.ZipArchive()}|*.zip",
                FileName = "TransferFiles.zip",
            })
            {
                DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.FileName))
                {
                    zipFile.SaveTo(dialog.FileName);
                }
            }
        }

        #endregion

    }
}
