using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GDMultiStash.Common.Overlay
{
    public class ResourceHandler
    {

        private readonly Dictionary<int, D3DHook.Hook.Common.IResource> _resourcesCache;
        private readonly List<D3DHook.Hook.Common.IResource> _resourcesQueue;

        public ResourceHandler()
        {
            _resourcesQueue = new List<D3DHook.Hook.Common.IResource>();
            _resourcesCache = new Dictionary<int, D3DHook.Hook.Common.IResource>();
        }

        public D3DHook.Hook.Common.IImageResource CreateImageResource(Bitmap bitmap)
        {
            D3DHook.Hook.Common.ImageResource res = new D3DHook.Hook.Common.ImageResource(bitmap, System.Drawing.Imaging.ImageFormat.Png);
            _resourcesQueue.Add(res);
            _resourcesCache.Add(res.UID, res);
            return res;
        }

        public D3DHook.Hook.Common.IImageResource CreateImageResource(Bitmap bitmap, System.Drawing.Imaging.ImageFormat format)
        {
            D3DHook.Hook.Common.ImageResource res = new D3DHook.Hook.Common.ImageResource(bitmap, format);
            _resourcesQueue.Add(res);
            _resourcesCache.Add(res.UID, res);
            return res;
        }

        public D3DHook.Hook.Common.IImageResource CreateImageResource(byte[] imageData, int width, int height)
        {
            D3DHook.Hook.Common.ImageResource res = new D3DHook.Hook.Common.ImageResource(imageData, width, height);
            _resourcesQueue.Add(res);
            _resourcesCache.Add(res.UID, res);
            return res;
        }

        public bool GetQueuedResources(out List<D3DHook.Hook.Common.IResource> list)
        {
            list = new List<D3DHook.Hook.Common.IResource>(_resourcesQueue);
            _resourcesQueue.Clear();
            return list.Count != 0;
        }

        public void LoadQueuedResourcesFromCache()
        {
            _resourcesQueue.Clear();
            _resourcesQueue.AddRange(_resourcesCache.Values);
        }

    }
}
