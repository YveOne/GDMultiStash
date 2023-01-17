using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

using D3DHook.Interface;
using D3DHook.Hook;
using D3DHook;

namespace GDMultiStash.Services
{
    internal class GDOverlayService : Base.Service
    {

        private readonly D3DHook.Hook.Common.Overlay _dummyOverlay;
        private readonly D3DHook.Hook.Common.Overlay _overlay;

        private readonly Overlay.GDMSViewport _viewport;

        private CaptureProcess _captureProcess;
        private Thread _drawThread = null;
        private bool _dummyFrameDrawn = false;

        public event MessageReceivedEvent RemoteMessage;
        public event FrameDrawingEvent FrameDrawing;

        public GDOverlayService(Overlay.GDMSViewport viewport)
        {
            _viewport = viewport;

            _dummyOverlay = new D3DHook.Hook.Common.Overlay()
            {
                Hidden = false,
                Elements = new List<D3DHook.Hook.Common.IOverlayElement>(),
            };
            _overlay = new D3DHook.Hook.Common.Overlay()
            {
                Hidden = false,
                Elements = new List<D3DHook.Hook.Common.IOverlayElement>(),
            };
        }














        public override bool Start()
        {
            if (Running) return false;
            if (_drawThread == null)
            {
                _drawThread = new Thread(InitializeOverlay);
                _drawThread.Start();
            }
            return base.Start();
        }

        public override bool Stop()
        {
            if (!Running) return false;
            if (_drawThread != null)
            {
                _drawThread.Abort();
                _drawThread = null;
            }
            DetachProcess();
            return base.Stop();
        }

        public override void Destroy()
        {
            base.Destroy();
            _viewport.Destroy();
        }


        private void SaveSleep(int ms)
        {
            long sleepUntill = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond + ms;
            while(DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond < sleepUntill)
            {
                if (_drawThread == null) return;
                Thread.Sleep(10);
            }
        }

        private void InitializeOverlay()
        {
            try
            {
                Console.WriteLine("[D3DHook] Init started");
                while (_drawThread != null && !IsAttached)
                {
                    AttachProcess();
                    //Thread.Sleep(3000);
                    SaveSleep(3000);
                }
                if (_drawThread == null) return;
                Console.WriteLine("[D3DHook] Process attached");

                _viewport.OverlayResources.LoadQueuedResourcesFromCache();
                _viewport.Redraw(true);

                //Thread.Sleep(1000);
                SaveSleep(1000);

                _dummyFrameDrawn = false;
                while (_drawThread != null && IsAttached && !_dummyFrameDrawn)
                {
                    DrawOverlay(_dummyOverlay);
                    //Thread.Sleep(100);
                    SaveSleep(100);
                }
                if (_drawThread == null) return;
                Console.WriteLine("[D3DHook] Overlay is ready");

            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception e)
            {
                Console.WriteLine("[D3DHook] Exception in InitializeOverlay()");
                Console.WriteLine(e.Message);
                if (e.InnerException != null) Console.WriteLine(e.InnerException.Message);
            }
            finally
            {
                _drawThread = null;
            }
        }

        private void DrawOverlay(D3DHook.Hook.Common.IOverlay overlay)
        {
            if (_captureProcess != null)
            {
                _captureProcess.CaptureInterface.DrawOverlayInGame(overlay);
            }
        }





















        public bool IsAttached { get { return _captureProcess != null; } }

        private void DetachProcess()
        {
            if (_captureProcess != null)
            {
                HookManager.RemoveHookedProcess(_captureProcess.Process.Id);
                _captureProcess.CaptureInterface.Disconnect();
                _captureProcess = null;
            }
        }

        private void AttachProcess()
        {
            if (_captureProcess != null) return;
            Process[] processes = Process.GetProcessesByName("Grim Dawn");
            if (processes.Length == 0) return;
            Console.WriteLine("[D3DHook] AttachProcess() - {0} processes found", processes.Length);

            int i = -1;
            foreach (Process process in processes)
            {
                i += 1;
                // Simply attach to the first one found.

                // If the process doesn't have a mainwindowhandle yet, skip it (we need to be able to get the hwnd to set foreground etc)
                if (process.MainWindowHandle == IntPtr.Zero)
                {
                    Console.WriteLine("[D3DHook] AttachProcess() - Process {0} is IntPtr.Zero", i);
                    continue;
                }

                // Skip if the process is already hooked (and we want to hook multiple applications)
                if (HookManager.IsHooked(process.Id))
                {
                    Console.WriteLine("[D3DHook] AttachProcess() - Process {0} already hooked", i);
                    continue;
                }

                CaptureConfig cc = new CaptureConfig()
                {
                    Direct3DVersion = Direct3DVersion.AutoDetect,
                    ShowOverlay = true,
                };
                CaptureInterface _captureInterface = new CaptureInterface();
                _captureInterface.FrameDrawing += new FrameDrawingEvent((float ms) => {
                    if (!_dummyFrameDrawn)
                    {
                        DrawOverlay(null);
                        _dummyFrameDrawn = true;
                    }
                    else
                    {
                        if (_viewport.OverlayResources.CreateAndGetNewResources(out List<D3DHook.Hook.Common.IResource> res))
                        {
                            _captureProcess.CaptureInterface.InitializeResources(res);
                        }
                        if (_viewport.UpdateRequested())
                        {
                            _overlay.Elements = _viewport.GetImagesRecursive();
                        }
                        if (_viewport.RedrawRequested())
                        {
                            DrawOverlay(_overlay);
                        }
                    }
                    FrameDrawing?.Invoke(ms);
                });
                _captureProcess = new CaptureProcess(process, cc, _captureInterface);
                _captureProcess.CaptureInterface.RemoteMessage += RemoteMessage;

                break;
            }
            Thread.Sleep(10);
        }





    }

}
