using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static partial class Native
{

    public abstract class Hook
    {

        private int hHook = -1;

        protected HookProc hProc { get; private set; }

        public Hook()
        {
            hProc = new HookProc(BaseHookProc);
        }

        public abstract int SetHook();

        protected int SetHook(WH idHook, IntPtr instance, uint threadId = 0)
        {
            if (hHook != -1) return hHook;
            hHook = SetWindowsHookEx((int)idHook, hProc, instance, threadId);
            return hHook;
        }

        public virtual void UnHook()
        {
            if (hHook == -1) return;
            UnhookWindowsHookEx(hHook);
            hHook = -1;
        }

        protected abstract int HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        private int BaseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
                if (HookProc(nCode, wParam, lParam) == 1)
                    return 0;
            return CallNextHookEx(hHook, nCode, wParam, lParam);
        }

    }


}
