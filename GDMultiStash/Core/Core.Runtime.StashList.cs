using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMultiStash
{
    internal static partial class Core
    {
        internal static partial class Runtime
        {

            private static class StashList
            {
                static StashList()
                {
                }
            }

            public class StashListChangedEventArgs : EventArgs
            {
                public Common.Stash[] Stashes { get => _stashes; }
                private Common.Stash[] _stashes;
                public StashListChangedEventArgs(IEnumerable<Common.Stash> stashes)
                {
                    _stashes = stashes.ToArray();
                }
                public StashListChangedEventArgs(Common.Stash stash)
                {
                    _stashes = new Common.Stash[] { stash };
                }
                public StashListChangedEventArgs()
                {
                    _stashes = new Common.Stash[] { };
                }
            }

            public delegate void StashListChangedEventHandler(object sender, StashListChangedEventArgs e);
            public static event StashListChangedEventHandler StashesOrderChanged;
            public static event StashListChangedEventHandler StashesAdded;
            public static event StashListChangedEventHandler StashesRemoved;
            public static event StashListChangedEventHandler StashesRestored;
            public static event StashListChangedEventHandler StashesUpdated;

            public static void NotifyStashesOrderChanged()
            {
                StashesOrderChanged?.Invoke(null, new StashListChangedEventArgs());
            }

            public static void NotifyStashesAdded(Common.Stash stash)
            {
                StashesAdded?.Invoke(null, new StashListChangedEventArgs(stash));
            }

            public static void NotifyStashesAdded(IEnumerable<Common.Stash> stashes)
            {
                StashesAdded?.Invoke(null, new StashListChangedEventArgs(stashes));
            }

            public static void NotifyStashesRemoved(Common.Stash stash)
            {
                StashesRemoved?.Invoke(null, new StashListChangedEventArgs(stash));
            }

            public static void NotifyStashesRemoved(IEnumerable<Common.Stash> stashes)
            {
                StashesRemoved?.Invoke(null, new StashListChangedEventArgs(stashes));
            }

            public static void NotifyStashesRestored(Common.Stash stash)
            {
                StashesRestored?.Invoke(null, new StashListChangedEventArgs(stash));
            }

            public static void NotifyStashesRestored(IEnumerable<Common.Stash> stashes)
            {
                StashesRestored?.Invoke(null, new StashListChangedEventArgs(stashes));
            }

            public static void NotifyStashesUpdated(Common.Stash stash)
            {
                StashesUpdated?.Invoke(null, new StashListChangedEventArgs(stash));
            }

            public static void NotifyStashesUpdated(IEnumerable<Common.Stash> stashes)
            {
                StashesUpdated?.Invoke(null, new StashListChangedEventArgs(stashes));
            }

        }
    }
}
