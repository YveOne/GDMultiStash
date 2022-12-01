using System;
using System.IO;
using System.Drawing;

using GDMultiStash.Common.Config;

namespace GDMultiStash.Common
{
    internal class StashObject
    {

        private readonly ConfigStash _configStash;
        private TransferFile _transferFile;

        public StashObject(ConfigStash configStash)
        {
            _configStash = configStash;
            _transferFile = new TransferFile();
        }

        #region Methods

        public bool LoadTransferFile()
        {
            string transferFilePath = Global.FileSystem.GetStashTransferFile(_configStash.ID);
            _transferFile = TransferFile.FromFile(transferFilePath, out bool success);
            if (success)
            {
                _transferFile.LoadUsage();
            }
            return success;
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

        #endregion

        #region properties

        public bool TransferFileLoaded => _transferFile != null;

        public int ID => _configStash.ID;

        public string FilePath => Global.FileSystem.GetStashTransferFile(ID);

        public string DirPath => Global.FileSystem.GetStashDirectory(ID);

        public DateTime LastWriteTime => File.GetLastWriteTime(FilePath);

        public float Usage => _transferFile != null ? _transferFile.Usage : 0f;

        public string UsageText => _transferFile != null ? _transferFile.UsageText : "???";

        public GrimDawnLib.GrimDawnGameExpansion Expansion => (GrimDawnLib.GrimDawnGameExpansion)_configStash.Expansion;

        public string Name
        {
            get { return _configStash.Name; }
            set { _configStash.Name = value; }
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

        public string Color
        {
            get { return _configStash.Color.ToLower(); }
            set
            {
                if (value == null) return;
                string col = value.Trim();
                if (!col.StartsWith("#")) col = "#" + col;
                _configStash.Color = col;
            }
        }

        public bool Locked
        {
            get { return _configStash.Locked; }
            set { _configStash.Locked = value; }
        }

        #endregion

    }
}
