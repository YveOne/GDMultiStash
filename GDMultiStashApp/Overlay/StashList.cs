using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using GDMultiStash.Common.Objects;
using GDMultiStash.Common.Overlay;
using GDMultiStash.Overlay.Controls;
using GDMultiStash.Overlay.Controls.Base;

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

            Global.Runtime.ActiveStashChanged += delegate (object sender, GlobalHandlers.RuntimeHandler.ActiveStashChangedEventArgs e) {
                ChangeActiveStash(e.OldID, e.NewID);
            };
            Global.Runtime.ActiveModeChanged += delegate { _loadItems = true; };
            Global.Runtime.ActiveExpansionChanged += delegate { _loadItems = true; };
            Global.Runtime.ActiveGroupChanged += delegate { _loadItems = true; };
            Global.Runtime.StashesRebuild += delegate { _loadItems = true; };
            Global.Runtime.StashesAdded += delegate { _loadItems = true; };
            Global.Runtime.StashesRemoved += delegate { _loadItems = true; };
            Global.Runtime.StashesInfoChanged += delegate (object sender, GlobalHandlers.RuntimeHandler.ListEventArgs<StashObject> e)
            {
                foreach (StashObject stash in e.Items)
                {
                    UpdateStashItemInfo(stash);
                }
            };
            Global.Runtime.StashesContentChanged += delegate (object sender, GlobalHandlers.RuntimeHandler.ListEventArgs<StashObject> e)
            {
                foreach (StashObject stash in e.Items)
                {
                    UpdateStashItemContent(stash);
                }
            };
            Global.Runtime.StashesImported += delegate (object sender, GlobalHandlers.RuntimeHandler.ListEventArgs<StashObject> e)
            {
                foreach (StashObject stash in e.Items)
                {
                    UpdateStashItemInfo(stash);
                    UpdateStashItemContent(stash);
                }
            };
            _updateAppearance = true; // update on startup
            Global.Configuration.AppearanceChanged += delegate {
                _updateAppearance = true;
            };
            MouseWheel += delegate (object sender, MouseWheelEventArgs e)
            {
                ScrollHandler.ScrollPositionY -= e.Delta < 0 ? -1 : 1;
            };
        }

        public override void Begin()
        {
            base.Begin();
            if (_loadItems)
            {
                _loadItems = false;

                foreach (var item in ScrollItems)
                    item.Active = false;
                _stashId2Item.Clear();
                ClearScrollItems();

                GrimDawnLib.GrimDawnGameMode mode = Global.Runtime.CurrentMode;
                GrimDawnLib.GrimDawnGameExpansion exp = Global.Runtime.CurrentExpansion;
                int activeGroupID = Global.Runtime.ActiveGroupID;
                int activeStashID = Global.Runtime.ActiveStashID;

                List<StashObject> stashes = Global.Stashes.GetAllStashes()
                    .Where(stash => stash.Expansion == exp && ((
                        stash.SC == true && mode == GrimDawnLib.GrimDawnGameMode.SC
                    ) || (
                        stash.HC == true && mode == GrimDawnLib.GrimDawnGameMode.HC
                    )) && stash.GroupID == activeGroupID)
                    .ToList();
                stashes.Sort(new Common.Objects.Sorting.StashesSortComparer());
                foreach (var stash in stashes)
                    AddStashItem(stash);

                if (_stashId2Item.ContainsKey(activeStashID))
                    _stashId2Item[activeStashID].Active = true;
            }
            if (_updateAppearance)
            {
                _updateAppearance = false;
                foreach(var item in ScrollItems)
                    item.ShowWorkload = Global.Configuration.Settings.OverlayShowWorkload;
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
            StashListChild item = GetCachedScrollItem();
            if (item == null)
                item = new StashListChild();
            item.Model = stash;
            item.Active = false;
            item.Text = stash.Name;
            item.Order = stash.Order;
            item.Locked = stash.Locked;
            item.Color = stash.DisplayColor;
            item.Font = stash.DisplayFont;
            item.Visible = true;
            item.ShowWorkload = Global.Configuration.Settings.OverlayShowWorkload;
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
