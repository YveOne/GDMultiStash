using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using GrimDawnLib;
using GDMultiStash.Common.Objects;
using GDMultiStash.Common.Objects.Sorting;

namespace GDMultiStash.GlobalHandlers
{
    internal partial class GroupsHandler : Base.ObjectsHandler<StashGroupObject>
    {
        public void LoadGroups()
        {
            Console.WriteLine($"Loading Stash Groups:");
            foreach (Common.Config.ConfigStashGroup cfgStashGroup in Global.Configuration.StashGroups)
            {
                StashGroupObject grp = new StashGroupObject(cfgStashGroup);
                Items.Add(grp.ID, grp);
                Console.WriteLine($"   #{grp.ID} {grp.Name}");
            }
        }

        public bool TryGetGroup(int grpId, out StashGroupObject group)
        {
            return Items.TryGetValue(grpId, out group);
        }

        public StashGroupObject GetGroup(int grpId)
        {
            if (TryGetGroup(grpId, out StashGroupObject grp))
                return grp;
            return null;
        }

        public StashGroupObject[] GetAllGroups()
        {
            return Items.Values.ToArray();
        }

        public StashGroupObject[] GetSortedGroups()
        {
            var l = Items.Values.ToList();
            l.Sort(new GroupsSortComparer());
            return l.ToArray();
        }

        public StashGroupObject CreateGroup(string name, bool withDateString = false)
        {
            if (withDateString) name = $"{name} ({DateTime.Now})";
            StashGroupObject group = new StashGroupObject(Global.Configuration.CreateStashGroup(name));
            Items.Add(group.ID, group);
            return group;
        }

        public bool DeleteGroup(StashGroupObject group)
        {
            if (Global.Configuration.IsMainStashGroupID(group.ID))
            {
                Console.AlertWarning(Global.L.CannotDeleteStashGroupMessage(group.Name, Global.L.StashGroupIsMainMessage()));
                return false;
            }
            if (Global.Stashes.GetStashesForGroup(group.ID).Length != 0)
            {
                Console.AlertWarning(Global.L.CannotDeleteStashGroupMessage(group.Name, Global.L.StashGroupIsNotEmptyMessage()));
                return false;
            }
            Items.Remove(group.ID);
            Global.Configuration.DeleteStashGroup(group.ID);
            if (group.ID == Global.Runtime.ActiveGroupID)
                Global.Runtime.ActiveGroupID = 0;
            return true;
        }

        public List<StashGroupObject> DeleteGroups(IEnumerable<StashGroupObject> list)
        {
            List<StashGroupObject> deletedItems = new List<StashGroupObject>();
            foreach (StashGroupObject toDelete in list)
            {
                if (DeleteGroup(toDelete))
                    deletedItems.Add(toDelete);
            }
            return deletedItems;
        }

    }
}
