using System;
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
            stashesListView.SubItemChecking += StashesListView_SubItemChecking;
            stashesListView.CellEditFinished += StashesListView_CellEditFinished;

            columnID = new OLVColumn()
            {
                DisplayIndex = 1,
                Name = "idColumn",
                AspectName = "ID",
                MaximumWidth = 30,
                MinimumWidth = 30,
                Width = 30,
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
                MaximumWidth = 120,
                MinimumWidth = 120,
                Width = 120,
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
                MaximumWidth = 60,
                MinimumWidth = 60,
                Width = 60,
                Searchable = false,
                Groupable = false,
                Sortable = false,

                IsEditable = true,
                CellEditUseWholeCell = true,
            };

            // register columns
            stashesListView.AllColumns.AddRange(new List<OLVColumn>() {
                columnOrder,
                columnID,
                columnName,
                columnExpansion,
                columnSC,
                columnHC,
                columnUsage,
                columnLastChange,
                columnActive,
                columnExpansion,
                columnColor,
            });

            stashesListView.MultiSelect = true;
            stashesListView.ShowGroups = false;

            stashesListView.PrimarySortColumn = columnOrder;
            stashesListView.PrimarySortOrder = SortOrder.Ascending;

            stashesListView.CustomSorter = delegate (OLVColumn column, SortOrder order) {
                stashesListView.ListViewItemSorter = new StashesSortComparer();
            };

            stashesListView.Columns.Clear();
            stashesListView.Columns.AddRange(new ColumnHeader[] { columnActive, columnID, columnName, columnColor, columnUsage, columnLastChange, columnExpansion, columnSC, columnHC });

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

            _dragHandler = new StashesDragHandler(stashesListView);
            _dragHandler.DragSource.DragEnd += delegate {
                UpdateObjects();
                Core.Config.Save();

                Core.Runtime.NotifyStashesRearranged();
                stashesListView.Sort();
            };

            stashesListView.UseCellFormatEvents = true;

            stashesListView.FormatCell += delegate (object sender, FormatCellEventArgs e)
            {
                if (e.Column == columnColor)
                {
                    Common.Stash stash = (Common.Stash)e.Model;
                    e.SubItem.BackColor = Color.Black;
                    e.SubItem.ForeColor = stash.GetColor();
                    e.SubItem.Font = new Font("Consolas", 9, FontStyle.Bold);
                }
            };

            stashesListView.FormatRow += delegate (object sender, FormatRowEventArgs e) {
                Common.Stash stash = (Common.Stash)e.Model;
                bool isMain = Core.Config.IsMainStashID(stash.ID);
                bool isDragging = _dragHandler.DragSource.DraggingStashes.Contains(stash);
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
                            e.Item.ForeColor = Color.Gray;
                        }
                    }
                    else
                    {
                        e.Item.ForeColor = Color.Red;
                    }
                    if (isMain)
                    {
                        e.Item.GetSubItem(7).Decoration = mainCheckBoxDeco;
                        e.Item.GetSubItem(8).Decoration = mainCheckBoxDeco;
                    }
                }
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
            if (_dragHandler.IsDragging) return;
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            Core.Windows.ShowImportDialog(files);
        }









        private void MainForm_Load(object sender, EventArgs e)
        {
            UpdateObjects();
        }

        private string _confirm_delete_stash;
        private string _err_cannot_delete_stash;
        private string _err_stash_is_active;

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

            createStashButton.Text = L["create_stash"];
            importStashesButton.Text = L["import_stashes"];

            selectAllButton.Text = L["button_select_all"];
            unselectAllButton.Text = L["button_unselect_all"];

            _confirm_delete_stash = L["confirm_delete_stash"];
            _err_cannot_delete_stash = L["err_cannot_delete_stash"];
            _err_stash_is_active = L["err_stash_is_active"];

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
            //stashesListView.DisableObjects(Core.Stashes.GetMainStashes());
            stashesListView.Sort();
            stashesListView.TopItemIndex = scrollY;
        }




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

        private void StashesListView_CellRightClick(object sender, CellRightClickEventArgs args)
        {
            if (args.Model == null) return;

            Core.Localization.StringsProxy L = new Core.Localization.StringsProxy();

            Common.Stash stash = (Common.Stash)args.Model;
            ContextMenuStrip menu = new ContextMenuStrip();
            bool isMainClicked = false;

            Common.Stash[] selectedStashes = GetSelectedObjects();
            if (selectedStashes.Length == 0)
            {
                // right click on disabled (main) stash
                selectedStashes = new Common.Stash[] { stash };
                isMainClicked = true;
            }

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
                        if (Core.Runtime.IsStashOpened(stash.ID))
                        {
                            Core.Runtime.ReloadOpenedStash();
                        }
                        Core.Stashes.RestoreTransferFile(stash.ID, file);
                        stash.LoadTransferFile();
                        UpdateObjects(); // because the cell is not updated correctly
                        Core.Runtime.NotifyStashesRestored(new Common.Stash[] { stash });
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


                /*
                List<string> filters = new List<string>();
                if (stash.SC == stash.HC || stash.SC) filters.Add("Softcore ({0})|*{1}");
                if (stash.SC == stash.HC || stash.HC) filters.Add("Hardcore ({0})|*{2}");

                string filter = string.Format(string.Join("|", filters),
                    GrimDawnLib.GrimDawn.GetExpansionName(stash.Expansion),
                    GrimDawnLib.GrimDawn.GetTransferExtension(stash.Expansion, GrimDawnLib.GrimDawnGameMode.SC),
                    GrimDawnLib.GrimDawn.GetTransferExtension(stash.Expansion, GrimDawnLib.GrimDawnGameMode.HC)
                    );

                using (var dialog = new SaveFileDialog()
                {
                    Filter = string.Format(filter),
                    FileName = stash.Name,
                })
                {
                    DialogResult result = dialog.ShowDialog();
                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.FileName))
                    {
                        Core.Files.ExportTransferFile(stash.ID, dialog.FileName);
                    }
                }
                */
            });

            if (!isMainClicked)
            {
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
            }

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
            Core.Runtime.NotifyStashesModeChanged(new Common.Stash[] { stash });
        }

        private void StashesListView_CellEditFinished(object sender, CellEditEventArgs args)
        {
            Common.Stash stash = (Common.Stash)args.RowObject;
            Core.Config.Save();
            if (args.Column == columnName)
                Core.Runtime.NotifyStashesNameCHanged(new Common.Stash[] { stash });
            if (args.Column == columnColor)
                Core.Runtime.NotifyStashesColorChanged(new Common.Stash[] { stash });
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
