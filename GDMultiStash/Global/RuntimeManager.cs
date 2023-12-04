using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GDMultiStash.Global
{
    using GrimDawnLib;
    using Runtime;
    namespace Runtime
    {

    }

    internal partial class RuntimeManager : Base.Manager
    {
        protected GrimDawnGameExpansion _activeExpansion = GrimDawnGameExpansion.Unknown;
        protected GrimDawnGameMode _activeMode = GrimDawnGameMode.None;
        protected string _activeModName = "???";
        protected bool _stashWasOpened = false; // this is used to trigger NotifyTransferStashSaved() only once
        protected bool _stashIsOpened = false;
        protected bool _gameWindowHasFocus = false;
        protected bool _gameWindowIsConnected = false;
        protected bool _windowLocSizeSet = false;
        protected Point _windowLocation;
        protected Size _windowSize;
        protected GrimDawnGameExpansion _shownExpansion = GrimDawnGameExpansion.Unknown;

        internal override void Initialize()
        {

        }

        public GrimDawnGameExpansion ActiveExpansion
        {
            get => _activeExpansion;
            set
            {
                if (_activeExpansion == value) return;
                var previous = _activeExpansion;
                _activeExpansion = value;
                Console.WriteLine($"active expansion changed: {previous} -> {_activeExpansion}");
                InvokeActiveExpansionChanged(_activeExpansion);
            }
        }

        public GrimDawnGameMode ActiveMode
        {
            get => _activeMode;
            set
            {
                if (_activeMode == value) return;
                var previous = _activeMode;
                _activeMode = value;
                Console.WriteLine($"active mode changed: {previous} -> {_activeMode}");
                InvokeActiveModeChanged(_activeMode);
            }
        }

        public string ActiveModName
        {
            get => _activeModName;
            set
            {
                if (_activeModName == value) return;
                var previous = _activeModName;
                _activeModName = value;
                Console.WriteLine($"active mod name changed: {previous} -> {_activeModName}");
                InvokeActiveModNameChanged(_activeModName);
            }
        }

        public bool StashIsOpened
        {
            get { return _stashIsOpened; }
            set
            {
                if (_activeMode == GrimDawnGameMode.None) return; // window may be shown on main screen Oo?

                if (_stashIsOpened == value) return;
                _stashIsOpened = value;
                _stashWasOpened = _stashWasOpened || value;
                Console.WriteLine($"stash is opened: {value}");
                if (!G.Ingame.StashIsReopening)
                {
                    if (_stashIsOpened) InvokeStashOpened();
                    else InvokeStashClosed();
                }
                else
                {
                    Console.WriteLine($"- reopening ingame");
                }
            }
        }

        public bool GameWindowFocused
        {
            get => _gameWindowHasFocus;
            set
            {
                if (_gameWindowHasFocus == value) return;
                _gameWindowHasFocus = value;
                if (_gameWindowHasFocus)
                    InvokeGameWindowGotFocus();
                else
                    InvokeGameWindowLostFocus();
            }
        }

        public bool GameWindowIsConnected
        {
            get => _gameWindowIsConnected;
            set
            {
                if (_gameWindowIsConnected == value) return;
                _gameWindowIsConnected = value;
                if (_gameWindowIsConnected)
                    InvokeGameWindowConnected();
                else
                    InvokeGameWindowDisconnected();
            }
        }

        public Point GameWindowLocation
        {
            get => _windowLocation;
            set
            {
                Point last = _windowLocation;
                _windowLocation = value;
                if (!_windowLocSizeSet || last.X != value.X || last.Y != value.Y)
                    InvokeGameWindowLocationChanged(_windowLocation);
            }
        }

        public Size GameWindowSize
        {
            get => _windowSize;
            set
            {
                Size last = _windowSize;
                _windowSize = value;
                if (!_windowLocSizeSet || last.Width != value.Width || last.Height != value.Height)
                    InvokeGameWindowSizeChanged(_windowSize);
            }
        }

        public bool GameInitialized => G.Stashes.ActiveStashID != -1
            && ActiveExpansion != GrimDawnGameExpansion.Unknown
            && ActiveMode != GrimDawnGameMode.None;

        public void SetGameWindowLocSize(Services.GDWindowHookService.LocationSize v)
        {
            if (v.Width == 0 || v.Height == 0)
            {
                // minimized
                GameWindowFocused = false;
            }
            else
            {
                GameWindowFocused = true;
                GameWindowLocation = new Point(v.X, v.Y);
                GameWindowSize = new Size(v.Width, v.Height);
                _windowLocSizeSet = true;
            }
        }

        public GrimDawnGameExpansion ShownExpansion
        {
            get => _shownExpansion;
            set
            {
                if (_shownExpansion == value) return;
                var previous = _shownExpansion;
                _shownExpansion = value;
                Console.WriteLine($"shown expansion changed: {previous} -> {_shownExpansion}");
                InvokeShownExpansionChanged(_shownExpansion);
            }
        }

    }
}
