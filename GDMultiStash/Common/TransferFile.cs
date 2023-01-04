using System;
using System.Collections.Generic;
using System.IO;

using GrimDawnLib;

namespace GDMultiStash.Common
{
	internal class TransferFile
	{

		private struct StashDataDefaults
		{
			public bool IsExpansion1;
			public uint Width;
			public uint Height;
			public uint MaxTabs;
		}

		private static readonly Dictionary<GrimDawnGameExpansion, StashDataDefaults> DefaultValues = new Dictionary<GrimDawnGameExpansion, StashDataDefaults> {
			{ GrimDawnGameExpansion.BaseGame, new StashDataDefaults {
				IsExpansion1 = false,
				Width = 8,
				Height = 16,
				MaxTabs = 4,
			} },
			{ GrimDawnGameExpansion.AshesOfMalmouth, new StashDataDefaults {
				IsExpansion1 = true,
				Width = 10,
				Height = 18,
				MaxTabs = 5,
			} },
			{ GrimDawnGameExpansion.ForgottenGods, new StashDataDefaults {
				IsExpansion1 = false,
				Width = 10,
				Height = 18,
				MaxTabs = 6,
			} },
		};

		public static uint GetMaxTabsForExpansion(GrimDawnGameExpansion exp)
        {
			return DefaultValues[exp].MaxTabs;

		}

		private readonly GDIALib.Parser.Stash.Stash stash;
		private float _usageTotal = 0f;
		private List<float> _usagePages = new List<float>();

		public TransferFile()
		{
			stash = new GDIALib.Parser.Stash.Stash();
		}

		public TransferFile(GDIALib.Parser.Stash.Stash stash)
		{
			this.stash = stash;
		}

		public List<GDIALib.Parser.Stash.StashTab> Tabs => stash.Tabs;

		public float TotalUsage => _usageTotal;

		public string TotalUsageText => ((int)(_usageTotal * 100)) + "%";

		public IList<float> TabsUsage => _usagePages.AsReadOnly();

		public uint MaxTabsCount => DefaultValues[Expansion].MaxTabs;

		public GrimDawnGameExpansion Expansion { get; private set; } = GrimDawnGameExpansion.Unknown;

		public static TransferFile CreateForExpansion(GrimDawnGameExpansion exp, int tabsCount = -1)
		{
			StashDataDefaults def = DefaultValues[exp];
			GDIALib.Parser.Stash.Stash stash = new GDIALib.Parser.Stash.Stash()
			{
				IsExpansion1 = def.IsExpansion1,
			};
			if (tabsCount <= 0 || tabsCount > def.MaxTabs) tabsCount = (int)def.MaxTabs;
			for (int i = 1; i <= tabsCount; i += 1)
			{
				stash.Tabs.Add(new GDIALib.Parser.Stash.StashTab()
				{
					Width = def.Width,
					Height = def.Height,
				});
			}
			return new TransferFile(stash);
		}

		public static bool FromFile(string filePath, out TransferFile transferFile)
		{
			transferFile = null;
			if (!File.Exists(filePath)) return false;
			transferFile = new TransferFile();
			return transferFile.ReadFromFile(filePath);
		}

		public void WriteToFile(string destPath)
		{
			GDIALib.Parser.Stash.DataBuffer buf = new GDIALib.Parser.Stash.DataBuffer();
			stash.Write(buf);
			if (File.Exists(destPath)) File.Delete(destPath);
			File.WriteAllBytes(destPath, buf.Data);
		}

		public bool ReadFromFile(string srcPath)
		{
			if (!File.Exists(srcPath)) return false;
			GDIALib.Parser.Stash.GDCryptoDataBuffer crypto = new GDIALib.Parser.Stash.GDCryptoDataBuffer(File.ReadAllBytes(srcPath));
			bool success = stash.Read(crypto);
			if (success)
            {
				LoadUsage();
				if (stash.Width == 8) Expansion = GrimDawnGameExpansion.BaseGame;
				else if (stash.IsExpansion1) Expansion = GrimDawnGameExpansion.AshesOfMalmouth;
				else Expansion = GrimDawnGameExpansion.ForgottenGods;
			}
			return success;
		}

		public void LoadUsage()
        {
			_usageTotal = 0f;
			long spacePerTab = stash.Width * stash.Height;
			long spacePerStash = stash.Tabs.Count * spacePerTab;
			long usedTotal = 0;
			_usagePages.Clear();
			foreach (GDIALib.Parser.Stash.StashTab tab in stash.Tabs)
			{
				int usedPage = 0;
				foreach (GDIALib.Parser.Stash.Item item in tab.Items)
					usedPage += Global.Database.GetItemSize(item.BaseRecord);
				usedTotal += usedPage;
				_usagePages.Add((float)usedPage / (float)spacePerTab);
			}
			_usageTotal = (float)usedTotal / (float)spacePerStash;
		}

		public static bool ValidateFile(string filePath, out TransferFile f)
		{
			f = null;
			if (!File.Exists(filePath)) return false;
			f = new TransferFile();
			return f.ReadFromFile(filePath);
		}

		public static bool ValidateFile(string filePath)
		{
            return ValidateFile(filePath, out _);
		}

		public static GrimDawnGameExpansion GetExpansionByFile(string filePath)
		{
			if (!ValidateFile(filePath, out TransferFile f)) return GrimDawnGameExpansion.Unknown;
			return f.Expansion;
		}

	}

}
