
using System;
using System.Drawing;

using D3DHook.Overlay;

namespace GDMultiStash.Overlay.Controls
{
    internal class GroupSelectButton : Base.LargeButton
    {

        private ImageElement _dropArrowImage;

        private bool _droppedDown = false;

        public EventHandler<EventArgs> DropDownOpened;
        public EventHandler<EventArgs> DropDownClosed;

        public GroupSelectButton() : base()
        {
            TextAnchor = Anchor.Left;
            TextAlign = StringAlignment.Near;
            _dropArrowImage = new ImageElement() {
                AnchorPoint = Anchor.Right,
                Width = 15,
                Height = 15,
            };
            AddChild(_dropArrowImage);
            MouseClick += delegate {
                ToggleDropDown();
            };
            HandleDropDown();



            Text = Global.Groups.GetGroup(Global.Runtime.ActiveGroupID).Name;
            Global.Runtime.ActiveGroupChanged += delegate {
                Text = Global.Groups.GetGroup(Global.Runtime.ActiveGroupID).Name;
            };
            Global.Runtime.StashGroupsInfoChanged += delegate {
                Text = Global.Groups.GetGroup(Global.Runtime.ActiveGroupID).Name;
            };

        }

        public void ToggleDropDown()
        {
            if (_droppedDown) CloseDropDown();
            else OpenDropDown();
        }

        public void OpenDropDown()
        {
            if (_droppedDown) return;
            _droppedDown = true;
            DropDownOpened?.Invoke(this, EventArgs.Empty);
            HandleDropDown();
        }

        public void CloseDropDown()
        {
            if (!_droppedDown) return;
            _droppedDown = false;
            DropDownClosed?.Invoke(this, EventArgs.Empty);
            HandleDropDown();
        }

        private void HandleDropDown()
        {
            if (_droppedDown)
            {
                _dropArrowImage.Resource = StaticResources.ButtonDropUpArrow;
                _dropArrowImage.Y = -2;
            }
            else
            {
                _dropArrowImage.Resource = StaticResources.ButtonDropDownArrow;
                _dropArrowImage.Y = 2;
            }
        }

        private bool _updateWidth = false;

        protected override void OnDraw()
        {
            base.OnDraw();
            if (_updateWidth)
            {
                _updateWidth = false;
                TextX = TotalWidth / TotalScale * 0.08f;
                TextWidth = TotalWidth / TotalScale * -0.16f - 20;
                _dropArrowImage.X = TotalWidth / TotalScale * -0.066f;
            }
        }

        protected override void OnDrawEnd()
        {
            base.OnDrawEnd();
            if (ResetWidth) _updateWidth = true;
        }

    }
}
