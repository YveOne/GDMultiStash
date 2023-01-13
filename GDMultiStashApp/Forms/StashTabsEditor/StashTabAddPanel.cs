using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

using GDMultiStash.Common;
using GDMultiStash.Common.Objects;

namespace GDMultiStash.Forms.StashTabsEditor
{

    internal class StashTabAddPanel : StashTabBasePanel
    {
        public StashTabAddPanel(StashObject stashObject) : base(stashObject)
        {
            var stashInfo = GrimDawnLib.GrimDawn.Stashes.GetStashInfoForExpansion(stashObject.Expansion);
            Image bgImage;
            switch (stashInfo.Width)
            {
                case 8:
                    bgImage = Properties.Resources.caravanWindow8x16add;
                    break;
                default:
                    bgImage = Properties.Resources.caravanWindow10x18add;
                    break;
            }
            BackgroundImage = bgImage;
        }
    }

}
