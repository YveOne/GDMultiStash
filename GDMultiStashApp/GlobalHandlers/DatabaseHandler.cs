using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Drawing;
using System.Drawing.Drawing2D;

using GDMultiStash.Common;

using GrimDawnLib;

namespace GDMultiStash.GlobalHandlers
{
    internal partial class DatabaseHandler : Base.BaseHandler
    {

        public class ItemRecordInfo
        {
            public uint Width;
            public uint Height;
            public uint RequiredLevel;
            public string Class;
            public string Quality;
            public string Texture;
            public uint Size => Width * Height;
        }

        public class AffixRecordInfo
        {
            public uint RequiredLevel;
            public string Quality;
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

        private readonly Dictionary<string, ItemRecordInfo> _itemRecordInfos = new Dictionary<string, ItemRecordInfo>();
        private readonly Dictionary<string, AffixRecordInfo> _affixRecordInfos = new Dictionary<string, AffixRecordInfo>();
        private MemoryStream _texturesMemoryStream = null;
        private ZipArchive _texturesZipArchive = null;
        private readonly Dictionary<string, ZipArchiveEntry> _itemTextureEntries = new Dictionary<string, ZipArchiveEntry>();
        
        public uint GetItemSize(string record, uint def = 1)
        {
            if (record == "") return 0;
            if (_itemRecordInfos.TryGetValue(record, out ItemRecordInfo itemInfo))
                return itemInfo.Size;
            Console.Error($"Unknown item record: {record}");
            return def;
        }

        public bool GetItemRecordInfo(string record, out ItemRecordInfo itemRecordInfo)
        {
            if (_itemRecordInfos.TryGetValue(record, out itemRecordInfo))
                return true;
            if (record != "") Console.Error($"Unknown item record: {record}");
            return false;
        }

        public bool GetAffixRecordInfo(string record, out AffixRecordInfo affixRecordInfo)
        {
            if (_affixRecordInfos.TryGetValue(record, out affixRecordInfo))
                return true;
            if (record != "") Console.Error($"Unknown affix record: {record}");
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
                shownLevelRequirement = Math.Max(shownQualityNumber, itemInfo.EnchantmentRecordinfo.RequiredLevel);
            }
            if (itemInfo.MateriaRecordInfo != null)
            {
                shownQualityNumber = Math.Max(shownQualityNumber, quality2number[itemInfo.MateriaRecordInfo.Quality]);
                shownLevelRequirement = Math.Max(shownQualityNumber, itemInfo.MateriaRecordInfo.RequiredLevel);
            }
            if (itemInfo.ModifierRecordInfo != null)
            {
                shownQualityNumber = Math.Max(shownQualityNumber, quality2number[itemInfo.ModifierRecordInfo.Quality]);
                shownLevelRequirement = Math.Max(shownQualityNumber, itemInfo.ModifierRecordInfo.RequiredLevel);
            }
            if (itemInfo.PrefixRecordInfo != null)
            {
                shownQualityNumber = Math.Max(shownQualityNumber, quality2number[itemInfo.PrefixRecordInfo.Quality]);
                shownLevelRequirement = Math.Max(shownQualityNumber, itemInfo.PrefixRecordInfo.RequiredLevel);
            }
            if (itemInfo.RelicCompletionBonusRecordInfo != null)
            {
                shownQualityNumber = Math.Max(shownQualityNumber, quality2number[itemInfo.RelicCompletionBonusRecordInfo.Quality]);
                shownLevelRequirement = Math.Max(shownQualityNumber, itemInfo.RelicCompletionBonusRecordInfo.RequiredLevel);
            }
            if (itemInfo.SuffixRecordInfo != null)
            {
                shownQualityNumber = Math.Max(shownQualityNumber, quality2number[itemInfo.SuffixRecordInfo.Quality]);
                shownLevelRequirement = Math.Max(shownQualityNumber, itemInfo.SuffixRecordInfo.RequiredLevel);
            }
            itemInfo.ShownQuality = number2quality[shownQualityNumber];
            itemInfo.ShownLevelRequirement = shownLevelRequirement;
            return true;
        }

