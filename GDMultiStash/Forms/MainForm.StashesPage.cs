using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using BrightIdeasSoftware;

using GDMultiStash.Common;
using GDMultiStash.Common.Objects;
using GDMultiStash.Common.Objects.Sorting;

namespace GDMultiStash.Forms
{
    //stashesListView.EnsureModelVisible(((OLVListItem)stashesListView.Items[10]).RowObject);

    internal partial class MainForm
    {

        private Dragging.StashesDragHandler stashes_dragHandler;
        private GrimDawnLib.GrimDawnGameExpansion stashes_shownExpansion;
        private StashesSortComparer stashes_sortComparer;

        #region columns

        private readonly OLVColumn stashes_columnOrder = new DefaultOLVColumn()
        {
            AspectName = "Order",
            Width = 50,
            Sortable = true,
        };

        private readonly OLVColumn stashes_columnStashGroup = new DefaultOLVColumn()
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

        private readonly OLVColumn stashes_columnActive = new DefaultOLVColumn()
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

        private readonly OLVColumn stashes_columnID = new DefaultOLVColumn()
        {
            AspectName = "ID",
            Width = 50,
            TextAlign = HorizontalAlignment.Left,
        };

        private readonly OLVColumn stashes_columnName = new DefaultOLVColumn()
        {
            AspectName = "Name",
            Width = 100,
            MaximumWidth = -1,
            IsEditable = true,
            FillsFreeSpace = true,
            CellEditUseWholeCell = true,
        };
        private readonly OLVColumn stashes_columnUsage = new DefaultOLVColumn()
        {
            Text = "%",
            ImageAspectName = "UsageIndicator",
            Width = 50,
            TextAlign = HorizontalAlignment.Center,
        };
        private readonly OLVColumn stashes_columnLastChange = new DefaultOLVColumn()
        {
            Width = 130,
            AspectGetter = delegate (object row) {
                if (row is StashDummyObject) return "";

                StashObject stash = (StashObject)row;
                return stash.TransferFileLoaded ? stash.LastWriteTime.ToString() : " File Not Found";
            },
            TextAlign = HorizontalAlignment.Right,
        };

        private readonly OLVColumn stashes_columnSC = new DefaultOLVColumn()
        {
            Text = "SC",
            AspectName = "SC",
            Width = 40,
            IsEditable = true,
            CheckBoxes = true,
            TextAlign = HorizontalAlignment.Center,
        };

        private readonly OLVColumn stashes_columnHC = new DefaultOLVColumn()
        {
            Text = "HC",
            AspectName = "HC",
            Width = 40,
            IsEditable = true,
            CheckBoxes = true,
            TextAlign = HorizontalAlignment.Center,
        };

        #endregion

        private void InitializeStashesListView()
        {
            stashes_columnStashGroup.GroupKeyGetter = delegate (object row)
            {
                if (row is StashDummyObject)
                    return Global.Stashes.GetStashGroup(((StashDummyObject)row).GroupID);

                StashObject stash = (StashObject)row;
                StashGroupObject grp;
                if (stashes_dragHandler.IsDraggingStash(stash))
                {
                    grp = stashes_dragHandler.DropSink.OverStashGroup;
                    if (grp != null) return grp;
                }
                grp = Global.Stashes.GetStashGroup(stash.GroupID);
                return grp;
            };

            stashes_columnStashGroup.GroupFormatter = (OLVGroup group, GroupingParameters parms) =>
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

        }

