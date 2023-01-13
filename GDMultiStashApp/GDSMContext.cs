using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;

using GrimDawnLib;
using GDMultiStash.Services;

namespace GDMultiStash
{
    internal class GDMSContext : ApplicationContext
    {

        private readonly Overlay.GDMSViewport _viewport;
        private readonly GDWindowHookService _gdWindowHookService;
        private readonly GDGameHookService _gdGameHookService;
        private readonly GDOverlayService _gdOverlayService;
        private readonly Native.Mouse.Hook _mouseHook;
        private readonly Mutex _singleInstanceMutex;
        private bool _servicesInstalled = false;
        private readonly NotifyIcon trayIcon;

        public GDMSContext()
        {

            Global.FileSystem.CreateDirectories();
            Global.Configuration.Load();

            Global.Localization.AddLanguageFile("deDE", Properties.Resources.local_deDE);
            Global.Localization.AddLanguageFile("enGB", Properties.Resources.local_enGB);
            Global.Localization.AddLanguageFile("enUS", Properties.Resources.local_enUS);
            Global.Localization.AddLanguageFile("zhCN", Properties.Resources.local_zhCN);
            Global.Configuration.LanguageChanged += Configuration_LanguageChanged;

            if (Global.Configuration.IsNew)
            {
                string curLangCode = Global.Localization.CurrentCode;
                if (Global.Localization.LoadLanguage(curLangCode))
                    Global.Configuration.Settings.Language = curLangCode;
                else
                    Global.Localization.LoadLanguage("enUS");
            }
            else
            {
                Global.Localization.LoadLanguage(Global.Configuration.Settings.Language);
            }
            
            // allow only one instance of gdms
            _singleInstanceMutex = new Mutex(true, "GDMultiStash", out bool createdNew);
            if (!createdNew)
            {
                Native.PostMessage(
                    (IntPtr)Native.HWND.BROADCAST,
                    Global.WM_SHOWME,
                    IntPtr.Zero,
                    IntPtr.Zero);
                Program.Quit();
                return;
            }

            if (!GrimDawn.ValidGamePath(Global.Configuration.Settings.GamePath))
                Global.Configuration.Settings.GamePath = GrimDawn.Steam.GamePath64 ?? "";
            if (!GrimDawn.ValidGamePath(Global.Configuration.Settings.GamePath))
                Global.Configuration.Settings.GamePath = GrimDawn.GOG.GamePath64 ?? "";

            if (Global.Configuration.IsNew)
            {
                Global.Windows.ShowConfigurationWindow();
            }

            if (!GrimDawn.ValidDocsPath())
            {
                Console.Warning(Global.L.DocumentsDirectoryNotFoundMessage());
                Program.Quit();
                return;
            }

            // game install path not found? let user choose path
            if (!GrimDawn.ValidGamePath(Global.Configuration.Settings.GamePath))
            {
                Console.Warning(Global.L.SelectGameDirectoryMessage());
                Global.Windows.ShowConfigurationWindow();
            }

            // game install path still not found? poooor user...
            if (!GrimDawn.ValidGamePath(Global.Configuration.Settings.GamePath))
            {
                Console.Warning(Global.L.GameDirectoryNotFoundMessage());
                Program.Quit();
                return;
            }

            // check for new version
            if (Global.Update.NewVersionAvailable())
            {
                string msg = string.Format(Global.L.UpdateAvailableMessage(), Global.Update.NewVersionName);
                if (MessageBox.Show(msg, "", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    Global.Update.StartUpdater();
                    return;
                }
            }

            Global.Configuration.UpdateAndCleanup();

            // warn user if cloud saving is enabled in gd settings
            {
                var f = Path.Combine(GrimDawn.DocumentsSettingsPath, "options.txt");
                if (File.Exists(f))
                {
                    var l = File.ReadAllText(f);
                    var d = GlobalHandlers.LocalizationHandler.Language.ParseDictionary(l);
                    if (d["cloudSaving"] == "true")
                    {
                        Console.Warning(Global.L.DisableCloudSyncMessage());
                    }
                }
            }

            Global.Database.LoadItemInfos(Properties.Resources.iteminfos);
            Global.Database.LoadItemAffixInfos(Properties.Resources.itemaffixes);
            Global.Database.LoadItemTextures(Properties.Resources.itemtextures);
            Console.WriteLine($"GD Game Path: {Global.Configuration.Settings.GamePath}");
            Console.WriteLine($"GD Game Expansion: {GrimDawn.GetInstalledExpansionFromPath(Global.Configuration.Settings.GamePath)}");
            Global.Stashes.LoadStashes();
            Global.Stashes.LoadStashGroups();

            _viewport = new Overlay.GDMSViewport();

            _gdOverlayService = new GDOverlayService(_viewport);
            _gdWindowHookService = new GDWindowHookService();
            _gdGameHookService = new GDGameHookService();
            _mouseHook = new Native.Mouse.Hook();

            StartServices();

            trayIcon = new NotifyIcon()
            {
                Icon = Properties.Resources.icon32,
                Visible = false,
                ContextMenu = new ContextMenu(new MenuItem[] {
                    new MenuItem(Global.L.SettingsButton(), delegate {
                        Global.Windows.ShowConfigurationWindow();
                    }),
                    new MenuItem(Global.L.ExitButton(), delegate {
                        Program.Quit();
                    }),
                }),
            };
            trayIcon.DoubleClick += delegate { Global.Windows.ShowMainWindow(); };
            trayIcon.Visible = true;

            Global.Windows.ShowMainWindow(() => {
                if (Global.Configuration.AppVersionUpdated)
                    Global.Windows.ShowChangelogWindow();
                if (Global.Configuration.Settings.AutoStartGame)
                    Global.Windows.StartGame();
            });
        }

        public void Destroy()
        {
            Global.Windows.CloseMainWindow();
            Global.Database.Destroy();
            StopServices();
            _gdWindowHookService.Destroy();
            _gdGameHookService.Destroy();
            _gdOverlayService.Destroy();
            trayIcon.Visible = false;
        }







        private void Configuration_LanguageChanged(object sender, EventArgs args)
        {
            Global.Windows.LocalizeWindows();
        }





        private void StartServices()
        {
            if (_servicesInstalled) return;
            _servicesInstalled = true;

            _mouseHook.MouseMove += MouseHook_MouseMove;
            _mouseHook.MouseDown += MouseHook_MouseDown;
            _mouseHook.MouseUp += MouseHook_MouseUp;
            _mouseHook.MouseWheel += MouseHook_MouseWheel;

            Console.WriteLine("Starting GD Game Hook Service ...");
            _gdGameHookService.Start();
            _gdGameHookService.StashStatusChanged += GDGameHook_StashStatusChanged;
            _gdGameHookService.ModeStatusChanged += GDGameHook_ModeStatusChanged;
            _gdGameHookService.ExpansionChanged += GDGameHook_ExpansionStatusChanged;
            _gdGameHookService.TransferStashSaved += GDGameHook_TransferStashSaved;

            Console.WriteLine("Starting Overlay Service ...");
            _gdOverlayService.Start();
            _gdOverlayService.FrameDrawing += D3DHook_FrameDrawing;

            Console.WriteLine("Starting GD Window Hook Service ...");
            _gdWindowHookService.HookInstalled += GDWindowHook_HookInstalled;
            _gdWindowHookService.MoveSize += GDWindowHook_MoveSize;
            _gdWindowHookService.GotFocus += GDWindowHook_GotFocus;
            _gdWindowHookService.LostFocus += GDWindowHook_LostFocus;
            _gdWindowHookService.WindowDestroyed += GDWindowHook_WindowDestroyed;
            _gdWindowHookService.Start();

            Global.Ingame.StashOpened += Ingame_StashOpened;
            Global.Ingame.StashClosed += Ingame_StashClosed;
        }

        private void StopServices()
        {
            Global.Ingame.StashOpened -= Ingame_StashOpened;
            Global.Ingame.StashClosed -= Ingame_StashClosed;

            _gdWindowHookService.HookInstalled -= GDWindowHook_HookInstalled;
            _gdWindowHookService.MoveSize -= GDWindowHook_MoveSize;
            _gdWindowHookService.GotFocus -= GDWindowHook_GotFocus;
            _gdWindowHookService.LostFocus -= GDWindowHook_LostFocus;
            _gdWindowHookService.WindowDestroyed -= GDWindowHook_WindowDestroyed;
            _gdWindowHookService.Stop();

            _gdOverlayService.FrameDrawing -= D3DHook_FrameDrawing;
            _gdOverlayService.Stop();

            _gdGameHookService.StashStatusChanged -= GDGameHook_StashStatusChanged;
            _gdGameHookService.ModeStatusChanged -= GDGameHook_ModeStatusChanged;
            _gdGameHookService.Stop();

            _mouseHook.MouseMove -= MouseHook_MouseMove;
            _mouseHook.MouseDown -= MouseHook_MouseDown;
            _mouseHook.MouseUp -= MouseHook_MouseUp;
            _mouseHook.MouseWheel -= MouseHook_MouseWheel;
            _mouseHook.UnHook();

            _servicesInstalled = false;
        }

        private void Ingame_StashOpened(object sender, EventArgs e)
        {
            _viewport.ShowMainWindow();
            //_mouseHook.UnHook(); // used to refresh hook if something happened in background
            _mouseHook.SetHook();
        }

        private void Ingame_StashClosed(object sender, EventArgs e)
        {
            _viewport.HideMainWindow();
            _mouseHook.UnHook();
        }

        #region D3DHook Events

        private void D3DHook_FrameDrawing(float ms)
        {
            _viewport.DrawRoutine(ms);
        }

        #endregion

        #region GDWindowHook Events

        private void GDWindowHook_HookInstalled(object sender, EventArgs e)
        {
            Global.Ingame.GameWindowIsConnected = true;
        }

        private void GDWindowHook_MoveSize(object sender, EventArgs e)
        {
            Global.Ingame.SetGameWindowLocSize(_gdWindowHookService.GetLocationSize());
        }

        private void GDWindowHook_GotFocus(object sender, EventArgs e)
        {
            Console.WriteLine("Grim Dawn window got focus");
            Global.Ingame.SetGameWindowLocSize(_gdWindowHookService.GetLocationSize());
            if (Global.Ingame.StashIsOpened)
                _mouseHook.SetHook();
        }

        private void GDWindowHook_LostFocus(object sender, EventArgs e)
        {
            Console.WriteLine("Grim Dawn window lost focus");
            Global.Ingame.GameWindowFocused = false;
            _mouseHook.UnHook();
        }

        private void GDWindowHook_WindowDestroyed(object sender, EventArgs e)
        {
            Console.WriteLine("Grim Dawn window closed");
            Global.Ingame.GameWindowFocused = false;
            Global.Ingame.GameWindowIsConnected = false;
            if (Global.Configuration.Settings.CloseWithGrimDawn)
            {
                Console.WriteLine("   Quitting...");
                Program.Quit();
            }
            else
            {
                Global.Windows.ShowMainWindow();
                Console.WriteLine("   Restarting services...");
                StopServices();
                StartServices();
            }
        }

        #endregion

        #region GDGameHook Events

        private void GDGameHook_StashStatusChanged(object sender, GDGameHookService.StashStatusChangedEventArgs e)
        {
            Global.Ingame.StashIsOpened = e.Opened;
        }

        private void GDGameHook_ModeStatusChanged(object sender, GDGameHookService.ModeChangedEventArgs e)
        {
            Global.Ingame.ActiveMode = e.IsHardcore ? GrimDawnGameMode.HC : GrimDawnGameMode.SC;
        }

        private void GDGameHook_ExpansionStatusChanged(object sender, GDGameHookService.ExpansionChangedEventArgs e)
        {
            Global.Ingame.ActiveExpansion = (GrimDawnGameExpansion)e.ExpansionID;
        }

        private void GDGameHook_TransferStashSaved(object sender, EventArgs e)
        {
            Global.Ingame.InvokeTransferStashSaved();
        }

        #endregion

        #region MouseHook Events

        private void MouseHook_MouseMove(object sender, Native.Mouse.Hook.MouseEventArgs e)
        {
            //if (!Global.Runtime.GameWindowFocused || (!Global.Runtime.StashOpened && !Global.Runtime.StashIsReopening)) return;
            if (!Global.Ingame.GameWindowFocused) return;
            bool hit = _viewport.CheckMouseMove(e.X - (int)Global.Ingame.GameWindowLocation.X, e.Y - (int)Global.Ingame.GameWindowLocation.Y);
            if (hit)
            {
            }
        }

        private void MouseHook_MouseDown(object sender, Native.Mouse.Hook.MouseEventArgs e)
        {
            //if (!Global.Runtime.GameWindowFocused || (!Global.Runtime.StashOpened && !Global.Runtime.StashIsReopening)) return;
            if (!Global.Ingame.GameWindowFocused) return;
            bool hit = _viewport.CheckMouseDown(e.X - (int)Global.Ingame.GameWindowLocation.X, e.Y - (int)Global.Ingame.GameWindowLocation.Y);
            if (hit)
            {
                // this is handled inside overlay mainwindow
            }
        }
        private void MouseHook_MouseUp(object sender, Native.Mouse.Hook.MouseEventArgs e)
        {
            Global.Ingame.EnableMovement();
            //if (!Global.Runtime.GameWindowFocused || (!Global.Runtime.StashOpened && !Global.Runtime.StashIsReopening)) return;
            if (!Global.Ingame.GameWindowFocused) return;
            bool hit = _viewport.CheckMouseUp(e.X - (int)Global.Ingame.GameWindowLocation.X, e.Y - (int)Global.Ingame.GameWindowLocation.Y);
            if (hit)
            {
            }
        }

        private bool isBySys = false;

        private void MouseHook_MouseWheel(object sender, Native.Mouse.Hook.MouseEventArgs e)
        {
            if (isBySys)
            {
                isBySys = false;
                return;
            }
            if (!Global.Ingame.GameWindowFocused || (!Global.Ingame.StashIsOpened && !Global.Ingame.StashIsReopening)) return;
            bool hit = _viewport.OnMouseWheel(e.X - (int)Global.Ingame.GameWindowLocation.X, e.Y - (int)Global.Ingame.GameWindowLocation.Y, e.Delta);
            if (hit)
            {
            }
        }

        #endregion

    }
}
