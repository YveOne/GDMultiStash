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
                Console.WriteLine("    _activeStashID: " + _activeStashID.ToString());
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
                    Global.Stashes.ExportStash(closedID);
                }
                Console.WriteLine("Runtime: TransferStashSaved END!");
            };



        }








        public event EventHandler<EventArgs> GameStarted;

        public enum GameStartResult
        {
            Disabled = 0,
            AlreadyRunning = 1,
            Success = 2,
            Error = 3,
        }

        public GameStartResult StartGame()
        {
            if (Native.FindWindow("Grim Dawn", null) != IntPtr.Zero) return GameStartResult.AlreadyRunning;

            Console.WriteLine("Starting Grim Dawn:");
            Console.WriteLine("- Command: " + Global.Configuration.Settings.StartGameCommand);
            Console.WriteLine("- Arguments: " + Global.Configuration.Settings.StartGameArguments);
            Console.WriteLine("- WorkingDir: " + Global.Configuration.Settings.GamePath);
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo
            {
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                FileName = Global.Configuration.Settings.StartGameCommand,
                Arguments = Global.Configuration.Settings.StartGameArguments,
                WorkingDirectory = File.Exists(Global.Configuration.Settings.StartGameCommand)
                    ? Path.GetDirectoryName(Global.Configuration.Settings.StartGameCommand)
                    : Global.Configuration.Settings.GamePath
            };
            process.StartInfo = startInfo;
            try
            {
                process.Start();
                GameStarted?.Invoke(null, EventArgs.Empty);
                return GameStartResult.Success;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return GameStartResult.Error;
            }
        }










        #region Active Group

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

        public event EventHandler<ActiveGroupChangedEventArgs> ActiveGroupChanged;

        private int _activeGroupID = 0;

        public int ActiveGroupID
        {
            get => _activeGroupID;
            set
            {
                if (value == _activeGroupID) return;
                int _previousID = _activeGroupID;
                _activeGroupID = value;
                Console.WriteLine("Runtime: Active group changed to #" + _activeGroupID);
                ActiveGroupChanged?.Invoke(null, new ActiveGroupChangedEventArgs(_previousID, _activeGroupID));
            }
        }

        #endregion

        #region ActiveStash

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

        public event EventHandler<ActiveStashChangedEventArgs> ActiveStashChanged;

        private int _activeStashID = -1;

        public int ActiveStashID
        {
            get => _activeStashID;
            set
            {
                if (value == _activeStashID) return;
                int _previousID = _activeStashID;
                _activeStashID = value;
                Global.Configuration.SetCurrentStashID(CurrentExpansion, CurrentMode, _activeStashID);
                Console.WriteLine("Runtime: Active stash changed to #" + _activeStashID);
                ActiveStashChanged?.Invoke(null, new ActiveStashChangedEventArgs(_previousID, _activeStashID));
            }
        }

        public void LoadActiveStashID()
        {
            if (CurrentExpansion == GrimDawnGameExpansion.Unknown) return;
            if (CurrentMode == GrimDawnGameMode.None) return;
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

        public event EventHandler<ActiveModeChangedEventArgs> ActiveModeChanged;

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

        public event EventHandler<ActiveExpansionChangedEventArgs> ActiveExpansionChanged;

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

        public class ListChangedEventArgs<T> : EventArgs
        {
            public T[] List { get; private set; }
            public ListChangedEventArgs(IEnumerable<T> list)
            {
                List = list.ToArray();
            }
            public ListChangedEventArgs(T obj)
            {
                List = new T[] { obj };
            }
            public ListChangedEventArgs()
            {
                List = new T[] { };
            }
        }

        public event EventHandler<EventArgs> StashesOrderChanged;
        public event EventHandler<ListChangedEventArgs<StashObject>> StashesAdded;
        public event EventHandler<ListChangedEventArgs<StashObject>> StashesRemoved;
        public event EventHandler<ListChangedEventArgs<StashObject>> StashesRestored;
        public event EventHandler<ListChangedEventArgs<StashObject>> StashesUpdated;
        public event EventHandler<ListChangedEventArgs<StashObject>> StashesImported;
        public event EventHandler<ListChangedEventArgs<StashObject>> StashesExported;

        public void NotifyStashesOrderChanged()
        {
            StashesOrderChanged?.Invoke(null, EventArgs.Empty);
        }

        public void NotifyStashesAdded(StashObject stash)
        {
            StashesAdded?.Invoke(null, new ListChangedEventArgs<StashObject>(stash));
        }

        public void NotifyStashesAdded(IEnumerable<StashObject> stashes)
        {
            StashesAdded?.Invoke(null, new ListChangedEventArgs<StashObject>(stashes));
        }

        public void NotifyStashesRemoved(StashObject stash)
        {
            StashesRemoved?.Invoke(null, new ListChangedEventArgs<StashObject>(stash));
        }

        public void NotifyStashesRemoved(IEnumerable<StashObject> stashes)
        {
            StashesRemoved?.Invoke(null, new ListChangedEventArgs<StashObject>(stashes));
        }

        public void NotifyStashesRestored(StashObject stash)
        {
            StashesRestored?.Invoke(null, new ListChangedEventArgs<StashObject>(stash));
        }

        public void NotifyStashesRestored(IEnumerable<StashObject> stashes)
        {
            StashesRestored?.Invoke(null, new ListChangedEventArgs<StashObject>(stashes));
        }

        public void NotifyStashesUpdated(StashObject stashes)
        {
            StashesUpdated?.Invoke(null, new ListChangedEventArgs<StashObject>(stashes));
        }

        public void NotifyStashesUpdated(IEnumerable<StashObject> stashes)
        {
            StashesUpdated?.Invoke(null, new ListChangedEventArgs<StashObject>(stashes));
        }

        public void NotifyStashesImported(StashObject stashes)
        {
            StashesImported?.Invoke(null, new ListChangedEventArgs<StashObject>(stashes));
        }

        public void NotifyStashesImported(IEnumerable<StashObject> stashes)
        {
            StashesImported?.Invoke(null, new ListChangedEventArgs<StashObject>(stashes));
        }

        public void NotifyStashesExported(StashObject stashes)
        {
            StashesExported?.Invoke(null, new ListChangedEventArgs<StashObject>(stashes));
        }

        public void NotifyStashesExported(IEnumerable<StashObject> stashes)
        {
            StashesExported?.Invoke(null, new ListChangedEventArgs<StashObject>(stashes));
        }

        #endregion

        




        public event EventHandler<EventArgs> StashGroupsOrderChanged;
        public event EventHandler<ListChangedEventArgs<StashGroupObject>> StashGroupsAdded;
        public event EventHandler<ListChangedEventArgs<StashGroupObject>> StashGroupsRemoved;
        public event EventHandler<ListChangedEventArgs<StashGroupObject>> StashGroupsUpdated;

        public void NotifyStashGroupsOrderChanged()
        {
            StashGroupsOrderChanged?.Invoke(null, EventArgs.Empty);
        }

        public void NotifyStashGroupsUpdated(StashGroupObject group)
        {
            StashGroupsUpdated?.Invoke(null, new ListChangedEventArgs<StashGroupObject>(group));
        }

        public void NotifyStashGroupsUpdated(IEnumerable<StashGroupObject> groups)
        {
            StashGroupsUpdated?.Invoke(null, new ListChangedEventArgs<StashGroupObject>(groups));
        }

        public void NotifyStashGroupsAdded(StashGroupObject group)
        {
            StashGroupsAdded?.Invoke(null, new ListChangedEventArgs<StashGroupObject>(group));
        }

        public void NotifyStashGroupsAdded(IEnumerable<StashGroupObject> groups)
        {
            StashGroupsAdded?.Invoke(null, new ListChangedEventArgs<StashGroupObject>(groups));
        }

        public void NotifyStashGroupsRemoved(StashGroupObject group)
        {
            StashGroupsRemoved?.Invoke(null, new ListChangedEventArgs<StashGroupObject>(group));
        }

        public void NotifyStashGroupsRemoved(IEnumerable<StashGroupObject> groups)
        {
            StashGroupsRemoved?.Invoke(null, new ListChangedEventArgs<StashGroupObject>(groups));
        }


        










        #region Event: GameWindow

        public event EventHandler<EventArgs> GameWindowFocusChanged;

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

        public event EventHandler<EventArgs> GameWindowSizeChanged;
        public event EventHandler<EventArgs> GameWindowLocationChanged;

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

        public event EventHandler<EventArgs> StashStatusChanged;

        private bool _stashOpened = false;
        private bool _stashWasOpened = false; // this is used to trigger NotifyTransferStashSaved() only once
        private bool _transferStashSaved = false;

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

        public event EventHandler<EventArgs> TransferStashSaved;

        public void NotifyTransferStashSaved()
        {
            if (!_stashWasOpened) return;
            _stashWasOpened = false;
            _transferStashSaved = true;
            TransferStashSaved?.Invoke(null, EventArgs.Empty);
        }

        #endregion
































        private bool _reloadOpenedStash = false;

        public void ReloadOpenedStash(int stashID)
        {
            if (StashOpened && stashID == ActiveStashID)
            {
                _reloadOpenedStash = true;
            }
        }












        private void _ReopenStashAction(Action action)
        {
            Console.WriteLine("_ReopenStashAction() START!");

            ushort keyEscape = (ushort)Native.Keyboard.KeyToScanCode(Keys.Escape);
            ushort keyInteract = GrimDawn.Keybindings.GetKeyBinding(GrimDawnKey.Interact).Primary;

            _transferStashSaved = false;

            Console.WriteLine("_ReopenStashAction() - sending escape key");
            Native.Keyboard.SendKey(keyEscape);

            Console.WriteLine("_ReopenStashAction() - waiting for TransferStashSaved event...");
            if (Utils.Funcs.WaitFor(() => !_stashOpened && _transferStashSaved, 5000, 33))
            {
                Console.WriteLine("_ReopenStashAction() - TransferStashSaved received!");
            }
            else
            {
                Console.WriteLine("_ReopenStashAction() - TransferStashSaved TIMEOUT! - aborting");
                return;
            }

            Console.WriteLine("_ReopenStashAction() - action() START!");
            action();
            Console.WriteLine("_ReopenStashAction() - action() DONE!");
            System.Threading.Thread.Sleep(100);

            Console.WriteLine("_ReopenStashAction() - sending interact key");
            Native.Keyboard.SendKey(keyInteract);
            Utils.Funcs.WaitFor(() => _stashOpened, 2000, 33);

            Console.WriteLine("_ReopenStashAction() DONE!");
        }
        







        public event EventHandler<EventArgs> StashReopenStart;
        public event EventHandler<EventArgs> StashReopenEnd;

        public bool StashIsReopening => _stashReopening;
        private bool _stashReopening = false;

        private void ReopenStashAction(Action action)
        {
            if (_stashReopening) return;
            _stashReopening = true;
            new System.Threading.Thread(new System.Threading.ThreadStart(() => {
                StashReopenStart?.Invoke(null, EventArgs.Empty);
                _ReopenStashAction(action);
                if (!StashOpened) // something happened
                {
                    // broadcast that the stash is closed
                    Console.WriteLine("ReopenStashAction() FAILED");
                    StashStatusChanged?.Invoke(null, EventArgs.Empty);
                }
                StashReopenEnd?.Invoke(null, EventArgs.Empty);
                _stashReopening = false;
            })).Start();
        }

        public void SwitchToStash(int stashID)
        {
            if (stashID == _activeStashID) return;
            ReopenStashAction(() => Global.Stashes.SwitchToStash(stashID));
        }

        public void SaveCurrentStash()
        {
            ReopenStashAction(() => {
                Global.Stashes.ImportStash(ActiveStashID);
                Global.Stashes.ExportStash(ActiveStashID);
            });
        }

        public void LoadCurrentStash()
        {
            ReopenStashAction(() => Global.Stashes.ExportStash(ActiveStashID));
        }

        public void ReloadCurrentStash()
        {
            string file = GrimDawn.GetTransferFile(CurrentExpansion, CurrentMode);
            string temp = Utils.Funcs.GetTempFileName();
            File.Copy(file, temp);
            ReopenStashAction(() => {
                if (File.Exists(file))
                    File.Delete(file);
                File.Move(temp, file);
            });
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
