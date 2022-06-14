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
        private readonly Core.Localization.StringsProxy L;

        private readonly GDWindowHookService _gdWindowHookService;
        private readonly GDGameHookService _gdGameHookService;
        private readonly GDOverlayService _gdOverlayService;
        private readonly Native.Mouse.Hook _mouseHook;

        private Overlay.Elements.Viewport _overlayViewport;

        private static Mutex mutex = null;

        public GDMSContext()
        {
            L = new Core.Localization.StringsProxy();

            Core.Files.EnsureDirectoriesExist();
            Core.Localization.LoadLanguages();
            Core.Config.Load();
            Core.Config.LanguageChanged += delegate {
                Core.Windows.LocalizeWindows();
            };

            if (Core.Config.IsNew)
            {
                Core.Localization.LoadLanguage("enUS");
                Core.Windows.ShowSetupDialog(true);
            }
            else
            {
                Core.Localization.LoadLanguage(Core.Config.Language);
            }

            Core.Stashes.CreateMainStashes();

            // allow only one instance of gdms
            mutex = new Mutex(true, "GDMultiStash", out bool createdNew);
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

            // check for new version
            if (Core.Update.NewVersionAvailable())
            {
                if (Core.Config.AutoUpdate)
                {
                    Core.Update.StartUpdater();
                    return;
                }
                string msg = "New version available: {0}\nUpdate now?".Format(Core.Update.NewVersionName);
                if (MessageBox.Show(msg, "", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    Core.Update.StartUpdater();
                    return;
                }
            }

            Core.GD.LoadItemSizes(Properties.Resources.itemsizes);
            Core.GD.SetCurrentGameExpansion();
            Console.WriteLine("GD Game Path: " + Core.Config.GamePath);
            Console.WriteLine("GD Game Expansion: " + GrimDawn.GetExpansionName(Core.GD.InstalledGameExpansion));
            Console.WriteLine("Loading Stashes:");
            Core.Stashes.LoadStashes();

            _overlayViewport = new Overlay.Elements.Viewport();
            _gdOverlayService = new GDOverlayService(_overlayViewport);

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
            _mouseHook.MouseWheel += MouseHook_MouseWheel;

            _gdGameHookService.StashStatusChanged += GDGameHook_StashStatusChanged;
            _gdGameHookService.ModeStatusChanged += GDGameHook_ModeStatusChanged;
            _gdGameHookService.ExpansionChanged += GDGameHook_ExpansionStatusChanged;
            _gdGameHookService.TransferStashSaved += GDGameHook_TransferStashSaved;

            _gdOverlayService.FrameDrawing += D3DHook_FrameDrawing;

            Core.Runtime.StashStatusChanged += Core_StashStatusChanged;

            Console.WriteLine("Starting GD Window Hook Service ...");
            _gdWindowHookService.Start();

            Console.WriteLine("Starting GD Game Hook Service ...");
            _gdGameHookService.Start();

            Console.WriteLine("Starting Overlay Service ...");
            _gdOverlayService.Start();

            ThreadExit += delegate {

                Core.Runtime.StashStatusChanged -= Core_StashStatusChanged;

                _gdWindowHookService.Stop();
                _gdGameHookService.Stop();
                _gdOverlayService.Stop();

                _overlayViewport.Destroy();
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
                _mouseHook.MouseWheel -= MouseHook_MouseWheel;
                _mouseHook.UnHook();

            };

            Core.Windows.ShowMainWindow();
            Core.AutoStartGame();
        }

        private void Core_StashStatusChanged(object sender, EventArgs e)
        {
            if (Core.Runtime.StashOpened)
            {
                _overlayViewport.ShowMainWindow();
                _mouseHook.SetHook();
            }
            else
            {
                _overlayViewport.HideMainWindow();
                _mouseHook.UnHook();
            }
        }

        #region D3DHook Events

        private void D3DHook_FrameDrawing(float ms)
        {
            _overlayViewport.DrawRoutine(ms);
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
            if (Core.Runtime.StashOpened)
                _mouseHook.SetHook();
        }

        private void GDWindowHook_LostFocus(object sender, EventArgs e)
        {
            Core.Runtime.WindowFocused = false;
            if (Core.Runtime.StashOpened)
                _mouseHook.UnHook();
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

        private void GDGameHook_TransferStashSaved(object sender, EventArgs e)
        {
            Core.Runtime.NotifyTransferStashSaved();
        }

        #endregion

        #region MouseHook Events

        private void MouseHook_MouseMove(object sender, Native.Mouse.Hook.MouseEventArgs e)
        {
            if (!Core.Runtime.WindowFocused || !Core.Runtime.StashOpened) return;
            bool hit = _overlayViewport.CheckMouseMove(e.X - (int)Core.Runtime.WindowLocation.X, e.Y - (int)Core.Runtime.WindowLocation.Y);
            if (hit)
            {
            }
        }

        private void MouseHook_MouseDown(object sender, Native.Mouse.Hook.MouseEventArgs e)
        {
            if (!Core.Runtime.WindowFocused || !Core.Runtime.StashOpened) return;
            bool hit = _overlayViewport.CheckMouseDown(e.X - (int)Core.Runtime.WindowLocation.X, e.Y - (int)Core.Runtime.WindowLocation.Y);
            if (hit)
            {
                // this is handled inside overlay mainwindow
            }
        }
        private void MouseHook_MouseUp(object sender, Native.Mouse.Hook.MouseEventArgs e)
        {
            Core.Runtime.EnableMovement();
            if (!Core.Runtime.WindowFocused || !Core.Runtime.StashOpened) return;
            bool hit = _overlayViewport.CheckMouseUp(e.X - (int)Core.Runtime.WindowLocation.X, e.Y - (int)Core.Runtime.WindowLocation.Y);
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
            if (!Core.Runtime.WindowFocused || !Core.Runtime.StashOpened) return;
            bool hit = _overlayViewport.OnMouseWheel(e.X - (int)Core.Runtime.WindowLocation.X, e.Y - (int)Core.Runtime.WindowLocation.Y, e.Delta);
            if (hit)
            {

                /*
                isBySys = true;
                Native.Input inp = new Native.Input
                {
                    type = Native.InputType.Mouse,
                    u = new Native.InputUnion
                    {
                        mi = new Native.MouseInput
                        {
                            dx = e.X,
                            dy = e.Y,
                            mouseData = -e.Delta,
                            dwFlags = 0x0800,
                        }
                    }
                };
                Native.SendInput(1, new Native.Input[] { inp }, System.Runtime.InteropServices.Marshal.SizeOf(typeof(Native.Input)));
                */


            }
        }

        #endregion

    }
}
