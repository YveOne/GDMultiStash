using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GrimDawnLib;

namespace GDMultiStash.Common
{
    internal partial class TransferFile
    {
        public struct Values
        {
            public bool IsExpansion1;
            public uint Width;
            public uint Height;
            public uint MaxTabs;
        }

        private static Dictionary<GrimDawnGameExpansion, Values> ValuesDict { get; } = new Dictionary<GrimDawnGameExpansion, Values> {
                { GrimDawnGameExpansion.Vanilla, new Values {
                    IsExpansion1 = false,
                    Width = 8,
                    Height = 16,
                    MaxTabs = 4,
                } },
                { GrimDawnGameExpansion.AoM, new Values {
                    IsExpansion1 = true,
                    Width = 10,
                    Height = 18,
                    MaxTabs = 5,
                } },
                { GrimDawnGameExpansion.FG, new Values {
                    IsExpansion1 = false,
                    Width = 10,
                    Height = 18,
                    MaxTabs = 6,
                } },
            };

        public static Values GetStashInfoForExpansion(GrimDawnGameExpansion exp)
        {
            if (ValuesDict.TryGetValue(exp, out Values v))
            {
                return new Values
                {
                    IsExpansion1 = v.IsExpansion1,
                    Width = v.Width,
                    Height = v.Height,
                    MaxTabs = v.MaxTabs,
                };
            }
            return new Values
            {
                IsExpansion1 = false,
                Width = 0,
                Height = 0,
                MaxTabs = 0,
            };
        }

    }
}
