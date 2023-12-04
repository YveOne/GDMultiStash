using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Drawing;

namespace GDMultiStash.Global
{
    using Database;
    namespace Database
    {

        public class ItemRecordInfo
        {
            public uint Width;
            public uint Height;
            public uint RequiredLevel;
            public string Class;
            public string Quality;
            public string Texture;
            public string ItemSetRecord;
            public uint Size => Width * Height;
        }

        public class AffixRecordInfo
        {
            public uint RequiredLevel;
            public string Quality;
        }

        public class ItemSetRecordInfo
        {
            public string ItemSetNameKey;
            public string[] ItemSetItemRecords;
        }

        public class ItemInfo
        {
            public ItemRecordInfo BaseRecordInfo;
            public AffixRecordInfo EnchantmentRecordinfo;
            public AffixRecordInfo MateriaRecordInfo;
            public AffixRecordInfo ModifierRecordInfo;
            public AffixRecordInfo PrefixRecordInfo;
            public AffixRecordInfo RelicCompletionBonusRecordInfo;
            public AffixRecordInfo SuffixRecordInfo;
            public AffixRecordInfo TransmuteRecordInfo;
            public string ShownQuality;
            public uint ShownLevelRequirement;
        }

    }

    internal partial class DatabaseManager : Base.Manager
    {

        private readonly Dictionary<string, ItemRecordInfo> _itemRecordInfos = new Dictionary<string, ItemRecordInfo>();
        private readonly Dictionary<string, AffixRecordInfo> _affixRecordInfos = new Dictionary<string, AffixRecordInfo>();
        private readonly Dictionary<string, ItemSetRecordInfo> _itemSetRecordInfos = new Dictionary<string, ItemSetRecordInfo>();
        
        private MemoryStream _texturesMemoryStream = null;
        private ZipArchive _texturesZipArchive = null;
        private readonly Dictionary<string, ZipArchiveEntry> _itemTextureEntries = new Dictionary<string, ZipArchiveEntry>();
        
        public uint GetItemSize(string record, uint def = 1)
        {
            if (GetItemRecordInfo(record, out ItemRecordInfo recordInfo))
                return recordInfo.Width * recordInfo.Height;
            return def;
        }

        public bool GetItemRecordInfo(string record, out ItemRecordInfo recordInfo)
        {
            if (_itemRecordInfos.TryGetValue(record, out recordInfo))
                return true;
            if (record != "") Console.WriteWarning($"Unknown item record: {record}");
            return false;
        }

        public bool GetAffixRecordInfo(string record, out AffixRecordInfo affixRecordInfo)
        {
            if (_affixRecordInfos.TryGetValue(record, out affixRecordInfo))
                return true;
            if (_itemRecordInfos.TryGetValue(record, out ItemRecordInfo itemRecordInfo))
            {
                affixRecordInfo = new AffixRecordInfo()
                {
                    Quality = itemRecordInfo.Quality,
                    RequiredLevel = itemRecordInfo.RequiredLevel,
                };
                return true;
            }
            if (record != "") Console.WriteWarning($"Unknown affix record: {record}");
            return false;
        }

        public bool GetItemSetRecordInfo(string record, out ItemSetRecordInfo recordInfo)
        {
            if (_itemSetRecordInfos.TryGetValue(record, out recordInfo))
                return true;
            if (record != "") Console.WriteWarning($"Unknown itemset record: {record}");
            return false;
        }

        public bool GetItemTexture(string filename, out Image itemTexture)
        {
            itemTexture = null;
            if (!_itemTextureEntries.TryGetValue(filename, out ZipArchiveEntry entry)) return false;
            using (Stream stream = entry.Open())
            {
                itemTexture = Image.FromStream(stream);
            }
            return true;
        }

