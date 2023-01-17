using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3DHook.Overlay.Animations
{
    public partial class Animator
    {

        private static List<Animator> _animators = new List<Animator>();

        public static Animator Create(float duration)
        {
            Animator anim = new Animator(duration);
            _animators.Add(anim);
            return anim;
        }

        public static bool AnimateAll(float ms)
        {
            bool animated = false;
            foreach (var anim in _animators)
            {
                if (anim.Animate(ms))
                    animated = true;
            }
            return animated;
        }

    }
}
