using System;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GDMultiStashDebug
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "GDMS Debug Log";
            Console.OutputEncoding = Encoding.UTF8;

            using (Process process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Normal,
                    FileName = "GDMultiStash.exe",
                    UseShellExecute = false,
                }
            })
            {
                process.Start();
                process.WaitForExit();
            }
        }

    }
}
