using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using BrightIdeasSoftware;

using GDMultiStash.Common;
using GDMultiStash.Common.Objects;
using GDMultiStash.Common.Objects.Sorting;

using GDMultiStash.Forms.Controls;

namespace GDMultiStash.Forms.Main
{
    internal partial class StashesPage : Page
    {

        private Dragging.StashesDragHandler dragHandler;
        private StashesSortComparer sortComparer;

        public GrimDawnLib.GrimDawnGameExpansion ShownExpansion { get; private set; }

        #region columns

        public readonly OLVColumn columnOrder = new DefaultOLVColumn()
        {
            AspectName = "Order",
            Width = 50,
            Sortable = true,
        };

        public readonly OLVColumn columnStashGroup = new DefaultOLVColumn()
        {
            Text = "",
            MaximumWidth = 30,
            MinimumWidth = 30,
            Width = 30,
            Searchable = false,
            Groupable = true,
            Sortable = true,
            IsEditable = false,
        };

        public readonly OLVColumn columnActive = new DefaultOLVColumn()
        {
            Text = "",
            Width = 30,
            AspectGetter = delegate (object row) {
                if (row is StashDummyObject) return "";

                StashObject stash = (StashObject)row;
                if (stash.ID == Global.Runtime.ActiveStashID) return ">>>";
                return "";
            },
            TextAlign = HorizontalAlignment.Center,
        };

        public readonly OLVColumn columnID = new DefaultOLVColumn()
        {
            AspectName = "ID",
            Width = 50,
            TextAlign = HorizontalAlignment.Left,
        };

        public readonly OLVColumn columnName = new DefaultOLVColumn()
        {
            AspectName = "Name",
            Width = 100,
            MaximumWidth = -1,
            IsEditable = true,
            FillsFreeSpace = true,
            CellEditUseWholeCell = true,
        };

        public readonly OLVColumn columnUsage = new DefaultOLVColumn()
        {
            Text = "%",
            ImageAspectName = "UsageIndicator",
            Width = 50,
            TextAlign = HorizontalAlignment.Center,
        };

        public readonly OLVColumn columnLastChange = new DefaultOLVColumn()
        {
            Width = 130,
            AspectGetter = delegate (object row) {
                if (row is StashDummyObject) return "";

                StashObject stash = (StashObject)row;
                return stash.TransferFileLoaded ? stash.LastWriteTime.ToString() : " File Not Found";
            },
            TextAlign = HorizontalAlignment.Right,
        };

        public readonly OLVColumn columnSC = new DefaultOLVColumn()
        {
            Text = "SC",
            AspectName = "SC",
            Width = 40,
            IsEditable = true,
            CheckBoxes = true,
            TextAlign = HorizontalAlignment.Center,
        };

        public readonly OLVColumn columnHC = new DefaultOLVColumn()
        {
            Text = "HC",
            AspectName = "HC",
            Width = 40,
            IsEditable = true,
            CheckBoxes = true,
            TextAlign = HorizontalAlignment.Center,
        };

        #endregion

