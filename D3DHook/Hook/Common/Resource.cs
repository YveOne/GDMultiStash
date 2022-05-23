using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D3DHook.Hook.Common
{
    [Serializable]
    public class Resource : IResource, IDisposable
    {

        private static int lastUid = 0;

        public int UID { get; private set; }

        private static int GetNextUID()
        {
            lastUid += 1;
            return lastUid;
        }

        public Resource()
        {
            UID = GetNextUID();
        }

        public virtual object Clone()
        {
            return MemberwiseClone();
        }









        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and optionally managed resources
        /// </summary>
        /// <param name="disposing">true if disposing both unmanaged and managed</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }

        protected void SafeDispose(IDisposable disposableObj)
        {
            if (disposableObj != null)
                disposableObj.Dispose();
        }

    }

}
