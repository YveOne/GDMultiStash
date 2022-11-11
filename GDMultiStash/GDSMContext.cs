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

        public Overlay.Elements.Viewport Viewport { get; private set; }

        private readonly GDWindowHookService _gdWindowHookService;
        private readonly GDGameHookService _gdGameHookService;
        private readonly GDOverlayService _gdOverlayService;
        private readonly Native.Mouse.Hook _mouseHook;
        private readonly Mutex _singleInstanceMutex;
        private bool _servicesInstalled = false;

        public GDMSContext()
        {
            Global.FileSystem.CreateDirectories();

            Global.Localization.SaveDefaultFile("enGB-English (GB).txt", Properties.Resources.local_enGB);
            Global.Localization.SaveDefaultFile("enUS-English (US).txt", Properties.Resources.local_enUS);
            Global.Localization.SaveDefaultFile("deDE-Deutsch.txt", Properties.Resources.local_deDE);
            Global.Localization.SaveDefaultFile("zhCN-简体中文.txt", Properties.Resources.local_zhCN);
            Global.Localization.LoadLanguages();
            Global.Localization.LoadLanguage("enUS");

            Global.Configuration.Load();
            Global.Configuration.LanguageChanged += Configuration_LanguageChanged;

            Global.Windows.InitWindows();

            if (Global.Configuration.IsNew)
            {
                Global.Windows.ShowSetupDialog(true);
                // language already loaded after setup dialog closed
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
                    (IntPtr)Native.HWND_BROADCAST,
                    Native.WM_SHOWME,
                    IntPtr.Zero,
                    IntPtr.Zero);
                Program.Quit();
                return;
            }

            Global.Windows.LocalizeWindows();

            if (Global.Configuration.IsNew)
            {
                Global.Stashes.CreateMainStashes();
            }

            // game install path not found? let user choose path
            if (!GrimDawn.ValidGamePath(Global.Configuration.Settings.GamePath))
            {
                Program.ShowError(Global.L["err_gamedir_not_found"]);
                Global.Windows.ShowSetupDialog(true);
            }

            // game install path still not found? poooor user...
            if (!GrimDawn.ValidGamePath(Global.Configuration.Settings.GamePath))
            {
                Program.ShowError(Global.L["err_gamedir_not_found"]);
                Program.Quit();
                return;
            }

            if (!GrimDawn.ValidDocsPath())
            {
                Program.ShowError(Global.L["err_docsdir_not_found"]);
                Program.Quit();
                return;
            }

            // check for new version
            if (Global.Update.NewVersionAvailable())
            {
                if (Global.Configuration.Settings.AutoUpdate)
                {
                    Global.Update.StartUpdater();
                    return;
                }
                string msg = string.Format("New version available: {0}\nUpdate now?", Global.Update.NewVersionName);
                if (MessageBox.Show(msg, "", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    Global.Update.StartUpdater();
                    return;
                }
            }

            Global.Database.LoadItemSizes(Properties.Resources.itemsizes);
            Console.WriteLine("GD Game Path: " + Global.Configuration.Settings.GamePath);
            Console.WriteLine("GD Game Expansion: " + GrimDawn.GetExpansionName(GrimDawn.GetInstalledExpansionFromPath(Global.Configuration.Settings.GamePath)));
            Console.WriteLine("Loading Stashes:");
            Global.Stashes.LoadStashes();

            Viewport = new Overlay.Elements.Viewport();

            _gdOverlayService = new GDOverlayService(Viewport);
            _gdWindowHookService = new GDWindowHookService();
            _gdGameHookService = new GDGameHookService();
            _mouseHook = new Native.Mouse.Hook();

            StartServices();
            ThreadExit += delegate {
                StopServices();
                _gdWindowHookService.Destroy();
                _gdGameHookService.Destroy();
                _gdOverlayService.Destroy();
            };

            Global.Windows.ShowMainWindow(() => {
                Global.Runtime.AutoStartGame();
            });
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

            Global.Runtime.StashStatusChanged += Core_StashStatusChanged;
        }

        private void StopServices()
        {
            Global.Runtime.StashStatusChanged -= Core_StashStatusChanged;

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

        private void Core_StashStatusChanged(object sender, EventArgs e)
        {
            if (Global.Runtime.StashOpened)
            {
                Viewport.ShowMainWindow();
                _mouseHook.SetHook();
            }
            else
            {
                Viewport.HideMainWindow();
                _mouseHook.UnHook();
            }
        }

        #region D3DHook Events

        private void D3DHook_FrameDrawing(float ms)
        {
            Viewport.DrawRoutine(ms);
        }

        #endregion

        #region GDWindowHook Events

        private void GDWindowHook_HookInstalled(object sender, EventArgs e)
        {
            if (Global.Configuration.Settings.AutoStartGD)
            {
                Console.WriteLine("Setting GD to foreground");
                IntPtr m_target = Native.FindWindow("Grim Dawn", null);
                if (Native.IsIconic(m_target))
                {
                    Native.ShowWindowAsync(m_target, Native.SW_RESTORE);
                }
                Native.SetForegroundWindow(m_target);
            }
        }

        private void GDWindowHook_MoveSize(object sender, EventArgs e)
        {
            Global.Runtime.SetGameWindowLocSize(_gdWindowHookService.GetLocationSize());
        }

        private void GDWindowHook_GotFocus(object sender, EventArgs e)
        {
            Console.WriteLine("Grim Dawn window got focus");
            Global.Runtime.SetGameWindowLocSize(_gdWindowHookService.GetLocationSize());
            Global.Runtime.GameWindowFocused = true;
            if (Global.Runtime.StashOpened)
                _mouseHook.SetHook();
        }

        private void GDWindowHook_LostFocus(object sender, EventArgs e)
        {
            Console.WriteLine("Grim Dawn window lost focus");
            Global.Runtime.GameWindowFocused = false;
            if (Global.Runtime.StashOpened)
                _mouseHook.UnHook();
        }

        private void GDWindowHook_WindowDestroyed(object sender, EventArgs e)
        {
            Console.WriteLine("Grim Dawn window closed");
            Global.Runtime.GameWindowFocused = false;
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
            Global.Runtime.StashOpened = e.Opened;
        }

        private void GDGameHook_ModeStatusChanged(object sender, GDGameHookService.ModeChangedEventArgs e)
        {
            Global.Runtime.CurrentMode = e.IsHardcore ? GrimDawnGameMode.HC : GrimDawnGameMode.SC;
        }

        private void GDGameHook_ExpansionStatusChanged(object sender, GDGameHookService.ExpansionChangedEventArgs e)
        {
            Global.Runtime.CurrentExpansion = (GrimDawnGameExpansion)e.ExpansionID;
        }

        private void GDGameHook_TransferStashSaved(object sender, EventArgs e)
        {
            Global.Runtime.NotifyTransferStashSaved();
        }

        #endregion

        #region MouseHook Events

        private void MouseHook_MouseMove(object sender, Native.Mouse.Hook.MouseEventArgs e)
        {
            //if (!Global.Runtime.GameWindowFocused || (!Global.Runtime.StashOpened && !Global.Runtime.StashIsReopening)) return;
            if (!Global.Runtime.GameWindowFocused) return;
            bool hit = Viewport.CheckMouseMove(e.X - (int)Global.Runtime.GameWindowLocation.X, e.Y - (int)Global.Runtime.GameWindowLocation.Y);
            if (hit)
            {
            }
        }

        private void MouseHook_MouseDown(object sender, Native.Mouse.Hook.MouseEventArgs e)
        {
            //if (!Global.Runtime.GameWindowFocused || (!Global.Runtime.StashOpened && !Global.Runtime.StashIsReopening)) return;
            if (!Global.Runtime.GameWindowFocused) return;
            bool hit = Viewport.CheckMouseDown(e.X - (int)Global.Runtime.GameWindowLocation.X, e.Y - (int)Global.Runtime.GameWindowLocation.Y);
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
            bool hit = Viewport.CheckMouseUp(e.X - (int)Global.Runtime.GameWindowLocation.X, e.Y - (int)Global.Runtime.GameWindowLocation.Y);
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
            if (!Global.Runtime.GameWindowFocused || (!Global.Runtime.StashOpened && !Global.Runtime.StashIsReopening)) return;
            bool hit = Viewport.OnMouseWheel(e.X - (int)Global.Runtime.GameWindowLocation.X, e.Y - (int)Global.Runtime.GameWindowLocation.Y, e.Delta);
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
