using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GDMultiStash.Forms
{
    internal partial class ChangelogDialogForm : DialogForm
    {
        private Dictionary<string, string> logs = new Dictionary<string, string>();

        public ChangelogDialogForm() : base()
        {
            InitializeComponent();

            versionSelectComboBox.SelectionChangeCommitted += VersionSelectComboBox_SelectionChangeCommitted;
        }

        private void ChangelogDialogForm_Load(object sender, EventArgs e)
        {
            Regex re = new Regex(@"^(^v[\.\d]+)\s*$[\r\n]+(.+?)^+\s*$", RegexOptions.Multiline|RegexOptions.Singleline);
            string text = File.ReadAllText("changelog.txt");
            MatchCollection matches = re.Matches(text);
            logs.Clear();
            versionSelectComboBox.Items.Clear();
            foreach (Match match in matches)
            {
                string k = match.Groups[1].Value;
                string v = match.Groups[2].Value;
                logs.Add(k, v);
                versionSelectComboBox.Items.Add(k);
            }
            versionSelectComboBox.SelectedIndex = 0;
            changelogTextBox.Text = logs.Values.ElementAt(0);
        }

        protected override void Localize(GlobalHandlers.LocalizationHandler.StringsHolder L)
        {

        }

        private void VersionSelectComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            changelogTextBox.Text = logs[versionSelectComboBox.Text];
        }

    }
}
