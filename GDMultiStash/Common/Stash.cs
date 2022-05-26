using System;
using System.IO;

namespace GDMultiStash.Common
{
    internal class Stash
    {

        private readonly Config.ConfigStash _configStash;
        private TransferFile _transferFile;

        public Stash(Config.ConfigStash ConfigStash)
        {
            _configStash = ConfigStash;
            _transferFile = new TransferFile();
        }

        public bool LoadTransferFile()
        {
            string stashFile = Core.Files.GetStashFilePath(_configStash.ID);
            _transferFile = TransferFile.FromFile(stashFile, out bool success);
            return success;
        }

        public bool TransferFileExists()
        {
            return _transferFile != null;
        }

        public int ID { get { return _configStash.ID; } }

        public string FilePath { get { return Core.Files.GetStashFilePath(ID); } }

        public string DirPath { get { return Core.Files.GetStashDir(ID); } }

        public DateTime LastWriteTime { get { return File.GetLastWriteTime(FilePath); } }

        public float Usage { get { return _transferFile.Usage; } }

        public string UsageText { get { return _transferFile.UsageText; } }

        public string Name
        {
            get { return _configStash.Name; }
            set { _configStash.Name = value; }
        }

        public GrimDawnLib.GrimDawnGameExpansion Expansion
        {
            get { return (GrimDawnLib.GrimDawnGameExpansion)_configStash.Expansion; }
        }

        public bool SC
        {
            get { return _configStash.SC; }
            set { _configStash.SC = value; }
        }

        public bool HC
        {
            get { return _configStash.HC; }
            set { _configStash.HC = value; }
        }

        public int Order
        {
            get { return _configStash.Order; }
            set { _configStash.Order = value; }
        }














    }
}
