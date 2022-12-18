using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMultiStash
{
    internal static class Global
    {
        public static readonly string AppName = "GDMultiStash";
        public static readonly int WM_SHOWME = Native.RegisterWindowMessage("GDMS_SHOW");

        public static GlobalHandlers.ConfigurationHandler Configuration = new GlobalHandlers.ConfigurationHandler();
        public static GlobalHandlers.FileSystemHandler FileSystem = new GlobalHandlers.FileSystemHandler();
        public static GlobalHandlers.LocalizationHandler Localization = new GlobalHandlers.LocalizationHandler();
        public static GlobalHandlers.LocalizationHandler.StringsHolder L = Localization.Strings;
        public static GlobalHandlers.DatabaseHandler Database = new GlobalHandlers.DatabaseHandler();
        public static GlobalHandlers.RuntimeHandler Runtime = new GlobalHandlers.RuntimeHandler();
        public static GlobalHandlers.StashesHandler Stashes = new GlobalHandlers.StashesHandler();
        public static GlobalHandlers.UpdateHandler Update = new GlobalHandlers.UpdateHandler();
        public static GlobalHandlers.WindowsHandler Windows = new GlobalHandlers.WindowsHandler();
        public static GlobalHandlers.SoundHandler Sounds = new GlobalHandlers.SoundHandler();
    }
}
