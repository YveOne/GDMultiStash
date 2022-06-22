using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

using GDMultiStashUpdater;

namespace GDMultiStash
{
    internal static partial class Core
    {
        internal static partial class Update
        {

            private static readonly long checkDelay = 3600 * 1; // in seconds

            public static string NewVersionName => UpdaterAPI.NewVersionName;

            public static bool NewVersionAvailable()
            {
                if (!Config.CheckForNewVersion) return false;

                long timestamp = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
                if (Config.LastVersionCheck > timestamp - checkDelay) return false;
                Config.LastVersionCheck = timestamp;
                Config.Save();

                return UpdaterAPI.NewVersionAvailable();
            }

            public static void StartUpdater()
            {
                string updaterPath = Path.Combine(Application.StartupPath, "GDMultiStashUpdater.exe");

                new System.Threading.Thread(new System.Threading.ThreadStart(() => {
                    Process.Start(updaterPath);
                })).Start();
                Program.Quit();
            }

        }
    }
}
