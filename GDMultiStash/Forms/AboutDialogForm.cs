using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GDMultiStash.Forms
{
    internal partial class AboutDialogForm : DialogForm
    {

        public AboutDialogForm() : base()
        {
            InitializeComponent();
        }

        protected override void Localize(Global.LocalizationManager.StringsHolder L)
        {
            Text = L.AboutButton();
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            Utils.AssemblyInfo ai = new Utils.AssemblyInfo();
            label1.Text = $"{ai.AppName} v{ai.Version}";
            label2.Text = $"{ai.Copyright}";
        }

        private void AboutForm_Shown(object sender, EventArgs e)
        {
            TopMost = false;
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Utils.Funcs.OpenUrl("https://forums.crateentertainment.com/t/gd-stash-changer/34625");
        }

        private void LinkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Utils.Funcs.OpenUrl("https://github.com/marius00/iagd");
        }

        private void LinkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Utils.Funcs.OpenUrl("https://github.com/justinstenning/Direct3DHook");
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Utils.Funcs.OpenUrl("https://forums.crateentertainment.com/t/tool-gd-multistash/");
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Utils.Funcs.OpenUrl("https://github.com/YveOne/GDMultiStash");
        }
    }
}
