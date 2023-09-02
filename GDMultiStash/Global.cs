using System;
using System.Linq;
using System.Text;
using System.IO;

using GrimDawnLib;
using GDMultiStash.GlobalHandlers;
using GDMultiStash.Common.Objects;

namespace GDMultiStash
{
    internal static class Global
    {
        public static ConfigurationHandler Configuration { get; } = new ConfigurationHandler();
        public static FileSystemHandler FileSystem { get; } = new FileSystemHandler();
        public static LocalizationHandler Localization { get; } = new LocalizationHandler();
        public static LocalizationHandler.StringsHolder L { get; } = Localization.Strings;
        public static DatabaseHandler Database { get; } = new DatabaseHandler();
        public static StashesHandler Stashes { get; } = new StashesHandler();
        public static GroupsHandler Groups { get; } = new GroupsHandler();
        public static UpdateHandler Update { get; } = new UpdateHandler();
        public static WindowsHandler Windows { get; } = new WindowsHandler();
        public static ResourceHandler Resources { get; } = new ResourceHandler();
        public static SoundHandler Sounds { get; } = new SoundHandler();
        public static RuntimeHandler Runtime { get; } = new RuntimeHandler();
        public static IngameHandler Ingame { get; } = new IngameHandler();

        public static void SetEventHandlers()
        {

            bool _reopenIngameStash = false;

            Runtime.TransferFileSaved += delegate {
                Console.WriteLine($"transfer file saved by game");

                string externalFile = GrimDawn.GetTransferFilePath(Runtime.ActiveExpansion, Runtime.ActiveMode);
                string externalFileName = Path.GetFileName(externalFile);
                FileSystem.Watcher.SkipNextFile(externalFileName);

                Console.WriteLine($"- file: {externalFileName}");
                Console.WriteLine($"- stash is reopening: {Ingame.StashIsReopening}");
                Console.WriteLine($"- stash is opened: {Runtime.StashIsOpened}");
                Console.WriteLine($"- active Stash ID: {Runtime.ActiveStashID}");
                if (!Ingame.StashIsReopening && !Runtime.StashIsOpened)
                {
                    if (!Utils.Funcs.WaitFor(() => !Utils.FileUtils.FileIsLocked(externalFile), 2000, 33))
                    {
                        Console.WriteLine("- file locked");
                        return;
                    }
                    if (!Runtime.AutoBackToMain())
                    {
                        Stashes.SwitchToStash(Runtime.ActiveStashID);
                    }
                }

            };

            FileSystem.TransferFileChanged += delegate (object sender, FileSystemHandler.TransferFileChangedEventArgs e) {
                Console.WriteLine($"transfer file changed by external");
                Console.WriteLine($"- file: {e.FileName}");
                var externalEnv = GrimDawn.GetEnvironmentByFilename(e.FileName);
                var stashId = Configuration.GetCurrentStashID(externalEnv);
                if (Configuration.Settings.SaveExternalChanges)
                {
                    Console.WriteLine($"saving external changes...");
                    Stashes.ImportStash(stashId, externalEnv, true);
                    // export again because of shared mode stashes (sc+hc)
                    Stashes.ExportStash(stashId);
                }
                Ingame.InvokeRequestReopenStash(Stashes.GetStash(stashId));
            };

            Ingame.RequestReopenStash += delegate (object sender, RuntimeHandler.ListEventArgs<StashObject> e) {
                foreach (var s in e.Items)
                {
                    if (Runtime.GameInitialized && Runtime.StashIsOpened && s.ID == Runtime.ActiveStashID)
                    {
                        Console.WriteLine($"requesting reopening stash");
                        if (Runtime.GameWindowFocused)
                        {
                            Ingame.ReloadCurrentStash();
                        }
                        else
                        {
                            _reopenIngameStash = true;
                        }
                        return;
                    }
                }
            };

            Runtime.ActiveGroupChanged += delegate (object sender, RuntimeHandler.ActiveGroupChangedEventArgs e) {
                if (Configuration.Settings.AutoSelectFirstStashInGroup)
                {
                    var stashesInGroup = Stashes.GetStashesForGroup(e.NewID).ToList();
                    if (stashesInGroup.Count != 0)
                    {
                        stashesInGroup.Sort(new Common.Objects.Sorting.StashesSortComparer());
                        var stashID = stashesInGroup[0].ID;
                        if (stashID != Runtime.ActiveStashID)
                        {
                            Ingame.SwitchToStash(stashID);
                        }
                    }
                }
            };

            Runtime.StashesContentChanged += delegate (object sender, RuntimeHandler.StashesContentChangedEventArgs args)
            {
                if (args.NeedExport)
                {
                    foreach (var s in args.Items)
                        Stashes.ExportStash(s.ID);
                    // because we are exporting we also need to request reopen ingame stash
                    Ingame.InvokeRequestReopenStash(args.Items);
                }
            };

            Runtime.GameWindowGotFocus += delegate
            {
                if (_reopenIngameStash)
                {
                    _reopenIngameStash = false;
                    new System.Threading.Thread(new System.Threading.ThreadStart(() => {
                        System.Threading.Thread.Sleep(500);
                        Ingame.ReloadCurrentStash();
                    })).Start();
                }
            };

            Runtime.ActiveModeChanged += delegate {
                Runtime.LoadActiveStashID();
            };

            Runtime.ActiveExpansionChanged += delegate {
                Runtime.LoadActiveStashID();
            };

            Runtime.GameWindowConnected += delegate {
                // automatically set active expansion to installed expansion
                Runtime.ActiveExpansion = GrimDawn.GetInstalledExpansionFromPath(Configuration.Settings.GamePath);
            };

            Runtime.GameWindowDisconnected += delegate {
                // grim dawn closed
                Runtime.StashIsOpened = false;
                Runtime.AutoBackToMain();
            };

        }

    }
}
