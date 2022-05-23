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
    internal class GDOverlayService : Service
    {

        private readonly D3DHook.Hook.Common.Overlay _dummyOverlay;
        private readonly D3DHook.Hook.Common.Overlay _overlay;

        private readonly Overlay.Elements.Viewport _viewport;

        private CaptureProcess _captureProcess;
        private Thread _drawThread = null;
        private bool _dummyFrameDrawn = false;

        public event MessageReceivedEvent RemoteMessage;
        public event FrameDrawingEvent FrameDrawing;

        public GDOverlayService(Overlay.Elements.Viewport viewport)
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

            _dummyFrameDrawn = false;
            if (_drawThread == null)
            {
                _drawThread = new Thread(DrawThreadSub);
                _drawThread.Start();
            }

            return base.Start();
        }

        public override bool Stop()
        {
            if (!Running) return false;

            if (_drawThread != null)
            {
                //_drawThread.Abort();
                // dont abort, just let the thread run out
                _drawThread = null;
            }
            DetachProcess();

            return base.Stop();
        }

        private void DrawThreadSub()
        {
            try
            {
                while (_drawThread != null && !IsAttached)
                {
                    AttachProcess();
                    Thread.Sleep(1000);
                }
                if (_drawThread == null) return;
                Console.WriteLine("[D3DHook] Process attached");
                Thread.Sleep(1000);
                while (_drawThread != null && IsAttached && !_dummyFrameDrawn)
                {
                    DrawOverlay(_dummyOverlay);
                    Thread.Sleep(100);
                }
                if (_drawThread == null) return;
                Console.WriteLine("[D3DHook] Overlay is ready");
            }
            catch (Exception)
            {
            }
            _drawThread = null;
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
            foreach (Process process in processes)
            {
                // Simply attach to the first one found.

                // If the process doesn't have a mainwindowhandle yet, skip it (we need to be able to get the hwnd to set foreground etc)
                if (process.MainWindowHandle == IntPtr.Zero) continue;

                // Skip if the process is already hooked (and we want to hook multiple applications)
                if (HookManager.IsHooked(process.Id)) continue;

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
                        if (_viewport.Resources.GetQueuedResources(out List<D3DHook.Hook.Common.IResource> res))
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
