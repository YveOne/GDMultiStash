using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3DHook.Overlay.Scrolling
{

    public interface IScrollable
    {
        ScrollHandler ScrollHandler { get; }
    }

}