        public StashesPage(MainForm mainForm) : base(mainForm)
        {
            InitializeComponent();

            columnStashGroup.GroupKeyGetter = delegate (object row)
            {
                if (row is StashDummyObject dummy)
                    return Global.Stashes.GetStashGroup(dummy.GroupID);

                StashObject stash = (StashObject)row;
                StashGroupObject grp;
                if (dragHandler.IsDraggingStash(stash))
                {
                    grp = dragHandler.DropSink.OverStashGroup;
                    if (grp != null) return grp;
                }
                grp = Global.Stashes.GetStashGroup(stash.GroupID);
                return grp;
            };

            columnStashGroup.GroupFormatter = (OLVGroup group, GroupingParameters parms) =>
            {
                group.TitleImage = "height";
                StashGroupObject stashGroup = (StashGroupObject)group.Key;

                group.Header = stashGroup.Name;
                //group.Id = cat.Order;

                GroupsSortComparer grpComp = new GroupsSortComparer();
                parms.GroupComparer = Comparer<OLVGroup>.Create((x, y) =>
                {
                    StashGroupObject grp1 = (StashGroupObject)x.Key;
                    StashGroupObject grp2 = (StashGroupObject)y.Key;
                    return grpComp.Compare(grp1, grp2);
                });
            };

            PseudoScrollBarPanel scrollCover = new PseudoScrollBarPanel(stashesListView)
            {
                BackColor = Constants.ListViewBackColor,
                BarColor = Constants.ScrollBarColor,
                BarWidth = SystemInformation.VerticalScrollBarWidth - 5
            };

            showExpansionComboBox.BorderColor = Constants.FormBackColor;
            showExpansionComboBox.ButtonColor = Constants.PassiveForeColor;
            showExpansionComboBox.BackColor = Constants.ListViewBackColor;
            showExpansionComboBox.ForeColor = Constants.InteractiveForeColor;
            showExpansionComboBox.BackColorHighlight = Constants.InteractiveForeColor;
            showExpansionComboBox.ForeColorHighlight = Constants.ListViewBackColor;

            listViewBorderPanel.BackColor = Constants.ListViewBackColor;
            listViewBorderPanel.Padding = Constants.ListViewBorderPadding;

            menuStrip.Renderer = new FlatToolStripRenderer();
            menuStrip.BackColor = Constants.ToolStripBackColor;
            menuFlowLayoutPanel.BackColor = Constants.ToolStripBackColor;

            Main.InitializeToolStripButton(createStashButton,
                Constants.ToolStripButtonBackColor, Constants.ToolStripButtonBackColorHover,
                Constants.InteractiveForeColor, Constants.InteractiveForeColorHighlight);

            showSoftCoreCheckbox.ForeColor = Constants.PassiveForeColor;
            showHardCoreCheckbox.ForeColor = Constants.PassiveForeColor;
            shownItemsCountLabel.ForeColor = Constants.PassiveForeColor;

            stashesListView.RowHeight = Constants.ListViewRowHeight;
            stashesListView.BackColor = Constants.ListViewBackColor;
            stashesListView.GridLines = false;
            stashesListView.HeaderMaximumHeight = Constants.ListViewColumnsHeight;
            stashesListView.HeaderMinimumHeight = Constants.ListViewColumnsHeight;
            stashesListView.HeaderUsesThemes = false;
            stashesListView.HeaderFormatStyle = new HeaderFormatStyle();
            stashesListView.HeaderFormatStyle.SetBackColor(Constants.ListViewBackColor);
            stashesListView.HeaderFormatStyle.SetForeColor(Constants.PassiveForeColor);
            stashesListView.HeaderFormatStyle.SetFont(new Font(stashesListView.Font.FontFamily, Constants.ListViewColumnsFontHeight, FontStyle.Regular));
            stashesListView.UseCellFormatEvents = true;
            stashesListView.MultiSelect = true;
            stashesListView.PrimarySortColumn = columnOrder;
            stashesListView.PrimarySortOrder = SortOrder.Ascending;
            stashesListView.ShowGroups = true;
            stashesListView.HasCollapsibleGroups = true;
            stashesListView.SeparatorColor = Constants.ListViewGroupHeaderSeparatorColor;
            stashesListView.GroupHeadingForeColor = Constants.ListViewGroupHeaderForeColor;
            stashesListView.GroupHeadingBackColor = Constants.ListViewGroupHeaderBackColor;
            stashesListView.GroupHeadingFont = new Font(stashesListView.Font.FontFamily, 10, FontStyle.Regular);
            stashesListView.GroupHeadingCountForeColor = Constants.ListViewGroupHeaderCountForeColor;
            stashesListView.GroupHeadingCountFont = new Font(stashesListView.Font.FontFamily, 8, FontStyle.Regular);
            stashesListView.SpaceBetweenGroups = Constants.ListViewGroupSpaceBetween;
            stashesListView.AlwaysGroupByColumn = columnStashGroup;

            showExpansionComboBox.Items.AddRange(GrimDawnLib.GrimDawn.GetExpansionList()
                .Where(exp => exp != GrimDawnLib.GrimDawnGameExpansion.Unknown)
                .Select(exp => GrimDawnLib.GrimDawn.GetExpansionName(exp)).ToArray());


            Main.SpaceClick += delegate { UnselectAll(); };
            Main.Click += delegate { UnselectAll(); };
            listViewBorderPanel.Click += delegate { UnselectAll(); };

            sortComparer = new StashesSortComparer((StashObject x, StashObject y, out int ret) =>
            {
                ret = 0;
                if (dragHandler.IsDragging)
                {
                    if (dragHandler.DropSink.OrderedList.Contains(x))
                        if (dragHandler.DropSink.OrderedList.Count > 0)
                        {
                            int s1i = dragHandler.DropSink.OrderedList.IndexOf(x);
                            int s2i = dragHandler.DropSink.OrderedList.IndexOf(y);
                            ret = s1i.CompareTo(s2i);
                            return true;
                        }
                }
                return false;
            });

            Load += delegate {

                ShownExpansion = GrimDawnLib.GrimDawn.GetInstalledExpansionFromPath(Global.Configuration.Settings.GamePath);
                showExpansionComboBox.SelectedIndex = (int)ShownExpansion;
                showSoftCoreCheckbox.CheckState = Global.Configuration.IntToCheckState(Global.Configuration.Settings.ShowSoftcoreState);
                showHardCoreCheckbox.CheckState = Global.Configuration.IntToCheckState(Global.Configuration.Settings.ShowHardcoreState);

                showExpansionComboBox.SelectionChangeCommitted += ShowExpansionComboBox_SelectionChangeCommitted;
                showSoftCoreCheckbox.CheckStateChanged += ShowSoftCoreCheckbox_CheckStateChanged;
                showHardCoreCheckbox.CheckStateChanged += ShowHardCoreCheckbox_CheckStateChanged;

                stashesListView.CellRightClick += StashesListView_CellRightClick;
                stashesListView.ColumnRightClick += StashesListView_ColumnRightClick;
                stashesListView.SubItemChecking += StashesListView_SubItemChecking;
                stashesListView.CellEditStarting += StashesListView_CellEditStarting;
                stashesListView.CellEditFinished += StashesListView_CellEditFinished;
                stashesListView.SelectedIndexChanged += StashesListView_SelectedIndexChanged;
                stashesListView.FormatCell += StashesListView_FormatCell;
                stashesListView.FormatRow += StashesListView_FormatRow;
                stashesListView.GroupExpandingCollapsing += StashesListView_GroupExpandingCollapsing;
                stashesListView.GroupExpandingCollapsing2 += StashesListView_GroupExpandingCollapsing2;
                stashesListView.BeforeCreatingGroups += StashesListView_BeforeCreatingGroups;
                stashesListView.MouseMove += delegate (object sen, MouseEventArgs eee) {
                    // this is used to refres the item the mouse is over
                    // needed to refresh checkbox images
                    OlvListViewHitTestInfo info = stashesListView.MouseMoveHitTest;
                    if (info == null || info.Item == null) return;
                    OLVListSubItem subItem;
                    subItem = info.Item.GetSubItem(columnSC.Index);
                    if (subItem != null)
                    {
                        Point p = stashesListView.PointToClient(Cursor.Position);
                        if (subItem.Bounds.Contains(p)) stashesListView.RefreshItem(info.Item);
                    }
                    subItem = info.Item.GetSubItem(columnHC.Index);
                    if (subItem != null)
                    {
                        Point p = stashesListView.PointToClient(Cursor.Position);
                        if (subItem.Bounds.Contains(p)) stashesListView.RefreshItem(info.Item);
                    }
                };

                dragHandler = new Dragging.StashesDragHandler(stashesListView);
                dragHandler.DragSource.DragEnd += DragHandler_DragEnd;
                stashesListView.DragEnter += StashesListView_DragEnter;
                stashesListView.DragDrop += StashesListView_DragDrop;

                Global.Runtime.ActiveStashChanged += delegate (object sender, GlobalHandlers.RuntimeHandler.ActiveStashChangedEventArgs e) {
                    stashesListView.Invoke((MethodInvoker)delegate {
                        foreach (OLVListItem item in stashesListView.Items)
                        {
                            StashObject stash = (StashObject)item.RowObject;
                            if (stash.ID == e.OldID || stash.ID == e.NewID)
                                stashesListView.RefreshItem(item);
                        }
                    });
                };

                Global.Runtime.ActiveModeChanged += delegate { ReloadList(); };
                Global.Runtime.StashesAdded += delegate (object sender, GlobalHandlers.RuntimeHandler.ListEventArgs<StashObject> args) {
                    int scrollY = stashesListView.LowLevelScrollPosition.Y;
                    stashesListView.AddObjects(args.Items);
                    stashesListView.LowLevelScroll(0, scrollY);
                    stashesListView.EnsureModelVisible(args.Items[0]);
                };
                Global.Runtime.StashesRemoved += delegate (object sender, GlobalHandlers.RuntimeHandler.ListEventArgs<StashObject> args) { stashesListView.RemoveObjects(args.Items); };
                Global.Runtime.StashesInfoChanged += delegate (object sender, GlobalHandlers.RuntimeHandler.ListEventArgs<StashObject> args) {
                    RefreshObjects(args.Items);
                };
                Global.Runtime.StashesContentChanged += delegate (object sender, GlobalHandlers.RuntimeHandler.ListEventArgs<StashObject> args) {
                    RefreshObjects(args.Items);
                };
                Global.Runtime.StashGroupsRebuild += delegate { ReloadList(); };
                Global.Runtime.StashGroupsInfoChanged += delegate { ReloadList(); }; // dont know how to trigger group header update
                Global.Runtime.StashGroupsAdded += delegate (object sender, GlobalHandlers.RuntimeHandler.ListEventArgs<StashGroupObject> args) {
                    ReloadGroupDummies();
                };
                Global.Runtime.StashGroupsRemoved += delegate (object sender, GlobalHandlers.RuntimeHandler.ListEventArgs<StashGroupObject> args) {
                    ReloadGroupDummies();
                };
                Global.Runtime.StashesImported += delegate (object sender, GlobalHandlers.RuntimeHandler.ListEventArgs<StashObject> args) {
                    RefreshObjects(args.Items);
                };
                Global.Runtime.ActiveExpansionChanged += delegate (object sender, GlobalHandlers.RuntimeHandler.ActiveExpansionChangedEventArgs args) {
                    showExpansionComboBox.SelectedIndex = (int)args.Expansion;
                    ShowExpansionComboBox_SelectionChangeCommitted(sender, EventArgs.Empty);
                };

                stashesListView.AboutToCreateGroups += delegate (object s, CreateGroupsEventArgs args) {
                    // TODO: create sub for me
                    foreach (OLVGroup grp in args.Groups)
                    {
                        StashGroupObject stashGroup = (StashGroupObject)grp.Key;
                        grp.Collapsed = stashGroup.Collapsed;
                    }
                };

                // this is used to increase height of listview group headers
                stashesListView.GroupImageList = new ImageList() { ImageSize = new Size(1, Constants.ListViewGroupHeaderHeight) };
                using (Bitmap bmp = new Bitmap(1, stashesListView.GroupImageList.ImageSize.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
                using (Graphics gfx = Graphics.FromImage(bmp))
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(0, 0, 0, 0)))
                {
                    gfx.FillRectangle(brush, 0, 0, 1, bmp.Height);
                    stashesListView.GroupImageList.Images.Add("height", bmp);
                }

                ReloadColumns();
                ReloadList();
            };
        }

