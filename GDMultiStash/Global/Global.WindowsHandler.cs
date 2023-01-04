using System;
using System.Windows.Forms;
using System.Collections.Generic;

using GDMultiStash.Common.Objects;

namespace GDMultiStash.GlobalHandlers
{
    internal class WindowsHandler
    {

        public readonly Forms.MainForm MainWindow;
        public readonly Forms.ConfigurationDialogForm ConfigurationWindow;
        public readonly Forms.AboutDialogForm AboutWindow;
        public readonly Forms.CreateStashDialogForm CreateStashWindow;
        public readonly Forms.CreateStashGroupDialogForm CreateStashGroupWindow;
        public readonly Forms.ImportDialogForm ImportWindow;
        public readonly Forms.ChangelogDialogForm ChangelogWindow;
        public readonly Forms.ProgressDialogForm ProgressDialog;

        public WindowsHandler()
        {
            ConfigurationWindow = new Forms.ConfigurationDialogForm();
            AboutWindow = new Forms.AboutDialogForm();
            CreateStashWindow = new Forms.CreateStashDialogForm();
            CreateStashGroupWindow = new Forms.CreateStashGroupDialogForm();
            ImportWindow = new Forms.ImportDialogForm();
            MainWindow = new Forms.MainForm();
            ChangelogWindow = new Forms.ChangelogDialogForm();
            ProgressDialog = new Forms.ProgressDialogForm();
        }

        public void LocalizeWindows()
        {
            ConfigurationWindow.Localize();
            AboutWindow.Localize();
            CreateStashWindow.Localize();
            CreateStashGroupWindow.Localize();
            ImportWindow.Localize();
            MainWindow.Localize();
            ChangelogWindow.Localize();
            ProgressDialog.Localize();
        }

        public void CloseMainWindow()
        {
            try
            {
                MainWindow.Invoke((MethodInvoker)delegate {
                    MainWindow.Close();
                });
            }
            catch (Exception)
            {
            }
        }

        public void ShowMainWindow(Action onShow = null)
        {
            if (MainWindow.Visible) return;

            void shownHandler(object sender, EventArgs e)
            {
                MainWindow.Shown -= shownHandler;
                onShow?.Invoke();
            }

            MainWindow.Shown += shownHandler;
            MainWindow.TopMost = false;
            MainWindow.Show();
        }

        private FormStartPosition DefaultStartPosition => MainWindow.Visible ? FormStartPosition.CenterParent : FormStartPosition.CenterScreen;
        private IWin32Window DefaultDialogOwner => MainWindow.Visible ? MainWindow : null;

        public void ShowConfigurationWindow()
        {
            if (ConfigurationWindow.Visible) return;
            ConfigurationWindow.StartPosition = DefaultStartPosition;
            ConfigurationWindow.ShowDialog(DefaultDialogOwner);
        }

        public void ShowAboutDialog()
        {
            if (AboutWindow.Visible) return;
            AboutWindow.StartPosition = DefaultStartPosition;
            AboutWindow.ShowDialog(DefaultDialogOwner);
        }

        public void ShowCreateStashDialog(GrimDawnLib.GrimDawnGameExpansion exp)
        {
            if (CreateStashWindow.Visible) return;
            CreateStashWindow.StartPosition = DefaultStartPosition;
            CreateStashWindow.ShowDialog(DefaultDialogOwner, exp);
        }

        public void ShowCreateStashGroupDialog()
        {
            if (CreateStashGroupWindow.Visible) return;
            CreateStashGroupWindow.StartPosition = DefaultStartPosition;
            CreateStashGroupWindow.ShowDialog(DefaultDialogOwner);
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

        public void ShowChangelogWindow()
        {
            if (ChangelogWindow.Visible) return;
            ChangelogWindow.StartPosition = DefaultStartPosition;
            ChangelogWindow.ShowDialog(DefaultDialogOwner);
        }




    }
}
