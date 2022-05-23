using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GrimDawnLib
{

    [Flags]
    public enum GrimDawnGameMode
    {
        None = 0,
        SC = 1,
        HC = 2,
    }

    public enum GrimDawnGameExpansion
    {
        Unknown = -1,
        BaseGame = 0,
        AshesOfMalmouth = 1,
        ForgottenGods = 2,
    }

    public class GrimDawnGameEnvironment
    {
        // using class because it can be null
        public GrimDawnGameExpansion Expansion;
        public GrimDawnGameMode Mode;
    }

    public static partial class GrimDawn
    {

        private static readonly string DocumentsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "My Games", "Grim Dawn");
        private static readonly string DocumentsSavePath = Path.Combine(DocumentsPath, "save");
        private static readonly string DocumentsSettingsPath = Path.Combine(DocumentsPath, "Settings");

        private static readonly Dictionary<GrimDawnGameExpansion, string> GameExpansionNames = new Dictionary<GrimDawnGameExpansion, string>
            {
                { GrimDawnGameExpansion.BaseGame, "Grim Dawn" },
                { GrimDawnGameExpansion.AshesOfMalmouth, "Grim Dawn: Ashes of Malmouth" },
                { GrimDawnGameExpansion.ForgottenGods, "Grim Dawn: Forgotten Gods" },
            };

        private static readonly Dictionary<string, GrimDawnGameExpansion> extension2expansion = new Dictionary<string, GrimDawnGameExpansion>
            {
                { "bs", GrimDawnGameExpansion.BaseGame },
                { "cs", GrimDawnGameExpansion.AshesOfMalmouth },
                { "gs", GrimDawnGameExpansion.ForgottenGods },
            };

        private static readonly Dictionary<GrimDawnGameExpansion, string> expansion2extension = new Dictionary<GrimDawnGameExpansion, string>
            {
                { GrimDawnGameExpansion.BaseGame, "bs" },
                { GrimDawnGameExpansion.AshesOfMalmouth, "cs" },
                { GrimDawnGameExpansion.ForgottenGods, "gs" },
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

        public static string[] GetExpansionNames()
        {
            return new string[3] {
                GameExpansionNames[GrimDawnGameExpansion.BaseGame],
                GameExpansionNames[GrimDawnGameExpansion.AshesOfMalmouth],
                GameExpansionNames[GrimDawnGameExpansion.ForgottenGods],
            };
        }

        public static string GetExpansionExtensionPart(GrimDawnGameExpansion expansion)
        {
            return "." + expansion2extension[expansion];
        }







        public static string GetExpansionName(GrimDawnGameExpansion exp)
        {
            return (exp != GrimDawnGameExpansion.Unknown) ? GameExpansionNames[exp] : "???";
        }

        public static string GetExpansionName(int exp)
        {
            return GetExpansionName((GrimDawnGameExpansion)exp);
        }

        public static GrimDawnGameExpansion GetInstalledExpansionFromPath(string gamePath)
        {
            if (!Directory.Exists(gamePath)) return GrimDawnGameExpansion.Unknown;
            if (Directory.Exists(Path.Combine(gamePath, "gdx2"))) return GrimDawnGameExpansion.ForgottenGods;
            if (Directory.Exists(Path.Combine(gamePath, "gdx1"))) return GrimDawnGameExpansion.AshesOfMalmouth;
            return GrimDawnGameExpansion.BaseGame;
        }

        public static bool ValidGamePath(string gamePath)
        {
            // todo!!
            return File.Exists(Path.Combine(gamePath, "Grim Dawn.exe"));
        }

        public static bool ValidDocsPath()
        {
            // TODO... could be done better
            return Directory.Exists(DocumentsPath);
        }






        public static GrimDawnGameEnvironment GetEnvironmentByExtension(string ext)
        {
            ext = ext.ToLower();
            if (ext.Length != 4) return null;

            string expKey = ext.Substring(1, 2);
            string modeKey = ext.Substring(3, 1);

            if (!extension2expansion.ContainsKey(expKey))
            {
                Console.WriteLine("Error in GetEnvironmentByExtension(\"{0}\")", ext);
                Console.WriteLine("  unknown expansion");
                return null;
            }

            if (!extension2mode.ContainsKey(modeKey))
            {
                Console.WriteLine("Error in GetEnvironmentByExtension(\"{0}\")", ext);
                Console.WriteLine("  unknown mode");
                return null;
            }

            return new GrimDawnGameEnvironment
            {
                Expansion = extension2expansion[expKey],
                Mode = extension2mode[modeKey],
            };
        }

        public static GrimDawnGameEnvironment GetEnvironmentByFilename(string fileName)
        {
            return GetEnvironmentByExtension(Path.GetExtension(fileName));
        }













        public static string CreateTransferExtension(GrimDawnGameExpansion expansion, GrimDawnGameMode mode)
        {
            return string.Format(".{0}{1}", expansion2extension[expansion], mode2extension[mode]);
        }

        public static string GetTransferFilePath(GrimDawnGameExpansion expansion, GrimDawnGameMode mode)
        {
            return Path.Combine(DocumentsSavePath, "transfer" + CreateTransferExtension(expansion, mode));
        }

        public static DateTime GetLastWriteTime(GrimDawnGameExpansion expansion, GrimDawnGameMode mode)
        {
            return File.GetLastWriteTime(GetTransferFilePath(expansion, mode));
        }




    }
}
