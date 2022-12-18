using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.ComponentModel;
using GDIALib.EvilsoftCommons.DllInjector;

namespace GDIALib.GDHook
{
    public class InjectionTargetForm : Form
    {

        // I created this to access CustomWndProc method (Yvonne Pautz)

        private RegisterWindow _window;
        private InjectionHelper _injector;
        private Action<RegisterWindow.DataAndType> _registerWindowDelegate;
        private ProgressChangedEventHandler _injectorCallbackDelegate;
        private readonly List<IMessageProcessor> _messageProcessors = new List<IMessageProcessor>();

        private bool injected = false;

        public InjectionTargetForm() : base()
        {
            _messageProcessors.Add(new SaveTransferStashHandler());
            _messageProcessors.Add(new StashStatusHandler());
            _messageProcessors.Add(new ModeStatusHandler());
            _messageProcessors.Add(new ExpansionStatusHandler());
            _messageProcessors.Add(new SetModNameStatusHandler());
        }

        public void StartInjector()
        {
            // its reinjecting itself already
            if (injected) return;
            injected = true;

            // Start looking for GD processes!
            _registerWindowDelegate = CustomWndProc;
            _window = new RegisterWindow("GDMSWindowClass", _registerWindowDelegate);

            // This prevents a implicit cast to new ProgressChangedEventHandler(func), which would hit the GC and before being used from another thread
            // Same happens when shutting down, fix unknown
            _injectorCallbackDelegate = InjectorCallback;

            _injector = new InjectionHelper(new BackgroundWorker(), _injectorCallbackDelegate, false, "Grim Dawn", string.Empty, "GDIAHook.dll");
        }

        public void Destroy()
        {
            if (_window != null) _window.Dispose();
            if (_injector != null) _injector.Dispose();
            if (_registerWindowDelegate != null) _registerWindowDelegate = null;
            if (_injectorCallbackDelegate != null) _injectorCallbackDelegate = null;
            Dispose();
        }

        /// <summary>
        /// Callback called when the Grim Dawn hook sends messages to IA
        /// </summary>
        /// <returns></returns>
        private void CustomWndProc(RegisterWindow.DataAndType bt)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { CustomWndProc(bt); });
                return;
            }
            MessageType type = (MessageType)bt.Type;
            foreach (IMessageProcessor t in _messageProcessors)
            {
                t.Process(type, bt.Data, bt.StringData);
            }
            switch (type)
            {
                case MessageType.TYPE_REPORT_WORKER_THREAD_LAUNCHED:
                    Console.WriteLine("[GDHook] Grim Dawn hook reports successful launch.");
                    break;
            }
        }

        /// <summary>
        /// Toolstrip callback for GDInjector
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InjectorCallback(object sender, ProgressChangedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { InjectorCallback(sender, e); });
            }
            else
            {
                switch (e.ProgressPercentage)
                {
                    case InjectionHelper.INJECTION_ERROR:
                    case InjectionHelper.INJECTION_ERROR_32BIT:
                    case InjectionHelper.INJECTION_ERROR_POSSIBLE_ACCESS_DENIED:
                        {
                            RuntimeSettings.StashStatus = StashAvailability.ERROR;
                            break;
                        }

                    // No grim dawn client, so stash is closed!
                    case InjectionHelper.NO_PROCESS_FOUND_ON_STARTUP:
                    case InjectionHelper.NO_PROCESS_FOUND:
                        RuntimeSettings.StashStatus = StashAvailability.CLOSED;
                        break;

                }
            }
        }

    }
}
