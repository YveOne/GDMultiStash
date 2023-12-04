using System;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

internal class Console
{

    public static void CreateConsole()
    {
        System.Console.OutputEncoding = Encoding.UTF8;
        Native.AttachConsole(Native.ATTACH_PARRENT);
    }

    public static void DestroyConsole()
    {
    }

    public static void WriteLine(object text, params object[] args)
    {
        var regex = new Regex(@"\{(\d+)\}");
        string t = DateTime.Now.ToString("HH:mm:ss.fff");
        string s = $"[{t}] " + regex.Replace(text.ToString(), m => {
            if (int.TryParse(m.Groups[1].Value, out int i))
            {
                var o = args.ElementAtOrDefault(i);
                if (o != null)
                {
                    return $"{o}";
                }
            }
            return m.Value;
        });
        System.Console.WriteLine(s);
    }

    public static DialogResult Alert(string msg, MessageBoxButtons btn = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.None)
    {
        string msg2 = string.Join(Environment.NewLine, msg.Split(new string[] { Environment.NewLine }, StringSplitOptions.None)
            .ToList()
            .Select((s, i) => (i == 0 ? "" : "    ") + s.Trim()));
        WriteLine($"[Alert:{icon}] {msg2}");
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

    public static void AlertWarning(string msg)
    {
        Alert(msg, MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }

    public static void AlertError(string msg)
    {
        Alert(msg, MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    public static void WriteWarning(string msg)
    {
        WriteLine($"[WARNING] {msg}");
    }

    public static void WriteError(string msg)
    {
        WriteLine($"[ERROR] {msg}");
    }

}
