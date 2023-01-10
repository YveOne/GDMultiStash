﻿using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
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
            LoadTransferFile();
        }

        public bool LoadTransferFile()
        {
            if (TransferFile.FromFile(FilePath, out _transferFile))
            {
                CreateUsageIndicator();
                return true;
            }
            return false;
        }

        public void SaveTransferFile()
        {
            _transferFile.WriteToFile(FilePath);
        }

        private void CreateUsageIndicator()
        {
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

            //int barWidth = (int)(42f / _transferFile.TabsUsage.Count + 0.5f);
            int maxTabs = (int)GrimDawnLib.GrimDawn.Stashes.GetStashInfoForExpansion(Expansion).MaxTabs;
            int barWidth = (int)(42f / maxTabs + 0.5f);

            _usageIndicator = new Bitmap(6 * 5 + 5 * 2, 10, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(_usageIndicator))
            {
                for (int i = 0; i < _transferFile.TabsUsage.Count; i += 1)
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
                            Math.Max(0, Math.Min(255, (int)((rCen - rMin) * p) + rMin)),
                            Math.Max(0, Math.Min(255, (int)((gCen - gMin) * p) + gMin)),
                            Math.Max(0, Math.Min(255, (int)((bCen - bMin) * p) + bMin))
                        );
                    }
                    else if (p <= posMax)
                    {
                        p = (p - posCen) / (posMax - posCen);
                        c = Color.FromArgb(
                            Math.Max(0, Math.Min(255, (int)((rMax - rCen) * p) + rCen)),
                            Math.Max(0, Math.Min(255, (int)((gMax - gCen) * p) + gCen)),
                            Math.Max(0, Math.Min(255, (int)((bMax - bCen) * p) + bCen))
                        );
                    }
                    else
                    {
                        p = (p - posMax) / (1 - posMax);
                        c = Color.FromArgb(
                            Math.Max(0, Math.Min(255, (int)((rFull - rMax) * p) + rMax)),
                            Math.Max(0, Math.Min(255, (int)((gFull - gMax) * p) + gMax)),
                            Math.Max(0, Math.Min(255, (int)((bFull - bMax) * p) + bMax))
                        );
                    }
                    using (var brush = new SolidBrush(c))
                    {
                        g.FillRectangle(brush, i * barWidth, 0, barWidth - 2, _usageIndicator.Height);
                    }
                }
            }
        }

        public GDIALib.Parser.Stash.StashTab RemoveTabAt(int index)
        {
            GDIALib.Parser.Stash.StashTab tab = _transferFile.Tabs[index];
            _transferFile.Tabs.RemoveAt(index);
            return tab;
        }

        public bool RemoveTab(GDIALib.Parser.Stash.StashTab tab)
        {
            if (!_transferFile.Tabs.Contains(tab)) return false;
            _transferFile.Tabs.Remove(tab);
            return true;
        }

        public void AddTab(GDIALib.Parser.Stash.StashTab tab, int atIndex)
        {
            if (tab == null) return;
            _transferFile.Tabs.Insert(atIndex, tab);
            CreateUsageIndicator();
        }

        public void AddTab(GDIALib.Parser.Stash.StashTab tab)
        {
            AddTab(tab, _transferFile.Tabs.Count);
        }

        public void AddTab()
        {
            var stashInfo = GrimDawnLib.GrimDawn.Stashes.GetStashInfoForExpansion(Expansion);
            AddTab(new GDIALib.Parser.Stash.StashTab()
            {
                Width = stashInfo.Width,
                Height = stashInfo.Height,
            });
        }

        public void AutoFill()
        {
            var stashWidth = _transferFile.Width;
            var stashHeight = _transferFile.Height;
            foreach (var tab in _transferFile.Tabs)
            {
                if (tab.Items.Count != 1) continue;
                var item = tab.Items[0];

                if (!Global.Database.GetItemRecordInfo(item.BaseRecord, out GlobalHandlers.DatabaseHandler.ItemRecordInfo itemInfo))
                    continue;

                var xCount = (int)(stashWidth / itemInfo.Width);
                var yCount = (int)(stashHeight / itemInfo.Height);

                tab.Items.Clear();
                for (uint x = 0; x < xCount; x += 1)
                    for (uint y = 0; y < yCount; y += 1)
                    {
                        var copy = item.DeepClone();
                        copy.RandomizeSeed();
                        copy.RandomizeRelicSeed();
                        copy.X = x * itemInfo.Width;
                        copy.Y = y * itemInfo.Height;
                        tab.Items.Add(copy);
                    }
            }
        }

        #endregion

        #region properties

        public Color DisplayColor {
            get
            {
                try
                {
                    return ColorTranslator.FromHtml(_configStash.Color);
                }
                catch (Exception)
                {
                    return Color.FromArgb(255, 235, 222, 195);
                }
            }
        }

        public Font DisplayFont => Utils.FontLoader.GetFont("Linux Biolinum", 22, FontStyle.Regular);

        public IList<GDIALib.Parser.Stash.StashTab> Tabs => _transferFile.Tabs.AsReadOnly();

        public uint MaxTabsCount => _transferFile.MaxTabsCount;

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

        public uint Width => _transferFile.Width;

        public uint Height => _transferFile.Height;

        #endregion

    }
}