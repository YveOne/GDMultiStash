using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using BrightIdeasSoftware;

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

        private readonly Dictionary<int, string> expansionNames;
        private readonly StashesDragHandler _dragHandler;

        public MainForm()
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


            columnID = new OLVColumn()
            {
                DisplayIndex = 1,
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
            
            columnOrder = new OLVColumn()
            {
                DisplayIndex = 0,
                Text = " ",
                Name = "orderColumn",
                AspectName = "Order",
                MaximumWidth = 30,
                MinimumWidth = 30,
                Width = 30,
                Searchable = false,
                Groupable = false,
                Sortable = true,

                TextAlign = HorizontalAlignment.Right,
            };
            
            columnName = new OLVColumn()
            {
                DisplayIndex = 2,
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
                DisplayIndex = 3,
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
                DisplayIndex = 4,
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
                DisplayIndex = 5,
                Name = "usageColumn",
                MaximumWidth = 45,
                MinimumWidth = 45,
                Width = 45,
                Searchable = false,
                Groupable = false,
                Sortable = false,

                IsEditable = false,
                AspectGetter = delegate (object row) {
                    Common.Stash stash = (Common.Stash)row;
                    return stash.TransferFileExists() ? stash.UsageText : "???";
                },
                TextAlign = HorizontalAlignment.Right,
            };

            columnLastChange = new OLVColumn()
            {
                DisplayIndex = 6,
                Name = "lastChangeColumn",
                MaximumWidth = 130,
                MinimumWidth = 130,
                Width = 130,
                Searchable = false,
                Groupable = false,
                Sortable = false,

                IsEditable = false,
                AspectGetter = delegate (object row) {
                    Common.Stash stash = (Common.Stash)row;
                    return stash.TransferFileExists() ? stash.LastWriteTime.ToString() : " File Not Found";
                },
                TextAlign = HorizontalAlignment.Right,
            };

            columnActive = new OLVColumn()
            {
                DisplayIndex = 7,
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
                    Common.Stash stash = (Common.Stash)row;
                    if (stash.ID == Core.Runtime.ActiveStashID) return ">>>";
                    return "";
                },
                TextAlign = HorizontalAlignment.Center,
            };

            columnExpansion = new OLVColumn()
            {
                DisplayIndex = 8,
                Text = "Exp",
                Name = "expansionColumn",
                MaximumWidth = 50,
                MinimumWidth = 50,
                Width = 50,
                Searchable = false,
                Groupable = false,
                Sortable = false,

                IsEditable = false,
                AspectGetter = delegate (object row) {
                    Common.Stash stash = (Common.Stash)row;
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
                DisplayIndex = 9,
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

            //stashesListView.SelectColumnsOnRightClick = true;
            //stashesListView.SelectColumnsOnRightClickBehaviour = ObjectListView.ColumnSelectBehaviour.None;

            stashesListView.MultiSelect = true;
            stashesListView.ShowGroups = false;

            stashesListView.PrimarySortColumn = columnOrder;
            stashesListView.PrimarySortOrder = SortOrder.Ascending;

            stashesListView.CustomSorter = delegate (OLVColumn column, SortOrder order) {
                stashesListView.ListViewItemSorter = new StashesSortComparer();
            };

            UpdateColumns();

            expansionNames = new Dictionary<int, string>() {
                { -1, "All" },
                { 0, GrimDawnLib.GrimDawn.GetExpansionName(0) },
                { 1, GrimDawnLib.GrimDawn.GetExpansionName(1) },
                { 2, GrimDawnLib.GrimDawn.GetExpansionName(2) },
            };
            showExpansionComboBox.DisplayMember = "Value";
            showExpansionComboBox.ValueMember = "Key";
            showExpansionComboBox.DataSource = new BindingSource(expansionNames, null);
            showExpansionComboBox.SelectedValue = Core.Config.ShowExpansion;
            showSoftCoreComboBox.Checked = Core.Config.ShowSoftcore;
            showHardCoreComboBox.Checked = Core.Config.ShowHardcore;
            showExpansionComboBox.SelectionChangeCommitted += delegate {
                Core.Config.ShowExpansion = (int)showExpansionComboBox.SelectedValue;
                Core.Config.Save();
                UpdateObjects();
            };
            showSoftCoreComboBox.CheckedChanged += delegate {
                Core.Config.ShowSoftcore = showSoftCoreComboBox.Checked;
                Core.Config.Save();
                UpdateObjects();
            };
            showHardCoreComboBox.CheckedChanged += delegate {
                Core.Config.ShowHardcore = showHardCoreComboBox.Checked;
                Core.Config.Save();
                UpdateObjects();
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




            Core.Runtime.ActiveStashChanged += delegate (object sender, Core.Runtime.ActiveStashChangedEventArgs e)
            {
                //UpdateObjects(); //we dont need to use it
                foreach (OLVListItem item in stashesListView.Items)
                {
                    Common.Stash stash = (Common.Stash)item.RowObject;
                    if (stash.ID == e.OldID || stash.ID == e.NewID)
                    {
                        stashesListView.RefreshItem(item);
                    }
                }
            };


            _dragHandler = new StashesDragHandler(stashesListView);
            _dragHandler.DragSource.DragEnd += delegate {
                UpdateObjects();
                Core.Config.Save();

                Core.Runtime.NotifyStashesOrderChanged();
            };

            stashesListView.UseCellFormatEvents = true;

            stashesListView.FormatCell += delegate (object sender, FormatCellEventArgs e)
            {
                if (e.Column != columnColor
                    && e.Column != columnName
                    && e.Column != columnID)
                    return;

                Common.Stash stash = (Common.Stash)e.Model;
                bool isMain = Core.Config.IsMainStashID(stash.ID);
                bool isActive = Core.Runtime.ActiveStashID == stash.ID;

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



            stashesListView.FormatRow += delegate (object sender, FormatRowEventArgs e) {
                Common.Stash stash = (Common.Stash)e.Model;
                bool isMain = Core.Config.IsMainStashID(stash.ID);
                bool isDragging = _dragHandler.IsDragging(stash);
                bool isSelected = e.Item.Selected;

                if (isDragging)
                {
                    e.Item.BackColor = Color.Teal;
                    e.Item.ForeColor = Color.White;
                }
                else
                {
                    if (stash.TransferFileExists())
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
                    if (isMain)
                    {
                        e.Item.GetSubItem(columnSC.Index).Decoration = mainCheckBoxDeco;
                        e.Item.GetSubItem(columnHC.Index).Decoration = mainCheckBoxDeco;
                    }
                }

            };

            Width = Core.Config.WindowWidth;
            Height = Core.Config.WindowHeight;
            ResizeEnd += delegate {
                Core.Config.WindowWidth = Width;
                Core.Config.WindowHeight = Height;
                Core.Config.Save();
            };

            AllowDrop = true;
            DragEnter += TransferFile_DragEnter;
            DragDrop += TransferFile_DragDrop;
            //stashesListView.AllowDrop = true;
            //stashesListView.DragEnter += TransferFile_DragEnter;
            stashesListView.DragDrop += TransferFile_DragDrop;

        }

        private void TransferFile_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void TransferFile_DragDrop(object sender, DragEventArgs e)
        {
            if (_dragHandler.IsDragging()) return;
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            Core.Windows.ShowImportDialog(files);
        }





        private void UpdateColumns()
        {
            stashesListView.Columns.Clear();
            List<ColumnHeader> h = new List<ColumnHeader>();
            h.Add(columnActive);
            if (Core.Config.ShowIDColumn) h.Add(columnID);
            h.Add(columnName);
            if (Core.Config.ShowColorColumn) h.Add(columnColor);
            h.Add(columnUsage);
            if (Core.Config.ShowLastChangeColumn) h.Add(columnLastChange);
            if (Core.Config.ShowExpansionColumn) h.Add(columnExpansion);
            h.Add(columnSC);
            h.Add(columnHC);
            stashesListView.Columns.AddRange(h.ToArray());
            UpdateObjects();
        }





        private void MainForm_Load(object sender, EventArgs e)
        {
            UpdateObjects();
        }

        private string _confirm_delete_stash;
        private string _err_cannot_delete_stash;
        private string _err_stash_is_active;
        private string _color_default;
        private string _color_green;
        private string _color_blue;
        private string _color_purple;
        private string _color_gold;
        private string _color_gray;

        protected override void Localize(Core.Localization.StringsProxy L)
        {
            Text = "GDMultiStash";

            fileButton.Text = L["button_file"];
            settingsButton.Text = L["button_settings"];
            aboutButton.Text = L["button_about"];
            helpButton.Text = L["button_help"];
            columnID.Text = L["column_id"];
            columnName.Text = L["column_name"];
            columnUsage.Text = L["column_usage"];
            columnLastChange.Text = L["column_last_change"];
            columnColor.Text = L["column_color"];
            columnExpansion.Text = L["column_expansion"];
            columnSC.Text = L["column_softcore"];
            columnHC.Text = L["column_hardcore"];

            createStashButton.Text = L["create_stash"];
            importStashesButton.Text = L["import_stashes"];

            selectAllButton.Text = L["button_select_all"];
            unselectAllButton.Text = L["button_unselect_all"];

            _confirm_delete_stash = L["confirm_delete_stash"];
            _err_cannot_delete_stash = L["err_cannot_delete_stash"];
            _err_stash_is_active = L["err_stash_is_active"];

            _color_default = L["color_default"];
            _color_green = L["color_green"];
            _color_blue = L["color_blue"];
            _color_purple = L["color_purple"];
            _color_gold = L["color_gold"];
            _color_gray = L["color_gray"];

            expansionNames[-1] = L["all"];
            showExpansionComboBox.DataSource = new BindingSource(expansionNames, null);
            showExpansionComboBox.SelectedValue = Core.Config.ShowExpansion;
        }

        public void UpdateObjects()
        {
            int scrollY = stashesListView.TopItemIndex;
            stashesListView.ClearObjects();
            stashesListView.SetObjects(Core.Stashes.GetShownStashes(
                Core.Config.ShowExpansion,
                Core.Config.ShowSoftcore,
                Core.Config.ShowHardcore
                ));
            stashesListView.Sort();
            stashesListView.TopItemIndex = scrollY;

            int wndStyle = Native.GetWindowLong(stashesListView.Handle, Native.GWL_STYLE);
            bool vsVisible = (wndStyle & Native.WS_VSCROLL) != 0;
            if (vsVisible)
            {
                if (!displayMenuPanelShiftLeft)
                {
                    displayMenuPanelShiftLeft = true;
                    displayMenuPanel.Left -= 17;
                }
            }
            else
            {
                if (displayMenuPanelShiftLeft)
                {
                    displayMenuPanelShiftLeft = false;
                    displayMenuPanel.Left += 17;
                }
            }
        }

        private bool displayMenuPanelShiftLeft = false;



        private bool ShowStashDeleteWarning()
        {
            return MessageBox.Show(_confirm_delete_stash, "", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK;
        }

        public Common.Stash[] GetSelectedObjects()
        {
            List<Common.Stash> objects = new List<Common.Stash>();
            foreach (OLVListItem item in stashesListView.SelectedItems)
            {
                objects.Add((Common.Stash)item.RowObject);
            }
            return objects.ToArray();
        }

        public Common.Stash[] GetDisabledObjects()
        {
            List<Common.Stash> objects = new List<Common.Stash>();
            foreach (OLVListItem item in stashesListView.Items)
            {
                if (!item.Enabled)
                    objects.Add((Common.Stash)item.RowObject);
            }
            return objects.ToArray();
        }

        public Common.Stash[] GetEnabledObjects()
        {
            List<Common.Stash> objects = new List<Common.Stash>();
            foreach (OLVListItem item in stashesListView.Items)
            {
                if (item.Enabled)
                    objects.Add((Common.Stash)item.RowObject);
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
            checkedList.AddItem(columnID.Text, Core.Config.ShowIDColumn);
            checkedList.AddItem(columnColor.Text, Core.Config.ShowColorColumn);
            checkedList.AddItem(columnLastChange.Text, Core.Config.ShowLastChangeColumn);
            checkedList.AddItem(columnExpansion.Text, Core.Config.ShowExpansionColumn);
            checkedList.ItemCheck += delegate (object s, ItemCheckEventArgs f) {
                bool chckd = f.NewValue == CheckState.Checked;
                switch (f.Index)
                {
                    case 0: Core.Config.ShowIDColumn = chckd; break;
                    case 1: Core.Config.ShowColorColumn = chckd; break;
                    case 2: Core.Config.ShowLastChangeColumn = chckd; break;
                    case 3: Core.Config.ShowExpansionColumn = chckd; break;
                }
                UpdateColumns();
                Core.Config.Save();
            };

            menu.Items.Insert(menu.Items.Count, checkedList);



            

            menu.Show(p.x, p.y);
        }

        private void StashesListView_CellRightClick(object sender, CellRightClickEventArgs args)
        {
            if (args.Model == null) return; // clicked in empty content

            Core.Localization.StringsProxy L = new Core.Localization.StringsProxy();

            Common.Stash stash = (Common.Stash)args.Model;
            ContextMenuStrip menu = new ContextMenuStrip();

            Common.Stash[] selectedStashes = GetSelectedObjects();

            /*
            if (selectedStashes.Length == 0)
            {
                // right clicked listviewitem is disabled
                selectedStashes = new Common.Stash[] { stash };
            }
            */

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

            if (selectedStashes.Length == 1)
            {
                menu.Items.Add(new ToolStripSeparator());
                menu.Items.Add(L["restore_backup"]);
                ToolStripMenuItem restoreButtn = menu.Items[menu.Items.Count - 1] as ToolStripMenuItem;
                restoreButtn.DropDownItems.AddRange(Array.ConvertAll<string, ToolStripItem>(Core.Stashes.GetBackupFiles(stash.ID), delegate (string file) {
                    string fileName = System.IO.Path.GetFileName(file);
                    string fileDate = System.IO.File.GetLastWriteTime(file).ToString();
                    string usageText = Common.TransferFile.FromFile(file).UsageText;
                    string itemText = string.Format("{0} - {1} - {2}", fileName, fileDate, usageText);
                    return new ToolStripMenuItem(itemText, null, delegate (object s, EventArgs e) {
                        Core.Runtime.ReloadOpenedStash(stash.ID);
                        Core.Stashes.RestoreTransferFile(stash.ID, file);
                        stash.LoadTransferFile();
                        UpdateObjects(); // because the cell is not updated correctly
                        Core.Runtime.NotifyStashesRestored(stash);
                    });
                }));
                if (restoreButtn.DropDownItems.Count == 0)
                {
                    restoreButtn.DropDownItems.Insert(0, new ToolStripMenuItem(L["no_backups"]) {
                        ForeColor = Color.Gray
                    });
                }
            }

            menu.Items.Add(L["context_export"], null, delegate (object s, EventArgs e) {


                Common.ExportZipFile zipFile = new Common.ExportZipFile();
                foreach (Common.Stash selStash in selectedStashes) zipFile.AddStash(selStash);

                using (var dialog = new SaveFileDialog()
                {
                    Filter = "Zip Archive ({0})|*.zip",
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
            menu.Items.Add(L["delete_stash"], null, delegate (object s, EventArgs e) {
                if (Core.Config.ConfirmStashDelete && !ShowStashDeleteWarning()) return;

                List<Common.Stash> deletedStashes = new List<Common.Stash>();
                foreach (Common.Stash stash2delete in selectedStashes)
                {
                    if (Core.Config.IsMainStashID(stash2delete.ID))
                    {
                        // TODO: show warning?
                        continue;
                    }

                    if (Core.Config.IsCurStashID(stash2delete.ID))
                    {
                        MessageBox.Show(string.Format(_err_cannot_delete_stash, stash2delete.Name, _err_stash_is_active), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }

                    Core.Stashes.DeleteStash(stash2delete.ID);
                    deletedStashes.Add(stash2delete);
                }
                Core.Config.Save();
                Core.Runtime.NotifyStashesRemoved(deletedStashes);
                UpdateObjects();
            });
            
            args.MenuStrip = menu;
        }

        private void StashesListView_SubItemChecking(object sender, SubItemCheckingEventArgs args)
        {
            Common.Stash stash = (Common.Stash)args.RowObject;
            if (Core.Config.IsMainStashID(stash.ID))
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
            }
            Core.Config.Save();
            Core.Runtime.NotifyStashesUpdated(stash);
        }

        private void StashesListView_CellEditStarting(object sender, CellEditEventArgs e)
        {
            if (e.Column == columnColor)
            {
                Common.Stash stash = (Common.Stash)e.RowObject;

                Dictionary<string, string> colorList = new Dictionary<string, string>();
                colorList.Add("#ebdec3", _color_default); // f2a04c


                colorList.Add("#34eb58", _color_green); // 0fe35a
                colorList.Add("#5ecfff", _color_blue); // 39aace
                colorList.Add("#af69ff", _color_purple); // a036f7
                colorList.Add("#ffcc00", _color_gold);
                colorList.Add("#aaaaaa", _color_gray);

                ComboBox cb = new ComboBox();
                cb.Bounds = e.CellBounds;
                cb.Font = ((ObjectListView)sender).Font;
                cb.DropDownStyle = ComboBoxStyle.DropDown;
                cb.DisplayMember = "Value";
                cb.ValueMember = "Key";
                cb.DataSource = new BindingSource(colorList, null);
                e.Control = cb;
                new System.Threading.Thread(() => {
                    System.Threading.Thread.Sleep(1);
                    cb.Invoke((MethodInvoker)delegate {
                        cb.Text = stash.Color.ToLower();
                        cb.SelectionStart = 0;
                        cb.SelectionLength = cb.Text.Length;
                    });
                }).Start();
                cb.SelectionChangeCommitted += delegate {
                    stashesListView.FinishCellEdit();
                };
            }
        }

        private void StashesListView_CellEditFinishing(object sender, CellEditEventArgs e)
        {
            if (e.Column == columnColor)
            {
                ComboBox cb = (ComboBox)e.Control;
                if (cb.SelectedIndex == -1)
                    e.NewValue = cb.Text;
            }
        }

        private void StashesListView_CellEditFinished(object sender, CellEditEventArgs args)
        {
            Common.Stash stash = (Common.Stash)args.RowObject;
            if (args.Column == columnName)
                Core.Runtime.NotifyStashesUpdated(stash);
            else if (args.Column == columnColor)
            {

                Core.Runtime.NotifyStashesUpdated(stash);
            }
            else
                return;
            Core.Config.Save();
        }

        #endregion

        #region Events

        private void SelectAllButton_Click(object sender, EventArgs e)
        {
            stashesListView.Focus();
            foreach (OLVListItem item in stashesListView.Items)
                item.Selected = true;
        }

        private void UnselectAllButton_Click(object sender, EventArgs e)
        {
            stashesListView.Focus();
            foreach (OLVListItem item in stashesListView.Items)
                item.Selected = false;
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            Core.Windows.ShowSetupDialog(false);
        }

        private void AboutButton_Click(object sender, EventArgs e)
        {
            Core.Windows.ShowAboutDialog();
        }

        private void ImportStashesButton_Click(object sender, EventArgs e)
        {
            Core.Windows.ShowImportDialog();
        }

        private void CreateStashButton_Click(object sender, EventArgs e)
        {
            Core.Windows.ShowCreateStashDialog();
        }




        #endregion

    }
}
