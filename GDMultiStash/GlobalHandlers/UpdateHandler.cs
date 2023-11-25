using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

using GDMultiStashUpdater;

namespace GDMultiStash.GlobalHandlers
{
    internal partial class UpdateHandler : Base.BaseHandler
    {

        public string NewVersionName => UpdaterAPI.NewVersionName;

        public bool ShowNewVersionAvailable()
        {
            if (!Global.Configuration.Settings.CheckForNewVersion) return false;
            return UpdaterAPI.NewVersionAvailable();
        }

        public void StartUpdater(string latestUrl = null)
        {
            string updaterPath = Path.Combine(Application.StartupPath, "GDMultiStashUpdater.exe");
            new System.Threading.Thread(new System.Threading.ThreadStart(() => {
                if (latestUrl == null)
                    Process.Start(updaterPath);
                else
                    Process.Start(updaterPath, $"\"{latestUrl}\"");
            })).Start();
            Program.Quit();
        }

    }
}
