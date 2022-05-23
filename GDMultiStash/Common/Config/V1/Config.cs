using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using GrimDawnLib;

namespace GDMultiStash.Common.Config.V1
{

    [Serializable]
    [XmlRoot("Config", IsNullable = false)]
    public partial class Config : ConfigBase
    {

        [XmlArray("Stashes")]
        [XmlArrayItem("Stash")]
        public ConfigStashList Stashes;

        [XmlElement("Settings")]
        public ConfigSettingList Settings;

        public Config(bool IsNew) : base(IsNew)
        {
            Stashes = new ConfigStashList();
            Settings = new ConfigSettingList();
        }

        public Config() : base()
        {
            Stashes = new ConfigStashList();
            Settings = new ConfigSettingList();
        }

        public ConfigSettingList GetSettings()
        {
            return Settings.Copy();
        }

        public void SetSettings(ConfigSettingList settings)
        {
            Settings.Set(settings);
        }

        public ConfigStash[] GetStashes()
        {
            return Stashes.ToArray();
        }

        public ConfigStash CreateStash(string name, GrimDawnGameMode mode = GrimDawnGameMode.None)
        {
            Settings.LastID += 1;
            ConfigStash stash = new ConfigStash
            {
                Name = name,
                ID = Settings.LastID,
                Order = Settings.LastID,
                SC = mode.HasFlag(GrimDawnGameMode.SC),
                HC = mode.HasFlag(GrimDawnGameMode.HC),
            };
            Stashes.Add(stash);
            return stash;
        }

        public ConfigStash GetStashByID(int stashID)
        {
            return Stashes.Find(s => { return s.ID == stashID; });
        }

        public int GetStashIndex(int stashID)
        {
            return Stashes.FindIndex((stash) => { return stash.ID == stashID; });
        }

        public void DeleteStashAt(int index)
        {
            Stashes.RemoveAt(index);
        }

    }

}
