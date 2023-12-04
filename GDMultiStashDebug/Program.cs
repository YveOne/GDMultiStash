using System;
using System.Text;
using System.Diagnostics;

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
                if (process.ExitCode == 1)
                    System.Threading.Thread.Sleep(5000);
            }
        }

    }
}
