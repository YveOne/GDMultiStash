using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using GDMultiStash.Common.Overlay;
using GDMultiStash.Common.Overlay.Animations;

namespace GDMultiStash.Overlay.Elements
{
    internal class MainWindow : Element
    {

        public static D3DHook.Hook.Common.IImageResource _BackgroundResource;
        public static D3DHook.Hook.Common.IImageResource _BackgroundLeftResource;

        private readonly FadeAnimation _fadeAnimation;
        private const float fadeMin = 0.1f;
        private const float fadeMax = 1.0f;
        private const float fadeDuration = 300;
        private const float fadeDurationSlow = 2000;
        private const float fadeDurationSlowDelay = 2000;

        private readonly MoveAnimation _moveAnimation;
        private const float moveDuration = 300;

        private bool _mouseOver = false;
        private bool _mouseDown = false;
        private bool _updateAppearance = false;

        private readonly InfoWindow _infoWindow;
        private readonly StashList _stashList;
        private readonly VerticalScrollBar _scrollBar;

        public MainWindow() : base()
        {
            AnchorPoint = Anchor.Left;
            Height = 740;

            AddChild(new ImageElement()
            {
                Resource = _BackgroundResource,
                AnchorPoint = Anchor.TopRight,
            });

            AddChild(new ImageElement()
            {
                Resource = _BackgroundLeftResource,
                AnchorPoint = Anchor.TopLeft,
                X = -5,
            });

            _stashList = new StashList()
            {
                X = 10,
                Y = 16,
                WidthToParent = true,
                Width =  -10 - 32,
                Height = 565,
            };
            AddChild(_stashList);

            _scrollBar = new VerticalScrollBar()
            {
                AnchorPoint= Anchor.TopRight,
                X = -8 - 3,
                Y = 16 + 3,
            };
            AddChild(_scrollBar);

            _infoWindow = new InfoWindow();
            AddChild(_infoWindow);

            _scrollBar.ScrollingStart += delegate {
                _stashList.LockMouseMove = true;
            };
            _scrollBar.ScrollingEnd += delegate {
                _stashList.LockMouseMove = false;
            };
            _scrollBar.Scrolling += delegate (object sender, ScrollBarElement.ScrollingEventArgs e) {
                _stashList.Scrollindex = e.Y;
            };
            _stashList.VisibleCountChanged += delegate (int visibleCount) {
                _scrollBar.ScrollHeightUnits = visibleCount;
            };


            





            Core.Runtime.ActiveStashChanged += delegate {
                _stashList.UpdateList();
            };
            Core.Runtime.StashesChanged += delegate {
                _stashList.RebuildList();
                _stashList.Scrollindex = 0;
                _scrollBar.ScrollUnitsY = 0;
            };


            Alpha = fadeMin;
            _fadeAnimation = new FadeAnimation(this, Utils.Easing.PolyOut(), fadeDuration);
            _fadeAnimation.MinAlpha = fadeMin;
            _fadeAnimation.MaxAlpha = fadeMax;
            MouseEnter += (object sender, EventArgs e) => {
                FadeInFast();
                _mouseOver = true;
                showing = false;
                shown = true;
            };
            MouseLeave += (object sender, EventArgs e) => {
                if (!_mouseDown) FadeOutFast();
                _mouseOver = false;
            };
            MouseDown += (object sender, EventArgs e) => {
                _mouseDown = true;
            };


            _moveAnimation = new MoveAnimation(this, Utils.Easing.BackOut(1.1f), moveDuration);

            _updateAppearance = true;
            Core.Config.AppearanceChanged += delegate { _updateAppearance = true; };

        }

        private void FadeInFast()
        {
            _fadeAnimation.Duration = fadeDuration;
            _fadeAnimation.Delay = 0f;
            _fadeAnimation.Value = 1f;
        }

        private void FadeOutFast()
        {
            _fadeAnimation.Duration = fadeDuration;
            _fadeAnimation.Delay = 0f;
            _fadeAnimation.Value = 0f;
        }

        public override bool CheckMouseDown(int x, int y)
        {
            bool hit = base.CheckMouseDown(x, y);
            if (hit) Core.Runtime.DisableMovement();
            return hit;
            // enabling movement is handled in gdsm context
        }

        public override bool CheckMouseUp(int x, int y)
        {
            if (_mouseDown && !_mouseOver)
            {
                FadeOutFast();
            }
            _mouseDown = false;
            return base.CheckMouseUp(x, y);
        }

        private bool showing = false;
        private bool shown = false;

        public override void Draw(float ms)
        {
            base.Draw(ms);
            bool redraw = false;
            if (_updateAppearance)
            {
                _updateAppearance = false;
                redraw = true;
                Scale = Core.Config.OverlayScale;
                Width = Core.Config.OverlayWidth;
                _moveAnimation.MinX = -Width - 5;
                _moveAnimation.MaxX = 0;
                _moveAnimation.Reset(Core.Runtime.StashOpened ? 1f : 0f);
            }
            if (_moveAnimation.Animate(ms))
            {
                redraw = true;
            }
            else
            {
                if (showing)
                {
                    showing = false;
                    shown = true;
                    _fadeAnimation.Duration = fadeDurationSlow;
                    _fadeAnimation.Delay = fadeDurationSlowDelay;
                    _fadeAnimation.Value = 0f;
                }
            }
            if (shown)
            {
                // only enable fading when sliding finished
                if (_fadeAnimation.Animate(ms))
                    redraw = true;
            }
            if (redraw)
                Redraw();
        }

        public void Show()
        {
            if (_moveAnimation.Value == 0f)
            {
                // box is max left (not visible on screen)
                // reset alpha to 1
                _fadeAnimation.Reset(1f);
            }
            _moveAnimation.Value = 1f;
            showing = true;
        }

        public void Hide()
        {
            _moveAnimation.Value = 0f;
            shown = false;
            showing = false;
        }

    }
}
