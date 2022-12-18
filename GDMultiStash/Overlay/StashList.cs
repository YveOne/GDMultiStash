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
            Global.Runtime.StashesOrderChanged += delegate { _loadItems = true; };
            Global.Runtime.StashesAdded += delegate { _loadItems = true; };
            Global.Runtime.StashesRemoved += delegate { _loadItems = true; };
            Global.Runtime.StashesUpdated += delegate (object sender, GlobalHandlers.RuntimeHandler.ListChangedEventArgs<StashObject> e)
            {
                foreach (StashObject stash in e.List)
                    UpdateStashItem(stash);
            };
            Global.Runtime.StashesImported += delegate (object sender, GlobalHandlers.RuntimeHandler.ListChangedEventArgs<StashObject> e)
            {
                foreach (StashObject stash in e.List)
                {
                    // we need to check if key is in list
                    // because when the user changed group ingame
                    // and switched to another stash
                    // the stash of previous shown group will be imported
                    // and that will also trigger this event
                    if (_stashId2Item.ContainsKey(stash.ID))
                    {
                        _stashId2Item[stash.ID].UpdateUsageIndicator();
                    }
                }
            };
            Global.Runtime.StashReopenStart += delegate {
                MouseCheckChildren = false;
                Alpha = 0.33f;
            };
            Global.Runtime.StashReopenEnd += delegate {
                MouseCheckChildren = true;
                Alpha = 1f;
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
            item.Color = stash.GetDisplayColor();
            item.Font = stash.GetDisplayFont();
            item.Visible = true;
            item.UpdateUsageIndicator();
            _stashId2Item.Add(stash.ID, item);
            AddScrollItem(item);
        }

        public void UpdateStashItem(StashObject stash)
        {
            if (!_stashId2Item.ContainsKey(stash.ID)) return;
            StashListChild item = _stashId2Item[stash.ID];
            item.Text = stash.Name;
            item.Color = stash.GetDisplayColor();
            item.Locked = stash.Locked;
        }

    }
}
