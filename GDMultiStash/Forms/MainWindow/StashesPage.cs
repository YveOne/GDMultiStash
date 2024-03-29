﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;

using BrightIdeasSoftware;

using GDMultiStash.Common;
using GDMultiStash.Common.Objects;
using GDMultiStash.Common.Objects.Sorting.Comparer;

using GDMultiStash.Forms.Controls;
using System.ComponentModel;

namespace GDMultiStash.Forms.MainWindow
{
    [DesignerCategory("code")]
    internal partial class StashesPage : Page
    {

        private Dragging.StashesDragHandler dragHandler;
        private StashesSortComparer sortComparer;

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
            Width = 50,
            AspectGetter = delegate (object row) {
                if (row is StashDummyObject) return "";

                StashObject stash = (StashObject)row;
                bool isCurrent = G.Configuration.IsCurrentStashID(stash.ID);
                if (!isCurrent) return "";

                List<string> modes = new List<string>();
                if (G.Configuration.IsCurrentStashID(stash.ID, stash.Expansion, GrimDawnLib.GrimDawnGameMode.SC))
                    modes.Add("sc");
                if (G.Configuration.IsCurrentStashID(stash.ID, stash.Expansion, GrimDawnLib.GrimDawnGameMode.HC))
                    modes.Add("hc");

                return string.Join(" ", modes);

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
            Width = 150,
            AspectGetter = delegate (object row) {
                if (row is StashDummyObject) return "";

                StashObject stash = (StashObject)row;
                return stash.TransferFileLoaded ? stash.LastWriteTime.ToString() : " File Not Found";
            },
            TextAlign = HorizontalAlignment.Center,
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
                    return G.StashGroups.GetGroup(dummy.GroupID);

                StashObject stash = (StashObject)row;
                if (dragHandler.IsDraggingStash(stash))
                {
                    StashGroupObject grp = dragHandler.DropSink.OverStashGroup;
                    if (grp != null) return grp;
                }
                return G.StashGroups.GetGroup(stash.GroupID);
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
                BackColor = C.ListViewBackColor,
                BarColor = C.ScrollBarColor,
                BarWidth = SystemInformation.VerticalScrollBarWidth - 5
            };

            listViewBorderPanel.BackColor = C.ListViewBackColor;
            listViewBorderPanel.Padding = C.ListViewBorderPadding;

            menuStrip.Renderer = new DefaultContentMenuStripRenderer();
            menuStrip.BackColor = C.ToolStripBackColor;
            menuFlowLayoutPanel.BackColor = C.ToolStripBackColor;

            showSoftCoreCheckbox.ForeColor = C.InteractiveForeColor;
            showSoftCoreCheckbox.ForeColorHover = C.InteractiveForeColorHighlight;
            showHardCoreCheckbox.ForeColor = C.InteractiveForeColor;
            showHardCoreCheckbox.ForeColorHover = C.InteractiveForeColorHighlight;
            shownItemsCountLabel.ForeColor = C.PassiveForeColor;

