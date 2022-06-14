using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using GDMultiStash.Common.Overlay;

namespace GDMultiStash.Overlay.Elements
{
    internal class StashList : PseudoScrollElement<StashListChild>
    {

        //public static Font _ItemFont = null;

        protected override float ItemHeight => 26;
        protected override float ItemMargin => 2f;

        private readonly Dictionary<int, StashListDataHolder> _listItems; // stashID -> StashListDataHolder
        private readonly Dictionary<int, int> _displayID2stashID;

        private readonly float _alphaActive = 1f;
        private readonly float _alphaInactive = 0.66f;

        public delegate void VisibleCountChangedEventHandler(int visibleCount);
        public event VisibleCountChangedEventHandler VisibleCountChanged;

        private int _lastVisibleCount = 0;

        private bool _reqBuildList = true; // build list on startup
        private bool _reqUpdateList = true;

        private class StashListDataHolder
        {
            public Common.Stash Stash;
            public StashListChild ListItem;
        }

        public StashList()
        {
            X = 6;
            Y = 19;
            WidthToParent = true;
            Width = -32;
            Height = 574;

            MaxVisibleCount = 20;
            MouseCheckNeedBaseHit = true;

            _listItems = new Dictionary<int, StashListDataHolder>();
            _displayID2stashID = new Dictionary<int, int>();

            Core.Runtime.ActiveStashChanged += delegate (object sender, Core.Runtime.ActiveStashChangedEventArgs e) {
                ChangeActiveStash(e.OldID, e.NewID);
            };
            Core.Runtime.CurrentModeChanged += delegate {
                UpdateList();
            };
            Core.Runtime.CurrentExpansionChanged += delegate {
                UpdateList();
            };
            Core.Runtime.StashesRearranged += delegate (object sender, Core.Runtime.StashesChangedEventArgs e) {
                UpdateList();
            };
            Core.Runtime.StashesAdded += delegate (object sender, Core.Runtime.StashesChangedEventArgs e)
            {
                foreach (Common.Stash stash in e.Stashes)
                    AddStashItem(stash);
            };
            Core.Runtime.StashesRemoved += delegate (object sender, Core.Runtime.StashesChangedEventArgs e)
            {
                foreach (Common.Stash stash in e.Stashes)
                    RemoveStashItem(stash);
            };
            Core.Runtime.StashesModeChanged += delegate (object sender, Core.Runtime.StashesChangedEventArgs e)
            {
                foreach (Common.Stash stash in e.Stashes)
                    UpdateStashItem(stash);
            };
            Core.Runtime.StashesNameChanged += delegate (object sender, Core.Runtime.StashesChangedEventArgs e)
            {
                foreach (Common.Stash stash in e.Stashes)
                    UpdateStashItem(stash);
            };
            Core.Runtime.StashesColorChanged += delegate (object sender, Core.Runtime.StashesChangedEventArgs e)
            {
                foreach (Common.Stash stash in e.Stashes)
                    UpdateStashItem(stash);
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

            if (_reqUpdateList)
            {
                _reqUpdateList = false;
                OnUpdateList();
            }

        }

        public void BuildList()
        {
            _reqBuildList = true;
        }

        public override void UpdateList()
        {
            _reqUpdateList = true;
        }

        private void OnBuildList()
        {
            foreach (StashListDataHolder item in _listItems.Values)
                item.ListItem.Active = false;
            _listItems.Clear();
            _displayID2stashID.Clear();
            ClearScrollItems();
            foreach (Common.Stash stash in Core.Stashes.GetAllStashes())
                AddStashItem(stash);
        }

        private void OnUpdateList()
        {
            int visibleCount = 0;
            GrimDawnLib.GrimDawnGameMode mode = Core.Runtime.CurrentMode;
            GrimDawnLib.GrimDawnGameExpansion exp = Core.Runtime.CurrentExpansion;
            foreach (StashListDataHolder i in _listItems.Values)
            {
                i.ListItem.Order = i.Stash.Order;
                i.ListItem.Alpha = _alphaInactive;
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
                _listItems[stashID].ListItem.Alpha = _alphaActive;
            }
            if (_lastVisibleCount != visibleCount)
            {
                _lastVisibleCount = visibleCount;
                VisibleCountChanged?.Invoke(visibleCount);
                CurrentVisibleCount = visibleCount;
            }
            base.UpdateList();
        }

        public void ChangeActiveStash(int oldID, int newID)
        {
            if (_listItems.ContainsKey(oldID))
            {
                _listItems[oldID].ListItem.Active = false;
                _listItems[oldID].ListItem.Alpha = _alphaInactive;
            }
            if (_listItems.ContainsKey(newID))
            {
                _listItems[newID].ListItem.Active = true;
                _listItems[newID].ListItem.Alpha = _alphaActive;
            }
        }

        public void AddStashItem(Common.Stash stash)
        {
            StashListChild si = GetCachedScrollItem();
            if (si == null)
            {
                si = new StashListChild()
                {
                    MouseCheckChildren = false,
                };
            }
            si.MouseEnter += Item_MouseEnter;
            si.MouseLeave += Item_MouseLeave;
            si.MouseClick += Item_OnClick;
            si.Active = false;
            si.Alpha = _alphaInactive;
            si.Text = stash.Name;
            si.Order = stash.Order;
            si.Color = stash.GetDisplayColor();
            si.Font = stash.GetDisplayFont(); //_ItemFont;

            _listItems.Add(stash.ID, new StashListDataHolder
            {
                Stash = stash,
                ListItem = si,
            });
            _displayID2stashID.Add(si.ID, stash.ID);
            AddScrollItem(si);
            _reqUpdateList = true;
        }

        public void RemoveStashItem(Common.Stash stash)
        {
            if (!_listItems.ContainsKey(stash.ID)) return; // not initialized
            StashListDataHolder data = _listItems[stash.ID];
            RemoveScrollItem(data.ListItem);
            _displayID2stashID.Remove(data.ListItem.ID);
            _listItems.Remove(stash.ID);
            _reqUpdateList = true;
        }

        public void UpdateStashItem(Common.Stash stash)
        {
            if (!_listItems.ContainsKey(stash.ID)) return; // not initialized
            StashListDataHolder data = _listItems[stash.ID];
            data.ListItem.Text = stash.Name;
            data.ListItem.Color = stash.GetDisplayColor();
            _reqUpdateList = true;
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
                if (!_displayID2stashID.ContainsKey(displayItem.ID)) return;
                int stashID = _displayID2stashID[displayItem.ID];


                _listDisabled = true;
                Alpha = 0.33f;
                Core.Runtime.SwitchToStash(stashID);
                _listDisabled = false;
                Alpha = 1f;

            }
        }

        #endregion

    }
}
