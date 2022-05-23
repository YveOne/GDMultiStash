using System;
using System.Windows.Forms;

namespace GDMultiStash
{
    internal static partial class Core
    {
        public static partial class Windows
        {

            private static readonly Forms.SetupDialogForm SetupWindow = new Forms.SetupDialogForm();
            private static readonly Forms.AboutDialogForm AboutWindow = new Forms.AboutDialogForm();
            private static readonly Forms.AddStashDialogForm AddStashWindow = new Forms.AddStashDialogForm();
            private static readonly Forms.ImportDialogForm ImportWindow = new Forms.ImportDialogForm();
            private static readonly Forms.MainForm MainWindow = new Forms.MainForm();

            static Windows()
            {

                Runtime.CurrentModeChanged += delegate {
                    MainWindow.UpdateObjects();
                };

                Runtime.StashesChanged += delegate {
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
                AddStashWindow.Localize();
            }
            
            public static void ShowMainWindow()
            {
                if (MainWindow.Visible) return;
                MainWindow.TopMost = false;
                MainWindow.Show();
            }

            public static DialogResult ShowSetupDialog(bool isFirstSetup = false)
            {
                if (SetupWindow.Visible) return DialogResult.None;
                SetupWindow.StartPosition = MainWindow.Visible ? FormStartPosition.CenterParent : FormStartPosition.CenterScreen;
                return SetupWindow.ShowDialog(isFirstSetup);
            }

            public static DialogResult ShowAboutDialog()
            {
                if (AboutWindow.Visible) return DialogResult.None;
                AboutWindow.StartPosition = MainWindow.Visible ? FormStartPosition.CenterParent : FormStartPosition.CenterScreen;
                return AboutWindow.ShowDialog();
            }

            public static DialogResult ShowAddStashDialog()
            {
                if (AddStashWindow.Visible) return DialogResult.None;
                AddStashWindow.StartPosition = MainWindow.Visible ? FormStartPosition.CenterParent : FormStartPosition.CenterScreen;
                return AddStashWindow.ShowDialog();
            }

            public static DialogResult ShowImportDialog()
            {
                if (ImportWindow.Visible) return DialogResult.None;
                ImportWindow.StartPosition = MainWindow.Visible ? FormStartPosition.CenterParent : FormStartPosition.CenterScreen;
                return ImportWindow.ShowDialog();
            }




            
        }
    }
}