        public bool GetItemInfo(GDIALib.Parser.Stash.Item item, out ItemInfo itemInfo)
        {
            itemInfo = null;
            if (!GetItemRecordInfo(item.BaseRecord, out ItemRecordInfo itemRecordInfo))
                return false;
            itemInfo = new ItemInfo() { BaseRecordInfo = itemRecordInfo };
            GetAffixRecordInfo(item.EnchantmentRecord, out itemInfo.EnchantmentRecordinfo);
            GetAffixRecordInfo(item.MateriaRecord, out itemInfo.MateriaRecordInfo);
            GetAffixRecordInfo(item.ModifierRecord, out itemInfo.ModifierRecordInfo);
            GetAffixRecordInfo(item.PrefixRecord, out itemInfo.PrefixRecordInfo);
            GetAffixRecordInfo(item.RelicCompletionBonusRecord, out itemInfo.RelicCompletionBonusRecordInfo);
            GetAffixRecordInfo(item.SuffixRecord, out itemInfo.SuffixRecordInfo);
            GetAffixRecordInfo(item.TransmuteRecord, out itemInfo.TransmuteRecordInfo);
            uint shownQualityNumber = quality2number[itemInfo.BaseRecordInfo.Quality];
            uint shownLevelRequirement = itemInfo.BaseRecordInfo.RequiredLevel;
            if (itemInfo.EnchantmentRecordinfo != null)
            {
                shownQualityNumber = Math.Max(shownQualityNumber, quality2number[itemInfo.EnchantmentRecordinfo.Quality]);
                shownLevelRequirement = Math.Max(shownLevelRequirement, itemInfo.EnchantmentRecordinfo.RequiredLevel);
            }
            if (itemInfo.MateriaRecordInfo != null)
            {
                shownQualityNumber = Math.Max(shownQualityNumber, quality2number[itemInfo.MateriaRecordInfo.Quality]);
                shownLevelRequirement = Math.Max(shownLevelRequirement, itemInfo.MateriaRecordInfo.RequiredLevel);
            }
            if (itemInfo.ModifierRecordInfo != null)
            {
                shownQualityNumber = Math.Max(shownQualityNumber, quality2number[itemInfo.ModifierRecordInfo.Quality]);
                shownLevelRequirement = Math.Max(shownLevelRequirement, itemInfo.ModifierRecordInfo.RequiredLevel);
            }
            if (itemInfo.PrefixRecordInfo != null)
            {
                shownQualityNumber = Math.Max(shownQualityNumber, quality2number[itemInfo.PrefixRecordInfo.Quality]);
                shownLevelRequirement = Math.Max(shownLevelRequirement, itemInfo.PrefixRecordInfo.RequiredLevel);
            }
            if (itemInfo.RelicCompletionBonusRecordInfo != null)
            {
                shownQualityNumber = Math.Max(shownQualityNumber, quality2number[itemInfo.RelicCompletionBonusRecordInfo.Quality]);
                shownLevelRequirement = Math.Max(shownLevelRequirement, itemInfo.RelicCompletionBonusRecordInfo.RequiredLevel);
            }
            if (itemInfo.SuffixRecordInfo != null)
            {
                shownQualityNumber = Math.Max(shownQualityNumber, quality2number[itemInfo.SuffixRecordInfo.Quality]);
                shownLevelRequirement = Math.Max(shownLevelRequirement, itemInfo.SuffixRecordInfo.RequiredLevel);
            }

            itemInfo.ShownQuality = number2quality[shownQualityNumber];
            itemInfo.ShownLevelRequirement = shownLevelRequirement;
            return true;
        }

        public void LoadItemInfos(string text)
        {
            Console.WriteLine("Reading item infos ...");
            foreach (var line in Utils.Funcs.ReadTextLinesIter(text))
            {
                var splits = line.Split('|');
                if (splits.Length < 8) continue;
                var record = splits[0].Trim();
                if (!uint.TryParse(splits[1], out uint width)) continue;
                if (!uint.TryParse(splits[2], out uint height)) continue;
                if (!uint.TryParse(splits[3], out uint level)) continue;
                _itemRecordInfos[record] = new ItemRecordInfo()
                {
                    Width = width,
                    Height = height,
                    RequiredLevel = level,
                    Class = splits[4],
                    Quality = splits[5],
                    Texture = splits[6],
                    ItemSetRecord = splits[7],
                };
            }
            Console.WriteLine("- Found " + _itemRecordInfos.Count + " records");
        }

        public void LoadItemAffixInfos(string text)
        {
            Console.WriteLine("Reading affix infos ...");
            foreach (var line in Utils.Funcs.ReadTextLinesIter(text))
            {
                var splits = line.Split('|');
                if (splits.Length < 2) continue;
                var record = splits[0].Trim();
                if (!uint.TryParse(splits[1], out uint level)) continue;
                _affixRecordInfos[record] = new AffixRecordInfo()
                {
                    RequiredLevel = level,
                    Quality = splits[2],
                };
            }
            Console.WriteLine("- Found " + _affixRecordInfos.Count + " records");
        }

        public void LoadItemSets(string text)
        {
            Console.WriteLine("Reading itemsets infos ...");
            foreach (var line in Utils.Funcs.ReadTextLinesIter(text))
            {
                var splits = line.Split('|');
                if (splits.Length < 3) continue;
                var record = splits[0].Trim();
                _itemSetRecordInfos[record] = new ItemSetRecordInfo()
                {
                    ItemSetNameKey = splits[1],
                    ItemSetItemRecords = splits[2].Split(';'),
                };
            }
            Console.WriteLine("- Found " + _itemSetRecordInfos.Count + " records");
        }

        public void LoadItemTextures(byte[] data)
        {
            Console.WriteLine("Loading item textures ...");
            _texturesMemoryStream = new MemoryStream(data);
            _texturesZipArchive = new ZipArchive(_texturesMemoryStream, ZipArchiveMode.Read);
            foreach (ZipArchiveEntry entry in _texturesZipArchive.Entries)
            {
                _itemTextureEntries.Add(entry.Name, entry);
            }
            Console.WriteLine("- Found " + _itemTextureEntries.Count + " textures");
        }

        public void Destroy()
        {
            if (_texturesZipArchive != null)
                _texturesZipArchive.Dispose();
            if (_texturesMemoryStream != null)
                _texturesMemoryStream.Close();
            if (_texturesMemoryStream != null)
                _texturesMemoryStream.Dispose();
        }

        private Dictionary<string, uint> quality2number = new Dictionary<string, uint>() {
            { "None", 0 },
            { "Broken", 1 },
            { "Common", 2 },
            { "Magical", 3 },
            { "Rare", 4 },
            { "Epic", 5 },
            { "Legendary", 6 },
            { "Quest", 7 },
        };

        private Dictionary<uint, string> number2quality = new Dictionary<uint, string>() {
            { 0, "None"},
            { 1, "Broken" },
            { 2, "Common" },
            { 3, "Magical" },
            { 4, "Rare" },
            { 5, "Epic" },
            { 6, "Legendary" },
            { 7, "Quest" },
        };

    }
}
