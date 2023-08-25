using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

using GDMultiStash.Forms.ContextMenues.SortStashes.Methods;
namespace GDMultiStash.Forms.ContextMenues.SortStashes.Handlers
{
    internal class ItemRecordSortHandler<M> : SortHandler<M,
        // sortedList[sortKey][record][]
        SortedDictionary<string,
            List<GDIALib.Parser.Stash.Item>>>
    {
        private Dictionary<string, Size> recordItemSizes;

        public ItemRecordSortHandler(ItemSortMethod<M> method) : base(method)
        {
            Console.WriteLine("Auto sorting items (by record)");
            recordItemSizes = new Dictionary<string, Size>();
        }

        protected override void FillSortedList(
            M sortKey, 
            GDIALib.Parser.Stash.Item item,
            GlobalHandlers.DatabaseHandler.ItemInfo itemInfo)
        {

            var itemWidth = itemInfo.BaseRecordInfo.Width;
            var itemHeight = itemInfo.BaseRecordInfo.Height;
            if (!recordItemSizes.ContainsKey(item.BaseRecord))
                recordItemSizes[item.BaseRecord] = new Size((int)itemWidth, (int)itemHeight);
            if (!sortedList.ContainsKey(sortKey))
                sortedList.Add(sortKey, new SortedDictionary<string, List<GDIALib.Parser.Stash.Item>>());
            if (!sortedList[sortKey].ContainsKey(item.BaseRecord))
                sortedList[sortKey].Add(item.BaseRecord, new List<GDIALib.Parser.Stash.Item>());
            sortedList[sortKey][item.BaseRecord].Add(item);
        }

        protected override List<GDIALib.Parser.Stash.StashTab> GetTabsQueue(M sortKey, SortedDictionary<string,
            List<GDIALib.Parser.Stash.Item>> sortList,
            uint stashWidth, uint stashHeight)
        {
            var tabsQueue = new List<GDIALib.Parser.Stash.StashTab>();

            foreach (var records in sortList)
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
