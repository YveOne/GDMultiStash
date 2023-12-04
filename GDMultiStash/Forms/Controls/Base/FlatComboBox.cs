using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace GDMultiStash.Forms.Controls.Base
{
    [DesignerCategory("code")]
    public class FlatComboBox : ComboBox
    {   // https://stackoverflow.com/a/65976649
        // https://stackoverflow.com/a/13212571

        private bool _hover = false;

        public FlatComboBox()
        {
            DrawMode = DrawMode.OwnerDrawFixed;
            DrawItem += delegate (object sender, DrawItemEventArgs e)
            {
                if (e.Index < 0)
                    return;
                bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
                e.Graphics.FillRectangle(new SolidBrush(selected ? BackColorHighlight : BackColor), e.Bounds);
                e.Graphics.DrawString(Items[e.Index].ToString(), e.Font,
                    new SolidBrush(selected ? ForeColorHighlight : ForeColor),
                    new Point(e.Bounds.X, e.Bounds.Y));
            };
            MouseEnter += delegate { _hover = true; };
            MouseLeave += delegate { _hover = false; };
        }

        #region List Border

        private ListNativeWindow listControl = null;
        private Color m_ListBorderColor = Color.Transparent;

        [DefaultValue(typeof(Color), "Transparent")]
        public Color ListBorderColor
        {
            get
            {
                return m_ListBorderColor;
            }
            set
            {
                m_ListBorderColor = value;
                if (listControl != null)
                {
                    listControl.BorderColor = m_ListBorderColor;
                }
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            listControl = new ListNativeWindow(GetComboBoxListInternal(Handle));
            listControl.BorderColor = ListBorderColor;
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            listControl.ReleaseHandle();
            base.OnHandleDestroyed(e);
        }

        public partial class ListNativeWindow : NativeWindow
        {
            public ListNativeWindow() : this(IntPtr.Zero) { }

            public ListNativeWindow(IntPtr hWnd)
            {
                if (hWnd != IntPtr.Zero) AssignHandle(hWnd);
            }

            public Color BorderColor { get; set; } = Color.Transparent;

            protected override void WndProc(ref Message m)
            {
                base.WndProc(ref m);
                switch (m.Msg)
                {
                    case WM_NCPAINT:
                        {
                            var hDC = GetWindowDC(Handle);
                            try
                            {
                                using (var g = Graphics.FromHdc(hDC))
                                using (var pen = new Pen(BorderColor))
                                {
                                    var rect = g.VisibleClipBounds;
                                    g.DrawRectangle(pen, 0f, 0f, rect.Width - 1f, rect.Height - 1f);
                                }
                            }
                            finally
                            {
                                ReleaseDC(Handle, hDC);
                            }
                            m.Result = IntPtr.Zero;
                            break;
                        }
                }
            }
        }


        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern bool GetComboBoxInfo(IntPtr hWnd, ref COMBOBOXINFO pcbi);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDc);

        internal const int WM_NCPAINT = 0x85;

        [StructLayout(LayoutKind.Sequential)]
        internal partial struct COMBOBOXINFO
        {
            public int cbSize;
            public Rectangle rcItem;
            public Rectangle rcButton;
            public int buttonState;
            public IntPtr hwndCombo;
            public IntPtr hwndEdit;
            public IntPtr hwndList;
            public void Init()
            {
                cbSize = Marshal.SizeOf<COMBOBOXINFO>();
            }
        }

        internal IntPtr GetComboBoxListInternal(IntPtr cboHandle)
        {
            var cbInfo = new COMBOBOXINFO();
            cbInfo.Init();
            GetComboBoxInfo(cboHandle, ref cbInfo);
            return cbInfo.hwndList;
        }

        #endregion











        private Color borderColor = Color.Gray;
        [DefaultValue(typeof(Color), "Gray")]
        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                if (borderColor != value)
                {
                    borderColor = value;
                    Invalidate();
                }
            }
        }

        private Color foreColorHighlight = Color.Gray;
        [DefaultValue(typeof(Color), "Gray")]
        public Color ForeColorHighlight
        {
            get { return foreColorHighlight; }
            set
            {
                if (foreColorHighlight != value)
                {
                    foreColorHighlight = value;
                    Invalidate();
                }
            }
        }
        private Color backColorHighlight = Color.Gray;
        [DefaultValue(typeof(Color), "Gray")]
        public Color BackColorHighlight
        {
            get { return backColorHighlight; }
            set
            {
                if (backColorHighlight != value)
                {
                    backColorHighlight = value;
                    Invalidate();
                }
            }
        }

        private Color buttonColor = Color.LightGray;
        [DefaultValue(typeof(Color), "LightGray")]
        public Color ButtonColor
        {
            get { return buttonColor; }
            set
            {
                if (buttonColor != value)
                {
                    buttonColor = value;
                    Invalidate();
                }
            }
        }

        private Color buttonColorHighlight = Color.LightGray;
        [DefaultValue(typeof(Color), "LightGray")]
        public Color ButtonColorHighlight
        {
            get { return buttonColorHighlight; }
            set
            {
                if (buttonColorHighlight != value)
                {
                    buttonColorHighlight = value;
                    Invalidate();
                }
            }
        }

        




        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_PAINT && DropDownStyle != ComboBoxStyle.Simple)
            {
                var clientRect = ClientRectangle;
                var dropDownButtonWidth = SystemInformation.HorizontalScrollBarArrowWidth;
                var outerBorder = new Rectangle(clientRect.Location,
                    new Size(clientRect.Width - 1, clientRect.Height - 1));
                var innerBorder = new Rectangle(outerBorder.X + 1, outerBorder.Y + 1,
                    outerBorder.Width - dropDownButtonWidth - 2, outerBorder.Height - 2);
                var innerInnerBorder = new Rectangle(innerBorder.X + 1, innerBorder.Y + 1,
                    innerBorder.Width - 2, innerBorder.Height - 2);
                var dropDownRect = new Rectangle(innerBorder.Right + 1, innerBorder.Y,
                    dropDownButtonWidth, innerBorder.Height + 1);
                if (RightToLeft == RightToLeft.Yes)
                {
                    innerBorder.X = clientRect.Width - innerBorder.Right;
                    innerInnerBorder.X = clientRect.Width - innerInnerBorder.Right;
                    dropDownRect.X = clientRect.Width - dropDownRect.Right;
                    dropDownRect.Width += 1;
                }
                var innerBorderColor = Enabled ? BackColor : SystemColors.Control;
                var outerBorderColor = Enabled ? BorderColor : SystemColors.ControlDark;
                var buttonColor = Enabled ? (Focused || _hover ? ButtonColorHighlight : ButtonColor) : SystemColors.Control;
                var middle = new Point(dropDownRect.Left + dropDownRect.Width / 2,
                    dropDownRect.Top + dropDownRect.Height / 2);
                var arrow = new Point[]
                {
                new Point(middle.X - 3, middle.Y - 2),
                new Point(middle.X + 4, middle.Y - 2),
                new Point(middle.X, middle.Y + 2)
                };
                var ps = new PAINTSTRUCT();
                bool shoulEndPaint = false;
                IntPtr dc;
                if (m.WParam == IntPtr.Zero)
                {
                    dc = BeginPaint(Handle, ref ps);
                    m.WParam = dc;
                    shoulEndPaint = true;
                }
                else
                {
                    dc = m.WParam;
                }
                var rgn = CreateRectRgn(innerInnerBorder.Left, innerInnerBorder.Top,
                    innerInnerBorder.Right, innerInnerBorder.Bottom);
                SelectClipRgn(dc, rgn);
                DefWndProc(ref m);
                DeleteObject(rgn);
                rgn = CreateRectRgn(clientRect.Left, clientRect.Top,
                    clientRect.Right, clientRect.Bottom);
                SelectClipRgn(dc, rgn);
                using (var g = Graphics.FromHdc(dc))
                {
                    using (var b = new SolidBrush(buttonColor))
                    {
                        g.FillRectangle(b, dropDownRect);
                    }
                    using (var b = new SolidBrush(outerBorderColor))
                    {
                        g.FillPolygon(b, arrow);
                    }
                    using (var p = new Pen(innerBorderColor))
                    {
                        g.DrawRectangle(p, innerBorder);
                        g.DrawRectangle(p, innerInnerBorder);
                    }
                    using (var p = new Pen(outerBorderColor))
                    {
                        g.DrawRectangle(p, outerBorder);
                    }
                }
                if (shoulEndPaint)
                    EndPaint(Handle, ref ps);
                DeleteObject(rgn);
            }
            else
                base.WndProc(ref m);
        }

        private const int WM_PAINT = 0xF;
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int L, T, R, B;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct PAINTSTRUCT
        {
            public IntPtr hdc;
            public bool fErase;
            public int rcPaint_left;
            public int rcPaint_top;
            public int rcPaint_right;
            public int rcPaint_bottom;
            public bool fRestore;
            public bool fIncUpdate;
            public int reserved1;
            public int reserved2;
            public int reserved3;
            public int reserved4;
            public int reserved5;
            public int reserved6;
            public int reserved7;
            public int reserved8;
        }
        [DllImport("user32.dll")]
        private static extern IntPtr BeginPaint(IntPtr hWnd,
            [In, Out] ref PAINTSTRUCT lpPaint);

        [DllImport("user32.dll")]
        private static extern bool EndPaint(IntPtr hWnd, ref PAINTSTRUCT lpPaint);

        [DllImport("gdi32.dll")]
        public static extern int SelectClipRgn(IntPtr hDC, IntPtr hRgn);

        [DllImport("user32.dll")]
        public static extern int GetUpdateRgn(IntPtr hwnd, IntPtr hrgn, bool fErase);
        public enum RegionFlags
        {
            ERROR = 0,
            NULLREGION = 1,
            SIMPLEREGION = 2,
            COMPLEXREGION = 3,
        }
        [DllImport("gdi32.dll")]
        internal static extern bool DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateRectRgn(int x1, int y1, int x2, int y2);
    }
}
