using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GDMultiStash.Common.Overlay
{
    public partial class Element
    {

        public Element()
        {
            _id = GetNextID();
        }

        #region ID

        private static int _lastID = -1;
        private static int GetNextID()
        {
            _lastID += 1;
            return _lastID;
        }

        protected readonly int _id;
        public int ID => _id;

        #endregion

        #region Viewport

        private Viewport _viewport = null;

        protected virtual Viewport GetViewport()
        {
            if (_viewport != null) return _viewport;
            _viewport = _parent.GetViewport();
            return _viewport;
        }

        #endregion

        #region Parent & Children

        private Element _parent = null;
        private readonly List<Element> _children = new List<Element>();

        public virtual Element Parent
        {
            get { return _parent; }
        }

        public void AddChild(Element child)
        {
            _children.Add(child);
            child._parent = this;
            Update();
            Redraw();
        }

        public void RemoveChild(int id)
        {
            _children.RemoveAll((Element e) => {
                if (e.ID == id)
                {
                    e.Destroy();
                    return true;
                }
                return false;
            });
            Update();
            Redraw();
        }

        public void RemoveChild(Element e)
        {
            RemoveChild(e.ID);
        }

        public void ClearChildren()
        {
            foreach (Element e in _children)
                e.Destroy();
            _children.Clear();
            Update();
            Redraw();
        }

        #endregion

        private bool _updateRequested = false;
        private bool _updateRequestResponse = false;

        private bool _redrawRequested = false;
        private bool _redrawRequestResponse = false;

        public virtual bool Update()
        {
            if (_updateRequested) return _updateRequestResponse;
            _updateRequested = true;
            if (_parent != null)
            {
                _updateRequestResponse = _parent.Update();
            }
            return _updateRequestResponse;
        }

        public virtual bool Redraw()
        {
            if (_redrawRequested) return _redrawRequestResponse;
            _redrawRequested = true;
            if (_parent != null)
            {
                _redrawRequestResponse = _parent.Redraw();
            }
            return _redrawRequestResponse;
        }

        public virtual void Reset()
        {
            _resetVisible = true;
            _resetAlpha = true;
            _resetScale = true;
            _resetWidth = true;
            _resetHeight = true;
            _resetX = true;
            _resetY = true;
            foreach (Element el in _children)
                el.Reset();
        }

        public virtual List<D3DHook.Hook.Common.IOverlayElement> GetImagesRecursive()
        {
            List<D3DHook.Hook.Common.IOverlayElement> imglist = new List<D3DHook.Hook.Common.IOverlayElement>();
            foreach (Element child in _children)
            {
                imglist.AddRange(child.GetImagesRecursive());
            }
            return imglist;
        }

        public virtual void Destroy()
        {
            MouseEnter = null;
            MouseLeave = null;
            MouseUp = null;
            MouseDown = null;
            MouseClick = null;
            foreach (Element child in _children)
                child.Destroy();
        }

        public virtual void Begin()
        {
            // update failed last frame
            if (_updateRequested)
            {
                _updateRequested = false;
                Update();
            }

            // redraw failed last frame
            if (_redrawRequested)
            {
                _redrawRequested = false;
                Redraw();
            }

            foreach (Element child in _children)
                child.Begin();
        }

        public virtual void Draw(float ms)
        {
            foreach (Element child in _children)
                child.Draw(ms);
        }

        public virtual void End()
        {
            _resetWidth |= _resetScale;
            _resetHeight |= _resetScale;
            _resetX |= _resetScale;
            _resetY |= _resetScale;
            if (_parent != null)
            {
                _resetVisible |= _parent._resetVisible;
                _resetAlpha |= _parent._resetAlpha;
                _resetScale |= _parent._resetScale;
                _resetWidth |= _parent._resetWidth || _resetScale;
                _resetHeight |= _parent._resetHeight || _resetScale;
                _resetX |= _parent._resetX || _resetWidth || _resetScale;
                _resetY |= _parent._resetY || _resetHeight || _resetScale;
            }
            if (_resetVisible) Visible = _visible;
            if (_resetAlpha) Alpha = _alpha;
            if (_resetScale) Scale = _scale;
            if (_resetWidth) Width = _width;
            if (_resetHeight) Height = _height;
            if (_resetX) X = _x;
            if (_resetY) Y = _y;
            if (_resetVisible || _resetAlpha || _resetScale || _resetWidth || _resetHeight || _resetX || _resetY)
            {
                Redraw();
            }
            foreach (Element child in _children)
                child.End();
        }

        public virtual void Cleanup()
        {
            if (_updateRequested && _updateRequestResponse)
            {
                _updateRequested = false;
                _updateRequestResponse = false;
            }
            if (_redrawRequested && _redrawRequestResponse)
            {
                _redrawRequested = false;
                _redrawRequestResponse = false;
            }

            _resetVisible = false;
            _resetAlpha = false;
            _resetWidth = false;
            _resetHeight = false;
            _resetScale = false;
            _resetX = false;
            _resetY = false;

            foreach (Element child in _children)
                child.Cleanup();
        }

        public bool ResetVisible => _resetVisible;
        private bool _resetVisible = true;

        public bool ResetAlpha => _resetAlpha;
        private bool _resetAlpha = true;

        public bool ResetScale => _resetScale;
        private bool _resetScale = true;

        public bool ResetWidth => _resetWidth;
        private bool _resetWidth = true;

        public bool ResetHeight => _resetHeight;
        private bool _resetHeight = true;

        public bool ResetX => _resetX;
        private bool _resetX = true;

        public bool ResetY => _resetY;
        private bool _resetY = true;

    }
}
