using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMultiStash.GlobalHandlers
{
    partial class LocalizationHandler
    {
        public class StringsHolder
        {

            public _Core.StringGetter LanguageName => _.Get("language_name");

            public _Core.StringGetter SelectGameDirectoryMessage => _.Get("msg_select_game_directory");
            public _Core.StringGetter GameDirectoryNotFoundMessage => _.Get("msg_game_directory_not_found");
            public _Core.StringGetter DocumentsDirectoryNotFoundMessage => _.Get("msg_documents_directory_not_found");
            public _Core.StringGetter UpdateAvailableMessage => _.Get("msg_update_available");
            public _Core.StringGetter ConfirmClosingMessage => _.Get("msg_confirm_closing");
            public _Core.StringGetter AlreadyRunningMessage => _.Get("msg_already_running");
            public _Core.StringGetter GameAlreadyRunningMessage => _.Get("msg_game_already_running");
            public _Core.StringGetter ShortcutCreatedMessage => _.Get("msg_shortcut_created");
            public _Core.StringGetter InvalidTransferFileMessage => _.Get("msg_invalid_transfer_file");
            public _Core.StringGetter NoStashSelectedMessage => _.Get("msg_no_stash_selected");
            public _Core.StringGetter BackupsCleanedUpMessage => _.Get("msg_backups_cleanedup");
            public _Core.StringGetter ConfirmDeleteStashesMessage => _.Get("msg_confirm_delete_stashes");
            public _Core.StringGetter ConfirmDeleteStashGroupsMessage => _.Get("msg_confirm_delete_stash_groups");
            public _Core.StringGetter CannotDeleteStashMessage => _.Get("msg_cannot_delete_stash");
            public _Core.StringGetter StashIsActiveMessage => _.Get("msg_stash_is_active");
            public _Core.StringGetter StashIsMainMessage => _.Get("msg_stash_is_main");
            public _Core.StringGetter CannotDeleteStashGroupMessage => _.Get("msg_cannot_delete_stash_group");
            public _Core.StringGetter StashGroupIsMainMessage => _.Get("msg_stash_group_is_main");
            public _Core.StringGetter StashGroupIsNotEmptyMessage => _.Get("msg_stash_group_is_not_empty");
            public _Core.StringGetter DisableCloudSyncMessage => _.Get("msg_disable_cloud_sync");
            public _Core.StringGetter ConfirmDeleteOldStashesMessage => _.Get("msg_confirm_delete_old_stashes");
            public _Core.StringGetter ConfirmDeleteStashTabMessage => _.Get("msg_confirm_delete_stash_tab");
            public _Core.StringGetter ConfirmClearStashTabMessage => _.Get("msg_confirm_clear_stash_tab");
            public _Core.StringGetter OldGDVersionNotSupported => _.Get("msg_old_gd_not_supported");

            





            public _Core.StringGetter FileButton => _.Get("btn_file");
            public _Core.StringGetter HelpButton => _.Get("btn_help");
            public _Core.StringGetter ImportButton => _.Get("btn_import");
            public _Core.StringGetter ExportButton => _.Get("btn_export");
            public _Core.StringGetter TransferFilesButton => _.Get("btn_transfer_files");
            public _Core.StringGetter StartGameButton => _.Get("btn_start_game");
            public _Core.StringGetter SettingsButton => _.Get("btn_settings");
            public _Core.StringGetter ChangelogButton => _.Get("btn_changelog");
            public _Core.StringGetter AboutButton => _.Get("btn_about");
            public _Core.StringGetter CreateStashButton => _.Get("btn_create_stash");
            public _Core.StringGetter ColorButton => _.Get("btn_color");
            public _Core.StringGetter LockButton => _.Get("btn_lock");
            public _Core.StringGetter UnlockButton => _.Get("btn_unlock");
            public _Core.StringGetter RenameButton => _.Get("btn_rename");
            public _Core.StringGetter RestoreBackupButton => _.Get("btn_restore_backup");
            public _Core.StringGetter DeleteButton => _.Get("btn_delete");
            public _Core.StringGetter DeleteSelectedStashesButton => _.Get("btn_delete_selected_stashes");
            public _Core.StringGetter DeleteEmptyStashesButton => _.Get("btn_delete_empty_stashes");
            public _Core.StringGetter SaveButton => _.Get("btn_save");
            public _Core.StringGetter ApplyButton => _.Get("btn_apply");
            public _Core.StringGetter SearchButton => _.Get("btn_search");
            public _Core.StringGetter CreateShortcutButton => _.Get("btn_create_shortcut");
            public _Core.StringGetter CleanupBackupsButton => _.Get("btn_cleanup_backups");
            public _Core.StringGetter CreateButton => _.Get("btn_create");
            public _Core.StringGetter LoadButton => _.Get("btn_load");
            public _Core.StringGetter CreateGroupButton => _.Get("btn_create_group");
            public _Core.StringGetter OverwriteButton => _.Get("btn_overwrite");
            public _Core.StringGetter StashesButton => _.Get("btn_stashes");
            public _Core.StringGetter GroupsButton => _.Get("btn_groups");
            public _Core.StringGetter CopyToExpansionButton => _.Get("btn_copy_to_expansion");
            public _Core.StringGetter EditTabsButton => _.Get("btn_edit_tabs");
            public _Core.StringGetter EmptyButton => _.Get("btn_empty");
            public _Core.StringGetter SortItemsButton => _.Get("btn_sort_items");
            public _Core.StringGetter AutoFillButton => _.Get("btn_auto_fill");
            public _Core.StringGetter AutoFillRandomSeedsButton => _.Get("btn_auto_fill_random_seeds");
            public _Core.StringGetter ExitButton => _.Get("btn_exit");
            public _Core.StringGetter ClearButton => _.Get("btn_clear");
            public _Core.StringGetter DuplicateButton => _.Get("btn_duplicate");
            public _Core.StringGetter CraftingModeButton => _.Get("btn_crafting_mode");
            public _Core.StringGetter StartButton => _.Get("btn_start");
            public _Core.StringGetter FinishButton => _.Get("btn_finish");







            public _Core.StringGetter ShownStashesLabel => _.Get("lbl_shown_stashes");
            public _Core.StringGetter ShownSelectedStashesLabel => _.Get("lbl_shown_stashes_selected");
            public _Core.StringGetter LanguageLabel => _.Get("lbl_language");
            public _Core.StringGetter GamePathLabel => _.Get("lbl_game_path");
            public _Core.StringGetter ConfirmClosingLabel => _.Get("lbl_confirm_closing");
            public _Core.StringGetter CloseWithGameLabel => _.Get("lbl_close_with_game");
            public _Core.StringGetter ConfirmDeletingLabel => _.Get("lbl_confirm_deleting");
            public _Core.StringGetter GameStartOptionsLabel => _.Get("lbl_game_start_options");
            public _Core.StringGetter GameStartCommandLabel => _.Get("lbl_game_start_command");
            public _Core.StringGetter GameStartArgumentsLabel => _.Get("lbl_game_start_arguments");
            public _Core.StringGetter AutoStartGameLabel => _.Get("lbl_auto_start_game");
            public _Core.StringGetter MaxBackupsLabel => _.Get("lbl_max_backups");
            public _Core.StringGetter BackupsOffLabel => _.Get("lbl_backups_off");
            public _Core.StringGetter OverlayWidthLabel => _.Get("lbl_overlay_width");
            public _Core.StringGetter OverlayScaleLabel => _.Get("lbl_overlay_scale");
            public _Core.StringGetter OverlayTransparencyLabel => _.Get("lbl_overlay_transparency");
            public _Core.StringGetter OverlayStashesCountLabel => _.Get("lbl_overlay_stashes_count");
            public _Core.StringGetter OverlayShowWorkloadLabel => _.Get("lbl_overlay_show_workload");
            public _Core.StringGetter AutoBackToMainLabel => _.Get("lbl_auto_back_to_main");
            public _Core.StringGetter CheckVersionLabel => _.Get("lbl_check_version");
            public _Core.StringGetter SaveLockedStashesLabel => _.Get("lbl_save_locked_stashes");
            public _Core.StringGetter ExpansionLabel => _.Get("lbl_expansion");
            public _Core.StringGetter TransferFileLabel => _.Get("lbl_transfer_file");
            public _Core.StringGetter StashNameLabel => _.Get("lbl_stash_name");
            public _Core.StringGetter GroupLabel => _.Get("lbl_group");
            public _Core.StringGetter NameLabel => _.Get("lbl_name");
            public _Core.StringGetter TabsLabel => _.Get("lbl_tabs");
            public _Core.StringGetter SelectFirstStashInGroupLabel => _.Get("lbl_select_first_stash_in_group");
            public _Core.StringGetter OverlayWindowLabel => _.Get("lbl_overlay_window");
            public _Core.StringGetter BehaviorLabel => _.Get("lbl_behavior");
            public _Core.StringGetter SaveExternalChangesLabel => _.Get("lbl_save_external_changes");
            public _Core.StringGetter StartPositionLabel => _.Get("lbl_start_position");
            public _Core.StringGetter StartPositionNormalLabel => _.Get("lbl_start_position_normal");
            public _Core.StringGetter StartPositionMinimizedLabel => _.Get("lbl_start_position_minimized");
            public _Core.StringGetter StartPositionTrayLabel => _.Get("lbl_start_position_tray");







            public _Core.StringGetter IdColumn => _.Get("lvc_id");
            public _Core.StringGetter NameColumn => _.Get("lvc_name");
            public _Core.StringGetter LastChangeColumn => _.Get("lvc_last_change");
            public _Core.StringGetter SoftcoreColumn => _.Get("lvc_softcore");
            public _Core.StringGetter HardcoreColumn => _.Get("lvc_hardcore");




            public _Core.StringGetter DefaultColor => _.Get("color_default");
            public _Core.StringGetter GreenColor => _.Get("color_green");
            public _Core.StringGetter BlueColor => _.Get("color_blue");
            public _Core.StringGetter PurpleColor => _.Get("color_purple");
            public _Core.StringGetter GoldColor => _.Get("color_gold");
            public _Core.StringGetter GrayColor => _.Get("color_gray");
            public _Core.StringGetter WhiteColor => _.Get("color_white");
            public _Core.StringGetter RoseColor => _.Get("color_rose");






            #region sort stashes

            public _Core.StringGetter SortByNone => _.Get("sort_by_none");
            public _Core.StringGetter SortByLevel => _.Get("sort_by_level");
            public _Core.StringGetter SortByQuality => _.Get("sort_by_quality");
            public _Core.StringGetter SortByRarity => _.Get("sort_by_rarity");
            public _Core.StringGetter SortByClass => _.Get("sort_by_class");
            public _Core.StringGetter SortByType => _.Get("sort_by_type");
            public _Core.StringGetter SortBySet => _.Get("sort_by_set");
            public _Core.StringGetter SortByAIO => _.Get("sort_by_aio");
            public _Core.StringGetter SortingFinishedMessage => _.Get("sorting_finished");
            
            #endregion

            #region stash/group names

            public _Core.StringGetter MainGroupName => _.Get("main_group_name");
            public _Core.StringGetter DefaultStashName => _.Get("default_stash_name");
            public _Core.StringGetter DefaultStashGroupName => _.Get("default_stash_group_name");

            #endregion

            #region item qualities

            public _Core.StringGetter ItemQualityNone => _.Get("item_quality_none");
            public _Core.StringGetter ItemQualityCommon => _.Get("item_quality_common");
            public _Core.StringGetter ItemQualityMagical => _.Get("item_quality_magical");
            public _Core.StringGetter ItemQualityRare => _.Get("item_quality_rare");
            public _Core.StringGetter ItemQualityEpic => _.Get("item_quality_epic");
            public _Core.StringGetter ItemQualityLegendary => _.Get("item_quality_legendary");
            public _Core.StringGetter ItemQualityQuest => _.Get("item_quality_quest");
            public _Core.StringGetter ItemQualityBroken => _.Get("item_quality_broken");
            public _Core.StringGetter ItemQualityUnknown => _.Get("item_quality_unknown");

            #endregion

            #region item classes

            public _Core.StringGetter ItemClassWeapons => _.Get("item_class_weapons");

            public _Core.StringGetter ItemClassWeaponsSwords1h => _.Get("item_class_weapons_swords1h");
            public _Core.StringGetter ItemClassWeaponsAxes1h => _.Get("item_class_weapons_axes1h");
            public _Core.StringGetter ItemClassWeaponsMaces1h => _.Get("item_class_weapons_maces1h");
            public _Core.StringGetter ItemClassWeaponsDaggers => _.Get("item_class_weapons_daggers");
            public _Core.StringGetter ItemClassWeaponsScepters => _.Get("item_class_weapons_scepters");
            public _Core.StringGetter ItemClassWeaponsRanged1h => _.Get("item_class_weapons_ranged1h");
            public _Core.StringGetter ItemClassWeaponsShields => _.Get("item_class_weapons_shields");
            public _Core.StringGetter ItemClassWeaponsOffhands => _.Get("item_class_weapons_offhands");
            public _Core.StringGetter ItemClassWeaponsSwords2h => _.Get("item_class_weapons_swords2h");
            public _Core.StringGetter ItemClassWeaponsAxes2h => _.Get("item_class_weapons_axes2h");
            public _Core.StringGetter ItemClassWeaponsMaces2h => _.Get("item_class_weapons_maces2h");
            public _Core.StringGetter ItemClassWeaponsRanged2h => _.Get("item_class_weapons_ranged2h");

            public _Core.StringGetter ItemClassArmor => _.Get("item_class_armor");

            public _Core.StringGetter ItemClassArmorHead => _.Get("item_class_armor_head");
            public _Core.StringGetter ItemClassArmorShoulders => _.Get("item_class_armor_shoulders");
            public _Core.StringGetter ItemClassArmorChest => _.Get("item_class_armor_chest");
            public _Core.StringGetter ItemClassArmorHands => _.Get("item_class_armor_hands");
            public _Core.StringGetter ItemClassArmorWaist => _.Get("item_class_armor_waist");
            public _Core.StringGetter ItemClassArmorLegs => _.Get("item_class_armor_legs");
            public _Core.StringGetter ItemClassArmorFeet => _.Get("item_class_armor_feet");

            public _Core.StringGetter ItemClassJewelry => _.Get("item_class_jewelry");

            public _Core.StringGetter ItemClassJewelryAmulets => _.Get("item_class_jewelry_amulets");
            public _Core.StringGetter ItemClassJewelryMedals => _.Get("item_class_jewelry_medals");
            public _Core.StringGetter ItemClassJewelryRings => _.Get("item_class_jewelry_rings");

            public _Core.StringGetter ItemClassOther => _.Get("item_class_other");

            public _Core.StringGetter ItemClassOtherRelics => _.Get("item_class_other_relics");
            public _Core.StringGetter ItemClassOtherFactions => _.Get("item_class_other_factions");
            public _Core.StringGetter ItemClassOtherConsumable => _.Get("item_class_other_consumable");
            public _Core.StringGetter ItemClassOtherBlueprints => _.Get("item_class_other_blueprints");
            public _Core.StringGetter ItemClassOtherLore => _.Get("item_class_other_lore");
            public _Core.StringGetter ItemClassOtherEnchantments => _.Get("item_class_other_enchantments");
            public _Core.StringGetter ItemClassOtherComponents => _.Get("item_class_other_components");
            public _Core.StringGetter ItemClassOtherTransmutes => _.Get("item_class_other_transmutes");
            public _Core.StringGetter ItemClassOtherBonus => _.Get("item_class_other_bonus");
            public _Core.StringGetter ItemClassOtherEndless => _.Get("item_class_other_endless");

            public _Core.StringGetter ItemClassUnknown => _.Get("item_class_unknown");

            #endregion




            public _Core.StringGetter ZipArchive => _.Get("zip_archive");



            #region Core

            public _Core _;

            public class _Core
            {
                public delegate string StringGetter(params object[] p);
                public readonly Dictionary<string, string> List = new Dictionary<string, string>();

                public _Core()
                {
                }

                public void Clear()
                {
                    List.Clear();
                }

                public void Add(string k, string v)
                {
                    List.Add(k, ParseString(v));
                }

                public StringGetter Get(string k)
                {
                    return delegate (object[] o) {
                        return string.Format(List.ContainsKey(k)
                        ? List[k]
                        : $"{k}", o);
                    };
                }

                private static string ParseString(string v)
                {
                    return v.Replace("\\n", Environment.NewLine);
                }

            }

            public StringsHolder()
            {
                _ = new _Core();
            }

            #endregion

        }

    }
}
