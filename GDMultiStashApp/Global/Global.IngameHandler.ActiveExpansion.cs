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

        public class ActiveExpansionChangedEventArgs : EventArgs
        {
            public GrimDawnGameExpansion Expansion { get => _expansion; }
            private GrimDawnGameExpansion _expansion;
            public ActiveExpansionChangedEventArgs(GrimDawnGameExpansion expansion)
            {
                _expansion = expansion;
            }
        }

        public event EventHandler<ActiveExpansionChangedEventArgs> ActiveExpansionChanged;

        public void InvokeActiveExpansionChanged(GrimDawnGameExpansion exp)
            => SaveInvoke(() => ActiveExpansionChanged?.Invoke(null, new ActiveExpansionChangedEventArgs(exp)));

        private GrimDawnGameExpansion _activeExpansion = GrimDawnGameExpansion.Unknown;

        public GrimDawnGameExpansion ActiveExpansion
        {
            get => _activeExpansion;
            set
            {
                if (_activeExpansion == value) return;
                _activeExpansion = value;
                Console.WriteLine($"ActiveExpansion: {_activeExpansion}");
                InvokeActiveExpansionChanged(_activeExpansion);
            }
        }

    }
}