        private void InitializeStashesPage()
        {
            InitializeStashesListView();


            Controls.PseudoScrollBarPanel scrollCover = new Controls.PseudoScrollBarPanel(stashes_listView)
            {
                BackColor = listViewBackColor,
                BarColor = scrollBarColor,
                BarWidth = SystemInformation.VerticalScrollBarWidth - 5
            };


            stashes_showExpansionComboBox.BorderColor = formBackColor;
            stashes_showExpansionComboBox.ButtonColor = passiveForeColor;
            stashes_showExpansionComboBox.BackColor = listViewBackColor;
            stashes_showExpansionComboBox.ForeColor = interactiveForeColor;
            stashes_showExpansionComboBox.BackColorHighlight = interactiveForeColor;
            stashes_showExpansionComboBox.ForeColorHighlight = listViewBackColor;

            stashes_listViewBorderPanel.BackColor = listViewBackColor;
            stashes_listViewBorderPanel.Padding = listViewBorderPadding;

            stashes_menuStrip.Renderer = new ToolStripRenderer();
            stashes_menuStrip.BackColor = toolStripBackColor;
            stashes_menuFlowLayoutPanel.BackColor = toolStripBackColor;

            InitializeToolStripButton(stashes_createStashButton,
                toolStripButtonBackColor, toolStripButtonBackColorHover,
                interactiveForeColor, interactiveForeColorHighlight);

            stashes_showSoftCoreCheckbox.ForeColor = passiveForeColor;
            stashes_showHardCoreCheckbox.ForeColor = passiveForeColor;
            stashes_shownItemsCountLabel.ForeColor = passiveForeColor;

            stashes_listView.RowHeight = listViewRowHeight;
            stashes_listView.BackColor = listViewBackColor;
            stashes_listView.GridLines = false;
            stashes_listView.HeaderMaximumHeight = listViewColumnsHeight;
            stashes_listView.HeaderMinimumHeight = listViewColumnsHeight;
            stashes_listView.HeaderUsesThemes = false;
            stashes_listView.HeaderFormatStyle = new HeaderFormatStyle();
            stashes_listView.HeaderFormatStyle.SetBackColor(listViewBackColor);
            stashes_listView.HeaderFormatStyle.SetForeColor(passiveForeColor);
            stashes_listView.HeaderFormatStyle.SetFont(new Font(stashes_listView.Font.FontFamily, listViewColumnsFontHeight, FontStyle.Regular));
            stashes_listView.UseCellFormatEvents = true;
            stashes_listView.MultiSelect = true;
            stashes_listView.PrimarySortColumn = stashes_columnOrder;
            stashes_listView.PrimarySortOrder = SortOrder.Ascending;
            stashes_listView.ShowGroups = true;
            stashes_listView.HasCollapsibleGroups = true;
            stashes_listView.SeparatorColor = listViewGroupHeaderSeparatorColor;
            stashes_listView.GroupHeadingForeColor = listViewGroupHeaderForeColor;
            stashes_listView.GroupHeadingBackColor = listViewGroupHeaderBackColor;
            stashes_listView.GroupHeadingFont = new Font(stashes_listView.Font.FontFamily, 10, FontStyle.Regular);
            stashes_listView.GroupHeadingCountForeColor = listViewGroupHeaderCountForeColor;
            stashes_listView.GroupHeadingCountFont = new Font(stashes_listView.Font.FontFamily, 8, FontStyle.Regular);
            stashes_listView.SpaceBetweenGroups = listViewGroupSpaceBetween;
            stashes_listView.AlwaysGroupByColumn = stashes_columnStashGroup;

            stashes_showExpansionComboBox.Items.AddRange(GrimDawnLib.GrimDawn.GetExpansionList()
                .Where(exp => exp != GrimDawnLib.GrimDawnGameExpansion.Unknown)
                .Select(exp => GrimDawnLib.GrimDawn.GetExpansionName(exp)).ToArray());

            SpaceClick += delegate { Stashes_UnselectAll(); };
            stashes_pagePanel.Click += delegate { Stashes_UnselectAll(); };
            stashes_listViewBorderPanel.Click += delegate { Stashes_UnselectAll(); };

            stashes_sortComparer = new StashesSortComparer((StashObject x, StashObject y, out int ret) =>
            {
                ret = 0;
                if (stashes_dragHandler.IsDragging)
                {
                    if (stashes_dragHandler.DropSink.OrderedList.Contains(x))
                        if (stashes_dragHandler.DropSink.OrderedList.Count > 0)
                        {
                            int s1i = stashes_dragHandler.DropSink.OrderedList.IndexOf(x);
                            int s2i = stashes_dragHandler.DropSink.OrderedList.IndexOf(y);
                            ret = s1i.CompareTo(s2i);
                            return true;
                        }
                }
                return false;
            });

            Load += delegate {

                stashes_shownExpansion = GrimDawnLib.GrimDawn.GetInstalledExpansionFromPath(Global.Configuration.Settings.GamePath);
                stashes_showExpansionComboBox.SelectedIndex = (int)stashes_shownExpansion;
                stashes_showSoftCoreCheckbox.CheckState = Global.Configuration.IntToCheckState(Global.Configuration.Settings.ShowSoftcoreState);
                stashes_showHardCoreCheckbox.CheckState = Global.Configuration.IntToCheckState(Global.Configuration.Settings.ShowHardcoreState);

                stashes_showExpansionComboBox.SelectionChangeCommitted += Stashes_ShowExpansionComboBox_SelectionChangeCommitted;
                stashes_showSoftCoreCheckbox.CheckStateChanged += Stashes_ShowSoftCoreCheckbox_CheckStateChanged;
                stashes_showHardCoreCheckbox.CheckStateChanged += Stashes_ShowHardCoreCheckbox_CheckStateChanged;

                stashes_listView.CellRightClick += Stashes_ListView_CellRightClick;
                stashes_listView.ColumnRightClick += Stashes_ListView_ColumnRightClick;
                stashes_listView.SubItemChecking += Stashes_ListView_SubItemChecking;
                stashes_listView.CellEditStarting += Stashes_ListView_CellEditStarting;
                stashes_listView.CellEditFinished += Stashes_ListView_CellEditFinished;
                stashes_listView.SelectedIndexChanged += Stashes_ListView_SelectedIndexChanged;
                stashes_listView.FormatCell += Stashes_ListView_FormatCell;
                stashes_listView.FormatRow += Stashes_ListView_FormatRow;
                stashes_listView.GroupExpandingCollapsing += Stashes_ListView_GroupExpandingCollapsing;
                stashes_listView.GroupExpandingCollapsing2 += Stashes_ListView_GroupExpandingCollapsing2;
                stashes_listView.BeforeCreatingGroups += Stashes_ListView_BeforeCreatingGroups;
                stashes_listView.MouseMove += delegate (object sen, MouseEventArgs eee) {
                    // this is used to refres the item the mouse is over
                    // needed to refresh checkbox images
                    OlvListViewHitTestInfo info = stashes_listView.MouseMoveHitTest;
                    if (info == null || info.Item == null) return;
                    OLVListSubItem subItem;
                    subItem = info.Item.GetSubItem(stashes_columnSC.Index);
                    if (subItem != null)
                    {
                        Point p = stashes_listView.PointToClient(Cursor.Position);
                        if (subItem.Bounds.Contains(p)) stashes_listView.RefreshItem(info.Item);
                    }
                    subItem = info.Item.GetSubItem(stashes_columnHC.Index);
                    if (subItem != null)
                    {
                        Point p = stashes_listView.PointToClient(Cursor.Position);
                        if (subItem.Bounds.Contains(p)) stashes_listView.RefreshItem(info.Item);
                    }
                };

                stashes_dragHandler = new Dragging.StashesDragHandler(stashes_listView);
                stashes_dragHandler.DragSource.DragEnd += Stashes_Dragging_DragEnd;
                stashes_listView.DragEnter += Stashes_ListView_DragEnter;
                stashes_listView.DragDrop += Stashes_ListView_DragDrop;

                Global.Runtime.ActiveStashChanged += delegate (object sender, GlobalHandlers.RuntimeHandler.ActiveStashChangedEventArgs e) {
                    stashes_listView.Invoke((MethodInvoker)delegate {
                        foreach (OLVListItem item in stashes_listView.Items)
                        {
                            StashObject stash = (StashObject)item.RowObject;
                            if (stash.ID == e.OldID || stash.ID == e.NewID)
                                stashes_listView.RefreshItem(item);
                        }
                    });
                };

                Global.Runtime.ActiveModeChanged += delegate { Stashes_ReloadList(); };
                Global.Runtime.StashesAdded += delegate (object sender, GlobalHandlers.RuntimeHandler.ListChangedEventArgs<StashObject> args)  { stashes_listView.AddObjects(args.List); };
                Global.Runtime.StashesRemoved += delegate (object sender, GlobalHandlers.RuntimeHandler.ListChangedEventArgs<StashObject> args)  { stashes_listView.RemoveObjects(args.List); };
                Global.Runtime.StashesUpdated += delegate (object sender, GlobalHandlers.RuntimeHandler.ListChangedEventArgs<StashObject> args) {
                    Stashes_UpdateObjects(args.List);
                };
                Global.Runtime.StashGroupsRebuild += delegate { Stashes_ReloadList(); };
                Global.Runtime.StashGroupsUpdated += delegate { Stashes_ReloadList(); }; // dont know how to trigger group header update
                Global.Runtime.StashGroupsAdded += delegate (object sender, GlobalHandlers.RuntimeHandler.ListChangedEventArgs<StashGroupObject> args) {
                    Stashes_ReloadGroupDummies();
                };
                Global.Runtime.StashGroupsRemoved += delegate (object sender, GlobalHandlers.RuntimeHandler.ListChangedEventArgs<StashGroupObject> args) {
                    Stashes_ReloadGroupDummies();
                };
                Global.Runtime.StashesImported += delegate (object sender, GlobalHandlers.RuntimeHandler.ListChangedEventArgs<StashObject> args) {
                    Stashes_UpdateObjects(args.List);
                };

                stashes_listView.AboutToCreateGroups += delegate (object s, CreateGroupsEventArgs args) {
                    // TODO: create sub for me
                    foreach (OLVGroup grp in args.Groups)
                    {
                        StashGroupObject stashGroup = (StashGroupObject)grp.Key;
                        grp.Collapsed = stashGroup.Collapsed;
                    }
                };

                // this is used to increase height of listvew group headers
                stashes_listView.GroupImageList = new ImageList() { ImageSize = new Size(1, listViewGroupHeaderHeight) };
                using (Bitmap bmp = new Bitmap(1, stashes_listView.GroupImageList.ImageSize.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
                using (Graphics gfx = Graphics.FromImage(bmp))
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(0, 0, 0, 0)))
                {
                    gfx.FillRectangle(brush, 0, 0, 1, bmp.Height);
                    stashes_listView.GroupImageList.Images.Add("height", bmp);
                }

                Stashes_ReloadColumns();
                Stashes_ReloadList();
            };
        }

