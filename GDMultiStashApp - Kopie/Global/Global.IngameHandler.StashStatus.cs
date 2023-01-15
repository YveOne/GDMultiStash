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

        //public event EventHandler<EventArgs> StashStatusChanged;
        public event EventHandler<EventArgs> StashOpened;
        public event EventHandler<EventArgs> StashClosed;
        public event EventHandler<EventArgs> TransferFileSaved;

        //public void InvokeStashStatusChanged()
        //    => StashStatusChanged?.Invoke(null, EventArgs.Empty);

        public void InvokeStashOpened()
            => SaveInvoke(() => StashOpened?.Invoke(null, EventArgs.Empty));

        public void InvokeStashClosed()
            => SaveInvoke(() => StashClosed?.Invoke(null, EventArgs.Empty));

        public void InvokeTransferStashSaved()
        {
            if (!_stashWasOpened) return;
            _stashWasOpened = false;
            TransferFileSaved?.Invoke(null, EventArgs.Empty);
        }

        private bool _stashIsOpened = false;
        private bool _stashWasOpened = false; // this is used to trigger NotifyTransferStashSaved() only once

        public bool StashIsOpened
        {
            get { return _stashIsOpened; }
            set
            {
                if (_stashIsOpened == value) return;
                _stashIsOpened = value;
                _stashWasOpened = _stashWasOpened || value;
                Console.WriteLine($"StashIsOpened: {value} (reopening: {StashIsReopening})");
                if (!StashIsReopening)
                {
                    //InvokeStashStatusChanged();
                    if (_stashIsOpened) InvokeStashOpened();
                    else InvokeStashClosed();
                }
            }
        }

    }
}