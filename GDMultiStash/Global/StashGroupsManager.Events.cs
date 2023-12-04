using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMultiStash.Global
{
    using GDMultiStash.Common.Objects;
    using GDMultiStash.Global.Base;
    using StashGroups;
    namespace StashGroups
    {

        public class ActiveStashGroupChangedEventArgs : EventArgs
        {
            public int OldID { get; private set; }
            public int NewID { get; private set; }
            public ActiveStashGroupChangedEventArgs(int oldId, int newID)
            {
                OldID = oldId;
                NewID = newID;
            }
        }

        class StashGroupObjectsEventArgs : ObjectsEventArgs<StashGroupObject>
        {
            public StashGroupObjectsEventArgs(IEnumerable<StashGroupObject> list) : base(list)
            {
            }
            public StashGroupObjectsEventArgs(StashGroupObject group) : base(group)
            {
            }
        }

    }

    partial class StashGroupsManager
    {
        public event EventHandler<ActiveStashGroupChangedEventArgs> ActiveGroupChanged;

        public event EventHandler<StashGroupObjectsEventArgs> StashGroupsAdded;
        public event EventHandler<StashGroupObjectsEventArgs> StashGroupsRemoved;
        public event EventHandler<StashGroupObjectsEventArgs> StashGroupsMoved;
        public event EventHandler<StashGroupObjectsEventArgs> StashGroupsInfoChanged;

        public void InvokeActiveGroupChanged(int oldId, int newId)
            => SaveInvoke(() => ActiveGroupChanged?.Invoke(null, new ActiveStashGroupChangedEventArgs(oldId, newId)));

        public void InvokeStashGroupsMoved(StashGroupObject group)
            => SaveInvoke(() => StashGroupsMoved?.Invoke(null, new StashGroupObjectsEventArgs(group)));
        public void InvokeStashGroupsMoved(IEnumerable<StashGroupObject> groups)
            => SaveInvoke(() => StashGroupsMoved?.Invoke(null, new StashGroupObjectsEventArgs(groups)));
        public void InvokeStashGroupsInfoChanged(StashGroupObject group)
            => SaveInvoke(() => StashGroupsInfoChanged?.Invoke(null, new StashGroupObjectsEventArgs(group)));
        public void InvokeStashGroupsInfoChanged(IEnumerable<StashGroupObject> groups)
            => SaveInvoke(() => StashGroupsInfoChanged?.Invoke(null, new StashGroupObjectsEventArgs(groups)));
        public void InvokeStashGroupsAdded(StashGroupObject group)
            => SaveInvoke(() => StashGroupsAdded?.Invoke(null, new StashGroupObjectsEventArgs(group)));
        public void InvokeStashGroupsAdded(IEnumerable<StashGroupObject> groups)
            => SaveInvoke(() => StashGroupsAdded?.Invoke(null, new StashGroupObjectsEventArgs(groups)));
        public void InvokeStashGroupsRemoved(StashGroupObject group)
            => SaveInvoke(() => StashGroupsRemoved?.Invoke(null, new StashGroupObjectsEventArgs(group)));
        public void InvokeStashGroupsRemoved(IEnumerable<StashGroupObject> groups)
            => SaveInvoke(() => StashGroupsRemoved?.Invoke(null, new StashGroupObjectsEventArgs(groups)));

    }
}
