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

            public const string GameAppID = "219990";
            public const string GameStartCommand = "steam://rungameid/" + GameAppID;
            private const string _regKey32 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Valve\Steam";
            private const string _regKey64 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Valve\Steam";

            private static string FindInstallPath(string k)
            {
                return (string)Registry.GetValue(k, "InstallPath", null);
            }

            public static string FindGamePath()
            {
                string InstallPath = FindInstallPath(_regKey64);
                if (InstallPath == null) InstallPath = FindInstallPath(_regKey32);
                if (InstallPath != null)
                {
                    string libIndexFile = Path.Combine(InstallPath, "steamapps", "libraryfolders.vdf");
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
