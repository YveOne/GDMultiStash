using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

using GDMultiStash.Common;
using GDMultiStash.Common.Objects;

using BrightIdeasSoftware;

namespace GDMultiStash.Forms.Main
{

    internal class StashesPageContextMenu : ContextMenuStrip
    {
        private readonly StashesPage page;
        private readonly int rowIndex;
        private readonly StashObject clickedStash;
        private readonly IEnumerable<StashObject> selectedStashes;
        private readonly int selectedStashesCount;

        private bool LastWasSeparator => Items.Count == 0 ? false : Items[Items.Count-1].GetType() == typeof(ToolStripSeparator);

        public StashesPageContextMenu(StashesPage page, CellRightClickEventArgs args)
        {
            this.page = page;
            this.rowIndex = args.RowIndex;
            this.clickedStash = (StashObject)args.Model;
            this.selectedStashes = page.GetSelectedObjects();
            this.selectedStashesCount = selectedStashes.Count();

            AddComment(selectedStashesCount == 1
                ? $"#{clickedStash.ID} {clickedStash.Name}"
                : $"({selectedStashesCount})");
            AddSeparator();
        }

        private string T(string s)
        {
            // used to escape & sign for toolstrip item text
            return s.Replace("&", "&&");
        }

        public void AddComment(string text)
        {
            Items.Add(new ToolStripLabel(T(text))
            { ForeColor = Color.Gray });
        }

        public void AddSeparator()
        {
            if (LastWasSeparator) return;
            Items.Add(new ToolStripSeparator());
        }

        public void AddColorOption()
        {
            string colorButtonText = T(Global.L.ColorButton());
            ToolStripMenuItem colorButton = (ToolStripMenuItem)Items.Add(colorButtonText);
            foreach (Common.Config.ConfigColor col in Global.Configuration.Colors)
            {
                string optionText = T(col.Name != "" ? col.Name : col.Value);
                ToolStripMenuItem option = new ToolStripMenuItem(optionText, null, delegate
                {
                    foreach (StashObject st in selectedStashes)
                        st.TextColor = col.Value;
                    Global.Configuration.Save();
                    Global.Runtime.NotifyStashesInfoChanged(selectedStashes);
                })
                {
                    BackColor = Color.FromArgb(0, 0, 0)
                };
                try
                {
                    Color cFore = ColorTranslator.FromHtml(col.Value);
                    option.ForeColor = cFore;
                    option.MouseEnter += delegate { option.ForeColor = Color.Black; };
                    option.MouseLeave += delegate { option.ForeColor = cFore; };
                    colorButton.DropDownItems.Add(option);
                }
                catch (Exception)
                {
                }
            }
            if (colorButton.DropDownItems.Count == 0)
            {
                colorButton.DropDownItems.Insert(0, new ToolStripMenuItem(T(Global.L.EmptyButton()))
                { ForeColor = Color.Gray });
            }
        }

        public void AddLockOption()
        {
            if (selectedStashesCount == 1)
            {
                Items.Add(T(clickedStash.Locked
                    ? Global.L.UnlockButton()
                    : Global.L.LockButton()
                ), Properties.Resources.LockBlackIcon, delegate {
                    clickedStash.Locked = !clickedStash.Locked;
                    Global.Runtime.NotifyStashesInfoChanged(clickedStash);
                });
            }
            else
            {
                Items.Add(T(Global.L.LockButton()), Properties.Resources.LockBlackIcon, delegate {
                    foreach (StashObject selStash in selectedStashes) selStash.Locked = true;
                    Global.Runtime.NotifyStashesInfoChanged(selectedStashes);
                });
                Items.Add(T(Global.L.UnlockButton()), Properties.Resources.LockBlackIcon, delegate {
                    foreach (StashObject selStash in selectedStashes) selStash.Locked = false;
                    Global.Runtime.NotifyStashesInfoChanged(selectedStashes);
                });
            }
        }

