using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GDMultiStash.Common.Overlay
{
    public class FontHandler
    {
        public EventHandler<EventArgs> FontChanged;
        private Font _font;
        public Font Font
        {
            get => _font;
            set
            {
                _font = value;
                FontChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
