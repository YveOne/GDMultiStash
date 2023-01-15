using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GDMultiStash
{

    internal static class Program
    {

        private static GDMSContext context;

        [STAThread]
        static void Main()
        {
            Console.CreateConsole();
            Console.LogToFile(System.IO.Path.Combine(Application.StartupPath, "log.txt"));

            _handler += new EventHandler(Handler);
            SetConsoleCtrlHandler(_handler, true);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            context = new GDMSContext();
            Application.Run(context);
        }

        public static bool IsQuitting { get; private set; }

        public static void Quit()
        {
            if (IsQuitting) return;
            IsQuitting = true;
            new System.Threading.Thread(new System.Threading.ThreadStart(() => {
                Destroy();
                Application.Exit();
            })).Start();
        }

        private static void Destroy()
        {
            context.Destroy();
            Console.DestroyConsole();
        }

        #region Trap application termination

        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

        private delegate bool EventHandler(CtrlType sig);
        static EventHandler _handler;

        enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        private static bool Handler(CtrlType sig)
        {
            Destroy();
            //Environment.Exit(-1);
            return true;
        }

        #endregion

    }
}
