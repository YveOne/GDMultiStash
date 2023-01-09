using System;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Runtime.InteropServices;

namespace GDMultiStash
{
    internal class Console
    {

        private static TextWriter writer = null;

        public static void CreateConsole()
        {
            writer = new StreamWriter(System.Console.OpenStandardOutput(), Encoding.UTF8)
            { AutoFlush = true };
            writer.WriteLine();
            System.Console.SetOut(writer);
            //System.Console.OutputEncoding = Encoding.UTF8;
            //Native.AttachConsole(Native.ATTACH_PARRENT);
        }

        public static void DestroyConsole()
        {
            if (writer == null) return;
            writer.Close();
            writer = null;
        }

        public static void WriteLine(object text, params object[] args)
        {
            if (writer == null) return;
            string t = DateTime.Now.ToString("HH:mm:ss.fff");
            string s = $"[{t}] " + string.Format(text.ToString(), args);
            System.Console.WriteLine(s);
        }

        public static DialogResult Alert(string msg, MessageBoxButtons btn = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.None)
        {
            string msg2 = string.Join(Environment.NewLine, msg.Split(new string[] { Environment.NewLine }, StringSplitOptions.None)
                .ToList()
                .Select((s, i) => (i == 0 ? "" : "    ") + s.Trim()));
            if (icon == MessageBoxIcon.None)
                WriteLine($"[MessageBox] {msg2}");
            else
                WriteLine($"[MessageBox] [{icon}] {msg2}");
            return MessageBox.Show(msg, icon == MessageBoxIcon.None ? "" : $"{icon}", btn, icon);
        }

        public static bool Question(string msg, MessageBoxIcon icon = MessageBoxIcon.Question)
        {
            return Alert(msg, MessageBoxButtons.YesNo, icon) == DialogResult.Yes;
        }

        public static bool Confirm(string msg, MessageBoxIcon icon = MessageBoxIcon.Question)
        {
            return Alert(msg, MessageBoxButtons.OKCancel, icon) == DialogResult.OK;
        }

        public static void Warning(string msg)
        {
            Alert(msg, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void Error(string msg)
        {
            Alert(msg, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

    }
}
