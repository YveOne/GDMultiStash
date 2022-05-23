using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMultiStash.Services
{
    internal abstract class Service : IDisposable
	{

		public bool Running { get; private set; }

		public Service()
        {

        }

		public virtual bool Start()
        {
			if (Running) return false;
			Running = true;
			return true;
        }

		public virtual bool Stop()
		{
			if (!Running) return false;
			Running = false;
			return true;
		}

		#region IDisposable

		private bool disposed = false;

		public void Dispose()
		{
			Stop();
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					// managed resources (anything within the .NET sandbox. This includes all .NET Framework classes.)
				}
				// unmanaged resources (anything that is returned to you through calls to Win32 API functions.)
				disposed = true;
			}
		}
		
		~Service()
		{
			Dispose(false);
		}

		#endregion

	}
}
