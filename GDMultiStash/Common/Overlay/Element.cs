using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using D3DHook.Hook.Common;

namespace GDMultiStash.Common.Overlay
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

        #region Viewport 

        private Viewport _parentViewport = null;
        protected virtual Viewport ParentViewport
        {
            get
            {
                if (_parentViewport == null && Parent != null)
                    _parentViewport = Parent.ParentViewport;
                return _parentViewport;
            }
        }

        public virtual void ViewportConnected(Viewport parentViewport)
        {
            Children.ForEach(child => child.ViewportConnected(parentViewport));

            if (Debugging)
                parentViewport.OverlayResources.AsyncCreateColorImageResource(DebugColor)
                    .ResourceCreated += delegate (object sender, ResourceHandler.ResourceCreatedEventArgs args) {
                        _debugImage.ResourceUID = args.Resource.UID;
                    };
        }

        #endregion

        #region Parent & Children

        public Element Parent { get; private set; } = null;

        private List<Element> Children { get; } = new List<Element>();

        public virtual void AddChild(Element child)
        {
            // check if child is already in children list
            //if (Children.FindIndex(e => e.ID == child.ID) != -1)
            if (Children.Contains(child))
                return;
            Children.Add(child);
            child.Parent = this;
            Redraw(true);
            if (ParentViewport != null)
                child.ViewportConnected(ParentViewport);
        }

        public virtual void RemoveChild(Element child, bool destroy = true)
        {
            if (destroy) child.Destroy();
            Children.Remove(child);
            Redraw(true);
        }

        public virtual void ClearChildren()
        {
            foreach (Element child in Children)
                child.Destroy();
            Children.Clear();
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

            foreach (Element child in Children)
                imglist.AddRange(child.GetImagesRecursive());

            return imglist;
        }

        public virtual void Destroy()
        {
            MouseEnter = null;
            MouseLeave = null;
            MouseUp = null;
            MouseDown = null;
            MouseClick = null;
            Children.ForEach(child => child.Destroy());
            Children.Clear();
        }

        public virtual void Begin()
        {
            Children.ForEach(child => child.Begin());
        }

        public virtual void Draw(float ms)
        {
            Children.ForEach(child => child.Draw(ms));
        }

        public virtual void End()
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
            Children.ForEach(child => child.End());
        }

        public virtual void Cleanup()
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

            Children.ForEach(child => child.Cleanup());
        }

    }
}
