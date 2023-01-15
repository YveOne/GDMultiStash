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
    internal class OLVGroupFeatures : OLVCatchScrolling
    {
        // https://stackoverflow.com/questions/32700669/c-sharp-change-color-of-groups-in-objectlistview


        #region Structs

        [StructLayout(LayoutKind.Sequential)]
        public struct NMHDR
        {
            public IntPtr hwndFrom;
            public IntPtr idFrom;
            public int code;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NMCUSTOMDRAW
        {
            public NMHDR hdr;
            public int dwDrawStage;
            public IntPtr hdc;
            public RECT rc;
            public IntPtr dwItemSpec;
            public uint uItemState;
            public IntPtr lItemlParam;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NMLVCUSTOMDRAW
        {
            public NMCUSTOMDRAW nmcd;
            public int clrText;
            public int clrTextBk;
            public int iSubItem;
            public int dwItemType;
            public int clrFace;
            public int iIconEffect;
            public int iIconPhase;
            public int iPartId;
            public int iStateId;
            public RECT rcText;
            public uint uAlign;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct LVGROUP
        {
            public uint cbSize;
            public uint mask;
            // <MarshalAs(UnmanagedType.LPTStr)>
            // Public pszHeader As String
            public IntPtr pszHeader;
            public int cchHeader;
            // <MarshalAs(UnmanagedType.LPTStr)>
            // Public pszFooter As String
            public IntPtr pszFooter;
            public int cchFooter;
            public int iGroupId;
            public uint stateMask;
            public uint state;
            public uint uAlign;

            // <MarshalAs(UnmanagedType.LPTStr)>
            // Public pszSubtitle As String
            public IntPtr pszSubtitle;
            public uint cchSubtitle;
            // <MarshalAs(UnmanagedType.LPTStr)>
            // Public pszTask As String
            public IntPtr pszTask;
            public uint cchTask;
            // <MarshalAs(UnmanagedType.LPTStr)>
            // Public pszDescriptionTop As String
            public IntPtr pszDescriptionTop;
            public uint cchDescriptionTop;
            // <MarshalAs(UnmanagedType.LPTStr)>
            // Public pszDescriptionBottom As String
            public IntPtr pszDescriptionBottom;
            public uint cchDescriptionBottom;
            public int iTitleImage;
            public int iExtendedImage;
            public int iFirstItem;
            public uint cItems;
            // <MarshalAs(UnmanagedType.LPTStr)>
            // Public pszSubsetTitle As String
            public IntPtr pszSubsetTitle;
            public uint cchSubsetTitle;
        }

        [Flags]
        public enum CDRF : int
        {
            CDRF_DODEFAULT = 0x0,
            CDRF_NEWFONT = 0x2,
            CDRF_SKIPDEFAULT = 0x4,
            CDRF_DOERASE = 0x8,
            CDRF_SKIPPOSTPAINT = 0x100,
            CDRF_NOTIFYPOSTPAINT = 0x10,
            CDRF_NOTIFYITEMDRAW = 0x20,
            CDRF_NOTIFYSUBITEMDRAW = 0x20,
            CDRF_NOTIFYPOSTERASE = 0x40
        }

        [Flags]
        public enum CDDS : int
        {
            CDDS_PREPAINT = 0x1,
            CDDS_POSTPAINT = 0x2,
            CDDS_PREERASE = 0x3,
            CDDS_POSTERASE = 0x4,
            CDDS_ITEM = 0x10000,
            CDDS_ITEMPREPAINT = (CDDS.CDDS_ITEM | CDDS.CDDS_PREPAINT),
            CDDS_ITEMPOSTPAINT = (CDDS.CDDS_ITEM | CDDS.CDDS_POSTPAINT),
            CDDS_ITEMPREERASE = (CDDS.CDDS_ITEM | CDDS.CDDS_PREERASE),
            CDDS_ITEMPOSTERASE = (CDDS.CDDS_ITEM | CDDS.CDDS_POSTERASE),
            CDDS_SUBITEM = 0x20000
        }

        #endregion

        #region Constants

        public const int LVCDI_ITEM = 0x0;
        public const int LVCDI_GROUP = 0x1;
        public const int LVCDI_ITEMSLIST = 0x2;

        public const int LVM_FIRST = 0x1000;
        public const int LVM_GETGROUPRECT = (LVM_FIRST + 98);
        public const int LVM_ENABLEGROUPVIEW = (LVM_FIRST + 157);
        public const int LVM_SETGROUPINFO = (LVM_FIRST + 147);
        public const int LVM_GETGROUPINFO = (LVM_FIRST + 149);
        public const int LVM_REMOVEGROUP = (LVM_FIRST + 150);
        public const int LVM_MOVEGROUP = (LVM_FIRST + 151);
        public const int LVM_GETGROUPCOUNT = (LVM_FIRST + 152);
        public const int LVM_GETGROUPINFOBYINDEX = (LVM_FIRST + 153);
        public const int LVM_MOVEITEMTOGROUP = (LVM_FIRST + 154);

        private const int WM_LBUTTONDOWN = 0x0201;
        private const int WM_LBUTTONUP = 0x0202;
        private const int WM_LBUTTONDBLCLK = 0x0203;

        private const int WM_RBUTTONDOWN = 0x0204;
        private const int WM_RBUTTONUP = 0x0205;

        public const int LVGF_NONE = 0x0;
        public const int LVGF_HEADER = 0x1;
        public const int LVGF_FOOTER = 0x2;
        public const int LVGF_STATE = 0x4;
        public const int LVGF_ALIGN = 0x8;
        public const int LVGF_GROUPID = 0x10;

        public const int LVGF_SUBTITLE = 0x100; // pszSubtitle is valid
        public const int LVGF_TASK = 0x200; // pszTask is valid
        public const int LVGF_DESCRIPTIONTOP = 0x400; // pszDescriptionTop is valid
        public const int LVGF_DESCRIPTIONBOTTOM = 0x800; // pszDescriptionBottom is valid
        public const int LVGF_TITLEIMAGE = 0x1000; // iTitleImage is valid
        public const int LVGF_EXTENDEDIMAGE = 0x2000; // iExtendedImage is valid
        public const int LVGF_ITEMS = 0x4000; // iFirstItem and cItems are valid
        public const int LVGF_SUBSET = 0x8000; // pszSubsetTitle is valid
        public const int LVGF_SUBSETITEMS = 0x10000; // readonly, cItems holds count of items in visible subset, iFirstItem is valid

        public const int LVGS_NORMAL = 0x0;
        public const int LVGS_COLLAPSED = 0x1;
        public const int LVGS_HIDDEN = 0x2;
        public const int LVGS_NOHEADER = 0x4;
        public const int LVGS_COLLAPSIBLE = 0x8;
        public const int LVGS_FOCUSED = 0x10;
        public const int LVGS_SELECTED = 0x20;
        public const int LVGS_SUBSETED = 0x40;
        public const int LVGS_SUBSETLINKFOCUSED = 0x80;

        public const int LVGA_HEADER_LEFT = 0x1;
        public const int LVGA_HEADER_CENTER = 0x2;
        public const int LVGA_HEADER_RIGHT = 0x4; // Don't forget to validate exclusivity
        public const int LVGA_FOOTER_LEFT = 0x8;
        public const int LVGA_FOOTER_CENTER = 0x10;
        public const int LVGA_FOOTER_RIGHT = 0x20; // Don't forget to validate exclusivity

        public const int LVGGR_GROUP = 0; // Entire expanded group
        public const int LVGGR_HEADER = 1;  // Header only (collapsed group)
        public const int LVGGR_LABEL = 2;  // Label only
        public const int LVGGR_SUBSETLINK = 3;  // subset link only

        #endregion

        #region Native

        [DllImport("User32.dll", EntryPoint = "SendMessageW", SetLastError = true)]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, ref IntPtr lParam);

        [DllImport("User32.dll", EntryPoint = "SendMessageW", SetLastError = true)]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, ref LVGROUP lParam);

        [DllImport("User32.dll", EntryPoint = "SendMessageW", SetLastError = true)]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, ref RECT lParam);

        #endregion

        public Color GroupHeadingBackColor { get; set; }

        public Color GroupHeadingForeColor { get; set; }

        public Color GroupHeadingCountForeColor { get; set; }

        public Font GroupHeadingFont { get; set; }

        public Font GroupHeadingCountFont { get; set; }

        public Color SeparatorColor { get; set; }








        public OLVGroupFeatures() : base()
        {
            GroupHeadingFont = this.Font;
            GroupHeadingCountFont = this.Font;
            GroupHeadingBackColor = Color.White;
            GroupHeadingForeColor = Color.Black;
            GroupHeadingCountForeColor = Color.Black;
            SeparatorColor = Color.Black;





            //LongClickTimer = new System.Timers.Timer(800);
            //LongClickTimer.AutoReset = false;
            //LongClickTimer.Elapsed += delegate {
            //  MessageBox.Show("left button long click");
            //};
        }


        /*
        public int SetGroupInfo(IntPtr hWnd, int nGroupID, uint nSate)
        {
            LVGROUP lvg = new LVGROUP();
            lvg.cbSize = System.Convert.ToUInt32(Marshal.SizeOf(lvg));
            lvg.mask = LVGF_STATE | LVGF_GROUPID | LVGF_HEADER;
            // for test
            int nRet2 = SendMessage(hWnd, LVM_GETGROUPINFO, nGroupID, ref lvg);

            lvg.state = nSate;
            lvg.mask = LVGF_STATE;
            nRet2 = SendMessage(hWnd, LVM_SETGROUPINFO, nGroupID, ref lvg);
            return -1;
        }
        */

        public EventHandler<GroupExpandingCollapsingEventArgs> GroupExpandingCollapsing2;


        public OLVGroup HitTestGroup(Point p)
        {
            foreach (OLVGroup grp in this.OLVGroups)
            {
                // get group header rect
                RECT rectHeader = new RECT();
                rectHeader.top = LVGGR_HEADER;
                SendMessage(Handle, LVM_GETGROUPRECT, grp.GroupId, ref rectHeader);
                Rectangle rect = new Rectangle(rectHeader.left, rectHeader.top, rectHeader.right - rectHeader.left, rectHeader.bottom - rectHeader.top);

                // and check if the clicked point is inside rect
                if (rect.Contains(p))
                {   // found the group header!!
                    return grp;
                }
            }
            return null;
        }

        //private DateTime leftPressStart = DateTime.Now;
        //private System.Timers.Timer LongClickTimer;

        // fixing bug: collapsing group activates cell edit mode
        // send dblclick message on btn up, instead of btn down
        private bool debugDblClick = false;


        protected override void WndProc(ref Message m)
        {









            Point clickedPoint = new Point(Native.LOWORD(m.LParam), Native.HIWORD(m.LParam));
            OLVGroup clickedGroup = null;

            switch(m.Msg)
            {
                case WM_LBUTTONDBLCLK:
                case WM_LBUTTONDOWN:
                case WM_LBUTTONUP:
                case WM_RBUTTONUP:
                    clickedGroup = HitTestGroup(clickedPoint);
                    break;
            }

            if (m.Msg == WM_LBUTTONDBLCLK)
            {
                if (clickedGroup != null)
                {
                    debugDblClick = true;
                    return;
                }
            }

            if (m.Msg == WM_LBUTTONDOWN)
            {
                if (clickedGroup != null)
                {
                    //LongClickTimer.Start();
                    //return;
                }
            }
            if (m.Msg == WM_LBUTTONUP)
            {
                if (clickedGroup != null)
                {
                    if (debugDblClick)
                    {
                        debugDblClick = false;
                        m.Msg = WM_LBUTTONDBLCLK;
                        base.WndProc(ref m);
                        GroupExpandingCollapsingEventArgs args = new GroupExpandingCollapsingEventArgs(clickedGroup);
                        GroupExpandingCollapsing2?.Invoke(this, args);
                        return;
                    }
                    //LongClickTimer.Stop();
                    //return;
                }
            }
            if (m.Msg == WM_RBUTTONUP)
            {
                if (clickedGroup != null)
                {
                    //MessageBox.Show("right button click");
                    //base.WndProc(ref m);
                    //return;
                }
            }

            



            base.WndProc(ref m);

            if (m.Msg == WM_REFLECT + WM_NOFITY)
            {
                var pnmhdr = (NMHDR)m.GetLParam(typeof(NMHDR));
                if (pnmhdr.code == NM_CUSTOMDRAW)
                {
                    var pnmlv = (NMLVCUSTOMDRAW)m.GetLParam(typeof(NMLVCUSTOMDRAW));
                    switch ((CDDS)pnmlv.nmcd.dwDrawStage)
                    {
                        case CDDS.CDDS_PREPAINT:
                            {
                                if ((pnmlv.dwItemType == LVCDI_GROUP))
                                {
                                    int groupId = (int)pnmlv.nmcd.dwItemSpec;
                                    OLVGroup olvGroup = OLVGroups.FirstOrDefault(grp => grp.GroupId == groupId);
                                    if (olvGroup == null) return;
                                    //Console.WriteLine($"---------------------- groupId: {groupId}");



                                    int nItem = (int)pnmlv.nmcd.dwItemSpec;
                                    // If (nItem = 0) Then




                                    RECT rectHeader = new RECT();
                                    rectHeader.top = LVGGR_HEADER;
                                    SendMessage(m.HWnd, LVM_GETGROUPRECT, nItem, ref rectHeader);



                                    using (Graphics g = Graphics.FromHdc(pnmlv.nmcd.hdc))
                                    {
                                        Rectangle rect = new Rectangle(rectHeader.left, rectHeader.top, rectHeader.right - rectHeader.left, rectHeader.bottom - rectHeader.top);

                                        // todo: create padding property for that
                                        //rect.Offset(2, 2);
                                        //rect.Width -= 4;
                                        //rect.Height -= 4;
                                        rect.Height -= 2;




                                        SolidBrush BgBrush = new SolidBrush(GroupHeadingBackColor);
                                        g.FillRectangle(BgBrush, rect);

                                        //string sText = olvGroup != null ? olvGroup.Header : "";
                                        string sText = olvGroup.Header;









                                        SizeF textSize = g.MeasureString(sText, GroupHeadingFont);

                                        int RectHeightMiddle = (int)((rect.Height - textSize.Height) * 0.5f);
                                        rect.Offset(5, RectHeightMiddle);
                                        rect.Width -= 5;

                                        using (SolidBrush drawBrush = new SolidBrush(GroupHeadingForeColor))
                                        {
                                            g.DrawString(sText, GroupHeadingFont, drawBrush, rect);
                                        }


                                        string countHintText = $"({olvGroup.Items.Count-1})"; // -1 because of the dummy
                                        SizeF countHintTextSize = g.MeasureString(countHintText, GroupHeadingCountFont);
                                        using (SolidBrush drawBrush = new SolidBrush(GroupHeadingCountForeColor))
                                        {
                                            g.DrawString(countHintText, GroupHeadingCountFont, drawBrush, new Rectangle() {
                                                X = rectHeader.right - 35 - (int)countHintTextSize.Width,
                                                Y = rect.Y + (int)((textSize.Height - countHintTextSize.Height)/2f),
                                                Width = rect.Width,
                                                Height = rect.Height,
                                            });
                                        }




                                        rect.Offset(0, -RectHeightMiddle);

                                        using (Pen linePen = new Pen(new SolidBrush(SeparatorColor), 2))
                                        {
                                            g.DrawLine(linePen,
                                                rect.X + g.MeasureString(sText, GroupHeadingFont).Width + 10,
                                                rect.Y + (int)(rect.Height * 0.5f) + 1,
                                                rect.X + rect.Width - 40 - (int)countHintTextSize.Width - 10,
                                                rect.Y + (int)(rect.Height * 0.5f) + 1
                                                );
                                        }




                                        using (Pen linePen = new Pen(new SolidBrush(GroupHeadingForeColor), 2))
                                        {
                                            if (olvGroup != null)
                                            {
                                                if (olvGroup.Collapsed)
                                                {
                                                    g.DrawLine(linePen,
                                                        rect.X + rect.Width - 20,
                                                        rect.Y + 7,
                                                        rect.X + rect.Width - 15,
                                                        rect.Y + 12
                                                        );
                                                    g.DrawLine(linePen,
                                                        rect.X + rect.Width - 15 - 1,
                                                        rect.Y + 12,
                                                        rect.X + rect.Width - 10 - 1,
                                                        rect.Y + 7
                                                        );
                                                }
                                                else
                                                {
                                                    g.DrawLine(linePen,
                                                        rect.X + rect.Width - 20,
                                                        rect.Y + 12,
                                                        rect.X + rect.Width - 15,
                                                        rect.Y + 7
                                                        );
                                                    g.DrawLine(linePen,
                                                        rect.X + rect.Width - 15 - 1,
                                                        rect.Y + 7,
                                                        rect.X + rect.Width - 10 - 1,
                                                        rect.Y + 12
                                                        );
                                                }
                                            }
                                        }
                                    }
                                    m.Result = new IntPtr((int)CDRF.CDRF_SKIPDEFAULT);
                                }
                                break;
                            }


                    }
                }
                return;
            }
        }
        
        private const int NM_FIRST = 0;
        private const int NM_CLICK = NM_FIRST - 2;
        private const int NM_CUSTOMDRAW = NM_FIRST - 12;
        private const int WM_REFLECT = 0x2000;
        private const int WM_NOFITY = 0x4E;



    }
}
