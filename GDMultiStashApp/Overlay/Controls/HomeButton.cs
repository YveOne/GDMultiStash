﻿
using System;
using System.Drawing;

namespace GDMultiStash.Overlay.Controls
{
    internal class HomeButton : Base.SmallButton
    {

        private Common.Overlay.ImageElement _homeImage;

        public HomeButton() : base()
        {
            _homeImage = new Common.Overlay.ImageElement() {
                AnchorPoint = Common.Overlay.Anchor.Center,
                Width = 20,
                Height = 20,
                Resource = StaticResources.HomeButtonIcon,
            };
            AddChild(_homeImage);
            MouseClick += delegate {
                int mainStashID = Global.Configuration.GetMainStashID(Global.Ingame.ActiveExpansion, Global.Ingame.ActiveMode);
                Global.Ingame.SwitchToStash(mainStashID);
                Global.Ingame.ActiveGroupID = 0;
            };
        }

    }
}