﻿using System;
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
        public string ConfigFile { get; private set; }

        public FileSystemHandler()
        {
            DataDirectory = Path.Combine(Application.StartupPath, "Data");
            StashesDirectory = Path.Combine(DataDirectory, "Stashes");
            ConfigFile = Path.Combine(DataDirectory, "Config.xml");
        }

        public void CreateDirectories()
        {
            if (!Directory.Exists(DataDirectory)) Directory.CreateDirectory(DataDirectory);
            if (!Directory.Exists(StashesDirectory)) Directory.CreateDirectory(StashesDirectory);
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

        public void CopyStashDirectory(int id1, int id2)
        {
            string dir1 = GetStashDirectory(id1);
            string dir2 = GetStashDirectory(id2);
            if (!Directory.Exists(dir1)) return;
            if (Directory.Exists(dir2)) return;
            CreateStashDirectory(id2);
            var src = new DirectoryInfo(dir1);
            var dest = new DirectoryInfo(dir2);
            foreach (FileInfo file in src.GetFiles())
            {
                file.CopyTo(Path.Combine(dir2, file.Name));
            }
        }

        public void DeleteStashDirectory(int stashID)
        {
            string stashDir = GetStashDirectory(stashID);
            if (Directory.Exists(stashDir)) Directory.Delete(stashDir, true);
        }

        public string GetStashTransferFile(int stashID, int backupIndex = -1)
        {
            return Path.Combine(GetStashDirectory(stashID), "transfer" + (backupIndex > 0 ? backupIndex.ToString() : ""));
        }

        public void CreateStashTransferFile(int stashID, GrimDawnLib.GrimDawnGameExpansion expansion, int tabsCount = -1)
        {
            CreateStashDirectory(stashID);
            Common.TransferFile
                .CreateForExpansion(expansion, tabsCount)
                .WriteToFile(GetStashTransferFile(stashID));
        }

        public bool ImportStashTransferFile(int stashID, string srcFile, out bool fileChanged)
        {
            fileChanged = false;
            if (!File.Exists(srcFile)) return false;
            string destFile = GetStashTransferFile(stashID);
            if (File.Exists(destFile))
            {
                string srcHash = Utils.FileUtils.GetFileHash(srcFile);
                string destHash = Utils.FileUtils.GetFileHash(destFile);
                if (srcHash == null || destHash == null) return false; // file locked
                fileChanged = srcHash != destHash;
                if (fileChanged) BackupStashTransferFile(stashID);
                File.Delete(destFile);
            }
            File.Copy(srcFile, destFile);
            return true;
        }

        public bool ExportStashTransferFile(int stashID, string destFile)
        {
            System.Threading.Thread.Sleep(100);
            string srcFile = GetStashTransferFile(stashID);
            if (!File.Exists(srcFile)) return false;
            string destDir = Path.GetDirectoryName(destFile);
            if (!Directory.Exists(destDir)) Directory.CreateDirectory(destDir);
            File.Copy(srcFile, destFile, true);
            return true;
        }

        public int BackupCleanupStashTransferFile(int stashID)
        {
            int deletedFiles = 0;
            int maxBackups = Global.Configuration.Settings.MaxBackups;
            System.Text.RegularExpressions.Regex re = new System.Text.RegularExpressions.Regex(@"^transfer(\d*)$");
            System.Text.RegularExpressions.Match m;
            int backupIndex;
            foreach (string f in Directory.GetFiles(GetStashDirectory(stashID)))
            {
                m = re.Match(Path.GetFileName(f));
                if (!m.Success) continue;
                if (m.Groups[1].Value == "") continue;
                backupIndex = int.Parse(m.Groups[1].Value);
                if (backupIndex > maxBackups)
                {
                    File.Delete(f);
                    deletedFiles++;
                }
            }
            return deletedFiles;
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
                int cur = (maxBackups <= 0) ? 1 : maxBackups;
                int end = backupIndex;
                for (int i = cur; i <= end; i += 1)
                {
                    File.Delete(GetStashTransferFile(stashID, i));
                    backupIndex -= 1;
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
            if (ImportStashTransferFile(stashID, srcFile + ".tmp", out bool changed))
            {
                File.Delete(srcFile + ".tmp");
                return true;
            }
            File.Move(srcFile + ".tmp", srcFile);
            return false;
        }

    }
}
