using System;

namespace GDIALib.GDHook
{

    public static class RuntimeSettings
    {

        public delegate void StashStatusChangedEventHandler(object sender, StashAvailability status);
        public static event StashStatusChangedEventHandler StashStatusChanged;

        public delegate void ModeChangedEventHandler(object sender, bool isHC);
        public static event ModeChangedEventHandler ModeChanged;

        public delegate void ExpansionChangedEventHandler(object sender, int expID);
        public static event ExpansionChangedEventHandler ExpansionChanged;

        public delegate void FatalErrorEventHandler(object sender, string message);
        public static event FatalErrorEventHandler FatalError;

        public delegate void TransferStashSavedEventHandler(object sender);
        public static event TransferStashSavedEventHandler TransferStashSaved;

        private static volatile StashAvailability _stashStatus = StashAvailability.UNKNOWN;
        private static volatile bool _isHC = false;
        private static volatile bool _isHCKnown = false;
        private static volatile int _expID = -1;

        public static void NoticeTransferStashSaved()
        {
            TransferStashSaved?.Invoke(null);
        }

        public static StashAvailability StashStatus
        {
            get => _stashStatus;
            set
            {
                if (_stashStatus == value) return;
                _stashStatus = value;
                StashStatusChanged?.Invoke(null, value);
            }
        }

        public static bool IsHardcore
        {
            get => _isHC;
            set
            {
                if (_isHC == value && _isHCKnown) return;
                _isHCKnown = true;
                _isHC = value;
                ModeChanged?.Invoke(null, value);
            }
        }

        public static int Expansion
        {
            get => _expID;
            set
            {
                if (_expID == value) return;
                _expID = value;
                ExpansionChanged?.Invoke(null, value);
            }
        }

        public static void ThrowFatalError(string message)
        {
            FatalError?.Invoke(null, message);
        }

    }
}
