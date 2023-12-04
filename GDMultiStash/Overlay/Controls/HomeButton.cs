
using D3DHook.Overlay.Common;

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
                G.Ingame.SwitchToMainStash();
            };
        }

    }
}
