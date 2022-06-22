using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GDMultiStash
{
    internal static partial class Core
    {
        public static class Localization
        {

            public static readonly string CurrentLangCode2 = System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
            public static readonly string CurrentLangCode4 = System.Globalization.CultureInfo.CurrentCulture.Name.Replace("-", string.Empty);

            public struct Language
            {
                public string File;
                public string Code;
                public string Name;
            }

            private static readonly Dictionary<string, Language> _languages = new Dictionary<string, Language>();
            private static readonly Dictionary<string, string> _strings = new Dictionary<string, string>();

            public static Language[] GetLanguages()
            {
                return _languages.Values.ToArray();
            }

            public static void SaveDefaultFile(string filename, string content)
            {
                string filepath = Path.Combine(Files.DataLocalesDirPath, filename);
                if (!File.Exists(filepath)) File.WriteAllText(filepath, content, Encoding.UTF8);
                else
                {
                    // update existing file
                    Dictionary<string, string> curDict = ParseDictionary(File.ReadAllText(filepath));
                    Dictionary<string, string> newDict = ParseDictionary(content);
                    Dictionary<string, string> addDict = new Dictionary<string, string>();
                    foreach(KeyValuePair<string,string> kv in newDict) {
                        if (!curDict.ContainsKey(kv.Key))
                            addDict.Add(kv.Key, kv.Value);
                    }
                    if (addDict.Count != 0)
                    {
                        List<string> addLines = new List<string>();
                        addLines.Add("");
                        foreach(KeyValuePair<string, string> kv in addDict)
                            addLines.Add(string.Format("{0} = {1}", kv.Key, kv.Value));
                        File.AppendAllLines(filepath, addLines);
                    }
                }
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

            public static void LoadLanguages()
            {

                SaveDefaultFile("enGB-English (GB).txt", Properties.Resources.local_enGB);
                SaveDefaultFile("enUS-English (US).txt", Properties.Resources.local_enUS);
                SaveDefaultFile("deDE-Deutsch.txt", Properties.Resources.local_deDE);
                SaveDefaultFile("zhCN-简体中文.txt", Properties.Resources.local_zhCN);

                Console.WriteLine("Loading languages...");
                string fileName;
                List<string> fileNameParts;
                string langCode;
                string langName;
                foreach (string fullPath in Directory.GetFiles(Files.DataLocalesDirPath))
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

            private static void LoadLanguage(string langCode, bool useFallback)
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
                    foreach (KeyValuePair<string,string> kv in ParseDictionary(lines))
                        _strings[kv.Key] = kv.Value.Replace("\\n", Environment.NewLine);
                    Console.WriteLine("- OK");
                }
                else
                {
                    if (useFallback)
                    {
                        Console.WriteLine("- Fallback to default: en");
                        LoadLanguage("en", false);
                    }
                }
            }

            public static void LoadLanguage(string langCode)
            {
                LoadLanguage(langCode, true);
            }

            public class StringsProxy
            {
                public string this[string key]
                {
                    get
                    {
                        return GetString(key);
                    }
                }

                public string Format(string key, params string[] l)
                {
                    return Localization.Format(key, l);
                }

            }

            public static string GetString(string key)
            {
                if (_strings.ContainsKey(key)) return _strings[key];
                Console.WriteLine("Unknown localized string: " + key);
                return "{" + key + "}";
            }

            public static string Format(string key, params string[] l)
            {
                return string.Format(GetString(key), l);
            }

        }
    }
}
