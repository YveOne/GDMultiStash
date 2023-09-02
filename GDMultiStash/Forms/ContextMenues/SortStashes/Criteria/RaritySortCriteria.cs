﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMultiStash.Forms.ContextMenues.SortStashes.Criteria
{
    internal class RaritySortCriteria : SortCriteria
    {
        // normal = 0
        // MI+ = 1
        // MI++ = 2
        // DR = 3
        // DR+ = 4
        // DR++ = 5

        public override string FormatKey(uint k) => k.ToString().PadLeft(2, '0');
        public override uint GetKey(GlobalHandlers.DatabaseHandler.ItemInfo itemInfo)
        {
            uint score = 0;
            if (itemInfo.BaseRecordInfo.Quality == "Rare")
                score = 3;
            if (itemInfo.PrefixRecordInfo != null && itemInfo.PrefixRecordInfo.Quality == "Rare")
                score += 1;
            if (itemInfo.SuffixRecordInfo != null && itemInfo.SuffixRecordInfo.Quality == "Rare")
                score += 1;
            return score;
        }
        public override string GetText(uint k)
        {
            switch (k)
            {
                case 1: return "M+";
                case 2: return "M++";
                case 3: return "DR";
                case 4: return "DR+";
                case 5: return "DR++";
            }
            return "NORMAL RARITY"; // Global.L.ItemQualityUnknown();
        }
    }
}
