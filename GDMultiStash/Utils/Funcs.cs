using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    internal class Funcs
    {
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

        public static Dictionary<string, string> ReadDictionaryFromFile(string file, char splitby = '=')
        {
            var kvp = new Dictionary<string, string>();
            foreach (var line in File.ReadAllLines(file))
            {
                var split = line.Split(splitby);
                if (split.Length < 2) continue;
                var key = split[0].Trim();
                var val = String.Join("=", split.Skip(1).ToArray()).Trim();
                kvp.Add(key, val);
            }
            return kvp;
        }

        public static Dictionary<string, string> ReadDictionaryFromText(string text, char splitby = '=')
        {
            var kvp = new Dictionary<string, string>();
            using (StringReader sr = new StringReader(text))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    var split = line.Split(splitby);
                    if (split.Length < 2) continue;
                    var key = split[0].Trim();
                    var val = String.Join("=", split.Skip(1).ToArray()).Trim();
                    kvp.Add(key, val);
                }
            }
            return kvp;
        }

        public static Dictionary<string, string> ReadDictionaryFromStream(Stream stream, char splitby = '=')
        {
            var kvp = new Dictionary<string, string>();
            foreach (var line in ReadLines(() => stream, Encoding.UTF8).ToList())
            {
                var split = line.Split(splitby);
                if (split.Length < 2) continue;
                var key = split[0].Trim();
                var val = String.Join("=", split.Skip(1).ToArray()).Trim();
                kvp.Add(key, val);
            }
            return kvp;
        }

        public static IEnumerable<string> ReadLines(Func<Stream> streamProvider, Encoding encoding)
        {
            using (var stream = streamProvider())
            using (var reader = new StreamReader(stream, encoding))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }


    }

}