        #region methods

        private void Stashes_UpdateObjects(IList<StashObject> l)
        {
            foreach (StashObject so in l)
                stashes_listView.UpdateObject(so);
        }

        private void Stashes_RefreshAllObjects()
        {
            foreach (OLVGroup grp in stashes_listView.OLVGroups)
                if (!grp.Collapsed)
                    foreach (OLVListItem li in grp.Items)
                        stashes_listView.RefreshItem(li);
        }

        public StashObject[] Stashes_GetSelectedObjects()
        {
            return stashes_listView.SelectedItems
                .Cast<OLVListItem>()
                .Select(l => (StashObject)l.RowObject)
                .ToArray();
        }

        public void Stashes_UnselectAll()
        {
            foreach (OLVListItem item in stashes_listView.SelectedItems)
            {
                item.Selected = false;
                stashes_listView.RefreshItem(item);
            }
            stashes_listView.CancelCellEdit();
            titlePanel.Focus();
        }

        private void Stashes_ReloadColumns()
        {
            stashes_listView.Columns.Clear();
            List<ColumnHeader> h = new List<ColumnHeader>();
            h.Add(stashes_columnActive);
            if (Global.Configuration.Settings.ShowIDColumn) h.Add(stashes_columnID);
            h.Add(stashes_columnName);
            if (Global.Configuration.Settings.ShowLastChangeColumn) h.Add(stashes_columnLastChange);
            h.Add(stashes_columnUsage);
            h.Add(stashes_columnSC);
            h.Add(stashes_columnHC);
            stashes_listView.Columns.AddRange(h.ToArray());
        }

