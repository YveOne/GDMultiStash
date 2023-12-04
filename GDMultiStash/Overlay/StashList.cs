using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using GDMultiStash.Common.Objects;

using D3DHook.Overlay.Common;

namespace GDMultiStash.Overlay
{
    internal class StashList : ListBoxElement<StashListChild, StashObject>
    {

        public override float ItemHeight => 26;
        public override float ItemMargin => 2;

        private readonly Dictionary<int, StashListChild> _stashId2Item;

        private bool _loadItems = false;
        private bool _updateAppearance = false;

        public override Color DebugColor => Color.FromArgb(128, 255, 0, 255);

        public StashList()
        {
            _stashId2Item = new Dictionary<int, StashListChild>();
            _loadItems = true; // build list on startup

            G.Stashes.ActiveStashChanged += delegate (object sender, Global.Stashes.ActiveStashChangedEventArgs e) {
                ChangeActiveStash(e.OldID, e.NewID);
            };
            G.Runtime.ActiveModeChanged += delegate { _loadItems = true; };
            G.Runtime.ActiveExpansionChanged += delegate { _loadItems = true; };
            G.StashGroups.ActiveGroupChanged += delegate { _loadItems = true; };
            G.Stashes.StashesMoved += delegate { _loadItems = true; };
            G.Stashes.StashesAdded += delegate { _loadItems = true; };
            G.Stashes.StashesRemoved += delegate { _loadItems = true; };
            G.Stashes.StashesInfoChanged += delegate (object sender, Global.Stashes.StashObjectsEventArgs e)
            {
                foreach (StashObject stash in e.Items)
                {
                    UpdateStashItemInfo(stash);
                }
            };
            G.Stashes.StashesContentChanged += delegate (object sender, Global.Stashes.StashesContentChangedEventArgs e)
            {
                foreach (StashObject stash in e.Items)
                {
                    //UpdateStashItemInfo(stash);
                    UpdateStashItemContent(stash);
                }
            };
            _updateAppearance = true; // update on startup
            G.Configuration.OverlayDesignChanged += delegate {
                _updateAppearance = true;
            };
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
                _stashId2Item.Clear();
                ClearScrollItems();

                GrimDawnLib.GrimDawnGameMode mode = G.Runtime.ActiveMode;
                GrimDawnLib.GrimDawnGameExpansion exp = G.Runtime.ActiveExpansion;
                int activeGroupID = G.StashGroups.ActiveGroupID;
                int activeStashID = G.Stashes.ActiveStashID;

                List<StashObject> stashes = G.Stashes.GetAllStashes()
                    .Where(stash => stash.Expansion == exp && ((
                        stash.SC == true && mode == GrimDawnLib.GrimDawnGameMode.SC
                    ) || (
                        stash.HC == true && mode == GrimDawnLib.GrimDawnGameMode.HC
                    )) && stash.GroupID == activeGroupID)
                    .ToList();
                stashes.Sort(new Common.Objects.Sorting.Comparer.StashesSortComparer());
                foreach (var stash in stashes)
                    AddStashItem(stash);

                if (_stashId2Item.ContainsKey(activeStashID))
                    _stashId2Item[activeStashID].Active = true;
            }
            if (_updateAppearance)
            {
                _updateAppearance = false;
                foreach(var item in ScrollItems)
                    item.ShowWorkload = G.Configuration.Settings.OverlayShowWorkload;
            }
        }

        public void ChangeActiveStash(int oldID, int newID)
        {
            if (_stashId2Item.ContainsKey(oldID))
                _stashId2Item[oldID].Active = false;
            if (_stashId2Item.ContainsKey(newID))
                _stashId2Item[newID].Active = true;
        }

        public void AddStashItem(StashObject stash)
        {
            StashListChild item = new StashListChild();
            item.Model = stash;
            item.Active = false;
            item.Text = stash.Name;
            item.Order = stash.Order;
            item.Locked = stash.Locked;
            item.Color = stash.DisplayColor;
            item.Visible = true;
            item.ShowWorkload = G.Configuration.Settings.OverlayShowWorkload;
            item.UpdateUsageIndicator();
            _stashId2Item.Add(stash.ID, item);
            AddScrollItem(item);
        }

        public void UpdateStashItemInfo(StashObject stash)
        {
            if (!_stashId2Item.ContainsKey(stash.ID)) return;
            StashListChild item = _stashId2Item[stash.ID];
            item.Text = stash.Name;
            item.Color = stash.DisplayColor;
            item.Locked = stash.Locked;
        }

        public void UpdateStashItemContent(StashObject stash)
        {
            if (!_stashId2Item.ContainsKey(stash.ID)) return;
            StashListChild item = _stashId2Item[stash.ID];
            item.UpdateUsageIndicator();
        }

    }
}
