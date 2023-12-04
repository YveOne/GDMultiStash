using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using D3DHook.Interface;
using SharpDX.Direct3D;
using D3DHook.Hook.Common;

namespace D3DHook.Hook
{
    enum D3D11DeviceVTbl : short
    {
        // IUnknown
        QueryInterface = 0,
        AddRef = 1,
        Release = 2,

        // ID3D11Device
        CreateBuffer = 3,
        CreateTexture1D = 4,
        CreateTexture2D = 5,
        CreateTexture3D = 6,
        CreateShaderResourceView = 7,
        CreateUnorderedAccessView = 8,
        CreateRenderTargetView = 9,
        CreateDepthStencilView = 10,
        CreateInputLayout = 11,
        CreateVertexShader = 12,
        CreateGeometryShader = 13,
        CreateGeometryShaderWithStreamOutput = 14,
        CreatePixelShader = 15,
        CreateHullShader = 16,
        CreateDomainShader = 17,
        CreateComputeShader = 18,
        CreateClassLinkage = 19,
        CreateBlendState = 20,
        CreateDepthStencilState = 21,
        CreateRasterizerState = 22,
        CreateSamplerState = 23,
        CreateQuery = 24,
        CreatePredicate = 25,
        CreateCounter = 26,
        CreateDeferredContext = 27,
        OpenSharedResource = 28,
        CheckFormatSupport = 29,
        CheckMultisampleQualityLevels = 30,
        CheckCounterInfo = 31,
        CheckCounter = 32,
        CheckFeatureSupport = 33,
        GetPrivateData = 34,
        SetPrivateData = 35,
        SetPrivateDataInterface = 36,
        GetFeatureLevel = 37,
        GetCreationFlags = 38,
        GetDeviceRemovedReason = 39,
        GetImmediateContext = 40,
        SetExceptionMode = 41,
        GetExceptionMode = 42,
    }

    /// <summary>
    /// Direct3D 11 Hook - this hooks the SwapChain.Present to take screenshots
    /// </summary>
    internal class DXHookD3D11: BaseDXHook
    {
        const int D3D11_DEVICE_METHOD_COUNT = 43;

        public DXHookD3D11(CaptureInterface ssInterface)
            : base(ssInterface)
        {
        }

        List<IntPtr> _d3d11VTblAddresses = null;
        List<IntPtr> _dxgiSwapChainVTblAddresses = null;

        Hook<DXGISwapChain_PresentDelegate> DXGISwapChain_PresentHook = null;
        Hook<DXGISwapChain_ResizeTargetDelegate> DXGISwapChain_ResizeTargetHook = null;

        object _lock = new object();

        #region Internal device resources

        SharpDX.Direct3D11.Device _device;
        SwapChain _swapChain;
        SharpDX.Windows.RenderForm _renderForm;

        #endregion

        protected override string HookName
        {
            get
            {
                return "DXHookD3D11";
            }
        }

