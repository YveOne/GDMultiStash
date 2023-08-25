using System;
using System.Collections.Generic;
using System.Linq;

using GDMultiStash.Forms.ContextMenues.SortStashes.Methods;
namespace GDMultiStash.Forms.ContextMenues.SortStashes.Handlers
{
    internal class ItemSizeSortHandler<M> : SortHandler<M,
        // sortedList[sortKey][width][height][record][]
        SortedDictionary<uint,
            SortedDictionary<uint,
                SortedDictionary<string,
                    List<GDIALib.Parser.Stash.Item>>>>>
    {

        public ItemSizeSortHandler(ItemSortMethod<M> method) : base(method)
        {
            Console.WriteLine("Auto sorting items (by width>height>record)");
        }

        protected override void FillSortedList(
            M sortKey, 
            GDIALib.Parser.Stash.Item item,
            GlobalHandlers.DatabaseHandler.ItemInfo itemInfo)
        {
            var itemBaseRecord = item.BaseRecord;
            var itemWidth = itemInfo.BaseRecordInfo.Width;
            var itemHeight = itemInfo.BaseRecordInfo.Height;
            if (!sortedList.ContainsKey(sortKey))
                sortedList.Add(sortKey, new SortedDictionary<uint, SortedDictionary<uint, SortedDictionary<string, List<GDIALib.Parser.Stash.Item>>>>());
            if (!sortedList[sortKey].ContainsKey(itemWidth))
                sortedList[sortKey].Add(itemWidth, new SortedDictionary<uint, SortedDictionary<string, List<GDIALib.Parser.Stash.Item>>>());
            if (!sortedList[sortKey][itemWidth].ContainsKey(itemHeight))
                sortedList[sortKey][itemWidth].Add(itemHeight, new SortedDictionary<string, List<GDIALib.Parser.Stash.Item>>());
            if (!sortedList[sortKey][itemWidth][itemHeight].ContainsKey(item.BaseRecord))
                sortedList[sortKey][itemWidth][itemHeight].Add(item.BaseRecord, new List<GDIALib.Parser.Stash.Item>());
            sortedList[sortKey][itemWidth][itemHeight][itemBaseRecord].Add(item);
        }

        protected override List<GDIALib.Parser.Stash.StashTab> GetTabsQueue(M sortKey, SortedDictionary<uint,
            SortedDictionary<uint,
                SortedDictionary<string,
                    List<GDIALib.Parser.Stash.Item>>>> sortList,
            uint stashWidth, uint stashHeight)
        {
            var tabsQueue = new List<GDIALib.Parser.Stash.StashTab>();

            foreach (var widths in sortList.Reverse()) // from large to small width
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
