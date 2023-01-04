﻿
using System;
using System.Drawing;

namespace GDMultiStash.Overlay.Controls
{
    internal class GroupSelectButton : Base.LargeButton
    {

        private Common.Overlay.ImageElement _dropArrowImage;

        public static D3DHook.Hook.Common.IImageResource _DropDownArrowResource;
        public static D3DHook.Hook.Common.IImageResource _DropUpArrowResource;

        protected D3DHook.Hook.Common.IImageResource DropDownArrowResource => _DropDownArrowResource;
        protected D3DHook.Hook.Common.IImageResource DropUpArrowResource => _DropUpArrowResource;

        private bool _droppedDown = false;

        public EventHandler<EventArgs> DropDownOpened;
        public EventHandler<EventArgs> DropDownClosed;

        public GroupSelectButton() : base()
        {
            TextAnchor = Common.Overlay.Anchor.Left;
            TextAlign = StringAlignment.Near;
            _dropArrowImage = new Common.Overlay.ImageElement() {
                AnchorPoint = Common.Overlay.Anchor.Right,
                Width = 15,
                Height = 15,
            };
            AddChild(_dropArrowImage);
            MouseClick += delegate {
                ToggleDropDown();
            };
            HandleDropDown();



            Text = Global.Stashes.GetStashGroup(Global.Runtime.ActiveGroupID).Name;
            Global.Runtime.ActiveGroupChanged += delegate {
                Text = Global.Stashes.GetStashGroup(Global.Runtime.ActiveGroupID).Name;
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
                _dropArrowImage.Resource = _DropUpArrowResource;
                _dropArrowImage.Y = -2;
            }
            else
            {
                _dropArrowImage.Resource = _DropDownArrowResource;
                _dropArrowImage.Y = 2;
            }
        }

        public override void End()
        {
            base.End();
            if (ResetWidth)
            {
                TextX = TotalWidth / TotalScale * 0.08f;
                TextWidth = TotalWidth / TotalScale * -0.16f - 20;
                _dropArrowImage.X = TotalWidth / TotalScale * -0.066f;
                base.End();
            }
        }
        
        
        
    }
}