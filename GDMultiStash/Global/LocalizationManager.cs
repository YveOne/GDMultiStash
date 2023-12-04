using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace GDMultiStash.Global
{
    using Localization;
    namespace Localization
    {

    }

    internal partial class LocalizationManager : Base.Manager
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

        public class GameLanguage
        {
            public string TextFileName;
            public Dictionary<string, string> ItemSetNames;
        }

        private readonly Dictionary<string, Language> _languages = new Dictionary<string, Language>();
        private readonly Dictionary<string, GameLanguage> _gameLanguages = new Dictionary<string, GameLanguage>();

        public StringsHolder Strings { get; private set; } = new StringsHolder();

        public Language[] Languages { get { return _languages.Values.ToArray(); } }

        public string CurrentCode => System.Globalization.CultureInfo.CurrentCulture.Name.Replace("-", string.Empty);

        public void AddLanguageFile(string langCode)
        {
            var resourceName = $"local_{langCode}";
            var resourceText = Properties.Resources.ResourceManager.GetString(resourceName);
            if (resourceText == null)
            {
                Console.AlertError($"Failed reading resource {resourceName}");
                return;
            }
            var resourceFile = resourceName.Replace("local_", "");
            resourceFile = Path.Combine(C.LocalesPath, $"{resourceFile}.txt");
            resourceText = G.Resources.WriteReadExternalResource(resourceText, resourceFile, '=');

            Language lang = new Language(langCode, resourceText);
            _languages[langCode.ToLower()] = lang;
            //_languages.Add(langCode.ToLower(), lang);
            Console.WriteLine($"Added language: {langCode} {lang.Name}");
        }

        public void AddLanguageFilesFrom(string dir)
        {
            foreach (var file in Directory.GetFiles(dir))
            {
                var langCode = Path.GetFileNameWithoutExtension(file).ToLower();
                if (!_languages.ContainsKey(langCode))
                {
                    Language lang = new Language(langCode, File.ReadAllText(file));
                    _languages[langCode.ToLower()] = lang;
                    Console.WriteLine($"Added custom language: {langCode} {lang.Name}");
                }
            }
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

        public void LoadGameLanguage(string textFileName, string languageName)
        {
            Console.WriteLine($"Loading game language: {textFileName}");

            var gameLang = new GameLanguage();
            gameLang.TextFileName = textFileName;
            gameLang.ItemSetNames = new Dictionary<string, string>();

            var setNamesResourceText = Properties.Resources.ResourceManager.GetString($"{textFileName}_setnames");
            if (setNamesResourceText != null)
            {
                foreach (var line in Utils.Funcs.ReadTextLinesIter(setNamesResourceText))
                {
                    var splits = line.Split('|');
                    if (splits.Length < 2) continue;
                    var setNameTag = splits[0].Trim();
                    var setName = splits[1].Trim();
                    gameLang.ItemSetNames[setNameTag] = setName;
                }
                Console.WriteLine($"- {gameLang.ItemSetNames.Count} item set names");
            }

            _gameLanguages[languageName] = gameLang;
        }

        public bool GetGameLanguage(string langName, out GameLanguage gameLang)
        {
            return _gameLanguages.TryGetValue(langName, out gameLang);
        }

    }
}
