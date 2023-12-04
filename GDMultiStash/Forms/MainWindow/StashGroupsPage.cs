using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using BrightIdeasSoftware;

using GDMultiStash.Common;
using GDMultiStash.Common.Objects;
using GDMultiStash.Common.Objects.Sorting.Comparer;

using GDMultiStash.Forms.Controls;
using System.ComponentModel;

namespace GDMultiStash.Forms.MainWindow
{
    [DesignerCategory("code")]
    internal partial class StashGroupsPage : Page
    {

        private Dragging.GroupsDragHandler dragHandler;
        private GroupsSortComparer sortComparer;

        #region columns

        private readonly OLVColumn columnSpacer = new DefaultOLVColumn()
        { // just a spacer to match stashes listview first columns
            Text = "",
            Width = 30,
        };

        private readonly OLVColumn columnOrder = new DefaultOLVColumn()
        {
            AspectName = "Order",
            Width = 50,
            Sortable = true,
        };

        private readonly OLVColumn columnID = new DefaultOLVColumn()
        {
            AspectName = "ID",
            Width = 50,
            TextAlign = HorizontalAlignment.Left,
        };

        private readonly OLVColumn columnName = new DefaultOLVColumn()
        {
            AspectName = "Name",
            Width = 100,
            MaximumWidth = -1,
            IsEditable = true,
            FillsFreeSpace = true,
            CellEditUseWholeCell = true,
        };

        private readonly OLVColumn columnStashesCount = new DefaultOLVColumn()
        {
            Text = "",
            AspectName = "StashesCount",
            Width = 60,
            TextAlign = HorizontalAlignment.Right,
        };

        #endregion

