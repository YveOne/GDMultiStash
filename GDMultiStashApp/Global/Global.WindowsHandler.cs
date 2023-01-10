using System;
using System.Windows.Forms;
using System.Collections.Generic;

using GDMultiStash.Common.Objects;
using GDMultiStash.Forms;

namespace GDMultiStash.GlobalHandlers
{
    internal class WindowsHandler
    {

        private MainForm MainWindow { get; }
        private ConfigurationDialogForm ConfigurationWindow { get; }
        private AboutDialogForm AboutWindow { get; }
        private CreateStashDialogForm CreateStashWindow { get; }
        private CreateStashGroupDialogForm CreateStashGroupWindow { get; }
        private ImportDialogForm ImportWindow { get; }
        private ChangelogDialogForm ChangelogWindow { get; }
        private ProgressDialogForm ProgressDialog { get; }
        private Dictionary<int, StashTabsEditorWindow> StashTabsEditorWindows { get; }
        private Forms.Plexiglass.ScreenPlexiglass StashTabsEditorPlexiglass { get; }

        public WindowsHandler()
        {
            ConfigurationWindow = new ConfigurationDialogForm();
            AboutWindow = new AboutDialogForm();
            CreateStashWindow = new CreateStashDialogForm();
            CreateStashGroupWindow = new CreateStashGroupDialogForm();
            ImportWindow = new ImportDialogForm();
            MainWindow = new MainForm();
            ChangelogWindow = new ChangelogDialogForm();
            ProgressDialog = new ProgressDialogForm();
            StashTabsEditorWindows = new Dictionary<int, StashTabsEditorWindow>();
            StashTabsEditorPlexiglass = new Forms.Plexiglass.ScreenPlexiglass();
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
            foreach(var w in StashTabsEditorWindows.Values)
                w.Localize();
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
            if (MainWindow.Visible)
            {
                MainWindow.Focus();
                return;
            }

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


        public void ShowStashTabsEditorWindow(StashObject stash)
        {
            if (StashTabsEditorWindows.TryGetValue(stash.ID, out StashTabsEditorWindow w))
            {
                w.BringToFront();
                return;
            }
            var win = new StashTabsEditorWindow(stash);
            win.FormClosed += delegate { StashTabsEditorWindows.Remove(stash.ID); };
            win.TopMost = false;
            //win.Owner = (Form)DefaultDialogOwner;
            StashTabsEditorWindows.Add(stash.ID, win);
            win.Show();
        }






    }
}
