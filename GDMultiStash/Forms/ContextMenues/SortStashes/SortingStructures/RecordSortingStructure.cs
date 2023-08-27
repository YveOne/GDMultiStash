using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace GDMultiStash.Forms.ContextMenues.SortStashes.SortingStructures
{
    internal class RecordSortingStructure : SortingStructure<
        SortedDictionary<string,
            List<GDIALib.Parser.Stash.Item>>>
    {
        private readonly Dictionary<string, Size> recordItemSizes = new Dictionary<string, Size>();

        public override void FillSortedList(
            string sortString,
            GDIALib.Parser.Stash.Item item,
            GlobalHandlers.DatabaseHandler.ItemInfo itemInfo)
        {
            var itemWidth = itemInfo.BaseRecordInfo.Width;
            var itemHeight = itemInfo.BaseRecordInfo.Height;
            if (!recordItemSizes.ContainsKey(item.BaseRecord))
                recordItemSizes[item.BaseRecord] = new Size((int)itemWidth, (int)itemHeight);

            if (!sortedList.ContainsKey(sortString))
                sortedList.Add(sortString, new SortedDictionary<string, List<GDIALib.Parser.Stash.Item>>());
            if (!sortedList[sortString].ContainsKey(item.BaseRecord))
                sortedList[sortString].Add(item.BaseRecord, new List<GDIALib.Parser.Stash.Item>());
            sortedList[sortString][item.BaseRecord].Add(item);
        }

        public override List<GDIALib.Parser.Stash.StashTab> GetTabsQueue(
            string sortString,
            uint stashWidth,
            uint stashHeight)
        {
            var tabsQueue = new List<GDIALib.Parser.Stash.StashTab>();

            foreach (var records in sortedList[sortString])
            {

                uint posX = 0;
                uint posY = 0;
                GDIALib.Parser.Stash.StashTab currentTab = null;

                foreach (var item in records.Value)
                {
                    var itemSize = recordItemSizes[records.Key];
                    if (currentTab == null || posY + itemSize.Height > stashHeight)
                    {
                        currentTab = new GDIALib.Parser.Stash.StashTab();
                        tabsQueue.Add(currentTab);
                        posX = 0;
                        posY = 0;
                    }
                    item.X = posX;
                    item.Y = posY;
                    currentTab.Items.Add(item);
                    posX += (uint)itemSize.Width;
                    if (posX + itemSize.Width > stashWidth)
                    {
                        posX = 0;
                        posY += (uint)itemSize.Height;
                    }
                }
            }

            return tabsQueue;
        }

    }
}