        public override void Hook()
        {
            this.DebugMessage("Hook: Begin");
            if (_d3d11VTblAddresses == null)
            {
                _d3d11VTblAddresses = new List<IntPtr>();
                _dxgiSwapChainVTblAddresses = new List<IntPtr>();

                #region Get Device and SwapChain method addresses
                // Create temporary device + swapchain and determine method addresses
                _renderForm = ToDispose(new SharpDX.Windows.RenderForm());
                this.DebugMessage("Hook: Before device creation");
                SharpDX.Direct3D11.Device.CreateWithSwapChain(
                    DriverType.Hardware,
                    DeviceCreationFlags.BgraSupport,
                    DXGI.CreateSwapChainDescription(_renderForm.Handle),
                    out _device,
                    out _swapChain);

                ToDispose(_device);
                ToDispose(_swapChain);

                if (_device != null && _swapChain != null)
                {
                    this.DebugMessage("Hook: Device created");
                    _d3d11VTblAddresses.AddRange(GetVTblAddresses(_device.NativePointer, D3D11_DEVICE_METHOD_COUNT));
                    _dxgiSwapChainVTblAddresses.AddRange(GetVTblAddresses(_swapChain.NativePointer, DXGI.DXGI_SWAPCHAIN_METHOD_COUNT));
                }
                else
                {
                    this.DebugMessage("Hook: Device creation failed");
                }
                #endregion
            }

            // We will capture the backbuffer here
            DXGISwapChain_PresentHook = new Hook<DXGISwapChain_PresentDelegate>(
                _dxgiSwapChainVTblAddresses[(int)DXGI.DXGISwapChainVTbl.Present],
                new DXGISwapChain_PresentDelegate(PresentHook),
                this);
            
            // We will capture target/window resizes here
            DXGISwapChain_ResizeTargetHook = new Hook<DXGISwapChain_ResizeTargetDelegate>(
                _dxgiSwapChainVTblAddresses[(int)DXGI.DXGISwapChainVTbl.ResizeTarget],
                new DXGISwapChain_ResizeTargetDelegate(ResizeTargetHook),
                this);

            /*
             * Don't forget that all hooks will start deactivated...
             * The following ensures that all threads are intercepted:
             * Note: you must do this for each hook.
             */
            DXGISwapChain_PresentHook.Activate();
            DXGISwapChain_ResizeTargetHook.Activate();

            Hooks.Add(DXGISwapChain_PresentHook);
            Hooks.Add(DXGISwapChain_ResizeTargetHook);
        }

