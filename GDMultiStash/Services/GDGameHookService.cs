using System;
using System.Collections.Generic;
using System.Windows.Forms;

using GDIALib.GDHook;

namespace GDMultiStash.Services
{
    internal class GDGameHookService : Service
    {

        public class StashStatusChangedEventArgs : EventArgs
        {
            public bool Opened;
        }

        public delegate void StashStatusChangedEventHandler(object sender, StashStatusChangedEventArgs e);
        public event StashStatusChangedEventHandler StashStatusChanged;

        public class ModeChangedEventArgs : EventArgs
        {
            public bool IsHardcore;
        }

        public delegate void ModeChangedEventHandler(object sender, ModeChangedEventArgs e);
        public event ModeChangedEventHandler ModeStatusChanged;

        public class ExpansionChangedEventArgs : EventArgs
        {
            public int ExpansionID;
        }

        public delegate void ExpansionChangedEventHandler(object sender, ExpansionChangedEventArgs e);
        public event ExpansionChangedEventHandler ExpansionChanged;

        private readonly InjectionTargetForm _targetForm;

        public GDGameHookService()
        {
            _targetForm = new InjectionTargetForm();
            RuntimeSettings.StashStatusChanged += delegate (object sender, StashAvailability status)
            {
                StashStatusChanged?.Invoke(sender, new StashStatusChangedEventArgs
                {
                    Opened = status != StashAvailability.CLOSED,
                });
            };
            RuntimeSettings.ModeChanged += delegate (object sender, bool ishc)
            {
                ModeStatusChanged?.Invoke(sender, new ModeChangedEventArgs
                {
                    IsHardcore = ishc,
                });
            };
            RuntimeSettings.ExpansionChanged += delegate (object sender, int expID)
            {
                ExpansionChanged?.Invoke(sender, new ExpansionChangedEventArgs
                {
                    ExpansionID = expID,
                });
            };
        }

        public override bool Start()
        {
            if (Running) return false;
            _targetForm.StartInjector();
            return base.Start();
        }

        public override bool Stop()
        {
            if (!Running) return false;
            _targetForm.Destroy();
            return base.Stop();
        }

    }
}
