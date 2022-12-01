using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using BrightIdeasSoftware;

using GDMultiStash.Common;

namespace GDMultiStash.Forms
{
    internal partial class MainForm : BaseForm
    {

        private readonly OLVColumn columnID;
        private readonly OLVColumn columnOrder;
        private readonly OLVColumn columnSC;
        private readonly OLVColumn columnHC;
        private readonly OLVColumn columnName;
        private readonly OLVColumn columnLastChange;
        private readonly OLVColumn columnUsage;
        private readonly OLVColumn columnActive;
        private readonly OLVColumn columnExpansion;
        private readonly OLVColumn columnColor;
        private readonly OLVColumn columnLocked;

        private Dictionary<int, string> _expansionNames;
        private StashesDragHandler _dragHandler;

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Native.WM_SHOWME)
            {
                if (Visible)
                    MessageBox.Show(Global.L["msg_gdms_already_running"], "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                    Show();
            }
            base.WndProc(ref m);
        }

        public MainForm() : base()
        {
            InitializeComponent();
            Icon = Properties.Resources.icon32;
            Text = "GD MultiStash";

            stashesListView.CellRightClick += StashesListView_CellRightClick;
            stashesListView.ColumnRightClick += StashesListView_ColumnRightClick;
            stashesListView.SubItemChecking += StashesListView_SubItemChecking;
            stashesListView.CellEditFinished += StashesListView_CellEditFinished;
            stashesListView.CellEditStarting += StashesListView_CellEditStarting;
            stashesListView.CellEditFinishing += StashesListView_CellEditFinishing;

            int displayIndex = 0;

            columnOrder = new OLVColumn()
            {
                DisplayIndex = displayIndex++,
                Text = "Order",
                Name = "orderColumn",
                AspectName = "Order",
                MaximumWidth = 50,
                MinimumWidth = 50,
                Width = 50,
                Searchable = false,
                Groupable = false,
                Sortable = true,

                TextAlign = HorizontalAlignment.Right,
            };

 
            stashesListView.HeaderUsesThemes = false;
            stashesListView.BaseSmallImageList = new ImageList();
            stashesListView.BaseSmallImageList.Images.Add("lock", Properties.Resources.lockBlack);


            columnLocked = new OLVColumn()
            {
                DisplayIndex = displayIndex++,
                Name = "lockedColumn",
                AspectName = "Locked",
                MaximumWidth = 40,
                MinimumWidth = 40,
                Width = 40,
                Searchable = false,
                Groupable = false,
                Sortable = false,

                ShowTextInHeader = false,
                HeaderImageKey = "lock",

                IsEditable = true,
                CheckBoxes = true,
                TextAlign = HorizontalAlignment.Center,
            };

            columnID = new OLVColumn()
            {
                DisplayIndex = displayIndex++,
                Name = "idColumn",
                AspectName = "ID",
                MaximumWidth = 50,
                MinimumWidth = 50,
                Width = 50,
                Searchable = false,
                Groupable = false,
                Sortable = false,

                TextAlign = HorizontalAlignment.Left,
                IsEditable = false,
            };

            columnName = new OLVColumn()
            {
                DisplayIndex = displayIndex++,
                Name = "nameColumn",
                AspectName = "Name",
                MaximumWidth = -1,
                MinimumWidth = 100,
                Width = 100,
                Searchable = false,
                Groupable = false,
                Sortable = false,

                IsEditable = true,
                FillsFreeSpace = true,
                CellEditUseWholeCell = true,
            };

            columnSC = new OLVColumn()
            {
                DisplayIndex = displayIndex++,
                Text = "SC",
                Name = "scColumn",
                AspectName = "SC",
                MaximumWidth = 30,
                MinimumWidth = 30,
                Width = 30,
                Searchable = false,
                Groupable = false,
                Sortable = false,

                IsEditable = true,
                CheckBoxes = true,
                TextAlign = HorizontalAlignment.Center,
            };

            columnHC = new OLVColumn()
            {
                DisplayIndex = displayIndex++,
                Text = "HC",
                Name = "hcColumn",
                AspectName = "HC",
                MaximumWidth = 30,
                MinimumWidth = 30,
                Width = 30,
                Searchable = false,
                Groupable = false,
                Sortable = false,

                IsEditable = true,
                CheckBoxes = true,
                TextAlign = HorizontalAlignment.Center,
            };

            columnUsage = new OLVColumn()
            {
                DisplayIndex = displayIndex++,
                Name = "usageColumn",
                MaximumWidth = 45,
                MinimumWidth = 45,
                Width = 45,
                Searchable = false,
                Groupable = false,
                Sortable = false,

                IsEditable = false,
                AspectGetter = delegate (object row) {
                    StashObject stash = (StashObject)row;
                    return stash.TransferFileLoaded ? stash.UsageText : "???";
                },
                TextAlign = HorizontalAlignment.Right,
            };

            columnLastChange = new OLVColumn()
            {
                DisplayIndex = displayIndex++,
                Name = "lastChangeColumn",
                MaximumWidth = 130,
                MinimumWidth = 130,
                Width = 130,
                Searchable = false,
                Groupable = false,
                Sortable = false,

                IsEditable = false,
                AspectGetter = delegate (object row) {
                    StashObject stash = (StashObject)row;
                    return stash.TransferFileLoaded ? stash.LastWriteTime.ToString() : " File Not Found";
                },
                TextAlign = HorizontalAlignment.Right,
            };

            columnActive = new OLVColumn()
            {
                DisplayIndex = displayIndex++,
                Text = "",
                Name = "activeColumn",
                MaximumWidth = 30,
                MinimumWidth = 30,
                Width = 30,
                Searchable = false,
                Groupable = false,
                Sortable = false,

                IsEditable = false,
                AspectGetter = delegate (object row) {
                    StashObject stash = (StashObject)row;
                    if (stash.ID == Global.Runtime.ActiveStashID) return ">>>";
                    return "";
                },
                TextAlign = HorizontalAlignment.Center,
            };

            columnExpansion = new OLVColumn()
            {
                DisplayIndex = displayIndex++,
                Text = "Exp",
                Name = "expansionColumn",
                MaximumWidth = 50,
                MinimumWidth = 50,
                Width = 50,
                Searchable = false,
                Groupable = true,
                Sortable = false,

                IsEditable = false,
                AspectGetter = delegate (object row) {
                    StashObject stash = (StashObject)row;
                    switch((int)stash.Expansion)
                    {
                        case 0: return "GD";
                        case 1: return "AoM";
                        case 2: return "FG";
                    }
                    return "???";
                },
                TextAlign = HorizontalAlignment.Right,
            };

            columnColor = new OLVColumn()
            {
                DisplayIndex = displayIndex++,
                Name = "colorColumn",
                AspectName = "Color",
                MaximumWidth = 70,
                MinimumWidth = 70,
                Width = 70,
                Searchable = false,
                Groupable = false,
                Sortable = false,

                IsEditable = true,
                CellEditUseWholeCell = true,
            };


        }


