using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using GrimDawnLib;

namespace GDMultiStash
{
    internal static partial class Core
    {
        internal static partial class Runtime
        {

            #region MainStashID

            public static int MainStashID
            {
                get
                {
                    switch (CurrentExpansion)
                    {
                        case GrimDawnGameExpansion.BaseGame:
                            if (CurrentMode == GrimDawnGameMode.SC) return Config.Main0SCID;
                            if (CurrentMode == GrimDawnGameMode.HC) return Config.Main0HCID;
                            break;
                        case GrimDawnGameExpansion.AshesOfMalmouth:
                            if (CurrentMode == GrimDawnGameMode.SC) return Config.Main1SCID;
                            if (CurrentMode == GrimDawnGameMode.HC) return Config.Main1HCID;
                            break;
                        case GrimDawnGameExpansion.ForgottenGods:
                            if (CurrentMode == GrimDawnGameMode.SC) return Config.Main2SCID;
                            if (CurrentMode == GrimDawnGameMode.HC) return Config.Main2HCID;
                            break;
                    }
                    return -1;
                }
            }

            #endregion

            #region ActiveStashID

            public delegate void ActiveStashChangedEventHandler(object sender, EventArgs e);
            public static event ActiveStashChangedEventHandler ActiveStashChanged;

            private static int _activeStashID = -1;
            public static int ActiveStashID
            {
                get
                {
                    return _activeStashID;
                }
                set
                {
                    if (value == _activeStashID) return;
                    _activeStashID = value;
                    switch (CurrentExpansion)
                    {
                        case GrimDawnGameExpansion.BaseGame:
                            if (CurrentMode == GrimDawnGameMode.SC) Config.Cur0SCID = value;
                            if (CurrentMode == GrimDawnGameMode.HC) Config.Cur0HCID = value;
                            break;
                        case GrimDawnGameExpansion.AshesOfMalmouth:
                            if (CurrentMode == GrimDawnGameMode.SC) Config.Cur1SCID = value;
                            if (CurrentMode == GrimDawnGameMode.HC) Config.Cur1HCID = value;
                            break;
                        case GrimDawnGameExpansion.ForgottenGods:
                            if (CurrentMode == GrimDawnGameMode.SC) Config.Cur2SCID = value;
                            if (CurrentMode == GrimDawnGameMode.HC) Config.Cur2HCID = value;
                            break;
                    }
                    Console.WriteLine("Runtime: Active stash changed: " + _activeStashID);
                    ActiveStashChanged?.Invoke(null, EventArgs.Empty);
                }
            }

            public static void LoadActiveStashID()
            {
                switch (CurrentExpansion)
                {
                    case GrimDawnGameExpansion.BaseGame:
                        if (CurrentMode == GrimDawnGameMode.SC) ActiveStashID = Config.Cur0SCID;
                        if (CurrentMode == GrimDawnGameMode.HC) ActiveStashID = Config.Cur0HCID;
                        break;
                    case GrimDawnGameExpansion.AshesOfMalmouth:
                        if (CurrentMode == GrimDawnGameMode.SC) ActiveStashID = Config.Cur1SCID;
                        if (CurrentMode == GrimDawnGameMode.HC) ActiveStashID = Config.Cur1HCID;
                        break;
                    case GrimDawnGameExpansion.ForgottenGods:
                        if (CurrentMode == GrimDawnGameMode.SC) ActiveStashID = Config.Cur2SCID;
                        if (CurrentMode == GrimDawnGameMode.HC) ActiveStashID = Config.Cur2HCID;
                        break;
                    default:
                        ActiveStashID = -1;
                        break;
                }
            }

            #endregion

            #region Event: StashesChanged

            public delegate void StashesChangedEventHandler(object sender, EventArgs e);
            public static event StashesChangedEventHandler StashesChanged;

            public static void NotifyStashesChanged()
            {
                StashesChanged?.Invoke(null, EventArgs.Empty);
            }

            #endregion

            #region Event: Window

            public delegate void WindowFocusChangedEventHandler(object sender, EventArgs e);
            public static event WindowFocusChangedEventHandler WindowFocusChanged;

            private static bool _hasFocus = false;

