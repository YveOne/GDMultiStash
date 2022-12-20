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

        #region constants

        private const int WindowCaptionDragHeight = 60;
        private const int WindowResizeBorderSize = 8;

        private const int listViewColumnsHeight = 30;
        private const int listViewColumnsFontHeight = 11;
        private const int listViewRowHeight = 25;
        private const int listViewGroupHeaderHeight = 25;
        private const int listViewGroupSpaceBetween = 5;

        private readonly Padding formPadding = new Padding(20, 20, 20, 20);
        private readonly Padding pagesPadding = new Padding(5, 5, 5, 5);
        private readonly Padding listViewBorderPadding = new Padding(5, 5, 5, 5);

        private readonly Color formBackColor = Color.FromArgb(28, 28, 28);

        private readonly Color interactiveForeColor = Color.FromArgb(200, 200, 200);
        private readonly Color interactiveForeColorHighlight = Color.FromArgb(250, 250, 250);
        private readonly Color passiveForeColor = Color.FromArgb(120, 120, 120);

        private readonly Color captionButtonBackColor = Color.FromArgb(28, 28, 28);
        private readonly Color captionButtonBackColorHover = Color.FromArgb(50, 50, 50);
        private readonly Color captionButtonBackColorPressed = Color.FromArgb(70, 70, 70);

        private readonly Color pageButtonBackColor = Color.FromArgb(35, 35, 35);
        private readonly Color pageButtonBackColorActive = Color.FromArgb(45, 45, 45);
        private readonly Color pagePanelBackColor = Color.FromArgb(45, 45, 45);

        private readonly Color toolStripBackColor = Color.FromArgb(45, 45, 45);
        private readonly Color toolStripButtonBackColor = Color.FromArgb(45, 45, 45);
        private readonly Color toolStripButtonBackColorHover = Color.FromArgb(45, 45, 45);

        private readonly Color listViewBackColor = Color.FromArgb(37, 37, 37);
        private readonly Color listViewItemBackColor = Color.FromArgb(50, 50, 50);
        private readonly Color listViewCellBorderColor = Color.FromArgb(37, 37, 37);
        private readonly Color listViewGroupHeaderBackColor = Color.FromArgb(60, 60, 60);
        private readonly Color listViewGroupHeaderCountForeColor = Color.FromArgb(180, 180, 180);
        private readonly Color listViewGroupHeaderForeColor = Color.FromArgb(245, 245, 245);
        private readonly Color listViewGroupHeaderSeparatorColor = Color.FromArgb(50, 50, 50);

        private readonly Color scrollBarColor = Color.FromArgb(100, 100, 100);

        #endregion

        #region classes

        private class ToolStripRenderer : ToolStripProfessionalRenderer
        {
            protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
            {
                if (e.Item.OwnerItem == null)
                {
                    // first level buttons
                    Rectangle rc = new Rectangle(Point.Empty, e.Item.Size);
                    e.Graphics.FillRectangle(new SolidBrush(e.Item.BackColor), rc);
                    return;
                }
                //if (!e.Item.Selected) base.OnRenderMenuItemBackground(e);
                base.OnRenderMenuItemBackground(e);
            }
        }

        private class DefaultOLVColumn : OLVColumn
        {
            public DefaultOLVColumn() : base()
            {
                Searchable = false;
                Groupable = false;
                Sortable = false;
                IsEditable = false;
                CheckBoxes = false;
            }
            public new int Width
            {
                get => base.Width;
                set
                {
                    base.Width = value;
                    base.MaximumWidth = value;
                    base.MinimumWidth = value;
                }
            }
        }

        //used to debug the border not positioned correctly
        private class CellBorderDecorationEx : CellBorderDecoration
        {
            private int w;
            public CellBorderDecorationEx(int w, Color col)
            {
                this.w = w;
                BorderPen = new Pen(col);
                FillBrush = null;
                BoundsPadding = new Size(0, 0);
                CornerRounding = 0;
            }
            protected override Rectangle CalculateBounds()
            {
                Rectangle r = base.CellBounds;
                r.X -= 1;
                r.Y -= 1;
                r.Width += w;
                r.Height += 1;
                return r;
            }
        }

        private class RowBorderDecorationEx : RowBorderDecoration
        {
            private int w;
            public RowBorderDecorationEx(int w, Color col)
            {
                this.w = w;
                BorderPen = new Pen(col);
                FillBrush = null;
                BoundsPadding = new Size(0, 0);
                CornerRounding = 0;
            }
            protected override Rectangle CalculateBounds()
            {
                Rectangle r = base.CellBounds;
                r.X -= 1;
                r.Y = r.Y - 2 + r.Height;
                r.Width += w;
                r.Height = 1;
                return r;
            }
        }

        #endregion

        #region WndProc 

        Rectangle BorderTop { get { return new Rectangle(0, 0, ClientSize.Width, WindowResizeBorderSize); } }
        Rectangle BorderLeft { get { return new Rectangle(0, 0, WindowResizeBorderSize, ClientSize.Height); } }
        Rectangle BorderBottom { get { return new Rectangle(0, ClientSize.Height - WindowResizeBorderSize, ClientSize.Width, WindowResizeBorderSize); } }
        Rectangle BorderRight { get { return new Rectangle(ClientSize.Width - WindowResizeBorderSize, 0, WindowResizeBorderSize, ClientSize.Height); } }

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
                else if (cursor.Y < WindowCaptionDragHeight)
                {
                    m.Result = (IntPtr)Native.HT.CAPTION;
                    return;
                }

            }

        }

        #endregion

        private delegate Image ButtonImageGetter();
        private event EventHandler SpaceClick;

        private readonly NotifyIcon trayIcon;

        private readonly CellBorderDecorationEx cellBorderFirstDecoration;
        private readonly CellBorderDecorationEx cellBorderDecoration;
        private readonly RowBorderDecorationEx rowBorderFirstDecoration;
        private readonly RowBorderDecorationEx rowBorderDecoration;

        private readonly ImageDecoration lockDecoration;

        private readonly ImageDecoration chkHideDecoration;
        private readonly ImageDecoration chkBackDecoration;
        private readonly ImageDecoration chkBackHoverDecoration;
        private readonly ImageDecoration chkTickDecoration;
        private readonly ImageDecoration chkTickDisabledDecoration;
        private readonly ImageDecoration chkCrossDecoration;
        private readonly ImageDecoration chkCrossDisabledDecoration;

        private int currentPageIndex = -1;
        private readonly EventHandler<EventArgs> PageChanged;
        private readonly List<Button> pageButtons = new List<Button>();
        private readonly List<Panel> pagePanels = new List<Panel>();

        private void InitializeButton(Button button,
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
            button.GotFocus += delegate { titlePanel.Focus(); }; // unfocus, hide ugly border
        }

        private void InitializeToolStripButton(ToolStripMenuItem item,
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

        private void ShowPage(int pageIndex)
        {
            if (currentPageIndex != -1)
            {
                pageButtons[currentPageIndex].BackColor = pageButtonBackColor;
                pageButtons[currentPageIndex].ForeColor = interactiveForeColor;
                pagePanels[currentPageIndex].Visible = false;
            }
            currentPageIndex = pageIndex;
            pageButtons[currentPageIndex].BackColor = pageButtonBackColorActive;
            pageButtons[currentPageIndex].ForeColor = interactiveForeColorHighlight;
            pagePanels[currentPageIndex].Visible = true;
            PageChanged?.Invoke(this, EventArgs.Empty);
        }

        private void InitializePage(Button button, Panel page)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseDownBackColor = pageButtonBackColorActive;
            button.FlatAppearance.MouseOverBackColor = pageButtonBackColorActive;
            button.BackColor = pageButtonBackColor;
            button.ForeColor = interactiveForeColor;

            page.BackColor = pagePanelBackColor;
            page.Dock = DockStyle.Fill;
            page.Visible = false;

            int pageIndex = pageButtons.Count;
            pageButtons.Add(button);
            pagePanels.Add(page);

            button.MouseEnter += delegate { button.ForeColor = interactiveForeColorHighlight; };
            button.MouseLeave += delegate { if (pageIndex != currentPageIndex) button.ForeColor = interactiveForeColor; };
            button.Click += delegate { ShowPage(pageIndex); };
            button.GotFocus += delegate { titlePanel.Focus(); }; // unfocus, hide ugly border
        }

        protected override void Localize(GlobalHandlers.LocalizationHandler.StringsHolder L)
        {
            Text = Global.AppName;

            stashesPageButton.Text = L.StashesButton();
            groupsPageButton.Text = L.GroupsButton();

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

            // stashes
            stashes_columnID.Text = L.IdColumn();
            stashes_columnName.Text = L.NameColumn();
            stashes_columnUsage.Text = L.UsageColumn();
            stashes_columnLastChange.Text = L.LastChangeColumn();
            stashes_columnSC.Text = L.SoftcoreColumn();
            stashes_columnHC.Text = L.HardcoreColumn();
            stashes_createStashButton.Text = L.CreateStashButton();
            stashes_showSoftCoreCheckbox.Text = stashes_columnSC.Text;
            stashes_showHardCoreCheckbox.Text = stashes_columnHC.Text;

            // groups
            groups_createStashGroupButton.Text = L.CreateGroupButton();
            groups_columnID.Text = L.IdColumn();
            groups_columnName.Text = L.NameColumn();
        }

        public MainForm() : base()
        {
            InitializeComponent();

            DoubleBuffered = true;
            Icon = Properties.Resources.icon32;
            FormBorderStyle = FormBorderStyle.None;
            SetStyle(ControlStyles.ResizeRedraw, true);

            #region Init Buttons

            InitializeButton(captionGameButton,
                captionButtonBackColor, Color.FromArgb(0, 102, 77), Color.FromArgb(0, 135, 102),
                interactiveForeColor, Color.FromArgb(255, 255, 255),
                delegate { return null; },
                delegate { return null; },
                delegate {
                    switch (Global.Runtime.StartGame())
                    {
                        case GlobalHandlers.RuntimeHandler.GameStartResult.AlreadyRunning:
                            MessageBox.Show(Global.L.GameAlreadyRunningMessage(), "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            break;
                        case GlobalHandlers.RuntimeHandler.GameStartResult.Success:
                            break;
                    }
                }
                );

            InitializeButton(captionTrayButton,
                captionButtonBackColor, captionButtonBackColorHover, captionButtonBackColorPressed,
                interactiveForeColor, interactiveForeColorHighlight,
                delegate { return Properties.Resources.buttonTrayGray; },
                delegate { return Properties.Resources.buttonTrayWhite; },
                delegate { Hide(); }
                );

            InitializeButton(captionMinimizeButton,
                captionButtonBackColor, captionButtonBackColorHover, captionButtonBackColorPressed,
                interactiveForeColor, interactiveForeColorHighlight,
                delegate { return Properties.Resources.buttonMinimizeGray; },
                delegate { return Properties.Resources.buttonMinimizeWhite; },
                delegate { WindowState = FormWindowState.Minimized; }
                );

            InitializeButton(captionCloseButton,
                captionButtonBackColor, Color.FromArgb(150, 32, 5), Color.FromArgb(207, 49, 12),
                interactiveForeColor, Color.FromArgb(255, 255, 255),
                delegate { return Properties.Resources.buttonCloseGray; },
                delegate { return Properties.Resources.buttonCloseWhite; },
                delegate { Close(); }
                );

            InitializeToolStripButton(captionFileButton,
                captionButtonBackColor, captionButtonBackColorHover,
                interactiveForeColor, interactiveForeColorHighlight
                );

            InitializeToolStripButton(captionHelpButton,
                captionButtonBackColor, captionButtonBackColorHover,
                interactiveForeColor, interactiveForeColorHighlight
                );

            captionMenuStrip.Renderer = new ToolStripRenderer();

            #endregion

            #region Init Decos

            cellBorderFirstDecoration = new CellBorderDecorationEx(-1, listViewCellBorderColor);
            cellBorderDecoration = new CellBorderDecorationEx(0, listViewCellBorderColor);
            rowBorderFirstDecoration = new RowBorderDecorationEx(-1, listViewCellBorderColor);
            rowBorderDecoration = new RowBorderDecorationEx(0, listViewCellBorderColor);

            using (Bitmap bmp = new Bitmap(15, 15, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
            using (Graphics gfx = Graphics.FromImage(bmp))
            using (SolidBrush brush = new SolidBrush(listViewItemBackColor))
            {
                gfx.FillRectangle(brush, 0, 0, 15, 15);
                chkHideDecoration = new ImageDecoration(new Bitmap(bmp), ContentAlignment.MiddleCenter)
                {
                    Transparency = 255,
                    Offset = new Size(-1, -1),
                    ShrinkToWidth = false,
                };
            }
            chkBackDecoration = new ImageDecoration(new Bitmap(Properties.Resources.CheckBoxBack, new Size(15, 15)), ContentAlignment.MiddleCenter)
            {
                Transparency = 255,
                Offset = new Size(-1, -1),
                ShrinkToWidth = false,
            };
            chkBackHoverDecoration = new ImageDecoration(new Bitmap(Properties.Resources.CheckBoxBackHover, new Size(15, 15)), ContentAlignment.MiddleCenter)
            {
                Transparency = 255,
                Offset = new Size(-1, -1),
                ShrinkToWidth = false,
            };
            chkTickDecoration = new ImageDecoration(new Bitmap(Properties.Resources.CheckBoxTick, new Size(21, 21)), ContentAlignment.MiddleCenter)
            {
                Transparency = 255,
                Offset = new Size(-1, -1),
                ShrinkToWidth = false,
            };
            chkTickDisabledDecoration = new ImageDecoration(new Bitmap(Properties.Resources.CheckBoxTickDisabled, new Size(21, 21)), ContentAlignment.MiddleCenter)
            {
                Transparency = 255,
                Offset = new Size(-1, -1),
                ShrinkToWidth = false,
            };
            chkCrossDecoration = new ImageDecoration(new Bitmap(Properties.Resources.CheckBoxCross, new Size(21, 21)), ContentAlignment.MiddleCenter)
            {
                Transparency = 255,
                Offset = new Size(-1, -1),
                ShrinkToWidth = false,
            };
            chkCrossDisabledDecoration = new ImageDecoration(new Bitmap(Properties.Resources.CheckBoxCrossDisabled, new Size(21, 21)), ContentAlignment.MiddleCenter)
            {
                Transparency = 255,
                Offset = new Size(-1, -1),
                ShrinkToWidth = false,
            };




            lockDecoration = new ImageDecoration(new Bitmap(Properties.Resources.lockedWhite, new Size(15, 15)), ContentAlignment.MiddleRight)
            {
                Transparency = 180,
                Offset = new Size(-5, 0),
            };

            #endregion

            BackColor = formBackColor;
            formBackgroundPanel.BackColor = formBackColor;
            BackgroundImage = Properties.Resources.border;
            BackgroundImageLayout = ImageLayout.Stretch;
            titlePanel.BackgroundImage = Properties.Resources.title;
            titlePanel.BackgroundImageLayout = ImageLayout.Zoom;

            formPaddingPanel.BackColor = formBackColor;
            formPaddingPanel.Padding = formPadding;

            pagesPaddingPanel.BackColor = pagePanelBackColor;
            pagesPaddingPanel.Padding = pagesPadding;

            captionMenuStrip.BackColor = formBackColor;

            trayIcon = new NotifyIcon()
            {
                Icon = Properties.Resources.icon32,
                Visible = false,
                ContextMenu = new ContextMenu(new MenuItem[] {
                    new MenuItem("Exit", delegate {
                        Program.Quit();
                    })
                }),
            };
            trayIcon.DoubleClick += delegate { Show(); };

            AllowDrop = true; // used to drag transfer files into the window

            // unselect items by clicking on empty space
            EventHandler spaceClickhandler = new EventHandler((sender, e) => SpaceClick?.Invoke(sender, e));
            formBackgroundPanel.Click += spaceClickhandler;
            formPaddingPanel.Click += spaceClickhandler;
            pagesPaddingPanel.Click += spaceClickhandler;

            InitializeStashesPage();
            InitializePage(stashesPageButton, stashes_pagePanel);

            InitializeGroupsPage();
            InitializePage(groupsPageButton, groups_pagePanel);

            ShowPage(0);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Width = Global.Configuration.Settings.WindowWidth;
            Height = Global.Configuration.Settings.WindowHeight;
            // following events only after cfg has been initialized!
            ResizeEnd += MainForm_ResizeEnd;
            FormClosed += MainForm_FormClosed;
            FormClosing += MainForm_FormClosing;
        }

        #region methods

        public new void Show()
        {
            base.Show();
            if (trayIcon != null)
                trayIcon.Visible = false;
        }

        public new void Hide()
        {
            base.Hide();
            if (trayIcon != null)
                trayIcon.Visible = true;
        }

        #endregion

        #region events

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.Quit();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Program.Quitting)
            {
                trayIcon.Visible = false;
                return;
            }
            bool gameOpened = Native.FindWindow("Grim Dawn", null) != IntPtr.Zero;
            if (gameOpened && Global.Configuration.Settings.ConfirmClosing)
            {
                DialogResult result = MessageBox.Show(Global.L.ConfirmClosingMessage(), "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                e.Cancel = (result == DialogResult.Cancel);
            }
        }

        private void MainForm_ResizeEnd(object sender, EventArgs e)
        {
            Global.Configuration.Settings.WindowWidth = Width;
            Global.Configuration.Settings.WindowHeight = Height;
            Global.Configuration.Save();
        }

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

        private void TopMenuExportTransferFilesButton_Click(object sender, EventArgs e)
        {

            // TODO: redundant code in right click context -> export selected stashes

            StashesZipFile zipFile = new StashesZipFile();
            foreach (StashObject selStash in Global.Stashes.GetAllStashes()) zipFile.AddStash(selStash);

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
