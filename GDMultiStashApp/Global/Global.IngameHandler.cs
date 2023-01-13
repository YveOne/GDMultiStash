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
    internal partial class IngameHandler
    {

        private Form _saveInvokeForm;

        public IngameHandler()
        {

            bool _reloadOpenedStash = false;

            ActiveModeChanged += delegate { LoadActiveStashID(); };
            ActiveExpansionChanged += delegate { LoadActiveStashID(); };

            TransferStashSaved += delegate
            {
                Console.WriteLine("Runtime: TransferStashSaved START!");
                Console.WriteLine("    _stashReopening: " + StashIsReopening.ToString());
                Console.WriteLine("    _stashOpened: " + _stashIsOpened.ToString());
                Console.WriteLine("    _activeStashID: " + _activeStashID.ToString());
                if (!StashIsReopening && !_stashIsOpened)
                {
                    int closedID = _activeStashID;
                    string externalFile = GrimDawn.GetTransferFilePath(_activeExpansion, _activeMode);
                    try
                    {
                        File.Open(externalFile, FileMode.Open).Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Runtime: TransferStashSaved - file locked");
                        return;
                    }

                    if (Global.Configuration.Settings.AutoBackToMain)
                    {
                        Console.WriteLine("Runtime: TransferStashSaved - Config.AutoBackToMain");

                        int mainStashID = Global.Configuration.GetMainStashID(ActiveExpansion, ActiveMode);
                        Global.Stashes.SwitchToStash(mainStashID);
                        ActiveGroupID = 0; // 0 is alwas main group
                    }
                    else
                    {
                        Global.Stashes.SwitchToStash(ActiveStashID);
                    }
                }
                Console.WriteLine("Runtime: TransferStashSaved END!");
            };

            ActiveGroupChanged += delegate (object sender, ActiveGroupChangedEventArgs e) {
                if (Global.Configuration.Settings.AutoSelectFirstStashInGroup)
                {
                    var stashesInGroup = Global.Stashes.GetStashesForGroup(e.NewID).ToList();
                    if (stashesInGroup.Count != 0)
                    {
                        stashesInGroup.Sort(new Common.Objects.Sorting.StashesSortComparer());
                        SwitchToStash(stashesInGroup[0].ID);
                    }
                }
            };

            StashesContentChanged += delegate (object sender, StashesContentChangedEventArgs args)
            {
                if (!args.NeedExport) return; // because its imported we dont need to export/reload it
                foreach(var s in args.Items)
                    Global.Stashes.ExportStash(s.ID);
            };

            GameWindowGotFocus += delegate
            {
                if (_reloadOpenedStash)
                {
                    _reloadOpenedStash = false;
                    new System.Threading.Thread(new System.Threading.ThreadStart(() => {
                        System.Threading.Thread.Sleep(500);
                        ReloadCurrentStash();
                    })).Start();
                }
            };

            GameWindowConnected += delegate {
                ActiveExpansion = GrimDawn.GetInstalledExpansionFromPath(Global.Configuration.Settings.GamePath);
            };

            TransferFileChanged += delegate {
                if (GameInitialized && !GameWindowFocused && StashIsOpened)
                    _reloadOpenedStash = true;
            };




            _saveInvokeForm = new Form() {
                Visible = false,
                Opacity = 0,
                FormBorderStyle = FormBorderStyle.FixedToolWindow,
                ShowInTaskbar = false,
            };
            _saveInvokeForm.Shown += delegate { _saveInvokeForm.Hide(); };
            _saveInvokeForm.Show();



            StartFileSystemWatcher();
        }

        private void SaveInvoke(Action a)
        {
            _saveInvokeForm.Invoke(a);
        }

        #region file system watcher

        public class TransferFileChangedEventArgs : EventArgs
        {
            public string FilePath { get; private set; }
            public bool IsActive { get; private set; }
            public TransferFileChangedEventArgs(string filePath, bool isActive)
            {
                FilePath = filePath;
                IsActive = isActive;
            }
        }

        public EventHandler<TransferFileChangedEventArgs> TransferFileChanged;

        public void InvokeTransferFileChanged(TransferFileChangedEventArgs e)
            => SaveInvoke(() => TransferFileChanged?.Invoke(this, e));

        private void StartFileSystemWatcher()
        {
            Dictionary<string, string> transferFilesHashes = new Dictionary<string, string>();
            foreach (var s in GrimDawn.GameEnvironmentList.Select(env => $"transfer{env.TransferFileExtension}"))
                transferFilesHashes.Add(s, Utils.FileUtils.GetFileHash(s));

            var watcher = new FileSystemWatcher(GrimDawn.DocumentsSavePath);
            watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.CreationTime;
            watcher.Changed += delegate (object sender, FileSystemEventArgs e)
            {
                string curHash = Utils.FileUtils.GetFileHash(e.FullPath);
                if (!transferFilesHashes.TryGetValue(e.Name, out string lastHash)) return;
                if (curHash == lastHash) return;
                transferFilesHashes[e.Name] = curHash;

                bool isActive = GameInitialized && GrimDawn.GetTransferFilePath(ActiveExpansion, ActiveMode) == e.FullPath;
                InvokeTransferFileChanged(new TransferFileChangedEventArgs(e.FullPath, isActive));
                if (isActive)
                    Console.WriteLine($"FileWatcher: {e.FullPath} changed (active)");
                else
                    Console.WriteLine($"FileWatcher: {e.FullPath} changed");
            };
            watcher.Filter = "transfer.*";
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;
        }

        #endregion

        #region Game Window

        public event EventHandler<EventArgs> GameWindowConnected;
        public event EventHandler<EventArgs> GameWindowDisconnected;
        public event EventHandler<EventArgs> GameWindowGotFocus;
        public event EventHandler<EventArgs> GameWindowLostFocus;

        public void InvokeGameWindowConnected()
            => SaveInvoke(() => GameWindowConnected?.Invoke(null, EventArgs.Empty));
        public void InvokeGameWindowDisconnected()
            => SaveInvoke(() => GameWindowDisconnected?.Invoke(null, EventArgs.Empty));
        public void InvokeGameWindowGotFocus()
            => SaveInvoke(() => GameWindowGotFocus?.Invoke(null, EventArgs.Empty));
        public void InvokeGameWindowLostFocus()
            => SaveInvoke(() => GameWindowLostFocus?.Invoke(null, EventArgs.Empty));

        private bool _gameWindowHasFocus = false;
        private bool _gameWindowIsConnected = false;

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

        public event EventHandler<GameWindowSizeChangedEventArgs> GameWindowSizeChanged;
        public event EventHandler<GameWindowLocationChangedEventArgs> GameWindowLocationChanged;

        public void InvokeGameWindowSizeChanged(System.Drawing.Size size)
            => SaveInvoke(() => GameWindowSizeChanged?.Invoke(null, new GameWindowSizeChangedEventArgs(size)));
        public void InvokeGameWindowLocationChanged(System.Drawing.Point point)
            => SaveInvoke(() => GameWindowLocationChanged?.Invoke(null, new GameWindowLocationChangedEventArgs(point)));

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
                    InvokeGameWindowLocationChanged(_windowLocation);
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
                    InvokeGameWindowSizeChanged(_windowSize);
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

        #region character movement

        public event EventHandler<EventArgs> CharacterMovementEnabled;
        public event EventHandler<EventArgs> CharacterMovementDisabled;

        public void InvokeCharacterMovementEnabled()
            => SaveInvoke(() => CharacterMovementEnabled?.Invoke(null, EventArgs.Empty));
        public void invokeCharacterMovementDisabled()
            => SaveInvoke(() => CharacterMovementDisabled?.Invoke(null, EventArgs.Empty));

        private bool _characterMovementDisabled = false;

        public void EnableMovement()
        {
            if (!_characterMovementDisabled) return;
            _characterMovementDisabled = false;
            ushort k = GrimDawn.Keybindings.GetKeyBinding(GrimDawnKey.StationaryAttack).Primary;
            Native.Keyboard.SendKeyUp(k);
            InvokeCharacterMovementEnabled();
        }

        public void DisableMovement()
        {
            if (_characterMovementDisabled) return;
            _characterMovementDisabled = true;
            ushort k = GrimDawn.Keybindings.GetKeyBinding(GrimDawnKey.StationaryAttack).Primary;
            Native.Keyboard.SendKeyDown(k);
            invokeCharacterMovementDisabled();
        }

        #endregion

        public bool GameInitialized => ActiveStashID != -1
            && ActiveExpansion != GrimDawnGameExpansion.Unknown
            && ActiveMode != GrimDawnGameMode.None;

        public class ListUpdatedEventArgs<T> : EventArgs
        {
            public T[] Items { get; private set; }
            public ListUpdatedEventArgs(IEnumerable<T> list)
            {
                Items = list.ToArray();
            }
            public ListUpdatedEventArgs(T obj)
            {
                Items = new T[] { obj };
            }
        }

        #region stashes

        public class StashesContentChangedEventArgs : ListUpdatedEventArgs<StashObject>
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

        public event EventHandler<EventArgs> StashesRebuild;
        public event EventHandler<ListUpdatedEventArgs<StashObject>> StashesAdded;
        public event EventHandler<ListUpdatedEventArgs<StashObject>> StashesRemoved;
        public event EventHandler<ListUpdatedEventArgs<StashObject>> StashesInfoChanged;
        public event EventHandler<StashesContentChangedEventArgs> StashesContentChanged;

        public void InvokeStashesRebuild()
            => SaveInvoke(() => StashesRebuild?.Invoke(null, EventArgs.Empty));

        public void InvokeStashesAdded(StashObject stash)
            => SaveInvoke(() => StashesAdded?.Invoke(null, new ListUpdatedEventArgs<StashObject>(stash)));

        public void InvokeStashesAdded(IEnumerable<StashObject> stashes)
            => SaveInvoke(() => StashesAdded?.Invoke(null, new ListUpdatedEventArgs<StashObject>(stashes)));

        public void InvokeStashesRemoved(StashObject stash)
            => SaveInvoke(() => StashesRemoved?.Invoke(null, new ListUpdatedEventArgs<StashObject>(stash)));

        public void InvokeStashesRemoved(IEnumerable<StashObject> stashes)
            => SaveInvoke(() => StashesRemoved?.Invoke(null, new ListUpdatedEventArgs<StashObject>(stashes)));

        public void InvokeStashesInfoChanged(StashObject stashes)
            => SaveInvoke(() => StashesInfoChanged?.Invoke(null, new ListUpdatedEventArgs<StashObject>(stashes)));

        public void InvokeStashesInfoChanged(IEnumerable<StashObject> stashes)
            => SaveInvoke(() => StashesInfoChanged?.Invoke(null, new ListUpdatedEventArgs<StashObject>(stashes)));
        
        public void InvokeStashesContentChanged(StashObject stashes, bool needExport)
            => SaveInvoke(() => StashesContentChanged?.Invoke(null, new StashesContentChangedEventArgs(stashes, needExport)));
        
        public void InvokeStashesContentChanged(IEnumerable<StashObject> stashes, bool needExport)
            => SaveInvoke(() => StashesContentChanged?.Invoke(null, new StashesContentChangedEventArgs(stashes, needExport)));
        

        #endregion

        #region groups

        public event EventHandler<EventArgs> StashGroupsRebuild;
        public event EventHandler<ListUpdatedEventArgs<StashGroupObject>> StashGroupsAdded;
        public event EventHandler<ListUpdatedEventArgs<StashGroupObject>> StashGroupsRemoved;
        public event EventHandler<ListUpdatedEventArgs<StashGroupObject>> StashGroupsInfoChanged;

        public void InvokeStashGroupsRebuild()
            => SaveInvoke(() => StashGroupsRebuild?.Invoke(null, EventArgs.Empty));

        public void InvokeStashGroupsInfoChanged(StashGroupObject group)
            => SaveInvoke(() => StashGroupsInfoChanged?.Invoke(null, new ListUpdatedEventArgs<StashGroupObject>(group)));

        public void InvokeStashGroupsInfoChanged(IEnumerable<StashGroupObject> groups)
            => SaveInvoke(() => StashGroupsInfoChanged?.Invoke(null, new ListUpdatedEventArgs<StashGroupObject>(groups)));

        public void InvokeStashGroupsAdded(StashGroupObject group)
            => SaveInvoke(() => StashGroupsAdded?.Invoke(null, new ListUpdatedEventArgs<StashGroupObject>(group)));

        public void InvokeStashGroupsAdded(IEnumerable<StashGroupObject> groups)
            => SaveInvoke(() => StashGroupsAdded?.Invoke(null, new ListUpdatedEventArgs<StashGroupObject>(groups)));

        public void InvokeStashGroupsRemoved(StashGroupObject group)
            => SaveInvoke(() => StashGroupsRemoved?.Invoke(null, new ListUpdatedEventArgs<StashGroupObject>(group)));

        public void InvokeStashGroupsRemoved(IEnumerable<StashGroupObject> groups)
            => SaveInvoke(() => StashGroupsRemoved?.Invoke(null, new ListUpdatedEventArgs<StashGroupObject>(groups)));

        #endregion

    }
}
