
using System;
using System.Drawing;

namespace GDMultiStash.Overlay.Controls
{
    internal class HomeButton : Base.SmallButton
    {

        private Common.Overlay.ImageElement _homeImage;

        public static D3DHook.Hook.Common.IImageResource _HomeIconResource;

        public HomeButton() : base()
        {
            _homeImage = new Common.Overlay.ImageElement() {
                AnchorPoint = Common.Overlay.Anchor.Center,
                Width = 20,
                Height = 20,
                Resource = _HomeIconResource,
            };
            AddChild(_homeImage);
            MouseClick += delegate {
                int mainStashID = Global.Configuration.GetMainStashID(Global.Runtime.CurrentExpansion, Global.Runtime.CurrentMode);
                Global.Runtime.SwitchToStash(mainStashID);
                Global.Runtime.ActiveGroupID = 0;
            };
        }

    }
}
