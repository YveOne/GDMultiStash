using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMultiStash.Forms.ContextMenues.SortStashes.Methods
{
    internal class ItemSet1SortMethod : ItemSetSortMethod
    {

        public override string GroupText => Global.L.GroupSortBySet1Name();

        public override string GetSortKey(GlobalHandlers.DatabaseHandler.ItemInfo itemInfo)
        {
            var setRecord = itemInfo.BaseRecordInfo.ItemSetRecord;
            if (setRecord == "") return "Z__NO_SET"; // no set item

            if (!Global.Database.GetItemSetRecordInfo(setRecord, out GlobalHandlers.DatabaseHandler.ItemSetRecordInfo recordInfo))
                return "ZZ__NO_RECORD"; // error

            if (!setNamesDict.TryGetValue(recordInfo.ItemSetNameKey, out string setName))
            {
                Console.WriteLine("unknown set name: " + recordInfo.ItemSetNameKey);
                return "ZZZ__UNKNOWN_RECORD"; // error
            }

            return $"{setName}/{itemInfo.ShownLevelRequirement.ToString().PadLeft(3, '0')}";
        }

        public override string[] IgnoreKeys => new string[] { "Z__NO_SET", "ZZ__NO_RECORD", "ZZZ__UNKNOWN_RECORD" };

        public override string GetText(string key)
        {
            // todo: localize me
            if (key == "Z__NO_SET") return "NO SET";
            if (key == "ZZ__NO_RECORD") return "RECORD NOT FOUND";
            if (key == "ZZZ__UNKNOWN_RECORD") return "SETNAME NOT FOUND";

            var split = key.Split('/');
            var setName = split[0];
            var itemLevel = int.Parse(split[1]);

            return $"{setName} ({itemLevel})";
        }
    }
}
