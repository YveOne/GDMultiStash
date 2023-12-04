using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;

namespace GDMultiStash.Global
{
    using GDMultiStash.Common.Objects;
    using GDMultiStash.Forms;
    using Windows;
    namespace Windows
    {
        public delegate void ExtendButtonCallback(ExtendedButton extendedButton);

        // TODO: remove me later
        public class ExtendedButton
        {
            private Button _button;
            private bool _doUpdate = false;
            private bool _isMouseOver = false;
            private Color _foreColor;
            private Color _foreColorHover;
            private Image _image;
            private Image _imageHover;

            public Button Button => _button;

            public Color BackColor
            {
                get => _button.BackColor;
                set
                {
                    _button.BackColor = value;
                }
            }

            public Color BackColorHover
            {
                get => _button.FlatAppearance.MouseOverBackColor;
                set
                {
                    _button.FlatAppearance.MouseOverBackColor = value;
                }
            }

            public Color BackColorPressed
            {
                get => _button.FlatAppearance.MouseDownBackColor;
                set
                {
                    _button.FlatAppearance.MouseDownBackColor = value;
                }
            }

            public Color ForeColor
            {
                get => _foreColor;
                set
                {
                    _foreColor = value;
                    UpdateAppearance();
                }
            }

            public Color ForeColorHover
            {
                get => _foreColorHover;
                set
                {
                    _foreColorHover = value;
                    UpdateAppearance();
                }
            }

            public Image Image
            {
                get => _image;
                set
                {
                    _image = value;
                    UpdateAppearance();
                }
            }

            public Image ImageHover
            {
                get => _imageHover;
                set
                {
                    _imageHover = value;
                    UpdateAppearance();
                }
            }

            public ExtendedButton(Button button)
            {
                _button = button;
                _button.FlatStyle = FlatStyle.Flat;
                _button.FlatAppearance.BorderSize = 0;
                _foreColor = button.ForeColor;
                _foreColorHover = button.ForeColor;
                BackColorHover = BackColor;
                BackColorPressed = BackColor;
                if (button.Image != null)
                {
                    var defImage = button.Image;
                    _image = defImage;
                    _imageHover = defImage;
                }
                _doUpdate = true;
                _button.MouseEnter += delegate
                {
                    _isMouseOver = true;
                    UpdateAppearance();
                };
                _button.MouseLeave += delegate
                {
                    _isMouseOver = false;
                    UpdateAppearance();
                };
            }

            private void UpdateAppearance()
            {
                if (!_doUpdate) return;
                if (_isMouseOver)
                {
                    _button.ForeColor = _foreColorHover;
                    _button.Image = _imageHover;
                }
                else
                {
                    _button.ForeColor = _foreColor;
                    _button.Image = _image;
                }
            }

        }
    }

    internal partial class WindowsManager : Base.Manager
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
        private SplashForm SplashScreen { get; }

