using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GDMultiStash.GlobalHandlers
{
    internal class DatabaseHandler
    {

        #region Item Sizes 

        private readonly Dictionary<string, int> _itemSizes = new Dictionary<string, int>();

        public void LoadItemSizes(string[] lines)
        {
            Console.WriteLine("Reading item sizes...");
            string[] splits;
            string record;
            foreach (string line in lines)
            {
                splits = line.Split(':');
                if (splits.Length != 2) continue;
                record = splits[0].Trim();
                if (record.StartsWith("//")) continue;
                if (!int.TryParse(splits[1], out int size)) continue;
                _itemSizes[record] = size;
            }
            Console.WriteLine("- Found " + _itemSizes.Count + " records");
        }

        public void LoadItemSizes(string lines)
        {
            LoadItemSizes(lines.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries));
        }

        public int GetItemSize(string record, int def = 1)
        {
            record = Path.ChangeExtension(record, null);
            if (!_itemSizes.ContainsKey(record))
            {
                Console.WriteLine("Unknown item size record: " + record);
                return def;
            }
            return _itemSizes[record];
        }

        #endregion

    }
}
