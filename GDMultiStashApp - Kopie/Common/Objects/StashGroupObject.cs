using System;
using System.IO;
using System.Drawing;

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

        #region Methods

        public void UpdateOrder()
        {

        }

        #endregion

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

        public bool Collapsed
        {
            get { return _configStashGroup.Collapsed; }
            set { _configStashGroup.Collapsed = value; }
        }

        #endregion

    }
}
