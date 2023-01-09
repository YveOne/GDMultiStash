
using System.Diagnostics;
using System.Reflection;
using System.IO;
using System.Runtime.InteropServices;

namespace GDMultiStashBuild
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Normal,
                    FileName = "Release\\GDMultiStashDebug.exe",
                    UseShellExecute = false,
                    WorkingDirectory = "Release",
                }
            })
            {
                process.Start();
                process.WaitForExit();
            }
        }
    }
}
