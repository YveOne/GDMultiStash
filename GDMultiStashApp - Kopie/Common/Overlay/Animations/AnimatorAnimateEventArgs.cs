using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMultiStash.Common.Overlay.Animations
{
    public class AnimatorAnimateEventArgs : EventArgs
    {
        public float Value { get; private set; }
        public AnimatorAnimateEventArgs(float value)
        {
            Value = value;
        }
    }
}
