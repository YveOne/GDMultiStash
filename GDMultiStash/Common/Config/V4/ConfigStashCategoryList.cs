﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMultiStash.Common.Config
{

    [Serializable]
    public class ConfigStashCategoryList : List<ConfigStashCategory>
    {

        public ConfigStashCategoryList() : base()
        {
        }

    }
}
