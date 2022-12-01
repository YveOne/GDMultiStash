using System.Windows.Forms;

namespace GDMultiStash
{
    internal class Console
    {

        public delegate void WriteLineEventHandler(string line);
        public static event WriteLineEventHandler OnWriteLine;

        public static void WriteLine(string text, params string[] args)
        {
            string t = System.DateTime.Now.ToString("HH:mm:ss.fff");
            string s = $"[{t}] " + string.Format(text, args);
            System.Console.WriteLine(s);
            OnWriteLine?.Invoke(s);
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
