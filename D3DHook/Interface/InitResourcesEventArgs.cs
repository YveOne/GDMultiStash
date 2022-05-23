using D3DHook.Hook.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3DHook.Interface
{
    [Serializable]
    public class InitResourcesEventArgs
    {
        public List<IResource> Resources { get; set; }
        public InitResourcesEventArgs()
        {
        }
    }
}