        public void Stashes_ReloadList()
        {
            //if (!Visible) return;

            int scrollY = stashes_listView.LowLevelScrollPosition.Y;

            stashes_listView.ClearObjects();

            int _sc = Global.Configuration.Settings.ShowSoftcoreState;
            int _hc = Global.Configuration.Settings.ShowHardcoreState;
            int displayCount = 0;
            int totalCount = 0;

            stashes_listView.SetObjects(Array.FindAll(Global.Stashes.GetAllStashes(), delegate (StashObject stash) {
                if (stashes_shownExpansion == stash.Expansion)
                {
                    totalCount += 1;
                    if (!(stash.SC && _sc == 0 || !stash.SC && _sc == 1 || stash.HC && _hc == 0 || !stash.HC && _hc == 1))
                    {
                        displayCount += 1;
                        return true;
                    }
                }
                return false;
            }));

            stashes_shownItemsCountLabel.Text = Global.L.ShownStashesLabel(displayCount.ToString(), totalCount.ToString());

            Stashes_ReloadGroupDummies();
            Groups_RefreshAllObjects();

            stashes_listView.Sort();
            stashes_listView.LowLevelScroll(0, scrollY);
        }

        private void Stashes_ReloadGroupDummies()
        {
            stashes_listView.RemoveObjects(stashes_listView.Items
                .Cast<OLVListItem>()
                .Select(item => item.RowObject)
                .Where(item => item is StashDummyObject)
                .ToList()
            );
            stashes_listView.AddObjects(
                Global.Stashes.GetAllStashGroups()
                .ToList()
                .Select(s => new StashDummyObject(null, s.ID))
                .ToList()
            );
        }

