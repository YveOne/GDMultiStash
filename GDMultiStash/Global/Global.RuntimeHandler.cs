using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using GrimDawnLib;

namespace GDMultiStash.GlobalHandlers
{
    internal class RuntimeHandler
    {

        public RuntimeHandler()
        {
            ActiveModeChanged += delegate { LoadActiveStashID(); };
            ActiveExpansionChanged += delegate { LoadActiveStashID(); };

            GameWindowFocusChanged += delegate
            {
                if (GameWindowFocused)
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
                    catch (Exception)
                    {
                        // stash reopened by using keybinding
                        Console.WriteLine("Runtime: TransferStashSaved - file locked - return");
                        return;
                    }
                    Console.WriteLine("Runtime: TransferStashSaved - file not locked");

                    if (Global.Configuration.Settings.AutoBackToMain)
                    {
                        Console.WriteLine("Runtime: TransferStashSaved - Config.AutoBackToMain");
                        // we dont need to use reopen because stash is closed

                        int mainStashID = Global.Configuration.GetMainStashID(CurrentExpansion, CurrentMode);
                        Global.Stashes.SwitchToStash(mainStashID);
                    }
                    else
                    {
                        // import the just saved transfer file
                        Global.Stashes.ImportStash(closedID);
                    }
                    Global.Stashes.ExportSharedModeStash(closedID);

                }
                Console.WriteLine("Runtime: TransferStashSaved END!");
            };



        }









        public enum AutoStartResult
        {
            Disabled = 0,
            AlreadyRunning = 1,
            Success = 2,
        }

        public AutoStartResult AutoStartGame(bool ignoreDisabled = false)
        {
            if (!Global.Configuration.Settings.AutoStartGD && !ignoreDisabled) return AutoStartResult.Disabled;
            if (Native.FindWindow("Grim Dawn", null) != IntPtr.Zero) return AutoStartResult.AlreadyRunning;

            Console.WriteLine("Autostarting Grim Dawn:");
            Console.WriteLine("- Command: " + Global.Configuration.Settings.AutoStartGDCommand);
            Console.WriteLine("- Arguments: " + Global.Configuration.Settings.AutoStartGDArguments);
            Console.WriteLine("- WorkingDir: " + Global.Configuration.Settings.GamePath);
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo
            {
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                FileName = Global.Configuration.Settings.AutoStartGDCommand,
                Arguments = Global.Configuration.Settings.AutoStartGDArguments,
                WorkingDirectory = File.Exists(Global.Configuration.Settings.AutoStartGDCommand)
                    ? Path.GetDirectoryName(Global.Configuration.Settings.AutoStartGDCommand)
                    : Global.Configuration.Settings.GamePath
            };
            process.StartInfo = startInfo;
            process.Start();

            return AutoStartResult.Success;
        }












        #region ActiveStash

        public class ActiveStashChangedEventArgs : EventArgs
        {
            public int OldID { get => _oldId; }
            private int _oldId;
            public int NewID { get => _newId; }
            private int _newId;
            public ActiveStashChangedEventArgs(int oldId, int newID)
            {
                _oldId = oldId;
                _newId = newID;
            }
        }

        public delegate void ActiveStashChangedEventHandler(object sender, ActiveStashChangedEventArgs e);
        public event ActiveStashChangedEventHandler ActiveStashChanged;

        private int _activeStashID = -1;

        public int ActiveStashID
        {
            get => _activeStashID;
            set
            {
                if (value == _activeStashID) return;
                int _previousStashID = _activeStashID;
                _activeStashID = value;
                Global.Configuration.SetCurrentStashID(CurrentExpansion, CurrentMode, _activeStashID);
                Console.WriteLine("Runtime: Active stash changed to #" + _activeStashID);
                ActiveStashChanged?.Invoke(null, new ActiveStashChangedEventArgs(_previousStashID, _activeStashID));
            }
        }

        public void LoadActiveStashID()
        {
            ActiveStashID = Global.Configuration.GetCurrentStashID(CurrentExpansion, CurrentMode);
        }

        #endregion

        #region ActiveMode

        public class ActiveModeChangedEventArgs : EventArgs
        {
            public GrimDawnGameMode Mode { get => _mode; }
            private GrimDawnGameMode _mode;
            public ActiveModeChangedEventArgs(GrimDawnGameMode mode)
            {
                _mode = mode;
            }
        }

        public delegate void ActiveModeChangedEventHandler(object sender, ActiveModeChangedEventArgs e);
        public event ActiveModeChangedEventHandler ActiveModeChanged;

        private GrimDawnGameMode _activeMode = GrimDawnGameMode.None;

        public GrimDawnGameMode CurrentMode
        {
            get => _activeMode;
            set
            {
                if (_activeMode == value) return;
                _activeMode = value;
                Console.WriteLine("Runtime: CurrentModeChanged: " + _activeMode);
                ActiveModeChanged?.Invoke(null, new ActiveModeChangedEventArgs(_activeMode));
            }
        }

