using System.Windows.Forms;

namespace GDMultiStash
{
    internal class Console
    {

        public static void WriteLine(object text, params object[] args)
        {
            string t = System.DateTime.Now.ToString("HH:mm:ss.fff");
            string s = $"[{t}] " + string.Format(text.ToString(), args);
            System.Console.WriteLine(s);
        }

        public static void Alert(string msg, MessageBoxIcon icon = MessageBoxIcon.None)
        {
            if (icon == MessageBoxIcon.None)
                WriteLine($"{msg}");
            else
                WriteLine($"[{icon}] {msg}");
            MessageBox.Show(msg, icon == MessageBoxIcon.None ? "" : $"{icon}", MessageBoxButtons.OK, icon);
        }

        public static void Warning(string msg)
        {
            Alert(msg, MessageBoxIcon.Warning);
        }

    }
}
