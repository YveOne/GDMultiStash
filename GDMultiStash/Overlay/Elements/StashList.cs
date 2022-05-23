using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using GDMultiStash.Common.Overlay;

namespace GDMultiStash.Overlay.Elements
{
    public class StashList : PseudoScrollElement<StashListChild>
    {

        public static Font _ItemFont = null;

        protected override float ItemHeight => 23f;
        protected override float ItemMargin => 5f;

        private readonly Dictionary<int, StashListDataHolder> _listItems; // stashID -> StashListDataHolder
        private readonly Dictionary<int, int> _displayID2stashID;

        private class StashListDataHolder
        {
            public Common.Stash Stash;
            public StashListChild ListItem;
        }

        public StashList()
        {
            MaxChildCount = 20;
            MouseCheckNeedBaseHit = true;
            _listItems = new Dictionary<int, StashListDataHolder>();
            _displayID2stashID = new Dictionary<int, int>();
            RebuildList();
        }

        private bool _rebuildList = false;
        private bool _updateList = false;

        public override void Begin()
        {
            base.Begin();

            if (_rebuildList)
            {
                _rebuildList = false;
                OnRebuildList();
            }
            if (_updateList)
            {
                _updateList = false;
                OnUpdateList();
            }
        }

        public void RebuildList()
        {
            _rebuildList = true;
            _updateList = true;
        }

        public override void UpdateList()
        {
            base.UpdateList();
            _updateList = true;
        }

        public void OnRebuildList()
        {

            foreach (StashListDataHolder item in _listItems.Values)
                item.ListItem.Active = false;
            
            _listItems.Clear();
            _displayID2stashID.Clear();
            ClearScrollItems();

            // refill the list
            foreach (Common.Stash stash in Core.Stashes.GetAllStashes())
            {
                StashListChild si = GetCachedScrollItem();
                if (si == null)
                {
                    si = new StashListChild(_ItemFont)
                    {
                        MouseCheckChildren = false,
                    };
                }
                si.MouseEnter += Item_MouseEnter;
                si.MouseLeave += Item_MouseLeave;
                si.MouseClick += Item_OnClick;
                si.Alpha = 0.5f;
                si.Text = stash.Name;
                si.Order = stash.Order;

                _listItems.Add(stash.ID, new StashListDataHolder
                {
                    Stash = stash,
                    ListItem = si,
                });
                _displayID2stashID.Add(si.ID, stash.ID);
                AddScrollItem(si);
            }
        }

        public delegate void VisibleCountChangedEventHandler(int visibleCount);
        public event VisibleCountChangedEventHandler VisibleCountChanged;

        private int lastVisibleCount = 0;

        protected void OnUpdateList()
        {
            int visibleCount = 0;
            GrimDawnLib.GrimDawnGameMode mode = Core.Runtime.CurrentMode;
            GrimDawnLib.GrimDawnGameExpansion exp = Core.Runtime.CurrentExpansion;
            foreach (StashListDataHolder i in _listItems.Values)
            {
                i.ListItem.Alpha = 0.5f;
                i.ListItem.Active = false;
                i.ListItem.Visible = ((
                    i.Stash.SC == true && mode == GrimDawnLib.GrimDawnGameMode.SC
                ) || (
                    i.Stash.HC == true && mode == GrimDawnLib.GrimDawnGameMode.HC
                )) && i.Stash.Expansion == exp;
                if (i.ListItem.Visible) visibleCount += 1;
            }
            int stashID = Core.Runtime.ActiveStashID;
            if (stashID != -1)
            {
                _listItems[stashID].ListItem.Active = true;
                _listItems[stashID].ListItem.Alpha = 1f;
            }
            if (lastVisibleCount != visibleCount)
            {
                lastVisibleCount = visibleCount;
                VisibleCountChanged?.Invoke(visibleCount);
            }
        }
















        public bool LockMouseMove = false;

        public override bool CheckMouseMove(int x, int y)
        {
            if (LockMouseMove) return false;
            return base.CheckMouseMove(x, y);
        }

        private void Item_MouseEnter(object sender, EventArgs e)
        {
            StashListChild si = ((StashListChild)sender);
            si.Alpha = 1.0f;
        }

        private void Item_MouseLeave(object sender, EventArgs e)
        {
            StashListChild si = ((StashListChild)sender);
            if (!si.Active) si.Alpha = 0.5f;
        }

        private bool _listDisabled = false;

        private void Item_OnClick(object sender, EventArgs e)
        {
            if (_listDisabled) return;
            if (sender is StashListChild displayItem)
            {
                if (!_displayID2stashID.ContainsKey(displayItem.ID)) return;
                int stashID = _displayID2stashID[displayItem.ID];

                _listDisabled = true;
                Alpha = 0.33f;
                Core.Runtime.SwitchToStash(stashID);
                _listDisabled = false;
                Alpha = 1f;

            }
        }

    }
}