        #endregion

        #region events

        private void Stashes_ListView_BeforeCreatingGroups(object s, CreateGroupsEventArgs args)
        {
            args.Parameters.ItemComparer = new ObjectListViewSortComparer<StashObject>(stashes_sortComparer);
        }

        private void Stashes_ListView_GroupExpandingCollapsing(object s, GroupExpandingCollapsingEventArgs args)
        {
            StashGroupObject stashGroup = (StashGroupObject)args.Group.Key;
            stashGroup.Collapsed = !args.IsExpanding;
            Global.Configuration.Save();
        }

        private void Stashes_ListView_GroupExpandingCollapsing2(object s, GroupExpandingCollapsingEventArgs args)
        {
            StashGroupObject stashGroup = (StashGroupObject)args.Group.Key;
            stashGroup.Collapsed = args.Group.Collapsed;
            //stashes_listView.CancelCellEdit();
            Global.Configuration.Save();
        }

        private void Stashes_ListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (OLVListItem item in stashes_listView.Items)
                if (item.RowObject is StashDummyObject)
                    item.Selected = false;
        }

        private void Stashes_ListView_FormatCell(object sender, FormatCellEventArgs e)
        {
            if (e.Model is StashDummyObject) return;

            if (e.ColumnIndex == stashes_columnName.Index)
            {
                StashObject stash = (StashObject)e.Model;
                bool isMain = Global.Configuration.IsMainStashID(stash.ID);
                bool isDragging = stashes_dragHandler.IsDraggingStash(stash);
                bool isSelected = e.Item.Selected;
                bool isActive = Global.Runtime.ActiveStashID == stash.ID;
                bool isLocked = stash.Locked;

                FontStyle fontStyle = FontStyle.Regular;
                if (isMain) fontStyle |= FontStyle.Italic;
                if (isActive) fontStyle |= FontStyle.Bold;

                e.SubItem.Font = new Font(e.Item.Font, fontStyle);
            }
        }

