using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    internal class Funcs
    {
        public delegate bool WaitForConditionDelegate();
        public static bool WaitFor(WaitForConditionDelegate condition, int time, int delay = 1)
        {
            long timeout = Environment.TickCount + time;
            while (Environment.TickCount < timeout)
            {
                if (condition()) return true;
                long timeout2 = Environment.TickCount + delay;
                while (Environment.TickCount < timeout2)
                {
                    System.Threading.Thread.Sleep(1);
                }
            }
            return false;
        }

        public static void OpenUrl(string url)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = url;
            process.StartInfo = startInfo;
            process.Start();
        }








    }
}
