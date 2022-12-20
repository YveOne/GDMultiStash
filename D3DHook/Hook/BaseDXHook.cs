using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using EasyHook;
using System.IO;
using System.Runtime.Remoting;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using D3DHook.Interface;
using System.Threading;

namespace D3DHook.Hook
{

    public abstract class BaseDXHook : SharpDX.Component, IDXHook
    {
        protected readonly ClientCaptureInterfaceEventProxy InterfaceEventProxy = new ClientCaptureInterfaceEventProxy();

        public BaseDXHook(CaptureInterface ssInterface)
        {
            Interface = ssInterface;
            Timer = new Stopwatch();
            Timer.Start();
            //Resources = new List<Common.IResource>();
            Resources = new Dictionary<int, Common.IResource>();

            Interface.DrawOverlay += InterfaceEventProxy.DrawOverlayProxyHandler;
            InterfaceEventProxy.DrawOverlay += InterfaceEventProxy_DrawOverlay;

            Interface.InitResources += InterfaceEventProxy.InitResourcesProxyHandler;
            InterfaceEventProxy.InitResources += InterfaceEventProxy_InitResources;
        }
        
        ~BaseDXHook()
        {
            Dispose(false);
        }

        private void InterfaceEventProxy_DrawOverlay(DrawOverlayEventArgs args)
        {
            Overlays = new List<Common.IOverlay>();
            if (args.Overlay != null)
                Overlays.Add(args.Overlay);
            IsOverlayUpdatePending = true;
        }

        private void InterfaceEventProxy_InitResources(InitResourcesEventArgs args)
        {
            if (args.Resources != null)
            {
                foreach (var r in args.Resources)
                    Resources[r.UID] = r;
            }
            IsResourcesUpdatePending = true;
        }


        

        protected Stopwatch Timer { get; set; }


        protected List<Common.IOverlay> Overlays { get; set; }
        //protected List<Common.IResource> Resources { get; set; }
        protected Dictionary<int, Common.IResource> Resources { get; set; }

        protected bool IsOverlayUpdatePending { get; set; }
        protected bool IsResourcesUpdatePending { get; set; }

        int _processId = 0;
        protected int ProcessId
        {
            get
            {
                if (_processId == 0)
                {
                    _processId = RemoteHooking.GetCurrentProcessId();
                }
                return _processId;
            }
        }

        protected virtual string HookName
        {
            get
            {
                return "BaseDXHook";
            }
        }


        private long lastTickCount = 0;

        protected void Frame()
        {
            float ms = 0;
            long nowTickCount = DateTime.Now.Ticks;
            if (lastTickCount != 0) ms = (nowTickCount - lastTickCount) / 10000f;
            lastTickCount = nowTickCount;

            Interface.Frame(ms);
        }

        protected void DebugMessage(string message)
        {
#if DEBUG
            try
            {
                Interface.Message(MessageType.Debug, HookName + ": " + message);
            }
            catch (RemotingException)
            {
                // Ignore remoting exceptions
            }
            catch (Exception)
            {
                // Ignore all other exceptions
            }
#endif
        }

        protected IntPtr[] GetVTblAddresses(IntPtr pointer, int numberOfMethods)
        {
            return GetVTblAddresses(pointer, 0, numberOfMethods);
        }

        protected IntPtr[] GetVTblAddresses(IntPtr pointer, int startIndex, int numberOfMethods)
        {
            List<IntPtr> vtblAddresses = new List<IntPtr>();

            IntPtr vTable = Marshal.ReadIntPtr(pointer);
            for (int i = startIndex; i < startIndex + numberOfMethods; i++)
                vtblAddresses.Add(Marshal.ReadIntPtr(vTable, i * IntPtr.Size)); // using IntPtr.Size allows us to support both 32 and 64-bit processes

            return vtblAddresses.ToArray();
        }


        #region IDXHook Members

        public CaptureInterface Interface
        {
            get;
            set;
        }

        public CaptureConfig Config { get; set; }

        protected List<Hook> Hooks = new List<Hook>();

        public abstract void Hook();

        public abstract void Cleanup();

        #endregion

        #region IDispose Implementation

        protected override void Dispose(bool disposeManagedResources)
        {
            // Only clean up managed objects if disposing (i.e. not called from destructor)
            if (disposeManagedResources)
            {
                try
                {
                    Cleanup();
                }
                catch { }

                try
                {
                    // Uninstall Hooks
                    if (Hooks.Count > 0)
                    {
                        // First disable the hook (by excluding all threads) and wait long enough to ensure that all hooks are not active
                        foreach (var hook in Hooks)
                        {
                            // Lets ensure that no threads will be intercepted again
                            hook.Deactivate();
                        }

                        System.Threading.Thread.Sleep(100);

                        // Now we can dispose of the hooks (which triggers the removal of the hook)
                        foreach (var hook in Hooks)
                        {
                            hook.Dispose();
                        }

                        Hooks.Clear();
                    }

                    try
                    {
                        // Remove the event handlers
                        //Interface.DisplayText -= InterfaceEventProxy.DisplayTextProxyHandler;
                        Interface.DrawOverlay -= InterfaceEventProxy_DrawOverlay;
                    }
                    catch (RemotingException) { } // Ignore remoting exceptions (host process may have been closed)
                }
                catch
                {
                }
            }

            base.Dispose(disposeManagedResources);
        }

        #endregion
    }
}
