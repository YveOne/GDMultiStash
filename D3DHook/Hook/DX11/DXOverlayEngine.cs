using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3DHook.Hook.Common;
using SharpDX.Direct3D11;
using SharpDX;
using System.Diagnostics;

namespace D3DHook.Hook.DX11
{
    internal class DXOverlayEngine: Component
    {
        public List<IOverlay> Overlays { get; set; }
        //public List<IResource> Resources { get; set; }
        public Dictionary<int, IResource> Resources { get; set; }
        public bool DeferredContext
        {
            get
            {
                return _deviceContext.TypeInfo == DeviceContextType.Deferred;
            }
        }

        bool _initialised = false;
        bool _initialising = false;

        Device _device;
        DeviceContext _deviceContext;
        Texture2D _renderTarget;
        RenderTargetView _renderTargetView;
        DXSprite _spriteEngine;
        Dictionary<int, DXImage> _imageCache = new Dictionary<int, DXImage>();

        public DXOverlayEngine()
        {
            Overlays = new List<IOverlay>();
            //Resources = new List<IResource>();
            Resources = new Dictionary<int, IResource>();
        }

        private void EnsureInitiliased()
        {
            if (!_initialised)
                throw new InvalidOperationException("DXOverlayEngine must be initialised.");
        }

        public bool Initialise(SharpDX.DXGI.SwapChain swapChain)
        {
            return Initialise(swapChain.GetDevice<Device>(), swapChain.GetBackBuffer<Texture2D>(0));
        }

        public bool Initialise(Device device, Texture2D renderTarget)
        {
            if (_initialising)
                return false;

            _initialising = true;
            
            try
            {

                _device = device;
                _renderTarget = renderTarget;
                try
                {
                    _deviceContext = ToDispose(new DeviceContext(_device));
                }
                catch (SharpDXException)
                {
                    _deviceContext = _device.ImmediateContext;
                }

                _renderTargetView = ToDispose(new RenderTargetView(_device, _renderTarget));

                //if (DeferredContext)
                //{
                //    ViewportF[] viewportf = { new ViewportF(0, 0, _renderTarget.Description.Width, _renderTarget.Description.Height, 0, 1) };
                //    _deviceContext.Rasterizer.SetViewports(viewportf);
                //    _deviceContext.OutputMerger.SetTargets(_renderTargetView);
                //}

                _spriteEngine = new DXSprite(_device, _deviceContext);
                if (!_spriteEngine.Initialize())
                    return false;

                _initialised = true;
                return true;
            }
            finally
            {
                _initialising = false;
            }
        }

        private void Begin()
        {
            //if (!DeferredContext)
            //{
                SharpDX.Mathematics.Interop.RawViewportF[] viewportf = { new ViewportF(0, 0, _renderTarget.Description.Width, _renderTarget.Description.Height, 0, 1) };
                _deviceContext.Rasterizer.SetViewports(viewportf);
                _deviceContext.OutputMerger.SetTargets(_renderTargetView);
            //}
        }

        /// <summary>
        /// Draw the overlay(s)
        /// </summary>
        public void Draw()
        {
            EnsureInitiliased();

            Begin();

            
            foreach (var res in Resources.Values)
            {
                if (res is ImageResource imageResource)
                {
                    GetImageForImageResource(imageResource);
                }
            }
            

            foreach (var overlay in Overlays)
            {
                if (overlay.Hidden)
                    continue;

                foreach (var element in overlay.Elements)
                {
                    if (element.Hidden)
                        continue;

                    var imageElement = element as ImageElement;
                    if (imageElement != null)
                    {

                        DXImage image = GetImageForImageElement(imageElement);

                        if (image != null)
                            _spriteEngine.DrawImage(
                                image,
                                (int)imageElement.X,
                                (int)imageElement.Y, 
                                (int)(imageElement.Width),
                                (int)(imageElement.Height),
                                imageElement.Tint,
                                imageElement.Angle,
                                imageElement.ZIndex
                            );
                    }
                    /*

                    var textElement = element as TextElement;
                    var imageElement = element as ImageElement;

                    if (textElement != null)
                    {
                        DXFont font = GetFontForTextElement(textElement);
                        if (font != null && !String.IsNullOrEmpty(textElement.Text))
                            _spriteEngine.DrawString(textElement.Location.X, textElement.Location.Y, textElement.Text, textElement.Color, font);
                    }
                    else if (imageElement != null)
                    {
                        DXImage image = GetImageForImageElement(imageElement);
                        if (image != null)
                            _spriteEngine.DrawImage(imageElement.Location.X, imageElement.Location.Y, imageElement.Scale, imageElement.Angle, imageElement.Tint, image);
                    }
                    */
                }
            }

            End();
        }

        private void End()
        {
            if (DeferredContext)
            {
                var commandList = _deviceContext.FinishCommandList(true);
                _device.ImmediateContext.ExecuteCommandList(commandList, true);
                commandList.Dispose();
            }
        }

        DXImage GetImageForImageElement(ImageElement img)
        {
            _imageCache.TryGetValue(img.ResourceUID, out DXImage result);
            return result;
            //return _imageCache[img.ResourceUID]; // DONT DO THIS!
        }

        DXImage GetImageForImageResource(ImageResource res)
        {
            if (res.ReCreate)
            {
                _imageCache.Remove(res.UID);
            }
            if (!_imageCache.TryGetValue(res.UID, out DXImage result))
            {
                System.Drawing.Bitmap bmp = res.Bitmap;
                if (bmp == null) return result;
                result = ToDispose(new DXImage(_device, _deviceContext));
                result.Initialise(bmp);
                bmp.Dispose();
                _imageCache[res.UID] = result;
            }
            return result;
        }

        /// <summary>
        /// Releases unmanaged and optionally managed resources
        /// </summary>
        /// <param name="disposing">true if disposing both unmanaged and managed</param>
        protected override void Dispose(bool disposing)
        {
            if (true)
            {
                _device = null;
            }
        }

        void SafeDispose(DisposeBase disposableObj)
        {
            if (disposableObj != null)
                disposableObj.Dispose();
        }
    }
}
