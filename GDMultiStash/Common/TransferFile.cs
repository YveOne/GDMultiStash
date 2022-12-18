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
				MaxTabs = 1,
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

		private readonly GDIALib.Parser.Stash.Stash stash;
		private float _usageTotal = 0f;
		private List<float> _usagePages = new List<float>() { 0,0,0,0,0,0 };

		public TransferFile()
		{
			stash = new GDIALib.Parser.Stash.Stash();
		}

		public TransferFile(GDIALib.Parser.Stash.Stash stash)
		{
			this.stash = stash;
		}

		public float TotalUsage
		{
			get { return _usageTotal; }
		}

		public string TotalUsageText
		{
			get { return ((int)(_usageTotal * 100)) + "%"; }
		}

		public IList<float> TabsUsage
		{
			get => _usagePages.AsReadOnly();
		}

		public bool IsEmpty
		{
			get
			{
				foreach (GDIALib.Parser.Stash.StashTab tab in stash.Tabs)
				{
					if (tab.Items.Count != 0) return false;
				}
				return true;
			}
		}

		public static TransferFile CreateForExpansion(GrimDawnGameExpansion exp)
		{
			StashDataDefaults def = DefaultValues[exp];
			GDIALib.Parser.Stash.Stash stash = new GDIALib.Parser.Stash.Stash()
			{
				IsExpansion1 = def.IsExpansion1,
			};
			for (int i = 1; i <= def.MaxTabs; i += 1)
			{
				stash.Tabs.Add(new GDIALib.Parser.Stash.StashTab()
				{
					Width = def.Width,
					Height = def.Height,
				});
			}
			return new TransferFile(stash);
		}

		public static TransferFile FromFile(string filePath, out bool success)
		{
			success = false;
			if (!File.Exists(filePath)) return null;
			TransferFile stash = new TransferFile();
			success = stash.ReadFromFile(filePath);
			return stash;
		}

		public static TransferFile FromFile(string filePath)
		{
            return FromFile(filePath, out _);
		}

		public void WriteToFile(string destPath)
		{
			GDIALib.Parser.Stash.DataBuffer buf = new GDIALib.Parser.Stash.DataBuffer();
			stash.Write(buf);
			File.WriteAllBytes(destPath, buf.Data);
		}

		public bool ReadFromFile(string srcPath)
		{
			if (!File.Exists(srcPath)) return false;
			GDIALib.Parser.Stash.GDCryptoDataBuffer crypto = new GDIALib.Parser.Stash.GDCryptoDataBuffer(File.ReadAllBytes(srcPath));
			bool success = stash.Read(crypto);
			return success;
		}

		public void LoadUsage()
        {
			_usageTotal = 0f;
			long spacePerTab = stash.Width * stash.Height;
			long spacePerStash = stash.Tabs.Count * spacePerTab;
			long usedTotal = 0;
			int index = 0;
			foreach (GDIALib.Parser.Stash.StashTab tab in stash.Tabs)
			{
				int usedPage = 0;
				foreach (GDIALib.Parser.Stash.Item item in tab.Items)
					usedPage += Global.Database.GetItemSize(item.BaseRecord);
				usedTotal += usedPage;
				_usagePages[index] = (float)usedPage / (float)spacePerTab;
				index += 1;
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

		public static GrimDawnGameExpansion GetExpansionByFile(string transferFile)
		{
			if (!File.Exists(transferFile)) return GrimDawnGameExpansion.Unknown;

			GDIALib.Parser.Stash.GDCryptoDataBuffer crypto = new GDIALib.Parser.Stash.GDCryptoDataBuffer(File.ReadAllBytes(transferFile));
			GDIALib.Parser.Stash.Stash stashData = new GDIALib.Parser.Stash.Stash();
			if (!stashData.Read(crypto)) return GrimDawnGameExpansion.Unknown;

			if (stashData.Width == 8) return GrimDawnGameExpansion.BaseGame;
			if (stashData.IsExpansion1) return GrimDawnGameExpansion.AshesOfMalmouth;
			return GrimDawnGameExpansion.ForgottenGods;
		}

	}

}
