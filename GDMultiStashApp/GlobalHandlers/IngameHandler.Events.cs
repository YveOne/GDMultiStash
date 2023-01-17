using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using GrimDawnLib;
using GDMultiStash.Common.Objects;

namespace GDMultiStash.GlobalHandlers
{
    partial class IngameHandler
    {
        public event EventHandler<EventArgs> StashReopenStart;
        public event EventHandler<EventArgs> StashReopenEnd;
        public event EventHandler<RuntimeHandler.ListEventArgs<StashObject>> RequestReopenStash;

        public void InvokeRequestReopenStash(StashObject stashes)
            => SaveInvoke(() => RequestReopenStash?.Invoke(null, new RuntimeHandler.ListEventArgs<StashObject>(stashes)));

        public void InvokeRequestReopenStash(IEnumerable<StashObject> stashes)
            => SaveInvoke(() => RequestReopenStash?.Invoke(null, new RuntimeHandler.ListEventArgs<StashObject>(stashes)));

        public void InvokeStashReopenStart()
            => SaveInvoke(() => StashReopenStart?.Invoke(null, EventArgs.Empty));

        public void InvokeStashReopenEnd()
            => SaveInvoke(() => StashReopenEnd?.Invoke(null, EventArgs.Empty));

    }
}
