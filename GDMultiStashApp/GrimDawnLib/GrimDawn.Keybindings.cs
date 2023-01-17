using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;



namespace GrimDawnLib
{

    public enum GrimDawnKey
    {
        // 0
        CharacterWindow = 1,
        SkillWindow = 2,
        CodexWindow = 3,
        MapWindow = 4,
        ChatWindow = 5,
        GroupWindow = 6,
        GameMenu = 7,
        HelpWindow = 8,
        QuickbarSlot1 = 9,
        QuickbarSlot2 = 10,
        QuickbarSlot3 = 11,
        QuickbarSlot4 = 12,
        QuickbarSlot5 = 13,
        QuickbarSlot6 = 14,
        QuickbarSlot7 = 15,
        QuickbarSlot8 = 16,
        QuickbarSlot9 = 17,
        QuickbarSlot10 = 18,
        // 19
        // 20
        CameraZoomIn = 21,
        CameraZoomOut = 22,
        CameraMaxZoomIn = 23,
        CameraMaxZoomOut = 24,
        CameraDefaultView = 25,
        CameraRotate = 26,
        CameraRotateLeft = 27,
        CameraRotateRight = 28,
        DrinkEnergyPotion = 29,
        DrinkHealthPotion = 30,
        CenterMap = 31,
        DropItem = 32,
        PersonalRiftgate = 33,
        SwitchWeapons = 34,
        ShowItemsNoFilter = 35,
        ShowItemTooltips = 36,
        ShowItemsFilterCommon = 37,
        TargetPet = 38,
        StationaryAttack = 39,
        PauseGame = 40,
        TogglePetDisplay = 41,
        TogglePartyDisplay = 42,
        QuickbarSwitch = 43,
        SelectPet1 = 44,
        SelectPet2 = 45,
        SelectPet3 = 46,
        SelectPet4 = 47,
        SelectPet5 = 48,
        SelectAllPets = 49,
        ForceMove = 50,
        Move = 51,
        FactionsWindow = 52,
        AchievementsWindow = 53,
        Interact = 54,
        Pickup = 55,
        LootFilterWindow = 56,
        ToggleHideAllItems = 57,
        PushToTalk = 58,
        // 59
    }

    public class GrimDawnKeyBinding
    {
        public ushort Primary { get; }
        public ushort Secondary { get; }
        public GrimDawnKeyBinding(ushort Primary, ushort Secondary)
        {
            this.Primary = Primary;
            this.Secondary = Secondary;
        }
    }


    public static partial class GrimDawn
    {
        public static partial class Keybindings
        {

            private static readonly string keybindingsFile = Path.Combine(DocumentsSettingsPath, "keybindings.txt");

            private static long lastWriteTicks = 0;
            private static Dictionary<GrimDawnKey, GrimDawnKeyBinding> kb;

            static Keybindings()
            {
                FileChanged();
                kb = LoadKeybindings(keybindingsFile);
            }

            public static GrimDawnKeyBinding GetKeyBinding(GrimDawnKey k)
            {
                if (FileChanged()) kb = LoadKeybindings(keybindingsFile);
                return kb[k];
            }

            private static bool FileChanged()
            {
                if (!File.Exists(keybindingsFile)) return false;
                long curWriteTicks = new FileInfo(keybindingsFile).LastWriteTime.Ticks;
                if (curWriteTicks != lastWriteTicks)
                {
                    lastWriteTicks = curWriteTicks;
                    return true;
                }
                return false;
            }

            private static readonly Regex regex = new Regex(@"^\s*(\d+)\s*:\s+(\d+)\s+(\d+)\s*$", RegexOptions.Multiline);

            public static Dictionary<GrimDawnKey, GrimDawnKeyBinding> LoadKeybindings(string filePath)
            {
                if (!File.Exists(filePath)) return KeyDefaults;

                Dictionary<GrimDawnKey, GrimDawnKeyBinding> kb = new Dictionary<GrimDawnKey, GrimDawnKeyBinding>();
                string filetext = File.ReadAllText(filePath);
                Match m = regex.Match(filetext);
                while (m.Success)
                {
                    uint keyId = UInt32.Parse(m.Groups[1].Value);
                    ushort priCode = ushort.Parse(m.Groups[2].Value);
                    ushort secCode = ushort.Parse(m.Groups[3].Value);
                    kb.Add((GrimDawnKey)keyId, new GrimDawnKeyBinding(priCode, secCode));
                    m = m.NextMatch();
                }
                return kb;
            }

