using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.IO.Compression;

using GrimDawnLib;

namespace GDMultiStash.Forms.ContextMenues.SortStashes.Criteria
{
    internal class SetSortCriteria : SortCriteria
    {

        private readonly Dictionary<string, string> setNamesDict = new Dictionary<string, string>();
        private readonly List<string> sortedSetNames = new List<string>();

        public SetSortCriteria()
        {
            ReadLanguageZip();
            sortedSetNames.Sort();
        }
        private void ReadLanguageZip()
        {
            var langName = GrimDawn.Options.GetOptionValue("language");
            if (langName != null)
            {
                var langsDir = Path.Combine(Global.Configuration.Settings.GamePath, "localization");
                foreach (string langZip in Directory.GetFiles(langsDir, "*.zip"))
                {
                    using (ZipArchive zip = ZipFile.OpenRead(langZip))
                    {
                        var entry = zip.GetEntry("language.def");
                        if (entry == null) continue; // invalid lang file?
                        var col = Utils.Funcs.ReadDictionaryFromStream(entry.Open());
                        if (col["language"] == langName)
                        {
                            AddDict(zip.GetEntry("tags_items.txt"));
                            AddDict(zip.GetEntry("aom/tagsgdx1_items.txt"));
                            AddDict(zip.GetEntry("aom/tagsgdx1_storyelements.txt")); // lokars prey
                            AddDict(zip.GetEntry("fg/tagsgdx2_items.txt"));
                            return;
                        }
                    }
                }
            }
        }
        private void AddDict(ZipArchiveEntry entry)
        {
            if (entry == null) return; // invalid lang file?
            foreach (var kvp in Utils.Funcs.ReadDictionaryFromStream(entry.Open()))
                AddDictKVP(kvp);
        }
        private void AddDictKVP(KeyValuePair<string, string> kvp)
        {
            if (!(
                   kvp.Key.StartsWith("tagItemSet")
                || kvp.Key.StartsWith("tagGDX1ItemSet")
                || kvp.Key.StartsWith("tagGDX2ItemSet")
                || kvp.Key.StartsWith("tagGDX1_NPC_S_Set") // lokars prey
            ) || kvp.Key.EndsWith("Desc")) return; // get only names, not descriptions

            setNamesDict.Add(kvp.Key, kvp.Value);
            sortedSetNames.Add(kvp.Value);
        }

        public override string FormatKey(uint k) => k.ToString().PadLeft(5, '0');
        public override uint GetKey(GlobalHandlers.DatabaseHandler.ItemInfo itemInfo)
        {
            var setRecord = itemInfo.BaseRecordInfo.ItemSetRecord;
            if (setRecord == "")
                return 0; // no set item
            if (!Global.Database.GetItemSetRecordInfo(setRecord, out GlobalHandlers.DatabaseHandler.ItemSetRecordInfo recordInfo))
            {
                Console.WriteLine("unknown setrecord: " + setRecord);
                return 0; // error
            }
            if (!setNamesDict.TryGetValue(recordInfo.ItemSetNameKey, out string setName))
            {
                Console.WriteLine("unknown setname: " + recordInfo.ItemSetNameKey);
                return 0; // error
            }
            return (uint)(sortedSetNames.FindIndex(n => n == setName) + 1);
        }
        public override string GetText(uint i)
        {
            return sortedSetNames[(int)i - 1];
        }

    }
}
