using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3DHook.Overlay.Common
{

    public class CheckableElement : Element
    {
        public EventHandler<EventArgs> CheckStateChanged;

        protected virtual D3DHook.Hook.Common.IImageResource UpResource { get; }
        protected virtual D3DHook.Hook.Common.IImageResource OverResource { get; }

        protected virtual D3DHook.Hook.Common.IImageResource CheckedUpResource { get; }
        protected virtual D3DHook.Hook.Common.IImageResource CheckedOverResource { get; }

        public bool Checked
        {
            get => _checked;
            set
            {
                if (_checked == value) return;
                _checked = value;
                UpdateImage();
                CheckStateChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        private bool _checked = false;

        protected ImageElement Image => _image;
        protected ImageElement _image;

        public CheckableElement()
        {
            _image = new ImageElement()
            {
                Resource = UpResource,
                WidthToParent = true,
                HeightToParent = true,
            };
            AddChild(_image);
            MouseEnter += delegate { UpdateImage(); };
            MouseLeave += delegate { UpdateImage(); };
            MouseClick += delegate { Checked = !_checked; };
            UpdateImage();
        }

        protected void UpdateImage()
        {
            if (_checked)
                _image.Resource = IsMouseOver ? CheckedOverResource : CheckedUpResource;
            else
                _image.Resource = IsMouseOver ? OverResource : UpResource;
        }

    }

}
