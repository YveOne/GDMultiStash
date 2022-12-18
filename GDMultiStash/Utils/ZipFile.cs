using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;

namespace Utils
{
    internal class ZipFile
    {

        private List<Entry> _entries;

        private abstract class Entry
        {
            public string Name { get; private set; }
            public Entry(string extryName)
            { 
                Name = extryName;
            }
            public abstract byte[] GetContent();
        }

        private class FileEntry : Entry
        {
            private string _path;
            public FileEntry(string extryName, string path) : base(extryName)
            {
                _path = path;
            }
            public override byte[] GetContent()
            {
                return File.ReadAllBytes(_path);
            }
        }

        private class StringEntry : Entry
        {
            private string _text;
            public StringEntry(string extryName, string text) : base(extryName)
            {
                _text = text;
            }
            public override byte[] GetContent()
            {
                return Encoding.UTF8.GetBytes(_text);
            }
        }





        public ZipFile()
        {
            _entries = new List<Entry>();
        }

        public void AddFile(string extryName, string path)
        {
            _entries.Add(new FileEntry(extryName, path));
        }

        public void AddString(string extryName, string text)
        {
            _entries.Add(new StringEntry(extryName, text));
        }

        public void SaveTo(string dstFile)
        {
            using (FileStream zipToOpen = new FileStream(dstFile, FileMode.Create))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                {
                    foreach (Entry entry in _entries)
                    {
                        ZipArchiveEntry readmeEntry = archive.CreateEntry(entry.Name);
                        using (StreamWriter writer = new StreamWriter(readmeEntry.Open()))
                        {
                            byte[] buffer = entry.GetContent();
                            writer.BaseStream.Write(buffer, 0, buffer.Length);
                        }
                    }
                }
            }
        }
        

    }
}
