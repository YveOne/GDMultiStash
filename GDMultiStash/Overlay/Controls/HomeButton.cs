
using D3DHook.Overlay;

namespace GDMultiStash.Overlay.Controls
{
    internal class HomeButton : Base.SmallButton
    {

        private ImageElement _homeImage;

        public HomeButton() : base()
        {
            _homeImage = new ImageElement() {
                AnchorPoint = Anchor.Center,
                Width = 20,
                Height = 20,
                Resource = StaticResources.HomeButtonIcon,
            };
            AddChild(_homeImage);
            MouseClick += delegate {
                Global.Ingame.SwitchToMainStash();
            };
        }

    }
}
