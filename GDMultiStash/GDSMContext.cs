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

            // allow only one instance of gdms
            _singleInstanceMutex = new Mutex(true, "GDMultiStash", out bool createdNew);
            if (!createdNew)
            {
                Native.PostMessage(
                    (IntPtr)Native.HWND.BROADCAST,
                    C.WM_SHOWME,
                    IntPtr.Zero,
                    IntPtr.Zero);
                Program.Quit();
                return;
            }

            G.Windows.ShowSplashScreen();

            G.FileSystem.CreateDirectories();
            G.Configuration.CreateBackup();
            G.Configuration.Load();

            G.Localization.AddLanguageFile("enUS");
            G.Localization.AddLanguageFile("deDE");
            G.Localization.AddLanguageFile("zhCN");
            G.Localization.AddLanguageFilesFrom(C.LocalesPath);
            G.Configuration.LanguageChanged += Global_Configuration_LanguageChanged;

            if (G.Configuration.IsNewConfiguration)
            {
                string curLangCode = G.Localization.CurrentCode;
                if (G.Localization.LoadLanguage(curLangCode))
                    G.Configuration.Settings.Language = curLangCode;
                else
                    G.Localization.LoadLanguage("enUS");
            }
            else
            {
                if (!G.Localization.LoadLanguage(G.Configuration.Settings.Language))
                    G.Localization.LoadLanguage("enUS");
            }

            if (!GrimDawn.ValidateDocumentsPath())
            {
                Console.AlertError(G.L.DocumentsDirectoryNotFoundMessage());
                Program.Quit();
                return;
            }

            if (!GrimDawn.ValidateGamePath(G.Configuration.Settings.GamePath))
                G.Configuration.Settings.GamePath = GrimDawn.Steam.GamePath64 ?? "";
            if (!GrimDawn.ValidateGamePath(G.Configuration.Settings.GamePath))
                G.Configuration.Settings.GamePath = GrimDawn.GOG.GamePath64 ?? "";

            if (G.Configuration.IsNewConfiguration)
            {
                G.Windows.ShowConfigurationWindow();
            }
            else
            {
                // game install path not found? let user choose path
                if (!GrimDawn.ValidateGamePath(G.Configuration.Settings.GamePath))
                {
                    Console.AlertWarning(G.L.SelectGameDirectoryMessage());
                    G.Windows.ShowConfigurationWindow();
                }
            }

            // game install path still not found? poooor user...
            if (!GrimDawn.ValidateGamePath(G.Configuration.Settings.GamePath))
            {
                Console.AlertError(G.L.GameDirectoryNotFoundMessage());
                Program.Quit();
                return;
            }

            if (!File.Exists(Path.Combine(G.Configuration.Settings.GamePath, "resources", "Text_DE.arc")))
            {
                G.Configuration.RestoreBackup();
                string msg = G.L.OldGDVersionNotSupported();
                if (Console.Confirm(msg, MessageBoxIcon.Information))
                {
                    G.Update.StartUpdater(GDMultiStashUpdater.UpdaterAPI.LatestUrlOld);
                }
                else
                {
                    System.Diagnostics.Process.Start("https://github.com/YveOne/GDMultiStashOld/releases");
                }
                return;
            }

            // check for new version
            if (G.Update.ShowNewVersionAvailable())
            {
                string msg = G.L.UpdateAvailableMessage(G.Update.NewVersionName);
                if (Console.Confirm(msg, MessageBoxIcon.Information))
                {
                    G.Update.StartUpdater();
                    return;
                }
            }

            G.Configuration.UpdateAndCleanup();
            G.Configuration.DeleteBackup();

            // warn user if cloud saving is enabled in gd settings
            if (GrimDawn.Options.GetOptionValue("cloudSaving") == "true")
            {
                Console.AlertWarning(G.L.DisableCloudSyncMessage());
            }

            G.Localization.LoadGameLanguage("Text_DE", "German");
            G.Localization.LoadGameLanguage("Text_EN", "English");
            G.Localization.LoadGameLanguage("Text_ES", "Spanish");
            G.Localization.LoadGameLanguage("Text_FR", "French");
            G.Localization.LoadGameLanguage("Text_IT", "Italian");
            G.Localization.LoadGameLanguage("Text_CS", "Czech");
            G.Localization.LoadGameLanguage("Text_JA", "Japanese");
            G.Localization.LoadGameLanguage("Text_KO", "Korean");
            G.Localization.LoadGameLanguage("Text_PL", "Polish");
            G.Localization.LoadGameLanguage("Text_PT", "Portuguese");
            G.Localization.LoadGameLanguage("Text_VI", "Vietnamese");
            G.Localization.LoadGameLanguage("Text_ZH", "Chinese");

            G.Database.LoadItemInfos(Properties.Resources.iteminfos);
            G.Database.LoadItemAffixInfos(Properties.Resources.itemaffixes);
            G.Database.LoadItemSets(Properties.Resources.itemsets);
            G.Database.LoadItemTextures(Properties.Resources.itemtextures);
            Console.WriteLine($"GD Game Path: {G.Configuration.Settings.GamePath}");
            Console.WriteLine($"GD Game Expansion: {GrimDawn.GetInstalledExpansionFromPath(G.Configuration.Settings.GamePath)}");
            G.Stashes.LoadStashes();
            G.StashGroups.LoadGroups();

            _viewport = new Overlay.GDMSViewport();
            _gdOverlayService = new GDOverlayService(_viewport);
            _gdWindowHookService = new GDWindowHookService();
            _gdGameHookService = new GDGameHookService();
            _mouseHook = new Native.Mouse.Hook();

            G.Runtime.StashOpened += Global_Runtime_StashOpened;
            G.Runtime.StashClosed += Global_Runtime_StashClosed;
            G.SetEventHandlers();

            StartServices();

            trayIcon = new NotifyIcon()
            {
                Icon = Properties.Resources.icon32,
                Visible = false,
                ContextMenu = new ContextMenu(new MenuItem[] {
                    new MenuItem(G.L.SettingsButton(), delegate {
                        G.Windows.ShowConfigurationWindow();
                    }),
                    new MenuItem(G.L.ExitButton(), delegate {
                        Program.Quit();
                    }),
                }),
            };
            trayIcon.DoubleClick += delegate { G.Windows.ShowMainWindow(); };
            trayIcon.Visible = true;

            // testing crash output to log file
            //var n = 0;
            //n = n / n;

            G.Windows.ShowMainWindow(() => {

                G.Windows.HideSplashScreen();

                if (G.Configuration.Settings.AutoStartGame)
                {
                    G.Windows.StartGame();
                }
                else
                {
                    if (G.Configuration.AppVersionUpdated)
                    {
                        G.Windows.ShowChangelogWindow();
                    }
                }
            });
        }

        public void Destroy()
        {
            G.Windows.CloseMainWindow();
            G.Database.Destroy();
            StopServices();
            if (_gdWindowHookService != null)
                _gdWindowHookService.Destroy();
            if (_gdGameHookService != null)
                _gdGameHookService.Destroy();
            if (_gdOverlayService != null)
                _gdOverlayService.Destroy();
            if (trayIcon != null)
                trayIcon.Visible = false;
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
            _gdGameHookService.StashStatusChanged += GDGameHook_StashStatusChanged;
            _gdGameHookService.ModeStatusChanged += GDGameHook_ModeStatusChanged;
            _gdGameHookService.ExpansionChanged += GDGameHook_ExpansionStatusChanged;
            _gdGameHookService.ModNameChanged += GDGameHook_ModNameChanged;
            _gdGameHookService.TransferStashSaved += GDGameHook_TransferStashSaved;
            _gdGameHookService.Start();

            Console.WriteLine("Starting Overlay Service ...");
            _gdOverlayService.FrameDrawing += D3DHook_FrameDrawing;
            _gdOverlayService.Start();

            Console.WriteLine("Starting GD Window Hook Service ...");
            _gdWindowHookService.HookInstalled += GDWindowHook_HookInstalled;
            _gdWindowHookService.MoveSize += GDWindowHook_MoveSize;
            _gdWindowHookService.GotFocus += GDWindowHook_GotFocus;
            _gdWindowHookService.LostFocus += GDWindowHook_LostFocus;
            _gdWindowHookService.WindowDestroyed += GDWindowHook_WindowDestroyed;
            _gdWindowHookService.Start();

        }

        private void StopServices()
        {
            if (!_servicesInstalled) return;

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

        #region Global

        private void Global_Configuration_LanguageChanged(object sender, EventArgs args)
        {
            G.Windows.LocalizeWindows();
        }

        private void Global_Runtime_StashOpened(object sender, EventArgs e)
        {
            _viewport.ShowMainWindow();
            _mouseHook.SetHook();
        }

        private void Global_Runtime_StashClosed(object sender, EventArgs e)
        {
            _viewport.HideMainWindow();
            _mouseHook.UnHook();
        }

        #endregion

        #region D3DHook Events

        private void D3DHook_FrameDrawing(float ms)
        {
            _viewport.DrawRoutine(ms);
        }

        #endregion

        #region GDWindowHook Events

        private void GDWindowHook_HookInstalled(object sender, EventArgs e)
        {
            G.Runtime.GameWindowIsConnected = true;
        }

        private void GDWindowHook_MoveSize(object sender, EventArgs e)
        {
            G.Runtime.SetGameWindowLocSize(_gdWindowHookService.GetLocationSize());
        }

        private void GDWindowHook_GotFocus(object sender, EventArgs e)
        {
            Console.WriteLine("Grim Dawn window got focus");
            G.Runtime.SetGameWindowLocSize(_gdWindowHookService.GetLocationSize());
            if (G.Runtime.StashIsOpened)
                _mouseHook.SetHook();
        }

        private void GDWindowHook_LostFocus(object sender, EventArgs e)
        {
            Console.WriteLine("Grim Dawn window lost focus");
            G.Runtime.GameWindowFocused = false;
            _mouseHook.UnHook();
        }

        private void GDWindowHook_WindowDestroyed(object sender, EventArgs e)
        {
            Console.WriteLine("Grim Dawn window closed");
            G.Runtime.GameWindowFocused = false;
            G.Runtime.GameWindowIsConnected = false;
            if (G.Configuration.Settings.CloseWithGrimDawn)
            {
                Console.WriteLine("Quitting...");
                Program.Quit();
            }
            else
            {
                G.Windows.ShowMainWindow();
                Console.WriteLine("Restarting services...");
                StopServices();
                StartServices();
            }
        }

        #endregion

        #region GDGameHook Events

        private void GDGameHook_StashStatusChanged(object sender, GDGameHookService.StashStatusChangedEventArgs e)
        {
            G.Runtime.StashIsOpened = e.Opened;
        }

        private void GDGameHook_ModeStatusChanged(object sender, GDGameHookService.ModeChangedEventArgs e)
        {
            G.Runtime.ActiveMode = e.IsHardcore ? GrimDawnGameMode.HC : GrimDawnGameMode.SC;
        }

        private void GDGameHook_ExpansionStatusChanged(object sender, GDGameHookService.ExpansionChangedEventArgs e)
        {
            G.Runtime.ActiveExpansion = (GrimDawnGameExpansion)e.ExpansionID;
        }

        private void GDGameHook_ModNameChanged(object sender, GDGameHookService.ModNameChangedEventArgs e)
        {
            G.Runtime.ActiveModName = e.ModName;
        }

        private void GDGameHook_TransferStashSaved(object sender, EventArgs e)
        {
            G.Runtime.InvokeTransferStashSaved();
        }

        #endregion

        #region MouseHook Events

        private void MouseHook_MouseMove(object sender, Native.Mouse.Hook.MouseEventArgs e)
        {
            //if (!Global.Runtime.GameWindowFocused || (!Global.Runtime.StashOpened && !Global.Runtime.StashIsReopening)) return;
            if (!G.Runtime.GameWindowFocused) return;
            bool hit = _viewport.CheckMouseMove(e.X - (int)G.Runtime.GameWindowLocation.X, e.Y - (int)G.Runtime.GameWindowLocation.Y);
            if (hit)
            {
            }
        }

        private void MouseHook_MouseDown(object sender, Native.Mouse.Hook.MouseEventArgs e)
        {
            //if (!Global.Runtime.GameWindowFocused || (!Global.Runtime.StashOpened && !Global.Runtime.StashIsReopening)) return;
            if (!G.Runtime.GameWindowFocused) return;
            bool hit = _viewport.CheckMouseDown(e.X - (int)G.Runtime.GameWindowLocation.X, e.Y - (int)G.Runtime.GameWindowLocation.Y);
            if (hit)
            {
                // this is handled inside overlay mainwindow
            }
        }
        private void MouseHook_MouseUp(object sender, Native.Mouse.Hook.MouseEventArgs e)
        {
            G.Ingame.EnableMovement();
            //if (!Global.Runtime.GameWindowFocused || (!Global.Runtime.StashOpened && !Global.Runtime.StashIsReopening)) return;
            if (!G.Runtime.GameWindowFocused) return;
            bool hit = _viewport.CheckMouseUp(e.X - (int)G.Runtime.GameWindowLocation.X, e.Y - (int)G.Runtime.GameWindowLocation.Y);
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
            if (!G.Runtime.GameWindowFocused || (!G.Runtime.StashIsOpened && !G.Ingame.StashIsReopening)) return;
            bool hit = _viewport.OnMouseWheel(e.X - (int)G.Runtime.GameWindowLocation.X, e.Y - (int)G.Runtime.GameWindowLocation.Y, e.Delta);
            if (hit)
            {
            }
        }

        #endregion


    }
}
