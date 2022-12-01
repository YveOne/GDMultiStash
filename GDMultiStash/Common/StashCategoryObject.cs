using System;
using System.IO;
using System.Drawing;

using GDMultiStash.Common.Config;

namespace GDMultiStash.Common
{
    internal class StashCategoryObject
    {

        private readonly ConfigStashCategory _configStashCategory;
        private uint _order;

        public StashCategoryObject(ConfigStashCategory configStashCategory, uint order)
        {
            _configStashCategory = configStashCategory;
            _order = order;
        }

        #region Methods

        public void UpdateOrder()
        {

        }

        #endregion

        #region properties

        public int ID => _configStashCategory.ID;

        public string Name
        {
            get { return _configStashCategory.Name; }
            set { _configStashCategory.Name = value; }
        }

        public uint Order
        {
            get { return _order; }
            set { _order = value; }
        }

        #endregion

    }
}