        protected override void Localize(GlobalHandlers.LocalizationHandler.StringsHolder L)
        {
            columnID.Text = L.IdColumn();
            columnName.Text = L.NameColumn();
            columnLastChange.Text = L.LastChangeColumn();
            columnSC.Text = L.SoftcoreColumn();
            columnHC.Text = L.HardcoreColumn();
            createStashButton.Text = L.CreateStashButton();
            showSoftCoreCheckbox.Text = columnSC.Text;
            showHardCoreCheckbox.Text = columnHC.Text;
        }

        #region public methods

        public void UnselectAll()
        {
            foreach (OLVListItem item in stashesListView.SelectedItems)
            {
                item.Selected = false;
                stashesListView.RefreshItem(item);
            }
            stashesListView.CancelCellEdit();
            Main.ClearFocus();
        }

        public void RefreshObjects(IList<StashObject> l)
        {
            stashesListView.RefreshObjects(l.ToArray());
        }

        public void RefreshAllObjects()
        {
            foreach (OLVGroup grp in stashesListView.OLVGroups)
                if (!grp.Collapsed)
                    foreach (OLVListItem li in grp.Items)
                        stashesListView.RefreshItem(li);
        }

        public StashObject[] GetSelectedObjects()
        {
            return stashesListView.SelectedItems
                .Cast<OLVListItem>()
                .Select(l => (StashObject)l.RowObject)
                .ToArray();
        }

