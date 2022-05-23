using D3DHook.Hook.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3DHook.Interface
{
    /// <summary>
    /// Note: Deliberately not using MarshalByRefObj
    /// </summary>
    [Serializable]
    public class DrawOverlayEventArgs
    {
        public IOverlay Overlay { get; set; }

        public bool IsUpdatePending { get; set; }

        public DrawOverlayEventArgs()
        {
        }
    }
}