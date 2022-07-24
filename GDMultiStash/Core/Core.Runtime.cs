using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using GrimDawnLib;

namespace GDMultiStash
{
    internal static partial class Core
    {
        internal static partial class Runtime
        {









            #region Event: Window





            // todo: rename to "GameWindow"

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

            #region Event: TransferStashSaved

            public delegate void StashStatusChangedEventHandler(object sender, EventArgs e);
            public static event StashStatusChangedEventHandler StashStatusChanged;

            private static bool _stashOpened = false;
            private static bool _stashWasOpened = false;


            public static bool StashOpened
            {
                get { return _stashOpened; }
                set
                {
                    if (_stashOpened == value) return;
                    _stashOpened = value;
                    _stashWasOpened = _stashWasOpened || value;
                    Console.WriteLine("Runtime: StashOpened: " + value);
                    if (_stashReopening)
                    {
                        Console.WriteLine("  reopening... skipped");
                        return;
                    }
                    StashStatusChanged?.Invoke(null, EventArgs.Empty);
                }
            }

            public delegate void TransferStashSavedEventHandler(object sender, EventArgs e);
            public static event TransferStashSavedEventHandler TransferStashSaved;

            public static void NotifyTransferStashSaved()
            {
                if (!_stashWasOpened) return;
                _stashWasOpened = false;
                TransferStashSaved?.Invoke(null, EventArgs.Empty);
            }

            #endregion

            static Runtime()
            {

                ActiveModeChanged += delegate { LoadActiveStashID(); };
                ActiveExpansionChanged += delegate { LoadActiveStashID(); };

                WindowFocusChanged += delegate
                {
                    if (WindowFocused)
                    {
                        if (_reloadOpenedStash)
                        {
                            _reloadOpenedStash = false;
                            new System.Threading.Thread(new System.Threading.ThreadStart(() => {
                                System.Threading.Thread.Sleep(500);
                                LoadCurrentStash();
                            })).Start();
                        }
                    }
                };

                TransferStashSaved += delegate
                {
                    Console.WriteLine("Runtime: TransferStashSaved START!");
                    Console.WriteLine("    _stashReopening: " + _stashReopening.ToString());
                    Console.WriteLine("    _stashOpened: " + _stashOpened.ToString());
                    if (!_stashReopening && !_stashOpened)
                    {
                        int closedID = _activeStashID;
                        Console.WriteLine("Runtime: TransferStashSaved - check file locked");
                        string externalFile = GrimDawn.GetTransferFilePath(_currentExpansion, _activeMode);
                        try
                        {
                            File.Open(externalFile, FileMode.Open).Close();
                        }
                        catch(Exception)
                        {
                            // stash reopened by using keybinding
                            Console.WriteLine("Runtime: TransferStashSaved - file locked - return");
                            return;
                        }
                        Console.WriteLine("Runtime: TransferStashSaved - file not locked");

                        if (Config.AutoBackToMain)
                        {
                            Console.WriteLine("Runtime: TransferStashSaved - Config.AutoBackToMain");
                            // we dont need to use reopen because stash is closed

                            int mainStashID = Config.GetMainStashID(CurrentExpansion, CurrentMode);
                            Stashes.SwitchToStash(mainStashID);
                        }
                        else
                        {
                            // import the just saved transfer file
                            Stashes.ImportStash(closedID);
                        }
                        Stashes.ExportSharedModeStash(closedID);

                    }
                    Console.WriteLine("Runtime: TransferStashSaved END!");
                };

            }

            private static bool _reloadOpenedStash = false;

            public static bool IsStashOpened(int stashID)
            {
                return (StashOpened && stashID == ActiveStashID);
            }

            public static void ReloadOpenedStash(int stashID)
            {
                if (IsStashOpened(stashID))
                    _reloadOpenedStash = true;
            }

            private delegate bool WaitForConditionDelegate();
            private static bool WaitFor(int time, WaitForConditionDelegate condition = null, int delay = 1)
            {
                long timeout = Environment.TickCount + time;
                while(Environment.TickCount < timeout)
                {
                    if (condition != null && condition()) return true;

                    long timeout2 = Environment.TickCount + delay;
                    while (Environment.TickCount < timeout2)
                    {
                        System.Threading.Thread.Sleep(1);
                        Application.DoEvents();
                    }
                }
                return false;
            }