        public void ActivateNameEditing(int index)
        {
            OLVListItem item = (OLVListItem)stashesListView.Items[index];
            stashesListView.EditSubItem(item, columnName.Index);
        }

        #endregion

        #region private methods

        private void ReloadColumns()
        {
            stashesListView.Columns.Clear();
            List<ColumnHeader> h = new List<ColumnHeader>();
            h.Add(columnActive);
            if (Global.Configuration.Settings.ShowIDColumn) h.Add(columnID);
            h.Add(columnName);
            if (Global.Configuration.Settings.ShowLastChangeColumn) h.Add(columnLastChange);
            h.Add(columnUsage);
            h.Add(columnSC);
            h.Add(columnHC);
            stashesListView.Columns.AddRange(h.ToArray());
        }

        private int totalStashesCount = 0;
        private int shownStashesCount = 0;

        private void ReloadList()
        {
            //if (!Visible) return;

            int scrollY = stashesListView.LowLevelScrollPosition.Y;

            stashesListView.ClearObjects();

            int _sc = Global.Configuration.Settings.ShowSoftcoreState;
            int _hc = Global.Configuration.Settings.ShowHardcoreState;
            shownStashesCount = 0;
            totalStashesCount = 0;

            stashesListView.SetObjects(Array.FindAll(Global.Stashes.GetAllStashes(), delegate (StashObject stash) {
                if (ShownExpansion == stash.Expansion)
                {
                    totalStashesCount += 1;
                    if (!(stash.SC && _sc == 0 || !stash.SC && _sc == 1 || stash.HC && _hc == 0 || !stash.HC && _hc == 1))
                    {
                        shownStashesCount += 1;
                        return true;
                    }
                }
                return false;
            }));

            ReloadGroupDummies();
            stashesListView.Sort();
            stashesListView.LowLevelScroll(0, scrollY);
            Main.StashGroupsPage.RefreshAllObjects();
            UpdateStashCountText();
        }

