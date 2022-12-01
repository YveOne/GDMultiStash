using System;
using System.Windows.Forms;
using System.Collections.Generic;

using GDMultiStash.Common;

namespace GDMultiStash.GlobalHandlers
{
    internal class WindowsHandler
    {

        private readonly Forms.MainForm MainWindow;
        private readonly Forms.ConfigurationDialogForm ConfigurationWindow;
        private readonly Forms.AboutDialogForm AboutWindow;
        private readonly Forms.CreateStashDialogForm CreateStashWindow;
        private readonly Forms.ImportDialogForm ImportWindow;
        private readonly Forms.CategoriesDialogForm CategoriesWindow;
        private readonly Forms.ChangelogDialogForm ChangelogWindow;

        public WindowsHandler()
        {
            ConfigurationWindow = new Forms.ConfigurationDialogForm();
            AboutWindow = new Forms.AboutDialogForm();
            CreateStashWindow = new Forms.CreateStashDialogForm();
            ImportWindow = new Forms.ImportDialogForm();
            MainWindow = new Forms.MainForm();
            CategoriesWindow = new Forms.CategoriesDialogForm();
            ChangelogWindow = new Forms.ChangelogDialogForm();

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
                    DialogResult result = MessageBox.Show(Global.Localization.GetString("msg_confirm_gdms_closing"), "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    e.Cancel = (result == DialogResult.Cancel);
                }
            };

        }

        public void InitWindows()
        {
            ConfigurationWindow.Initialize();
            AboutWindow.Initialize();
            CreateStashWindow.Initialize();
            ImportWindow.Initialize();
            MainWindow.Initialize();
            CategoriesWindow.Initialize();
            ChangelogWindow.Initialize();
        }

        public void LocalizeWindows()
        {
            ConfigurationWindow.Localize();
            AboutWindow.Localize();
            CreateStashWindow.Localize();
            ImportWindow.Localize();
            MainWindow.Localize();
            CategoriesWindow.Localize();
            ChangelogWindow.Localize();
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

        private FormStartPosition DefaultStartPosition => MainWindow.Visible ? FormStartPosition.CenterParent : FormStartPosition.CenterScreen;
        private IWin32Window DefaultDialogOwner => MainWindow.Visible ? MainWindow : null;

        public void ShowConfigurationWindow(bool isFirstSetup = false)
        {
            if (ConfigurationWindow.Visible) return;
            ConfigurationWindow.StartPosition = DefaultStartPosition;
            ConfigurationWindow.ShowDialog(DefaultDialogOwner, isFirstSetup);
        }

        public void ShowAboutDialog()
        {
            if (AboutWindow.Visible) return;
            AboutWindow.StartPosition = DefaultStartPosition;
            AboutWindow.ShowDialog(DefaultDialogOwner);
        }

        public void ShowCreateStashDialog()
        {
            if (CreateStashWindow.Visible) return;
            CreateStashWindow.StartPosition = DefaultStartPosition;

            bool loop = true;
            while (loop)
            {

                if (CreateStashWindow.ShowDialog(DefaultDialogOwner, out StashObject stash) == DialogResult.OK)
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
            ImportWindow.StartPosition = DefaultStartPosition;
            if (ImportWindow.ShowDialog(DefaultDialogOwner, out StashObject[] stashes) == DialogResult.OK)
            {
                Global.Configuration.Save();
                Global.Runtime.NotifyStashesAdded(stashes);
            }
        }

        public void ShowImportDialog(IEnumerable<string> files)
        {
            if (ImportWindow.Visible) return;
            ImportWindow.StartPosition = DefaultStartPosition;
            if (ImportWindow.ShowDialog(DefaultDialogOwner, files, out StashObject[] stashes) == DialogResult.OK)
            {
                Global.Configuration.Save();
                Global.Runtime.NotifyStashesAdded(stashes);
            }
        }

        public void ShowCategoriesWindow()
        {
            if (CategoriesWindow.Visible) return;
            CategoriesWindow.StartPosition = DefaultStartPosition;
            if (CategoriesWindow.ShowDialog(DefaultDialogOwner) == DialogResult.OK)
            {
                Global.Configuration.Save();
            }
        }

        public void ShowChangelogWindow()
        {
            if (ChangelogWindow.Visible) return;
            ChangelogWindow.StartPosition = DefaultStartPosition;
            ChangelogWindow.ShowDialog(DefaultDialogOwner);
        }





    }
}
