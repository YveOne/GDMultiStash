using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

using GDMultiStash.Common;
using GDMultiStash.Common.Objects;
using GrimDawnLib;
using static GDMultiStash.Global.DatabaseManager;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace GDMultiStash.Forms.StashTabsEditor
{
    [DesignerCategory("code")]
    internal class StashTabPanel : StashTabBasePanel
    {
        public GDIALib.Parser.Stash.StashTab StashTab { get; private set; }
        public StashObject StashObject { get; private set; }
        public int Index { get; private set; }

        public StashTabPanel(StashObject stashObject, GDIALib.Parser.Stash.StashTab tab) : base(stashObject)
        {
            this.StashTab = tab;
            this.StashObject = stashObject;
            this.BackgroundImage = CreateTabImage(tab, stashObject.Expansion);
        }

        public void AppendTo(Control parent, int index, bool ignoreIndex = false)
        {
            if (parent == null) return;
            if (!parent.Controls.Contains(this))
            {
                if (Parent == null || Parent != parent)
                {
                    if (Parent != null)
                        Parent.Controls.Remove(this);
                    parent.Controls.Add(this);
                }
            }
            index = Math.Min(index, parent.Controls.Count - 2);
            parent.Controls.SetChildIndex(this, index);
            if (!ignoreIndex) Index = index;
        }

        public void Remove()
        {
            if (Parent == null) return;
            Parent.Controls.Remove(this);
        }

        private static Image CreateTabImage(GDIALib.Parser.Stash.StashTab tab, GrimDawnGameExpansion expansion)
        {
            var stashInfo = TransferFile.GetStashInfoForExpansion(expansion);
            Image bgImage;
            switch (stashInfo.Width)
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
                    if (!G.Database.GetItemInfo(item, out Global.Database.ItemInfo iteminfo)) continue;
                    if (!G.Database.GetItemTexture(iteminfo.BaseRecordInfo.Texture, out Image itemTexture)) continue;
                    int x = (int)item.X * cellSize + xPosOffset;
                    int y = (int)item.Y * cellSize + yPosOffset;
                    int w = (int)iteminfo.BaseRecordInfo.Width * cellSize;
                    int h = (int)iteminfo.BaseRecordInfo.Height * cellSize;

                    Color itemColor = Color.FromArgb(0, 0, 0, 0);

                    switch (iteminfo.BaseRecordInfo.Class)
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
                    gr.DrawImage(itemTexture, x + 2, y + 2, w - 2, h - 2);
                    itemTexture.Dispose();
                }
            }
            return img;
        }

    }
}
