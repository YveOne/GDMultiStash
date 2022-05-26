using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Drawing;

using GrimDawnLib;
using GDMultiStash.Services;

namespace GDMultiStash
{

    internal class GDMSContext : ApplicationContext
    {
        private readonly Core.Localization.StringsProxy L;

        private readonly GDWindowHookService _gdWindowHookService;
        private readonly GDGameHookService _gdGameHookService;
        private readonly GDOverlayService _gdOverlayService;
        private readonly Native.Mouse.Hook _mouseHook;

        private Overlay.OverlayManager _overlayManager;

        private static Mutex mutex = null;

        public GDMSContext()
        {
            L = new Core.Localization.StringsProxy();

            Core.Files.EnsureDirectoriesExist();
            Core.Localization.SaveDefaultFile("en-English.txt", Properties.Resources.en_English);
            Core.Localization.SaveDefaultFile("de-Deutsch.txt", Properties.Resources.de_Deutsch);
            Core.Localization.LoadLanguages();
            Core.Config.Load();
            Core.Config.LanguageChanged += delegate {
                Core.Windows.LocalizeWindows();
            };

            if (Core.Config.IsNew)
            {
                Core.Localization.LoadLanguage("en");
                Core.Windows.ShowSetupDialog(true);
            }
            else
            {
                Core.Localization.LoadLanguage(Core.Config.Language);
            }

            // allow only one instance of gdms
            bool createdNew;
            mutex = new Mutex(true, "GDMultiStash", out createdNew);
            if (!createdNew)
            {
                //app is already running! Exiting the application
                MessageBox.Show(L["err_gdms_already_running"]);
                Program.Quit();
                return;
            }

            // game install path not found? let user choose path
            if (!GrimDawn.ValidGamePath(Core.Config.GamePath))
            {
                Program.ShowError(L["err_gamedir_not_found"]);
                Core.Windows.ShowSetupDialog(true);
            }

            // game install path still not found? poooor user...
            if (!GrimDawn.ValidGamePath(Core.Config.GamePath))
            {
                Program.ShowError(L["err_gamedir_not_found"]);
                Program.Quit();
                return;
            }

            if (!GrimDawn.ValidDocsPath())
            {
                Program.ShowError(L["err_docsdir_not_found"]);
                Program.Quit();
                return;
            }

            Core.GD.LoadItemSizes(Properties.Resources.itemsizes);
            Core.GD.SetCurrentGameExpansion();
            Console.WriteLine("GD Game Path: " + Core.Config.GamePath);
            Console.WriteLine("GD Game Expansion: " + GrimDawn.GetExpansionName(Core.GD.InstalledGameExpansion));
            Core.Stashes.LoadStashes();


            _overlayManager = new Overlay.OverlayManager();
            _gdOverlayService = new GDOverlayService(_overlayManager.Viewport);













































            _gdWindowHookService = new GDWindowHookService();
            _gdGameHookService = new GDGameHookService();
            _mouseHook = new Native.Mouse.Hook();

            _gdWindowHookService.MoveSize += GDWindowHook_MoveSize;
            _gdWindowHookService.GotFocus += GDWindowHook_GotFocus;
            _gdWindowHookService.LostFocus += GDWindowHook_LostFocus;
            _gdWindowHookService.WindowDestroyed += GDWindowHook_WindowDestroyed;

            _mouseHook.MouseMove += MouseHook_MouseMove;
            _mouseHook.MouseDown += MouseHook_MouseDown;
            _mouseHook.MouseUp += MouseHook_MouseUp;

            _gdGameHookService.StashStatusChanged += GDGameHook_StashStatusChanged;
            _gdGameHookService.ModeStatusChanged += GDGameHook_ModeStatusChanged;
            _gdGameHookService.ExpansionChanged += GDGameHook_ExpansionStatusChanged;

            _gdOverlayService.FrameDrawing += D3DHook_FrameDrawing;

            Core.Runtime.StashStatusChanged += Core_StashStatusChanged;

            Console.WriteLine("Starting GD Window Hook Service ...");
            _gdWindowHookService.Start();

            Console.WriteLine("Starting GD Game Hook Service ...");
            _gdGameHookService.Start();

            Console.WriteLine("Starting Overlay Service ...");
            _gdOverlayService.Start();

            ThreadExit += delegate {
                //_titleFont.Dispose();

                Core.Runtime.StashStatusChanged -= Core_StashStatusChanged;

                _gdWindowHookService.Stop();
                _gdGameHookService.Stop();
                _gdOverlayService.Stop();

                _overlayManager.Destroy();
                _gdOverlayService.FrameDrawing -= D3DHook_FrameDrawing;

                _gdWindowHookService.MoveSize -= GDWindowHook_MoveSize;
                _gdWindowHookService.GotFocus -= GDWindowHook_GotFocus;
                _gdWindowHookService.LostFocus -= GDWindowHook_LostFocus;
                _gdWindowHookService.WindowDestroyed -= GDWindowHook_WindowDestroyed;

                _gdGameHookService.StashStatusChanged -= GDGameHook_StashStatusChanged;
                _gdGameHookService.ModeStatusChanged -= GDGameHook_ModeStatusChanged;

                _mouseHook.MouseMove -= MouseHook_MouseMove;
                _mouseHook.MouseDown -= MouseHook_MouseDown;
                _mouseHook.MouseUp -= MouseHook_MouseUp;
                _mouseHook.UnHook();

            };

            Core.Windows.ShowMainWindow();
            Core.AutoStartGame();
        }

