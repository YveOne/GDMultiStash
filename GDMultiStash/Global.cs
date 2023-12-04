using System;
using System.Linq;
using System.Text;
using System.IO;

namespace GDMultiStash
{
    using GrimDawnLib;
    using GDMultiStash.Common.Objects;
    using GDMultiStash.Global;

    internal static class G
    {
        public static FileSystemManager FileSystem { get; } = new FileSystemManager();
        public static ConfigurationManager Configuration { get; } = new ConfigurationManager();
        public static LocalizationManager Localization { get; } = new LocalizationManager();
        public static LocalizationManager.StringsHolder L { get; } = Localization.Strings;
        public static DatabaseManager Database { get; } = new DatabaseManager();
        public static StashesManager Stashes { get; } = new StashesManager();
        public static StashGroupsManager StashGroups { get; } = new StashGroupsManager();
        public static UpdateManager Update { get; } = new UpdateManager();
        public static WindowsManager Windows { get; } = new WindowsManager();
        public static ResourcesManager Resources { get; } = new ResourcesManager();
        public static SoundsManager Sounds { get; } = new SoundsManager();
        public static RuntimeManager Runtime { get; } = new RuntimeManager();
        public static IngameManager Ingame { get; } = new IngameManager();

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
                Console.WriteLine($"- active Stash ID: {Stashes.ActiveStashID}");
                if (!Ingame.StashIsReopening && !Runtime.StashIsOpened)
                {
                    if (!Utils.Funcs.WaitFor(() => !Utils.FileUtils.FileIsLocked(externalFile), 2000, 33))
                    {
                        Console.WriteLine("- file locked");
                        return;
                    }
                    if (!Stashes.SwitchBackToMainStash())
                    {
                        Stashes.SwitchToStash(Stashes.ActiveStashID);
                    }
                }

            };

            FileSystem.TransferFileChanged += delegate (object sender, FileSystemManager.TransferFileChangedEventArgs e) {
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

            Ingame.RequestReopenStash += delegate (object sender, Global.Stashes.StashObjectsEventArgs e) {
                foreach (var s in e.Items)
                {
                    if (Runtime.GameInitialized && Runtime.StashIsOpened && s.ID == Stashes.ActiveStashID)
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

            StashGroups.ActiveGroupChanged += delegate (object sender, Global.StashGroups.ActiveStashGroupChangedEventArgs e) {
                if (Configuration.Settings.AutoSelectFirstStashInGroup)
                {
                    var stashesInGroup = Stashes.GetStashesForGroup(e.NewID).ToList();
                    if (stashesInGroup.Count != 0)
                    {
                        stashesInGroup.Sort(new Common.Objects.Sorting.Comparer.StashesSortComparer());
                        var stashID = stashesInGroup[0].ID;
                        if (stashID != Stashes.ActiveStashID)
                        {
                            Ingame.SwitchToStash(stashID);
                        }
                    }
                }
            };

            Stashes.StashesContentChanged += delegate (object sender, Global.Stashes.StashesContentChangedEventArgs args)
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
                Stashes.LoadActiveStashID();
            };

            Runtime.ActiveExpansionChanged += delegate {
                Stashes.LoadActiveStashID();
            };

            Runtime.GameWindowConnected += delegate {
                // automatically set active expansion to installed expansion
                Runtime.ActiveExpansion = GrimDawn.GetInstalledExpansionFromPath(Configuration.Settings.GamePath);
            };

            Runtime.GameWindowDisconnected += delegate {
                // grim dawn closed
                Runtime.StashIsOpened = false;
                Stashes.SwitchBackToMainStash();
            };

        }

    }
}