        public void AddEditNameOption()
        {
            if (selectedStashesCount > 1) return;

            Items.Add(T(Global.L.RenameButton()), null, delegate (object s, EventArgs e) {
                page.ActivateNameEditing(rowIndex);
            });
        }

        public void AddRestoreBackupOption()
        {
            if (selectedStashesCount > 1) return;

            var restoreButtonText = T(Global.L.RestoreBackupButton());
            var restoreButton = (ToolStripMenuItem)Items.Add(restoreButtonText);
            Global.Stashes.GetBackupFiles(clickedStash.ID)
                .ToList().ForEach(file => {
                    string fileName = System.IO.Path.GetFileName(file);
                    string fileDate = System.IO.File.GetLastWriteTime(file).ToString();
                    if (TransferFile.FromFile(file, out TransferFile transferFile))
                    {
                        string itemText = $"{fileName} - {fileDate} - {transferFile.TotalUsageText}";

                        restoreButton.DropDownItems.Add(T(itemText), null, delegate (object s, EventArgs e) {
                            Global.Stashes.RestoreTransferFile(clickedStash.ID, file);
                            clickedStash.LoadTransferFile();
                            Global.Runtime.ReloadOpenedStash(clickedStash.ID);
                            Global.Runtime.NotifyStashesContentChanged(clickedStash);
                        });
                    }
                    else
                    {
                        restoreButton.DropDownItems.Add(T($"{fileName} - UNABLE TO LOAD"));
                    }
                }
            );
            if (restoreButton.DropDownItems.Count == 0)
            {
                restoreButton.DropDownItems.Insert(0, new ToolStripMenuItem(T(Global.L.EmptyButton()))
                { ForeColor = Color.Gray });
            }
        }

        public void AddOverwriteOption()
        {
            if (selectedStashesCount > 1) return;

            Items.Add(T(Global.L.OverwriteButton()), null, delegate {
                DialogResult result = GrimDawnLib.GrimDawn.ShowSelectTransferFilesDialog(out string[] files, false, true);
                if (result == DialogResult.OK)
                {
                    if (Global.Stashes.ImportOverwriteStash(files[0], clickedStash))
                    {
                        Global.Runtime.NotifyStashesContentChanged(clickedStash);
                    }
                }
            });
        }

