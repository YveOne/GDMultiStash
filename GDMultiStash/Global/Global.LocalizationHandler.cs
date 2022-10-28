using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GDMultiStash.GlobalHandlers
{
    internal class LocalizationHandler
    {

        public struct Language
        {
            public string File;
            public string Code;
            public string Name;
        }

        private readonly Dictionary<string, Language> _languages = new Dictionary<string, Language>();
        private readonly Dictionary<string, string> _strings = new Dictionary<string, string>();

        public Language[] Languages { get { return _languages.Values.ToArray();} }

        public string GetLocalizationFile(string filename)
        {
            return Path.Combine(Global.FileSystem.LocalsDirectory, filename);
        }

        public void SaveDefaultFile(string filename, string content)
        {
            string filepath = GetLocalizationFile(filename);
            if (!File.Exists(filepath)) File.WriteAllText(filepath, content, Encoding.UTF8);
            else
            {
                // update existing file
                Dictionary<string, string> curDict = ParseDictionary(File.ReadAllText(filepath));
                Dictionary<string, string> newDict = ParseDictionary(content);
                Dictionary<string, string> addDict = new Dictionary<string, string>();
                foreach (KeyValuePair<string, string> kv in newDict)
                {
                    if (!curDict.ContainsKey(kv.Key))
                        addDict.Add(kv.Key, kv.Value);
                }
                if (addDict.Count != 0)
                {
                    List<string> addLines = new List<string> { "" };
                    foreach (KeyValuePair<string, string> kv in addDict)
                        addLines.Add(string.Format("{0} = {1}", kv.Key, kv.Value));
                    File.AppendAllLines(filepath, addLines);
                }
            }
        }

        public void LoadLanguages()
        {
            Console.WriteLine("Loading languages...");
            string fileName;
            List<string> fileNameParts;
            string langCode;
            string langName;
            foreach (string fullPath in Directory.GetFiles(Global.FileSystem.LocalsDirectory))
            {
                fileName = Path.GetFileNameWithoutExtension(fullPath).Trim();
                if (fileName.StartsWith("_")) continue;
                fileNameParts = fileName.Split('-').ToList();
                langCode = fileNameParts[0].Trim();
                fileNameParts.RemoveAt(0);
                langName = string.Join("-", fileNameParts).Trim();
                Console.WriteLine("- " + fileName);
                _languages.Add(langCode.ToLower(), new Language()
                {
                    File = fullPath,
                    Name = langName,
                    Code = langCode,
                });
            }
        }

        private void LoadLanguage(string langCode, bool useFallback = true)
        {
            Console.WriteLine("Loading language: " + langCode);
            string lines = null;
            langCode = langCode.ToLower();
            if (_languages.ContainsKey(langCode))
            {
                // requested lang exists
                lines = File.ReadAllText(_languages[langCode].File, Encoding.UTF8);
            }
            else
            {
                Console.WriteLine("- Not found");
            }
            if (lines != null)
            {
                foreach (KeyValuePair<string, string> kv in ParseDictionary(lines))
                    _strings[kv.Key] = ParseString(kv.Value);
                Console.WriteLine("- OK");
            }
            else
            {
                if (useFallback)
                {
                    Console.WriteLine("- Fallback to default: enUS");
                    LoadLanguage("enUS", false);
                }
            }
        }

        public void LoadLanguage(string langCode)
        {
            LoadLanguage(langCode, true);
        }

        public class StringsProxy
        {
            private readonly LocalizationHandler _l;

            public StringsProxy(LocalizationHandler l)
            {
                _l = l;
            }

            public string this[string key]
            {
                get
                {
                    return _l.GetString(key);
                }
            }

            public string Format(string key, params string[] l)
            {
                return _l.Format(key, l);
            }

        }

        public StringsProxy GetProxy()
        {
            return new StringsProxy(this);
        }

        public string GetString(string key)
        {
            if (_strings.ContainsKey(key)) return _strings[key];
            Console.WriteLine("Unknown localized string: " + key);
            return "{" + key + "}";
        }

        public string Format(string key, params string[] l)
        {
            return string.Format(GetString(key), l);
        }

        public static string GetCurrentCode()
        {
            return System.Globalization.CultureInfo.CurrentCulture.Name.Replace("-", string.Empty);
        }

        private static string ParseString(string str)
        {
            return str
                .Replace("\\n", Environment.NewLine);
        }

        private static Dictionary<string, string> ParseDictionary(string lines)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (string line in lines.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
            {
                string[] splits = line.Split('=');
                if (splits.Length == 2)
                {
                    string k = splits[0].Trim();
                    string v = splits[1].Trim();
                    if (k.StartsWith("//")) continue;
                    dict[k] = v;
                }
            }
            return dict;
        }

    }
}
