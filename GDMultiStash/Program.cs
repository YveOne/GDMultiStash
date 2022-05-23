using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.Diagnostics;

namespace GDMultiStash
{
    internal static class Program
    {

        [STAThread]
        static void Main()
        {
            Native.AttachConsole(Native.ATTACH_PARRENT);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GDMSContext());
        }

        public static void ShowError(string errmsg)
        {
            Console.WriteLine("ERROR: " + errmsg);
            MessageBox.Show(errmsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static bool Quitting { get; private set; }

        public static void Quit()
        {
            if (Quitting) return;
            Quitting = true;
            new System.Threading.Thread(new System.Threading.ThreadStart(() => {
                Application.Exit();
            })).Start();
        }

        public static bool Restarting { get; private set; }

        public static void Restart()
        {
            if (Restarting) return;
            Restarting = true;
            new System.Threading.Thread(new System.Threading.ThreadStart(() => {
                Application.Exit();
                Process.Start(Application.ExecutablePath);
            })).Start();
        }

    }
}
