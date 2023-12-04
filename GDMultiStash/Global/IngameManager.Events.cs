using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GDMultiStash.Global
{
    using GDMultiStash.Common.Objects;
    using Ingame;
    namespace Ingame
    {

    }

    partial class IngameManager
    {
        public event EventHandler<EventArgs> StashReopenStart;
        public event EventHandler<EventArgs> StashReopenEnd;
        public event EventHandler<Stashes.StashObjectsEventArgs> RequestReopenStash;
        public event EventHandler<EventArgs> CharacterMovementEnabled;
        public event EventHandler<EventArgs> CharacterMovementDisabled;

        public void InvokeRequestReopenStash(StashObject stashes)
            => SaveInvoke(() => RequestReopenStash?.Invoke(null, new Stashes.StashObjectsEventArgs(stashes)));

        public void InvokeRequestReopenStash(IEnumerable<StashObject> stashes)
            => SaveInvoke(() => RequestReopenStash?.Invoke(null, new Stashes.StashObjectsEventArgs(stashes)));

        public void InvokeStashReopenStart()
            => SaveInvoke(() => StashReopenStart?.Invoke(null, EventArgs.Empty));

        public void InvokeStashReopenEnd()
            => SaveInvoke(() => StashReopenEnd?.Invoke(null, EventArgs.Empty));

        public void InvokeCharacterMovementEnabled()
            => SaveInvoke(() => CharacterMovementEnabled?.Invoke(null, EventArgs.Empty));
        public void InvokeCharacterMovementDisabled()
            => SaveInvoke(() => CharacterMovementDisabled?.Invoke(null, EventArgs.Empty));

    }
}