            stashesListView.RowHeight = C.ListViewRowHeight;
            stashesListView.BackColor = C.ListViewBackColor;
            stashesListView.GridLines = false;
            stashesListView.HeaderMaximumHeight = C.ListViewColumnsHeight;
            stashesListView.HeaderMinimumHeight = C.ListViewColumnsHeight;
            stashesListView.HeaderUsesThemes = false;
            stashesListView.HeaderFormatStyle = new HeaderFormatStyle();
            stashesListView.HeaderFormatStyle.SetBackColor(C.ListViewBackColor);
            stashesListView.HeaderFormatStyle.SetForeColor(C.PassiveForeColor);
            stashesListView.HeaderFormatStyle.SetFont(new Font(stashesListView.Font.FontFamily, C.ListViewColumnsFontHeight, FontStyle.Regular));
            stashesListView.UseCellFormatEvents = true;
            stashesListView.MultiSelect = true;
            stashesListView.PrimarySortColumn = columnOrder;
            stashesListView.PrimarySortOrder = SortOrder.Ascending;
            stashesListView.ShowGroups = true;
            stashesListView.HasCollapsibleGroups = true;
            stashesListView.SeparatorColor = C.ListViewGroupHeaderSeparatorColor;
            stashesListView.GroupHeadingForeColor = C.ListViewGroupHeaderForeColor;
            stashesListView.GroupHeadingForeColorEmpty = C.ListViewGroupHeaderForeColorEmpty;
            stashesListView.GroupHeadingBackColor = C.ListViewGroupHeaderBackColor;
            stashesListView.GroupHeadingBackColorEmpty = C.ListViewGroupHeaderBackColorEmpty;
            stashesListView.GroupHeadingBackColorSelected = C.ListViewGroupHeaderBackColorSelected;
            stashesListView.GroupHeadingFont = new Font(stashesListView.Font.FontFamily, 10, FontStyle.Regular);
            stashesListView.GroupHeadingCountForeColor = C.ListViewGroupHeaderCountForeColor;
            stashesListView.GroupHeadingCountFont = new Font(stashesListView.Font.FontFamily, 8, FontStyle.Regular);
            stashesListView.SpaceBetweenGroups = C.ListViewGroupSpaceBetween;
            stashesListView.AlwaysGroupByColumn = columnStashGroup;



            showExpansionComboBox.Items.AddRange(GrimDawnLib.GrimDawn.ExpansionList
                .Where(exp => exp != GrimDawnLib.GrimDawnGameExpansion.Unknown)
                .Select(exp => GrimDawnLib.GrimDawn.ExpansionNames[exp]).ToArray());

            GotFocus += delegate { stashesListView.Focus(); };
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

                G.Runtime.ShownExpansion = GrimDawnLib.GrimDawn.GetInstalledExpansionFromPath(G.Configuration.Settings.GamePath);
                showExpansionComboBox.SelectedIndex = (int)G.Runtime.ShownExpansion;
                showSoftCoreCheckbox.CheckState = G.Configuration.IntToCheckState(G.Configuration.Settings.ShowSoftcoreState);
                showHardCoreCheckbox.CheckState = G.Configuration.IntToCheckState(G.Configuration.Settings.ShowHardcoreState);

                showExpansionComboBox.SelectionChangeCommitted += ShowExpansionComboBox_SelectionChangeCommitted;
                showSoftCoreCheckbox.CheckStateChanged += ShowSoftCoreCheckbox_CheckStateChanged;
                showHardCoreCheckbox.CheckStateChanged += ShowHardCoreCheckbox_CheckStateChanged;

                stashesListView.CellRightClick += StashesListView_CellRightClick;
                stashesListView.ColumnRightClick += StashesListView_ColumnRightClick;
                stashesListView.SubItemChecking += StashesListView_SubItemChecking;
                stashesListView.CellEditStarting += StashesListView_CellEditStarting;
                stashesListView.CellEditFinished += StashesListView_CellEditFinished;
                stashesListView.SelectionChanged += StashesListView_SelectionChanged;
                stashesListView.ItemSelectionChanged += StashesListView_ItemSelectionChanged;
                stashesListView.FormatCell += StashesListView_FormatCell;
                stashesListView.FormatRow += StashesListView_FormatRow;
                stashesListView.GroupExpandingCollapsing += StashesListView_GroupExpandingCollapsing;
                stashesListView.GroupExpandingCollapsing2 += StashesListView_GroupExpandingCollapsing2;
                stashesListView.GroupHeaderClicked += StashesListView_GroupHeaderClicked;
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

                G.Stashes.ActiveStashChanged += delegate (object sender, Global.Stashes.ActiveStashChangedEventArgs e) {
                    stashesListView.Invoke(new Action(() => {
                        foreach (OLVListItem item in stashesListView.Items)
                        {
                            StashObject stash = (StashObject)item.RowObject;
                            if (stash.ID == e.OldID || stash.ID == e.NewID)
                                stashesListView.RefreshItem(item);
                        }
                    }));
                };