        #endregion

        #region ActiveExpansion

        public class ActiveExpansionChangedEventArgs : EventArgs
        {
            public GrimDawnGameExpansion Expansion { get => _expansion; }
            private GrimDawnGameExpansion _expansion;
            public ActiveExpansionChangedEventArgs(GrimDawnGameExpansion expansion)
            {
                _expansion = expansion;
            }
        }

        public delegate void ActiveExpansionChangedEventHandler(object sender, ActiveExpansionChangedEventArgs e);
        public event ActiveExpansionChangedEventHandler ActiveExpansionChanged;

        private GrimDawnGameExpansion _currentExpansion = GrimDawnGameExpansion.Unknown;

        public GrimDawnGameExpansion CurrentExpansion
        {
            get => _currentExpansion;
            set
            {
                if (_currentExpansion == value) return;
                _currentExpansion = value;
                Console.WriteLine("Runtime: CurrentExpansionChanged: " + _currentExpansion);
                ActiveExpansionChanged?.Invoke(null, new ActiveExpansionChangedEventArgs(_currentExpansion));
            }
        }

        #endregion

        #region StashList

        public class StashListChangedEventArgs : EventArgs
        {
            public StashObject[] Stashes { get => _stashes; }
            private StashObject[] _stashes;
            public StashListChangedEventArgs(IEnumerable<StashObject> stashes)
            {
                _stashes = stashes.ToArray();
            }
            public StashListChangedEventArgs(StashObject stash)
            {
                _stashes = new StashObject[] { stash };
            }
            public StashListChangedEventArgs()
            {
                _stashes = new StashObject[] { };
            }
        }

        public delegate void StashListChangedEventHandler(object sender, StashListChangedEventArgs e);
        public event StashListChangedEventHandler StashesOrderChanged;
        public event StashListChangedEventHandler StashesAdded;
        public event StashListChangedEventHandler StashesRemoved;
        public event StashListChangedEventHandler StashesRestored;
        public event StashListChangedEventHandler StashesUpdated;

        public void NotifyStashesOrderChanged()
        {
            StashesOrderChanged?.Invoke(null, new StashListChangedEventArgs());
        }

        public void NotifyStashesAdded(StashObject stash)
        {
            StashesAdded?.Invoke(null, new StashListChangedEventArgs(stash));
        }

        public void NotifyStashesAdded(IEnumerable<StashObject> stashes)
        {
            StashesAdded?.Invoke(null, new StashListChangedEventArgs(stashes));
        }

        public void NotifyStashesRemoved(StashObject stash)
        {
            StashesRemoved?.Invoke(null, new StashListChangedEventArgs(stash));
        }

        public void NotifyStashesRemoved(IEnumerable<StashObject> stashes)
        {
            StashesRemoved?.Invoke(null, new StashListChangedEventArgs(stashes));
        }

        public void NotifyStashesRestored(StashObject stash)
        {
            StashesRestored?.Invoke(null, new StashListChangedEventArgs(stash));
        }

        public void NotifyStashesRestored(IEnumerable<StashObject> stashes)
        {
            StashesRestored?.Invoke(null, new StashListChangedEventArgs(stashes));
        }

        public void NotifyStashesUpdated(StashObject stash)
        {
            StashesUpdated?.Invoke(null, new StashListChangedEventArgs(stash));
        }

        public void NotifyStashesUpdated(IEnumerable<StashObject> stashes)
        {
            StashesUpdated?.Invoke(null, new StashListChangedEventArgs(stashes));
        }

        #endregion

        #region Event: GameWindow

        public delegate void GameWindowFocusChangedEventHandler(object sender, EventArgs e);
        public event GameWindowFocusChangedEventHandler GameWindowFocusChanged;

        private bool _hasFocus = false;

