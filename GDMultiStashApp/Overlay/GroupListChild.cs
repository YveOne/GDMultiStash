using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using GDMultiStash.Common.Objects;
using GDMultiStash.Common.Overlay;
using GDMultiStash.Overlay.Controls;
using GDMultiStash.Overlay.Controls.Base;

namespace GDMultiStash.Overlay
{
    internal class GroupListChild : SelectableListChild<StashGroupObject>
    {
        public override Color DebugColor => Color.FromArgb(128, 0, 255, 255);

        public GroupListChild() : base(StaticResources.GroupListItemFontHandler)
        {
            MouseClick += delegate { Global.Ingame.ActiveGroupID = Model.ID; };
        }

    }
}
