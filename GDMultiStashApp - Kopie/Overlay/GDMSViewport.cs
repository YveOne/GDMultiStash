using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

using GDMultiStash.Common.Overlay;
using GDMultiStash.Common.Overlay.Animations;

using GDMultiStash.Properties;

namespace GDMultiStash.Overlay
{
    internal class GDMSViewport : Viewport
    {

        private readonly ImageElement _startUpLogo;
        private readonly OverlayWindow _mainWindow;

        public GDMSViewport()
        {
            MouseCheckNeedBaseHit = true;
            Debugging = false;

            Utils.FontLoader.LoadFromResource(Resources.font_LinBiolinum_R);
            Utils.FontLoader.LoadFromResource(Resources.font_LinBiolinum_RB);
            Utils.FontLoader.LoadFromResource(Resources.font_LinBiolinum_RI);

            StaticResources.InfoBoxTitleFont = Utils.FontLoader.GetFont("Linux Biolinum", 21, FontStyle.Regular);
            StaticResources.InfoBoxTextFont = Utils.FontLoader.GetFont("Linux Biolinum", 19, FontStyle.Regular);
            StaticResources.SmallButtonFont = Utils.FontLoader.GetFont("Linux Biolinum", 15, FontStyle.Regular);
            StaticResources.LargeButtonFont = Utils.FontLoader.GetFont("Linux Biolinum", 20, FontStyle.Regular);
            StaticResources.GroupListItemFont = Utils.FontLoader.GetFont("Linux Biolinum", 22, FontStyle.Regular);
            StaticResources.StashListItemFont = Utils.FontLoader.GetFont("Linux Biolinum", 22, FontStyle.Regular);

            StaticResources.GDMSLogo = OverlayResources.CreateImageResource(Resources.GDMSLogo, ImageFormat.Png);

            StaticResources.ButtonRoundUp = OverlayResources.CreateImageResource(Resources.ButtonRoundUp, ImageFormat.Png);
            StaticResources.ButtonRoundOver = OverlayResources.CreateImageResource(Resources.ButtonRoundOver, ImageFormat.Png);
            StaticResources.ButtonRoundDown = OverlayResources.CreateImageResource(Resources.ButtonRoundDown, ImageFormat.Png);
            StaticResources.ButtonRoundDownOver = OverlayResources.CreateImageResource(Resources.ButtonRoundDownOver, ImageFormat.Png);

            StaticResources.LockWhiteIcon = OverlayResources.CreateImageResource(Resources.LockWhiteIcon, ImageFormat.Png);
            StaticResources.HomeButtonIcon = OverlayResources.CreateImageResource(Resources.HomeButtonIcon, ImageFormat.Png);
            StaticResources.WindowBackground = OverlayResources.CreateImageResource(Resources.WindowBackground, ImageFormat.Jpeg);
            StaticResources.WindowOverShadow = OverlayResources.CreateColorImageResource(Color.FromArgb(0, 0, 0));
            StaticResources.GroupListBackground = OverlayResources.CreateColorImageResource(Color.FromArgb(28, 25, 22));

            StaticResources.WindowBorderTop = OverlayResources.CreateImageResource(Resources.WindowBorderTop, ImageFormat.Png);
            StaticResources.WindowBorderTopRight = OverlayResources.CreateImageResource(Resources.WindowBorderTopRight, ImageFormat.Png);
            StaticResources.WindowBorderBottom = OverlayResources.CreateImageResource(Resources.WindowBorderBottom, ImageFormat.Png);
            StaticResources.WindowBorderBottomRight = OverlayResources.CreateImageResource(Resources.WindowBorderBottomRight, ImageFormat.Png);
            StaticResources.WindowBorderRight = OverlayResources.CreateImageResource(Resources.WindowBorderRight, ImageFormat.Png);

            StaticResources.ShadowTopLeft = OverlayResources.CreateImageResource(Resources.ShadowTopLeft, ImageFormat.Png);
            StaticResources.ShadowTop = OverlayResources.CreateImageResource(Resources.ShadowTop, ImageFormat.Png);
            StaticResources.ShadowTopRight = OverlayResources.CreateImageResource(Resources.ShadowTopRight, ImageFormat.Png);
            StaticResources.ShadowLeft = OverlayResources.CreateImageResource(Resources.ShadowLeft, ImageFormat.Png);
            StaticResources.ShadowMiddle = OverlayResources.CreateImageResource(Resources.ShadowMiddle, ImageFormat.Png);
            StaticResources.ShadowRight = OverlayResources.CreateImageResource(Resources.ShadowRight, ImageFormat.Png);
            StaticResources.ShadowBottomLeft = OverlayResources.CreateImageResource(Resources.ShadowBottomLeft, ImageFormat.Png);
            StaticResources.ShadowBottom = OverlayResources.CreateImageResource(Resources.ShadowBottom, ImageFormat.Png);
            StaticResources.ShadowBottomRight = OverlayResources.CreateImageResource(Resources.ShadowBottomRight, ImageFormat.Png);

            StaticResources.StashListBorderTL = OverlayResources.CreateImageResource(Resources.StashListBorderTL, ImageFormat.Png);
            StaticResources.StashListBorderT = OverlayResources.CreateImageResource(Resources.StashListBorderT, ImageFormat.Png);
            StaticResources.StashListBorderTR = OverlayResources.CreateImageResource(Resources.StashListBorderTR, ImageFormat.Png);
            StaticResources.StashListBorderL = OverlayResources.CreateImageResource(Resources.StashListBorderL, ImageFormat.Png);
            StaticResources.StashListBackground = OverlayResources.CreateImageResource(Resources.StashListBackground, ImageFormat.Jpeg);
            StaticResources.StashListBorderR = OverlayResources.CreateImageResource(Resources.StashListBorderR, ImageFormat.Png);
            StaticResources.StashListBorderBL = OverlayResources.CreateImageResource(Resources.StashListBorderBL, ImageFormat.Png);
            StaticResources.StashListBorderB = OverlayResources.CreateImageResource(Resources.StashListBorderB, ImageFormat.Png);
            StaticResources.StashListBorderBR = OverlayResources.CreateImageResource(Resources.StashListBorderBR, ImageFormat.Png);

            StaticResources.ScrollAreaTop = OverlayResources.CreateImageResource(Resources.ScrollAreaTop, ImageFormat.Png);
            StaticResources.ScrollAreaMiddle = OverlayResources.CreateImageResource(Resources.ScrollAreaMiddle, ImageFormat.Png);
            StaticResources.ScrollAreaBottom = OverlayResources.CreateImageResource(Resources.ScrollAreaBottom, ImageFormat.Png);

            StaticResources.ButtonLargeUp = OverlayResources.CreateImageResource(Resources.ButtonLargeUp, ImageFormat.Png);
            StaticResources.ButtonLargeOver = OverlayResources.CreateImageResource(Resources.ButtonLargeOver, ImageFormat.Png);
            StaticResources.ButtonLargeDown = OverlayResources.CreateImageResource(Resources.ButtonLargeDown, ImageFormat.Png);

            StaticResources.ButtonSmallUp = OverlayResources.CreateImageResource(Resources.ButtonSmallUp, ImageFormat.Png);
            StaticResources.ButtonSmallOver = OverlayResources.CreateImageResource(Resources.ButtonSmallOver, ImageFormat.Png);
            StaticResources.ButtonSmallDown = OverlayResources.CreateImageResource(Resources.ButtonSmallDown, ImageFormat.Png);

            StaticResources.ButtonDropDownArrow = OverlayResources.CreateImageResource(Resources.ButtonDropDownArrow, ImageFormat.Png);
            StaticResources.ButtonDropUpArrow = OverlayResources.CreateImageResource(Resources.ButtonDropUpArrow, ImageFormat.Png);

            StaticResources.ScrollBar = OverlayResources.CreateImageResource(Resources.ScrollBar, ImageFormat.Png);
            StaticResources.ScrollBarTop = OverlayResources.CreateImageResource(Resources.ScrollBarTop, ImageFormat.Png);
            StaticResources.ScrollBarBottom = OverlayResources.CreateImageResource(Resources.ScrollBarBottom, ImageFormat.Png);

            StaticResources.ReloadButtonSmallUp = OverlayResources.CreateImageResource(Resources.ReloadButtonSmallUp, ImageFormat.Png);
            StaticResources.ReloadButtonSmallDown = OverlayResources.CreateImageResource(Resources.ReloadButtonSmallDown, ImageFormat.Png);
            StaticResources.ReloadButtonSmallOver = OverlayResources.CreateImageResource(Resources.ReloadButtonSmallOver, ImageFormat.Png);
            


            _mainWindow = new OverlayWindow();
            AddChild(_mainWindow);

            _startUpLogo = new ImageElement()
            {
                AnchorPoint = Anchor.TopLeft,
                X = 10,
                Y = 20,
                Width = 115,
                Height = 40,
                Alpha = 1,
                Visible = true,
                Resource = StaticResources.GDMSLogo,
            };
            AddChild(_startUpLogo);
            {
                AnimationValue logoFading = new AnimationValue()
                {
                    Easer = Easing.ExpOut(),
                    Min = 1f,
                    Max = 0f,
                };
                Animator animator = Animator.Create(2000);
                animator.Delay = 2000;
                animator.AnimationAnimate += delegate (object sender, AnimatorAnimateEventArgs args) {
                    _startUpLogo.Alpha = logoFading.Convert(args.Value);
                };
                animator.AnimationEnd += delegate {
                    _startUpLogo.Visible = false;
                    _startUpLogo.Resource = null;
                    OverlayResources.DeleteResource(StaticResources.GDMSLogo);
                };
                animator.Value = 1;
            }







            _mainWindow.StateChanged += delegate (OverlayWindow.States state)
            {
                switch(state)
                {
                    case OverlayWindow.States.Showing:
                    case OverlayWindow.States.Hidden:
                        Redraw(true);
                        break;
                }
            };

            Global.Ingame.GameWindowSizeChanged += delegate {
                Size s = Global.Ingame.GameWindowSize;
                Width = s.Width;
                Height = s.Height;
            };

        }

        public override List<D3DHook.Hook.Common.IOverlayElement> GetImagesRecursive()
        {
            List<D3DHook.Hook.Common.IOverlayElement> l = new List<D3DHook.Hook.Common.IOverlayElement>();
            l.AddRange(_startUpLogo.GetImagesRecursive());
            if (_mainWindow.State != OverlayWindow.States.Hidden)
                l.AddRange(base.GetImagesRecursive());
            return l;
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