        private void UpdateStashCountText()
        {
            var selectedCount = stashesListView.SelectedItems.Count;
            shownItemsCountLabel.Text =
                selectedCount == 0
                ? Global.L.ShownStashesLabel(shownStashesCount, totalStashesCount)
                : Global.L.ShownSelectedStashesLabel(shownStashesCount, totalStashesCount, selectedCount);
        }

        private void ReloadGroupDummies()
        {
            stashesListView.RemoveObjects(stashesListView.Items
                .Cast<OLVListItem>()
                .Select(item => item.RowObject)
                .Where(item => item is StashDummyObject)
                .ToList()
            );
            stashesListView.AddObjects(
                Global.Stashes.GetAllStashGroups()
                .ToList()
                .Select(s => new StashDummyObject(null, s.ID))
                .ToList()
            );
        }

        #endregion

        #region events

        private void CreateStashButton_Click(object sender, EventArgs e)
        {
            Global.Windows.ShowCreateStashDialog(ShownExpansion);
        }

        private void StashesListView_BeforeCreatingGroups(object s, CreateGroupsEventArgs args)
        {
            args.Parameters.ItemComparer = new ObjectListViewSortComparer<StashObject>(sortComparer);
        }

        private void StashesListView_GroupExpandingCollapsing(object s, GroupExpandingCollapsingEventArgs args)
        {
            StashGroupObject stashGroup = (StashGroupObject)args.Group.Key;
            stashGroup.Collapsed = !args.IsExpanding;
            Global.Configuration.Save();
        }

