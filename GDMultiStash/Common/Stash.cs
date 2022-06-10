using System;
using System.IO;
using System.Drawing;

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

        public string Color
        {
            get { return _configStash.Color.ToLower(); }
            set {
                string col = value.Trim();
                if (!col.StartsWith("#")) col = "#" + col;
                _configStash.Color = col;
            }
        }

        public Color GetDisplayColor()
        {
            try
            {
                return ColorTranslator.FromHtml(_configStash.Color);
            }
            catch (Exception)
            {
                return System.Drawing.Color.FromArgb(255, 235, 222, 195);
            }
        }

        public Font GetDisplayFont()
        {
            return Utils.FontLoader.GetFont("Linux Biolinum", 22, FontStyle.Regular);
        }

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