            private static void _ReopenStashAction(Action action)
            {
                Console.WriteLine("_ReopenStashAction() START!");

                ushort keyEscape = (ushort)Native.Keyboard.KeyToScanCode(Keys.Escape);
                ushort keyInteract = GrimDawn.Keybindings.GetKeyBinding(GrimDawnKey.Interact).Primary;
                int triesLeft;

                bool transferSaved = false;
                TransferStashSavedEventHandler transferSavedHandler = delegate { transferSaved = true; };
                TransferStashSaved += transferSavedHandler;
                
                triesLeft = 4;
                while (triesLeft > 0 && _stashOpened)
                {
                    triesLeft -= 1;
                    Console.WriteLine("_ReopenStashAction() - sending escape key");
                    Native.Keyboard.SendKey(keyEscape);
                    WaitFor(500, () => !_stashOpened, 33);
                    if (_stashOpened)
                        Console.WriteLine("_ReopenStashAction() failed! stash not closed. Tries left: " + triesLeft);
                    else
                        break;
                }
                if (_stashOpened)
                {
                    Console.WriteLine("_ReopenStashAction() ABORT! stash not closed");
                    return;
                }

                Console.WriteLine("_ReopenStashAction() - waiting for TransferStashSaved event...");
                if (WaitFor(500, () =>
                {
                    return transferSaved;
                }, 33))
                {
                    TransferStashSaved -= transferSavedHandler;
                    Console.WriteLine("_ReopenStashAction() - TransferStashSaved received!");
                }
                else
                {
                    TransferStashSaved -= transferSavedHandler;
                    Console.WriteLine("_ReopenStashAction() - TransferStashSaved TIMEOUT! - aborting");
                    return;
                }
                
                Console.WriteLine("_ReopenStashAction() - action() START!");
                action();
                Console.WriteLine("_ReopenStashAction() - action() DONE!");

                triesLeft = 4;
                while (triesLeft > 0 && !_stashOpened)
                {
                    triesLeft -= 1;
                    Console.WriteLine("_ReopenStashAction() - sending interact key");
                    Native.Keyboard.SendKey(keyInteract);
                    WaitFor(500, () => _stashOpened, 33);
                    if (!_stashOpened)
                        Console.WriteLine("_ReopenStashAction() failed! stash not reopened. Tries left: " + triesLeft);
                    else
                        break;
                }
                if (!_stashOpened)
                {
                    Console.WriteLine("_ReopenStashAction() ABORT! stash not reopened!");
                    return;
                }

                Console.WriteLine("_ReopenStashAction() DONE!");
            }








            public delegate void StashReopenStartEventHandler(object sender, EventArgs e);
            public delegate void StashReopenEndEventHandler(object sender, EventArgs e);
            public static event StashReopenStartEventHandler StashReopenStart;
            public static event StashReopenEndEventHandler StashReopenEnd;

            public static bool StashIsReopening => _stashReopening;
            private static bool _stashReopening = false;




            private static void ReopenStashAction(Action action)
            {
                if (_stashReopening) return;
                _stashReopening = true;
                StashReopenStart?.Invoke(null, EventArgs.Empty);
                _ReopenStashAction(action);
                _stashReopening = false;
                if (!StashOpened)
                {
                    // broadcast that the stash is closed
                    Console.WriteLine("ReopenStashAction() FAILED");
                    StashStatusChanged?.Invoke(null, EventArgs.Empty);
                }
                StashReopenEnd?.Invoke(null, EventArgs.Empty);
            }

            public static void SwitchToStash(int stashID)
            {
                if (stashID == _activeStashID) return;
                ReopenStashAction(() => Stashes.SwitchToStash(stashID));
            }

            public static void SaveCurrentStash()
            {
                ReopenStashAction(() => Stashes.ImportStash(ActiveStashID));
            }

            public static void LoadCurrentStash()
            {
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
