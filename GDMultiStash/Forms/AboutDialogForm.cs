using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace GDMultiStash.Forms
{
    internal partial class AboutDialogForm : DialogForm
    {

        public AboutDialogForm() : base()
        {
            InitializeComponent();
        }

        protected override void Localize(Core.Localization.StringsProxy L)
        {

        }






        private void AboutForm_Load(object sender, EventArgs e)
        {
            string appname = Assembly.GetExecutingAssembly().GetName().Name;
            Assembly asm = Assembly.GetExecutingAssembly();
            string copyright = ((AssemblyCopyrightAttribute)asm.GetCustomAttribute(typeof(AssemblyCopyrightAttribute))).Copyright;
            string version = ((AssemblyFileVersionAttribute)asm.GetCustomAttribute(typeof(AssemblyFileVersionAttribute))).Version;

            label1.Text = string.Format("{0} v{1}", appname, version);
            label2.Text = string.Format(copyright);
        }

        private void AboutForm_Shown(object sender, EventArgs e)
        {
            TopMost = false;
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenUrl("https://forums.crateentertainment.com/t/gd-stash-changer/34625");
        }

        private void LinkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenUrl("https://github.com/marius00/iagd");
        }

        private void LinkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenUrl("https://github.com/justinstenning/Direct3DHook");
        }



        private static void OpenUrl(string url)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = url;
            process.StartInfo = startInfo;
            process.Start();
        }

    }
}
