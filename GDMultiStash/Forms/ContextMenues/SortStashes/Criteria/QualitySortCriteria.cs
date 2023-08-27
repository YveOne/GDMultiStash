using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMultiStash.Forms.ContextMenues.SortStashes.Criteria
{
    internal class QualitySortCriteria : SortCriteria
    {
        public override string FormatKey(uint k) => k.ToString().PadLeft(2, '0');
        public override uint GetKey(GlobalHandlers.DatabaseHandler.ItemInfo itemInfo)
        {
            switch (itemInfo.ShownQuality)
            {
                case "None": return 1; // ?
                case "Common": return 2; // white
                case "Magical": return 3; // yellow
                case "Rare": return 4; // green
                case "Epic": return 5; // blue
                case "Legendary": return 6; // purple
                case "Quest": return 7;
                case "Broken": return 8;
            }
            return 0; // Unknown
        }
        public override string GetText(uint k)
        {
            switch (k)
            {
                case 1: return Global.L.ItemQualityNone();
                case 2: return Global.L.ItemQualityCommon();
                case 3: return Global.L.ItemQualityMagical();
                case 4: return Global.L.ItemQualityRare();
                case 5: return Global.L.ItemQualityEpic();
                case 6: return Global.L.ItemQualityLegendary();
                case 7: return Global.L.ItemQualityQuest();
                case 8: return Global.L.ItemQualityBroken();
            }
            return Global.L.ItemQualityUnknown();
        }
    }
}
