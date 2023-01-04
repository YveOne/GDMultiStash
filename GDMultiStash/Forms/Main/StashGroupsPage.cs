﻿using System;
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
    internal partial class StashGroupsPage : Page
    {

        private Dragging.GroupsDragHandler dragHandler;
        private GroupsSortComparer sortComparer;

        #region columns

        private readonly OLVColumn columnSpacer = new DefaultOLVColumn()
        { // just is just a spacer to match stashes listview first columns
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

        #endregion

        public StashGroupsPage(MainForm mainForm) : base(mainForm)
        {
            InitializeComponent();

            PseudoScrollBarPanel scrollCover = new PseudoScrollBarPanel(groupsListView)
            {
                BackColor = Constants.ListViewBackColor,
                BarColor = Constants.ScrollBarColor,
                BarWidth = SystemInformation.VerticalScrollBarWidth - 5
            };

            listViewBorderPanel.BackColor = Constants.ListViewBackColor;
            listViewBorderPanel.Padding = Constants.ListViewBorderPadding;

            menuStrip.Renderer = new FlatToolStripRenderer();
            menuStrip.BackColor = Constants.ToolStripBackColor;

            Main.InitializeToolStripButton(createStashGroupButton,
                Constants.ToolStripButtonBackColor, Constants.ToolStripButtonBackColorHover,
                Constants.InteractiveForeColor, Constants.InteractiveForeColorHighlight);

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

            groupsListView.RowHeight = Constants.ListViewGroupHeaderHeight + 3;
            groupsListView.BackColor = Constants.ListViewBackColor;
            groupsListView.GridLines = false;
            groupsListView.HeaderMaximumHeight = Constants.ListViewColumnsHeight;
            groupsListView.HeaderMinimumHeight = Constants.ListViewColumnsHeight;
            groupsListView.HeaderUsesThemes = false;
            groupsListView.HeaderFormatStyle = new HeaderFormatStyle();
            groupsListView.HeaderFormatStyle.SetBackColor(Constants.ListViewBackColor);
            groupsListView.HeaderFormatStyle.SetForeColor(Constants.PassiveForeColor);
            groupsListView.HeaderFormatStyle.SetFont(new Font(groupsListView.Font.FontFamily, Constants.ListViewColumnsFontHeight, FontStyle.Regular));
            groupsListView.UseCellFormatEvents = true;
            groupsListView.MultiSelect = true;
            groupsListView.FullRowSelect = true;
            groupsListView.ShowGroups = false;
            groupsListView.PrimarySortColumn = columnOrder;
            groupsListView.PrimarySortOrder = SortOrder.Ascending;
            groupsListView.CustomSorter = delegate (OLVColumn column, SortOrder order) {
                groupsListView.ListViewItemSorter = new ObjectListViewSortComparer<StashGroupObject>(sortComparer);
            };

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

                Global.Runtime.StashesAdded += delegate (object sender, GlobalHandlers.RuntimeHandler.ListChangedEventArgs<StashObject> args) { RefreshStashesObjects(args.List); };
                Global.Runtime.StashesRemoved += delegate (object sender, GlobalHandlers.RuntimeHandler.ListChangedEventArgs<StashObject> args) { RefreshStashesObjects(args.List); };
                Global.Runtime.StashGroupsAdded += delegate (object sender, GlobalHandlers.RuntimeHandler.ListChangedEventArgs<StashGroupObject> args) { groupsListView.AddObjects(args.List); };
                Global.Runtime.StashGroupsRemoved += delegate (object sender, GlobalHandlers.RuntimeHandler.ListChangedEventArgs<StashGroupObject> args) { groupsListView.RemoveObjects(args.List); };
                Global.Runtime.StashesRebuild += delegate (object sender, EventArgs args) { RefreshAllObjects(); };

                ReloadColumns();
                ReloadList();
            };
        }

        protected override void Localize(GlobalHandlers.LocalizationHandler.StringsHolder L)
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
                .Select(id => Global.Stashes.GetStashGroup(id))
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
                columnName
            });
        }

        public void ReloadList()
        {
            //if (!Visible) return;

            int scrollY = groupsListView.TopItemIndex;
            groupsListView.ClearObjects();
            groupsListView.SetObjects(Global.Stashes.GetAllStashGroups());
            groupsListView.Sort();
            groupsListView.TopItemIndex = scrollY;
        }

        #endregion

        #region events

        private void CreateStashGroupButton_Click(object sender, EventArgs e)
        {
            Global.Windows.ShowCreateStashGroupDialog();
        }

        private void GroupsListView_FormatCell(object sender, FormatCellEventArgs e)
        {
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
                e.Item.BackColor = Constants.ListViewGroupHeaderBackColor;
                e.Item.ForeColor = Constants.ListViewGroupHeaderForeColor;
            }
        }

        private void GroupsListView_CellEditFinished(object sender, CellEditEventArgs args)
        {
            StashGroupObject group = (StashGroupObject)args.RowObject;
            if (args.Column == columnName) Global.Runtime.NotifyStashGroupsUpdated(group);
            else return;
            Global.Configuration.Save();
            UnselectAll();
        }

        private void DragHandler_DragEnd(object sender, EventArgs e)
        {
            UnselectAll();
            Global.Configuration.Save();
            Global.Runtime.NotifyStashGroupsRebuild();
        }

        private void GroupsListView_CellRightClick(object sender, CellRightClickEventArgs args)
        {
            if (!(args.Model is StashGroupObject group)) return;

            ContextMenuStrip menu = new ContextMenuStrip();

            StashGroupObject[] selectedItems = GetSelectedObjects();

            // used to escape & sign for toolstrip item text
            string T(string t) => t.Replace("&", "&&");

            menu.Items.Insert(0, new ToolStripLabel(
                selectedItems.Length == 1
                ? T($"#{group.ID} {group.Name}")
                : T($"({selectedItems.Length})")
            ) { ForeColor = Color.Gray });

            if (selectedItems.Length == 1)
            {
                menu.Items.Add(T(Global.L.RenameButton()), null, delegate (object s, EventArgs e) {
                    OLVListItem item = (OLVListItem)groupsListView.SelectedItems[0];
                    groupsListView.EditSubItem(item, columnName.Index);
                });
            }

            menu.Items.Add(new ToolStripSeparator());

            menu.Items.Add(T(Global.L.DeleteButton()), null, delegate (object s, EventArgs e) {
                if (Global.Configuration.Settings.ConfirmStashDelete && MessageBox.Show(Global.L.ConfirmDeleteStashGroupsMessage(), "", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK) return;

                List<StashGroupObject> deletedItems = Global.Stashes.DeleteStashGroups(selectedItems);
                Global.Configuration.Save();
                Global.Runtime.NotifyStashGroupsRemoved(deletedItems);
            });

            args.MenuStrip = menu;
        }

        #endregion

    }
}