        private void StashesListView_GroupExpandingCollapsing2(object s, GroupExpandingCollapsingEventArgs args)
        {
            StashGroupObject stashGroup = (StashGroupObject)args.Group.Key;
            stashGroup.Collapsed = args.Group.Collapsed;
            Global.Configuration.Save();
        }

        private void StashesListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (OLVListItem item in stashesListView.Items)
                if (item.RowObject is StashDummyObject)
                    item.Selected = false;
            UpdateStashCountText();
        }

        private void StashesListView_FormatCell(object sender, FormatCellEventArgs e)
        {
            if (e.Model is StashDummyObject) return;

            if (e.ColumnIndex == columnName.Index)
            {
                StashObject stash = (StashObject)e.Model;
                bool isMain = Global.Configuration.IsMainStashID(stash.ID);
                bool isDragging = dragHandler.IsDraggingStash(stash);
                bool isSelected = e.Item.Selected;
                bool isActive = Global.Runtime.ActiveStashID == stash.ID;
                bool isLocked = stash.Locked;

                FontStyle fontStyle = FontStyle.Regular;
                if (isMain) fontStyle |= FontStyle.Italic;
                if (isActive) fontStyle |= FontStyle.Bold;

                e.SubItem.Font = new Font(e.Item.Font, fontStyle);
            }
        }