            public static bool WindowFocused
            {
                get => _hasFocus;
                set {
                    if (_hasFocus == value) return;
                    _hasFocus = value;
                    WindowFocusChanged?.Invoke(null, EventArgs.Empty);
                }
            }

            public delegate void WindowSizeChangedEventHandler(object sender, EventArgs e);
            public static event WindowSizeChangedEventHandler WindowSizeChanged;

            public delegate void WindowLocationChangedEventHandler(object sender, EventArgs e);
            public static event WindowLocationChangedEventHandler WindowLocationChanged;

            private static bool _windowLocSizeSet = false;
            private static System.Drawing.Point _windowLocation;
            private static System.Drawing.Size _windowSize;

            public static System.Drawing.Point WindowLocation
            {
                get => _windowLocation;
                private set
                {
                    System.Drawing.Point last = _windowLocation;
                    _windowLocation = value;
                    if (!_windowLocSizeSet || last.X != value.X || last.Y != value.Y)
                        WindowLocationChanged?.Invoke(null, EventArgs.Empty);
                }
            }

            public static System.Drawing.Size WindowSize
            {
                get => _windowSize;
                private set
                {
                    System.Drawing.Size last = _windowSize;
                    _windowSize = value;
                    if (!_windowLocSizeSet || last.Width != value.Width || last.Height != value.Height)
                        WindowSizeChanged?.Invoke(null, EventArgs.Empty);
                    
                }
            }

            public static void SetWindowLocSize(Services.GDWindowHookService.LocationSize v)
            {
                if (v.X == -32000)
                {
                    // minimized
                    WindowFocused = false;
                }
                else
                {
                    WindowFocused = true;
                    WindowLocation = new System.Drawing.Point(v.X, v.Y);
                    WindowSize = new System.Drawing.Size(v.Width, v.Height);
                    _windowLocSizeSet = true;
                }
            }

            #endregion

            #region Event: StashStatusChanged

            public delegate void StashStatusChangedEventHandler(object sender, EventArgs e);
            public static event StashStatusChangedEventHandler StashStatusChanged;

            private static bool _stashOpened = false;
            public static bool StashOpened
            {
                get { return _stashOpened; }
                set
                {
                    if (_stashOpened == value) return;
                    _stashOpened = value;
                    Console.WriteLine("Runtime: StashStatusChanged: " + value);
                    if (_stashReopening)
                    {
                        Console.WriteLine("  reopening... skipped");
                        return;
                    }
                    StashStatusChanged?.Invoke(null, EventArgs.Empty);
                }
            }

            #endregion

            #region Event: CurrentModeChanged

            public delegate void CurrentModeChangedEventHandler(object sender, EventArgs e);
            public static event CurrentModeChangedEventHandler CurrentModeChanged;

            private static GrimDawnGameMode _currentMode = GrimDawnGameMode.None;
            public static GrimDawnGameMode CurrentMode
            {
                get { return _currentMode; }
                set
                {
                    if (_currentMode == value) return;
                    _currentMode = value;
                    Console.WriteLine("Runtime: CurrentModeChanged: " + value);
                    CurrentModeChanged?.Invoke(null, EventArgs.Empty);
                    LoadActiveStashID();
                }
            }

            #endregion

            #region Event: CurrentExpansionChanged

            public delegate void CurrentExpansionChangedEventHandler(object sender, EventArgs e);
            public static event CurrentExpansionChangedEventHandler CurrentExpansionChanged;

            private static GrimDawnGameExpansion _currentExpansion = GrimDawnGameExpansion.Unknown;
            public static GrimDawnGameExpansion CurrentExpansion
            {
                get { return _currentExpansion; }
                set
                {
                    if (_currentExpansion == value) return;
                    _currentExpansion = value;
                    Console.WriteLine("Runtime: CurrentExpansionChanged: " + value);
                    CurrentExpansionChanged?.Invoke(null, EventArgs.Empty);
                    LoadActiveStashID();
                }
            }

            #endregion

