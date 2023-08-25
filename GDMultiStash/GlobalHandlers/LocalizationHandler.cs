﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GDMultiStash.GlobalHandlers
{
    internal partial class LocalizationHandler : Base.BaseHandler
    {

        public class Language
        {
            public string Code { get; private set; }
            public string Name { get; private set; }
            public string Text { get; private set; }
            public Language(string Code, string Text)
            {
                this.Code = Code;
                this.Text = Text;
                Dictionary<string, string> dict = GetDict();
                Name = dict["language_name"];
            }
            public Dictionary<string, string> GetDict()
            {
                return ParseDictionary(Text);
            }
            public static Dictionary<string, string> ParseDictionary(string lines)
            {
                Dictionary<string, string> dict = new Dictionary<string, string>();
                foreach(var kvp in Utils.Funcs.ReadDictionaryFromText(lines))
                {
                    if (kvp.Key.StartsWith("//")) continue;
                    dict[kvp.Key] = kvp.Value;

                }
                return dict;
            }
        }

        private readonly Dictionary<string, Language> _languages = new Dictionary<string, Language>();
        public StringsHolder Strings { get; private set; } = new StringsHolder();

        public Language[] Languages { get { return _languages.Values.ToArray(); } }

        public string CurrentCode => System.Globalization.CultureInfo.CurrentCulture.Name.Replace("-", string.Empty);

        public void AddLanguageFile(string langCode, string content)
        {
            Language lang = new Language(langCode, content);
            _languages[langCode.ToLower()] = lang;
            //_languages.Add(langCode.ToLower(), lang);
            Console.WriteLine($"Added language: {langCode} {lang.Name}");
        }

        public bool LoadLanguage(string langCode)
        {
            Console.WriteLine($"Loading language: {langCode}");
            langCode = langCode.ToLower();
            if (_languages.ContainsKey(langCode))
            {
                Strings._.Clear();
                foreach (KeyValuePair<string, string> kv in _languages[langCode].GetDict())
                    Strings._.Add(kv.Key, kv.Value);
                Console.WriteLine("- OK");
                InvokeLanguageLoaded();
                return true;
            }
            Console.WriteLine("- Not found");
            return false;
        }

    }
}
