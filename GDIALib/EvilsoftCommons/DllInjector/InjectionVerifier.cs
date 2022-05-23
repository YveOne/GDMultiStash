using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace GDIALib.EvilsoftCommons.DllInjector
{
    /// <summary>
    /// Runs the Microsoft "Listdlls.exe" to verify that the DLL injection was successful.
    /// Sometimes the injection reports as successful, but the DLL does not persist. (unloaded by anti virus?)
    /// </summary>
    public class InjectionVerifier
    {


        /// <summary>
        /// Remove nag screens on running ListDLLs
        /// </summary>
        public static void FixRegistryNagOnListDlls()
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey("Software", true);


                key.CreateSubKey("Sysinternals");
                key = key.OpenSubKey("Sysinternals", true);

                key.CreateSubKey("ListDLLs");
                key = key.OpenSubKey("ListDLLs", true);

                key.SetValue("EulaAccepted", 1);

                key.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error trying to create registry keys, this is not critical.");
                Console.WriteLine(ex.Message);
            }
        }

        public static bool VerifyInjection(long pid, string dll)
        {
            FixRegistryNagOnListDlls();

            Console.WriteLine("[GDHook] Running Listdlls...");
            if (File.Exists("Listdlls.exe"))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "Listdlls.exe",
                    Arguments = $"{pid}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                Process processTemp = new Process();
                processTemp.StartInfo = startInfo;
                processTemp.EnableRaisingEvents = true;
                try
                {
                    string spid = pid.ToString();
                    processTemp.Start();
                    processTemp.WaitForExit(3000);


                    List<string> output = new List<string>();
                    while (!processTemp.StandardOutput.EndOfStream)
                    {
                        string line = processTemp.StandardOutput.ReadLine();
                        output.Add(line);
                        if (line.Contains(dll))
                            return true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[GDHook] Exception while attempting to verify injection.. " + ex.Message + ex.StackTrace);
                }
            }
            else
            {
                Console.WriteLine("[GDHook] Could not find Listdlls.exe, unable to verify successful injection.");
            }
            return false;
        }

    }
}
