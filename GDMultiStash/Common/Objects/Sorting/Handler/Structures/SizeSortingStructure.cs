using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMultiStash.Common.Objects.Sorting.Handler.Structures
{
    internal class SizeSortingStructure : SortingStructure<
        SortedDictionary<uint,
            SortedDictionary<uint,
                SortedDictionary<string,
                    List<GDIALib.Parser.Stash.Item>
                        >>>>
    {
        public override void FillSortedList(
            string sortString,
            GDIALib.Parser.Stash.Item item,
            Global.Database.ItemInfo itemInfo)
        {
            var itemBaseRecord = item.BaseRecord;
            var itemWidth = itemInfo.BaseRecordInfo.Width;
            var itemHeight = itemInfo.BaseRecordInfo.Height;
            if (!sortedList.ContainsKey(sortString))
                sortedList.Add(sortString, new SortedDictionary<uint, SortedDictionary<uint, SortedDictionary<string, List<GDIALib.Parser.Stash.Item>>>>());
            if (!sortedList[sortString].ContainsKey(itemWidth))
                sortedList[sortString].Add(itemWidth, new SortedDictionary<uint, SortedDictionary<string, List<GDIALib.Parser.Stash.Item>>>());
            if (!sortedList[sortString][itemWidth].ContainsKey(itemHeight))
                sortedList[sortString][itemWidth].Add(itemHeight, new SortedDictionary<string, List<GDIALib.Parser.Stash.Item>>());
            if (!sortedList[sortString][itemWidth][itemHeight].ContainsKey(item.BaseRecord))
                sortedList[sortString][itemWidth][itemHeight].Add(item.BaseRecord, new List<GDIALib.Parser.Stash.Item>());
            sortedList[sortString][itemWidth][itemHeight][itemBaseRecord].Add(item);
        }

        public override List<GDIALib.Parser.Stash.StashTab> GetTabsQueue(
            string sortString, 
            uint stashWidth,
            uint stashHeight)
        {
            var tabsQueue = new List<GDIALib.Parser.Stash.StashTab>();

            foreach (var widths in sortedList[sortString].Reverse()) // from large to small width
            {
                foreach (var heights in widths.Value.Reverse()) // from large to small height
                {

                    var itemWidth = widths.Key;
                    var itemHeight = heights.Key;

                    uint posX = 0;
                    uint posY = 0;
                    GDIALib.Parser.Stash.StashTab currentTab = null;

                    foreach (var records in heights.Value)
                    {
                        foreach (var item in records.Value)
                        {
                            if (currentTab == null || posY + itemHeight > stashHeight)
                            {
                                currentTab = new GDIALib.Parser.Stash.StashTab();
                                tabsQueue.Add(currentTab);
                                posX = 0;
                                posY = 0;
                            }
                            item.X = posX;
                            item.Y = posY;
                            currentTab.Items.Add(item);
                            posX += itemWidth;
                            if (posX + itemWidth > stashWidth)
                            {
                                posX = 0;
                                posY += itemHeight;
                            }
                        }
                    }
                }
            }
            return tabsQueue;
        }
    }
}
