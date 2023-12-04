﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

using D3DHook.Overlay.Utils.Extensions;

namespace D3DHook.Overlay
{
    public class ResourceHandler
    {

        private const int _maxSendQueuedRessources = 20;
        private const int _maxCreateRessources = 20;

        private readonly List<D3DHook.Hook.Common.IImageResource> _unusedImageResources;
        private readonly Dictionary<int, D3DHook.Hook.Common.IResource> _resourcesCacheByUID;
        private readonly List<D3DHook.Hook.Common.IResource> _resourcesQueueToBeSend;
        private readonly List<ICreating> _resourcesCreatingQueue;

        public ResourceHandler()
        {
            _resourcesQueueToBeSend = new List<D3DHook.Hook.Common.IResource>();
            _resourcesCacheByUID = new Dictionary<int, D3DHook.Hook.Common.IResource>();
            _resourcesCreatingQueue = new List<ICreating>();
            _unusedImageResources = new List<D3DHook.Hook.Common.IImageResource>();
        }

        public void LoadQueuedResourcesFromCache()
        {
            _resourcesQueueToBeSend.Clear();
            _resourcesQueueToBeSend.AddRange(_resourcesCacheByUID.Values);
        }

        public void DeleteResource(int resUID)
        {
            if (_resourcesCacheByUID.TryGetValue(resUID, out D3DHook.Hook.Common.IResource res))
            {
                DeleteResource(res);
            }
        }

        public void DeleteResource(D3DHook.Hook.Common.IResource res)
        {
            if (res != null)
            {
                if (res is D3DHook.Hook.Common.IImageResource imgres)
                {
                    DeleteResource(imgres);
                }
                else
                {
                    // ...
                }
            }
        }

        public void DeleteResource(D3DHook.Hook.Common.IImageResource res)
        {
            if (res != null)
            {
                _unusedImageResources.Add(res);
            }
        }

        public bool CreateAndGetNewResources(out List<D3DHook.Hook.Common.IResource> list)
        {
            _resourcesCreatingQueue.RemoveAndGetRange(0, _maxCreateRessources)
                .ForEach(c => {
                    c.Callback(this, c.Create(this));
                });

            // get resources
            list = new List<D3DHook.Hook.Common.IResource>(_resourcesQueueToBeSend.RemoveAndGetRange(0, _maxSendQueuedRessources));
            return list.Count != 0;
        }

        #region creating image resources

        public delegate void CreatedCallbackInvokeDelegate(object sender, D3DHook.Hook.Common.IImageResource resource);

        public class ResourceCreatedEventArgs : EventArgs
        {
            public D3DHook.Hook.Common.IImageResource Resource { get; private set; }
            public ResourceCreatedEventArgs(D3DHook.Hook.Common.IImageResource resource)
            {
                Resource = resource;
            }
        }

        public class CreatedCallback
        {
            public EventHandler<ResourceCreatedEventArgs> ResourceCreated;
            public CreatedCallback(out CreatedCallbackInvokeDelegate invoke)
            {
                invoke = (object sender, D3DHook.Hook.Common.IImageResource resource) => {
                    ResourceCreated?.Invoke(sender, new ResourceCreatedEventArgs(resource));
                };
            }
        }

        private interface ICreating
        {
            CreatedCallbackInvokeDelegate Callback { get; }
            D3DHook.Hook.Common.IImageResource Create(ResourceHandler rh);
        }

        private class ImageCreating : ICreating
        {
            private Image _image;
            private ImageFormat _format;
            public CreatedCallbackInvokeDelegate Callback { get; private set; } = null;
            public ImageCreating(CreatedCallbackInvokeDelegate cb, Image image, ImageFormat format)
            {
                _image = image;
                _format = format;
                Callback = cb;
            }

            public D3DHook.Hook.Common.IImageResource Create(ResourceHandler rh)
            {
                return rh.CreateImageResource(_image, _format);
            }

        }

