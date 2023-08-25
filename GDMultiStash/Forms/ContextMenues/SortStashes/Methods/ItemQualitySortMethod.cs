using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMultiStash.Forms.ContextMenues.SortStashes.Methods
{
    internal class ItemQualitySortMethod : ItemSortMethod<int>
    {
        public override string GroupText => Global.L.GroupSortByQualityName();

        public override int GetSortKey(GlobalHandlers.DatabaseHandler.ItemInfo itemInfo)
        {
            switch (itemInfo.ShownQuality)
            {
                case "None": return 0; // ?
                case "Common": return 1; // white
                case "Magical": return 2; // yellow
                case "Rare": return 3; // green
                case "Epic": return 4; // blue
                case "Legendary": return 5; // purple
                case "Quest": return 6;
                case "Broken": return 999;
            }
            return 9999; // Unknown
        }

        public override int[] IgnoreKeys => new int[] { };

        public override string GetText(int key)
        {
            switch (key)
            {
                case 0: return Global.L.ItemQualityNone();
                case 1: return Global.L.ItemQualityCommon();
                case 2: return Global.L.ItemQualityMagical();
                case 3: return Global.L.ItemQualityRare();
                case 4: return Global.L.ItemQualityEpic();
                case 5: return Global.L.ItemQualityLegendary();
                case 6: return Global.L.ItemQualityQuest();
                case 99: return Global.L.ItemQualityBroken();
            }
            return Global.L.ItemQualityUnknown();
        }
    }
}
