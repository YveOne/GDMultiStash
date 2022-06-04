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
        private const float _moveDuration = 300;

        private bool _mouseOver = false;
        private bool _mouseDown = false;
        private bool _updateAppearance = false;

        private readonly InfoWindow _infoWindow;
        private readonly StashList _stashList;
        private readonly VerticalScrollBar _scrollBar;

        //private bool showing = false;
        //private bool shown = false;

        public enum States
        {
            Hidden = 0,
            Showing = 1,
            Shown = 2,
            Hiding = 3,
        }

        public delegate void StateChangedEventHandler(States state);
        public event StateChangedEventHandler StateChanged;

        private States _state = States.Hidden;
        public States State
        {
            get => _state;
            private set
            {
                if (_state == value) return;
                _state = value;
                StateChanged?.Invoke(_state);
            }
        }

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
                Y = 20,
                WidthToParent = true,
                Width =  -10 - 32,
                Height = 561,
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





















            Alpha = fadeMin;
            _fadeAnimation = new FadeAnimation(this, Utils.Easing.PolyOut(), fadeDuration);
            _fadeAnimation.MinAlpha = fadeMin;
            _fadeAnimation.MaxAlpha = fadeMax;
            MouseEnter += (object sender, EventArgs e) => {
                FadeInFast();
                _mouseOver = true;
                State = States.Shown;
            };
            MouseLeave += (object sender, EventArgs e) => {
                if (!_mouseDown) FadeOutFast();
                _mouseOver = false;
            };
            MouseDown += (object sender, EventArgs e) => {
                _mouseDown = true;
            };


            _moveAnimation = new MoveAnimation(this, Utils.Easing.BackOut(1.1f), _moveDuration);

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
                switch(_state)
                {
                    case States.Showing:
                        State = States.Shown;
                        _fadeAnimation.Duration = fadeDurationSlow;
                        _fadeAnimation.Delay = fadeDurationSlowDelay;
                        _fadeAnimation.Value = 0f;
                        break;
                    case States.Hiding:
                        State = States.Hidden;
                        break;
                }
            }
            if (_state == States.Shown)
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
            State = States.Showing;
        }

        public void Hide()
        {
            _moveAnimation.Value = 0f;
            State = States.Hiding;
        }

    }
}
