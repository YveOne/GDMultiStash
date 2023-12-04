using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace GDMultiStash.Forms
{
    internal partial class SplashForm : BaseForm
    {

        private float _opacity = 1.0f;
        public Bitmap BackgroundBitmap;

        public new float Opacity
        {
            get
            {
                return _opacity;
            }
            set
            {
                _opacity = value;
                SelectBitmap(BackgroundBitmap);
            }
        }

        public SplashForm()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;

            StartPosition = FormStartPosition.CenterScreen;
            TopMost = true;
        }

        private void SplashForm_Load(object sender, EventArgs e)
        {
            this.BackgroundBitmap = Properties.Resources.GDMSSplash;
            this.SelectBitmap(BackgroundBitmap);
            this.BackColor = Color.Red;
        }

        protected override void Localize(Global.LocalizationManager.StringsHolder L)
        {
        }

        // Sets the current bitmap
        public void SelectBitmap(Bitmap bitmap)
        {
            // Does this bitmap contain an alpha channel?   
            if (bitmap.PixelFormat != PixelFormat.Format32bppArgb)
            {
                throw new ApplicationException("The bitmap must be 32bpp with alpha-channel.");
            }
            // Get device contexts   
            IntPtr screenDc = APIHelp.GetDC(IntPtr.Zero);
            IntPtr memDc = APIHelp.CreateCompatibleDC(screenDc);
            IntPtr hBitmap = IntPtr.Zero;
            IntPtr hOldBitmap = IntPtr.Zero;
            try
            {
                // Get handle to the new bitmap and select it into the current device context      
                hBitmap = bitmap.GetHbitmap(Color.FromArgb(0));
                hOldBitmap = APIHelp.SelectObject(memDc, hBitmap);
                // Set parameters for layered window update      
                APIHelp.Size newSize = new APIHelp.Size(bitmap.Width, bitmap.Height);
                // Size window to match bitmap      
                APIHelp.Point sourceLocation = new APIHelp.Point(0, 0);
                APIHelp.Point newLocation = new APIHelp.Point(this.Left, this.Top);
                // Same as this window      
                APIHelp.BLENDFUNCTION blend = new APIHelp.BLENDFUNCTION();
                blend.BlendOp = APIHelp.AC_SRC_OVER;
                // Only works with a 32bpp bitmap      
                blend.BlendFlags = 0; // Always 0   
                blend.SourceConstantAlpha = (byte)(Opacity * 255); // Set to 255 for per-pixel alpha values
                blend.AlphaFormat = APIHelp.AC_SRC_ALPHA;
                // Only works when the bitmap contains an alpha channel      
                // Update the window      
                APIHelp.UpdateLayeredWindow(Handle, screenDc, ref newLocation, ref newSize, memDc, ref sourceLocation, 0, ref blend, APIHelp.ULW_ALPHA);
            }
            finally
            {
                // Release device context      
                APIHelp.ReleaseDC(IntPtr.Zero, screenDc);
                if (hBitmap != IntPtr.Zero)
                {
                    APIHelp.SelectObject(memDc, hOldBitmap);
                    APIHelp.DeleteObject(hBitmap);
                    // Remove bitmap resources      
                }
                APIHelp.DeleteDC(memDc);
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {     
                CreateParams createParams = base.CreateParams;
                createParams.ExStyle |= APIHelp.WS_EX_LAYERED;
                createParams.ExStyle |= APIHelp.WS_EX_TRANSPARENT;
                return createParams;
            }
        }


    }

    // Class to assist with Win32 API calls
    // source: https://stackoverflow.com/questions/22979232/how-to-design-a-cool-semi-transparent-splash-screen
    class APIHelp
    {
        public const Int32 WS_EX_LAYERED = 0x80000;
        public const Int32 WS_EX_TRANSPARENT = 0x20;
        public const Int32 HTCAPTION = 0x02;
        public const Int32 WM_NCHITTEST = 0x84;
        public const Int32 ULW_ALPHA = 0x02;
        public const byte AC_SRC_OVER = 0x00;
        public const byte AC_SRC_ALPHA = 0x01;

        public enum Bool
        {
            False = 0, True = 1
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Point
        {
            public Int32 x;
            public Int32 y;
            public Point(Int32 x, Int32 y)
            {
                this.x = x;
                this.y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Size
        {
            public Int32 cx;
            public Int32 cy;
            public Size(Int32 cx, Int32 cy)
            {
                this.cx = cx;
                this.cy = cy;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct ARGB
        {
            public byte Blue;
            public byte Green;
            public byte Red;
            public byte Alpha;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BLENDFUNCTION
        {
            public byte BlendOp;
            public byte BlendFlags;
            public byte SourceConstantAlpha;
            public byte AlphaFormat;
        }

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern Bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref Point pptDst, ref Size psize, IntPtr hdcSrc, ref Point pprSrc, Int32 crKey, ref BLENDFUNCTION pblend, Int32 dwFlags);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll", ExactSpelling = true)]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern Bool DeleteDC(IntPtr hdc);

        [DllImport("gdi32.dll", ExactSpelling = true)]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern Bool DeleteObject(IntPtr hObject);
    }

}