            public static Dictionary<GrimDawnKey, GrimDawnKeyBinding> KeyDefaults = new Dictionary<GrimDawnKey, GrimDawnKeyBinding> {
                // 0
                { GrimDawnKey.CharacterWindow, new GrimDawnKeyBinding(46, 23) },
                { GrimDawnKey.SkillWindow, new GrimDawnKeyBinding(31, 0) },
                { GrimDawnKey.CodexWindow, new GrimDawnKeyBinding(16, 0) },
                { GrimDawnKey.MapWindow, new GrimDawnKeyBinding(50, 154) },
                { GrimDawnKey.ChatWindow, new GrimDawnKeyBinding(28, 0) },
                { GrimDawnKey.GroupWindow, new GrimDawnKeyBinding(25, 0) },
                { GrimDawnKey.GameMenu, new GrimDawnKeyBinding(34, 0) },
                { GrimDawnKey.HelpWindow, new GrimDawnKeyBinding(35, 0) },
                { GrimDawnKey.QuickbarSlot1, new GrimDawnKeyBinding(2, 165) },
                { GrimDawnKey.QuickbarSlot2, new GrimDawnKeyBinding(3, 167) },
                { GrimDawnKey.QuickbarSlot3, new GrimDawnKeyBinding(4, 168) },
                { GrimDawnKey.QuickbarSlot4, new GrimDawnKeyBinding(5, 166) },
                { GrimDawnKey.QuickbarSlot5, new GrimDawnKeyBinding(6, 164) },
                { GrimDawnKey.QuickbarSlot6, new GrimDawnKeyBinding(7, 155) },
                { GrimDawnKey.QuickbarSlot7, new GrimDawnKeyBinding(8, 156) },
                { GrimDawnKey.QuickbarSlot8, new GrimDawnKeyBinding(9, 0) },
                { GrimDawnKey.QuickbarSlot9, new GrimDawnKeyBinding(10, 0) },
                { GrimDawnKey.QuickbarSlot10, new GrimDawnKeyBinding(11, 0) },
                // 19
                // 20
                { GrimDawnKey.CameraZoomIn, new GrimDawnKeyBinding(145, 0) },
                { GrimDawnKey.CameraZoomOut, new GrimDawnKeyBinding(146, 0) },
                { GrimDawnKey.CameraMaxZoomIn, new GrimDawnKeyBinding(79, 0) },
                { GrimDawnKey.CameraMaxZoomOut, new GrimDawnKeyBinding(81, 0) },
                { GrimDawnKey.CameraDefaultView, new GrimDawnKeyBinding(80, 0) },
                { GrimDawnKey.CameraRotate, new GrimDawnKeyBinding(147, 0) },
                { GrimDawnKey.CameraRotateLeft, new GrimDawnKeyBinding(51, 0) },
                { GrimDawnKey.CameraRotateRight, new GrimDawnKeyBinding(52, 0) },
                { GrimDawnKey.DrinkEnergyPotion, new GrimDawnKeyBinding(18, 162) },
                { GrimDawnKey.DrinkHealthPotion, new GrimDawnKeyBinding(19, 161) },
                { GrimDawnKey.CenterMap, new GrimDawnKeyBinding(82, 0) },
                { GrimDawnKey.DropItem, new GrimDawnKeyBinding(32, 0) },
                { GrimDawnKey.PersonalRiftgate, new GrimDawnKeyBinding(38, 153) },
                { GrimDawnKey.SwitchWeapons, new GrimDawnKeyBinding(17, 159) },
                { GrimDawnKey.ShowItemsNoFilter, new GrimDawnKeyBinding(56, 118) },
                { GrimDawnKey.ShowItemTooltips, new GrimDawnKeyBinding(45, 0) },
                { GrimDawnKey.ShowItemsFilterCommon, new GrimDawnKeyBinding(44, 0) },
                { GrimDawnKey.TargetPet, new GrimDawnKeyBinding(29, 107) },
                { GrimDawnKey.StationaryAttack, new GrimDawnKeyBinding(42, 163) },
                { GrimDawnKey.PauseGame, new GrimDawnKeyBinding(57, 0) },
                { GrimDawnKey.TogglePetDisplay, new GrimDawnKeyBinding(14, 0) },
                { GrimDawnKey.TogglePartyDisplay, new GrimDawnKeyBinding(43, 0) },
                { GrimDawnKey.QuickbarSwitch, new GrimDawnKeyBinding(21, 160) },
                { GrimDawnKey.SelectPet1, new GrimDawnKeyBinding(60, 0) },
                { GrimDawnKey.SelectPet2, new GrimDawnKeyBinding(61, 0) },
                { GrimDawnKey.SelectPet3, new GrimDawnKeyBinding(62, 0) },
                { GrimDawnKey.SelectPet4, new GrimDawnKeyBinding(63, 0) },
                { GrimDawnKey.SelectPet5, new GrimDawnKeyBinding(64, 0) },
                { GrimDawnKey.SelectAllPets, new GrimDawnKeyBinding(65, 0) },
                { GrimDawnKey.ForceMove, new GrimDawnKeyBinding(33, 0) },
                { GrimDawnKey.Move, new GrimDawnKeyBinding(30, 0) },
                { GrimDawnKey.FactionsWindow, new GrimDawnKeyBinding(36, 0) },
                { GrimDawnKey.AchievementsWindow, new GrimDawnKeyBinding(47, 0) },
                { GrimDawnKey.Interact, new GrimDawnKeyBinding(22, 0) },
                { GrimDawnKey.Pickup, new GrimDawnKeyBinding(0, 0) },
                { GrimDawnKey.LootFilterWindow, new GrimDawnKeyBinding(24, 0) },
                { GrimDawnKey.ToggleHideAllItems, new GrimDawnKeyBinding(0, 0) },
                { GrimDawnKey.PushToTalk, new GrimDawnKeyBinding(15, 0) },
                // 59
            };

        }
    }
}