        public void AddExportButton()
        {
            Items.Add(T(Global.L.ExportButton()), null, delegate {

                StashesZipFile zipFile = new StashesZipFile();
                foreach (StashObject selStash in selectedStashes) zipFile.AddStash(selStash);
                using (var dialog = new SaveFileDialog()
                {
                    Filter = $"{Global.L.ZipArchive()}|*.zip",
                    FileName = "TransferFiles.zip",
                })
                {
                    DialogResult result = dialog.ShowDialog();
                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.FileName))
                    {
                        zipFile.SaveTo(dialog.FileName);
                    }
                }
            });
        }

        public void AddEditTabsButton()
        {
            Items.Add(T(Global.L.EditTabsButton()), null, delegate {
                foreach (StashObject s in selectedStashes)
                    Global.Windows.ShowStashTabsEditorWindow(s);
            });
        }

        public void AddChangeExpansionOption()
        {
            if (page.ShownExpansion == GrimDawnLib.GrimDawn.LatestExpansion) return;

            ToolStripMenuItem copyToExpButton = (ToolStripMenuItem)Items.Add(T(Global.L.CopyToExpansionButton()));
            for (int i = (int)page.ShownExpansion + 1; i <= (int)GrimDawnLib.GrimDawn.LatestExpansion; i += 1)
            {
                GrimDawnLib.GrimDawnGameExpansion exp = (GrimDawnLib.GrimDawnGameExpansion)i;
                copyToExpButton.DropDownItems.Add(T(GrimDawnLib.GrimDawn.GetExpansionName(exp)), null, delegate {

                    var addedStashes = new List<StashObject>();
                    var removedStashes = new List<StashObject>();
                    foreach (var st in selectedStashes)
                    {
                        StashObject copied = Global.Stashes.CreateStashCopy(st);
                        copied.Expansion = exp;
                        addedStashes.Add(copied);
                    }
                    if (Console.Confirm(Global.L.ConfirmDeleteOldStashesMessage()))
                    {
                        removedStashes = Global.Stashes.DeleteStashes(selectedStashes);
                    }
                    Global.Configuration.Save();
                    Global.Runtime.NotifyStashesRebuild();
                    Global.Runtime.NotifyStashesRemoved(removedStashes);
                    Global.Runtime.NotifyStashesAdded(addedStashes);
                });
            }
        }

        public void AddAutoFillOption()
        {
            var fillButtonText = T(Global.L.AutoFillButton());
            var fillButton = (ToolStripMenuItem)Items.Add(fillButtonText);
            fillButton.DropDownItems.Add(T(Global.L.AutoFillRandomSeedsButton()), null, delegate {
                foreach (var s in selectedStashes)
                {
                    Global.FileSystem.BackupStashTransferFile(s.ID);
                    s.AutoFill();
                    s.SaveTransferFile();
                    s.LoadTransferFile();
                    Global.Runtime.NotifyStashesContentChanged(s);
                }
            });
        }

        #region sort by

        private abstract class ItemSortHandler
        {
            public abstract string GroupText { get; }
            public abstract uint GetSortKey(GlobalHandlers.DatabaseHandler.ItemInfo itemInfo);
            public abstract string GetText(uint key);
        }

        private class ItemLevelSortHandler : ItemSortHandler
        {
            public override string GroupText => Global.L.GroupSortByLevelName();
            public override uint GetSortKey(GlobalHandlers.DatabaseHandler.ItemInfo itemInfo)
            {
                return itemInfo.BaseRecordInfo.RequiredLevel;
            }
            public override string GetText(uint key)
            {
                return $"{key}";
            }
        }

        private class ItemNoneSortHandler : ItemSortHandler
        {
            public override string GroupText => Global.L.GroupSortByNoneName();
            public override uint GetSortKey(GlobalHandlers.DatabaseHandler.ItemInfo itemInfo)
            {
                return 0;
            }
            public override string GetText(uint key)
            {
                return $"Unsorted";
            }
        }

        private class ItemTypeSortHandler : ItemSortHandler
        {
            public override string GroupText => Global.L.GroupSortByTypeName();
            public override uint GetSortKey(GlobalHandlers.DatabaseHandler.ItemInfo itemInfo)
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

            public override string GetText(uint key)
            {
                switch (key)
                {
                    case 1: return Global.L.StashSortByTypeName(Global.L.ItemClassWeapons());
                    case 2: return Global.L.StashSortByTypeName(Global.L.ItemClassArmor());
                    case 3: return Global.L.StashSortByTypeName(Global.L.ItemClassJewelry());
                    case 4: return Global.L.StashSortByTypeName(Global.L.ItemClassOther());
                }
                return Global.L.StashSortByTypeName(Global.L.ItemClassUnknown());
            }
        }

        private class ItemClassSortHandler : ItemSortHandler
        {
            public override string GroupText => Global.L.GroupSortByClassName();
            public override uint GetSortKey(GlobalHandlers.DatabaseHandler.ItemInfo itemInfo)
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
                return 9999; // Unknown
            }

            public override string GetText(uint key)
            {
                switch (key)
                {
                    case 1: return Global.L.StashSortByTypeName(Global.L.ItemClassWeaponsSwords1h());
                    case 2: return Global.L.StashSortByTypeName(Global.L.ItemClassWeaponsAxes1h());
                    case 3: return Global.L.StashSortByTypeName(Global.L.ItemClassWeaponsMaces1h());
                    case 4: return Global.L.StashSortByTypeName(Global.L.ItemClassWeaponsDaggers());
                    case 5: return Global.L.StashSortByTypeName(Global.L.ItemClassWeaponsScepters());
                    case 6: return Global.L.StashSortByTypeName(Global.L.ItemClassWeaponsRanged1h());
                    case 7: return Global.L.StashSortByTypeName(Global.L.ItemClassWeaponsShields());
                    case 8: return Global.L.StashSortByTypeName(Global.L.ItemClassWeaponsOffhands());
                    case 9: return Global.L.StashSortByTypeName(Global.L.ItemClassWeaponsSwords2h());
                    case 10: return Global.L.StashSortByTypeName(Global.L.ItemClassWeaponsAxes2h());
                    case 11: return Global.L.StashSortByTypeName(Global.L.ItemClassWeaponsMaces2h());
                    case 12: return Global.L.StashSortByTypeName(Global.L.ItemClassWeaponsRanged2h());

                    case 13: return Global.L.StashSortByTypeName(Global.L.ItemClassArmorHead());
                    case 14: return Global.L.StashSortByTypeName(Global.L.ItemClassArmorShoulders());
                    case 15: return Global.L.StashSortByTypeName(Global.L.ItemClassArmorChest());
                    case 16: return Global.L.StashSortByTypeName(Global.L.ItemClassArmorHands());
                    case 17: return Global.L.StashSortByTypeName(Global.L.ItemClassArmorWaist());
                    case 18: return Global.L.StashSortByTypeName(Global.L.ItemClassArmorLegs());
                    case 19: return Global.L.StashSortByTypeName(Global.L.ItemClassArmorFeet());

                    case 20: return Global.L.StashSortByTypeName(Global.L.ItemClassJewelryAmulets());
                    case 21: return Global.L.StashSortByTypeName(Global.L.ItemClassJewelryMedals());
                    case 22: return Global.L.StashSortByTypeName(Global.L.ItemClassJewelryRings());

                    case 23: return Global.L.StashSortByTypeName(Global.L.ItemClassOtherRelics());
                    case 24: return Global.L.StashSortByTypeName(Global.L.ItemClassOtherFactions());
                    case 25: return Global.L.StashSortByTypeName(Global.L.ItemClassOtherConsumable());
                    case 26: return Global.L.StashSortByTypeName(Global.L.ItemClassOtherBlueprints());
                    case 27: return Global.L.StashSortByTypeName(Global.L.ItemClassOtherLore());
                    case 28: return Global.L.StashSortByTypeName(Global.L.ItemClassOtherEnchantments());
                    case 29: return Global.L.StashSortByTypeName(Global.L.ItemClassOtherComponents());
                    case 30: return Global.L.StashSortByTypeName(Global.L.ItemClassOtherTransmutes());
                    case 31: return Global.L.StashSortByTypeName(Global.L.ItemClassOtherBonus());
                    case 32: return Global.L.StashSortByTypeName(Global.L.ItemClassOtherEndless());
                }
                return Global.L.StashSortByTypeName(Global.L.ItemClassUnknown());
            }
        }

        private class ItemQualitySortHandler : ItemSortHandler
        {
            public override string GroupText => Global.L.GroupSortByQualityName();
            public override uint GetSortKey(GlobalHandlers.DatabaseHandler.ItemInfo itemInfo)
            {
                switch (itemInfo.ShownQuality)
                {
                    case "None": return 0; // ?
                    case "Common": return 1; // white
                    case "Magical": return 2; // yellow
                    case "Rare": return 3; // green
                    case "Epic": return 4; // blue
                    case "Legendary": return 5; // purple
                    case "Quest": return 6;
                    case "Broken": return 999;
                }
                return 9999; // Unknown
            }
            public override string GetText(uint key)
            {
                switch (key)
                {
                    case 0: return "None";
                    case 1: return "Common (White)";
                    case 2: return "Magical (Yellow)";
                    case 3: return "Rare (Green)";
                    case 4: return "Epic (Blue)";
                    case 5: return "Legendary (Purple)";
                    case 6: return "Quest";
                    case 99: return "Broken";
                }
                return "Unknown";
            }
        }

        private void SortItemsBy(ItemSortHandler handler)
        {
            // this is used to delete empty groups afterwards
            var parentGroups = new Dictionary<int, bool>();

            // first we need to fill all items into sorted multiarray
            // sortedList[sortKey][width][height][record][]
            var sortedList = new SortedDictionary<uint, SortedDictionary<uint, SortedDictionary<uint, SortedDictionary<string, List<GDIALib.Parser.Stash.Item>>>>>();
            foreach (var stash in selectedStashes)
            {
                Global.FileSystem.BackupStashTransferFile(stash.ID);
                foreach (var tab in stash.Tabs)
                {
                    var removeItems = new List<GDIALib.Parser.Stash.Item>();
                    foreach (var item in tab.Items)
                    {
                        if (!Global.Database.GetItemInfo(item, out GlobalHandlers.DatabaseHandler.ItemInfo itemInfo))
                            continue;
                        var itemBaseRecord = item.BaseRecord;
                        var itemWidth = itemInfo.BaseRecordInfo.Width;
                        var itemHeight = itemInfo.BaseRecordInfo.Height;
                        var sortKey = handler.GetSortKey(itemInfo);
                        if (!sortedList.ContainsKey(sortKey))
                            sortedList.Add(sortKey, new SortedDictionary<uint, SortedDictionary<uint, SortedDictionary<string, List<GDIALib.Parser.Stash.Item>>>>());
                        if (!sortedList[sortKey].ContainsKey(itemWidth))
                            sortedList[sortKey].Add(itemWidth, new SortedDictionary<uint, SortedDictionary<string, List<GDIALib.Parser.Stash.Item>>>());
                        if (!sortedList[sortKey][itemWidth].ContainsKey(itemHeight))
                            sortedList[sortKey][itemWidth].Add(itemHeight, new SortedDictionary<string, List<GDIALib.Parser.Stash.Item>>());
                        if (!sortedList[sortKey][itemWidth][itemHeight].ContainsKey(item.BaseRecord))
                            sortedList[sortKey][itemWidth][itemHeight].Add(item.BaseRecord, new List<GDIALib.Parser.Stash.Item>());
                        sortedList[sortKey][itemWidth][itemHeight][itemBaseRecord].Add(item);
                        removeItems.Add(item);
                    }
                    foreach (var item in removeItems)
                        tab.Items.Remove(item);
                    // dont clear whole list
                    // maybe there has been an unknown record
                    //tab.Items.Clear();
                }
                parentGroups[stash.GroupID] = true;
                stash.SaveTransferFile();
                stash.LoadTransferFile();
                Global.Runtime.NotifyStashesContentChanged(stash);
            }

            // create new group for new stashes
            var group = Global.Stashes.CreateStashGroup(handler.GroupText, true);
            var stashWidth = clickedStash.Width; // all selected stashes should have same width and hight
            var stashHeight = clickedStash.Height;
            var addedStashes = new List<StashObject>();

            // the tabs are already multi-sorted
            // now just loop recursive
            foreach (var sortKeys in sortedList)
            {
                // first we need to create a list of tabs
                var tabsQueue = new List<GDIALib.Parser.Stash.StashTab>();

                // append all items into new tabs
                foreach (var widths in sortKeys.Value.Reverse()) // from large to small width
                    foreach (var heights in widths.Value.Reverse()) // from large to small height
                    {
                        var itemWidth = widths.Key;
                        var itemHeight = heights.Key;

                        uint posX = 0;
                        uint posY = 0;
                        GDIALib.Parser.Stash.StashTab currentTab = null;

                        foreach (var records in heights.Value)
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

                // now we got all items inside the tabs queue
                while (tabsQueue.Count != 0)
                {
                    var stash = Global.Stashes.CreateStash(handler.GetText(sortKeys.Key), page.ShownExpansion, GrimDawnLib.GrimDawnGameMode.Both, 0);
                    stash.GroupID = group.ID;
                    int maxTabs = (int)GrimDawnLib.GrimDawn.Stashes.GetStashInfoForExpansion(page.ShownExpansion).MaxTabs;
                    for (int tabI = 0; tabI < maxTabs; tabI += 1)
                    {
                        if (tabsQueue.Count == 0) continue; // just skip...
                        stash.AddTab(tabsQueue[0]);
                        tabsQueue.RemoveAt(0);
                    }
                    stash.SaveTransferFile();
                    stash.LoadTransferFile();
                    addedStashes.Add(stash);
                }
            }

            if (Console.Question(Global.L.ConfirmDeleteEmptyStashesMessage()))
            {
                var deletedStashes = Global.Stashes.DeleteStashes(selectedStashes, true);
                Global.Runtime.NotifyStashesRemoved(deletedStashes);
                var emptyGroups = parentGroups.Keys
                    .Where(grpId => Global.Stashes.GetStashesForGroup(grpId).Length == 0)
                    .Select(grpId => Global.Stashes.GetStashGroup(grpId))
                    .ToArray();
                if (emptyGroups.Length != 0)
                {
                    if (Console.Question(Global.L.ConfirmDeleteEmptyGroupsMessage()))
                    {
                        var deletedGroups = Global.Stashes.DeleteStashGroups(emptyGroups);
                        Global.Runtime.NotifyStashGroupsRemoved(deletedGroups);
                        Global.Runtime.NotifyStashesRebuild();
                    }
                }
                // maybe there was a stash that could not be deleted (maybe its active)
                foreach (var notDeletedStash in selectedStashes.Where(s => !deletedStashes.Contains(s)))
                {
                    Global.Runtime.ReloadOpenedStash(notDeletedStash.ID);
                }
            }


            // done
            Global.Configuration.Save();
            Global.Runtime.NotifyStashGroupsAdded(group);
            Global.Runtime.NotifyStashesAdded(addedStashes);
        }

        #endregion

        public void AddAutoSortOption()
        {
            var sortButtonText = T(Global.L.SortItemsButton());
            var sortButton = (ToolStripMenuItem)Items.Add(sortButtonText);
            sortButton.DropDownItems.Add(T(Global.L.SortByLevelButton()), null, delegate {
                SortItemsBy(new ItemLevelSortHandler());
            });
            sortButton.DropDownItems.Add(T(Global.L.SortByTypeButton()), null, delegate {
                SortItemsBy(new ItemTypeSortHandler());
            });
            sortButton.DropDownItems.Add(T(Global.L.SortByClassButton()), null, delegate {
                SortItemsBy(new ItemClassSortHandler());
            });
            sortButton.DropDownItems.Add(T(Global.L.SortByQualityButton()), null, delegate {
                SortItemsBy(new ItemQualitySortHandler());
            });
            sortButton.DropDownItems.Add(T(Global.L.SortByNoneButton()), null, delegate {
                SortItemsBy(new ItemNoneSortHandler());
            });
        }

        public void AddDeleteOption()
        {
            var deleteButtonText = T(Global.L.DeleteButton());
            var deleteButton = (ToolStripMenuItem)Items.Add(deleteButtonText);
            deleteButton.DropDownItems.Add(T(Global.L.DeleteSelectedStashesButton()), null, delegate {
                if (Global.Configuration.Settings.ConfirmStashDelete
                && !Console.Confirm(Global.L.ConfirmDeleteStashesMessage()))
                    return;
                List<StashObject> deletedItems = Global.Stashes.DeleteStashes(selectedStashes);
                Global.Configuration.Save();
                Global.Runtime.NotifyStashesRemoved(deletedItems);
            });
            deleteButton.DropDownItems.Add(T(Global.L.DeleteEmptyStashesButton()), null, delegate {
                List<StashObject> deletedItems = Global.Stashes.DeleteStashes(selectedStashes, true);
                Global.Configuration.Save();
                Global.Runtime.NotifyStashesRemoved(deletedItems);
            });
        }

    }
}