        public StashGroupsPage(MainForm mainForm) : base(mainForm)
        {
            InitializeComponent();

            PseudoScrollBarPanel scrollCover = new PseudoScrollBarPanel(groupsListView)
            {
                BackColor = C.ListViewBackColor,
                BarColor = C.ScrollBarColor,
                BarWidth = SystemInformation.VerticalScrollBarWidth - 5
            };

            listViewBorderPanel.BackColor = C.ListViewBackColor;
            listViewBorderPanel.Padding = C.ListViewBorderPadding;

            menuStrip.Renderer = new DefaultContentMenuStripRenderer();
            menuStrip.BackColor = C.ToolStripBackColor;

            sortComparer = new GroupsSortComparer(delegate (StashGroupObject x, StashGroupObject y, out int ret)
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

            groupsListView.RowHeight = C.ListViewGroupHeaderHeight + 3;
            groupsListView.BackColor = C.ListViewBackColor;
            groupsListView.GridLines = false;
            groupsListView.HeaderMaximumHeight = C.ListViewColumnsHeight;
            groupsListView.HeaderMinimumHeight = C.ListViewColumnsHeight;
            groupsListView.HeaderUsesThemes = false;
            groupsListView.HeaderFormatStyle = new HeaderFormatStyle();
            groupsListView.HeaderFormatStyle.SetBackColor(C.ListViewBackColor);
            groupsListView.HeaderFormatStyle.SetForeColor(C.PassiveForeColor);
            groupsListView.HeaderFormatStyle.SetFont(new Font(groupsListView.Font.FontFamily, C.ListViewColumnsFontHeight, FontStyle.Regular));
            groupsListView.UseCellFormatEvents = true;
            groupsListView.MultiSelect = true;
            groupsListView.FullRowSelect = true;
            groupsListView.ShowGroups = false;
            groupsListView.PrimarySortColumn = columnOrder;
            groupsListView.PrimarySortOrder = SortOrder.Ascending;
            groupsListView.CustomSorter = delegate (OLVColumn column, SortOrder order) {
                groupsListView.ListViewItemSorter = new ObjectListViewSortComparer<StashGroupObject>(sortComparer);
            };

            GotFocus += delegate { groupsListView.Focus(); };
            Main.SpaceClick += delegate { UnselectAll(); };
            Main.Click += delegate { UnselectAll(); };
            listViewBorderPanel.Click += delegate { UnselectAll(); };

            Load += delegate {

                groupsListView.CellRightClick += GroupsListView_CellRightClick;
                groupsListView.CellEditFinished += GroupsListView_CellEditFinished;
                groupsListView.FormatCell += GroupsListView_FormatCell;
                groupsListView.FormatRow += GroupsListView_FormatRow;

                dragHandler = new Dragging.GroupsDragHandler(groupsListView);
                dragHandler.DragSource.DragEnd += DragHandler_DragEnd;
                //stashes_listView.DragEnter += Stashes_Dragging_DragEnter;
                //stashes_listView.DragDrop += Stashes_Dragging_DragDrop;

                G.Stashes.StashesAdded += delegate (object sender, Global.Stashes.StashObjectsEventArgs args) { RefreshStashesObjects(args.Items); };
                G.Stashes.StashesRemoved += delegate (object sender, Global.Stashes.StashObjectsEventArgs args) { RefreshAllObjects(); };
                G.Stashes.StashesMoved += delegate (object sender, Global.Stashes.StashObjectsEventArgs args) { RefreshAllObjects(); };
                G.StashGroups.StashGroupsAdded += delegate (object sender, Global.StashGroups.StashGroupObjectsEventArgs args) { groupsListView.AddObjects(args.Items); };
                G.StashGroups.StashGroupsRemoved += delegate (object sender, Global.StashGroups.StashGroupObjectsEventArgs args) { groupsListView.RemoveObjects(args.Items); };

                ReloadColumns();
                ReloadList();
            };
        }

        protected override void Localize(Global.LocalizationManager.StringsHolder L)
        {
            createStashGroupButton.Text = L.CreateGroupButton();
            columnID.Text = L.IdColumn();
            columnName.Text = L.NameColumn();
        }

        #region public methods

        public void UnselectAll()
        {
            foreach (OLVListItem item in groupsListView.SelectedItems)
            {
                item.Selected = false;
                groupsListView.RefreshItem(item);
            }
            groupsListView.CancelCellEdit();
        }

        public void RefreshAllObjects()
        {
            foreach (OLVListItem li in groupsListView.Items)
                groupsListView.RefreshItem(li);
        }

        public void RefreshStashesObjects(IEnumerable<StashObject> stashes)
        {
            groupsListView.RefreshObjects(stashes
                .Select(stash => stash.GroupID)
                .Distinct() // remove duplicates
                .Select(id => G.StashGroups.GetGroup(id)) // todo: check for !null
                .ToArray()
            );
        }

        public StashGroupObject[] GetSelectedObjects()
        {
            List<StashGroupObject> objects = new List<StashGroupObject>();
            foreach (OLVListItem item in groupsListView.SelectedItems)
                objects.Add((StashGroupObject)item.RowObject);
            return objects.ToArray();
        }

        #endregion

        #region private methods

        private void ReloadColumns()
        {
            groupsListView.Columns.Clear();
            groupsListView.Columns.AddRange(new ColumnHeader[]
            {
                columnSpacer,
                columnID,
                columnName,
                columnStashesCount
            });
        }

        public void ReloadList()
        {
            //if (!Visible) return;

            int scrollY = groupsListView.TopItemIndex;
            groupsListView.ClearObjects();
            groupsListView.SetObjects(G.StashGroups.GetAllGroups());
            groupsListView.Sort();
            groupsListView.TopItemIndex = scrollY;
        }

        #endregion

        #region events

        private void CreateStashGroupButton_Click(object sender, EventArgs e)
        {
            G.Windows.ShowCreateStashGroupDialog();
        }

        private void GroupsListView_FormatCell(object sender, FormatCellEventArgs e)
        {
            if (e.ColumnIndex == columnStashesCount.Index)
            {
                e.Item.SubItems[columnStashesCount.Index].Font = new Font(e.Item.Font.FontFamily, 8);
            }
        }

        private void GroupsListView_FormatRow(object sender, FormatRowEventArgs e)
        {
            //OLVListSubItem subItem;

            int siIndex = 0;
            foreach (OLVListSubItem si in e.Item.SubItems)
            {
                si.Decoration = siIndex == 0
                    ? Main.Decorations.RowBorderFirstDecoration
                    : Main.Decorations.RowBorderDecoration;
                siIndex += 1;
            }

            StashGroupObject group = (StashGroupObject)e.Model;
            bool isDragging = dragHandler.IsDraggingGroup(group);
            bool isSelected = e.Item.Selected;

            if (isDragging)
            {
                e.Item.BackColor = Color.Teal;
                e.Item.ForeColor = Color.WhiteSmoke;
            }
            else
            {
                e.Item.BackColor = C.ListViewGroupHeaderBackColor;
                e.Item.ForeColor = C.ListViewGroupHeaderForeColor;
            }
        }

        private void GroupsListView_CellEditFinished(object sender, CellEditEventArgs args)
        {
            StashGroupObject group = (StashGroupObject)args.RowObject;
            if (args.Column == columnName) G.StashGroups.InvokeStashGroupsInfoChanged(group);
            else return;
            G.Configuration.Save();
            UnselectAll();
        }

        private void DragHandler_DragEnd(object sender, EventArgs e)
        {
            UnselectAll();
            G.Configuration.Save();
            G.StashGroups.InvokeStashGroupsMoved(dragHandler.DragSource.Items);
        }

        private void GroupsListView_CellRightClick(object sender, CellRightClickEventArgs args)
        {
            if (!(args.Model is StashGroupObject group)) return;


            var menu = new ContextMenues.StashGroupsPageContextMenu(this, args);

            menu.Closed += delegate {
                UnselectAll();
            };

            menu.AddDeleteOption();

            args.MenuStrip = menu;
        }

        #endregion

    }
}
