using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace GDMultiStash.Global
{
    using GDMultiStashUpdater;
    using Update;
    namespace Update
    {

    }

    internal partial class UpdateManager : Base.Manager
    {

        public string NewVersionName => UpdaterAPI.NewVersionName;

        public bool ShowNewVersionAvailable()
        {
            if (!G.Configuration.Settings.CheckForNewVersion) return false;
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
