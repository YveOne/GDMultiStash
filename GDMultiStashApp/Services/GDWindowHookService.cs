using System;
using System.Windows.Forms;

namespace GDMultiStash.Services
{
    internal class GDWindowHookService : Base.Service
    {

        private class TargetForm : Form
        {
            public TargetForm()
            {
                Opacity = 0.001337;
                Show();
                Hide();
            }
        }

        public struct LocationSize
        {
            public int X;
            public int Y;
            public int Width;
            public int Height;
        }

        private LocationSize _locationSize;

        public event EventHandler<EventArgs> HookInstalled;
        //public event EventHandler<EventArgs> HookDestroyed;
        public event EventHandler<EventArgs> MoveSize;
        public event EventHandler<EventArgs> GotFocus;
        public event EventHandler<EventArgs> LostFocus;
        public event EventHandler<EventArgs> WindowDestroyed;

        private readonly System.Timers.Timer _timer;
        private readonly TargetForm _target;

        private uint m_processId, m_threadId;
        private Native.WinEventDelegate m_winEventDelegate;
        private IntPtr m_target;
        private IntPtr m_hook1; // move size start/end
        private IntPtr m_hook2; // location change
        private IntPtr m_hook3; // foreground change
        private IntPtr m_hook4; // window destroy

        public GDWindowHookService() {
            _locationSize = new LocationSize
            {
                X = 0,
                Y = 0,
                Width = 0,
                Height = 0
            };
            _target = new TargetForm();
            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += delegate {
                _target.Invoke(new Action(() => { Hook(); }));
            };
        }

        private void StartTimer()
        {
            _timer.AutoReset = true;
            _timer.Start();
        }

        private void StopTimer()
        {
            _timer.AutoReset = false;
            _timer.Stop();
            Application.DoEvents();
        }

        private void Hook()
        {
            m_target = Native.FindWindow("Grim Dawn", null);
            if (m_target == IntPtr.Zero) return;
            StopTimer();

            m_threadId = Native.GetWindowThreadProcessId(m_target, out m_processId);
            m_winEventDelegate = CatchWinEvent;
            m_hook4 = Native.SetWinEventHook(
                Native.EVENT.OBJECT_DESTROY,
                Native.EVENT.OBJECT_DESTROY,
                m_target, m_winEventDelegate, m_processId, m_threadId,
                (uint)Native.WINEVENT.OUTOFCONTEXT);
            m_hook1 = Native.SetWinEventHook(
                Native.EVENT.SYSTEM_MOVESIZESTART,
                Native.EVENT.SYSTEM_MOVESIZEEND,
                m_target, m_winEventDelegate, m_processId, m_threadId,
                (uint)Native.WINEVENT.OUTOFCONTEXT);
            m_hook2 = Native.SetWinEventHook(
                Native.EVENT.OBJECT_LOCATIONCHANGE,
                Native.EVENT.OBJECT_LOCATIONCHANGE,
                m_target, m_winEventDelegate, m_processId, m_threadId,
                (uint)Native.WINEVENT.OUTOFCONTEXT);
            m_hook3 = Native.SetWinEventHook(
                Native.EVENT.SYSTEM_FOREGROUND,
                Native.EVENT.SYSTEM_FOREGROUND,
                m_target, m_winEventDelegate, 0, 0,
                (uint)Native.WINEVENT.OUTOFCONTEXT);

            Console.WriteLine("[GDWindowHook] Hooks installed");
            Console.WriteLine(" target: " + m_target.ToString());
            Console.WriteLine(" thread: " + m_threadId.ToString());
            Console.WriteLine(" process: " + m_processId.ToString());

            HookInstalled?.Invoke(null, EventArgs.Empty);
            //Console.WriteLine($"------- {m_target} {Native.GetForegroundWindow()}");
            //if (Native.GetForegroundWindow() == m_target)
            //    GotFocus?.Invoke(null, EventArgs.Empty);
            hasFocus = true;
            GotFocus?.Invoke(null, new EventArgs());
            new System.Threading.Thread(() => {
                System.Threading.Thread.Sleep(1000);
                Native.SetForegroundWindow(m_target);
            }).Start();
        }

        private void Unhook()
        {
            if (m_target == IntPtr.Zero) return;
            m_target = IntPtr.Zero;
            Native.UnhookWinEvent(m_hook4);
            Native.UnhookWinEvent(m_hook1);
            Native.UnhookWinEvent(m_hook2);
            Native.UnhookWinEvent(m_hook3);
            Console.WriteLine("[GDWindowHook] Hooks uninstalled");
            //HookDestroyed?.Invoke(null, EventArgs.Empty);
        }

        public override bool Start()
        {
            if (Running) return false;
            StartTimer();
            return base.Start();
        }

        public override bool Stop()
        {
            if (!Running) return false;
            Unhook();
            StopTimer();
            return base.Stop();
        }

        public override void Destroy()
        {
            base.Destroy();
        }

        private bool isMoving = false;
        private bool hasFocus = false;

        public void SetHasFocus(bool v)
        {
            hasFocus = v;
        }

        private void CatchWinEvent(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            switch(eventType)
            {
                case Native.EVENT.OBJECT_DESTROY:
                    if (Native.FindWindow("Grim Dawn", null) != IntPtr.Zero) return;
                    WindowDestroyed?.Invoke(null, new EventArgs());
                    hasFocus = false;
                    Unhook();
                    StartTimer();
                    break;
                case Native.EVENT.SYSTEM_MOVESIZESTART:
                    isMoving = true;
                    break;
                case Native.EVENT.SYSTEM_MOVESIZEEND:
                    MoveSizeInvoke();
                    isMoving = false;
                    break;
                case Native.EVENT.OBJECT_LOCATIONCHANGE:
                    if (!isMoving) MoveSizeInvoke();
                    break;
                case Native.EVENT.SYSTEM_FOREGROUND:
                    if (hwnd == m_target)
                    {
                        if (hasFocus) return;
                        hasFocus = true;
                        GotFocus?.Invoke(null, new EventArgs());
                    }
                    else
                    {
                        if (!hasFocus) return;
                        hasFocus = false;
                        LostFocus?.Invoke(null, new EventArgs());
                    }
                    break;
            }
        }
        
        private string curSizeKey = "";

        private void MoveSizeInvoke()
        {

            Native.POINT point1 = new Native.POINT(0, 0);
            Native.ClientToScreen(m_target, ref point1);
            Native.GetClientRect(m_target, out Native.RECT rect2);

            int x = point1.X;
            int y = point1.Y;
            int w = rect2.Right - rect2.Left;
            int h = rect2.Bottom - rect2.Top;

            string sizeKey = string.Format("{0}.{1}.{2}.{3}", x, y, w, h);
            if (curSizeKey == sizeKey) return;
            curSizeKey = sizeKey;

            _locationSize = new LocationSize
            {
                X = x,
                Y = y,
                Width = w,
                Height = h,
            };

            MoveSize?.Invoke(null, new EventArgs());
        }

        public LocationSize GetLocationSize()
        {
            return _locationSize;
        }

    }
}
