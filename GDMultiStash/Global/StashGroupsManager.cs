using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMultiStash.Global
{
    using GDMultiStash.Common.Objects;
    using GDMultiStash.Common.Objects.Sorting.Comparer;
    using StashGroups;
    namespace StashGroups
    {

    }

    internal partial class StashGroupsManager : Base.ObjectsManager<StashGroupObject>
    {
        protected int _activeGroupID = 0;

        public int ActiveGroupID
        {
            get => _activeGroupID;
            set
            {
                if (value == _activeGroupID) return;
                var previous = _activeGroupID;
                _activeGroupID = value;
                Console.WriteLine($"active group changed: #{previous} -> #{_activeGroupID}");
                InvokeActiveGroupChanged(previous, _activeGroupID);
            }
        }

        public void LoadGroups()
        {
            Console.WriteLine($"Loading Stash Groups:");
            foreach (Common.Config.ConfigStashGroup cfgStashGroup in G.Configuration.StashGroups)
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
            StashGroupObject group = new StashGroupObject(G.Configuration.CreateStashGroup(name));
            Items.Add(group.ID, group);
            return group;
        }

        public bool DeleteGroup(StashGroupObject group)
        {
            if (G.Configuration.IsMainStashGroupID(group.ID))
            {
                Console.AlertWarning(G.L.CannotDeleteStashGroupMessage(group.Name, G.L.StashGroupIsMainMessage()));
                return false;
            }
            if (G.Stashes.GetStashesForGroup(group.ID).Length != 0)
            {
                Console.AlertWarning(G.L.CannotDeleteStashGroupMessage(group.Name, G.L.StashGroupIsNotEmptyMessage()));
                return false;
            }
            Items.Remove(group.ID);
            G.Configuration.DeleteStashGroup(group.ID);
            if (group.ID == ActiveGroupID)
                ActiveGroupID = 0;
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
