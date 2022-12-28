using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using BrightIdeasSoftware;

using GDMultiStash.Common;
using GDMultiStash.Common.Objects;
using GDMultiStash.Common.Objects.Sorting;

namespace GDMultiStash.Forms
{
    internal partial class MainForm
    {
        private Dragging.GroupsDragHandler groups_dragHandler;
        private GroupsSortComparer groups_sortComparer;

        #region columns

        private readonly OLVColumn groups_columnSpacer = new DefaultOLVColumn()
        { // just is just a spacer to match stashes listview first columns
            Text = "",
            Width = 30,
        };

        private readonly OLVColumn groups_columnOrder = new DefaultOLVColumn()
        {
            AspectName = "Order",
            Width = 50,
            Sortable = true,
        };

        private readonly OLVColumn groups_columnID = new DefaultOLVColumn()
        {
            AspectName = "ID",
            Width = 50,
            TextAlign = HorizontalAlignment.Left,
        };

        private readonly OLVColumn groups_columnName = new DefaultOLVColumn()
        {
            AspectName = "Name",
            Width = 100,
            MaximumWidth = -1,
            IsEditable = true,
            FillsFreeSpace = true,
            CellEditUseWholeCell = true,
        };

        #endregion

        private void InitializeGroupsPage()
        {

            Controls.PseudoScrollBarPanel scrollCover = new Controls.PseudoScrollBarPanel(groups_listView)
            {
                BackColor = listViewBackColor,
                BarColor = scrollBarColor,
                BarWidth = SystemInformation.VerticalScrollBarWidth - 5
            };

            groups_listViewBorderPanel.BackColor = listViewBackColor;
            groups_listViewBorderPanel.Padding = listViewBorderPadding;

            groups_menuStrip.Renderer = new ToolStripRenderer();
            groups_menuStrip.BackColor = toolStripBackColor;

            InitializeToolStripButton(groups_createStashGroupButton,
                toolStripButtonBackColor, toolStripButtonBackColorHover,
                interactiveForeColor, interactiveForeColorHighlight);

            groups_sortComparer = new GroupsSortComparer(delegate (StashGroupObject x, StashGroupObject y, out int ret)
            {
                ret = 0;
                if (groups_dragHandler.IsDragging)
                {
                    if (groups_dragHandler.DropSink.OrderedList.Contains(x))
                        if (groups_dragHandler.DropSink.OrderedList.Count > 0)
                        {
                            int s1i = groups_dragHandler.DropSink.OrderedList.IndexOf(x);
                            int s2i = groups_dragHandler.DropSink.OrderedList.IndexOf(y);
                            ret = s1i.CompareTo(s2i);
                            return true;
                        }
                }
                return false;
            });

            groups_listView.RowHeight = listViewGroupHeaderHeight + 3;
            groups_listView.BackColor = listViewBackColor;
            groups_listView.GridLines = false;
            groups_listView.HeaderMaximumHeight = listViewColumnsHeight;
            groups_listView.HeaderMinimumHeight = listViewColumnsHeight;
            groups_listView.HeaderUsesThemes = false;
            groups_listView.HeaderFormatStyle = new HeaderFormatStyle();
            groups_listView.HeaderFormatStyle.SetBackColor(listViewBackColor);
            groups_listView.HeaderFormatStyle.SetForeColor(passiveForeColor);
            groups_listView.HeaderFormatStyle.SetFont(new Font(stashes_listView.Font.FontFamily, listViewColumnsFontHeight, FontStyle.Regular));
            groups_listView.UseCellFormatEvents = true;
            groups_listView.MultiSelect = true;
            groups_listView.FullRowSelect = true;
            groups_listView.ShowGroups = false;
            groups_listView.PrimarySortColumn = groups_columnOrder;
            groups_listView.PrimarySortOrder = SortOrder.Ascending;
            groups_listView.CustomSorter = delegate (OLVColumn column, SortOrder order) {
                groups_listView.ListViewItemSorter = new ObjectListViewSortComparer<StashGroupObject>(groups_sortComparer);
            };

            SpaceClick += delegate { Groups_UnselectAll(); };
            groups_pagePanel.Click += delegate { Groups_UnselectAll(); };
            groups_listViewBorderPanel.Click += delegate { Groups_UnselectAll(); };

            Load += delegate {

                groups_listView.CellRightClick += Groups_ListView_CellRightClick;
                groups_listView.CellEditFinished += Groups_ListView_CellEditFinished;
                groups_listView.FormatCell += Groups_ListView_FormatCell;
                groups_listView.FormatRow += Groups_ListView_FormatRow;

                groups_dragHandler = new Dragging.GroupsDragHandler(groups_listView);
                groups_dragHandler.DragSource.DragEnd += Groups_Dragging_DragEnd;
                //stashes_listView.DragEnter += Stashes_Dragging_DragEnter;
                //stashes_listView.DragDrop += Stashes_Dragging_DragDrop;

                Global.Runtime.StashesAdded += delegate (object sender, GlobalHandlers.RuntimeHandler.ListChangedEventArgs<StashObject> args) { Groups_RefreshStashesObjects(args.List); };
                Global.Runtime.StashesRemoved += delegate (object sender, GlobalHandlers.RuntimeHandler.ListChangedEventArgs<StashObject> args) { Groups_RefreshStashesObjects(args.List); };
                Global.Runtime.StashGroupsAdded += delegate (object sender, GlobalHandlers.RuntimeHandler.ListChangedEventArgs<StashGroupObject> args)  { groups_listView.AddObjects(args.List); };
                Global.Runtime.StashGroupsRemoved += delegate (object sender, GlobalHandlers.RuntimeHandler.ListChangedEventArgs<StashGroupObject> args) { groups_listView.RemoveObjects(args.List); };

                Groups_ReloadColumns();
                Groups_ReloadList();
            };
        }

