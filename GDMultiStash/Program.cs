using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
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

        public static bool Quitting { get; private set; }

        public static void Quit()
        {
            if (Quitting) return;
            Quitting = true;
            new System.Threading.Thread(new System.Threading.ThreadStart(() => {
                Global.Windows.CloseMainWindow();
                Application.Exit();
            })).Start();
        }

    }
}
