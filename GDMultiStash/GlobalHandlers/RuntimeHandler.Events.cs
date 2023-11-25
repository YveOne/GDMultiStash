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
    partial class RuntimeHandler
    {
        public class ActiveExpansionChangedEventArgs : EventArgs
        {
            public GrimDawnGameExpansion Expansion { get => _expansion; }
            private GrimDawnGameExpansion _expansion;
            public ActiveExpansionChangedEventArgs(GrimDawnGameExpansion expansion)
            {
                _expansion = expansion;
            }
        }

        public class ActiveModNameChangedEventArgs : EventArgs
        {
            public string ModName { get => _modName; }
            private string _modName;
            public ActiveModNameChangedEventArgs(string modName)
            {
                _modName = modName;
            }
        }

        public class ActiveModeChangedEventArgs : EventArgs
        {
            public GrimDawnGameMode Mode { get => _mode; }
            private GrimDawnGameMode _mode;
            public ActiveModeChangedEventArgs(GrimDawnGameMode mode)
            {
                _mode = mode;
            }
        }

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

        public class ActiveGroupChangedEventArgs : EventArgs
        {
            public int OldID { get; private set; }
            public int NewID { get; private set; }
            public ActiveGroupChangedEventArgs(int oldId, int newID)
            {
                OldID = oldId;
                NewID = newID;
            }
        }

        public class GameWindowSizeChangedEventArgs : EventArgs
        {
            public System.Drawing.Size Size { get; private set; }
            public GameWindowSizeChangedEventArgs(System.Drawing.Size size)
            {
                this.Size = size;
            }
        }

        public class GameWindowLocationChangedEventArgs : EventArgs
        {
            public System.Drawing.Point Point { get; private set; }
            public GameWindowLocationChangedEventArgs(System.Drawing.Point point)
            {
                this.Point = point;
            }
        }

        public class ListEventArgs<T> : EventArgs
        {
            public T[] Items { get; private set; }
            public ListEventArgs(IEnumerable<T> list)
            {
                Items = list.ToArray();
            }
            public ListEventArgs(T obj)
            {
                Items = new T[] { obj };
            }
        }

        public class StashesContentChangedEventArgs : ListEventArgs<StashObject>
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

        public class ShownExpansionChangedEventArgs : EventArgs
        {
            public GrimDawnGameExpansion Expansion { get => _expansion; }
            private GrimDawnGameExpansion _expansion;
            public ShownExpansionChangedEventArgs(GrimDawnGameExpansion expansion)
            {
                _expansion = expansion;
            }
        }

        public event EventHandler<ActiveExpansionChangedEventArgs> ActiveExpansionChanged;
        public event EventHandler<ActiveModNameChangedEventArgs> ActiveModNameChanged;
        public event EventHandler<ActiveModeChangedEventArgs> ActiveModeChanged;
        public event EventHandler<ActiveStashChangedEventArgs> ActiveStashChanged;
        public event EventHandler<ActiveGroupChangedEventArgs> ActiveGroupChanged;

        public event EventHandler<EventArgs> StashOpened;
        public event EventHandler<EventArgs> StashClosed;
        public event EventHandler<EventArgs> TransferFileSaved;

        public event EventHandler<EventArgs> GameWindowConnected;
        public event EventHandler<EventArgs> GameWindowDisconnected;
        public event EventHandler<EventArgs> GameWindowGotFocus;
        public event EventHandler<EventArgs> GameWindowLostFocus;
        public event EventHandler<GameWindowSizeChangedEventArgs> GameWindowSizeChanged;
        public event EventHandler<GameWindowLocationChangedEventArgs> GameWindowLocationChanged;

        public event EventHandler<ListEventArgs<StashObject>> StashesAdded;
        public event EventHandler<ListEventArgs<StashObject>> StashesRemoved;
        public event EventHandler<ListEventArgs<StashObject>> StashesMoved;
        public event EventHandler<ListEventArgs<StashObject>> StashesInfoChanged;
        public event EventHandler<StashesContentChangedEventArgs> StashesContentChanged;

        public event EventHandler<ListEventArgs<StashGroupObject>> StashGroupsAdded;
        public event EventHandler<ListEventArgs<StashGroupObject>> StashGroupsRemoved;
        public event EventHandler<ListEventArgs<StashGroupObject>> StashGroupsMoved;
        public event EventHandler<ListEventArgs<StashGroupObject>> StashGroupsInfoChanged;

        public event EventHandler<EventArgs> CharacterMovementEnabled;
        public event EventHandler<EventArgs> CharacterMovementDisabled;

        public event EventHandler<ShownExpansionChangedEventArgs> ShownExpansionChanged;

        public void InvokeActiveExpansionChanged(GrimDawnGameExpansion exp)
            => SaveInvoke(() => ActiveExpansionChanged?.Invoke(null, new ActiveExpansionChangedEventArgs(exp)));
        public void InvokeActiveModNameChanged(string modName)
             => SaveInvoke(() => ActiveModNameChanged?.Invoke(null, new ActiveModNameChangedEventArgs(modName)));
        public void InvokeActiveModeChanged(GrimDawnGameMode mode)
            => SaveInvoke(() => ActiveModeChanged?.Invoke(null, new ActiveModeChangedEventArgs(mode)));
        public void InvokeActiveStashChanged(int oldId, int newId)
            => SaveInvoke(() => ActiveStashChanged?.Invoke(null, new ActiveStashChangedEventArgs(oldId, newId)));
        public void InvokeActiveGroupChanged(int oldId, int newId)
            => SaveInvoke(() => ActiveGroupChanged?.Invoke(null, new ActiveGroupChangedEventArgs(oldId, newId)));
        public void InvokeStashOpened()
            => SaveInvoke(() => StashOpened?.Invoke(null, EventArgs.Empty));
        public void InvokeStashClosed()
            => SaveInvoke(() => StashClosed?.Invoke(null, EventArgs.Empty));

        public void InvokeTransferStashSaved()
        {
            if (!_stashWasOpened) return;
            _stashWasOpened = false;
            TransferFileSaved?.Invoke(null, EventArgs.Empty);
        }

        public void InvokeGameWindowConnected()
            => SaveInvoke(() => GameWindowConnected?.Invoke(null, EventArgs.Empty));
        public void InvokeGameWindowDisconnected()
            => SaveInvoke(() => GameWindowDisconnected?.Invoke(null, EventArgs.Empty));
        public void InvokeGameWindowGotFocus()
            => SaveInvoke(() => GameWindowGotFocus?.Invoke(null, EventArgs.Empty));
        public void InvokeGameWindowLostFocus()
            => SaveInvoke(() => GameWindowLostFocus?.Invoke(null, EventArgs.Empty));
        public void InvokeGameWindowSizeChanged(System.Drawing.Size size)
            => SaveInvoke(() => GameWindowSizeChanged?.Invoke(null, new GameWindowSizeChangedEventArgs(size)));
        public void InvokeGameWindowLocationChanged(System.Drawing.Point point)
            => SaveInvoke(() => GameWindowLocationChanged?.Invoke(null, new GameWindowLocationChangedEventArgs(point)));

        public void InvokeStashesAdded(StashObject stash)
            => SaveInvoke(() => StashesAdded?.Invoke(null, new ListEventArgs<StashObject>(stash)));
        public void InvokeStashesAdded(IEnumerable<StashObject> stashes)
            => SaveInvoke(() => StashesAdded?.Invoke(null, new ListEventArgs<StashObject>(stashes)));
        public void InvokeStashesRemoved(StashObject stash)
            => SaveInvoke(() => StashesRemoved?.Invoke(null, new ListEventArgs<StashObject>(stash)));
        public void InvokeStashesRemoved(IEnumerable<StashObject> stashes)
            => SaveInvoke(() => StashesRemoved?.Invoke(null, new ListEventArgs<StashObject>(stashes)));
        public void InvokeStashesMoved(StashObject stashes)
            => SaveInvoke(() => StashesMoved?.Invoke(null, new ListEventArgs<StashObject>(stashes)));
        public void InvokeStashesMoved(IEnumerable<StashObject> stashes)
            => SaveInvoke(() => StashesMoved?.Invoke(null, new ListEventArgs<StashObject>(stashes)));
        public void InvokeStashesInfoChanged(StashObject stashes)
            => SaveInvoke(() => StashesInfoChanged?.Invoke(null, new ListEventArgs<StashObject>(stashes)));
        public void InvokeStashesInfoChanged(IEnumerable<StashObject> stashes)
            => SaveInvoke(() => StashesInfoChanged?.Invoke(null, new ListEventArgs<StashObject>(stashes)));
        public void InvokeStashesContentChanged(StashObject stashes, bool needExport)
            => SaveInvoke(() => StashesContentChanged?.Invoke(null, new StashesContentChangedEventArgs(stashes, needExport)));
        public void InvokeStashesContentChanged(IEnumerable<StashObject> stashes, bool needExport)
            => SaveInvoke(() => StashesContentChanged?.Invoke(null, new StashesContentChangedEventArgs(stashes, needExport)));

        public void InvokeStashGroupsMoved(StashGroupObject group)
            => SaveInvoke(() => StashGroupsMoved?.Invoke(null, new ListEventArgs<StashGroupObject>(group)));
        public void InvokeStashGroupsMoved(IEnumerable<StashGroupObject> groups)
            => SaveInvoke(() => StashGroupsMoved?.Invoke(null, new ListEventArgs<StashGroupObject>(groups)));
        public void InvokeStashGroupsInfoChanged(StashGroupObject group)
            => SaveInvoke(() => StashGroupsInfoChanged?.Invoke(null, new ListEventArgs<StashGroupObject>(group)));
        public void InvokeStashGroupsInfoChanged(IEnumerable<StashGroupObject> groups)
            => SaveInvoke(() => StashGroupsInfoChanged?.Invoke(null, new ListEventArgs<StashGroupObject>(groups)));
        public void InvokeStashGroupsAdded(StashGroupObject group)
            => SaveInvoke(() => StashGroupsAdded?.Invoke(null, new ListEventArgs<StashGroupObject>(group)));
        public void InvokeStashGroupsAdded(IEnumerable<StashGroupObject> groups)
            => SaveInvoke(() => StashGroupsAdded?.Invoke(null, new ListEventArgs<StashGroupObject>(groups)));
        public void InvokeStashGroupsRemoved(StashGroupObject group)
            => SaveInvoke(() => StashGroupsRemoved?.Invoke(null, new ListEventArgs<StashGroupObject>(group)));
        public void InvokeStashGroupsRemoved(IEnumerable<StashGroupObject> groups)
            => SaveInvoke(() => StashGroupsRemoved?.Invoke(null, new ListEventArgs<StashGroupObject>(groups)));

        public void InvokeCharacterMovementEnabled()
            => SaveInvoke(() => CharacterMovementEnabled?.Invoke(null, EventArgs.Empty));
        public void InvokeCharacterMovementDisabled()
            => SaveInvoke(() => CharacterMovementDisabled?.Invoke(null, EventArgs.Empty));

        public void InvokeShownExpansionChanged(GrimDawnGameExpansion exp)
            => SaveInvoke(() => ShownExpansionChanged?.Invoke(null, new ShownExpansionChangedEventArgs(exp)));

    }
}
