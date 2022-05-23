using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GDMultiStash.Overlay
{
    internal class OverlayManager
    {

        private readonly Elements.Viewport _viewport;
        private readonly Elements.MainWindow _mainWindow;

        private static Font _titleFont;

        public Elements.Viewport Viewport => _viewport;

        public OverlayManager()
        {

            Utils.FontLoader.LoadFromResource(Properties.Resources.LinBiolinum_RB);
            _titleFont = Utils.FontLoader.GetFont("Linux Biolinum", 25, FontStyle.Bold);

            _viewport = new Elements.Viewport();

            Elements.MainWindow._BackgroundResource = Viewport.Resources.CreateImageResource(
                    Properties.Resources.background,
                    System.Drawing.Imaging.ImageFormat.Png
                    );

            Elements.VerticalScrollBar._ScrollBarResource = Viewport.Resources.CreateImageResource(
                    Properties.Resources.ScrollBarInner,
                    System.Drawing.Imaging.ImageFormat.Png
                    );

            Elements.StashListChild._Radio0Resource = Viewport.Resources.CreateImageResource(
                    Properties.Resources.radio0,
                    System.Drawing.Imaging.ImageFormat.Png
                    );

            Elements.StashListChild._Radio1Resource = Viewport.Resources.CreateImageResource(
                    Properties.Resources.radio1,
                    System.Drawing.Imaging.ImageFormat.Png
                    );

            Elements.InfoButton._UpResource = Viewport.Resources.CreateImageResource(
                    Properties.Resources.ButtonSmallUp,
                    System.Drawing.Imaging.ImageFormat.Png
                    );

            Elements.InfoButton._DownResource = Viewport.Resources.CreateImageResource(
                    Properties.Resources.ButtonSmallDown,
                    System.Drawing.Imaging.ImageFormat.Png
                    );

            Elements.InfoButton._OverResource = Viewport.Resources.CreateImageResource(
                    Properties.Resources.ButtonSmallOver,
                    System.Drawing.Imaging.ImageFormat.Png
                    );

            Elements.StashList._ItemFont = Utils.FontLoader.GetFont("Linux Biolinum", 21, FontStyle.Bold);
            Elements.InfoWindow._TitleFont = Utils.FontLoader.GetFont("Linux Biolinum", 21, FontStyle.Bold);
            Elements.InfoWindow._TextFont = Utils.FontLoader.GetFont("Linux Biolinum", 19, FontStyle.Bold);
            Elements.InfoButton._Font = Utils.FontLoader.GetFont("Linux Biolinum", 15, FontStyle.Bold);










            _mainWindow = new Elements.MainWindow
            {
                Scale = 1f,
            };
            _viewport.AddChild(_mainWindow);
            _viewport.Reset();




            Core.Runtime.WindowSizeChanged += delegate {
                Size s = Core.Runtime.WindowSize;
                _viewport.Width = s.Width;
                _viewport.Height = s.Height;
            };

        }








        public void Destroy()
        {
            _viewport.Destroy();
        }

        public void ShowMainWindow()
        {
            _mainWindow.Show();
        }

        public void HideMainWindow()
        {
            _mainWindow.Hide();
        }





    }
}