        public void LoadItemInfos(string text)
        {
            Console.WriteLine("Reading item infos ...");
            string[] splits;
            string record;
            using (StringReader sr = new StringReader(text))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    splits = line.Split('|');
                    if (splits.Length < 4) continue;
                    record = splits[0].Trim();
                    if (record.StartsWith("//")) continue;
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
                    };
                }
            }
            Console.WriteLine("- Found " + _itemRecordInfos.Count + " records");
        }

        public void LoadItemAffixInfos(string text)
        {
            Console.WriteLine("Reading affix infos ...");
            string[] splits;
            string record;
            using (StringReader sr = new StringReader(text))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    splits = line.Split('|');
                    if (splits.Length < 2) continue;
                    record = splits[0].Trim();
                    if (record.StartsWith("//")) continue;
                    if (!uint.TryParse(splits[1], out uint level)) continue;
                    _affixRecordInfos[record] = new AffixRecordInfo()
                    {
                        RequiredLevel = level,
                        Quality = splits[2],
                    };
                }
            }
            Console.WriteLine("- Found " + _affixRecordInfos.Count + " records");
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
            _texturesZipArchive.Dispose();
            _texturesMemoryStream.Close();
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

        public Image CreateTabImage(GDIALib.Parser.Stash.StashTab tab, GrimDawnGameExpansion expansion)
        {
            var stashInfo = TransferFile.GetStashInfoForExpansion(expansion);
            Image bgImage;
            switch(stashInfo.Width)
            {
                case 8:
                    bgImage = Properties.Resources.caravanWindow8x16;
                    break;
                default:
                    bgImage = Properties.Resources.caravanWindow10x18;
                    break;
            }

            int xPosOffset = 1;
            int yPosOffset = 1;
            int cellSize = 16;

            Color ItemColorQualityBroken = Color.FromArgb(0, 0, 0, 0); // UNKNOWN
            Color ItemColorQualityNone = Color.FromArgb(0, 0, 0, 0);
            Color ItemColorQualityCommon = Color.FromArgb(255, 90, 90, 90);
            Color ItemColorQualityMagical = Color.FromArgb(255, 251, 218, 50);
            Color ItemColorQualityRare = Color.FromArgb(255, 97, 204, 0);
            Color ItemColorQualityEpic = Color.FromArgb(255, 74, 111, 209);
            Color ItemColorQualityLegendary = Color.FromArgb(255, 106, 48, 186);

            Color ItemColorClassRelic = Color.FromArgb(255, 1, 242, 242);
            Color ItemColorClassQuest = Color.FromArgb(255, 198, 50, 222);
            Color ItemColorClassEnchant = Color.FromArgb(255, 141, 197, 0);
            Color ItemColorClassNote = Color.FromArgb(255, 128, 39, 217);

            Image img = new Bitmap(bgImage.Width, bgImage.Height);
            using (Graphics gr = Graphics.FromImage(img))
            {
                gr.DrawImage(bgImage, 0, 0, bgImage.Width, bgImage.Height);
                foreach (var item in tab.Items)
                {
                    if (!GetItemInfo(item, out ItemInfo iteminfo)) continue;
                    if (!GetItemTexture(iteminfo.BaseRecordInfo.Texture, out Image itemTexture)) continue;
                    int x = (int)item.X * cellSize + xPosOffset;
                    int y = (int)item.Y * cellSize + yPosOffset;
                    int w = (int)iteminfo.BaseRecordInfo.Width * cellSize;
                    int h = (int)iteminfo.BaseRecordInfo.Height * cellSize;

                    Color itemColor = Color.FromArgb(0,0,0,0);

                    switch(iteminfo.BaseRecordInfo.Class)
                    {
                        case "ArmorJewelry_Amulet":
                        case "ArmorJewelry_Medal":
                        case "ArmorJewelry_Ring":
                        case "ArmorProtective_Chest":
                        case "ArmorProtective_Feet":
                        case "ArmorProtective_Hands":
                        case "ArmorProtective_Head":
                        case "ArmorProtective_Legs":
                        case "ArmorProtective_Shoulders":
                        case "ArmorProtective_Waist":
                        case "WeaponArmor_Offhand":
                        case "WeaponArmor_Shield":
                        case "WeaponHunting_Ranged1h":
                        case "WeaponHunting_Ranged2h":
                        case "WeaponMelee_Axe":
                        case "WeaponMelee_Axe2h":
                        case "WeaponMelee_Dagger":
                        case "WeaponMelee_Mace":
                        case "WeaponMelee_Mace2h":
                        case "WeaponMelee_Scepter":
                        case "WeaponMelee_Sword":
                        case "WeaponMelee_Sword2h":
                            switch (iteminfo.ShownQuality)
                            {
                                case "Broken": itemColor = ItemColorQualityBroken; break;
                                case "Common": itemColor = ItemColorQualityCommon; break;
                                case "Epic": itemColor = ItemColorQualityEpic; break;
                                case "Legendary": itemColor = ItemColorQualityLegendary; break;
                                case "Magical": itemColor = ItemColorQualityMagical; break;
                                case "None": itemColor = ItemColorQualityNone; break;
                                case "Quest": itemColor = ItemColorClassQuest; break;
                                case "Rare": itemColor = ItemColorQualityRare; break;
                            }
                            break;
                        case "ItemArtifact":
                            itemColor = ItemColorClassRelic;
                            break;
                        case "ItemRelic":
                            itemColor = ItemColorQualityMagical;
                            break;
                        case "QuestItem":
                            itemColor = ItemColorClassQuest;
                            break;
                        case "ItemEnchantment":
                            itemColor = ItemColorClassEnchant;
                            break;
                        case "ItemNote":
                            itemColor = ItemColorClassNote;
                            break;
                        case "OneShot_Scroll":
                        case "OneShot_PotionHealth":
                        case "OneShot_PotionMana":
                        case "ItemUsableSkill":
                        case "ItemArtifactFormula":
                        case "ItemDifficultyUnlock":
                        case "ItemAttributeReset":
                        case "ItemDevotionReset":
                        case "ItemFactionBooster":
                        case "ItemFactionWarrant":
                            // no background color
                            break;
                        case "OneShot_Sack":
                        case "OneShot_Food":
                        case "ItemTransmuter":
                        case "ItemTransmuterSet":
                        case "OneShot_EndlessDungeon":
                            // Unknown
                            break;
                        default:
                            Console.WriteLine($"{item.BaseRecord} - {iteminfo.BaseRecordInfo.Class} - {iteminfo.BaseRecordInfo.Quality}");
                            break;
                    }

                    using (Brush brush = new SolidBrush(Color.FromArgb(66, itemColor)))
                    {
                        gr.FillRectangle(brush, x + 2, y + 3, w - 4, h - 5);
                    }
                    using (Pen pen = new Pen(Color.FromArgb(140, itemColor), 1)
                    { Alignment = PenAlignment.Inset })
                    {
                        gr.DrawRectangle(pen, x + 2, y + 3, w - 4, h - 5);
                    }
                    gr.DrawImage(itemTexture, x+2, y+2, w-2, h-2);
                    itemTexture.Dispose();
                }
            }
            return img;
        }

    }
}
