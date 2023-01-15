using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using GDMultiStash.Common.Overlay;
using GDMultiStash.Common.Overlay.Animations;
using GDMultiStash.Overlay.Controls;
using GDMultiStash.Overlay.Controls.Base;

namespace GDMultiStash.Overlay
{
    internal class OverlayWindow : Element
    {

        public enum States
        {
            Hidden = 0,
            Showing = 1,
            Shown = 2,
            Hiding = 3,
        }

        private readonly Animator _fadeAnimator;
        private readonly AnimationValue _fadeValue;
        private const float fadeDuration = 300;
        private const float fadeDurationSlow = 2000;
        private const float fadeDurationSlowDelay = 2000;

        private readonly Animator _moveAnimator;
        private readonly AnimationValue _moveValue;
        private const float _moveDuration = 300;

        private bool _mouseOver = false;
        private bool _mouseDown = false;
        private bool _updateAppearance = false;

        private readonly InfoBox _infoWindow;
        private readonly StashList _stashList;
        private readonly VerticalScrollBar _stashListScrollBar;
        private readonly HomeButton _homeButton;
        private readonly GroupSelectButton _groupSelectButton;
        private readonly ImageElement _overShadow;
        private readonly GroupList _groupList;
        private readonly VerticalScrollBar _groupListScrollBar;
        private readonly ImageElement _groupListBackground;

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

        public OverlayWindow()
        {
            AnchorPoint = Anchor.Left;

            _stashList = new StashList()
            {
                X = 6,
                Y = 61,
                WidthToParent = true,
                Width = -25,
            };
            _stashListScrollBar = new VerticalScrollBar()
            {
                AnchorPoint = Anchor.TopRight,
                X = -5,
                Y = 55,
                Width = 9,
                HeightToParent = true,
                Height = -213, //567,
            };
            _infoWindow = new InfoBox()
            {
                X = 6,
                Y = -10,
                WidthToParent = true,
                Width = -14,
                Height = 127,
                AnchorPoint = Anchor.BottomLeft,
            };
            _overShadow = new ImageElement()
            {
                AnchorPoint = Anchor.TopLeft,
                WidthToParent = true,
                HeightToParent = true,
                Visible = false,
                Alpha = 0,
                Resource = StaticResources.WindowOverShadow,
            };
            _homeButton = new HomeButton()
            {
                X = 4,
                Y = 6,
                Width = 50,
                Height = 41,
            };
            _groupSelectButton = new GroupSelectButton()
            {
                X = _homeButton.X + _homeButton.Width + 2,
                Y = 6,
                WidthToParent = true,
                Width = -(_homeButton.X + _homeButton.Width + 4) - 4,
                Height = 41,
            };
            _groupListBackground = new ImageElement()
            {
                X = _groupSelectButton.X + 10,
                Y = _groupSelectButton.Y + _groupSelectButton.Height,
                WidthToParent = true,
                Width = _groupSelectButton.Width - 10 -10,
                Resource = StaticResources.GroupListBackground,
                Visible = false,
            };
            _groupList = new GroupList()
            {
                X = 5,
                Y = 10,
                WidthToParent = true,
                Width = -15,
            };
            _groupListScrollBar = new VerticalScrollBar()
            {
                AnchorPoint = Anchor.TopRight,
                X = -2,
                Y = 10,
                Width = 10,
                HeightToParent = true,
                Height = -20,
            };

            AddChild(new ImageElement()
            {
                DebugColor = Color.FromArgb(128, 0, 0, 0),
                Resource = StaticResources.WindowBackground,
                AnchorPoint = Anchor.TopRight,
                Width = 600,
                HeightToParent = true,
            });

            AddChild(new Panels.BackgroundBorderPanel()
            {
                AnchorPoint = Anchor.TopRight,
                WidthToParent = true,
                HeightToParent = true,
                Height = -143,
            });

            AddChild(new Panels.BackgroundBorderPanel()
            {
                AnchorPoint = Anchor.BottomRight,
                WidthToParent = true,
                Height = 143,
            });

            AddChild(new Panels.BackgroundShadowPanel()
            {
                AnchorPoint = Anchor.BottomRight,
                WidthToParent = true,
                Width = -9,
                Height = 131,
                X = -6,
                Y = -6,
                Alpha = 0.8f
            });

            AddChild(new Panels.ListBorderPanel()
            {
                AnchorPoint = Anchor.TopRight,
                WidthToParent = true,
                HeightToParent = true,
                Width = -4,
                Height = -199,
                X = -3,
                Y = 48,
            });

            AddChild(new Panels.ScrollBorderPanel()
            {
                AnchorPoint = Anchor.TopRight,
                HeightToParent = true,
                Width = 45,
                Height = -205,
                X = -1,
                Y = 51,
            });

            AddChild(_stashList);
            AddChild(_stashListScrollBar);
            AddChild(_infoWindow);
            AddChild(_homeButton);

            AddChild(_overShadow);

            AddChild(_groupSelectButton);
            _groupListBackground.AddChild(_groupList);
            _groupListBackground.AddChild(_groupListScrollBar);
            AddChild(_groupListBackground);









            new ScrollManager(_stashList, _stashListScrollBar);
            _stashListScrollBar.ScrollingStart += delegate {
                _groupSelectButton.EventsEnabled = false;
                _stashList.EventsEnabled = false;
                _infoWindow.EventsEnabled = false;
            };
            _stashListScrollBar.ScrollingEnd += delegate {
                _groupSelectButton.EventsEnabled = true;
                _stashList.EventsEnabled = true;
                _infoWindow.EventsEnabled = true;
            };







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
                Global.Ingame.DisableMovement();
                _mouseDown = true;
            };
            MouseWheel += (object sender, MouseWheelEventArgs e) => {
                System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(() => {
                    Native.Input inp = new Native.Input
                    {
                        type = Native.InputType.Mouse,
                        u = new Native.InputUnion
                        {
                            mi = new Native.MouseInput
                            {
                                dx = e.X,
                                dy = e.Y,
                                mouseData = -e.Delta,
                                dwFlags = 0x0800,
                            }
                        }
                    };
                    Native.SendInput(1, new Native.Input[] { inp }, System.Runtime.InteropServices.Marshal.SizeOf(typeof(Native.Input)));
                }));
                t.Start();
            };



