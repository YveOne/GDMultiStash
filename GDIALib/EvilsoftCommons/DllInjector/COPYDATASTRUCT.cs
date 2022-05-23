using System;

namespace GDIALib.EvilsoftCommons.DllInjector
{
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct COPYDATASTRUCT
    {
        public int cbData;
        public IntPtr dwData;
        public IntPtr lpData;
    }
}
