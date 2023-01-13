using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.ObjectModel;

namespace GrimDawnLib
{

    [Flags]
    public enum GrimDawnGameMode
    {
        None = 0,
        SC = 1,
        HC = 2,
        Both = 3,
    }

    public enum GrimDawnGameExpansion
    {
        Unknown = -1,
        Vanilla = 0,
        AoM = 1,
        FG = 2,
    }

    public class GrimDawnGameEnvironment
    {
        public GrimDawnGameExpansion GameExpansion { get; private set; }
        public GrimDawnGameMode GameMode { get; private set; }
        public string TransferFileName { get; private set; }
        public string TransferFilePath { get; private set; }
        public string TransferFileExtension { get; private set; }
        public GrimDawnGameEnvironment(GrimDawnGameExpansion exp, GrimDawnGameMode mode)
        {
            GameExpansion = exp;
            GameMode = mode;
            TransferFilePath = GrimDawn.GetTransferFilePath(exp, mode);
            TransferFileName = Path.GetFileName(TransferFilePath);
            TransferFileExtension = Path.GetExtension(TransferFilePath);
        }
        public override string ToString()
        {
            return $"{GameMode} {GameExpansion} ({TransferFileExtension})";
        }
    }

    public static partial class GrimDawn
    {

        public static string DocumentsPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "My Games", "Grim Dawn");
        public static string DocumentsSavePath => Path.Combine(DocumentsPath, "save");
        public static string DocumentsSettingsPath => Path.Combine(DocumentsPath, "Settings");

        private static readonly Dictionary<string, GrimDawnGameExpansion> extension2expansion = new Dictionary<string, GrimDawnGameExpansion>
            {
                { "bs", GrimDawnGameExpansion.Vanilla },
                { "cs", GrimDawnGameExpansion.AoM },
                { "gs", GrimDawnGameExpansion.FG },
            };

        private static readonly Dictionary<GrimDawnGameExpansion, string> expansion2extension = new Dictionary<GrimDawnGameExpansion, string>
            {
                { GrimDawnGameExpansion.Vanilla, "bs" },
                { GrimDawnGameExpansion.AoM, "cs" },
                { GrimDawnGameExpansion.FG, "gs" },
            };

        private static readonly Dictionary<string, GrimDawnGameMode> extension2mode = new Dictionary<string, GrimDawnGameMode>
            {
                { "t", GrimDawnGameMode.SC },
                { "h", GrimDawnGameMode.HC },
            };

        private static readonly Dictionary<GrimDawnGameMode, string> mode2extension = new Dictionary<GrimDawnGameMode, string>
            {
                { GrimDawnGameMode.SC, "t" },
                { GrimDawnGameMode.HC, "h" },
            };

        public static GrimDawnGameExpansion LatestExpansion => GrimDawnGameExpansion.FG;

        public static readonly IReadOnlyDictionary<GrimDawnGameExpansion, string> ExpansionNames = new Dictionary<GrimDawnGameExpansion, string>
            {
                { GrimDawnGameExpansion.Vanilla, "Grim Dawn" },
                { GrimDawnGameExpansion.AoM, "Grim Dawn: Ashes of Malmouth" },
                { GrimDawnGameExpansion.FG, "Grim Dawn: Forgotten Gods" },
            };

        public static readonly IReadOnlyList<GrimDawnGameExpansion> ExpansionList = new List<GrimDawnGameExpansion>
            {
                GrimDawnGameExpansion.Vanilla,
                GrimDawnGameExpansion.AoM,
                GrimDawnGameExpansion.FG,
            };