            _fadeValue = new AnimationValue() {
                Easer = Easing.PolyOut(),
                Min = 1,
                Max = 1,
            };
            _fadeAnimator = Animator.Create(fadeDuration);
            _fadeAnimator.AnimationStart += delegate {
            };
            _fadeAnimator.AnimationAnimate += delegate (object sender, AnimatorAnimateEventArgs args) {
                Alpha = _fadeValue.Convert(args.Value);
            };
            _fadeAnimator.AnimationEnd += delegate {
            };



            _moveValue = new AnimationValue()
            {
                Easer = Easing.PolyOut(), //Easing.BackOut(1.1f),
                Min = 0,
                Max = 0,
            };
            _moveAnimator = Animator.Create(_moveDuration);
            _moveAnimator.Delay = 100; // debug: fix lag on npc stash window open
            _moveAnimator.AnimationStart += delegate {
            };
            _moveAnimator.AnimationAnimate += delegate (object sender, AnimatorAnimateEventArgs args) {
                X = _moveValue.Convert(args.Value);
            };
            _moveAnimator.AnimationEnd += delegate {
                switch (_state)
                {
                    case States.Showing:
                        State = States.Shown;
                        _fadeAnimator.Duration = fadeDurationSlow;
                        _fadeAnimator.Delay = fadeDurationSlowDelay;
                        _fadeAnimator.Value = 0f;
                        break;
                    case States.Hiding:
                        State = States.Hidden;
                        _groupSelectButton.CloseDropDown();
                        _fadeAnimator.Reset();
                        break;
                }
            };

