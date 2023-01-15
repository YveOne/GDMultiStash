using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMultiStash.Common.Overlay
{
    public class Viewport : Element
    {


        public ResourceHandler OverlayResources { get; private set; }

        protected override Viewport ParentViewport => this;

        public Viewport()
        {
            OverlayResources = new ResourceHandler();
            MouseCheckNeedBaseHit = true;
        }

        public bool UpdateRequested()
        {
            bool r = _updateRequested;
            _updateRequested = false;
            return r;
        }

        public bool RedrawRequested()
        {
            bool r = _redrawRequested;
            _redrawRequested = false;
            return r;
        }

        public virtual bool DrawRoutine(float ms)
        {
            if (TotalWidth <= 0) return false;
            if (TotalHeight <= 0) return false;
            base.DrawBegin();
            _redrawRequested |= Animations.Animator.AnimateAll(ms);
            base.Draw(ms);
            base.DrawEnd();
            base.Cleanup();
            return true;
        }

    }
}
