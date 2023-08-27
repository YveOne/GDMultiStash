using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMultiStash.Forms.ContextMenues.SortStashes.Criteria
{

    internal class ClassSortCriteria : SortCriteria
    {
        public override string FormatKey(uint k) => k.ToString().PadLeft(2, '0');
        public override uint GetKey(GlobalHandlers.DatabaseHandler.ItemInfo itemInfo)
        {
            switch (itemInfo.BaseRecordInfo.Class)
            {
                case "WeaponMelee_Sword": return 1;
                case "WeaponMelee_Axe": return 2;
                case "WeaponMelee_Mace": return 3;
                case "WeaponMelee_Dagger": return 4;
                case "WeaponMelee_Scepter": return 5;
                case "WeaponHunting_Ranged1h": return 6;
                case "WeaponArmor_Shield": return 7;
                case "WeaponArmor_Offhand": return 8;
                case "WeaponMelee_Sword2h": return 9;
                case "WeaponMelee_Axe2h": return 10;
                case "WeaponMelee_Mace2h": return 11;
                case "WeaponHunting_Ranged2h": return 12;

                case "ArmorProtective_Head": return 13;
                case "ArmorProtective_Shoulders": return 14;
                case "ArmorProtective_Chest": return 15;
                case "ArmorProtective_Hands": return 16;
                case "ArmorProtective_Waist": return 17;
                case "ArmorProtective_Legs": return 18;
                case "ArmorProtective_Feet": return 19;

                case "ArmorJewelry_Amulet": return 20;
                case "ArmorJewelry_Medal": return 21;
                case "ArmorJewelry_Ring": return 22;

                // Relics
                case "ItemArtifact": return 23;

                case "ItemFactionBooster":      // 3x2
                case "ItemFactionWarrant":      // 2x3
                    return 24;

                // Useable
                case "ItemDifficultyUnlock":    // 2x2
                case "ItemAttributeReset":      // 1x2
                case "ItemDevotionReset":       // 1x2
                case "OneShot_Food":            // 1x1
                case "OneShot_PotionHealth":    // 1x1
                case "OneShot_PotionMana":      // 1x1
                case "OneShot_Sack":            // 1x1
                case "OneShot_Scroll":          // 1x1
                    return 25;

                // Blueprints
                case "ItemArtifactFormula":
                    return 26;

                // Lore objects
                case "ItemNote":
                case "QuestItem":
                    return 27;

                // Enchantments
                case "ItemEnchantment":
                    return 28;

                // Components
                case "ItemRelic":
                    return 29;

                // Transmutes
                case "ItemTransmuter":
                case "ItemTransmuterSet":
                    return 30;

                // Bonus
                case "ItemUsableSkill":
                    return 31;

                // Shattered Realms
                case "OneShot_EndlessDungeon":
                    return 32;

            }
            return 99; // Unknown
        }
        public override string GetText(uint k)
        {
            switch (k)
            {
                case 1: return Global.L.ItemClassWeaponsSwords1h();
                case 2: return Global.L.ItemClassWeaponsAxes1h();
                case 3: return Global.L.ItemClassWeaponsMaces1h();
                case 4: return Global.L.ItemClassWeaponsDaggers();
                case 5: return Global.L.ItemClassWeaponsScepters();
                case 6: return Global.L.ItemClassWeaponsRanged1h();
                case 7: return Global.L.ItemClassWeaponsShields();
                case 8: return Global.L.ItemClassWeaponsOffhands();
                case 9: return Global.L.ItemClassWeaponsSwords2h();
                case 10: return Global.L.ItemClassWeaponsAxes2h();
                case 11: return Global.L.ItemClassWeaponsMaces2h();
                case 12: return Global.L.ItemClassWeaponsRanged2h();

                case 13: return Global.L.ItemClassArmorHead();
                case 14: return Global.L.ItemClassArmorShoulders();
                case 15: return Global.L.ItemClassArmorChest();
                case 16: return Global.L.ItemClassArmorHands();
                case 17: return Global.L.ItemClassArmorWaist();
                case 18: return Global.L.ItemClassArmorLegs();
                case 19: return Global.L.ItemClassArmorFeet();

                case 20: return Global.L.ItemClassJewelryAmulets();
                case 21: return Global.L.ItemClassJewelryMedals();
                case 22: return Global.L.ItemClassJewelryRings();

                case 23: return Global.L.ItemClassOtherRelics();
                case 24: return Global.L.ItemClassOtherFactions();
                case 25: return Global.L.ItemClassOtherConsumable();
                case 26: return Global.L.ItemClassOtherBlueprints();
                case 27: return Global.L.ItemClassOtherLore();
                case 28: return Global.L.ItemClassOtherEnchantments();
                case 29: return Global.L.ItemClassOtherComponents();
                case 30: return Global.L.ItemClassOtherTransmutes();
                case 31: return Global.L.ItemClassOtherBonus();
                case 32: return Global.L.ItemClassOtherEndless();
            }
            return Global.L.ItemClassUnknown();
        }
    }
}