        public override void Initialize()
        {
            base.Initialize();



            Width = Global.Configuration.Settings.WindowWidth;
            Height = Global.Configuration.Settings.WindowHeight;
            ResizeEnd += delegate {
                Global.Configuration.Settings.WindowWidth = Width;
                Global.Configuration.Settings.WindowHeight = Height;
                Global.Configuration.Save();
                UpdateDisplayMenu();
            };
            FormWindowState LastWindowState = WindowState;
            Resize += delegate {
                if (LastWindowState == WindowState) return;
                LastWindowState = WindowState;
                UpdateDisplayMenu();
            };





            stashesListView.MultiSelect = true;

            stashesListView.PrimarySortColumn = columnOrder;
            stashesListView.PrimarySortOrder = SortOrder.Ascending;

            stashesListView.CustomSorter = delegate (OLVColumn column, SortOrder order) {
                stashesListView.ListViewItemSorter = new StashesSortComparer();
            };
            
            _expansionNames = new Dictionary<int, string>() {
                { -1, "" },
                { 0, GrimDawnLib.GrimDawn.GetExpansionName(0) },
                { 1, GrimDawnLib.GrimDawn.GetExpansionName(1) },
                { 2, GrimDawnLib.GrimDawn.GetExpansionName(2) },
            };
            showExpansionComboBox.DisplayMember = "Value";
            showExpansionComboBox.ValueMember = "Key";
            showExpansionComboBox.DataSource = new BindingSource(_expansionNames, null);
            showExpansionComboBox.SelectedValue = Global.Configuration.Settings.ShowExpansion;
            showSoftCoreComboBox.Checked = Global.Configuration.Settings.ShowSoftcore;
            showHardCoreComboBox.Checked = Global.Configuration.Settings.ShowHardcore;
            showExpansionComboBox.SelectionChangeCommitted += delegate {
                Global.Configuration.Settings.ShowExpansion = (int)showExpansionComboBox.SelectedValue;
                Global.Configuration.Save();
                UpdateObjects();
            };
            showSoftCoreComboBox.CheckedChanged += delegate {
                Global.Configuration.Settings.ShowSoftcore = showSoftCoreComboBox.Checked;
                Global.Configuration.Save();
                UpdateObjects();
            };
            showHardCoreComboBox.CheckedChanged += delegate {
                Global.Configuration.Settings.ShowHardcore = showHardCoreComboBox.Checked;
                Global.Configuration.Save();
                UpdateObjects();
            };











            Global.Runtime.ActiveStashChanged += delegate (object sender, GlobalHandlers.RuntimeHandler.ActiveStashChangedEventArgs e)
            {
                //UpdateObjects(); //we dont need to use it
                stashesListView.Invoke((MethodInvoker)delegate {
                    foreach (OLVListItem item in stashesListView.Items)
                    {
                        StashObject stash = (StashObject)item.RowObject;
                        if (stash.ID == e.OldID || stash.ID == e.NewID)
                        {
                            stashesListView.RefreshItem(item);
                        }
                    }
                });
            };

            _dragHandler = new StashesDragHandler(stashesListView);
            _dragHandler.DragSource.DragEnd += delegate {
                UpdateObjects();
                Global.Configuration.Save();
                Global.Runtime.NotifyStashesOrderChanged();
            };

            stashesListView.UseCellFormatEvents = true;

            stashesListView.SelectedIndexChanged += delegate {
                bool allSelected = true;
                foreach (OLVListItem item in stashesListView.Items)
                    if (!item.Selected) allSelected = false;
                selectAllButton.Text = allSelected ? _unselect_all : _select_all;
            };

            stashesListView.FormatCell += delegate (object sender, FormatCellEventArgs e) {

                if (e.Column != columnColor
                    && e.Column != columnName
                    && e.Column != columnID)
                    return;

                StashObject stash = (StashObject)e.Model;
                bool isMain = Global.Configuration.IsMainStashID(stash.ID);
                bool isActive = Global.Runtime.ActiveStashID == stash.ID;

                if (e.Column == columnColor)
                {
                    e.SubItem.BackColor = Color.Black;
                    e.SubItem.ForeColor = stash.GetDisplayColor();
                    e.SubItem.Font = new Font("Consolas", 9, FontStyle.Bold);
                }
                if (e.Column == columnName || e.Column == columnID)
                {
                    FontStyle fs = FontStyle.Regular;
                    if (isMain) fs |= FontStyle.Italic;
                    if (isActive) fs |= FontStyle.Bold;
                    e.SubItem.Font = new Font(e.Item.Font, fs);
                }
            };

            Bitmap mainCheckBoxOverlay = new Bitmap(13, 13, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (Graphics gfx = Graphics.FromImage(mainCheckBoxOverlay))
            using (SolidBrush brush = new SolidBrush(Color.FromArgb(255, 255, 255)))
            {
                gfx.FillRectangle(brush, 0, 0, 13, 13);
            }
            ImageDecoration mainCheckBoxDeco = new ImageDecoration(mainCheckBoxOverlay, ContentAlignment.MiddleCenter)
            {
                Transparency = 180,
                Offset = new Size(-1, 0),
            };
            ImageDecoration lockDecoration = new ImageDecoration(new Bitmap(Properties.Resources.lockBlack, new Size(15, 15)), ContentAlignment.MiddleRight)
            {
                Transparency = 180,
                Offset = new Size(-5, 0),
            };

            


            stashesListView.FormatRow += delegate (object sender, FormatRowEventArgs e) {

                StashObject stash = (StashObject)e.Model;
                bool isMain = Global.Configuration.IsMainStashID(stash.ID);
                bool isDragging = _dragHandler.IsDragging(stash);
                bool isSelected = e.Item.Selected;

                if (isDragging)
                {
                    e.Item.BackColor = Color.Teal;
                    e.Item.ForeColor = Color.White;
                }
                else
                {
                    if (stash.TransferFileLoaded)
                    {
                        if (isMain)
                        {
                            //e.Item.ForeColor = Color.Gray;
                        }
                    }
                    else
                    {
                        e.Item.ForeColor = Color.Red;
                    }
                    OLVListSubItem subItem;
                    if (isMain)
                    {
                        subItem = e.Item.GetSubItem(columnLocked.Index);
                        if (subItem != null) subItem.Decoration = mainCheckBoxDeco;
                        subItem = e.Item.GetSubItem(columnSC.Index);
                        if (subItem != null) subItem.Decoration = mainCheckBoxDeco;
                        subItem = e.Item.GetSubItem(columnHC.Index);
                        if (subItem != null) subItem.Decoration = mainCheckBoxDeco;
                    }
                    if (stash.Locked)
                    {
                        subItem = e.Item.GetSubItem(columnName.Index);
                        if (subItem != null) subItem.Decoration = lockDecoration;
                    }
                }

            };

            AllowDrop = true;
            DragEnter += TransferFile_DragEnter;
            DragDrop += TransferFile_DragDrop;
            stashesListView.DragDrop += TransferFile_DragDrop;

            UpdateColumns();
            UpdateObjects();








            stashesListView.ShowGroups = false;
            if (stashesListView.ShowGroups)
            {
                stashesListView.HasCollapsibleGroups = true;
                stashesListView.AlwaysGroupByColumn = columnExpansion;
                columnExpansion.GroupKeyGetter = delegate (object rowObject) {
                    StashObject stash = (StashObject)rowObject;
                    return GrimDawnLib.GrimDawn.GetExpansionName(stash.Expansion);
                };
                stashesListView.BuildGroups(columnExpansion, SortOrder.None);
            }
            editCategoriesButton.Visible = false;
            
        }


        private void TransferFile_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void TransferFile_DragDrop(object sender, DragEventArgs e)
        {
            if (_dragHandler.IsDragging()) return;
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            Global.Windows.ShowImportDialog(files);
        }







        private void UpdateColumns()
        {
            stashesListView.Columns.Clear();
            List<ColumnHeader> h = new List<ColumnHeader>();
            h.Add(columnActive);
            if (Global.Configuration.Settings.ShowLockedColumn) h.Add(columnLocked);
            if (Global.Configuration.Settings.ShowIDColumn) h.Add(columnID);
            h.Add(columnName);
            if (Global.Configuration.Settings.ShowColorColumn) h.Add(columnColor);
            h.Add(columnUsage);
            if (Global.Configuration.Settings.ShowLastChangeColumn) h.Add(columnLastChange);
            if (Global.Configuration.Settings.ShowExpansionColumn) h.Add(columnExpansion);
            h.Add(columnSC);
            h.Add(columnHC);
            stashesListView.Columns.AddRange(h.ToArray());
            //UpdateObjects();
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            UpdateObjects();
            UpdateDisplayMenu();
        }

        private string _msg_gd_already_running;
        private string _msg_confirm_delete_stashes;
        private string _msg_cannot_delete_stash;
        private string _msg_stash_is_active;
        private string _msg_stash_is_main;

        private string _select_all;
        private string _unselect_all;

        private string _color_default;
        private string _color_green;
        private string _color_blue;
        private string _color_purple;
        private string _color_gold;
        private string _color_gray;
        private string _color_white;
        private string _color_rose;

        protected override void Localize(GlobalHandlers.LocalizationHandler.StringsProxy L)
        {
            Text = L["mainWindow"];

            // top menu
            fileButton.Text = L["mainWindow_fileButton"];
            importStashesButton.Text = L["mainWindow_importStashesButton"];
            settingsButton.Text = L["mainWindow_settingsButton"];
            helpButton.Text = L["mainWindow_helpButton"];
            changelogButton.Text = L["mainWindow_changelogButton"];
            aboutButton.Text = L["mainWindow_aboutButton"];
            startGameButton.Text = L["mainWindow_startGameButton"];
            selectAllButton.Text = L["mainWindow_selectAllButton"];

            // bottom menu
            createStashButton.Text = L["mainWindow_createStashButton"];
            editCategoriesButton.Text = L["mainWindow_editCategoriesButton"];

            // columns
            columnLocked.Text = L["mainWindow_columnLocked"];
            columnID.Text = L["mainWindow_columnID"];
            columnName.Text = L["mainWindow_columnName"];
            columnUsage.Text = L["mainWindow_columnUsage"];
            columnLastChange.Text = L["mainWindow_columnLastChange"];
            columnColor.Text = L["mainWindow_columnColor"];
            columnExpansion.Text = L["mainWindow_columnExpansion"];
            columnSC.Text = L["mainWindow_columnSC"];
            columnHC.Text = L["mainWindow_columnHC"];

            // strings

            _msg_gd_already_running = L["msg_gd_already_running"];
            _msg_confirm_delete_stashes = L["msg_confirm_delete_stashes"];
            _msg_cannot_delete_stash = L["msg_cannot_delete_stash"];
            _msg_stash_is_active = L["msg_stash_is_active"];
            _msg_stash_is_main = L["msg_stash_is_main"];

            _select_all = L["mainWindow_selectAllButton"];
            _unselect_all = L["mainWindow_unselectAllButton"];

            _color_default = L["color_default"];
            _color_green = L["color_green"];
            _color_blue = L["color_blue"];
            _color_purple = L["color_purple"];
            _color_gold = L["color_gold"];
            _color_gray = L["color_gray"];
            _color_white = L["color_white"];
            _color_rose = L["color_rose"];

            _expansionNames[-1] = L["mainWindow_expansionNames_all"];
            showExpansionComboBox.DataSource = new BindingSource(_expansionNames, null);
            showExpansionComboBox.SelectedValue = Global.Configuration.Settings.ShowExpansion;
        }

        public void UpdateObjects()
        {
            if (!Visible) return;

            
            int scrollY = stashesListView.TopItemIndex;
            stashesListView.ClearObjects();
            stashesListView.SetObjects(Global.Stashes.GetShownStashes(
                Global.Configuration.Settings.ShowExpansion,
                Global.Configuration.Settings.ShowSoftcore,
                Global.Configuration.Settings.ShowHardcore
                ));
            stashesListView.Sort();
            stashesListView.TopItemIndex = scrollY;

            UpdateDisplayMenu();
        }

        private void UpdateDisplayMenu()
        {
            int wndStyle = Native.GetWindowLong(stashesListView.Handle, Native.GWL_STYLE);
            bool vsVisible = (wndStyle & Native.WS_VSCROLL) != 0;
            displayMenuPanel.Left = (Width - 650) + 354 + (vsVisible ? -17 : 0);
        }


        private bool ShowStashDeleteWarning()
        {
            return MessageBox.Show(_msg_confirm_delete_stashes, "", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK;
        }

        public StashObject[] GetSelectedObjects()
        {
            List<StashObject> objects = new List<StashObject>();
            foreach (OLVListItem item in stashesListView.SelectedItems)
            {
                objects.Add((StashObject)item.RowObject);
            }
            return objects.ToArray();
        }

        public StashObject[] GetDisabledObjects()
        {
            List<StashObject> objects = new List<StashObject>();
            foreach (OLVListItem item in stashesListView.Items)
            {
                if (!item.Enabled)
                    objects.Add((StashObject)item.RowObject);
            }
            return objects.ToArray();
        }

        public StashObject[] GetEnabledObjects()
        {
            List<StashObject> objects = new List<StashObject>();
            foreach (OLVListItem item in stashesListView.Items)
            {
                if (item.Enabled)
                    objects.Add((StashObject)item.RowObject);
            }
            return objects.ToArray();
        }




        #region StashListView Events

        private void StashesListView_ColumnRightClick(object sender, ColumnClickEventArgs e)
        {
            if (!Native.GetCursorPos(out Native.Point p)) return;
            ContextMenuStrip menu = new ContextMenuStrip() {
                Width = 200,
            };
            ToolStripCheckedListBox checkedList = new ToolStripCheckedListBox() { 
            };
            checkedList.AddItem(columnLocked.Text, Global.Configuration.Settings.ShowLockedColumn);
            checkedList.AddItem(columnID.Text, Global.Configuration.Settings.ShowIDColumn);
            checkedList.AddItem(columnColor.Text, Global.Configuration.Settings.ShowColorColumn);
            checkedList.AddItem(columnLastChange.Text, Global.Configuration.Settings.ShowLastChangeColumn);
            checkedList.AddItem(columnExpansion.Text, Global.Configuration.Settings.ShowExpansionColumn);
            checkedList.ItemCheck += delegate (object s, ItemCheckEventArgs f) {
                bool chckd = f.NewValue == CheckState.Checked;
                switch (f.Index)
                {
                    case 0: Global.Configuration.Settings.ShowLockedColumn = chckd; break;
                    case 1: Global.Configuration.Settings.ShowIDColumn = chckd; break;
                    case 2: Global.Configuration.Settings.ShowColorColumn = chckd; break;
                    case 3: Global.Configuration.Settings.ShowLastChangeColumn = chckd; break;
                    case 4: Global.Configuration.Settings.ShowExpansionColumn = chckd; break;
                }
                UpdateColumns();
                UpdateObjects();
                Global.Configuration.Save();
            };

            menu.Items.Insert(menu.Items.Count, checkedList);



            

            menu.Show(p.x, p.y);
        }

        private void StashesListView_CellRightClick(object sender, CellRightClickEventArgs args)
        {
            if (args.Model == null) return; // clicked in empty content

            StashObject stash = (StashObject)args.Model;
            ContextMenuStrip menu = new ContextMenuStrip();

            StashObject[] selectedStashes = GetSelectedObjects();

            if (selectedStashes.Length == 1)
            {
                menu.Items.Insert(0, new ToolStripLabel("#" + stash.ID + " " + stash.Name)
                {
                    ForeColor = Color.Gray,
                });
                menu.Items.Insert(1, new ToolStripLabel(stash.LastWriteTime.ToString())
                {
                    ForeColor = Color.Gray,
                });
            }
            else
            {
                menu.Items.Insert(0, new ToolStripLabel(string.Format("({0})", selectedStashes.Length))
                {
                    ForeColor = Color.Gray,
                });
            }

            menu.Items.Add(new ToolStripSeparator());

            {
                Dictionary<string, string> colorList = new Dictionary<string, string>();
                colorList.Add("#ebdec3", _color_default);
                colorList.Add("#34eb58", _color_green);
                colorList.Add("#5ecfff", _color_blue);
                colorList.Add("#af69ff", _color_purple);
                colorList.Add("#ffcc00", _color_gold);
                colorList.Add("#aaaaaa", _color_gray);
                colorList.Add("#f0f0f0", _color_white);
                colorList.Add("#f765ad", _color_rose);

                menu.Items.Add(Global.L["mainWindow_context_Color"]);
                ToolStripMenuItem btn = menu.Items[menu.Items.Count - 1] as ToolStripMenuItem;
                foreach(KeyValuePair<string,string> kvp in colorList)
                {
                    ToolStripMenuItem mi = new ToolStripMenuItem(kvp.Value, null, delegate (object s, EventArgs e)
                    {
                        foreach (StashObject st in selectedStashes)
                        {
                            st.Color = kvp.Key;
                        }
                        Global.Configuration.Save();
                        UpdateObjects();
                    });
                    mi.BackColor = Color.FromArgb(0,0,0);
                    Color cFore;
                    try
                    {
                        cFore = ColorTranslator.FromHtml(kvp.Key);
                    }
                    catch (Exception)
                    {
                        cFore = Color.FromArgb(255, 235, 222, 195);
                    }
                    mi.ForeColor = cFore;
                    mi.MouseEnter += delegate { mi.ForeColor = Color.Black; };
                    mi.MouseLeave += delegate { mi.ForeColor = cFore; };
                    btn.DropDownItems.Add(mi);
                }
                if (btn.DropDownItems.Count == 0)
                {
                    //TODO?
                }
            }

            menu.Items.Add(new ToolStripSeparator());

            if (selectedStashes.Length == 1)
            {
                menu.Items.Add(Global.L["mainWindow_context_restorBackup"]);
                ToolStripMenuItem restoreButtn = menu.Items[menu.Items.Count - 1] as ToolStripMenuItem;
                restoreButtn.DropDownItems.AddRange(Array.ConvertAll<string, ToolStripItem>(Global.Stashes.GetBackupFiles(stash.ID), delegate (string file) {
                    string fileName = System.IO.Path.GetFileName(file);
                    string fileDate = System.IO.File.GetLastWriteTime(file).ToString();
                    TransferFile transferFile = Common.TransferFile.FromFile(file);
                    transferFile.LoadUsage();
                    string itemText = string.Format("{0} - {1} - {2}", fileName, fileDate, transferFile.UsageText);
                    return new ToolStripMenuItem(itemText, null, delegate (object s, EventArgs e) {
                        Global.Stashes.RestoreTransferFile(stash.ID, file);
                        Global.Runtime.ReloadOpenedStash(stash.ID);
                        stash.LoadTransferFile();
                        UpdateObjects(); // because the cell is not updated correctly
                        Global.Runtime.NotifyStashesRestored(stash);
                    });
                }));
                if (restoreButtn.DropDownItems.Count == 0)
                {
                    restoreButtn.DropDownItems.Insert(0, new ToolStripMenuItem(Global.L["mainWindow_context_noBackups"]) {
                        ForeColor = Color.Gray
                    });
                }
            }

            menu.Items.Add(Global.L["mainWindow_context_exportStashes"], null, delegate (object s, EventArgs e) {

                ExportZipFile zipFile = new ExportZipFile();
                foreach (StashObject selStash in selectedStashes) zipFile.AddStash(selStash);

                using (var dialog = new SaveFileDialog()
                {
                    Filter = "Zip Archive|*.zip",
                    FileName = "TransferFiles.zip",
                })
                {
                    DialogResult result = dialog.ShowDialog();
                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.FileName))
                    {
                        zipFile.SaveTo(dialog.FileName);
                    }
                }

            });

            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(Global.L["mainWindow_context_deleteStashes"], null, delegate (object s, EventArgs e) {
                if (Global.Configuration.Settings.ConfirmStashDelete && !ShowStashDeleteWarning()) return;

                List<StashObject> deletedStashes = new List<StashObject>();
                foreach (StashObject stash2delete in selectedStashes)
                {
                    if (Global.Configuration.IsMainStashID(stash2delete.ID))
                    {
                        MessageBox.Show(string.Format(_msg_cannot_delete_stash, stash2delete.Name, _msg_stash_is_main), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }
                    if (Global.Configuration.IsCurrentStashID(stash2delete.ID))
                    {
                        MessageBox.Show(string.Format(_msg_cannot_delete_stash, stash2delete.Name, _msg_stash_is_active), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }
                    Global.Stashes.DeleteStash(stash2delete.ID);
                    deletedStashes.Add(stash2delete);
                }
                Global.Configuration.Save();
                Global.Runtime.NotifyStashesRemoved(deletedStashes);
                UpdateObjects();
            });
            
            args.MenuStrip = menu;
        }

        private void StashesListView_SubItemChecking(object sender, SubItemCheckingEventArgs args)
        {
            StashObject stash = (StashObject)args.RowObject;
            if (Global.Configuration.IsMainStashID(stash.ID))
            {
                args.Canceled = true;
                return;
            }
            switch (args.Column.AspectName.ToLower())
            {
                case "sc":
                    stash.SC = args.NewValue == CheckState.Checked;
                    break;
                case "hc":
                    stash.HC = args.NewValue == CheckState.Checked;
                    break;
                case "locked":
                    stash.Locked = args.NewValue == CheckState.Checked;
                    break;
            }
            Global.Configuration.Save();
            //Global.Runtime.NotifyStashUpdated(stash);
            Global.Runtime.NotifyStashesOrderChanged();
        }

        private void StashesListView_CellEditStarting(object sender, CellEditEventArgs e)
        {
            if (e.Column == columnColor)
            {
                StashObject stash = (StashObject)e.RowObject;

                Controls.ExComboBox cb = new Controls.ExComboBox();
                cb.Bounds = e.CellBounds;
                cb.Font = new Font("Consolas", 9, FontStyle.Bold);
                cb.DropDownStyle = ComboBoxStyle.DropDown;
                cb.SelectionChangeCommitted += delegate {
                    stashesListView.FinishCellEdit();
                };

                Dictionary<string, string> colorList = new Dictionary<string, string>();
                colorList.Add("#ebdec3", _color_default);
                colorList.Add("#34eb58", _color_green);
                colorList.Add("#5ecfff", _color_blue);
                colorList.Add("#af69ff", _color_purple);
                colorList.Add("#ffcc00", _color_gold);
                colorList.Add("#aaaaaa", _color_gray);
                colorList.Add("#f0f0f0", _color_white);
                colorList.Add("#f765ad", _color_rose);

                foreach (KeyValuePair<string, string> kvp in colorList)
                {
                    Controls.ExComboBoxItem cbItem = new Controls.ExComboBoxItem(kvp.Key, kvp.Value);
                    cbItem.Backcolor = Color.FromArgb(0, 0, 0);
                    try
                    {
                        cbItem.Forecolor = ColorTranslator.FromHtml(kvp.Key);
                    }
                    catch (Exception)
                    {
                        cbItem.Forecolor = Color.FromArgb(255, 235, 222, 195);
                    }
                    cb.Items.Add(cbItem);
                }

                e.Control = cb;
                new System.Threading.Thread(() => {
                    System.Threading.Thread.Sleep(1);
                    cb.Invoke((MethodInvoker)delegate {
                        cb.Text = stash.Color.ToLower();
                        cb.SelectionStart = 0;
                        cb.SelectionLength = cb.Text.Length;
                    });
                }).Start();
            }
        }

        private void StashesListView_CellEditFinishing(object sender, CellEditEventArgs e)
        {
            if (e.Column == columnColor)
            {
                Controls.ExComboBox cb = (Controls.ExComboBox)e.Control;
                if (cb.SelectedIndex != -1)
                {
                    Controls.ExComboBoxItem item = (Controls.ExComboBoxItem)cb.SelectedItem;
                    e.NewValue = item.Value;
                }
            }
        }

        private void StashesListView_CellEditFinished(object sender, CellEditEventArgs args)
        {
            StashObject stash = (StashObject)args.RowObject;
            if (args.Column == columnName || args.Column == columnColor || args.Column == columnLocked)
            {
                Global.Runtime.NotifyStashUpdated(stash);
            }
            else
            {
                return;
            }
            Global.Configuration.Save();
        }

        #endregion

        #region Events

        private void SelectAllButton_Click(object sender, EventArgs e)
        {
            stashesListView.Focus();
            bool allSelected = true;
            foreach (OLVListItem item in stashesListView.Items)
                if (!item.Selected) allSelected = false;
            foreach (OLVListItem item in stashesListView.Items)
                item.Selected = !allSelected;


            //stashesListView.EnsureModelVisible(((OLVListItem)stashesListView.Items[10]).RowObject);


        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            Global.Windows.ShowConfigurationWindow(false);
        }

        private void AboutButton_Click(object sender, EventArgs e)
        {
            Global.Windows.ShowAboutDialog();
        }

        private void CreateStashButton_Click(object sender, EventArgs e)
        {
            Global.Windows.ShowCreateStashDialog();
        }

        private void EditCategoriesButton_Click(object sender, EventArgs e)
        {
            Global.Windows.ShowCategoriesWindow();
        }

        private void ChangelogButton_Click(object sender, EventArgs e)
        {
            Global.Windows.ShowChangelogWindow();
        }

        private void ImportStashesButton_Click(object sender, EventArgs e)
        {
            Global.Windows.ShowImportDialog();
        }

        private void StartGameButton_Click(object sender, EventArgs e)
        {
            switch (Global.Runtime.StartGame())
            {
                case GlobalHandlers.RuntimeHandler.GameStartResult.AlreadyRunning:
                    MessageBox.Show(_msg_gd_already_running, "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case GlobalHandlers.RuntimeHandler.GameStartResult.Success:
                    break;
            }
        }

        #endregion

    }
}