        public CreatedCallback AsyncCreateImageResource(Image image, ImageFormat format)
        {
            CreatedCallback cb = new CreatedCallback(out CreatedCallbackInvokeDelegate invoker);
            _resourcesCreatingQueue.Add(new ImageCreating(invoker, image, format));
            return cb;
        }

        public D3DHook.Hook.Common.IImageResource CreateImageResource(Image image, ImageFormat format)
        {
            D3DHook.Hook.Common.IImageResource res;
            if (_unusedImageResources.Count > 0)
            {
                res = _unusedImageResources[0];
                _unusedImageResources.RemoveAt(0);
                res.SetImage(image, format);
            }
            else
            {
                res = new D3DHook.Hook.Common.ImageResource(new Bitmap(image), format);
                _resourcesCacheByUID.Add(res.UID, res);
            }
            _resourcesQueueToBeSend.Add(res);
            return res;
        }

        #endregion

        #region creating color image region

        private class ColorCreating : ICreating
        {
            private Color _color;
            public CreatedCallbackInvokeDelegate Callback { get; private set; } = null;
            public ColorCreating(CreatedCallbackInvokeDelegate cb, Color color)
            {
                _color = color;
                Callback = cb;
            }

            public D3DHook.Hook.Common.IImageResource Create(ResourceHandler rh)
            {
                return rh.CreateColorImageResource(_color);
            }

        }

        public CreatedCallback AsyncCreateColorImageResource(Color color)
        {
            CreatedCallback cb = new CreatedCallback(out CreatedCallbackInvokeDelegate invoker);
            _resourcesCreatingQueue.Add(new ColorCreating(invoker, color));
            return cb;
        }

        public D3DHook.Hook.Common.IImageResource CreateColorImageResource(Color color)
        {
            using (Bitmap bmp = new Bitmap(1, 1, PixelFormat.Format32bppArgb))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    using (var brush = new SolidBrush(color))
                    {
                        g.FillRectangle(brush, 0, 0, bmp.Width, bmp.Height);
                    }
                }
                return CreateImageResource(bmp, ImageFormat.Png);
            }
        }

        #endregion

        #region creating text image resources

        private class TextCreating : ICreating
        {
            private string _text;
            private Font _font;
            private int _width;
            private int _height;
            private Color _color;
            private StringAlignment _align;
            public CreatedCallbackInvokeDelegate Callback { get; private set; } = null;
            public TextCreating(CreatedCallbackInvokeDelegate cb, string text, Font font, int width, int height, Color color, StringAlignment align)
            {
                _text = text;
                _font = font;
                _width = width;
                _height = height;
                _color = color;
                _align = align;
                Callback = cb;
            }

            public D3DHook.Hook.Common.IImageResource Create(ResourceHandler rh)
            {
                return rh.CreateTextImageResource(_text, _font, _width, _height, _color, _align);
            }

        }

        public CreatedCallback AsyncCreateTextImageResource(string text, Font font, int width, int height, Color color, StringAlignment align)
        {
            CreatedCallback cb = new CreatedCallback(out CreatedCallbackInvokeDelegate invoker);
            _resourcesCreatingQueue.Add(new TextCreating(invoker, text, font, width, height, color, align));
            return cb;
        }

        public D3DHook.Hook.Common.IImageResource CreateTextImageResource(string text, Font font, int width, int height, Color color, StringAlignment align)
        {
            using (Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    var textBounds = new Rectangle(0, 0, bmp.Width, bmp.Height);
                    var textFormat = new StringFormat()
                    {
                        Alignment = align,
                        LineAlignment = StringAlignment.Center,
                        Trimming = StringTrimming.None,
                        FormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.NoClip,
                    };
                    using (var brush = new SolidBrush(color))
                    {
                        //g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                        //g.DrawString(text, font, brush, textBounds, textFormat);
                        g.DrawString(text, font, brush, textBounds, textFormat);
                    }
                }
                return CreateImageResource(bmp, ImageFormat.Png);
            }
        }

        #endregion

    }
}