        #region methods

        private void Groups_ReloadColumns()
        {
            groups_listView.Columns.Clear();
            groups_listView.Columns.AddRange(new ColumnHeader[]
            {
                groups_columnSpacer,
                groups_columnID,
                groups_columnName
            });
        }

        public void Groups_ReloadList()
        {
            //if (!Visible) return;

            int scrollY = groups_listView.TopItemIndex;
            groups_listView.ClearObjects();
            groups_listView.SetObjects(Global.Stashes.GetAllStashGroups());
            groups_listView.Sort();
            groups_listView.TopItemIndex = scrollY;
        }

        public void Groups_UnselectAll()
        {
            foreach (OLVListItem item in groups_listView.SelectedItems)
            {
                item.Selected = false;
                groups_listView.RefreshItem(item);
            }
            groups_listView.CancelCellEdit();
        }

        private void Groups_RefreshAllObjects()
        {
            foreach (OLVListItem li in groups_listView.Items)
                groups_listView.RefreshItem(li);
        }

        private void Groups_RefreshStashesObjects(IEnumerable<StashObject> stashes)
        {
            groups_listView.RefreshObjects(stashes
                .Select(stash => stash.GroupID)
                .Distinct() // remove duplicates
                .Select(id => Global.Stashes.GetStashGroup(id))
                .ToArray()
            );
        }

        public StashGroupObject[] Groups_GetSelectedObjects()
        {
            List<StashGroupObject> objects = new List<StashGroupObject>();
            foreach (OLVListItem item in groups_listView.SelectedItems)
                objects.Add((StashGroupObject)item.RowObject);
            return objects.ToArray();
        }






        #endregion

        #region events

        private void Groups_CreateStashGroupButton_Click(object sender, EventArgs e)
        {
            Global.Windows.ShowCreateStashGroupDialog();
        }

        private void Groups_ListView_FormatCell(object sender, FormatCellEventArgs e)
        {
        }

        private void Groups_ListView_FormatRow(object sender, FormatRowEventArgs e)
        {
            //OLVListSubItem subItem;

            int siIndex = 0;
            foreach (OLVListSubItem si in e.Item.SubItems)
            {
                si.Decoration = siIndex == 0 ? rowBorderFirstDecoration : rowBorderDecoration;
                siIndex += 1;
            }

            StashGroupObject group = (StashGroupObject)e.Model;
            bool isDragging = groups_dragHandler.IsDraggingGroup(group);
            bool isSelected = e.Item.Selected;

            if (isDragging)
            {
                e.Item.BackColor = Color.Teal;
                e.Item.ForeColor = Color.WhiteSmoke;
            }
            else
            {
                e.Item.BackColor = listViewGroupHeaderBackColor;
                e.Item.ForeColor = listViewGroupHeaderForeColor;
            }
        }

        private void Groups_ListView_CellEditFinished(object sender, CellEditEventArgs args)
        {
            StashGroupObject group = (StashGroupObject)args.RowObject;
            if (args.Column == groups_columnName) Global.Runtime.NotifyStashGroupsUpdated(group);
            else return;
            Global.Configuration.Save();
            Groups_UnselectAll();
        }

        private void Groups_Dragging_DragEnd(object sender, EventArgs e)
        {
            Groups_UnselectAll();
            Global.Configuration.Save();
            Global.Runtime.NotifyStashGroupsRebuild();
        }

        private void Groups_ListView_CellRightClick(object sender, CellRightClickEventArgs args)
        {
            if (!(args.Model is StashGroupObject group)) return;

            ContextMenuStrip menu = new ContextMenuStrip();

            StashGroupObject[] selectedItems = Groups_GetSelectedObjects();

            if (selectedItems.Length == 1)
            {
                menu.Items.Insert(0, new ToolStripLabel("#" + group.ID + " " + group.Name)
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

            if (selectedItems.Length == 1)
            {
                menu.Items.Add(Global.L.RenameButton(), null, delegate (object s, EventArgs e) {
                    OLVListItem item = (OLVListItem)groups_listView.SelectedItems[0];
                    groups_listView.EditSubItem(item, groups_columnName.Index);
                });
            }

            menu.Items.Add(new ToolStripSeparator());

            menu.Items.Add(Global.L.DeleteButton(), null, delegate (object s, EventArgs e) {
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
