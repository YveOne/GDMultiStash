using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace GDMultiStash.GlobalHandlers
{
    internal class FileSystemHandler
    {

        public string DataDirectory { get; private set; }
        public string StashesDirectory { get; private set; }
        public string LocalsDirectory { get; private set; }
        public string ConfigFile { get; private set; }

        public FileSystemHandler()
        {
            DataDirectory = Path.Combine(Application.StartupPath, "Data");
            StashesDirectory = Path.Combine(DataDirectory, "Stashes");
            LocalsDirectory = Path.Combine(DataDirectory, "Locales");
            ConfigFile = Path.Combine(DataDirectory, "Config.xml");
        }

        public void CreateDirectories()
        {
            if (!Directory.Exists(DataDirectory)) Directory.CreateDirectory(DataDirectory);
            if (!Directory.Exists(StashesDirectory)) Directory.CreateDirectory(StashesDirectory);
            if (!Directory.Exists(LocalsDirectory)) Directory.CreateDirectory(LocalsDirectory);
        }

        public string GetStashDirectory(int stashID)
        {
            return Path.Combine(StashesDirectory, stashID.ToString());
        }

        public void CreateStashDirectory(int stashID)
        {
            string stashDir = GetStashDirectory(stashID);
            if (!Directory.Exists(stashDir)) Directory.CreateDirectory(stashDir);
        }

        public void DeleteStashDirectory(int stashID)
        {
            string stashDir = GetStashDirectory(stashID);
            if (Directory.Exists(stashDir)) Directory.Delete(stashDir, true);
        }

        public string GetStashTransferFile(int stashID, int backupIndex = -1)
        {
            return Path.Combine(GetStashDirectory(stashID), "transfer" + (backupIndex >= 0 ? backupIndex.ToString() : ""));
        }

        public void CreateStashTransferFile(int stashID, GrimDawnLib.GrimDawnGameExpansion expansion)
        {
            CreateStashDirectory(stashID);
            Common.TransferFile
                .CreateForExpansion(expansion)
                .WriteToFile(GetStashTransferFile(stashID));
        }

        public bool ImportStashTransferFile(int stashID, string srcFile, bool forceBackup = false)
        {
            if (!File.Exists(srcFile)) return false;
            string destFile = GetStashTransferFile(stashID);
            if (File.Exists(destFile))
            {
                string srcHash = Utils.FileUtils.GetFileHash(srcFile);
                string destHash = Utils.FileUtils.GetFileHash(destFile);
                if (srcHash == null || destHash == null) return false; // file locked
                if (srcHash != destHash || forceBackup) BackupStashTransferFile(stashID);
                File.Delete(destFile);
            }
            File.Copy(srcFile, destFile);
            return true;
        }

        public bool ExportStashTransferFile(int stashID, string destFile)
        {
            string srcFile = GetStashTransferFile(stashID);
            if (!File.Exists(srcFile)) return false;
            string destDir = Path.GetDirectoryName(destFile);
            if (!Directory.Exists(destDir)) Directory.CreateDirectory(destDir);
            File.Copy(srcFile, destFile, true);
            return true;
        }

        public void BackupStashTransferFile(int stashID)
        {
            if (!File.Exists(GetStashTransferFile(stashID))) return;
            int backupIndex = 0;
            while (File.Exists(GetStashTransferFile(stashID, backupIndex + 1)))
            {
                backupIndex += 1;
            }
            if (backupIndex > 0)
            {
                int maxBackups = Global.Configuration.Settings.MaxBackups;
                if (maxBackups >= 0)
                {
                    for (int i = (maxBackups == 0) ? 1 : maxBackups; i <= backupIndex; i += 1)
                    {
                        File.Delete(GetStashTransferFile(stashID, i));
                        backupIndex -= 1;
                    }
                }
                else
                {
                    // unlimited backups
                }
            }
            while (backupIndex > 0)
            {
                File.Move(GetStashTransferFile(stashID, backupIndex), GetStashTransferFile(stashID, backupIndex + 1));
                backupIndex -= 1;
            }
            File.Move(GetStashTransferFile(stashID), GetStashTransferFile(stashID, 1));
        }

        public bool RestoreStashTransferFile(int stashID, string srcFile)
        {
            Console.WriteLine("Restoring stash: {0} -> {1}", stashID.ToString(), srcFile);
            File.Move(srcFile, srcFile + ".tmp");
            if (ImportStashTransferFile(stashID, srcFile + ".tmp", true))
            {
                File.Delete(srcFile + ".tmp");
                return true;
            }
            File.Move(srcFile + ".tmp", srcFile);
            return false;
        }

    }
}
