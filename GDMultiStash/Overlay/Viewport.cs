using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GDMultiStash.Overlay.Elements
{
    public class Viewport : Common.Overlay.Viewport
    {

        private readonly OverlayWindow _mainWindow;

        public Viewport()
        {

            Utils.FontLoader.LoadFromResource(Properties.Resources.font_LinBiolinum_R);
            Utils.FontLoader.LoadFromResource(Properties.Resources.font_LinBiolinum_RB);
            Utils.FontLoader.LoadFromResource(Properties.Resources.font_LinBiolinum_RI);

            OverlayWindow._BackgroundResource = Resources.CreateImageResource(
                    Properties.Resources.background,
                    System.Drawing.Imaging.ImageFormat.Png
                    );

            OverlayWindow._BackgroundLeftResource = Resources.CreateImageResource(
                    Properties.Resources.background_left,
                    System.Drawing.Imaging.ImageFormat.Png
                    );

            VerticalScrollBar._ScrollBarResource = Resources.CreateImageResource(
                    Properties.Resources.scrollbar,
                    System.Drawing.Imaging.ImageFormat.Png
                    );

            VerticalScrollBar._ScrollBarTopResource = Resources.CreateImageResource(
                    Properties.Resources.scrollbar_top,
                    System.Drawing.Imaging.ImageFormat.Png
                    );

            VerticalScrollBar._ScrollBarBottomResource = Resources.CreateImageResource(
                    Properties.Resources.scrollbar_bottom,
                    System.Drawing.Imaging.ImageFormat.Png
                    );

            StashListChild._Radio0Resource = Resources.CreateImageResource(
                    Properties.Resources.radio0,
                    System.Drawing.Imaging.ImageFormat.Png
                    );

            StashListChild._Radio1Resource = Resources.CreateImageResource(
                    Properties.Resources.radio1,
                    System.Drawing.Imaging.ImageFormat.Png
                    );

            InfoBoxButton._UpResource = Resources.CreateImageResource(
                    Properties.Resources.ButtonSmallUp,
                    System.Drawing.Imaging.ImageFormat.Png
                    );

            InfoBoxButton._DownResource = Resources.CreateImageResource(
                    Properties.Resources.ButtonSmallDown,
                    System.Drawing.Imaging.ImageFormat.Png
                    );

            InfoBoxButton._OverResource = Resources.CreateImageResource(
                    Properties.Resources.ButtonSmallOver,
                    System.Drawing.Imaging.ImageFormat.Png
                    );

            //StashList._ItemFont = Utils.FontLoader.GetFont("Linux Biolinum", 23, FontStyle.Regular);
            InfoBox._TitleFont = Utils.FontLoader.GetFont("Linux Biolinum", 21, FontStyle.Regular);
            InfoBox._TextFont = Utils.FontLoader.GetFont("Linux Biolinum", 19, FontStyle.Regular);
            InfoBoxButton._Font = Utils.FontLoader.GetFont("Linux Biolinum", 15, FontStyle.Regular);










            _mainWindow = new OverlayWindow
            {
                Scale = 1f,
            };
            AddChild(_mainWindow);
            Reset();





            _mainWindow.StateChanged += delegate (OverlayWindow.States state)
            {
                switch(state)
                {
                    case OverlayWindow.States.Showing:
                    case OverlayWindow.States.Hidden:
                        Update();
                        break;
                }
            };




            Core.Runtime.WindowSizeChanged += delegate {
                Size s = Core.Runtime.WindowSize;
                Width = s.Width;
                Height = s.Height;
            };

        }




        public override List<D3DHook.Hook.Common.IOverlayElement> GetImagesRecursive()
        {
            if (_mainWindow.State == OverlayWindow.States.Hidden)
            {
                Redraw();
                return null;
            }
            return base.GetImagesRecursive();
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
