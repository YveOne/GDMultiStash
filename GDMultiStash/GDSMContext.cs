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
                    Constants.WM_SHOWME,
                    IntPtr.Zero,
                    IntPtr.Zero);
                Program.Quit();
                return;
            }

            Global.FileSystem.CreateDirectories();
            Global.Configuration.SaveBackup();
            Global.Configuration.Load();

            Global.Localization.AddLanguageFile("enUS");
            Global.Localization.AddLanguageFile("deDE");
            Global.Localization.AddLanguageFile("zhCN");
            Global.Localization.AddLanguageFilesFrom(Global.FileSystem.LocalesDirectory);
            Global.Configuration.LanguageChanged += Global_Configuration_LanguageChanged;

            if (Global.Configuration.IsNewConfiguration)
            {
                string curLangCode = Global.Localization.CurrentCode;
                if (Global.Localization.LoadLanguage(curLangCode))
                    Global.Configuration.Settings.Language = curLangCode;
                else
                    Global.Localization.LoadLanguage("enUS");
            }
            else
            {
                if (!Global.Localization.LoadLanguage(Global.Configuration.Settings.Language))
                    Global.Localization.LoadLanguage("enUS");
            }

            if (!GrimDawn.ValidateDocumentsPath())
            {
                Console.Warning(Global.L.DocumentsDirectoryNotFoundMessage());
                Program.Quit();
                return;
            }

            if (!GrimDawn.ValidateGamePath(Global.Configuration.Settings.GamePath))
                Global.Configuration.Settings.GamePath = GrimDawn.Steam.GamePath64 ?? "";
            if (!GrimDawn.ValidateGamePath(Global.Configuration.Settings.GamePath))
                Global.Configuration.Settings.GamePath = GrimDawn.GOG.GamePath64 ?? "";

            if (Global.Configuration.IsNewConfiguration)
            {
                Global.Windows.ShowConfigurationWindow();
            }
            else
            {
                // game install path not found? let user choose path
                if (!GrimDawn.ValidateGamePath(Global.Configuration.Settings.GamePath))
                {
                    Console.Warning(Global.L.SelectGameDirectoryMessage());
                    Global.Windows.ShowConfigurationWindow();
                }
            }

            // game install path still not found? poooor user...
            if (!GrimDawn.ValidateGamePath(Global.Configuration.Settings.GamePath))
            {
                Console.Warning(Global.L.GameDirectoryNotFoundMessage());
                Program.Quit();
                return;
            }

            // directory gdx3 doesnt exist yet in 1.2
            if (!File.Exists(Path.Combine(Global.Configuration.Settings.GamePath, "resources", "Text_DE.arc")))
            {
                Global.Configuration.RestoreBackup();
                string msg = Global.L.OldGDVersionNotSupported();
                if (Console.Confirm(msg, MessageBoxIcon.Information))
                {
                    Global.Update.StartUpdater(GDMultiStashUpdater.UpdaterAPI.LatestUrlOld);
                }
                else
                {
                    System.Diagnostics.Process.Start("https://github.com/YveOne/GDMultiStashOld/releases");
                    Program.Quit();
                }
                return;
            }

            // check for new version
            if (Global.Update.NewVersionAvailable())
            {
                string msg = Global.L.UpdateAvailableMessage(Global.Update.NewVersionName);
                if (Console.Confirm(msg, MessageBoxIcon.Information))
                {
                    Global.Update.StartUpdater();
                    return;
                }
            }

            Global.Configuration.UpdateAndCleanup();
            Global.Configuration.DeleteBackup();

            // warn user if cloud saving is enabled in gd settings
            {
                if (GrimDawn.Options.GetOptionValue("cloudSaving") == "true")
                {
                    Console.Warning(Global.L.DisableCloudSyncMessage());
                }
            }

            Global.Localization.LoadGameLanguage("Text_DE", "German");
            Global.Localization.LoadGameLanguage("Text_EN", "English");
            Global.Localization.LoadGameLanguage("Text_ES", "Spanish");
            Global.Localization.LoadGameLanguage("Text_FR", "French");
            Global.Localization.LoadGameLanguage("Text_IT", "Italian");
            Global.Localization.LoadGameLanguage("Text_CS", "Czech");
            Global.Localization.LoadGameLanguage("Text_JA", "Japanese");
            Global.Localization.LoadGameLanguage("Text_KO", "Korean");
            Global.Localization.LoadGameLanguage("Text_PL", "Polish");
            Global.Localization.LoadGameLanguage("Text_PT", "Portuguese");
            Global.Localization.LoadGameLanguage("Text_VI", "Vietnamese");
            Global.Localization.LoadGameLanguage("Text_ZH", "Chinese");

            Global.Database.LoadItemInfos(Properties.Resources.iteminfos);
            Global.Database.LoadMissingItemInfos();
            Global.Database.LoadItemAffixInfos(Properties.Resources.itemaffixes);
            Global.Database.LoadItemSets(Properties.Resources.itemsets);
            Global.Database.LoadItemTextures(Properties.Resources.itemtextures);
            Console.WriteLine($"GD Game Path: {Global.Configuration.Settings.GamePath}");
            Console.WriteLine($"GD Game Expansion: {GrimDawn.GetInstalledExpansionFromPath(Global.Configuration.Settings.GamePath)}");
            Global.Stashes.LoadStashes();
            Global.Groups.LoadGroups();

            _viewport = new Overlay.GDMSViewport();
            _gdOverlayService = new GDOverlayService(_viewport);
            _gdWindowHookService = new GDWindowHookService();
            _gdGameHookService = new GDGameHookService();
            _mouseHook = new Native.Mouse.Hook();

            Global.Runtime.StashOpened += Global_Runtime_StashOpened;
            Global.Runtime.StashClosed += Global_Runtime_StashClosed;
            Global.SetEventHandlers();

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
                if (Global.Configuration.Settings.AutoStartGame)
                {
                    Global.Windows.StartGame();
                }
                else
                {
                    if (Global.Configuration.AppVersionUpdated)
                    {
                        Cleanup.DoCleanup();
                        Global.Windows.ShowChangelogWindow();
                    }
                }
            });
        }

        public void Destroy()
        {
            Global.Windows.CloseMainWindow();
            Global.Database.Destroy();
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
            Global.Windows.LocalizeWindows();
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
            Global.Runtime.GameWindowIsConnected = true;
        }

        private void GDWindowHook_MoveSize(object sender, EventArgs e)
        {
            Global.Runtime.SetGameWindowLocSize(_gdWindowHookService.GetLocationSize());
        }

        private void GDWindowHook_GotFocus(object sender, EventArgs e)
        {
            Console.WriteLine("Grim Dawn window got focus");
            Global.Runtime.SetGameWindowLocSize(_gdWindowHookService.GetLocationSize());
            if (Global.Runtime.StashIsOpened)
                _mouseHook.SetHook();
        }

        private void GDWindowHook_LostFocus(object sender, EventArgs e)
        {
            Console.WriteLine("Grim Dawn window lost focus");
            Global.Runtime.GameWindowFocused = false;
            _mouseHook.UnHook();
        }

        private void GDWindowHook_WindowDestroyed(object sender, EventArgs e)
        {
            Console.WriteLine("Grim Dawn window closed");
            Global.Runtime.GameWindowFocused = false;
            Global.Runtime.GameWindowIsConnected = false;
            if (Global.Configuration.Settings.CloseWithGrimDawn)
            {
                Console.WriteLine("Quitting...");
                Program.Quit();
            }
            else
            {
                Global.Windows.ShowMainWindow();
                Console.WriteLine("Restarting services...");
                StopServices();
                StartServices();
            }
        }

        #endregion

        #region GDGameHook Events

        private void GDGameHook_StashStatusChanged(object sender, GDGameHookService.StashStatusChangedEventArgs e)
        {
            Global.Runtime.StashIsOpened = e.Opened;
        }

        private void GDGameHook_ModeStatusChanged(object sender, GDGameHookService.ModeChangedEventArgs e)
        {
            Global.Runtime.ActiveMode = e.IsHardcore ? GrimDawnGameMode.HC : GrimDawnGameMode.SC;
        }

        private void GDGameHook_ExpansionStatusChanged(object sender, GDGameHookService.ExpansionChangedEventArgs e)
        {
            Global.Runtime.ActiveExpansion = (GrimDawnGameExpansion)e.ExpansionID;
        }

        private void GDGameHook_TransferStashSaved(object sender, EventArgs e)
        {
            Global.Runtime.InvokeTransferStashSaved();
        }

        #endregion

        #region MouseHook Events

        private void MouseHook_MouseMove(object sender, Native.Mouse.Hook.MouseEventArgs e)
        {
            //if (!Global.Runtime.GameWindowFocused || (!Global.Runtime.StashOpened && !Global.Runtime.StashIsReopening)) return;
            if (!Global.Runtime.GameWindowFocused) return;
            bool hit = _viewport.CheckMouseMove(e.X - (int)Global.Runtime.GameWindowLocation.X, e.Y - (int)Global.Runtime.GameWindowLocation.Y);
            if (hit)
            {
            }
        }

        private void MouseHook_MouseDown(object sender, Native.Mouse.Hook.MouseEventArgs e)
        {
            //if (!Global.Runtime.GameWindowFocused || (!Global.Runtime.StashOpened && !Global.Runtime.StashIsReopening)) return;
            if (!Global.Runtime.GameWindowFocused) return;
            bool hit = _viewport.CheckMouseDown(e.X - (int)Global.Runtime.GameWindowLocation.X, e.Y - (int)Global.Runtime.GameWindowLocation.Y);
            if (hit)
            {
                // this is handled inside overlay mainwindow
            }
        }
        private void MouseHook_MouseUp(object sender, Native.Mouse.Hook.MouseEventArgs e)
        {
            Global.Runtime.EnableMovement();
            //if (!Global.Runtime.GameWindowFocused || (!Global.Runtime.StashOpened && !Global.Runtime.StashIsReopening)) return;
            if (!Global.Runtime.GameWindowFocused) return;
            bool hit = _viewport.CheckMouseUp(e.X - (int)Global.Runtime.GameWindowLocation.X, e.Y - (int)Global.Runtime.GameWindowLocation.Y);
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
            if (!Global.Runtime.GameWindowFocused || (!Global.Runtime.StashIsOpened && !Global.Ingame.StashIsReopening)) return;
            bool hit = _viewport.OnMouseWheel(e.X - (int)Global.Runtime.GameWindowLocation.X, e.Y - (int)Global.Runtime.GameWindowLocation.Y, e.Delta);
            if (hit)
            {
            }
        }

        #endregion

    }
}
