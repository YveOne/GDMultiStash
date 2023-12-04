using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using D3DHook.Hook.Common;

namespace D3DHook.Overlay.Common
{
    public partial class Element
    {
        public Element()
        {
            _debugImage = new D3DHook.Hook.Common.ImageElement();
        }

        #region ID

        private static int _lastID = -1;

        private static int GetNextID()
        {
            _lastID += 1;
            return _lastID;
        }

        public int ID { get; } = GetNextID();

        #endregion

        #region Parent, Viewport, Children

        private Element _parent = null;
        private Viewport _parentViewport = null;
        private readonly List<Element> _children = new List<Element>();

        public Element Parent
        {
            get => _parent;
            private set
            {
                // no current parent
                if (_parent == null)
                {
                    // connect
                    if (value != null)
                    {
                        ParentConnect(value);
                    }
                }
                else
                {
                    // first disconnect
                    ParentDisconnect();
                    // connect to new
                    if (value != null)
                    {
                        ParentConnect(value);
                    }
                }
            }
        }

        protected virtual Viewport ParentViewport => _parentViewport;

        protected virtual void OnParentConnected(Element newElement) { }
        protected virtual void OnParentDisconnected(Element oldElement) { }
        protected virtual void OnViewportConnected(Element newElement) { }
        protected virtual void OnViewportDisconnected(Element oldElement) { }
        protected virtual void OnChildConnected(Element newElement) { }
        protected virtual void OnChildDisconnected(Element oldElement) { }

        private void ParentConnect(Element parent)
        {
            if (_parent == null)
            {
                _parent = parent;
                OnParentConnected(_parent);
                if (_parent.ParentViewport != null)
                {
                    ViewportConnect(_parent.ParentViewport);
                }
            }
        }

        private void ParentDisconnect()
        {
            if (_parent != null)
            {
                var oldParent = _parent;
                _parent = null;
                OnParentDisconnected(oldParent);
                ViewportDisconnect();
            }
        }

        private void ViewportConnect(Viewport parentViewport)
        {
            if (_parentViewport == null)
            {
                _parentViewport = parentViewport;
                _children.ForEach(child => child.ViewportConnect(parentViewport));
                if (Debugging)
                {
                    parentViewport.OverlayResources.AsyncCreateColorImageResource(DebugColor)
                        .ResourceCreated += delegate (object sender, ResourceHandler.ResourceCreatedEventArgs args) {
                            _debugImage.ResourceUID = args.Resource.UID;
                        };
                }
                OnViewportConnected(_parentViewport);
            }
        }

        private void ViewportDisconnect()
        {
            if (_parentViewport != null)
            {
                if (Debugging)
                {
                    _parentViewport.OverlayResources.DeleteResource(_debugImage.ResourceUID);
                    _debugImage.ResourceUID = -1;
                }
                _children.ForEach(child => child.ViewportDisconnect());
                var oldViewport = _parentViewport;
                _parentViewport = null;
                OnViewportDisconnected(oldViewport);
            }
        }

        public virtual void AddChild(Element child)
        {
            // check if child is already in children list
            //if (Children.FindIndex(e => e.ID == child.ID) != -1)
            if (_children.Contains(child))
                return;
            _children.Add(child);
            child.Parent = this;
            OnChildConnected(child);
            Redraw(true);
        }

        public virtual void RemoveChild(Element child)
        {
            if (!_children.Contains(child)) return;
            child.Parent = null;
            _children.Remove(child);
            OnChildDisconnected(child);
            Redraw(true);
        }

        #endregion

        #region Requests

        protected bool _redrawRequested = false;
        protected bool _updateRequested = false;

        public virtual void Redraw(bool andUpdate = false)
        {
            _redrawRequested = true;
            _updateRequested |= andUpdate;
        }

        #endregion

        #region Debugging 

        protected static bool Debugging = false;
        private readonly D3DHook.Hook.Common.ImageElement _debugImage;
        public virtual Color DebugColor { get; set; } = Color.FromArgb(0, 0, 0, 0);

        #endregion

        public virtual List<IOverlayElement> GetImagesRecursive()
        {
            List<IOverlayElement> imglist = new List<IOverlayElement>();

            if (Debugging)
                imglist.Add(_debugImage);

            foreach (Element child in _children)
                imglist.AddRange(child.GetImagesRecursive());

            return imglist;
        }

        protected virtual void OnDestroy() { }
        protected virtual void OnDrawBegin() { }
        protected virtual void OnDraw() { }
        protected virtual void OnDrawEnd() { }
        protected virtual void OnCleanup() { }

        public void Destroy()
        {
            MouseEnter = null;
            MouseLeave = null;
            MouseUp = null;
            MouseDown = null;
            MouseClick = null;
            _children.ForEach(child => child.Destroy());
            _children.Clear();
            OnDestroy();
        }

        public void DrawBegin()
        {
            _children.ForEach(child => child.DrawBegin());
            OnDrawBegin();
        }

        public void Draw(float ms)
        {
            _children.ForEach(child => child.Draw(ms));
            OnDraw();
        }

        public void DrawEnd()
        {
            ResetWidth |= ResetScale;
            ResetHeight |= ResetScale;
            ResetX |= ResetScale || ResetWidth;
            ResetY |= ResetScale || ResetHeight;
            if (Parent != null)
            {
                ResetVisible |= Parent.ResetVisible;
                ResetAlpha |= Parent.ResetAlpha;
                ResetScale |= Parent.ResetScale;
                ResetWidth |= Parent.ResetWidth;
                ResetHeight |= Parent.ResetHeight;
                ResetX |= Parent.ResetX;
                ResetY |= Parent.ResetY;
            }
            if (ResetVisible) Visible = _visible;
            if (ResetAlpha) Alpha = _alpha;
            if (ResetScale) Scale = _scale;
            if (ResetWidth) Width = _width;
            if (ResetHeight) Height = _height;
            if (ResetX) X = _x;
            if (ResetY) Y = _y;
            if (ResetVisible || ResetAlpha || ResetScale || ResetWidth || ResetHeight || ResetX || ResetY)
            {
                if (ResetX) _debugImage.X = _xTotal + 0.5f;
                if (ResetY) _debugImage.Y = _yTotal + 0.5f;
                if (ResetWidth) _debugImage.Width = _widthTotal + 0.5f;
                if (ResetHeight) _debugImage.Height = _heightTotal + 0.5f;
                if (ResetAlpha) _debugImage.Tint = Color.FromArgb((int)(255f * _alphaTotal), 255, 255, 255);
                if (ResetVisible) _debugImage.Hidden = !_visibleTotal;
                Redraw();
            }
            _children.ForEach(child => child.DrawEnd());
            OnDrawEnd();
        }

        public void Cleanup()
        {
            if (ParentViewport != null && _redrawRequested)
            {
                ParentViewport.Redraw(_updateRequested);
                _redrawRequested = false;
                _updateRequested = false;
            }
            ResetVisible = false;
            ResetAlpha = false;
            ResetWidth = false;
            ResetHeight = false;
            ResetScale = false;
            ResetX = false;
            ResetY = false;
            _children.ForEach(child => child.Cleanup());
            OnCleanup();
        }

    }
}