            // group dropdown effect
            {
                AnimationValue overShadowFading = new AnimationValue()
                {
                    Easer = Easing.PolyOut(),
                    Min = 0,
                    Max = 0.66f,
                };
                AnimationValue groupListBackFading = new AnimationValue()
                {
                    Easer = Easing.PolyOut(),
                    Min = 0,
                    Max = 1,
                };
                Animator animator = Animator.Create(200);
                animator.AnimationAnimate += delegate (object sender, AnimatorAnimateEventArgs args) {
                    _overShadow.Alpha = overShadowFading.Convert(args.Value);
                    _groupListBackground.Alpha = groupListBackFading.Convert(args.Value);
                };
                animator.AnimationStart += delegate {
                    _overShadow.Visible = true;
                    _groupListBackground.Visible = true;
                };
                animator.AnimationEnd += delegate {
                    _overShadow.Visible = animator.Value != 0;
                    _groupListBackground.Visible = animator.Value != 0;
                };
                _groupSelectButton.DropDownOpened += delegate {
                    animator.Value = 1;
                    _stashList.EventsEnabled = false;
                    _stashListScrollBar.EventsEnabled = false;
                    _infoWindow.EventsEnabled = false;
                    _homeButton.EventsEnabled = false;
                };
                _groupSelectButton.DropDownClosed += delegate {
                    animator.Value = 0;
                    _stashList.EventsEnabled = true;
                    _stashListScrollBar.EventsEnabled = true;
                    _infoWindow.EventsEnabled = true;
                    _homeButton.EventsEnabled = true;
                };
            }





            new ScrollManager(_groupList, _groupListScrollBar);
            _groupListScrollBar.ScrollingStart += delegate {
                _groupSelectButton.EventsEnabled = false;
            };
            _groupListScrollBar.ScrollingEnd += delegate {
                _groupSelectButton.EventsEnabled = true;
            };











            _updateAppearance = true; // update on startup
            Global.Configuration.AppearanceChanged += delegate {
                _updateAppearance = true;
            };
            Global.Ingame.ActiveGroupChanged += delegate {
                _groupSelectButton.CloseDropDown();
            };

            Global.Ingame.StashReopenStart += delegate {
                MouseCheckChildren = false;
                _infoWindow.Alpha = 0.70f;
                _stashList.Alpha = 0.70f;
                _homeButton.Alpha = 0.80f;
                _groupSelectButton.Alpha = 0.80f;
            };

            Global.Ingame.StashReopenEnd += delegate {
                MouseCheckChildren = true;
                _infoWindow.Alpha = 1.0f;
                _stashList.Alpha = 1.0f;
                _homeButton.Alpha = 1.0f;
                _groupSelectButton.Alpha = 1.0f;
            };

        }

        private void FadeInFast()
        {
            _fadeAnimator.Duration = fadeDuration;
            _fadeAnimator.Delay = 0f;
            _fadeAnimator.Value = 1f;
        }

        private void FadeOutFast()
        {
            _fadeAnimator.Duration = fadeDuration;
            _fadeAnimator.Delay = 0f;
            _fadeAnimator.Value = 0f;
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

        protected override void OnDraw()
        {
            base.OnDraw();
            if (_updateAppearance)
            {
                _updateAppearance = false;

                Scale = (float)Global.Configuration.Settings.OverlayScale / 100f;
                Width = Global.Configuration.Settings.OverlayWidth;

                _stashList.ScrollHandler.VisibleUnitsY = Global.Configuration.Settings.OverlayStashesCount;
                Height = 222 + _stashList.Height;

                _groupList.ScrollHandler.VisibleUnitsY = Global.Configuration.Settings.OverlayStashesCount + 5;
                _groupListBackground.Height = _groupList.Height + 20;

                _moveValue.Min = -TotalWidth - 5;
                _moveAnimator.Reset(Global.Ingame.StashIsOpened ? 1f : 0f);
                _fadeValue.Min = (float)(100 - Global.Configuration.Settings.OverlayTransparency) / 100f;
                _fadeAnimator.Reset(Global.Ingame.StashIsOpened ? 1f : 0f);

                Redraw();
            }
        }

        public void Show()
        {
            if (_moveAnimator.Value == 0f)
            {
                // box is max left (not visible on screen)
                // reset alpha to 1 (visible)
                _fadeAnimator.Reset(1);
                //if (Global.Configuration.Settings.AutoBackToMain)
                //    _stashList.ScrollHandler.ScrollPositionY = 0;
            }
            _moveAnimator.Value = 1f;
            State = States.Showing;
        }

        public void Hide()
        {
            _moveAnimator.Value = 0f;
            State = States.Hiding;
        }

    }
}
