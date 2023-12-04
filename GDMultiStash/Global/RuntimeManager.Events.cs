using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMultiStash.Global
{
    using GrimDawnLib;
    using GDMultiStash.Common.Objects;
    using Runtime;
    namespace Runtime
    {

    }

    partial class RuntimeManager
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

        public event EventHandler<EventArgs> StashOpened;
        public event EventHandler<EventArgs> StashClosed;
        public event EventHandler<EventArgs> TransferFileSaved;

        public event EventHandler<EventArgs> GameWindowConnected;
        public event EventHandler<EventArgs> GameWindowDisconnected;
        public event EventHandler<EventArgs> GameWindowGotFocus;
        public event EventHandler<EventArgs> GameWindowLostFocus;
        public event EventHandler<GameWindowSizeChangedEventArgs> GameWindowSizeChanged;
        public event EventHandler<GameWindowLocationChangedEventArgs> GameWindowLocationChanged;

        public event EventHandler<ShownExpansionChangedEventArgs> ShownExpansionChanged;

        public void InvokeActiveExpansionChanged(GrimDawnGameExpansion exp)
            => SaveInvoke(() => ActiveExpansionChanged?.Invoke(null, new ActiveExpansionChangedEventArgs(exp)));
        public void InvokeActiveModNameChanged(string modName)
             => SaveInvoke(() => ActiveModNameChanged?.Invoke(null, new ActiveModNameChangedEventArgs(modName)));
        public void InvokeActiveModeChanged(GrimDawnGameMode mode)
            => SaveInvoke(() => ActiveModeChanged?.Invoke(null, new ActiveModeChangedEventArgs(mode)));
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

        public void InvokeShownExpansionChanged(GrimDawnGameExpansion exp)
            => SaveInvoke(() => ShownExpansionChanged?.Invoke(null, new ShownExpansionChangedEventArgs(exp)));

    }
}