        public override void Cleanup()
        {
            try
            {
                if (_overlayEngine != null)
                {
                    _overlayEngine.Dispose();
                    _overlayEngine = null;
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// The IDXGISwapChain.Present function definition
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
        delegate int DXGISwapChain_PresentDelegate(IntPtr swapChainPtr, int syncInterval, /* int */ SharpDX.DXGI.PresentFlags flags);

        /// <summary>
        /// The IDXGISwapChain.ResizeTarget function definition
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
        delegate int DXGISwapChain_ResizeTargetDelegate(IntPtr swapChainPtr, ref ModeDescription newTargetParameters);

        /// <summary>
        /// Hooked to allow resizing a texture/surface that is reused. Currently not in use as we create the texture for each request
        /// to support different sizes each time (as we use DirectX to copy only the region we are after rather than the entire backbuffer)
        /// </summary>
        /// <param name="swapChainPtr"></param>
        /// <param name="newTargetParameters"></param>
        /// <returns></returns>
        int ResizeTargetHook(IntPtr swapChainPtr, ref ModeDescription newTargetParameters)
        {
            // Dispose of overlay engine (so it will be recreated with correct renderTarget view size)
            if (_overlayEngine != null)
            {
                _overlayEngine.Dispose();
                _overlayEngine = null;
            }

            return DXGISwapChain_ResizeTargetHook.Original(swapChainPtr, ref newTargetParameters);
        }

        /// <summary>
        /// Our present hook that will grab a copy of the backbuffer when requested. Note: this supports multi-sampling (anti-aliasing)
        /// </summary>
        /// <param name="swapChainPtr"></param>
        /// <param name="syncInterval"></param>
        /// <param name="flags"></param>
        /// <returns>The HRESULT of the original method</returns>
        int PresentHook(IntPtr swapChainPtr, int syncInterval, SharpDX.DXGI.PresentFlags flags)
        {
            this.Frame();
            SwapChain swapChain = (SharpDX.DXGI.SwapChain)swapChainPtr;
            try
            {

                #region Draw overlay
                if (this.Config.ShowOverlay && Overlays != null)
                {
                    // Initialise Overlay Engine
                    if (_swapChainPointer != swapChain.NativePointer || _overlayEngine == null)
                    {
                        if (_overlayEngine != null) _overlayEngine.Dispose();
                        _overlayEngine = new DX11.DXOverlayEngine();
                        _overlayEngine.Initialise(swapChain);
                        _swapChainPointer = swapChain.NativePointer;
                        IsOverlayUpdatePending = true;
                        IsResourcesUpdatePending = true;
                    }
                    
                    if (IsOverlayUpdatePending)
                    {
                        IsOverlayUpdatePending = false;
                        _overlayEngine.Overlays = Overlays;
                    }

                    if (IsResourcesUpdatePending)
                    {
                        IsResourcesUpdatePending = false;
                        _overlayEngine.Resources = Resources;
                    }

                    // Draw Overlay(s)
                    foreach (var overlay in _overlayEngine.Overlays)
                    {
                        overlay.Frame();
                    }
                    _overlayEngine.Draw();
                }
                #endregion
            }
            catch (Exception e)
            {
                // If there is an error we do not want to crash the hooked application, so swallow the exception
                this.DebugMessage("PresentHook: Exeception: " + e.GetType().FullName + ": " + e.ToString());
                //return unchecked((int)0x8000FFFF); //E_UNEXPECTED
            }

            // As always we need to call the original method, note that EasyHook will automatically skip the hook and call the original method
            // i.e. calling it here will not cause a stack overflow into this function
            return DXGISwapChain_PresentHook.Original(swapChainPtr, syncInterval, flags);
        }

        IntPtr _swapChainPointer = IntPtr.Zero;

        D3DHook.Hook.DX11.DXOverlayEngine _overlayEngine;

        //IntPtr _swapChainPointer = IntPtr.Zero;

        SharpDX.WIC.ImagingFactory2 wicFactory;

        /// <summary>
        /// Copies to a stream using WIC. The format is converted if necessary.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="texture"></param>
        /// <param name="outputFormat"></param>
        /// <param name="stream"></param>
        public void ToStream(SharpDX.Direct3D11.DeviceContext context, Texture2D texture, ImageFormat outputFormat, Stream stream)
        {
            if (wicFactory == null)
                wicFactory = ToDispose(new SharpDX.WIC.ImagingFactory2());

            DataStream dataStream;
            var dataBox = context.MapSubresource(
                texture,
                0,
                0,
                MapMode.Read,
                SharpDX.Direct3D11.MapFlags.None,
                out dataStream);
            try
            {
                var dataRectangle = new DataRectangle
                {
                    DataPointer = dataStream.DataPointer,
                    Pitch = dataBox.RowPitch
                };

                var format = PixelFormatFromFormat(texture.Description.Format);

                if (format == Guid.Empty)
                    return;

                using (var bitmap = new SharpDX.WIC.Bitmap(
                    wicFactory,
                    texture.Description.Width,
                    texture.Description.Height,
                    format,
                    dataRectangle))
                {
                    stream.Position = 0;

                    SharpDX.WIC.BitmapEncoder bitmapEncoder = null;
                    switch (outputFormat)
                    {
                        case ImageFormat.Bitmap:
                            bitmapEncoder = new SharpDX.WIC.BmpBitmapEncoder(wicFactory, stream);
                            break;
                        case ImageFormat.Jpeg:
                            bitmapEncoder = new SharpDX.WIC.JpegBitmapEncoder(wicFactory, stream);
                            break;
                        case ImageFormat.Png:
                            bitmapEncoder = new SharpDX.WIC.PngBitmapEncoder(wicFactory, stream);
                            break;
                        default:
                            return;
                    }

                    try
                    {
                        using (var bitmapFrameEncode = new SharpDX.WIC.BitmapFrameEncode(bitmapEncoder))
                        {
                            bitmapFrameEncode.Initialize();
                            bitmapFrameEncode.SetSize(bitmap.Size.Width, bitmap.Size.Height);
                            var pixelFormat = format;
                            bitmapFrameEncode.SetPixelFormat(ref pixelFormat);

                            if (pixelFormat != format)
                            {
                                // IWICFormatConverter
                                using (var converter = new SharpDX.WIC.FormatConverter(wicFactory))
                                {
                                    if (converter.CanConvert(format, pixelFormat))
                                    {
                                        converter.Initialize(bitmap, SharpDX.WIC.PixelFormat.Format24bppBGR, SharpDX.WIC.BitmapDitherType.None, null, 0, SharpDX.WIC.BitmapPaletteType.MedianCut);
                                        bitmapFrameEncode.SetPixelFormat(ref pixelFormat);
                                        bitmapFrameEncode.WriteSource(converter);
                                    }
                                    else
                                    {
                                        this.DebugMessage(string.Format("Unable to convert Direct3D texture format {0} to a suitable WIC format", texture.Description.Format.ToString()));
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                bitmapFrameEncode.WriteSource(bitmap);
                            }
                            bitmapFrameEncode.Commit();
                            bitmapEncoder.Commit();
                        }
                    }
                    finally
                    {
                        bitmapEncoder.Dispose();
                    }
                }
            }
            finally
            {
                context.UnmapSubresource(texture, 0);
            }
        }


        public static Guid PixelFormatFromFormat(SharpDX.DXGI.Format format)
        {
            switch (format)
            {
                case SharpDX.DXGI.Format.R32G32B32A32_Typeless:
                case SharpDX.DXGI.Format.R32G32B32A32_Float:
                    return SharpDX.WIC.PixelFormat.Format128bppRGBAFloat;
                case SharpDX.DXGI.Format.R32G32B32A32_UInt:
                case SharpDX.DXGI.Format.R32G32B32A32_SInt:
                    return SharpDX.WIC.PixelFormat.Format128bppRGBAFixedPoint;
                case SharpDX.DXGI.Format.R32G32B32_Typeless:
                case SharpDX.DXGI.Format.R32G32B32_Float:
                    return SharpDX.WIC.PixelFormat.Format96bppRGBFloat;
                case SharpDX.DXGI.Format.R32G32B32_UInt:
                case SharpDX.DXGI.Format.R32G32B32_SInt:
                    return SharpDX.WIC.PixelFormat.Format96bppRGBFixedPoint;
                case SharpDX.DXGI.Format.R16G16B16A16_Typeless:
                case SharpDX.DXGI.Format.R16G16B16A16_Float:
                case SharpDX.DXGI.Format.R16G16B16A16_UNorm:
                case SharpDX.DXGI.Format.R16G16B16A16_UInt:
                case SharpDX.DXGI.Format.R16G16B16A16_SNorm:
                case SharpDX.DXGI.Format.R16G16B16A16_SInt:
                    return SharpDX.WIC.PixelFormat.Format64bppRGBA;
                case SharpDX.DXGI.Format.R32G32_Typeless:
                case SharpDX.DXGI.Format.R32G32_Float:
                case SharpDX.DXGI.Format.R32G32_UInt:
                case SharpDX.DXGI.Format.R32G32_SInt:
                case SharpDX.DXGI.Format.R32G8X24_Typeless:
                case SharpDX.DXGI.Format.D32_Float_S8X24_UInt:
                case SharpDX.DXGI.Format.R32_Float_X8X24_Typeless:
                case SharpDX.DXGI.Format.X32_Typeless_G8X24_UInt:
                    return Guid.Empty;
                case SharpDX.DXGI.Format.R10G10B10A2_Typeless:
                case SharpDX.DXGI.Format.R10G10B10A2_UNorm:
                case SharpDX.DXGI.Format.R10G10B10A2_UInt:
                    return SharpDX.WIC.PixelFormat.Format32bppRGBA1010102;
                case SharpDX.DXGI.Format.R11G11B10_Float:
                    return Guid.Empty;
                case SharpDX.DXGI.Format.R8G8B8A8_Typeless:
                case SharpDX.DXGI.Format.R8G8B8A8_UNorm:
                case SharpDX.DXGI.Format.R8G8B8A8_UNorm_SRgb:
                case SharpDX.DXGI.Format.R8G8B8A8_UInt:
                case SharpDX.DXGI.Format.R8G8B8A8_SNorm:
                case SharpDX.DXGI.Format.R8G8B8A8_SInt:
                    return SharpDX.WIC.PixelFormat.Format32bppRGBA;
                case SharpDX.DXGI.Format.R16G16_Typeless:
                case SharpDX.DXGI.Format.R16G16_Float:
                case SharpDX.DXGI.Format.R16G16_UNorm:
                case SharpDX.DXGI.Format.R16G16_UInt:
                case SharpDX.DXGI.Format.R16G16_SNorm:
                case SharpDX.DXGI.Format.R16G16_SInt:
                    return Guid.Empty;
                case SharpDX.DXGI.Format.R32_Typeless:
                case SharpDX.DXGI.Format.D32_Float:
                case SharpDX.DXGI.Format.R32_Float:
                case SharpDX.DXGI.Format.R32_UInt:
                case SharpDX.DXGI.Format.R32_SInt:
                    return Guid.Empty;
                case SharpDX.DXGI.Format.R24G8_Typeless:
                case SharpDX.DXGI.Format.D24_UNorm_S8_UInt:
                case SharpDX.DXGI.Format.R24_UNorm_X8_Typeless:
                    return SharpDX.WIC.PixelFormat.Format32bppGrayFloat;
                case SharpDX.DXGI.Format.X24_Typeless_G8_UInt:
                case SharpDX.DXGI.Format.R9G9B9E5_Sharedexp:
                case SharpDX.DXGI.Format.R8G8_B8G8_UNorm:
                case SharpDX.DXGI.Format.G8R8_G8B8_UNorm:
                    return Guid.Empty;
                case SharpDX.DXGI.Format.B8G8R8A8_UNorm:
                case SharpDX.DXGI.Format.B8G8R8X8_UNorm:
                    return SharpDX.WIC.PixelFormat.Format32bppBGRA;
                case SharpDX.DXGI.Format.R10G10B10_Xr_Bias_A2_UNorm:
                    return SharpDX.WIC.PixelFormat.Format32bppBGR101010;
                case SharpDX.DXGI.Format.B8G8R8A8_Typeless:
                case SharpDX.DXGI.Format.B8G8R8A8_UNorm_SRgb:
                case SharpDX.DXGI.Format.B8G8R8X8_Typeless:
                case SharpDX.DXGI.Format.B8G8R8X8_UNorm_SRgb:
                    return SharpDX.WIC.PixelFormat.Format32bppBGRA;
                case SharpDX.DXGI.Format.R8G8_Typeless:
                case SharpDX.DXGI.Format.R8G8_UNorm:
                case SharpDX.DXGI.Format.R8G8_UInt:
                case SharpDX.DXGI.Format.R8G8_SNorm:
                case SharpDX.DXGI.Format.R8G8_SInt:
                    return Guid.Empty;
                case SharpDX.DXGI.Format.R16_Typeless:
                case SharpDX.DXGI.Format.R16_Float:
                case SharpDX.DXGI.Format.D16_UNorm:
                case SharpDX.DXGI.Format.R16_UNorm:
                case SharpDX.DXGI.Format.R16_SNorm:
                    return SharpDX.WIC.PixelFormat.Format16bppGrayHalf;
                case SharpDX.DXGI.Format.R16_UInt:
                case SharpDX.DXGI.Format.R16_SInt:
                    return SharpDX.WIC.PixelFormat.Format16bppGrayFixedPoint;
                case SharpDX.DXGI.Format.B5G6R5_UNorm:
                    return SharpDX.WIC.PixelFormat.Format16bppBGR565;
                case SharpDX.DXGI.Format.B5G5R5A1_UNorm:
                    return SharpDX.WIC.PixelFormat.Format16bppBGRA5551;
                case SharpDX.DXGI.Format.B4G4R4A4_UNorm:
                    return Guid.Empty;

                case SharpDX.DXGI.Format.R8_Typeless:
                case SharpDX.DXGI.Format.R8_UNorm:
                case SharpDX.DXGI.Format.R8_UInt:
                case SharpDX.DXGI.Format.R8_SNorm:
                case SharpDX.DXGI.Format.R8_SInt:
                    return SharpDX.WIC.PixelFormat.Format8bppGray;
                case SharpDX.DXGI.Format.A8_UNorm:
                    return SharpDX.WIC.PixelFormat.Format8bppAlpha;
                case SharpDX.DXGI.Format.R1_UNorm:
                    return SharpDX.WIC.PixelFormat.Format1bppIndexed;

                default:
                    return Guid.Empty;
            }
        }
    }


}