        private void StashesListView_FormatRow(object sender, FormatRowEventArgs e)
        {
            OLVListSubItem subItem;

            if (e.Model is StashDummyObject)
            {
                e.Item.BackColor = Constants.ListViewItemBackColor;
                e.Item.ForeColor = Constants.PassiveForeColor;
                e.Item.Font = new Font(stashesListView.Font.FontFamily, Constants.ListViewColumnsFontHeight);
                subItem = e.Item.GetSubItem(columnSC.Index);
                if (subItem != null) subItem.Decorations.Add(Main.Decorations.CheckBoxHideDecoration);
                subItem = e.Item.GetSubItem(columnHC.Index);
                if (subItem != null) subItem.Decorations.Add(Main.Decorations.CheckBoxHideDecoration);
                return;
            }

            for (var i = 0; i < e.Item.SubItems.Count; i += 1)
                ((OLVListSubItem)e.Item.SubItems[i]).Decoration = i == 0
                    ? Main.Decorations.CellBorderFirstDecoration
                    : Main.Decorations.CellBorderDecoration;

            StashObject stash = (StashObject)e.Model;
            bool isMain = Global.Configuration.IsMainStashID(stash.ID);
            bool isDragging = dragHandler.IsDraggingStash(stash);
            bool isSelected = e.Item.Selected;
            bool isActive = Global.Runtime.ActiveStashID == stash.ID;
            bool isLocked = stash.Locked;

            subItem = e.Item.GetSubItem(columnName.Index);
            if (subItem != null)
            {
                if (isLocked)
                {
                    subItem.Decorations.Add(Main.Decorations.LockDecoration);
                }
            }

            subItem = e.Item.GetSubItem(columnSC.Index);
            if (subItem != null)
            {
                Rectangle bounds = new Rectangle(
                    subItem.Bounds.X + (subItem.Bounds.Width / 2) - 7,
                    subItem.Bounds.Y + (subItem.Bounds.Height / 2) - 8,
                    15, 15);
                Point p = stashesListView.PointToClient(Cursor.Position);
                bool hit = bounds.Contains(p);

                subItem.Decorations.Add(isMain
                    ? Main.Decorations.CheckBoxBackDecoration
                    : (hit
                        ? Main.Decorations.CheckBoxBackHoverDecoration
                        : Main.Decorations.CheckBoxBackDecoration
                    ));
                subItem.Decorations.Add(isMain
                    ? (stash.SC
                        ? Main.Decorations.CheckBoxTickDisabledDecoration
                        : Main.Decorations.CheckBoxCrossDisabledDecoration
                        )
                    : (stash.SC
                        ? Main.Decorations.CheckBoxTickDecoration
                        : Main.Decorations.CheckBoxCrossDecoration
                    ));
            }
            subItem = e.Item.GetSubItem(columnHC.Index);
            if (subItem != null)
            {
                Rectangle bounds = new Rectangle(
                    subItem.Bounds.X + (subItem.Bounds.Width / 2) - 7,
                    subItem.Bounds.Y + (subItem.Bounds.Height / 2) - 8,
                    15, 15);
                Point p = stashesListView.PointToClient(Cursor.Position);
                bool hit = bounds.Contains(p);

                subItem.Decorations.Add(isMain
                    ? Main.Decorations.CheckBoxBackDecoration
                    : (hit
                        ? Main.Decorations.CheckBoxBackHoverDecoration
                        : Main.Decorations.CheckBoxBackDecoration
                    ));
                subItem.Decorations.Add(isMain
                    ? (stash.HC
                        ? Main.Decorations.CheckBoxTickDisabledDecoration
                        : Main.Decorations.CheckBoxCrossDisabledDecoration
                        )
                    : (stash.HC
                        ? Main.Decorations.CheckBoxTickDecoration
                        : Main.Decorations.CheckBoxCrossDecoration
                    ));
            }

            if (isDragging)
            {
                e.Item.BackColor = Color.Teal;
                e.Item.ForeColor = Color.WhiteSmoke;
            }
            else
            {
                e.Item.BackColor = Constants.ListViewItemBackColor;
                e.Item.ForeColor = stash.DisplayColor;
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
            }
        }

        private void StashesListView_ColumnRightClick(object sender, ColumnClickEventArgs e)
        {
            if (!Native.GetCursorPos(out Native.POINT p)) return;
            ContextMenuStrip menu = new ContextMenuStrip()
            {
                Width = 200,
            };
            ToolStripCheckedListBox checkedList = new ToolStripCheckedListBox()
            {
            };
            checkedList.AddItem(columnID.Text, Global.Configuration.Settings.ShowIDColumn);
            checkedList.AddItem(columnLastChange.Text, Global.Configuration.Settings.ShowLastChangeColumn);
            checkedList.ItemCheck += delegate (object s, ItemCheckEventArgs f) {
                bool chckd = f.NewValue == CheckState.Checked;
                switch (f.Index)
                {
                    case 0: Global.Configuration.Settings.ShowIDColumn = chckd; break;
                    case 1: Global.Configuration.Settings.ShowLastChangeColumn = chckd; break;
                }
                ReloadColumns();
                ReloadList();
                Global.Configuration.Save();
            };

            menu.Items.Insert(menu.Items.Count, checkedList);
            menu.Show(p.X, p.Y);
        }

