﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using GDMultiStash.Common.Objects;
using GDMultiStash.Overlay.Controls;
using GDMultiStash.Overlay.Controls.Base;

using D3DHook.Overlay;

namespace GDMultiStash.Overlay
{
    internal class GroupList : ListBoxElement<GroupListChild, StashGroupObject>
    {
        public override float ItemHeight => 26;
        public override float ItemMargin => 2;

        private readonly Dictionary<int, GroupListChild> _groupId2Item;

        private bool _loadItems = false;

        private readonly Color _itemTextColor = Color.FromArgb(162, 128, 75);

        public override Color DebugColor => Color.FromArgb(128, 255, 0, 255);

        public GroupList()
        {
            _groupId2Item = new Dictionary<int, GroupListChild>();
            _loadItems = true; // build list on startup

            Global.Runtime.ActiveGroupChanged += delegate (object sender, GlobalHandlers.RuntimeHandler.ActiveGroupChangedEventArgs e) {
                ChangeActiveGroup(e.OldID, e.NewID);
            };
            Global.Runtime.StashGroupsMoved += delegate { _loadItems = true; };
            Global.Runtime.ActiveExpansionChanged += delegate { _loadItems = true; };
            Global.Runtime.ActiveModeChanged += delegate { _loadItems = true; };
            Global.Runtime.StashGroupsAdded += delegate { _loadItems = true; };
            Global.Runtime.StashGroupsRemoved += delegate { _loadItems = true; };
            Global.Runtime.StashGroupsInfoChanged += delegate (object sender, GlobalHandlers.RuntimeHandler.ListEventArgs<StashGroupObject> e)
            {
                foreach (StashGroupObject item in e.Items)
                    UpdateItem(item);
            };
            Global.Runtime.StashesMoved += delegate { _loadItems = true; }; // maybe a stash's group has been changed
            Global.Runtime.StashesAdded += delegate { _loadItems = true; };
            Global.Runtime.StashesRemoved += delegate { _loadItems = true; };
            MouseWheel += delegate (object sender, MouseWheelEventArgs e)
            {
                ScrollHandler.ScrollPositionY -= e.Delta < 0 ? -1 : 1;
            };
        }

        protected override void OnDrawBegin()
        {
            base.OnDrawBegin();
            if (_loadItems)
            {
                _loadItems = false;

                foreach (var item in ScrollItems)
                    item.Active = false;
                _groupId2Item.Clear();
                ClearScrollItems();

                GrimDawnLib.GrimDawnGameMode mode = Global.Runtime.ActiveMode;
                GrimDawnLib.GrimDawnGameExpansion exp = Global.Runtime.ActiveExpansion;
                int activeGroupID = Global.Runtime.ActiveGroupID;

                // first we need to find the groups that are not empty
                Dictionary<int, int> GroupStashesCount = new Dictionary<int, int>();
                foreach (var group in Global.Groups.GetAllGroups())
                    GroupStashesCount.Add(group.ID, 0);
                Global.Stashes.GetAllStashes()
                    .Where(stash => stash.Expansion == exp && ((
                        stash.SC == true && mode == GrimDawnLib.GrimDawnGameMode.SC
                    ) || (
                        stash.HC == true && mode == GrimDawnLib.GrimDawnGameMode.HC
                    ))).ToList().ForEach(stash => {
                        GroupStashesCount[stash.GroupID] += 1;
                    });
                    
                List<StashGroupObject> groups = Global.Groups.GetSortedGroups()
                    .Where(group => GroupStashesCount[group.ID] != 0)
                    .ToList();

                foreach (var group in groups)
                    AddGroupItem(group);

                if (_groupId2Item.ContainsKey(activeGroupID))
                    _groupId2Item[activeGroupID].Active = true;
            }
        }

        public void ChangeActiveGroup(int oldID, int newID)
        {
            if (_groupId2Item.ContainsKey(oldID))
                _groupId2Item[oldID].Active = false;
            if (_groupId2Item.ContainsKey(newID))
                _groupId2Item[newID].Active = true;
        }

        public void AddGroupItem(StashGroupObject group)
        {
            GroupListChild item = new GroupListChild();
            item.Model = group;
            item.Active = false;
            item.Text = group.Name;
            item.Order = group.Order;
            item.Color = _itemTextColor;
            item.Visible = true;
            _groupId2Item.Add(group.ID, item);
            AddScrollItem(item);
        }

        public void UpdateItem(StashGroupObject group)
        {
            if (!_groupId2Item.ContainsKey(group.ID)) return;
            GroupListChild item = _groupId2Item[group.ID];
            item.Text = group.Name;
        }

    }
}
