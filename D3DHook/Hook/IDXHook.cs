using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3DHook.Interface;

namespace D3DHook.Hook
{
    internal interface IDXHook : IDisposable
    {
        CaptureInterface Interface
        {
            get;
            set;
        }
        CaptureConfig Config
        {
            get;
            set;
        }

        void Hook();

        void Cleanup();
    }
}