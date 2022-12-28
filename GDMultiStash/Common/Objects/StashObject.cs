using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

using GDMultiStash.Common.Config;

namespace GDMultiStash.Common.Objects
{
    internal class StashDummyObject : StashObject
    {
        public StashDummyObject(ConfigStash cfgStash, int grpId) : base(new ConfigStash())
        {
            this.GroupID = grpId;
        }
        public new string ID => "";
        public new string UsageText => "";
    }

    internal class StashObject : IBaseObject
    {

        private readonly ConfigStash _configStash;
        private TransferFile _transferFile;
        private Bitmap _usageIndicator = null;

        public StashObject(ConfigStash configStash)
        {
            _configStash = configStash;
            _transferFile = new TransferFile();
        }

        #region Methods

        public void SetTransferFile(TransferFile transfer)
        {
            _transferFile = transfer;
            _transferFile.LoadUsage();
        }

        public bool LoadTransferFile()
        {
            string transferFilePath = Global.FileSystem.GetStashTransferFile(_configStash.ID);
            _transferFile = TransferFile.FromFile(transferFilePath, out bool success);
            if (success)
            {
                _transferFile.LoadUsage();

                int rZero = 0;
                int gZero = 255;
                int bZero = 0;

                int rMin = 0;
                int gMin = 255;
                int bMin = 0;

                int rCen = 100;
                int gCen = 100;
                int bCen = 0;

                int rMax = 100;
                int gMax = 0;
                int bMax = 0;

                int rFull = 100; // 0
                int gFull = 0;
                int bFull = 0;

                float posMin = 0.1f;
                float posCen = 0.5f;
                float posMax = 0.9f;

                _usageIndicator = new Bitmap(6 * 5 + 5 * 2, 10, PixelFormat.Format32bppArgb);
                using (Graphics g = Graphics.FromImage(_usageIndicator))
                {
                    for (int i = 0; i < 6; i += 1)
                    {
                        float p = _transferFile.TabsUsage[i];
                        Color c;
                        if (p == 0)
                        {
                            c = Color.FromArgb(rZero, gZero, bZero);
                        }
                        else if (p <= posMin)
                        {
                            c = Color.FromArgb(rMin, gMin, bMin);
                        }
                        else if (p <= posCen)
                        {
                            p = (p - posMin) / (posCen - posMin);
                            c = Color.FromArgb(
                                (int)((rCen - rMin) * p) + rMin,
                                (int)((gCen - gMin) * p) + gMin,
                                (int)((bCen - bMin) * p) + bMin);
                        }
                        else if (p <= posMax)
                        {
                            p = (p - posCen) / (posMax - posCen);
                            c = Color.FromArgb(
                                (int)((rMax - rCen) * p) + rCen,
                                (int)((gMax - gCen) * p) + gCen,
                                (int)((bMax - bCen) * p) + bCen);
                        }
                        else
                        {
                            p = (p - posMax) / (1 - posMax);
                            c = Color.FromArgb(
                                (int)((rFull - rMax) * p) + rMax,
                                (int)((gFull - gMax) * p) + gMax,
                                (int)((bFull - bMax) * p) + bMax);
                        }
                        using (var brush = new SolidBrush(c))
                        {
                            g.FillRectangle(brush, i * (5 + 2), 0, 5, _usageIndicator.Height);
                        }
                    }
                }
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

        public float Usage => _transferFile != null ? _transferFile.TotalUsage : 0f;

        public string UsageText => _transferFile != null ? _transferFile.TotalUsageText : "???";

        public Image UsageIndicator => _usageIndicator;

        public GrimDawnLib.GrimDawnGameExpansion Expansion
        {
            get => (GrimDawnLib.GrimDawnGameExpansion)_configStash.Expansion;
            set { _configStash.Expansion = (int)value; }
        }

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

        public string TextColor
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

        public int GroupID
        {
            get { return _configStash.GroupID; }
            set { _configStash.GroupID = value; }
        }

        #endregion

    }
}