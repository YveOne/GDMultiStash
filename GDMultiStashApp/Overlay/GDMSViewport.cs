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
    internal class GDMSViewport : Common.Overlay.Viewport
    {

        private readonly ImageElement _startUpLogo;
        private readonly OverlayWindow _mainWindow;

        public GDMSViewport()
        {
            MouseCheckNeedBaseHit = true;
            Debugging = false;







            //todo: what about resources IDisposable?




            Panels.BackgroundBorderPanel._T = OverlayResources.CreateImageResource(Resources.windowBorderTop, ImageFormat.Png);
            Panels.BackgroundBorderPanel._TR = OverlayResources.CreateImageResource(Resources.windowBorderTopRight, ImageFormat.Png);
            Panels.BackgroundBorderPanel._B = OverlayResources.CreateImageResource(Resources.windowBorderBottom, ImageFormat.Png);
            Panels.BackgroundBorderPanel._BR = OverlayResources.CreateImageResource(Resources.windowBorderBottomRight, ImageFormat.Png);
            Panels.BackgroundBorderPanel._R = OverlayResources.CreateImageResource( Resources.windowBorderRight, ImageFormat.Png);

            Panels.BackgroundShadowPanel._TL = OverlayResources.CreateImageResource(Resources.shadowTopLeft, ImageFormat.Png);
            Panels.BackgroundShadowPanel._T = OverlayResources.CreateImageResource(Resources.shadowTop, ImageFormat.Png);
            Panels.BackgroundShadowPanel._TR = OverlayResources.CreateImageResource(Resources.shadowTopRight, ImageFormat.Png);
            Panels.BackgroundShadowPanel._L = OverlayResources.CreateImageResource(Resources.shadowLeft, ImageFormat.Png);
            Panels.BackgroundShadowPanel._M = OverlayResources.CreateImageResource(Resources.shadowMiddle, ImageFormat.Png);
            Panels.BackgroundShadowPanel._R = OverlayResources.CreateImageResource(Resources.shadowRight, ImageFormat.Png);
            Panels.BackgroundShadowPanel._BL = OverlayResources.CreateImageResource(Resources.shadowBottomLeft, ImageFormat.Png);
            Panels.BackgroundShadowPanel._B = OverlayResources.CreateImageResource(Resources.shadowBottom, ImageFormat.Png);
            Panels.BackgroundShadowPanel._BR = OverlayResources.CreateImageResource(Resources.shadowBottomRight, ImageFormat.Png);

            Panels.ListBorderPanel._TL = OverlayResources.CreateImageResource(Resources.listBorderTL, ImageFormat.Png);
            Panels.ListBorderPanel._T = OverlayResources.CreateImageResource(Resources.listBorderT, ImageFormat.Png);
            Panels.ListBorderPanel._TR = OverlayResources.CreateImageResource(Resources.listBorderTR, ImageFormat.Png);
            Panels.ListBorderPanel._L = OverlayResources.CreateImageResource(Resources.listBorderL, ImageFormat.Png);
            Panels.ListBorderPanel._M = OverlayResources.CreateImageResource(Resources.marble, ImageFormat.Jpeg);
            Panels.ListBorderPanel._R = OverlayResources.CreateImageResource(Resources.listBorderR, ImageFormat.Png);
            Panels.ListBorderPanel._BL = OverlayResources.CreateImageResource(Resources.listBorderBL, ImageFormat.Png);
            Panels.ListBorderPanel._B = OverlayResources.CreateImageResource(Resources.listBorderB, ImageFormat.Png);
            Panels.ListBorderPanel._BR = OverlayResources.CreateImageResource(Resources.listBorderBR, ImageFormat.Png);

            Panels.ScrollBorderPanel._T = OverlayResources.CreateImageResource(Resources.scrollareaTop, ImageFormat.Png);
            Panels.ScrollBorderPanel._M = OverlayResources.CreateImageResource(Resources.scrollareaMiddle, ImageFormat.Png);
            Panels.ScrollBorderPanel._B = OverlayResources.CreateImageResource(Resources.scrollareaBottom, ImageFormat.Png);

            Controls.Base.LargeButton._UpResource = OverlayResources.CreateImageResource(Resources.ButtonLargeUp, ImageFormat.Png);
            Controls.Base.LargeButton._OverResource = OverlayResources.CreateImageResource(Resources.ButtonLargeOver, ImageFormat.Png);
            Controls.Base.LargeButton._DownResource = OverlayResources.CreateImageResource(Resources.ButtonLargeDown, ImageFormat.Png);

            Controls.Base.SmallButton._UpResource = OverlayResources.CreateImageResource(Resources.ButtonSmallUp, ImageFormat.Png);
            Controls.Base.SmallButton._DownResource = OverlayResources.CreateImageResource(Resources.ButtonSmallDown, ImageFormat.Png);
            Controls.Base.SmallButton._OverResource = OverlayResources.CreateImageResource(Resources.ButtonSmallOver, ImageFormat.Png);

            Controls.GroupSelectButton._DropDownArrowResource = OverlayResources.CreateImageResource(Resources.ButtonDropDownArrow, ImageFormat.Png);
            Controls.GroupSelectButton._DropUpArrowResource = OverlayResources.CreateImageResource(Resources.ButtonDropUpArrow, ImageFormat.Png);

            Controls.HomeButton._HomeIconResource = OverlayResources.CreateImageResource(Resources.HomeButtonIcon, ImageFormat.Png);

            OverlayWindow._BackgroundResource = OverlayResources.CreateImageResource(Resources.windowBackground, ImageFormat.Jpeg);
            OverlayWindow._OverShadowResource = OverlayResources.CreateColorImageResource(Color.FromArgb(0, 0, 0));
            OverlayWindow._GroupListBackgroundResource = OverlayResources.CreateColorImageResource(Color.FromArgb(28, 25, 22));

            Controls.Base.VerticalScrollBar._ScrollBarResource = OverlayResources.CreateImageResource(Resources.scrollbar, ImageFormat.Png);
            Controls.Base.VerticalScrollBar._ScrollBarTopResource = OverlayResources.CreateImageResource(Resources.scrollbar_top, ImageFormat.Png);
            Controls.Base.VerticalScrollBar._ScrollBarBottomResource = OverlayResources.CreateImageResource(Resources.scrollbar_bottom, ImageFormat.Png);

            StaticResources.ButtonRoundUp = OverlayResources.CreateImageResource(Resources.ButtonRoundUp, ImageFormat.Png);
            StaticResources.ButtonRoundOver = OverlayResources.CreateImageResource(Resources.ButtonRoundOver, ImageFormat.Png);
            StaticResources.ButtonRoundDown = OverlayResources.CreateImageResource(Resources.ButtonRoundDown, ImageFormat.Png);
            StaticResources.ButtonRoundDownOver = OverlayResources.CreateImageResource(Resources.ButtonRoundDownOver, ImageFormat.Png);
            StaticResources.LockWhiteIcon = OverlayResources.CreateImageResource(Resources.LockWhiteIcon, ImageFormat.Png);

            Controls.InfoBoxReloadButton._UpResource = OverlayResources.CreateImageResource(Resources.ReloadButtonSmallUp, ImageFormat.Png);
            Controls.InfoBoxReloadButton._DownResource = OverlayResources.CreateImageResource(Resources.ReloadButtonSmallDown, ImageFormat.Png);
            Controls.InfoBoxReloadButton._OverResource = OverlayResources.CreateImageResource(Resources.ReloadButtonSmallOver, ImageFormat.Png);

            Utils.FontLoader.LoadFromResource(Resources.font_LinBiolinum_R);
            Utils.FontLoader.LoadFromResource(Resources.font_LinBiolinum_RB);
            Utils.FontLoader.LoadFromResource(Resources.font_LinBiolinum_RI);

            InfoBox._TitleFont = Utils.FontLoader.GetFont("Linux Biolinum", 21, FontStyle.Regular);
            InfoBox._TextFont = Utils.FontLoader.GetFont("Linux Biolinum", 19, FontStyle.Regular);
            Controls.Base.SmallButton._Font = Utils.FontLoader.GetFont("Linux Biolinum", 15, FontStyle.Regular);
            Controls.Base.LargeButton._Font = Utils.FontLoader.GetFont("Linux Biolinum", 20, FontStyle.Regular);
            GroupList._Font = Utils.FontLoader.GetFont("Linux Biolinum", 22, FontStyle.Regular);

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
                Resource = OverlayResources.CreateImageResource(Resources.title, ImageFormat.Png),
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
