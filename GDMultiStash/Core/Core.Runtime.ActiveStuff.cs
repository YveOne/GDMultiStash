using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GrimDawnLib;

namespace GDMultiStash
{
    internal static partial class Core
    {
        internal static partial class Runtime
        {

            #region ActiveStash

            public class ActiveStashChangedEventArgs : EventArgs
            {
                public int OldID { get => _oldId; }
                private int _oldId;
                public int NewID { get => _newId; }
                private int _newId;
                public ActiveStashChangedEventArgs(int oldId, int newID)
                {
                    _oldId = oldId;
                    _newId = newID;
                }
            }

            public delegate void ActiveStashChangedEventHandler(object sender, ActiveStashChangedEventArgs e);
            public static event ActiveStashChangedEventHandler ActiveStashChanged;

            private static int _activeStashID = -1;

            public static int ActiveStashID
            {
                get => _activeStashID;
                set
                {
                    if (value == _activeStashID) return;
                    int _previousStashID = _activeStashID;
                    _activeStashID = value;
                    Config.SetCurrentStashID(CurrentExpansion, CurrentMode, _activeStashID);
                    Console.WriteLine("Runtime: Active stash changed to #" + _activeStashID);
                    ActiveStashChanged?.Invoke(null, new ActiveStashChangedEventArgs(_previousStashID, _activeStashID));
                }
            }

            public static void LoadActiveStashID()
            {
                ActiveStashID = Config.GetCurrentStashID(CurrentExpansion, CurrentMode);
            }

            #endregion

            #region ActiveMode

            public class ActiveModeChangedEventArgs : EventArgs
            {
                public GrimDawnGameMode Mode { get => _mode; }
                private GrimDawnGameMode _mode;
                public ActiveModeChangedEventArgs(GrimDawnGameMode mode)
                {
                    _mode = mode;
                }
            }

            public delegate void ActiveModeChangedEventHandler(object sender, ActiveModeChangedEventArgs e);
            public static event ActiveModeChangedEventHandler ActiveModeChanged;

            private static GrimDawnGameMode _activeMode = GrimDawnGameMode.None;

            public static GrimDawnGameMode CurrentMode
            {
                get => _activeMode;
                set
                {
                    if (_activeMode == value) return;
                    _activeMode = value;
                    Console.WriteLine("Runtime: CurrentModeChanged: " + _activeMode);
                    ActiveModeChanged?.Invoke(null, new ActiveModeChangedEventArgs(_activeMode));
                }
            }

            #endregion

            #region ActiveExpansion

            public class ActiveExpansionChangedEventArgs : EventArgs
            {
                public GrimDawnGameExpansion Expansion { get => _expansion; }
                private GrimDawnGameExpansion _expansion;
                public ActiveExpansionChangedEventArgs(GrimDawnGameExpansion expansion)
                {
                    _expansion = expansion;
                }
            }

            public delegate void ActiveExpansionChangedEventHandler(object sender, ActiveExpansionChangedEventArgs e);
            public static event ActiveExpansionChangedEventHandler ActiveExpansionChanged;

            private static GrimDawnGameExpansion _currentExpansion = GrimDawnGameExpansion.Unknown;

            public static GrimDawnGameExpansion CurrentExpansion
            {
                get => _currentExpansion;
                set
                {
                    if (_currentExpansion == value) return;
                    _currentExpansion = value;
                    Console.WriteLine("Runtime: CurrentExpansionChanged: " + _currentExpansion);
                    ActiveExpansionChanged?.Invoke(null, new ActiveExpansionChangedEventArgs(_currentExpansion));
                }
            }

            #endregion

        }
    }
}
