using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

public static partial class Native
{
    public static partial class Mouse
    {
        public class Hook : Native.Hook
        {

            private Point _point;

            public Point Point => _point;

            public class MouseEventArgs : System.Windows.Forms.MouseEventArgs
            {
                public bool Block = false;
                public MouseEventArgs(MouseButtons button, int clicks, int x, int y, int delta)
                    : base(button, clicks, x, y, delta)
                {

                }
            }

            public Hook()
            {
                _point = new Point();
            }

            public override int SetHook()
            {
                return SetHook(WH.MOUSE_LL, IntPtr.Zero);
            }

            public override void UnHook()
            {
                base.UnHook();
            }

            protected override int HookProc(int nCode, IntPtr wParam, IntPtr lParam)
            {
                MouseHookStruct mouseStruct = (MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseHookStruct));
                _point.x = mouseStruct.pt.x;
                _point.y = mouseStruct.pt.y;
                MouseEventArgs args = null;
                switch ((Int32)wParam)
                {

                    case WM_MOUSEMOVE:
                        args = new MouseEventArgs(MouseButtons.None, 0, mouseStruct.pt.x, mouseStruct.pt.y, 0);
                        MouseMove?.Invoke(this, args);
                        break;

                    case WM_LBUTTONDOWN:
                        args = new MouseEventArgs(MouseButtons.Left, 0, mouseStruct.pt.x, mouseStruct.pt.y, 0);
                        MouseDown?.Invoke(this, args);
                        break;

                    case WM_RBUTTONDOWN:
                        args = new MouseEventArgs(MouseButtons.Right, 0, mouseStruct.pt.x, mouseStruct.pt.y, 0);
                        MouseDown?.Invoke(this, args);
                        break;

                    case WM_MBUTTONDOWN:
                        args = new MouseEventArgs(MouseButtons.Middle, 0, mouseStruct.pt.x, mouseStruct.pt.y, 0);
                        MouseDown?.Invoke(this, args);
                        break;

                    case WM_LBUTTONUP:
                        args = new MouseEventArgs(MouseButtons.Left, 0, mouseStruct.pt.x, mouseStruct.pt.y, 0);
                        MouseUp?.Invoke(this, args);
                        break;

                    case WM_RBUTTONUP:
                        args = new MouseEventArgs(MouseButtons.Right, 0, mouseStruct.pt.x, mouseStruct.pt.y, 0);
                        MouseUp?.Invoke(this, args);
                        break;

                    case WM_MBUTTONUP:
                        args = new MouseEventArgs(MouseButtons.Middle, 0, mouseStruct.pt.x, mouseStruct.pt.y, 0);
                        MouseUp?.Invoke(this, args);
                        break;

                    case WM_MOUSEWHEEL:
                        args = new MouseEventArgs(MouseButtons.None, 0, mouseStruct.pt.x, mouseStruct.pt.y, mouseStruct.mouseData < 0 ? -1 : +1);
                        MouseWheel?.Invoke(this, args);
                        break;

                }
                return args != null && args.Block ? 1 : 0;
            }

            public delegate void MouseMoveEventHandler(object sender, MouseEventArgs e);
            public event MouseMoveEventHandler MouseMove;

            public delegate void MouseWheelEventHandler(object sender, MouseEventArgs e);
            public event MouseWheelEventHandler MouseWheel;

            public delegate void MouseDownEventHandler(object sender, MouseEventArgs e);
            public event MouseDownEventHandler MouseDown;

            public delegate void MouseUpEventHandler(object sender, MouseEventArgs e);
            public event MouseUpEventHandler MouseUp;





        }
    }
}
