using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3DHook.Overlay.Animations
{
    public class AnimatorEventHandler
    {
        public EventHandler<EventArgs> AnimationStart;
        public EventHandler<EventArgs> AnimationEnd;
        public EventHandler<AnimatorAnimateEventArgs> AnimationAnimate;
    }
}
