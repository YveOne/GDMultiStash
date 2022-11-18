
namespace GDMultiStash
{
    internal class Console
    {

        public delegate void WriteLineEventHandler(string line);
        public static event WriteLineEventHandler OnWriteLine;

        public static void WriteLine(string text, params string[] args)
        {
            string t = System.DateTime.Now.ToString("HH:mm:ss.fff");
            string s = "[" + t + "] " + string.Format(text, args);
            System.Console.WriteLine(s);
            OnWriteLine?.Invoke(s);
        }

    }
}
