using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.IO;
using System.Text.RegularExpressions;

namespace GrimDawnLib
{
    public static partial class GrimDawn
    {
        public static partial class Steam
        {

            private const string _regKey32 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Valve\Steam";
            private const string _regKey64 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Valve\Steam";

            public static readonly int GameAppID = 219990;

            public static readonly string SteamClientPath32 = (string)Registry.GetValue(_regKey32, "InstallPath", null);
            public static readonly string SteamClientPath64 = (string)Registry.GetValue(_regKey64, "InstallPath", null);

            public static readonly string GamePath32 = FindGamePath(SteamClientPath32);
            public static readonly string GamePath64 = FindGamePath(SteamClientPath64);

            private static string FindGamePath(string steamInstallPath)
            {

                if (steamInstallPath != null)
                {
                    string libIndexFile = Path.Combine(steamInstallPath, "steamapps", "libraryfolders.vdf");
                    if (File.Exists(libIndexFile))
                    {
                        Regex pathRE = new Regex(@"^\s*""path""\s+""(?<path>.*)""\s*$", RegexOptions.IgnoreCase);
                        Regex appRE = new Regex(string.Format(@"^\s*""{0}""\s+""\d+""\s*$", GameAppID), RegexOptions.IgnoreCase);
                        Match m;

                        string libPath = null;
                        string gamePath;

                        foreach (string line in File.ReadAllLines(libIndexFile))
                        {
                            m = pathRE.Match(line);
                            if (m.Success)
                            {
                                libPath = m.Groups["path"].Value.Replace(@"\\", @"\");
                            }
                            else
                            {
                                if (libPath != null)
                                {
                                    m = appRE.Match(line);
                                    if (m.Success)
                                    {
                                        gamePath = Path.Combine(libPath, "steamapps", "common", "Grim Dawn");
                                        if (Directory.Exists(gamePath)) return gamePath;
                                    }
                                }
                            }
                        }
                    }
                }
                return null;
            }




        }
    }
}
