using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using GDMultiStash.Common.Config;

namespace GDMultiStash.Common.Objects
{
    internal class StashGroupObject : IBaseObject
    {

        private readonly ConfigStashGroup _configStashGroup;


        public StashGroupObject(ConfigStashGroup configStashGroup)
        {
            _configStashGroup = configStashGroup;
        }

        #region properties

        public int ID => _configStashGroup.ID;

        public string Name
        {
            get { return _configStashGroup.Name; }
            set { _configStashGroup.Name = value; }
        }

        public int Order
        {
            get { return _configStashGroup.Order; }
            set { _configStashGroup.Order = value; }
        }

        public string StashesCount
        {
            get
            {
                var expansion = G.Runtime.ShownExpansion;
                var allStashes = G.Stashes.GetStashesForGroup(ID).ToList();
                var curStashes = allStashes.Where(s => s.Expansion == expansion);
                return $"{curStashes.Count()}/{allStashes.Count()}";
            }
        }

        public bool Collapsed
        {
            get { return _configStashGroup.Collapsed; }
            set { _configStashGroup.Collapsed = value; }
        }

        #endregion

    }
}
