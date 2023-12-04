using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMultiStash.Common.Objects.Sorting.Handler.Criteria
{
    internal class QualitySortCriteria : SortCriteria
    {
        public override string FormatKey(uint k) => k.ToString().PadLeft(2, '0');
        public override uint GetKey(Global.Database.ItemInfo itemInfo)
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
                case 1: return G.L.ItemQualityNone();
                case 2: return G.L.ItemQualityCommon();
                case 3: return G.L.ItemQualityMagical();
                case 4: return G.L.ItemQualityRare();
                case 5: return G.L.ItemQualityEpic();
                case 6: return G.L.ItemQualityLegendary();
                case 7: return G.L.ItemQualityQuest();
                case 8: return G.L.ItemQualityBroken();
            }
            return G.L.ItemQualityUnknown();
        }
    }
}
