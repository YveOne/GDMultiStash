using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

using GDMultiStash.Common;

namespace GDMultiStash
{
    internal class Test
    {
        public Test()
        {
            return;


            using (var dialog = new System.Windows.Forms.OpenFileDialog()
            {
                Filter = "gd_conf|gd_conf.xml",
                Multiselect = false,
            })
            {
                DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string dir = Path.Combine(Path.GetDirectoryName(dialog.FileName), "Stashes");
                    string text = File.ReadAllText(dialog.FileName);
                    TransferFile transfer = new TransferFile();
                    MatchCollection matches = Regex.Matches(text, @"<item\s+ID=""(\d{5})""\s+name=""([^""]+)""");

                    int max = 10;

                    foreach(Match m in matches)
                    {
                        //if (max-- == 0) return;

                        int.TryParse(m.Groups[1].Value, out int id);
                        string name = m.Groups[2].Value;
                        string file = Path.Combine(dir, id.ToString(), "transfer.gsh");
                        if (transfer.ReadFromFile(file))
                        {
                            transfer.LoadUsage();
                            if (transfer.TotalUsage != 0)
                                Console.WriteLine($"------------ {id} {name}");




                        }
                        else
                        {
                            // unable to read file
                        }
                    }
                }
            }





            // 

        }

    }
}
