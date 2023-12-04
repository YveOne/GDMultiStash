using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;


namespace Utils
{
    internal static class Funcs
    {
        public static bool IsDirectoryEmpty(string path)
        {
            if (!Directory.Exists(path)) return true;
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        public static void RunThread(int sleep, Action action)
        {
            new System.Threading.Thread(() => {
                System.Threading.Thread.Sleep(sleep);
                action();
            }).Start();
        }

        public static void Invoke(System.Windows.Forms.Control control, Action action)
        {
            control.Invoke((System.Windows.Forms.MethodInvoker)(() => {
                action();
            }));
        }

        public delegate bool WaitForConditionDelegate();

        public static bool WaitFor(WaitForConditionDelegate condition, int time, int delay = 1)
        {
            long timeout = Environment.TickCount + time;
            while (Environment.TickCount < timeout)
            {
                if (condition()) return true;
                long timeout2 = Environment.TickCount + delay;
                while (Environment.TickCount < timeout2)
                {
                    System.Threading.Thread.Sleep(1);
                }
            }
            return false;
        }

        public static void OpenUrl(string url)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = url;
            process.StartInfo = startInfo;
            process.Start();
        }

        public static string GetTempFileName()
        {
            return Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        }

        public static IEnumerable<string> ReadFileLinesIter(string file)
        {
            foreach (var line in File.ReadAllLines(file))
            {
                yield return line;
            }
        }

        public static IEnumerable<string> ReadTextLinesIter(string text)
        {
            using (StringReader sr = new StringReader(text))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }

        public static IEnumerable<string> ReadStreamLinesIter(Stream stream, string encodingName = "UTF-8")
        {
            var encoding = Encoding.GetEncoding(encodingName);
            using (var reader = new StreamReader(stream, encoding))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }

        public static bool GetKeyValueFromString(string line, out KeyValuePair<string, string> kvp, char splitBy = '=')
        {
            kvp = new KeyValuePair<string, string>();
            var split = line.Split(splitBy);
            if (split.Length < 2) return false;
            var key = split[0].Trim();
            if (key.StartsWith("#")) return false;
            if (key.StartsWith("//")) return false;
            if (key.StartsWith("--")) return false;
            var val = String.Join("=", split.Skip(1).ToArray()).Trim();
            kvp = new KeyValuePair<string, string>(key, val);
            return true;
        }

        public delegate IEnumerable<string> ReadDictionaryIterDelegate();

        public static Dictionary<string, string> ReadDictionaryFromIter(ReadDictionaryIterDelegate iter, char splitby = '=')
        {
            var dict = new Dictionary<string, string>();
            foreach (var line in iter())
            {
                if (GetKeyValueFromString(line, out var kvp, splitby))
                    dict[kvp.Key] = kvp.Value;
            }
            return dict;
        }

        public static Dictionary<string, string> ReadDictionaryFromFile(string file, char splitby = '=')
        {
            return ReadDictionaryFromIter(() => ReadFileLinesIter(file), splitby);
        }

        public static Dictionary<string, string> ReadDictionaryFromText(string text, char splitby = '=')
        {
            return ReadDictionaryFromIter(() => ReadTextLinesIter(text), splitby);
        }

        public static Dictionary<string, string> ReadDictionaryFromStream(Stream stream, char splitby = '=')
        {
            return ReadDictionaryFromIter(() => ReadStreamLinesIter(stream), splitby);
        }

        public static string[] StringLines(string str)
            => str.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

    }

}