        public bool GameWindowFocused
        {
            get => _hasFocus;
            set
            {
                if (_hasFocus == value) return;
                _hasFocus = value;
                GameWindowFocusChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        public delegate void GameWindowSizeChangedEventHandler(object sender, EventArgs e);
        public event GameWindowSizeChangedEventHandler GameWindowSizeChanged;

        public delegate void GameWindowLocationChangedEventHandler(object sender, EventArgs e);
        public event GameWindowLocationChangedEventHandler GameWindowLocationChanged;

        private bool _windowLocSizeSet = false;
        private System.Drawing.Point _windowLocation;
        private System.Drawing.Size _windowSize;

        public System.Drawing.Point GameWindowLocation
        {
            get => _windowLocation;
            private set
            {
                System.Drawing.Point last = _windowLocation;
                _windowLocation = value;
                if (!_windowLocSizeSet || last.X != value.X || last.Y != value.Y)
                    GameWindowLocationChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        public System.Drawing.Size GameWindowSize
        {
            get => _windowSize;
            private set
            {
                System.Drawing.Size last = _windowSize;
                _windowSize = value;
                if (!_windowLocSizeSet || last.Width != value.Width || last.Height != value.Height)
                    GameWindowSizeChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        public void SetGameWindowLocSize(Services.GDWindowHookService.LocationSize v)
        {
            if (v.X == -32000)
            {
                // minimized
                GameWindowFocused = false;
            }
            else
            {
                GameWindowFocused = true;
                GameWindowLocation = new System.Drawing.Point(v.X, v.Y);
                GameWindowSize = new System.Drawing.Size(v.Width, v.Height);
                _windowLocSizeSet = true;
            }
        }

        #endregion

        #region Event: TransferStashSaved

        public delegate void StashStatusChangedEventHandler(object sender, EventArgs e);
        public event StashStatusChangedEventHandler StashStatusChanged;

        private bool _stashOpened = false;
        private bool _stashWasOpened = false;

        public bool StashOpened
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
        public event TransferStashSavedEventHandler TransferStashSaved;

        public void NotifyTransferStashSaved()
        {
            if (!_stashWasOpened) return;
            _stashWasOpened = false;
            TransferStashSaved?.Invoke(null, EventArgs.Empty);
        }

        #endregion

















        private bool _reloadOpenedStash = false;

        public bool IsStashOpened(int stashID)
        {
            return (StashOpened && stashID == ActiveStashID);
        }

        public void ReloadOpenedStash(int stashID)
        {
            if (IsStashOpened(stashID))
                _reloadOpenedStash = true;
        }

        private delegate bool WaitForConditionDelegate();
        private bool WaitFor(int time, WaitForConditionDelegate condition = null, int delay = 1)
        {
            long timeout = Environment.TickCount + time;
            while (Environment.TickCount < timeout)
            {
                if (condition != null && condition()) return true;

                long timeout2 = Environment.TickCount + delay;
                while (Environment.TickCount < timeout2)
                {
                    System.Threading.Thread.Sleep(1);
                }
            }
            return false;
        }














        private void _ReopenStashAction(Action action)
        {
            Console.WriteLine("_ReopenStashAction() START!");

            ushort keyEscape = (ushort)Native.Keyboard.KeyToScanCode(Keys.Escape);
            ushort keyInteract = GrimDawn.Keybindings.GetKeyBinding(GrimDawnKey.Interact).Primary;
            int triesLeft;

            bool transferSaved = false;
            TransferStashSavedEventHandler transferSavedHandler = delegate { transferSaved = true; };
            TransferStashSaved += transferSavedHandler;

            System.Threading.Thread.Sleep(33);

            triesLeft = 4;
            while (triesLeft > 0 && _stashOpened)
            {
                triesLeft -= 1;
                Console.WriteLine("_ReopenStashAction() - sending escape key");
                Native.Keyboard.SendKey(keyEscape);
                WaitFor(333, () => !_stashOpened, 33);
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

            System.Threading.Thread.Sleep(33);

            Console.WriteLine("_ReopenStashAction() - waiting for TransferStashSaved event...");
            if (WaitFor(333, () =>
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

            System.Threading.Thread.Sleep(33);

            triesLeft = 4;
            while (triesLeft > 0 && !_stashOpened)
            {
                triesLeft -= 1;
                Console.WriteLine("_ReopenStashAction() - sending interact key");
                Native.Keyboard.SendKey(keyInteract);
                WaitFor(333, () => _stashOpened, 33);
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
        public event StashReopenStartEventHandler StashReopenStart;
        public event StashReopenEndEventHandler StashReopenEnd;

        public bool StashIsReopening => _stashReopening;
        private bool _stashReopening = false;




        private void ReopenStashAction(Action action)
        {
            if (_stashReopening) return;
            new System.Threading.Thread(new System.Threading.ThreadStart(() => {

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

            })).Start();


        }

        public void SwitchToStash(int stashID)
        {
            if (stashID == _activeStashID) return;
            ReopenStashAction(() => Global.Stashes.SwitchToStash(stashID));
        }

        public void SaveCurrentStash()
        {
            ReopenStashAction(() => Global.Stashes.ImportStash(ActiveStashID));
        }

        public void LoadCurrentStash()
        {
            ReopenStashAction(() => Global.Stashes.ExportStash(ActiveStashID));
        }

        private bool moveDisabled = false;

        public void EnableMovement()
        {
            if (!moveDisabled) return;
            moveDisabled = false;
            ushort k = GrimDawn.Keybindings.GetKeyBinding(GrimDawnKey.StationaryAttack).Primary;
            Native.Keyboard.SendKeyUp(k);
        }

        public void DisableMovement()
        {
            if (moveDisabled) return;
            moveDisabled = true;
            ushort k = GrimDawn.Keybindings.GetKeyBinding(GrimDawnKey.StationaryAttack).Primary;
            Native.Keyboard.SendKeyDown(k);
        }




    }
}
