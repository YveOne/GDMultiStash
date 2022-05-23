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
		private float _usage = 0f;

		public TransferFile()
		{
			stash = new GDIALib.Parser.Stash.Stash();
		}

		public TransferFile(GDIALib.Parser.Stash.Stash stash)
		{
			this.stash = stash;
		}

		public float Usage
		{
			get { return _usage; }
		}

		public string UsageText
		{
			get { return ((int)(_usage * 100)) + "%"; }
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
			if (success)
			{
				_usage = 0f;
				long space = stash.Tabs.Count * stash.Width * stash.Height;
				long used = 0;
				foreach (GDIALib.Parser.Stash.StashTab tab in stash.Tabs)
				{
					foreach (GDIALib.Parser.Stash.Item item in tab.Items)
					{
						used += Core.GD.GetItemSize(item.BaseRecord);
					}
				}
				_usage = (float)used / (float)space;
			}
			return success;
		}

		public static bool ValidateFile(string filePath, out TransferFile stash)
		{
			stash = null;
			if (!File.Exists(filePath)) return false;
			stash = new TransferFile();
			return stash.ReadFromFile(filePath);
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
