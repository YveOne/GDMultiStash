using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

using GDMultiStashUpdater;

namespace GDMultiStash.GlobalHandlers
{
    internal class UpdateHandler
    {

        public string NewVersionName => UpdaterAPI.NewVersionName;

        public bool NewVersionAvailable()
        {
            if (!Global.Configuration.Settings.CheckForNewVersion) return false;
            return UpdaterAPI.NewVersionAvailable();
        }

        public void StartUpdater()
        {
            string updaterPath = Path.Combine(Application.StartupPath, "GDMultiStashUpdater.exe");
            new System.Threading.Thread(new System.Threading.ThreadStart(() => {
                Process.Start(updaterPath);
            })).Start();
            Program.Quit();
        }

    }
}
