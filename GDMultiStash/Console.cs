
namespace GDMultiStash
{
    internal class Console
    {

        public static void WriteLine(string text, params string[] args)
        {
            string t = System.DateTime.Now.ToString("HH:mm:ss.fff");
            System.Console.WriteLine("["+t+"] " + text, args);
        }

    }
}
