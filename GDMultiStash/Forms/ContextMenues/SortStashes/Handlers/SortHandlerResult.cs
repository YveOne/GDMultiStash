using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GDMultiStash.Common;
using GDMultiStash.Common.Objects;

namespace GDMultiStash.Forms.ContextMenues.SortStashes.Handlers
{
    internal struct SortHandlerResult
    {
        public StashGroupObject AddedGroup;
        public StashObject[] AddedStashes;
        public StashObject[] RemainingStashes;
    }
}
