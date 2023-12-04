using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.IO.Compression;

using GrimDawnLib;

namespace GDMultiStash.Common.Objects.Sorting.Handler.Criteria
{
    internal class SetSortCriteria : SortCriteria
    {

        private Global.LocalizationManager.GameLanguage gameLanguage;
        private readonly Dictionary<string, string> setNamesDict = new Dictionary<string, string>();
        private readonly List<string> sortedSetNames = new List<string>();

        public SetSortCriteria()
        {
            var gameLangname = GrimDawn.Options.GetOptionValue("language");
            if (G.Localization.GetGameLanguage(gameLangname, out gameLanguage))
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
        public override uint GetKey(Global.Database.ItemInfo itemInfo)
        {
            var setRecord = itemInfo.BaseRecordInfo.ItemSetRecord;
            if (setRecord == "")
                return 0; // no set item
            if (!G.Database.GetItemSetRecordInfo(setRecord, out Global.Database.ItemSetRecordInfo recordInfo))
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
