using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using GrimDawnLib;

namespace GDMultiStash
{
    internal static partial class Core
    {
        internal static partial class Files
        {

            public static readonly string DataDirPath = Path.Combine(Application.StartupPath, "Data");
            public static readonly string DataStashesDirPath = Path.Combine(DataDirPath, "Stashes");
            public static readonly string DataLocalesDirPath = Path.Combine(DataDirPath, "Locales");
            public static readonly string DataConfigFilePath = Path.Combine(DataDirPath, "Config.xml");

            public static void EnsureDirectoriesExist()
            {
                if (!Directory.Exists(DataDirPath)) Directory.CreateDirectory(DataDirPath);
                if (!Directory.Exists(DataStashesDirPath)) Directory.CreateDirectory(DataStashesDirPath);
                if (!Directory.Exists(DataLocalesDirPath)) Directory.CreateDirectory(DataLocalesDirPath);
            }

            public static string GetStashDir(string stashID)
            {
                return Path.Combine(DataStashesDirPath, stashID);
            }

            public static string GetStashDir(int stashID)
            {
                return GetStashDir(stashID.ToString());
            }

            public static string GetStashFilePath(int stashID, int backupIndex = -1)
            {
                return Path.Combine(DataStashesDirPath, stashID.ToString(), "transfer" + (backupIndex >= 0 ? backupIndex.ToString() : ""));
            }

            public static void CreateStashDir(int stashID)
            {
                string stashDir = GetStashDir(stashID);
                if (!Directory.Exists(stashDir)) Directory.CreateDirectory(stashDir);
            }

            public static void DeleteStashDir(int stashID)
            {
                string stashDir = GetStashDir(stashID);
                if (Directory.Exists(stashDir)) Directory.Delete(stashDir, true);
            }

            public static void CreateTransferFile(int stashID, GrimDawnGameExpansion expansion)
            {
                Common.TransferFile
                    .CreateForExpansion(expansion)
                    .WriteToFile(GetStashFilePath(stashID));
            }

            public static bool ImportTransferFile(int stashID, string srcFile, bool forceBackup = false)
            {
                if (!File.Exists(srcFile)) return false; // what happened???

                string destFile = GetStashFilePath(stashID);
                string srcHash = Utils.FileUtils.GetFileHash(srcFile);
                string destHash = Utils.FileUtils.GetFileHash(destFile);
                if (srcHash == null || destHash == null) return false; // file locked

                if (srcHash != destHash || forceBackup) CreateBackup(stashID);
                if (File.Exists(destFile)) File.Delete(destFile);
                File.Copy(srcFile, destFile);
                return true;
            }

            public static bool ExportTransferFile(int stashID, string dstFile)
            {
                string srcFile = GetStashFilePath(stashID);
                if (!File.Exists(srcFile)) return false;

                string dstDir = Path.GetDirectoryName(dstFile);
                if (!Directory.Exists(dstDir)) Directory.CreateDirectory(dstDir);

                File.Copy(srcFile, dstFile, true);
                return true;
            }

            public static void CreateBackup(int stashID)
            {
                if (!File.Exists(GetStashFilePath(stashID))) return;

                int backupIndex = 0;
                while (File.Exists(GetStashFilePath(stashID, backupIndex + 1)))
                {
                    backupIndex += 1;
                }
                if (backupIndex > 0)
                {
                    if (Config.MaxBackups >= 0)
                    {
                        for (int i = (Config.MaxBackups == 0) ? 1 : Config.MaxBackups; i <= backupIndex; i += 1)
                        {
                            File.Delete(GetStashFilePath(stashID, i));
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
                    File.Move(GetStashFilePath(stashID, backupIndex), GetStashFilePath(stashID, backupIndex + 1));
                    backupIndex -= 1;
                }
                File.Move(GetStashFilePath(stashID), GetStashFilePath(stashID, 1));
            }



        }
    }
}
