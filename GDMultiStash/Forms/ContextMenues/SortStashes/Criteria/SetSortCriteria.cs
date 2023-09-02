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

        private GlobalHandlers.LocalizationHandler.GameLanguage gameLanguage;
        private readonly Dictionary<string, string> setNamesDict = new Dictionary<string, string>();
        private readonly List<string> sortedSetNames = new List<string>();

        public SetSortCriteria()
        {
            var gameLangname = GrimDawn.Options.GetOptionValue("language");
            if (Global.Localization.GetGameLanguage(gameLangname, out gameLanguage))
            {
                foreach(var kvp in gameLanguage.ItemSetNames)
                {
                    setNamesDict.Add(kvp.Key, kvp.Value);
                    sortedSetNames.Add(kvp.Value);
                }
                sortedSetNames.Sort();
            }
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