        private void Core_StashStatusChanged(object sender, EventArgs e)
        {
            if (Core.Runtime.StashOpened)
            {
                _overlayManager.ShowMainWindow();
                _mouseHook.SetHook();
            }
            else
            {
                _overlayManager.HideMainWindow();
                _mouseHook.UnHook();
            }
        }

        #region D3DHook Events

        private void D3DHook_FrameDrawing(float ms)
        {
            _overlayManager.Viewport.DrawRoutine(ms);
        }

        #endregion

        #region GDWindowHook Events

        private void GDWindowHook_MoveSize(object sender, EventArgs e)
        {
            Core.Runtime.SetWindowLocSize(_gdWindowHookService.GetLocationSize());
        }

        private void GDWindowHook_GotFocus(object sender, EventArgs e)
        {
            Core.Runtime.WindowFocused = true;
        }

        private void GDWindowHook_LostFocus(object sender, EventArgs e)
        {
            Core.Runtime.WindowFocused = false;
        }

        private void GDWindowHook_WindowDestroyed(object sender, EventArgs e)
        {
            Core.Runtime.WindowFocused = false;
            if (Core.Config.CloseWithGrimDawn) Program.Quit();
        }

        #endregion

        #region GDGameHook Events

        private void GDGameHook_StashStatusChanged(object sender, GDGameHookService.StashStatusChangedEventArgs e)
        {
            Core.Runtime.StashOpened = e.Opened;
        }

        private void GDGameHook_ModeStatusChanged(object sender, GDGameHookService.ModeChangedEventArgs e)
        {
            Core.Runtime.CurrentMode = e.IsHardcore ? GrimDawnGameMode.HC : GrimDawnGameMode.SC;
        }

        private void GDGameHook_ExpansionStatusChanged(object sender, GDGameHookService.ExpansionChangedEventArgs e)
        {
            Core.Runtime.CurrentExpansion = (GrimDawnGameExpansion)e.ExpansionID;
        }

        #endregion

        #region MouseHook Events

        private void MouseHook_MouseMove(object sender, Native.Mouse.Hook.MouseEventArgs e)
        {
            if (!Core.Runtime.WindowFocused || !Core.Runtime.StashOpened) return;
            bool hit = _overlayManager.Viewport.CheckMouseMove(e.X - (int)Core.Runtime.WindowLocation.X, e.Y - (int)Core.Runtime.WindowLocation.Y);
            if (hit)
            {
            }
        }

        private void MouseHook_MouseDown(object sender, Native.Mouse.Hook.MouseEventArgs e)
        {
            if (!Core.Runtime.WindowFocused || !Core.Runtime.StashOpened) return;
            bool hit = _overlayManager.Viewport.CheckMouseDown(e.X - (int)Core.Runtime.WindowLocation.X, e.Y - (int)Core.Runtime.WindowLocation.Y);
            if (hit)
            {
                // this is handled inside overlay mainwindow
            }
        }

        private void MouseHook_MouseUp(object sender, Native.Mouse.Hook.MouseEventArgs e)
        {
            Core.Runtime.EnableMovement();
            if (!Core.Runtime.WindowFocused || !Core.Runtime.StashOpened) return;
            bool hit = _overlayManager.Viewport.CheckMouseUp(e.X - (int)Core.Runtime.WindowLocation.X, e.Y - (int)Core.Runtime.WindowLocation.Y);
            if (hit)
            {
            }
        }

        #endregion

    }
}
