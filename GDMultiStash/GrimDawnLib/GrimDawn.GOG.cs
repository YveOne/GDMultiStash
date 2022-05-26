using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.IO;

namespace GrimDawnLib
{
    public static partial class GrimDawn
    {
        public static partial class GOG
        {

            public static readonly int GameAppID = 1449651388;

            public static readonly string GalaxyPath = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\GOG.com\GalaxyClient\paths", "client", null);
            public static readonly string GalaxyExe = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\GOG.com\GalaxyClient\paths", "client", null);
            public static readonly string GalaxyClientPath = (GalaxyPath != null && GalaxyExe != null) ? Path.Combine(GalaxyPath, GalaxyExe) : null;

            public static readonly string GamePath64 = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\GOG.com\Games\" + GameAppID.ToString(), "path", null);
            public static readonly string GameStartCommand64 = string.Format(@"""{0}"" /command=runGame /gameId=1449651388 /path=""{1}""", GalaxyClientPath, GamePath64);

        }
    }
}
