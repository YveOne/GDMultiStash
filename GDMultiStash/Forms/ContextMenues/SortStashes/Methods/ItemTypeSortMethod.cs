using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMultiStash.Forms.ContextMenues.SortStashes.Methods
{
    internal class ItemTypeSortMethod : ItemSortMethod<int>
    {
        public override string GroupText => Global.L.GroupSortByTypeName();
        public override int GetSortKey(GlobalHandlers.DatabaseHandler.ItemInfo itemInfo)
        {
            switch (itemInfo.BaseRecordInfo.Class)
            {

                // Weapons
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
                    return 1;

                // Armor
                case "ArmorProtective_Chest":
                case "ArmorProtective_Feet":
                case "ArmorProtective_Hands":
                case "ArmorProtective_Head":
                case "ArmorProtective_Legs":
                case "ArmorProtective_Shoulders":
                case "ArmorProtective_Waist":
                    return 2;

                // Jewelry
                case "ArmorJewelry_Amulet":
                case "ArmorJewelry_Medal":
                case "ArmorJewelry_Ring":
                    return 3;

                // Other
                case "ItemArtifact":
                case "ItemFactionBooster":
                case "ItemFactionWarrant":
                case "ItemDifficultyUnlock":
                case "ItemAttributeReset":
                case "ItemDevotionReset":
                case "OneShot_Food":
                case "OneShot_PotionHealth":
                case "OneShot_PotionMana":
                case "OneShot_Sack":
                case "OneShot_Scroll":
                case "ItemArtifactFormula":
                case "ItemNote":
                case "QuestItem":
                case "ItemEnchantment":
                case "ItemRelic":
                case "ItemTransmuter":
                case "ItemTransmuterSet":
                case "ItemUsableSkill":
                case "OneShot_EndlessDungeon":
                    return 4;

            }
            return 9999; // Unknown
        }

        public override int[] IgnoreKeys => new int[] { };

        public override string GetText(int key)
        {
            switch (key)
            {
                case 1: return Global.L.ItemClassWeapons();
                case 2: return Global.L.ItemClassArmor();
                case 3: return Global.L.ItemClassJewelry();
                case 4: return Global.L.ItemClassOther();
            }
            return Global.L.ItemClassUnknown();
        }
    }
}
