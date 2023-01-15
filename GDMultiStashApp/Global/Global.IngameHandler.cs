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

            bool _reopenIngameStash = false;

            TransferFileSaved += delegate {
                Console.WriteLine($"transfer file saved by game");

                string externalFile = GrimDawn.GetTransferFilePath(_activeExpansion, _activeMode);
                string externalFileName = Path.GetFileName(externalFile);
                Global.FileSystem.Watcher.SkipNextFile(externalFileName);

                Console.WriteLine($"- file: {externalFileName}");
                Console.WriteLine($"- stash is reopening: {StashIsReopening}");
                Console.WriteLine($"- stash is opened: {_stashIsOpened}");
                Console.WriteLine($"- active Stash ID: {_activeStashID}");
                if (!StashIsReopening && !_stashIsOpened)
                {
                    int closedID = _activeStashID;

                    if (!Utils.Funcs.WaitFor(() => !Utils.FileUtils.FileIsLocked(externalFile), 2000, 33))
                    {
                        Console.WriteLine("- file locked");
                        return;
                    }

                    if (Global.Configuration.Settings.AutoBackToMain)
                    {
                        Console.WriteLine("- auto back to main");

                        int mainStashID = Global.Configuration.GetMainStashID(ActiveExpansion, ActiveMode);
                        Global.Stashes.SwitchToStash(mainStashID);
                        ActiveGroupID = 0; // 0 is alwas main group
                    }
                    else
                    {
                        Global.Stashes.SwitchToStash(ActiveStashID);
                    }
                }

            };

            Global.FileSystem.TransferFileChanged += delegate (object sender, FileSystemHandler.TransferFileChangedEventArgs e) {
                Console.WriteLine($"transfer file changed by external");
                Console.WriteLine($"- file: {e.FileName}");
                var canReopen = GameInitialized && StashIsOpened;
                var externalEnv = GrimDawn.GetEnvironmentByFilename(e.FileName);
                var stashId = Global.Configuration.GetCurrentStashID(externalEnv);
                if (Global.Configuration.Settings.SaveExternalChanges)
                {
                    Console.WriteLine($"saving external changes...");
                    Global.Stashes.ImportStash(stashId, externalEnv, true);
                    // export again because of shared mode stashes (sc+hc)
                    Global.Stashes.ExportStash(stashId);
                }
                if (canReopen && stashId == ActiveStashID)
                {
                    Console.WriteLine($"requesting reopening stash");
                    if (GameWindowFocused)
                    {
                        ReloadCurrentStash();
                    }
                    else
                    {
                        _reopenIngameStash = true;
                    }
                }
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
                if (_reopenIngameStash)
                {
                    _reopenIngameStash = false;
                    new System.Threading.Thread(new System.Threading.ThreadStart(() => {
                        System.Threading.Thread.Sleep(500);
                        ReloadCurrentStash();
                    })).Start();
                }
            };

            ActiveModeChanged += delegate { LoadActiveStashID(); };
            ActiveExpansionChanged += delegate { LoadActiveStashID(); };
            GameWindowConnected += delegate {
                // automatically set active expansion to installed expansion
                ActiveExpansion = GrimDawn.GetInstalledExpansionFromPath(Global.Configuration.Settings.GamePath);
            };




            _saveInvokeForm = new Form() {
                Visible = false,
                Opacity = 0,
                FormBorderStyle = FormBorderStyle.FixedToolWindow,
                ShowInTaskbar = false,
            };
            _saveInvokeForm.Shown += delegate { _saveInvokeForm.Hide(); };
            _saveInvokeForm.Show();



        }











        












        public void SaveInvoke(Action a)
        {
            _saveInvokeForm.Invoke(a);
        }

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
            if (v.Width == 0 || v.Height == 0)
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
        public void InvokeCharacterMovementDisabled()
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
            InvokeCharacterMovementDisabled();
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
