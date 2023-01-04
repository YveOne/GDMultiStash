using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace GDMultiStash.GlobalHandlers
{
    internal class DatabaseHandler
    {

        public class ItemInfo
        {
            public int Width;
            public int Height;
            public int Level;
            public int Size => Width * Height;
        }

        private readonly Dictionary<string, ItemInfo> _itemInfos = new Dictionary<string, ItemInfo>();

        public int GetItemSize(string record, int def = 1)
        {
            record = Path.ChangeExtension(record, null);
            if (!_itemInfos.TryGetValue(record, out ItemInfo info)) return def;
            return info.Size;
        }

        public ItemInfo GetItemInfo(string record)
        {
            if (!_itemInfos.TryGetValue(record, out ItemInfo info)) return null;
            return info;
        }

        private void LoadItemInfos(string[] lines)
        {
            Console.WriteLine("Reading item sizes...");
            string[] splits;
            string record;
            foreach (string line in lines)
            {
                splits = line.Split(':');
                if (splits.Length != 4) continue;
                record = splits[0].Trim();
                if (record.StartsWith("//")) continue;
                if (!int.TryParse(splits[1], out int width)) continue;
                if (!int.TryParse(splits[2], out int height)) continue;
                if (!int.TryParse(splits[3], out int level)) continue;
                _itemInfos[record] = new ItemInfo()
                {
                    Width = width,
                    Height = height,
                    Level = level,
                };
            }
            Console.WriteLine("- Found " + _itemInfos.Count + " records");
        }

        public void LoadItemInfos(string lines)
        {
            LoadItemInfos(lines.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries));
        }

    }
}
