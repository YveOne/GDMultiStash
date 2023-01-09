using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMultiStash
{
    internal static class Global
    {
        public static string AppName { get; } = "GDMultiStash";
        public static int WM_SHOWME { get; } = Native.RegisterWindowMessage("GDMS_SHOW");

        public static GlobalHandlers.ConfigurationHandler Configuration { get; } = new GlobalHandlers.ConfigurationHandler();
        public static GlobalHandlers.FileSystemHandler FileSystem { get; } = new GlobalHandlers.FileSystemHandler();
        public static GlobalHandlers.LocalizationHandler Localization { get; } = new GlobalHandlers.LocalizationHandler();
        public static GlobalHandlers.LocalizationHandler.StringsHolder L { get; } = Localization.Strings;
        public static GlobalHandlers.DatabaseHandler Database { get; } = new GlobalHandlers.DatabaseHandler();
        public static GlobalHandlers.RuntimeHandler Runtime { get; } = new GlobalHandlers.RuntimeHandler();
        public static GlobalHandlers.StashesHandler Stashes { get; } = new GlobalHandlers.StashesHandler();
        public static GlobalHandlers.UpdateHandler Update { get; } = new GlobalHandlers.UpdateHandler();
        public static GlobalHandlers.WindowsHandler Windows { get; } = new GlobalHandlers.WindowsHandler();
        public static GlobalHandlers.SoundHandler Sounds { get; } = new GlobalHandlers.SoundHandler();
    }
}
