using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace GrimDawnLib
{

    public static partial class GrimDawn
    {
        public static partial class Options
        {

            private static readonly string optionsFile = Path.Combine(DocumentsSettingsPath, "options.txt");
            private static long lastWriteTicks = 0;
            private static Dictionary<string, string> opts;

            static Options()
            {
                FileChanged();
                opts = LoadOptions(optionsFile);
            }

            public static string GetOptionValue(string k)
            {
                if (FileChanged()) opts = LoadOptions(optionsFile);
                return opts.TryGetValue(k, out var value) ? value : null;
            }

            private static bool FileChanged()
            {
                if (!File.Exists(optionsFile)) return false;
                long curWriteTicks = new FileInfo(optionsFile).LastWriteTime.Ticks;
                if (curWriteTicks != lastWriteTicks)
                {
                    lastWriteTicks = curWriteTicks;
                    return true;
                }
                return false;
            }

            public static Dictionary<string, string> LoadOptions(string filePath)
            {
                var opts1 = Utils.Funcs.ReadDictionaryFromFile(filePath);
                var opts2 = new Dictionary<string, string>();
                foreach (var kvp in opts1)
                {
                    opts2[kvp.Key] = kvp.Value.Trim('"');
                }
                return opts2;
            }

        }
    }
}