        private void Stashes_ListView_FormatRow(object sender, FormatRowEventArgs e)
        {
            OLVListSubItem subItem;

            if (e.Model is StashDummyObject)
            {
                e.Item.BackColor = listViewItemBackColor;
                e.Item.ForeColor = passiveForeColor;
                e.Item.Font = new Font(stashes_listView.Font.FontFamily, listViewColumnsFontHeight);
                subItem = e.Item.GetSubItem(stashes_columnSC.Index);
                if (subItem != null) subItem.Decorations.Add(chkHideDecoration);
                subItem = e.Item.GetSubItem(stashes_columnHC.Index);
                if (subItem != null) subItem.Decorations.Add(chkHideDecoration);
                return;
            }

            int siIndex = 0;
            foreach (OLVListSubItem si in e.Item.SubItems)
            {
                si.Decoration = siIndex == 0 ? cellBorderFirstDecoration : cellBorderDecoration;
                siIndex += 1;
            }

            StashObject stash = (StashObject)e.Model;
            bool isMain = Global.Configuration.IsMainStashID(stash.ID);
            bool isDragging = stashes_dragHandler.IsDraggingStash(stash);
            bool isSelected = e.Item.Selected;
            bool isActive = Global.Runtime.ActiveStashID == stash.ID;
            bool isLocked = stash.Locked;

            subItem = e.Item.GetSubItem(stashes_columnName.Index);
            if (subItem != null)
            {
                if (isLocked)
                {
                    subItem.Decorations.Add(lockDecoration);
                }
            }

            subItem = e.Item.GetSubItem(stashes_columnSC.Index);
            if (subItem != null)
            {
                Rectangle bounds = new Rectangle(
                    subItem.Bounds.X + (subItem.Bounds.Width / 2) - 7,
                    subItem.Bounds.Y + (subItem.Bounds.Height / 2) - 8,
                    15, 15);
                Point p = stashes_listView.PointToClient(Cursor.Position);
                bool hit = bounds.Contains(p);

                subItem.Decorations.Add(isMain
                    ? chkBackDecoration
                    : (hit
                        ? chkBackHoverDecoration
                        : chkBackDecoration
                    ));
                subItem.Decorations.Add(isMain
                    ? (stash.SC
                        ? chkTickDisabledDecoration
                        : chkCrossDisabledDecoration
                        )
                    : (stash.SC
                        ? chkTickDecoration
                        : chkCrossDecoration
                    ));
            }
            subItem = e.Item.GetSubItem(stashes_columnHC.Index);
            if (subItem != null)
            {
                Rectangle bounds = new Rectangle(
                    subItem.Bounds.X + (subItem.Bounds.Width / 2) - 7,
                    subItem.Bounds.Y + (subItem.Bounds.Height / 2) - 8,
                    15, 15);
                Point p = stashes_listView.PointToClient(Cursor.Position);
                bool hit = bounds.Contains(p);

                subItem.Decorations.Add(isMain
                    ? chkBackDecoration
                    : (hit
                        ? chkBackHoverDecoration
                        : chkBackDecoration
                    ));
                subItem.Decorations.Add(isMain
                    ? (stash.HC
                        ? chkTickDisabledDecoration
                        : chkCrossDisabledDecoration
                        )
                    : (stash.HC
                        ? chkTickDecoration
                        : chkCrossDecoration
                    ));
            }

            if (isDragging)
            {
                e.Item.BackColor = Color.Teal;
                e.Item.ForeColor = Color.WhiteSmoke;
            }
            else
            {
                e.Item.BackColor = listViewItemBackColor;
                e.Item.ForeColor = stash.GetDisplayColor();
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

        private void Stashes_ListView_ColumnRightClick(object sender, ColumnClickEventArgs e)
        {
            if (!Native.GetCursorPos(out Native.POINT p)) return;
            ContextMenuStrip menu = new ContextMenuStrip()
            {
                Width = 200,
            };
            ToolStripCheckedListBox checkedList = new ToolStripCheckedListBox()
            {
            };
            checkedList.AddItem(stashes_columnID.Text, Global.Configuration.Settings.ShowIDColumn);
            checkedList.AddItem(stashes_columnLastChange.Text, Global.Configuration.Settings.ShowLastChangeColumn);
            checkedList.ItemCheck += delegate (object s, ItemCheckEventArgs f) {
                bool chckd = f.NewValue == CheckState.Checked;
                switch (f.Index)
                {
                    case 0: Global.Configuration.Settings.ShowIDColumn = chckd; break;
                    case 1: Global.Configuration.Settings.ShowLastChangeColumn = chckd; break;
                }
                Stashes_ReloadColumns();
                Stashes_ReloadList();
                Global.Configuration.Save();
            };

            menu.Items.Insert(menu.Items.Count, checkedList);
            menu.Show(p.X, p.Y);
        }

        private void Stashes_ListView_CellRightClick(object sender, CellRightClickEventArgs args)
        {
            if (!(args.Model is StashObject stash)) return;
            if (stash is StashDummyObject) return;

            ContextMenuStrip menu = new ContextMenuStrip();

            StashObject[] selectedItems = Stashes_GetSelectedObjects();

            if (selectedItems.Length == 1)
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
                menu.Items.Insert(0, new ToolStripLabel(string.Format("({0})", selectedItems.Length))
                {
                    ForeColor = Color.Gray,
                });
            }

            menu.Items.Add(new ToolStripSeparator());
            {
                menu.Items.Add(Global.L.ColorButton());
                ToolStripMenuItem btn = menu.Items[menu.Items.Count - 1] as ToolStripMenuItem;
                foreach (Common.Config.ConfigColor col in Global.Configuration.Colors)
                {
                    string colorName = col.Name != "" ? col.Name : col.Value;
                    ToolStripMenuItem mi = new ToolStripMenuItem(colorName, null, delegate (object s, EventArgs e)
                    {
                        foreach (StashObject st in selectedItems)
                            st.TextColor = col.Value;
                        Global.Configuration.Save();
                        Stashes_UnselectAll();
                        Global.Runtime.NotifyStashesUpdated(selectedItems);
                    });
                    mi.BackColor = Color.FromArgb(0, 0, 0);
                    try
                    {
                        Color cFore = ColorTranslator.FromHtml(col.Value);
                        mi.ForeColor = cFore;
                        mi.MouseEnter += delegate { mi.ForeColor = Color.Black; };
                        mi.MouseLeave += delegate { mi.ForeColor = cFore; };
                        btn.DropDownItems.Add(mi);
                    }
                    catch (Exception)
                    {
                    }
                }
                if (btn.DropDownItems.Count == 0)
                {
                    //TODO?
                }
            }

            if (selectedItems.Length == 1)
            {
                string lockStr = stash.Locked ? Global.L.UnlockButton() : Global.L.LockButton();
                menu.Items.Add(lockStr, Properties.Resources.lockedBlack, delegate (object s, EventArgs e) {
                    stash.Locked = !stash.Locked;
                    Stashes_UnselectAll();
                    Global.Runtime.NotifyStashesUpdated(selectedItems);
                });
            }
            else
            {
                menu.Items.Add(Global.L.LockButton(), Properties.Resources.lockedBlack, delegate (object s, EventArgs e) {
                    foreach (StashObject selStash in selectedItems) selStash.Locked = true;
                    Stashes_UnselectAll();
                    Global.Runtime.NotifyStashesUpdated(selectedItems);
                });
                menu.Items.Add(Global.L.UnlockButton(), Properties.Resources.lockedBlack, delegate (object s, EventArgs e) {
                    foreach (StashObject selStash in selectedItems) selStash.Locked = false;
                    Stashes_UnselectAll();
                    Global.Runtime.NotifyStashesUpdated(selectedItems);
                });
            }

            if (selectedItems.Length == 1)
            {
                menu.Items.Add(Global.L.RenameButton(), null, delegate (object s, EventArgs e) {
                    OLVListItem item = (OLVListItem)stashes_listView.SelectedItems[0];
                    stashes_listView.EditSubItem(item, stashes_columnName.Index);
                });
            }

            menu.Items.Add(new ToolStripSeparator());

            if (selectedItems.Length == 1)
            {
                menu.Items.Add(Global.L.RestoreBackupButton());
                ToolStripMenuItem restoreButtn = menu.Items[menu.Items.Count - 1] as ToolStripMenuItem;
                restoreButtn.DropDownItems.AddRange(Array.ConvertAll<string, ToolStripItem>(Global.Stashes.GetBackupFiles(stash.ID), delegate (string file) {
                    string fileName = System.IO.Path.GetFileName(file);
                    string fileDate = System.IO.File.GetLastWriteTime(file).ToString();
                    TransferFile transferFile = Common.TransferFile.FromFile(file);
                    transferFile.LoadUsage();
                    string itemText = string.Format("{0} - {1} - {2}", fileName, fileDate, transferFile.TotalUsageText);
                    return new ToolStripMenuItem(itemText, null, delegate (object s, EventArgs e) {
                        Global.Stashes.RestoreTransferFile(stash.ID, file);
                        stash.LoadTransferFile();
                        Global.Runtime.ReloadOpenedStash(stash.ID);
                        Global.Runtime.NotifyStashesRestored(stash);
                        Stashes_UnselectAll();
                    });
                }));
                if (restoreButtn.DropDownItems.Count == 0)
                {
                    restoreButtn.DropDownItems.Insert(0, new ToolStripMenuItem(Global.L.NoBackupsLabel())
                    {
                        ForeColor = Color.Gray
                    });
                }
            }

            menu.Items.Add(Global.L.ExportButton(), null, delegate (object s, EventArgs e) {

                StashesZipFile zipFile = new StashesZipFile();
                foreach (StashObject selStash in selectedItems) zipFile.AddStash(selStash);

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

            });

            if (selectedItems.Length == 1)
            {
                menu.Items.Add(Global.L.OverwriteButton(), null, delegate (object s, EventArgs e) {
                    OLVListItem item = (OLVListItem)stashes_listView.SelectedItems[0];
                    DialogResult result = GrimDawnLib.GrimDawn.ShowSelectTransferFilesDialog(out string[] files, false, true);
                    if (result == DialogResult.OK)
                    {
                        if (Global.Stashes.ImportOverwriteStash(files[0], stash))
                        {
                            Global.Runtime.NotifyStashesUpdated(stash);
                            Global.Runtime.ReloadOpenedStash(stash.ID);
                        }
                    }
                });
            }


            if (stashes_shownExpansion != GrimDawnLib.GrimDawn.LatestExpansion)
            {
                menu.Items.Add(Global.L.CopyToExpansionButton());
                ToolStripMenuItem btn = menu.Items[menu.Items.Count - 1] as ToolStripMenuItem;
                for (int i = (int)stashes_shownExpansion + 1; i <= (int)GrimDawnLib.GrimDawn.LatestExpansion; i += 1)
                {
                    GrimDawnLib.GrimDawnGameExpansion exp = (GrimDawnLib.GrimDawnGameExpansion)i;
                    btn.DropDownItems.Add(GrimDawnLib.GrimDawn.GetExpansionName(exp), null, delegate (object s, EventArgs e) {

                        foreach (var st in selectedItems)
                        {
                            StashObject copied = Global.Stashes.CreateStashCopy(st);
                            copied.Expansion = exp;
                        }
                        if (Console.Confirm(Global.L.ConfirmDeleteOldStashesMessage()))
                        {
                            Global.Stashes.DeleteStashes(selectedItems);
                        }
                        Global.Configuration.Save();
                        Stashes_ReloadList();
                        Global.Runtime.NotifyStashesRebuild();
                    });
                }
            }





            menu.Items.Add(new ToolStripSeparator());

            menu.Items.Add(Global.L.DeleteButton(), null, delegate (object s, EventArgs e) {
                if (Global.Configuration.Settings.ConfirmStashDelete && !Console.Confirm(Global.L.ConfirmDeleteStashesMessage())) return;

                List<StashObject> deletedItems = Global.Stashes.DeleteStashes(selectedItems);
                Global.Configuration.Save();
                Global.Runtime.NotifyStashesRemoved(deletedItems);
            });

            args.MenuStrip = menu;
        }

        private void Stashes_ListView_SubItemChecking(object sender, SubItemCheckingEventArgs args)
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
                    Invoke((MethodInvoker) delegate {
                        stashes_listView.RemoveObject(stash);
                    });
                }).Start();
            }
        }

        private void Stashes_ListView_CellEditStarting(object sender, CellEditEventArgs e)
        {
            if (e.RowObject is StashDummyObject)
                e.Cancel = true;
        }

        private void Stashes_ListView_CellEditFinished(object sender, CellEditEventArgs args)
        {
            if (args.RowObject is StashDummyObject) return;

            StashObject stash = (StashObject)args.RowObject;
            if (args.Column == stashes_columnName) Global.Runtime.NotifyStashesUpdated(stash);
            else return;
            Global.Configuration.Save();
            Stashes_UnselectAll();
        }

        private void Stashes_ListView_DragEnter(object sender, DragEventArgs e)
        {
            if (stashes_dragHandler.IsDragging)
                e.Effect = DragDropEffects.None;
            else
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                    e.Effect = DragDropEffects.Copy;
        }

        private void Stashes_ListView_DragDrop(object sender, DragEventArgs e)
        {
            if (stashes_dragHandler.IsDragging) return;
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            Global.Windows.ShowImportDialog(files);
        }

        private void Stashes_Dragging_DragEnd(object sender, EventArgs e)
        {
            Stashes_UnselectAll();
            Groups_RefreshAllObjects();
            Global.Configuration.Save();
            Global.Runtime.NotifyStashesRebuild();
        }

        private void Stashes_ShowExpansionComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            stashes_shownExpansion = (GrimDawnLib.GrimDawnGameExpansion)stashes_showExpansionComboBox.SelectedIndex;
            Stashes_ReloadList();
        }

        private void Stashes_ShowSoftCoreCheckbox_CheckStateChanged(object sender, EventArgs e)
        {
            Global.Configuration.Settings.ShowSoftcoreState = Global.Configuration.CheckStateToInt(stashes_showSoftCoreCheckbox.CheckState);
            Global.Configuration.Save();
            Stashes_ReloadList();
        }

        private void Stashes_ShowHardCoreCheckbox_CheckStateChanged(object sender, EventArgs e)
        {
            Global.Configuration.Settings.ShowHardcoreState = Global.Configuration.CheckStateToInt(stashes_showHardCoreCheckbox.CheckState);
            Global.Configuration.Save();
            Stashes_ReloadList();
        }

        private void Stashes_CreateStashButton_Click(object sender, EventArgs e)
        {
            Global.Windows.ShowCreateStashDialog(stashes_shownExpansion);
        }

        #endregion

    }
}
