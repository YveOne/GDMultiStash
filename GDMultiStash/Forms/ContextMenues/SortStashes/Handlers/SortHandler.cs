using System;
using System.Collections.Generic;
using System.Linq;

using GDMultiStash.Common;
using GDMultiStash.Common.Objects;

using GrimDawnLib;

using GDMultiStash.Forms.ContextMenues.SortStashes.Methods;
namespace GDMultiStash.Forms.ContextMenues.SortStashes.Handlers
{
    internal abstract class SortHandler<M, S>
    {
        private readonly Dictionary<int, bool> parentGroups; // this is used to delete empty groups afterwards
        protected readonly ItemSortMethod<M> method;
        protected readonly SortedDictionary<M, S> sortedList;

        public SortHandler(ItemSortMethod<M> method)
        {
            this.method = method;
            this.parentGroups = new Dictionary<int, bool>();
            this.sortedList = new SortedDictionary<M, S>();
        }

        public SortHandlerResult Run(
            IEnumerable<StashObject> selectedStashes,
            GrimDawnGameExpansion gdExpansion
            )
        {

            var ignoreKeys = method.IgnoreKeys;



            foreach (var stash in selectedStashes)
            {
                Global.FileSystem.BackupStashTransferFile(stash.ID);
                foreach (var tab in stash.Tabs)
                {
                    var removeItems = new List<GDIALib.Parser.Stash.Item>();
                    foreach (var item in tab.Items)
                    {
                        if (!Global.Database.GetItemInfo(item, out GlobalHandlers.DatabaseHandler.ItemInfo itemInfo))
                            continue;

                        M sortKey = method.GetSortKey(itemInfo);
                        if (ignoreKeys.Contains(sortKey))
                            continue;

                        FillSortedList(sortKey, item, itemInfo);
                        removeItems.Add(item);
                    }
                    foreach (var item in removeItems)
                        tab.Items.Remove(item);
                }
                parentGroups[stash.GroupID] = true;
                stash.SaveTransferFile();
                stash.LoadTransferFile();
                Global.Runtime.InvokeStashesContentChanged(stash, true);
            }

            // create new group for new stashes
            var group = Global.Groups.CreateGroup(method.GroupText, true);
            var firstSelectedStash = selectedStashes.ToList()[0];
            var stashWidth = firstSelectedStash.Width; // all selected stashes should have same width and hight
            var stashHeight = firstSelectedStash.Height;
            var addedStashes = new List<StashObject>();

            // the tabs are already multi-sorted
            // now just loop recursive
            foreach (var sortKVP in sortedList)
            {
                var sortKey = sortKVP.Key;
                var sortList = sortKVP.Value;

                // first we need to create a list of tabs
                var tabsQueue = GetTabsQueue(sortKey, sortList, stashWidth, stashHeight);

                // now we got all items inside the tabs queue
                while (tabsQueue.Count != 0)
                {
                    var stash = Global.Stashes.CreateStash(method.GetText(sortKey), gdExpansion, GrimDawnGameMode.Both, 0, false);
                    stash.GroupID = group.ID;
                    int maxTabs = (int)TransferFile.GetStashInfoForExpansion(gdExpansion).MaxTabs;
                    for (int tabI = 0; tabI < maxTabs; tabI += 1)
                    {
                        if (tabsQueue.Count == 0) continue; // just skip...
                        stash.AddTab(tabsQueue[0]);
                        tabsQueue.RemoveAt(0);
                    }
                    stash.SaveTransferFile();
                    stash.LoadTransferFile();
                    addedStashes.Add(stash);
                }
            }

            // delete empty stashes/groups
            var deletedStashes = Global.Stashes.DeleteStashes(selectedStashes, true); // true = only empty
            Global.Runtime.InvokeStashesRemoved(deletedStashes);
            var emptyGroups = parentGroups.Keys
                .Where(grpId => Global.Stashes.GetStashesForGroup(grpId).Length == 0)
                .Select(grpId => Global.Groups.GetGroup(grpId))
                .Where(grp => grp != null)
                .ToArray();
            if (emptyGroups.Length != 0)
            {
                var deletedGroups = Global.Groups.DeleteGroups(emptyGroups);
                Global.Runtime.InvokeStashGroupsRemoved(deletedGroups);
            }
            var remainingStashIds = selectedStashes
                .Where((s) => Global.Stashes.GetStash(s.ID) != null);

            Global.Configuration.Save();
            Global.Runtime.InvokeStashGroupsAdded(group);
            Global.Runtime.InvokeStashesAdded(addedStashes);
            return new SortHandlerResult
            {
                AddedGroup = group,
                AddedStashes = addedStashes.ToArray(),
                RemainingStashes = remainingStashIds.ToArray(),
            };
        }

        protected abstract void FillSortedList(
            M sortKey,
            GDIALib.Parser.Stash.Item item,
            GlobalHandlers.DatabaseHandler.ItemInfo itemInfo);

        protected abstract List<GDIALib.Parser.Stash.StashTab> GetTabsQueue(M sortKey, S sortList,
            uint stashWidth, uint stashHeight);


    }
}
