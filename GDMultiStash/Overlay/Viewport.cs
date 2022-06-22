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

        private readonly MainWindow _mainWindow;

        public Viewport()
        {

            Utils.FontLoader.LoadFromResource(Properties.Resources.font_LinBiolinum_R);
            Utils.FontLoader.LoadFromResource(Properties.Resources.font_LinBiolinum_RB);
            Utils.FontLoader.LoadFromResource(Properties.Resources.font_LinBiolinum_RI);

            MainWindow._BackgroundResource = Resources.CreateImageResource(
                    Properties.Resources.background,
                    System.Drawing.Imaging.ImageFormat.Png
                    );

            MainWindow._BackgroundLeftResource = Resources.CreateImageResource(
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

            InfoButton._UpResource = Resources.CreateImageResource(
                    Properties.Resources.ButtonSmallUp,
                    System.Drawing.Imaging.ImageFormat.Png
                    );

            InfoButton._DownResource = Resources.CreateImageResource(
                    Properties.Resources.ButtonSmallDown,
                    System.Drawing.Imaging.ImageFormat.Png
                    );

            InfoButton._OverResource = Resources.CreateImageResource(
                    Properties.Resources.ButtonSmallOver,
                    System.Drawing.Imaging.ImageFormat.Png
                    );

            //StashList._ItemFont = Utils.FontLoader.GetFont("Linux Biolinum", 23, FontStyle.Regular);
            InfoWindow._TitleFont = Utils.FontLoader.GetFont("Linux Biolinum", 21, FontStyle.Regular);
            InfoWindow._TextFont = Utils.FontLoader.GetFont("Linux Biolinum", 19, FontStyle.Regular);
            InfoButton._Font = Utils.FontLoader.GetFont("Linux Biolinum", 15, FontStyle.Regular);










            _mainWindow = new MainWindow
            {
                Scale = 1f,
            };
            AddChild(_mainWindow);
            Reset();





            _mainWindow.StateChanged += delegate (MainWindow.States state)
            {
                switch(state)
                {
                    case MainWindow.States.Showing:
                    case MainWindow.States.Hidden:
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
            if (_mainWindow.State == MainWindow.States.Hidden)
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
