using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;

using GrimDawnLib;

namespace GDMultiStash.Common
{
    internal class ExportZipFile
    {

        private class Entry
        {
            private string _source;
            private string _name;
            public string Source => _source;
            public string Name => _name;
            public Entry(string srcFile, string extryName)
            {
                _source = srcFile;
                _name = extryName;
            }
        }

        private List<Entry> _entries;

        public ExportZipFile()
        {
            _entries = new List<Entry>();
        }

        public void AddFile(string srcFile, string extryName)
        {
            _entries.Add(new Entry(srcFile, extryName));
        }

        public void AddStash(GlobalHandlers.StashObject stash)
        {
            string transferName = string.Format("{0} - {1}", stash.ID, stash.Name);
            transferName = string.Join("_", transferName.Split(Path.GetInvalidFileNameChars()));

            string transferExt;
            string transferFile;
            if (!stash.SC && !stash.HC)
            {
                transferExt = GrimDawn.GetTransferExtension(stash.Expansion, GrimDawnGameMode.None);
                transferFile = string.Format("{0} [no mode]{1}", transferName, transferExt);
                AddFile(stash.FilePath, transferFile);
            }
            else
            {
                if (stash.SC)
                {
                    transferExt = GrimDawn.GetTransferExtension(stash.Expansion, GrimDawnGameMode.SC);
                    transferFile = string.Format("{0} [softcore mode]{1}", transferName, transferExt);
                    AddFile(stash.FilePath, transferFile);
                }
                if (stash.HC)
                {
                    transferExt = GrimDawn.GetTransferExtension(stash.Expansion, GrimDawnGameMode.HC);
                    transferFile = string.Format("{0} [hardcore mode]{1}", transferName, transferExt);
                    AddFile(stash.FilePath, transferFile);
                }
            }
        }

        private string _dstFile;
        public string DstFile => _dstFile;

        public void SaveTo(string dstFile)
        {
            _dstFile = dstFile;
            using (FileStream zipToOpen = new FileStream(dstFile, FileMode.Create))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                {
                    foreach (Entry entry in _entries)
                    {
                        ZipArchiveEntry readmeEntry = archive.CreateEntry(entry.Name);
                        using (StreamWriter writer = new StreamWriter(readmeEntry.Open()))
                        {
                            byte[] buffer = File.ReadAllBytes(entry.Source);
                            writer.BaseStream.Write(buffer, 0, buffer.Length);
                        }
                    }
                }
            }
        }

    }
}
