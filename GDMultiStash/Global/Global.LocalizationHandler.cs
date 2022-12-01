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
            public string Code;
            public string Name;
            public Dictionary<string, string> Dict;
        }

        private readonly Dictionary<string, Language> _languages = new Dictionary<string, Language>();
        private readonly Dictionary<string, string> _strings = new Dictionary<string, string>();

        public Language[] Languages { get { return _languages.Values.ToArray();} }

        public void AddLanguageFile(string langCode, string content)
        {
            Dictionary<string, string> dict = ParseDictionary(content);
            string langName = dict["language_name"];
            Console.WriteLine($"Adding language: {langCode} {langName}");
            _languages.Add(langCode.ToLower(), new Language()
            {
                Name = langName,
                Code = langCode,
                Dict = dict,
            });
        }

        private void LoadLanguage(string langCode, bool useFallback = true)
        {
            Console.WriteLine("Loading language: " + langCode);
            Dictionary<string, string> dict = null;
            langCode = langCode.ToLower();
            if (_languages.ContainsKey(langCode))
            {
                // requested lang exists
                dict = _languages[langCode].Dict;
            }
            else
            {
                Console.WriteLine("- Not found");
            }
            if (dict != null)
            {
                foreach (KeyValuePair<string, string> kv in dict)
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
