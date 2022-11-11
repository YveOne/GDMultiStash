using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace GDMultiStash.GlobalHandlers
{
    internal class WindowsHandler
    {

        private readonly Forms.MainForm MainWindow;
        private readonly Forms.SetupDialogForm SetupWindow;
        private readonly Forms.AboutDialogForm AboutWindow;
        private readonly Forms.CreateStashDialogForm CreateStashWindow;
        private readonly Forms.ImportDialogForm ImportWindow;

        public WindowsHandler()
        {
            SetupWindow = new Forms.SetupDialogForm();
            AboutWindow = new Forms.AboutDialogForm();
            CreateStashWindow = new Forms.CreateStashDialogForm();
            ImportWindow = new Forms.ImportDialogForm();
            MainWindow = new Forms.MainForm();

            Global.Runtime.ActiveModeChanged += delegate {
                MainWindow.UpdateObjects();
            };

            Global.Runtime.StashesAdded += delegate {
                MainWindow.UpdateObjects();
            };

            Global.Runtime.StashesRemoved += delegate {
                MainWindow.UpdateObjects();
            };

            MainWindow.FormClosed += delegate (object sender, FormClosedEventArgs e) {
                Program.Quit();
            };

            MainWindow.FormClosing += delegate (object sender, FormClosingEventArgs e) {
                if (Native.FindWindow("Grim Dawn", null) != IntPtr.Zero && Global.Configuration.Settings.HideOnFormClosed)
                {
                    e.Cancel = true;
                    MainWindow.Hide();
                    return;
                }
                if (Native.FindWindow("Grim Dawn", null) != IntPtr.Zero && Global.Configuration.Settings.ConfirmClosing)
                {
                    DialogResult result = MessageBox.Show(Global.Localization.GetString("confirm_closing"), "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    e.Cancel = (result == DialogResult.Cancel);
                }
            };

        }

        public void InitWindows()
        {
            SetupWindow.InitWindow();
            MainWindow.InitWindow();
            AboutWindow.InitWindow();
            CreateStashWindow.InitWindow();
        }

        public void LocalizeWindows()
        {
            SetupWindow.Localize();
            MainWindow.Localize();
            AboutWindow.Localize();
            CreateStashWindow.Localize();
        }

        public void ShowMainWindow(Action onShow = null)
        {
            if (MainWindow.Visible) return;

            EventHandler shownHandler = null;
            shownHandler = delegate {
                MainWindow.Shown -= shownHandler;
                if (onShow != null) onShow();
            };

            MainWindow.Shown += shownHandler;
            MainWindow.TopMost = false;
            MainWindow.Show();
            MainWindow.Refresh();
            MainWindow.Update();
        }

        public void ShowSetupDialog(bool isFirstSetup = false)
        {
            if (SetupWindow.Visible) return;
            SetupWindow.StartPosition = MainWindow.Visible ? FormStartPosition.CenterParent : FormStartPosition.CenterScreen;
            IWin32Window owner = MainWindow.Visible ? MainWindow : null;
            SetupWindow.ShowDialog(owner, isFirstSetup);
        }

        public void ShowAboutDialog()
        {
            if (AboutWindow.Visible) return;
            AboutWindow.StartPosition = MainWindow.Visible ? FormStartPosition.CenterParent : FormStartPosition.CenterScreen;
            IWin32Window owner = MainWindow.Visible ? MainWindow : null;
            AboutWindow.ShowDialog(owner);
        }

        public void ShowCreateStashDialog()
        {
            if (CreateStashWindow.Visible) return;
            CreateStashWindow.StartPosition = MainWindow.Visible ? FormStartPosition.CenterParent : FormStartPosition.CenterScreen;
            IWin32Window owner = MainWindow.Visible ? MainWindow : null;

            bool loop = true;
            while (loop)
            {

                if (CreateStashWindow.ShowDialog(owner, out StashObject stash) == DialogResult.OK)
                {
                    Global.Configuration.Save();
                    Global.Runtime.NotifyStashesAdded(stash);
                }
                else
                {
                    loop = false;
                }
            }
        }

        public void ShowImportDialog()
        {
            if (ImportWindow.Visible) return;
            ImportWindow.StartPosition = MainWindow.Visible ? FormStartPosition.CenterParent : FormStartPosition.CenterScreen;
            IWin32Window owner = MainWindow.Visible ? MainWindow : null;
            if (ImportWindow.ShowDialog(owner, out StashObject[] stashes) == DialogResult.OK)
            {
                Global.Configuration.Save();
                Global.Runtime.NotifyStashesAdded(stashes);
            }
        }

        public void ShowImportDialog(IEnumerable<string> files)
        {
            if (ImportWindow.Visible) return;
            ImportWindow.StartPosition = MainWindow.Visible ? FormStartPosition.CenterParent : FormStartPosition.CenterScreen;
            IWin32Window owner = MainWindow.Visible ? MainWindow : null;
            if (ImportWindow.ShowDialog(owner, files, out StashObject[] stashes) == DialogResult.OK)
            {
                Global.Configuration.Save();
                Global.Runtime.NotifyStashesAdded(stashes);
            }
        }

    }
}
