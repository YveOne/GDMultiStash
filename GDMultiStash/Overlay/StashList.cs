using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using GDMultiStash.Common;
using GDMultiStash.Common.Overlay;

namespace GDMultiStash.Overlay.Elements
{
    internal class StashList : PseudoScrollElement<StashListChild>
    {

        protected override float ItemHeight => 26;
        protected override float ItemMargin => 2f;
        protected override float ItemMarginStart => 2f;

        private readonly Dictionary<int, StashListChild> _stashId2Item;
        private readonly Dictionary<int, int> _itemID2stashID;

        private readonly float _alphaActive = 1f;
        private readonly float _alphaInactive = 0.66f;

        private bool _reqBuildList = true; // build list on startup

        public StashList()
        {
            X = 6;
            Y = 19;
            WidthToParent = true;
            Width = -25;
            Height = 574;

            MaxVisibleCount = 20;
            MouseCheckNeedBaseHit = true;

            _itemID2stashID = new Dictionary<int, int>();
            _stashId2Item = new Dictionary<int, StashListChild>();

            Global.Runtime.ActiveStashChanged += delegate (object sender, GlobalHandlers.RuntimeHandler.ActiveStashChangedEventArgs e) {
                ChangeActiveStash(e.OldID, e.NewID);
            };
            Global.Runtime.ActiveModeChanged += delegate {
                UpdateList();
            };
            Global.Runtime.ActiveExpansionChanged += delegate {
                UpdateList();
            };
            Global.Runtime.StashesOrderChanged += delegate (object sender, GlobalHandlers.RuntimeHandler.StashListChangedEventArgs e) {
                UpdateList();
            };
            Global.Runtime.StashesAdded += delegate (object sender, GlobalHandlers.RuntimeHandler.StashListChangedEventArgs e)
            {
                foreach (StashObject stash in e.Stashes)
                    AddStashItem(stash);
            };
            Global.Runtime.StashesRemoved += delegate (object sender, GlobalHandlers.RuntimeHandler.StashListChangedEventArgs e)
            {
                foreach (StashObject stash in e.Stashes)
                    RemoveStashItem(stash);
            };
            Global.Runtime.StashesUpdated += delegate (object sender, GlobalHandlers.RuntimeHandler.StashListChangedEventArgs e)
            {
                foreach (StashObject stash in e.Stashes)
                    UpdateStashItem(stash);
            };
            Global.Runtime.StashReopenStart += delegate {
                _listDisabled = true;
                Alpha = 0.33f;
            };
            Global.Runtime.StashReopenEnd += delegate {
                _listDisabled = false;
                Alpha = 1f;
            };
        }

        public override void Begin()
        {
            base.Begin();
            if (_reqBuildList)
            {
                _reqBuildList = false;
                OnBuildList();
            }
        }

        public void BuildList()
        {
            _reqBuildList = true;
        }

        private void OnBuildList()
        {
            foreach (var item in Items)
                item.Active = false;
            Items.Clear();
            _itemID2stashID.Clear();
            _stashId2Item.Clear();
            ClearScrollItems();
            foreach (StashObject stash in Global.Stashes.GetAllStashes())
                AddStashItem(stash);
        }

        protected override void OnUpdateListStart()
        {
            GrimDawnLib.GrimDawnGameMode mode = Global.Runtime.CurrentMode;
            GrimDawnLib.GrimDawnGameExpansion exp = Global.Runtime.CurrentExpansion;
            foreach (var item in Items)
            {
                StashObject stash = item.Model as StashObject;
                item.Order = stash.Order;
                item.Alpha = _alphaInactive;
                item.Active = false;
                item.Visible = ((
                    stash.SC == true && mode == GrimDawnLib.GrimDawnGameMode.SC
                ) || (
                    stash.HC == true && mode == GrimDawnLib.GrimDawnGameMode.HC
                )) && stash.Expansion == exp;
            }
            int activeStashID = Global.Runtime.ActiveStashID;
            if (_stashId2Item.ContainsKey(activeStashID))
            {
                StashListChild activeItem = _stashId2Item[activeStashID];
                activeItem.Alpha = _alphaActive;
                activeItem.Active = true;
            }
        }

        public void ChangeActiveStash(int oldID, int newID)
        {
            if (_stashId2Item.ContainsKey(oldID))
            {
                _stashId2Item[oldID].Active = false;
                _stashId2Item[oldID].Alpha = _alphaInactive;
            }
            if (_stashId2Item.ContainsKey(newID))
            {
                _stashId2Item[newID].Active = true;
                _stashId2Item[newID].Alpha = _alphaActive;
            }
        }

        public void AddStashItem(StashObject stash)
        {
            StashListChild item = GetCachedScrollItem();
            if (item == null)
            {
                item = new StashListChild(stash)
                {
                    MouseCheckChildren = false,
                };
            }
            item.MouseEnter += Item_MouseEnter;
            item.MouseLeave += Item_MouseLeave;
            item.MouseClick += Item_OnClick;
            item.Active = false;
            item.Alpha = _alphaInactive;
            item.Text = stash.Name;
            item.Order = stash.Order;
            item.Locked = stash.Locked;
            item.Color = stash.GetDisplayColor();
            item.Font = stash.GetDisplayFont();

            _itemID2stashID.Add(item.ID, stash.ID);
            _stashId2Item.Add(stash.ID, item);
            AddScrollItem(item);
            UpdateList();
        }

        public void RemoveStashItem(StashObject stash)
        {
            if (!_stashId2Item.ContainsKey(stash.ID)) return;
            StashListChild item = _stashId2Item[stash.ID];
            RemoveScrollItem(item);
            _itemID2stashID.Remove(item.ID);
            _stashId2Item.Remove(stash.ID);
            UpdateList();
        }

        public void UpdateStashItem(StashObject stash)
        {
            if (!_stashId2Item.ContainsKey(stash.ID)) return;
            StashListChild item = _stashId2Item[stash.ID];
            item.Text = stash.Name;
            item.Color = stash.GetDisplayColor();
            item.Locked = stash.Locked;
        }
















        #region Mouse stuff

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
            if (!si.Active) si.Alpha = _alphaInactive;
        }

        private bool _listDisabled = false;

        private void Item_OnClick(object sender, EventArgs e)
        {
            if (_listDisabled) return;
            if (sender is StashListChild displayItem)
            {
                if (!_itemID2stashID.ContainsKey(displayItem.ID)) return;
                int stashID = _itemID2stashID[displayItem.ID];
                Global.Runtime.SwitchToStash(stashID);
            }
        }

        #endregion

    }
}
