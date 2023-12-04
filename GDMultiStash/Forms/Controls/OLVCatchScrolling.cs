using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using BrightIdeasSoftware;

namespace GDMultiStash.Forms.Controls
{
    [DesignerCategory("code")]
    internal class OLVCatchScrolling : ObjectListView
    {   // https://github.com/deveck/Deveck.Utils

        [StructLayout(LayoutKind.Sequential)]
        struct SCROLLINFO
        {
            public uint cbSize;
            public uint fMask;
            public int nMin;
            public int nMax;
            public uint nPage;
            public int nPos;
            public int nTrackPos;
        }

        private enum ScrollInfoMask
        {
            SIF_RANGE = 0x1,
            SIF_PAGE = 0x2,
            SIF_POS = 0x4,
            SIF_DISABLENOSCROLL = 0x8,
            SIF_TRACKPOS = 0x10,
            SIF_ALL = SIF_RANGE + SIF_PAGE + SIF_POS + SIF_TRACKPOS
        }

        //fnBar values
        private enum SBTYPES
        {
            SB_HORZ = 0,
            SB_VERT = 1,
            SB_CTL = 2,
            SB_BOTH = 3
        }

        const int GWL_STYLE = -16;
        const int WS_VSCROLL = 0x00200000;
        private const UInt32 WM_VSCROLL = 0x0115;
        private const UInt32 WM_NCCALCSIZE = 0x83;

        public class NCCalcSizeArgs : EventArgs
        {
            public bool vScroll { get; private set; }
            public NCCalcSizeArgs(bool vScroll)
            {
                this.vScroll = vScroll;
            }
        }
        public class VerticalScrollingEventArgs : EventArgs
        {
            public int Y { get; private set; }
            public VerticalScrollingEventArgs(int y)
            {
                this.Y = y;
            }
        }
        public EventHandler<NCCalcSizeArgs> NCCalcSize;
        public EventHandler<VerticalScrollingEventArgs> VerticalScrolling;

        [DllImport("user32.dll")][return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetScrollInfo(IntPtr hwnd, int fnBar, ref SCROLLINFO lpsi);

        [DllImport("user32.dll", EntryPoint = "GetWindowLong", CharSet = CharSet.Auto)]
        public static extern IntPtr GetWindowLong32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr", CharSet = CharSet.Auto)]
        public static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

        public static int GetWindowLong(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size == 4)
                return (int)GetWindowLong32(hWnd, nIndex);
            else
                return (int)(long)GetWindowLongPtr64(hWnd, nIndex);
        }


        // need to catch that call
        // because calling this method will NOT update the set position corretly
        public new void LowLevelScroll(int dx, int dy)
        {
            base.LowLevelScroll(dx, dy);
            VerticalScrolling?.Invoke(this, new VerticalScrollingEventArgs(dy));
        }


        public void GetScrollPosition(out int min, out int max, out int pos, out int page)
        {
            SCROLLINFO scrollinfo = new SCROLLINFO();
            scrollinfo.cbSize = (uint)Marshal.SizeOf(typeof(SCROLLINFO));
            scrollinfo.fMask = (int)ScrollInfoMask.SIF_ALL;
            if (GetScrollInfo(this.Handle, (int)SBTYPES.SB_VERT, ref scrollinfo))
            {
                min = scrollinfo.nMin;
                max = scrollinfo.nMax;
                pos = scrollinfo.nPos;
                page = (int)scrollinfo.nPage;
            }
            else
            {
                min = 0;
                max = 0;
                pos = 0;
                page = 0;
            }
        }


        private bool shown = false;

        private const UInt32 LVM_FIRST = 0x1000;
        private const UInt32 LVM_SCROLL = LVM_FIRST + 20;
        private const UInt32 WM_PAINT = 0xF;


        protected override void WndProc(ref Message m)
        {




            if (m.Msg == WM_VSCROLL)
            {
                //VerticalScrolling?.Invoke(this, EventArgs.Empty);
            }
            else if (m.Msg == LVM_SCROLL)
            {
                //VerticalScrolling?.Invoke(this, EventArgs.Empty);
            }
            else if (m.Msg == WM_NCCALCSIZE)
            {
                int style = (int)GetWindowLong(this.Handle, GWL_STYLE);
                shown = (style & WS_VSCROLL) == WS_VSCROLL;
                NCCalcSize?.Invoke(this, new NCCalcSizeArgs(shown));
            }
            else if (m.Msg == WM_PAINT)
            {
                NCCalcSize?.Invoke(this, new NCCalcSizeArgs(shown));
            }
            



            base.WndProc(ref m);

        }

    }
}