        public WindowsManager() : base()
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
            SplashScreen = new SplashForm();
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
            SplashScreen.Localize();
        }

        #region Splash Screen

        public void ShowSplashScreen()
        {
            SplashScreen.Show();
        }

        public void HideSplashScreen()
        {
            SplashScreen.Hide();
        }

        #endregion

        #region Main Window

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

        #endregion

        #region All other windows

        private FormStartPosition DefaultStartPosition => MainWindow.Visible ? FormStartPosition.CenterParent : FormStartPosition.CenterScreen;
        private IWin32Window DefaultDialogOwner => MainWindow.Visible ? MainWindow : null;

        public void ShowConfigurationWindow()
        {
            G.Windows.HideSplashScreen();
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
                G.Configuration.Save();
                G.Stashes.InvokeStashesAdded(stashes);
            }
        }

        public void ShowImportDialog(IEnumerable<string> files)
        {
            if (ImportWindow.Visible) return;
            ImportWindow.StartPosition = DefaultStartPosition;
            if (ImportWindow.ShowDialog(DefaultDialogOwner, files, out StashObject[] stashes) == DialogResult.OK)
            {
                G.Configuration.Save();
                G.Stashes.InvokeStashesAdded(stashes);
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

        #endregion

        #region Caption Buttons

        // TODO: remove this whole region later

        public void ExtendButton(Button button, ExtendButtonCallback callback = null)
        {
            callback?.Invoke(new ExtendedButton(button));
        }

        public void ExtendCaptionButton(Button button, ExtendButtonCallback callback = null)
        {
            ExtendButton(button, (ExtendedButton exButton) => {
                exButton.BackColor = C.ControlBoxButtonBackColor;
                exButton.BackColorHover = C.ControlBoxButtonBackColorHover;
                exButton.BackColorPressed = C.ControlBoxButtonBackColorPressed;
                exButton.ForeColor = C.InteractiveForeColor;
                exButton.ForeColorHover = C.InteractiveForeColorHighlight;
                callback?.Invoke(exButton);
            });
        }

        public void ExtendCaptionCloseButton(Button button, ExtendButtonCallback callback = null)
        {
            ExtendCaptionButton(button, (ExtendedButton exButton) => {
                exButton.BackColorHover = Color.FromArgb(150, 32, 5); // TODO: put me inside Constants.cs
                exButton.BackColorPressed = Color.FromArgb(207, 49, 12); // TODO: put me inside Constants.cs
                exButton.Image = Properties.Resources.buttonCloseGray;
                exButton.ImageHover = Properties.Resources.buttonCloseWhite;
                callback?.Invoke(exButton);
            });
        }

        #endregion







        // TODO: i do not belong here
        public enum GameStartResult
        {
            Disabled = 0,
            AlreadyRunning = 1,
            Success = 2,
            Error = 3,
        }

        // TODO: i do not belong here
        public GameStartResult StartGame()
        {
            if (Native.FindWindow("Grim Dawn", null) != IntPtr.Zero) return GameStartResult.AlreadyRunning;

            var runCommand = G.Configuration.GetStartGameCommand();
            var runArguments = G.Configuration.Settings.StartGameArguments;

            switch (runCommand)
            {
                case "steam":
                    runCommand = Path.Combine(GrimDawnLib.GrimDawn.Steam.SteamClientPath64, "Steam.exe");
                    runArguments = $"-applaunch 219990 /x64 {runArguments}";
                    break;
                case "gog":
                    runCommand = GrimDawnLib.GrimDawn.GOG.GameStartCommand64;
                    break;
                case "grimdawn":
                    runCommand = Path.Combine(G.Configuration.Settings.GamePath, "x64", "Grim Dawn.exe");
                    runArguments = $"/x64 {runArguments}";
                    break;
                case "griminternals":
                    runCommand = Path.Combine(G.Configuration.Settings.GamePath, "GrimInternals64.exe");
                    break;
                case "grimcam":
                    runCommand = Path.Combine(G.Configuration.Settings.GamePath, "GrimCam.exe");
                    break;
            }
            /*
            var runDirectory = File.Exists(runCommand)
                    ? Path.GetDirectoryName(runCommand)
                    : G.Configuration.Settings.GamePath;
            */
            var runDirectory = G.Configuration.Settings.GamePath;

            Console.WriteLine($"Starting Grim Dawn:");
            Console.WriteLine($"- Command: {runCommand}");
            Console.WriteLine($"- Arguments: {runArguments}");
            Console.WriteLine($"- WorkingDir: {runDirectory}");
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo = new System.Diagnostics.ProcessStartInfo
            {
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                FileName = runCommand,
                Arguments = runArguments,
                WorkingDirectory = runDirectory
            };
            try
            {
                process.Start();
                return GameStartResult.Success;
            }
            catch (Exception ex)
            {
                Console.AlertError(ex.Message);
                return GameStartResult.Error;
            }
        }

    }
}
