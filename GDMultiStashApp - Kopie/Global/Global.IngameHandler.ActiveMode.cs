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

        public class ActiveModeChangedEventArgs : EventArgs
        {
            public GrimDawnGameMode Mode { get => _mode; }
            private GrimDawnGameMode _mode;
            public ActiveModeChangedEventArgs(GrimDawnGameMode mode)
            {
                _mode = mode;
            }
        }

        public event EventHandler<ActiveModeChangedEventArgs> ActiveModeChanged;

        public void InvokeActiveModeChanged(GrimDawnGameMode mode)
            => SaveInvoke(() => ActiveModeChanged?.Invoke(null, new ActiveModeChangedEventArgs(mode)));

        private GrimDawnGameMode _activeMode = GrimDawnGameMode.None;

        public GrimDawnGameMode ActiveMode
        {
            get => _activeMode;
            set
            {
                if (_activeMode == value) return;
                _activeMode = value;
                Console.WriteLine($"ActiveMode: {_activeMode}");
                InvokeActiveModeChanged(_activeMode);
            }
        }

    }
}
