using System;
using System.Windows.Forms;
using System.Collections.Generic;

using GDMultiStash.Common.Objects;
using GDMultiStash.Forms;

namespace GDMultiStash.GlobalHandlers
{
    internal partial class WindowsHandler : Base.BaseHandler
    {

        private MainForm MainWindow { get; }
        private ConfigurationDialogForm ConfigurationWindow { get; }
        private AboutDialogForm AboutWindow { get; }
        private CreateStashDialogForm CreateStashWindow { get; }
        private CreateStashGroupDialogForm CreateStashGroupWindow { get; }
        private ImportDialogForm ImportWindow { get; }
        private CraftingModeDialogForm CraftingModeWindow { get; }
        private ChangelogDialogForm ChangelogWindow { get; }
        private ProgressDialogForm ProgressDialog { get; }
        private Dictionary<int, StashTabsEditorWindow> StashTabsEditorWindows { get; }
        private Forms.Plexiglass.ScreenPlexiglass StashTabsEditorPlexiglass { get; }

        public WindowsHandler() : base()
        {
            ConfigurationWindow = new ConfigurationDialogForm();
            AboutWindow = new AboutDialogForm();
            CreateStashWindow = new CreateStashDialogForm();
            CreateStashGroupWindow = new CreateStashGroupDialogForm();
            ImportWindow = new ImportDialogForm();
            CraftingModeWindow = new CraftingModeDialogForm();
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
            CraftingModeWindow.Localize();
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
                MainWindow.Invoke(new Action(() => {
                    MainWindow.Close();
                }));
            }
            catch (Exception)
            {
            }
        }

        public void ShowMainWindow(Action onShow = null)
        {
            if (MainWindow.Visible)
            {
                Native.SetForegroundWindow(MainWindow.Handle);
                return;
            }
            void shownHandler(object sender, EventArgs e)
            {
                MainWindow.Shown -= shownHandler;
                onShow?.Invoke();
            }
            MainWindow.Shown += shownHandler;
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
                Global.Runtime.InvokeStashesAdded(stashes);
            }
        }

        public void ShowImportDialog(IEnumerable<string> files)
        {
            if (ImportWindow.Visible) return;
            ImportWindow.StartPosition = DefaultStartPosition;
            if (ImportWindow.ShowDialog(DefaultDialogOwner, files, out StashObject[] stashes) == DialogResult.OK)
            {
                Global.Configuration.Save();
                Global.Runtime.InvokeStashesAdded(stashes);
            }
        }

        public void ShowCraftingModeDialog()
        {
            if (CraftingModeWindow.Visible) return;
            CraftingModeWindow.StartPosition = DefaultStartPosition;
            CraftingModeWindow.ShowDialog(DefaultDialogOwner);
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
            win.Owner = (Form)DefaultDialogOwner;
            StashTabsEditorWindows.Add(stash.ID, win);
            win.Show();
        }

        public enum GameStartResult
        {
            Disabled = 0,
            AlreadyRunning = 1,
            Success = 2,
            Error = 3,
        }

        public GameStartResult StartGame()
        {
            if (Native.FindWindow("Grim Dawn", null) != IntPtr.Zero) return GameStartResult.AlreadyRunning;

            Console.WriteLine($"Starting Grim Dawn:");
            Console.WriteLine($"- Command: {Global.Configuration.Settings.StartGameCommand}");
            Console.WriteLine($"- Arguments: {Global.Configuration.Settings.StartGameArguments}");
            Console.WriteLine($"- WorkingDir: {Global.Configuration.Settings.GamePath}");
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo
            {
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                FileName = Global.Configuration.Settings.StartGameCommand,
                Arguments = Global.Configuration.Settings.StartGameArguments,
                WorkingDirectory = System.IO.File.Exists(Global.Configuration.Settings.StartGameCommand)
                    ? System.IO.Path.GetDirectoryName(Global.Configuration.Settings.StartGameCommand)
                    : Global.Configuration.Settings.GamePath
            };
            process.StartInfo = startInfo;
            try
            {
                process.Start();
                return GameStartResult.Success;
            }
            catch (Exception ex)
            {
                Console.Error(ex.Message);
                return GameStartResult.Error;
            }
        }

    }
}
