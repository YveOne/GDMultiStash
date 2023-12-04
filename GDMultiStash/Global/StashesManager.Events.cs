using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMultiStash.Global
{
    using GDMultiStash.Common.Objects;
    using GDMultiStash.Global.Base;
    using Stashes;
    namespace Stashes
    {

        public class ActiveStashChangedEventArgs : EventArgs
        {
            public int OldID { get; private set; }
            public int NewID { get; private set; }
            public ActiveStashChangedEventArgs(int oldId, int newID)
            {
                OldID = oldId;
                NewID = newID;
            }
        }

        class StashObjectsEventArgs : ObjectsEventArgs<StashObject>
        {
            public StashObjectsEventArgs(IEnumerable<StashObject> list) : base(list)
            {
            }
            public StashObjectsEventArgs(StashObject stash) : base(stash)
            {
            }
        }

        class StashesContentChangedEventArgs : ObjectsEventArgs<StashObject>
        {
            public bool NeedExport { get; private set; }
            public StashesContentChangedEventArgs(IEnumerable<StashObject> list, bool needExport) : base(list)
            {
                NeedExport = needExport;
            }
            public StashesContentChangedEventArgs(StashObject obj, bool needExport) : base(obj)
            {
                NeedExport = needExport;
            }
        }

    }

    partial class StashesManager
    {

        public event EventHandler<ActiveStashChangedEventArgs> ActiveStashChanged;

        public event EventHandler<StashObjectsEventArgs> StashesAdded;
        public event EventHandler<StashObjectsEventArgs> StashesRemoved;
        public event EventHandler<StashObjectsEventArgs> StashesMoved;
        public event EventHandler<StashObjectsEventArgs> StashesInfoChanged;
        public event EventHandler<StashesContentChangedEventArgs> StashesContentChanged;

        public void InvokeActiveStashChanged(int oldId, int newId)
            => SaveInvoke(() => ActiveStashChanged?.Invoke(null, new ActiveStashChangedEventArgs(oldId, newId)));

        public void InvokeStashesAdded(StashObject stash)
            => SaveInvoke(() => StashesAdded?.Invoke(null, new StashObjectsEventArgs(stash)));
        public void InvokeStashesAdded(IEnumerable<StashObject> stashes)
            => SaveInvoke(() => StashesAdded?.Invoke(null, new StashObjectsEventArgs(stashes)));
        public void InvokeStashesRemoved(StashObject stash)
            => SaveInvoke(() => StashesRemoved?.Invoke(null, new StashObjectsEventArgs(stash)));
        public void InvokeStashesRemoved(IEnumerable<StashObject> stashes)
            => SaveInvoke(() => StashesRemoved?.Invoke(null, new StashObjectsEventArgs(stashes)));
        public void InvokeStashesMoved(StashObject stashes)
            => SaveInvoke(() => StashesMoved?.Invoke(null, new StashObjectsEventArgs(stashes)));
        public void InvokeStashesMoved(IEnumerable<StashObject> stashes)
            => SaveInvoke(() => StashesMoved?.Invoke(null, new StashObjectsEventArgs(stashes)));
        public void InvokeStashesInfoChanged(StashObject stashes)
            => SaveInvoke(() => StashesInfoChanged?.Invoke(null, new StashObjectsEventArgs(stashes)));
        public void InvokeStashesInfoChanged(IEnumerable<StashObject> stashes)
            => SaveInvoke(() => StashesInfoChanged?.Invoke(null, new StashObjectsEventArgs(stashes)));
        public void InvokeStashesContentChanged(StashObject stashes, bool needExport)
            => SaveInvoke(() => StashesContentChanged?.Invoke(null, new StashesContentChangedEventArgs(stashes, needExport)));
        public void InvokeStashesContentChanged(IEnumerable<StashObject> stashes, bool needExport)
            => SaveInvoke(() => StashesContentChanged?.Invoke(null, new StashesContentChangedEventArgs(stashes, needExport)));

    }
}
