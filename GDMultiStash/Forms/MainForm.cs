using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using BrightIdeasSoftware;

namespace GDMultiStash.Forms
{
    internal partial class MainForm : BaseForm
    {

        private readonly OLVColumn columnID;
        private readonly OLVColumn columnOrder;
        private readonly OLVColumn columnOrderUp;
        private readonly OLVColumn columnOrderDown;
        private readonly OLVColumn columnSC;
        private readonly OLVColumn columnHC;
        private readonly OLVColumn columnName;
        private readonly OLVColumn columnLastChange;
        private readonly OLVColumn columnUsage;
        private readonly OLVColumn columnActive;
        private readonly OLVColumn columnExpansion;

        public MainForm()
        {
            InitializeComponent();
            Icon = Properties.Resources.icon32;
            Text = "GD MultiStash";

            stashesListView.ButtonClick += StashesListView_ButtonClick;
            stashesListView.ItemSelectionChanged += StashesListView_ItemSelectionChanged;
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

            columnOrderUp = new OLVColumn()
            {
                DisplayIndex = 2,
                Text = " ",
                Name = "orderUpColumn",
                MaximumWidth = 22,
                MinimumWidth = 22,
                Width = 22,
                Searchable = false,
                Groupable = false,
                Sortable = false,

                IsButton = true,
                IsEditable = false,
                ButtonSizing = OLVColumn.ButtonSizingMode.CellBounds,
                AspectGetter = delegate (Object row) { return "▲"; },
                TextAlign = HorizontalAlignment.Center,
            };

            columnOrderDown = new OLVColumn()
            {
                DisplayIndex = 3,
                Text = " ",
                Name = "orderDownColumn",
                MaximumWidth = 22,
                MinimumWidth = 22,
                Width = 22,
                Searchable = false,
                Groupable = false,
                Sortable = false,

                IsButton = true,
                IsEditable = false,
                ButtonSizing = OLVColumn.ButtonSizingMode.CellBounds,
                AspectGetter = delegate (Object row) { return "▼"; },
                TextAlign = HorizontalAlignment.Center,
            };

            columnName = new OLVColumn()
            {
                DisplayIndex = 4,
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
                DisplayIndex = 5,
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
                DisplayIndex = 6,
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
                DisplayIndex = 7,
                Name = "usageColumn",
                MaximumWidth = 45,
                MinimumWidth = 45,
                Width = 45,
                Searchable = false,
                Groupable = false,
                Sortable = false,

                IsEditable = false,
                AspectGetter = delegate (Object row) { return ((Common.Stash)row).UsageText; },
                TextAlign = HorizontalAlignment.Right,
            };

            columnLastChange = new OLVColumn()
            {
                DisplayIndex = 8,
                Name = "lastChangeColumn",
                MaximumWidth = 120,
                MinimumWidth = 120,
                Width = 120,
                Searchable = false,
                Groupable = false,
                Sortable = false,

                IsEditable = false,
                AspectGetter = delegate (object row) { return ((Common.Stash)row).LastWriteTime.ToString(); },
                TextAlign = HorizontalAlignment.Right,
            };

            columnActive = new OLVColumn()
            {
                DisplayIndex = 9,
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
                DisplayIndex = 10,
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

            // register columns
            stashesListView.AllColumns.AddRange(new List<OLVColumn>() {
                columnOrder,
                columnID,
                columnOrderUp,
                columnOrderDown,
                columnName,
                columnExpansion,
                columnSC,
                columnHC,
                columnUsage,
                columnLastChange,
                columnActive,
                columnExpansion,
            });

            stashesListView.MultiSelect = true;
            stashesListView.ShowGroups = false;

            stashesListView.PrimarySortColumn = columnOrder;
            stashesListView.PrimarySortOrder = SortOrder.Ascending;

            stashesListView.CustomSorter = delegate (OLVColumn column, SortOrder order) {
                stashesListView.ListViewItemSorter = new CustomComparer();
            };

            stashesListView.Columns.Clear();
            stashesListView.Columns.AddRange(new ColumnHeader[] { columnActive, columnOrderUp, columnOrderDown, columnID, columnName, columnUsage, columnLastChange, columnExpansion, columnSC, columnHC });

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

        }

        private Dictionary<int, string> expansionNames;

        class CustomComparer : IComparer
        {

            public CustomComparer()
            {
            }

            public int Compare(object x, object y)
            {
                Common.Stash s1 = ((OLVListItem)x).RowObject as Common.Stash;
                Common.Stash s2 = ((OLVListItem)y).RowObject as Common.Stash;

                bool s1m = Core.Stashes.IsMainStash(s1);
                bool s2m = Core.Stashes.IsMainStash(s2);
                if (s1m && s2m) return s1.Expansion.CompareTo(s2.Expansion);
                if (s1m) return -1;
                if (s2m) return+1;
                return s1.Order.CompareTo(s2.Order);

            }
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

            createStashButton.Text = L["create_stash"];
            importStashesButton.Text = L["import_stashes"];

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
            stashesListView.DisableObjects(Core.Stashes.GetMainStashes());
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

        private void StashesListView_ButtonClick(object sender, CellClickEventArgs args)
        {
            Common.Stash stash = (Common.Stash)args.Model;

            int targetIndex = args.RowIndex;
            if (args.Column == columnOrderUp) targetIndex -= 1;
            if (args.Column == columnOrderDown) targetIndex += 1;

            int mainsCount = GetDisabledObjects().Length;
            int itemCount = stashesListView.GetItemCount();
            if (targetIndex < mainsCount) targetIndex = itemCount - 1;
            if (targetIndex == itemCount) targetIndex = mainsCount;

            Common.Stash target = (Common.Stash)stashesListView.GetItem(targetIndex).RowObject;
            if (target == null || target.ID == stash.ID) return;

            // switch order values
            int stashOrder = stash.Order;
            int targetOrder = target.Order;
            stash.Order = targetOrder;
            target.Order = stashOrder;
            Core.Config.Save();

            Core.Runtime.NotifyStashesChanged();
            stashesListView.Sort();
        }

        private void StashesListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            //if (!Editing) e.Item.Selected = false;
        }

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
                    return new ToolStripMenuItem(fileName + " - " + fileDate, null, delegate (object s, EventArgs e) {
                        if (Core.Runtime.IsStashOpened(stash.ID))
                        {
                            Core.Runtime.RequestLoadingStash();
                        }
                        Core.Stashes.RestoreTransferFile(stash.ID, file);
                        Core.Runtime.NotifyStashesChanged();
                    });
                }));
                if (restoreButtn.DropDownItems.Count == 0)
                {
                    restoreButtn.DropDownItems.Insert(0, new ToolStripMenuItem(L["no_backups"]) {
                        ForeColor = Color.Gray
                    });
                }
                menu.Items.Add(L["export_stash"], null, delegate (object s, EventArgs e) {

                    List<string> filters = new List<string>();
                    if (stash.SC == stash.HC || stash.SC) filters.Add("Softcore ({0})|*{1}t");
                    if (stash.SC == stash.HC || stash.HC) filters.Add("Hardcore ({0})|*{1}h");

                    string filter = string.Format(string.Join("|", filters),
                        GrimDawnLib.GrimDawn.GetExpansionName(stash.Expansion),
                        GrimDawnLib.GrimDawn.GetExpansionExtensionPart(stash.Expansion)
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
                });
            }

            if (!isMainClicked)
            {
                menu.Items.Add(new ToolStripSeparator());
                menu.Items.Add(L["delete_stash"], null, delegate (object s, EventArgs e) {
                    if (Core.Config.ConfirmStashDelete && !ShowStashDeleteWarning()) return;

                    int cur0SCID = Core.Config.Cur0SCID;
                    int cur0HCID = Core.Config.Cur0HCID;
                    int cur1SCID = Core.Config.Cur1SCID;
                    int cur1HCID = Core.Config.Cur1HCID;
                    int cur2SCID = Core.Config.Cur2SCID;
                    int cur2HCID = Core.Config.Cur2HCID;
                    foreach (Common.Stash stash2delete in selectedStashes)
                    {
                        int stashId = stash2delete.ID;
                        if (stashId == cur0SCID || stashId == cur0HCID
                        || stashId == cur1SCID || stashId == cur1HCID
                        || stashId == cur2SCID || stashId == cur2HCID)
                        {
                            MessageBox.Show(string.Format(_err_cannot_delete_stash, stash2delete.Name, _err_stash_is_active), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            continue;
                        }
                        Core.Stashes.DeleteStash(stashId);
                        Core.Runtime.NotifyStashesChanged();
                    }
                    Core.Config.Save();
                    UpdateObjects();
                });
            }

            args.MenuStrip = menu;
        }

        private void StashesListView_SubItemChecking(object sender, SubItemCheckingEventArgs args)
        {
            Common.Stash stash = (Common.Stash)args.RowObject;
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
            Core.Runtime.NotifyStashesChanged();
        }

        private void StashesListView_CellEditFinished(object sender, CellEditEventArgs args)
        {
            Common.Stash stash = (Common.Stash)args.RowObject;
            Core.Config.Save();
            Core.Runtime.NotifyStashesChanged();
        }

        #endregion

        #region Events

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
            if (Core.Windows.ShowImportDialog() == DialogResult.OK)
            {
                Core.Config.Save();
                Core.Runtime.NotifyStashesChanged();
            }
        }

        private void CreateStashButton_Click(object sender, EventArgs e)
        {
            if (Core.Windows.ShowAddStashDialog() == DialogResult.OK)
            {
                Core.Config.Save();
                Core.Runtime.NotifyStashesChanged();
            }
        }

        #endregion




    }
}