                G.Runtime.ActiveModeChanged += delegate { ReloadList(); };
                G.Stashes.StashesAdded += delegate (object sender, Global.Stashes.StashObjectsEventArgs args) {
                    int scrollY = stashesListView.LowLevelScrollPosition.Y;
                    stashesListView.AddObjects(args.Items.Where(s => s.Expansion == G.Runtime.ShownExpansion).ToList());
                    stashesListView.LowLevelScroll(0, scrollY);
                    stashesListView.EnsureModelVisible(args.Items[0]);
                };
                G.Stashes.StashesRemoved += delegate (object sender, Global.Stashes.StashObjectsEventArgs args) {
                    stashesListView.RemoveObjects(args.Items);
                    stashesListView.AddObjects(new List<StashObject>()); // debug: stashes count not refreshing
                };
                G.Stashes.StashesInfoChanged += delegate (object sender, Global.Stashes.StashObjectsEventArgs args) {
                    RefreshObjects(args.Items);
                };
                G.Stashes.StashesContentChanged += delegate (object sender, Global.Stashes.StashesContentChangedEventArgs args) {
                    RefreshObjects(args.Items);
                };
                G.StashGroups.StashGroupsMoved += delegate { ReloadList(); };
                G.StashGroups.StashGroupsInfoChanged += delegate { ReloadList(); }; // dont know how to trigger group header update
                G.StashGroups.StashGroupsAdded += delegate (object sender, Global.StashGroups.StashGroupObjectsEventArgs args) {
                    ReloadGroupDummies();
                };
                G.StashGroups.StashGroupsRemoved += delegate (object sender, Global.StashGroups.StashGroupObjectsEventArgs args) {
                    ReloadGroupDummies();
                };
                G.Runtime.ActiveExpansionChanged += delegate (object sender, Global.RuntimeManager.ActiveExpansionChangedEventArgs args) {
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
                stashesListView.GroupImageList = new ImageList() { ImageSize = new Size(1, C.ListViewGroupHeaderHeight) };
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

        protected override void Localize(Global.LocalizationManager.StringsHolder L)
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

        public void SelectStashes(IEnumerable<StashObject> stashes)
        {
            stashesListView.Focus();
            stashesListView.SelectObjects(stashes.ToList());
        }

        public void RefreshObjects(IEnumerable<StashObject> l)
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

        public void StashEnsureVisible(StashObject stash)
        {
            stashesListView.EnsureModelVisible(stash);
        }

        #endregion

        #region private methods

        private void ReloadColumns()
        {
            stashesListView.Columns.Clear();
            List<ColumnHeader> h = new List<ColumnHeader>();
            h.Add(columnActive);
            if (G.Configuration.Settings.ShowIDColumn) h.Add(columnID);
            h.Add(columnName);
            if (G.Configuration.Settings.ShowLastChangeColumn) h.Add(columnLastChange);
            h.Add(columnUsage);
            h.Add(columnSC);
            h.Add(columnHC);
            stashesListView.Columns.AddRange(h.ToArray());
        }

        private int totalStashesCount = 0;
        private int shownStashesCount = 0;

        private void ReloadList(Action action = null)
        {
            //if (!Visible) return;

            int scrollY = stashesListView.LowLevelScrollPosition.Y;

            stashesListView.ClearObjects();

            if (action != null)
                action();

            int _sc = G.Configuration.Settings.ShowSoftcoreState;
            int _hc = G.Configuration.Settings.ShowHardcoreState;
            shownStashesCount = 0;
            totalStashesCount = 0;

            stashesListView.SetObjects(Array.FindAll(G.Stashes.GetAllStashes(), delegate (StashObject stash) {
                if (G.Runtime.ShownExpansion == stash.Expansion)
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
                ? G.L.ShownStashesLabel(shownStashesCount, totalStashesCount)
                : G.L.ShownSelectedStashesLabel(shownStashesCount, totalStashesCount, selectedCount);
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
                G.StashGroups.GetAllGroups()
                .ToList()
                .Select(s => new StashDummyObject(null, s.ID))
                .ToList()
            );
        }

        #endregion

        #region events

        private void CreateStashButton_Click(object sender, EventArgs e)
        {
            G.Windows.ShowCreateStashDialog(G.Runtime.ShownExpansion);
        }

        private void StashesListView_BeforeCreatingGroups(object s, CreateGroupsEventArgs args)
        {
            args.Parameters.ItemComparer = new ObjectListViewSortComparer<StashObject>(sortComparer);
        }

        private void StashesListView_GroupExpandingCollapsing(object s, GroupExpandingCollapsingEventArgs args)
        {
            StashGroupObject stashGroup = (StashGroupObject)args.Group.Key;
            stashGroup.Collapsed = !args.IsExpanding;
            G.Configuration.Save();
        }

        private void StashesListView_GroupExpandingCollapsing2(object s, GroupExpandingCollapsingEventArgs args)
        {
            StashGroupObject stashGroup = (StashGroupObject)args.Group.Key;
            stashGroup.Collapsed = args.Group.Collapsed;
            G.Configuration.Save();
        }

        private bool ignoreSelectionChangedEvent = false;
        private void StashesListView_GroupHeaderClicked(object s, OLVGroupFeatures.GroupHeaderClickArgs args)
        {
            // dont do anything if group is empty
            if (args.Group.Items.Count() <= 1)
                return;
            
            stashesListView.Focus();
            ignoreSelectionChangedEvent = true;
            StashGroupObject stashGroup = (StashGroupObject)args.Group.Key;
            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
            {

                // first find any selected item
                OLVListItem otherSelectedItem = null;
                foreach (OLVListItem item in stashesListView.Items)
                    if (item.Selected)
                        otherSelectedItem = item;
                // is there any selected item?
                if (otherSelectedItem != null)
                {
                    // first select all items of clicked group
                    foreach (OLVListItem item in args.Group.Items)
                        item.Selected = true;
                    // next select all items between clicked group and found selected item
                    var doSelect = false;
                    var groupFirstItem = args.Group.Items[0];

                    foreach (OLVListItem item in stashesListView.Items)
                    {
                        if (item == otherSelectedItem || item == groupFirstItem)
                            doSelect = !doSelect;
                        if (doSelect)
                            item.Selected = true;
                    }
                }
            }
            else
            {

                if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                {
                    // ctrl key down.. DONT unselect all
                }
                else
                {
                    // first unselect all
                    foreach (OLVListItem item in stashesListView.Items)
                        item.Selected = false;
                }

                // now select all stashes inside clicked group
                foreach (OLVListItem item in args.Group.Items)
                    item.Selected = true;

            }

            ignoreSelectionChangedEvent = false;
            UpdateStashCountText();
        }

        private void StashesListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            var item = (OLVListItem)e.Item;
            if (item.RowObject is StashDummyObject)
                item.Selected = false;
        }

        private void StashesListView_SelectionChanged(object sender, EventArgs e)
        {
            if (ignoreSelectionChangedEvent)
                return;
            UpdateStashCountText();
            stashesListView.Refresh();
        }

        private void StashesListView_FormatCell(object sender, FormatCellEventArgs e)
        {
            if (e.Model is StashDummyObject) return;

            if (e.ColumnIndex == columnActive.Index)
            {
                StashObject stash = (StashObject)e.Model;
                bool isActive = G.Stashes.ActiveStashID == stash.ID;
                Color stashColor = stash.DisplayColor;
                if (!isActive)
                {
                    e.SubItem.ForeColor = Color.FromArgb(stashColor.R / 2, stashColor.G / 2, stashColor.B / 2);
                }
            }
        }

        private void StashesListView_FormatRow(object sender, FormatRowEventArgs e)
        {
            OLVListSubItem subItem;

            if (e.Model is StashDummyObject)
            {
                e.Item.BackColor = C.ListViewBackColor;
                e.Item.ForeColor = C.PassiveForeColor;
                e.Item.Font = new Font(stashesListView.Font.FontFamily, C.ListViewColumnsFontHeight);
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
            bool isMain = G.Configuration.IsMainStashID(stash.ID);
            bool isDragging = dragHandler.IsDraggingStash(stash);
            bool isSelected = e.Item.Selected;
            bool isActive = G.Stashes.ActiveStashID == stash.ID;
            bool isLocked = stash.Locked;
            Color stashColor = stash.DisplayColor;

            subItem = e.Item.GetSubItem(columnName.Index);
            if (subItem != null)
            {
                if (isLocked)
                    subItem.Decorations.Add(Main.Decorations.LockIconDecoration);
                if (isMain)
                    subItem.Decorations.Add(Main.Decorations.HomeIconDecoration);
                var i = 0;
                foreach (var deco in subItem.Decorations)
                {
                    if (deco is ImageDecoration imgDeco)
                    {
                        imgDeco.Offset = new Size(i * -16 + -5, 0);
                        i += 1;
                    }
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
                e.Item.BackColor = C.ListViewItemBackColorSelected;
                e.Item.ForeColor = Color.WhiteSmoke;
            }
            else
            {
                e.Item.BackColor = C.ListViewItemBackColor;
                if (stash.TransferFileLoaded)
                {
                    e.Item.ForeColor = stashColor;
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
            checkedList.AddItem(columnID.Text, G.Configuration.Settings.ShowIDColumn);
            checkedList.AddItem(columnLastChange.Text, G.Configuration.Settings.ShowLastChangeColumn);
            checkedList.ItemCheck += delegate (object s, ItemCheckEventArgs f) {
                bool chckd = f.NewValue == CheckState.Checked;
                switch (f.Index)
                {
                    case 0: G.Configuration.Settings.ShowIDColumn = chckd; break;
                    case 1: G.Configuration.Settings.ShowLastChangeColumn = chckd; break;
                }
                G.Configuration.Save();

                ReloadList(() => ReloadColumns());
            };

            menu.Items.Insert(menu.Items.Count, checkedList);
            menu.Show(p.X, p.Y);
        }

        private void StashesListView_CellRightClick(object sender, CellRightClickEventArgs args)
        {
            if (!(args.Model is StashObject stash)) return;
            if (stash is StashDummyObject) return;

            var menu = new ContextMenues.StashesPageContextMenu(this, args);

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
            if (G.Configuration.Settings.ExperimentalFeatures)
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
            if (G.Configuration.IsMainStashID(stash.ID))
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
            G.Configuration.Save();
            G.Stashes.InvokeStashesMoved(stash);
            int _sc = G.Configuration.Settings.ShowSoftcoreState;
            int _hc = G.Configuration.Settings.ShowHardcoreState;
            if (!(stash.SC && _sc == 0 || !stash.SC && _sc == 1 || stash.HC && _hc == 0 || !stash.HC && _hc == 1))
            {
            }
            else
            {
                // i am creating a thread to remove it a lil bit delayed
                // so the user can see the checkbox icon changing
                new System.Threading.Thread(() => {
                    System.Threading.Thread.Sleep(100);
                    Invoke(new Action(() => {
                        stashesListView.RemoveObject(stash);
                    }));
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
            if (args.Column == columnName) G.Stashes.InvokeStashesInfoChanged(stash);
            else return;
            G.Configuration.Save();
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
                G.Windows.ShowImportDialog(files);
            }
        }

        private void DragHandler_DragEnd(object sender, EventArgs e)
        {
            UnselectAll();
            G.Configuration.Save();
            G.Stashes.InvokeStashesMoved(dragHandler.DragSource.Items);
        }

        private void ShowExpansionComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            G.Runtime.ShownExpansion = (GrimDawnLib.GrimDawnGameExpansion)showExpansionComboBox.SelectedIndex;
            ReloadList();
        }

        private void ShowSoftCoreCheckbox_CheckStateChanged(object sender, EventArgs e)
        {
            G.Configuration.Settings.ShowSoftcoreState = G.Configuration.CheckStateToInt(showSoftCoreCheckbox.CheckState);
            G.Configuration.Save();
            ReloadList();
        }

        private void ShowHardCoreCheckbox_CheckStateChanged(object sender, EventArgs e)
        {
            G.Configuration.Settings.ShowHardcoreState = G.Configuration.CheckStateToInt(showHardCoreCheckbox.CheckState);
            G.Configuration.Save();
            ReloadList();
        }

        #endregion

    }
}
