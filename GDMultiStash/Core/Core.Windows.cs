using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace GDMultiStash
{
    internal static partial class Core
    {
        public static partial class Windows
        {

            private static readonly Forms.SetupDialogForm SetupWindow = new Forms.SetupDialogForm();
            private static readonly Forms.AboutDialogForm AboutWindow = new Forms.AboutDialogForm();
            private static readonly Forms.CreateStashDialogForm CreateStashWindow = new Forms.CreateStashDialogForm();
            private static readonly Forms.ImportDialogForm ImportWindow = new Forms.ImportDialogForm();
            private static readonly Forms.MainForm MainWindow = new Forms.MainForm();

            static Windows()
            {

                // dont need this
                //Runtime.CurrentExpansionChanged += delegate {
                    //MainWindow.UpdateObjects();
                //};

                Runtime.CurrentModeChanged += delegate {
                    MainWindow.UpdateObjects();
                };

                Runtime.StashAdded += delegate {
                    MainWindow.UpdateObjects();
                };

                Runtime.StashRemoved += delegate {
                    MainWindow.UpdateObjects();
                };

                MainWindow.FormClosed += delegate {
                    Program.Quit();
                };

                MainWindow.FormClosing += delegate (object sender, FormClosingEventArgs e) {
                    if (Program.Restarting) return;
                    if (Native.FindWindow("Grim Dawn", null) != IntPtr.Zero && Config.ShowCloseConfirm)
                    {
                        DialogResult result = MessageBox.Show(Localization.GetString("confirm_closing"), "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                        e.Cancel = (result == DialogResult.Cancel);
                    }
                };
            }





            
            public static void LocalizeWindows()
            {
                SetupWindow.Localize();
                MainWindow.Localize();
                AboutWindow.Localize();
                CreateStashWindow.Localize();
            }
            
            public static void ShowMainWindow()
            {
                if (MainWindow.Visible) return;
                MainWindow.TopMost = false;
                MainWindow.Show();
            }

            public static void ShowSetupDialog(bool isFirstSetup = false)
            {
                if (SetupWindow.Visible) return;
                SetupWindow.StartPosition = MainWindow.Visible ? FormStartPosition.CenterParent : FormStartPosition.CenterScreen;
                IWin32Window owner = MainWindow.Visible ? MainWindow : null;
                SetupWindow.ShowDialog(owner, isFirstSetup);
            }

            public static void ShowAboutDialog()
            {
                if (AboutWindow.Visible) return;
                AboutWindow.StartPosition = MainWindow.Visible ? FormStartPosition.CenterParent : FormStartPosition.CenterScreen;
                IWin32Window owner = MainWindow.Visible ? MainWindow : null;
                AboutWindow.ShowDialog(owner);
            }

            public static void ShowCreateStashDialog()
            {
                if (CreateStashWindow.Visible) return;
                CreateStashWindow.StartPosition = MainWindow.Visible ? FormStartPosition.CenterParent : FormStartPosition.CenterScreen;
                IWin32Window owner = MainWindow.Visible ? MainWindow : null;
                if (CreateStashWindow.ShowDialog(owner, out Common.Stash[] stashes) == DialogResult.OK)
                {
                    Config.Save();
                    Runtime.NotifyStashesAdded(stashes);
                }
            }

            public static void ShowImportDialog()
            {
                if (ImportWindow.Visible) return;
                ImportWindow.StartPosition = MainWindow.Visible ? FormStartPosition.CenterParent : FormStartPosition.CenterScreen;
                IWin32Window owner = MainWindow.Visible ? MainWindow : null;
                if (ImportWindow.ShowDialog(owner, out Common.Stash[] stashes) == DialogResult.OK)
                {
                    Config.Save();
                    Runtime.NotifyStashesAdded(stashes);
                }
            }

            public static void ShowImportDialog(IEnumerable<string> files)
            {
                if (ImportWindow.Visible) return;
                ImportWindow.StartPosition = MainWindow.Visible ? FormStartPosition.CenterParent : FormStartPosition.CenterScreen;
                IWin32Window owner = MainWindow.Visible ? MainWindow : null;
                if (ImportWindow.ShowDialog(owner, files, out Common.Stash[] stashes) == DialogResult.OK)
                {
                    Config.Save();
                    Runtime.NotifyStashesAdded(stashes);
                }
            }





        }
    }
}