        private void StashesListView_CellRightClick(object sender, CellRightClickEventArgs args)
        {
            if (!(args.Model is StashObject stash)) return;
            if (stash is StashDummyObject) return;

            StashesPageContextMenu menu = new StashesPageContextMenu(this, args);

            menu.Closed += delegate {
                UnselectAll();
            };

            menu.AddColorOption();
            menu.AddLockOption();
            menu.AddEditNameOption();

            menu.AddSeparator();

            menu.AddRestoreBackupOption();
            menu.AddOverwriteOption();
            menu.AddExportButton();

            menu.AddSeparator();

            menu.AddEditTabsButton();
            menu.AddChangeExpansionOption();
            menu.AddAutoFillOption();
            menu.AddAutoSortOption();
            if (Global.Configuration.Settings.ExperimentalFeatures)
            {
            }

            menu.AddSeparator();

            menu.AddDeleteOption();

            args.MenuStrip = menu;
        }

        private void StashesListView_SubItemChecking(object sender, SubItemCheckingEventArgs args)
        {
            if (args.RowObject is StashDummyObject) return;

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
            }
            Global.Configuration.Save();
            Global.Runtime.NotifyStashesRebuild(); // todo: maybe add a better event for this?
            int _sc = Global.Configuration.Settings.ShowSoftcoreState;
            int _hc = Global.Configuration.Settings.ShowHardcoreState;
            if (!(stash.SC && _sc == 0 || !stash.SC && _sc == 1 || stash.HC && _hc == 0 || !stash.HC && _hc == 1))
            {
            }
            else
            {
                // i am creating a thread to remove it a lil bit delayed
                // so the user can see the checkbox icon changing
                new System.Threading.Thread(() => {
                    System.Threading.Thread.Sleep(100);
                    Invoke((MethodInvoker)delegate {
                        stashesListView.RemoveObject(stash);
                    });
                }).Start();
            }
        }

        private void StashesListView_CellEditStarting(object sender, CellEditEventArgs e)
        {
            if (e.RowObject is StashDummyObject)
                e.Cancel = true;
        }

        private void StashesListView_CellEditFinished(object sender, CellEditEventArgs args)
        {
            if (args.RowObject is StashDummyObject) return;

            StashObject stash = (StashObject)args.RowObject;
            if (args.Column == columnName) Global.Runtime.NotifyStashesInfoChanged(stash);
            else return;
            Global.Configuration.Save();
            UnselectAll();
        }

        private void StashesListView_DragEnter(object sender, DragEventArgs e)
        {
            if (dragHandler.IsDragging)
                e.Effect = DragDropEffects.None;
            else
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                    e.Effect = DragDropEffects.Copy;
        }

        private void StashesListView_DragDrop(object sender, DragEventArgs e)
        {
            if (dragHandler.IsDragging) return;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                Global.Windows.ShowImportDialog(files);
            }
        }

        private void DragHandler_DragEnd(object sender, EventArgs e)
        {
            UnselectAll();
            Global.Configuration.Save();
            Global.Runtime.NotifyStashesRebuild();
        }

        private void ShowExpansionComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ShownExpansion = (GrimDawnLib.GrimDawnGameExpansion)showExpansionComboBox.SelectedIndex;
            ReloadList();
        }

        private void ShowSoftCoreCheckbox_CheckStateChanged(object sender, EventArgs e)
        {
            Global.Configuration.Settings.ShowSoftcoreState = Global.Configuration.CheckStateToInt(showSoftCoreCheckbox.CheckState);
            Global.Configuration.Save();
            ReloadList();
        }

        private void ShowHardCoreCheckbox_CheckStateChanged(object sender, EventArgs e)
        {
            Global.Configuration.Settings.ShowHardcoreState = Global.Configuration.CheckStateToInt(showHardCoreCheckbox.CheckState);
            Global.Configuration.Save();
            ReloadList();
        }

        #endregion

    }
}
