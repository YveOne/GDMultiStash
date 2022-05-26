using System;
using System.IO;

namespace GDMultiStash
{
    internal static partial class Core
    {


        public enum AutoStartResult
        {
            Disabled = 0,
            AlreadyRunning = 1,
            Success = 2,
        }

        public static AutoStartResult AutoStartGame(bool ignoreDisabled = false)
        {
            if (!Config.AutoStartGD && !ignoreDisabled) return AutoStartResult.Disabled;
            if (Native.FindWindow("Grim Dawn", null) != IntPtr.Zero) return AutoStartResult.AlreadyRunning;

            Console.WriteLine("Autostarting Grim Dawn:");
            Console.WriteLine("- Command: " + Config.AutoStartGDCommand);
            Console.WriteLine("- Arguments: " + Config.AutoStartGDArguments);
            Console.WriteLine("- WorkingDir: " + Config.GamePath);
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo
            {
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                FileName = Config.AutoStartGDCommand,
                Arguments = Config.AutoStartGDArguments,
                WorkingDirectory = File.Exists(Config.AutoStartGDCommand)
                    ? Path.GetDirectoryName(Config.AutoStartGDCommand)
                    : Config.GamePath
            };
            process.StartInfo = startInfo;
            process.Start();

            return AutoStartResult.Success;
        }



    }
}
