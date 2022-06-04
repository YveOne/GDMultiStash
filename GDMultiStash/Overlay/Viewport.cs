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

            Utils.FontLoader.LoadFromResource(Properties.Resources.LinBiolinum_RB);

            MainWindow._BackgroundResource = Resources.CreateImageResource(
                    Properties.Resources.background,
                    System.Drawing.Imaging.ImageFormat.Png
                    );

            MainWindow._BackgroundLeftResource = Resources.CreateImageResource(
                    Properties.Resources.background_left,
                    System.Drawing.Imaging.ImageFormat.Png
                    );

            VerticalScrollBar._ScrollBarResource = Resources.CreateImageResource(
                    Properties.Resources.ScrollBarInner,
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

            StashList._ItemFont = Utils.FontLoader.GetFont("Linux Biolinum", 21, FontStyle.Bold);
            InfoWindow._TitleFont = Utils.FontLoader.GetFont("Linux Biolinum", 21, FontStyle.Bold);
            InfoWindow._TextFont = Utils.FontLoader.GetFont("Linux Biolinum", 19, FontStyle.Bold);
            InfoButton._Font = Utils.FontLoader.GetFont("Linux Biolinum", 15, FontStyle.Bold);










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
            Console.WriteLine(_mainWindow.State);
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
