using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMultiStash.Common.Overlay
{
    public class Viewport : Element
    {

        public ResourceHandler Resources => _resourceHandler;
        private readonly ResourceHandler _resourceHandler;

        public Viewport()
        {
            _resourceHandler = new ResourceHandler();
            MouseCheckNeedBaseHit = true;
        }

        protected override Viewport GetViewport()
        {
            return this;
        }

        private bool _updateRequested = false;
        private bool _redrawRequested = false;

        public override bool Update()
        {
            _updateRequested = true;
            return true;
        }

        public override bool Redraw()
        {
            _redrawRequested = true;
            return true;
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
            if (Width <= 0) return false;
            if (Height <= 0) return false;
            base.Begin();
            base.Draw(ms);
            base.End();
            base.Cleanup();
            return true;
        }

    }
}
