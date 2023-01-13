using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using GrimDawnLib;
using GDMultiStash.Common.Objects;

namespace GDMultiStash.GlobalHandlers
{
    internal partial class IngameHandler
    {

        public class ActiveStashChangedEventArgs : EventArgs
        {
            public int OldID { get; private set; }
            public int NewID { get; private set; }
            public ActiveStashChangedEventArgs(int oldId, int newID)
            {
                OldID = oldId;
                NewID = newID;
            }
        }

        public event EventHandler<ActiveStashChangedEventArgs> ActiveStashChanged;

        public void InvokeActiveStashChanged(int oldId, int newId)
            => SaveInvoke(() => ActiveStashChanged?.Invoke(null, new ActiveStashChangedEventArgs(oldId, newId)));
        
        private int _activeStashID = -1;

        public int ActiveStashID
        {
            get => _activeStashID;
            set
            {
                if (value == _activeStashID) return;
                int _previousID = _activeStashID;
                _activeStashID = value;
                Global.Configuration.SetCurrentStashID(ActiveExpansion, ActiveMode, _activeStashID);
                Console.WriteLine($"ActiveStashID: #{_activeStashID}");
                InvokeActiveStashChanged(_previousID, _activeStashID);
            }
        }

        public void LoadActiveStashID()
        {
            if (ActiveExpansion == GrimDawnGameExpansion.Unknown) return;
            if (ActiveMode == GrimDawnGameMode.None) return;
            ActiveStashID = Global.Configuration.GetCurrentStashID(ActiveExpansion, ActiveMode);
        }

    }
}
