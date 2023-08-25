using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

using Utils.Extensions;

namespace D3DHook.Overlay
{
    public class ResourceHandler
    {

        private const int _maxSendQueuedRessources = 20;
        private const int _maxCreateRessources = 20;

        private readonly List<Hook.Common.ImageResource> _unusedResources;
        private readonly Dictionary<int, Hook.Common.Resource> _resourcesCacheByUID;
        private readonly List<Hook.Common.Resource> _resourcesQueueToBeSend;
        private readonly List<ICreating> _resourcesCreatingQueue;

        public ResourceHandler()
        {
            _resourcesQueueToBeSend = new List<Hook.Common.Resource>();
            _resourcesCacheByUID = new Dictionary<int, Hook.Common.Resource>();
            _resourcesCreatingQueue = new List<ICreating>();
            _unusedResources = new List<Hook.Common.ImageResource>();
        }

        public void LoadQueuedResourcesFromCache()
        {
            _resourcesQueueToBeSend.Clear();
            _resourcesQueueToBeSend.AddRange(_resourcesCacheByUID.Values);
        }

        public void DeleteResource(Hook.Common.IImageResource res)
        {
            if (res == null) return;
            if (!_unusedResources.Contains(res))
            {
                if (_resourcesCacheByUID[res.UID] is Hook.Common.ImageResource img)
                {
                    _unusedResources.Add(img);
                    //_resourcesQueueToBeSend.Remove(img);
                }
            }
        }

        public bool CreateAndGetNewResources(out List<Hook.Common.IResource> list)
        {
            _resourcesCreatingQueue.RemoveAndGetRange(0, _maxCreateRessources)
                .ForEach(c => {
                    c.Callback(this, c.Create(this));
                });

            // get resources
            list = new List<Hook.Common.IResource>(_resourcesQueueToBeSend.RemoveAndGetRange(0, _maxSendQueuedRessources));
            return list.Count != 0;
        }

        #region creating image resources

        public delegate void CreatedCallbackInvokeDelegate(object sender, Hook.Common.IImageResource resource);

        public class ResourceCreatedEventArgs : EventArgs
        {
            public Hook.Common.IImageResource Resource { get; private set; }
            public ResourceCreatedEventArgs(Hook.Common.IImageResource resource)
            {
                Resource = resource;
            }
        }

        public class CreatedCallback
        {
            public EventHandler<ResourceCreatedEventArgs> ResourceCreated;
            public CreatedCallback(out CreatedCallbackInvokeDelegate invoke)
            {
                invoke = (object sender, Hook.Common.IImageResource resource) => {
                    ResourceCreated?.Invoke(sender, new ResourceCreatedEventArgs(resource));
                };
            }
        }

        private interface ICreating
        {
            CreatedCallbackInvokeDelegate Callback { get; }
            Hook.Common.IImageResource Create(ResourceHandler rh);
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

            public Hook.Common.IImageResource Create(ResourceHandler rh)
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

        public Hook.Common.IImageResource CreateImageResource(Image image, ImageFormat format)
        {
            Hook.Common.ImageResource res;
            if (_unusedResources.Count > 0)
            {
                res = _unusedResources[0];
                _unusedResources.RemoveAt(0);
                res.SetImage(image, format);
            }
            else
            {
                res = new Hook.Common.ImageResource(new Bitmap(image), format);
                _resourcesCacheByUID.Add(res.UID, res);
            }
            _resourcesQueueToBeSend.Add(res);
            return res;
        }

        /*
        public D3DHook.Hook.Common.IImageResource CreateImageResource(byte[] imageData, int width, int height)
        {
            D3DHook.Hook.Common.ImageResource res = new D3DHook.Hook.Common.ImageResource(imageData, width, height);
            _resourcesQueueToBeSend.Add(res);
            _resourcesCacheByUID.Add(res.UID, res);
            return res;
        }
        */

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

            public Hook.Common.IImageResource Create(ResourceHandler rh)
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

        public Hook.Common.IImageResource CreateColorImageResource(Color color)
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

            public Hook.Common.IImageResource Create(ResourceHandler rh)
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

        public Hook.Common.IImageResource CreateTextImageResource(string text, Font font, int width, int height, Color color, StringAlignment align)
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