            static Runtime()
            {
                WindowFocusChanged += delegate
                {
                    if (WindowFocused)
                    {
                        if (_needLoadStash)
                        {
                            _needLoadStash = false;
                            new System.Threading.Thread(new System.Threading.ThreadStart(() => {
                                System.Threading.Thread.Sleep(500);
                                LoadCurrentStash();
                            })).Start();
                        }
                    }
                };
                StashStatusChanged += delegate {
                    if (!_stashOpened && !_stashReopening && Config.AutoBackToMain)
                    {
                        // we dont need to use reopen because stash is closed
                        Stashes.SwitchToStash(MainStashID);
                    }
                };
            }

            private static bool _needLoadStash = false;

            public static bool IsStashOpened(int stashID)
            {
                return (StashOpened && stashID == ActiveStashID);
            }

            public static void RequestLoadingStash()
            {
                _needLoadStash = true;
            }

            private static bool _stashReopening = false;

            private delegate bool SleepForConditionDelegate();
            private static void SleepFor(int time, SleepForConditionDelegate condition = null)
            {
                long timeout = Environment.TickCount + time;
                while(Environment.TickCount < timeout)
                {
                    if (condition != null)
                        if (!condition()) return;
                    System.Threading.Thread.Sleep(1);
                    Application.DoEvents();
                }
            }





            private static void _ReopenStashAction(Action action)
            {
                Console.WriteLine("Reopen stash...");

                ushort keyEscape = (ushort)Native.Keyboard.KeyToScanCode(Keys.Escape);
                ushort keyInteract = GrimDawn.Keybindings.GetKeyBinding(GrimDawnKey.Interact).Primary;
                int triesLeft;

                SleepFor(100);

                triesLeft = 3;
                while (triesLeft > 0 && _stashOpened)
                {
                    triesLeft -= 1;
                    Native.Keyboard.SendKey(keyEscape);
                    SleepFor(1000, () => _stashOpened);
                    if (_stashOpened)
                        Console.WriteLine("ReopenStash() failed! stash not closed. Tries left: " + triesLeft);
                    else
                        break;
                }
                if (_stashOpened)
                {
                    Console.WriteLine("ReopenStash() failed! stash not closed");
                    return;
                }

                SleepFor(100);
                action();
                SleepFor(100);

                triesLeft = 3;
                while (triesLeft > 0 && !_stashOpened)
                {
                    triesLeft -= 1;
                    Native.Keyboard.SendKey(keyInteract);
                    SleepFor(1000, () => !_stashOpened);
                    if (!_stashOpened)
                        Console.WriteLine("ReopenStash() failed! stash not reopened. Tries left: " + triesLeft);
                    else
                        break;
                }
                if (!_stashOpened)
                {
                    Console.WriteLine("ReopenStash() failed! stash not reopened!");
                    return;
                }
            }

            private static void ReopenStashAction(Action action)
            {
                _stashReopening = true;
                _ReopenStashAction(action);
                _stashReopening = false;
                if (!StashOpened)
                {
                    // broadcast that the stash is closed
                    StashStatusChanged?.Invoke(null, EventArgs.Empty);
                }
            }

            public static void SwitchToStash(int stashID)
            {
                if (stashID == _activeStashID) return;
                Console.WriteLine("Switching to stash #" + stashID);
                ReopenStashAction(() => Stashes.SwitchToStash(stashID));
            }

            public static void SaveCurrentStash()
            {
                Console.WriteLine("Saving Stash");
                ReopenStashAction(() => Stashes.ImportStash(ActiveStashID));
            }

            public static void LoadCurrentStash()
            {
                Console.WriteLine("Loading Stash");
                ReopenStashAction(() => Stashes.ExportStash(ActiveStashID));
            }

            private static bool moveDisabled = false;

            public static void EnableMovement()
            {
                if (!moveDisabled) return;
                moveDisabled = false;
                ushort k = GrimDawn.Keybindings.GetKeyBinding(GrimDawnKey.StationaryAttack).Primary;
                Native.Keyboard.SendKeyUp(k);
            }

            public static void DisableMovement()
            {
                if (moveDisabled) return;
                moveDisabled = true;
                ushort k = GrimDawn.Keybindings.GetKeyBinding(GrimDawnKey.StationaryAttack).Primary;
                Native.Keyboard.SendKeyDown(k);
            }

        }
    }
}
