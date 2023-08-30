using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System.Windows.Forms;

namespace GDMultiStashStarter
{
    internal static class Program
    {
        [DllImport("kernel32.dll")]
        public static extern uint AttachConsole(uint dwProcessId);
        public const uint ATTACH_PARRENT = 0xFFFFFFFF;

        private static void Del(string f)
        {
            if (File.Exists(f)) File.Delete(f);
        }

        private static void Chk(string f)
        {
            if (!File.Exists(f)) MessageBox.Show($"Missing: {f}", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        [STAThread]
        static void Main()
        {
            AttachConsole(ATTACH_PARRENT);

            _handler += new EventHandler(Handler);
            SetConsoleCtrlHandler(_handler, true);

            Del("GDMultiStash.debug.bat");
            Del("SharpDX.Direct3D9.dll");
            Del("SharpDX.Direct3D10.dll");
            Del("SharpDX.xml");
            Del("SharpDX.Mathematics.xml");
            Del("SharpDX.DXGI.xml");
            Del("SharpDX.Direct3D9.xml");
            Del("SharpDX.Direct3D10.xml");
            Del("SharpDX.Direct3D11.xml");
            Del("SharpDX.Direct3D11.Effects.xml");
            Del("SharpDX.Direct2D1.xml");
            Del("SharpDX.Desktop.xml");
            Del("SharpDX.D3DCompiler.xml");
            Del("ObjectListView.xml");
            Del("EasyHook.xml");
            Del("GDMultiStashUpdater.pdb");
            Del("GDMultiStash.pdb");
            Del("GDIALib.pdb");
            Del("D3DHook.pdb");
            Del("GDMultiStash.exe.config");
            Del("GDMultiStashUpdater.exe.config");

            Chk("sharpdx_direct3d11_1_effects_x86.dll");
            Chk("sharpdx_direct3d11_1_effects_x64.dll");
            Chk("SharpDX.Mathematics.dll");
            Chk("SharpDX.DXGI.dll");
            Chk("SharpDX.dll");
            Chk("SharpDX.Direct3D11.Effects.dll");
            Chk("SharpDX.Direct3D11.dll");
            Chk("SharpDX.Direct2D1.dll");
            Chk("SharpDX.Desktop.dll");
            Chk("SharpDX.D3DCompiler.dll");
            Chk("ObjectListView.dll");
            Chk("GDIALib.dll");
            Chk("GDIAHook.dll");
            Chk("EasyLoad64.dll");
            Chk("EasyLoad32.dll");
            Chk("EasyHook64.dll");
            Chk("EasyHook32.dll");
            Chk("EasyHook.dll");
            Chk("DllInjector.dll");
            Chk("D3DHook.dll");
            Chk("Listdlls.exe");
            Chk("EasyHook64Svc.exe");
            Chk("EasyHook32Svc.exe");
            Chk("DllInjector64.exe");
            Chk("GDMultiStash.bin");

            process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Normal,
                    FileName = "GDMultiStash.bin",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                }
            };
            
            process.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
            process.ErrorDataReceived += new DataReceivedEventHandler(OutputHandler);
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
        }

        private static Process process;

        static void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            Console.WriteLine(outLine.Data);
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
            process.Kill();
            Environment.Exit(-1);
            return true;
        }

        #endregion

    }
}
