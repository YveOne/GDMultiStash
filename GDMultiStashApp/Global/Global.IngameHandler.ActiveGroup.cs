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

        public class ActiveGroupChangedEventArgs : EventArgs
        {
            public int OldID { get; private set; }
            public int NewID { get; private set; }
            public ActiveGroupChangedEventArgs(int oldId, int newID)
            {
                OldID = oldId;
                NewID = newID;
            }
        }

        public event EventHandler<ActiveGroupChangedEventArgs> ActiveGroupChanged;

        public void InvokeActiveGroupChanged(int oldId, int newId)
            => SaveInvoke(() => ActiveGroupChanged?.Invoke(null, new ActiveGroupChangedEventArgs(oldId, newId)));

        private int _activeGroupID = 0;

        public int ActiveGroupID
        {
            get => _activeGroupID;
            set
            {
                if (value == _activeGroupID) return;
                int _previousID = _activeGroupID;
                _activeGroupID = value;
                Console.WriteLine($"ActiveGroupID: #{_activeGroupID}");
                InvokeActiveGroupChanged(_previousID, _activeGroupID);
            }
        }

    }
}
