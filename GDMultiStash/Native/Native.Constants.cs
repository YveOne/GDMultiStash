using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static partial class Native
{
    /// <summary>
    /// Window Styles
    /// </summary>
    public static class WS
    {
        public const int MINIMIZEBOX = 0x00020000;
        public const int MAXIMIZEBOX = 0x00010000;
        public const int HSCROLL = 0x00100000;
        public const int VSCROLL = 0x00200000;
    }
    public static class EVENT
    {
        public const int OBJECT_DESTROY = 0x8001;
        public const int SYSTEM_MOVESIZESTART = 0x000A;
        public const int SYSTEM_MOVESIZEEND = 0x000B;
        public const int OBJECT_LOCATIONCHANGE = 0x800B;
        public const int SYSTEM_FOREGROUND = 0x0003;
    }
    public static class WINEVENT
    {
        public const int OUTOFCONTEXT = 0x0000; // Events are ASYNC
        public const int SKIPOWNTHREAD = 0x0001; // Don't call back for events on installer's thread
        public const int SKIPOWNPROCESS = 0x0002; // Don't call back for events on installer's process
        public const int INCONTEXT = 0x0004; // Events are SYNC, this causes your dll to be injected into every process
    }
    /// <summary>
    /// Hit Test Values
    /// </summary>
    public static class HT
    {
        public const int CAPTION = 2;
        public const int LEFT = 10;
        public const int RIGHT = 11;
        public const int TOP = 12;
        public const int TOPLEFT = 13;
        public const int TOPRIGHT = 14;
        public const int BOTTOM = 15;
        public const int BOTTOMLEFT = 16;
        public const int BOTTOMRIGHT = 17;
    }
    public static class HWND
    {
        public const int BROADCAST = 0xffff;
    }





    public const uint ATTACH_PARRENT = 0xFFFFFFFF;




    // offset of window style value
    public const int GWL_STYLE = -16;

    public const int CS_DBLCLKS = 0x8;



    public const int WH_MOUSE_LL = 14;

    public const int WM_MOUSEMOVE = 0x200;
    public const int WM_LBUTTONDOWN = 0x201;
    public const int WM_RBUTTONDOWN = 0x204;
    public const int WM_MBUTTONDOWN = 0x207;
    public const int WM_LBUTTONUP = 0x202;
    public const int WM_RBUTTONUP = 0x205;
    public const int WM_MBUTTONUP = 0x208;
    public const int WM_LBUTTONDBLCLK = 0x203;
    public const int WM_RBUTTONDBLCLK = 0x206;
    public const int WM_MBUTTONDBLCLK = 0x209;
    public const int WM_MOUSEWHEEL = 0x020A;
    public const int WM_NCLBUTTONDBLCLK = 0x00A3;



}