        public static readonly IReadOnlyList<GrimDawnGameEnvironment> GameEnvironmentList = new List<GrimDawnGameEnvironment>
            {
                new GrimDawnGameEnvironment(GrimDawnGameExpansion.Vanilla, GrimDawnGameMode.SC),
                new GrimDawnGameEnvironment(GrimDawnGameExpansion.Vanilla, GrimDawnGameMode.HC),
                new GrimDawnGameEnvironment(GrimDawnGameExpansion.AoM, GrimDawnGameMode.SC),
                new GrimDawnGameEnvironment(GrimDawnGameExpansion.AoM, GrimDawnGameMode.HC),
                new GrimDawnGameEnvironment(GrimDawnGameExpansion.FG, GrimDawnGameMode.SC),
                new GrimDawnGameEnvironment(GrimDawnGameExpansion.FG, GrimDawnGameMode.HC),
            };

        public static string GetTransferFileExtension(GrimDawnGameExpansion exp, GrimDawnGameMode mode)
        {
            return $".{expansion2extension[exp]}{mode2extension[mode]}";
        }

        public static string GetTransferFilePath(GrimDawnGameExpansion exp, GrimDawnGameMode mode)
        {
            return Path.Combine(DocumentsSavePath, "transfer" + GetTransferFileExtension(exp, mode));
        }

        public static GrimDawnGameExpansion GetInstalledExpansionFromPath(string gamePath)
        {
            if (!Directory.Exists(gamePath)) return GrimDawnGameExpansion.Unknown;
            if (Directory.Exists(Path.Combine(gamePath, "gdx2"))) return GrimDawnGameExpansion.FG;
            if (Directory.Exists(Path.Combine(gamePath, "gdx1"))) return GrimDawnGameExpansion.AoM;
            return GrimDawnGameExpansion.Vanilla;
        }

        public static bool ValidGamePath(string gamePath)
        {
            if (!File.Exists(Path.Combine(gamePath, "Grim Dawn.exe"))) return false;
            if (!Directory.Exists(Path.Combine(gamePath, "database"))) return false;
            if (!Directory.Exists(Path.Combine(gamePath, "resources"))) return false;
            return true;
        }

        public static bool ValidDocsPath()
        {
            // TODO... could be done better
            return Directory.Exists(DocumentsPath);
        }

        public static System.Windows.Forms.DialogResult ShowSelectTransferFilesDialog(out string[] files, bool multiselect, bool allExtensions)
        {
            string filter = string.Join(";", GameEnvironmentList.Select(env => $"*{env.TransferFileExtension}"));
            files = new string[0];
            using (var dialog = new System.Windows.Forms.OpenFileDialog()
            {
                Filter = $"transfer|{filter}" + (allExtensions ? "|*|*.*" : ""),
                Multiselect = multiselect,
            })
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                    files = dialog.FileNames;
                return result;
            }
        }









































        public static partial class Stashes
        {

            public struct StashFileValues
            {
                public bool IsExpansion1;
                public uint Width;
                public uint Height;
                public uint MaxTabs;
            }

            private static readonly Dictionary<GrimDawnGameExpansion, StashFileValues> _stashFileValues = new Dictionary<GrimDawnGameExpansion, StashFileValues> {
                { GrimDawnGameExpansion.Vanilla, new StashFileValues {
                    IsExpansion1 = false,
                    Width = 8,
                    Height = 16,
                    MaxTabs = 4,
                } },
                { GrimDawnGameExpansion.AoM, new StashFileValues {
                    IsExpansion1 = true,
                    Width = 10,
                    Height = 18,
                    MaxTabs = 5,
                } },
                { GrimDawnGameExpansion.FG, new StashFileValues {
                    IsExpansion1 = false,
                    Width = 10,
                    Height = 18,
                    MaxTabs = 6,
                } },
            };

            public static StashFileValues GetStashInfoForExpansion(GrimDawnGameExpansion exp)
            {
                if (_stashFileValues.TryGetValue(exp, out StashFileValues v))
                {
                    return new StashFileValues
                    {
                        IsExpansion1 = v.IsExpansion1,
                        Width = v.Width,
                        Height = v.Height,
                        MaxTabs = v.MaxTabs,
                    };
                }
                return new StashFileValues
                {
                    IsExpansion1 = false,
                    Width = 0,
                    Height = 0,
                    MaxTabs = 0,
                };
            }

        }








    }
}
