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
    internal class StashTabPanel : StashTabBasePanel
    {
        public GDIALib.Parser.Stash.StashTab StashTab { get; private set; }
        public StashObject StashObject { get; private set; }
        public int Index { get; private set; }

        public StashTabPanel(StashObject stashObject, GDIALib.Parser.Stash.StashTab tab) : base(stashObject)
        {
            this.StashTab = tab;
            this.StashObject = stashObject;
            this.BackgroundImage = Global.Database.CreateTabImage(tab, stashObject.Expansion);
        }

        public void AppendTo(Control parent, int index, bool ignoreIndex = false)
        {
            if (parent == null) return;
            if (!parent.Controls.Contains(this))
            {
                if (Parent == null || Parent != parent)
                {
                    if (Parent != null)
                        Parent.Controls.Remove(this);
                    parent.Controls.Add(this);
                }
            }
            index = Math.Min(index, parent.Controls.Count - 2);
            parent.Controls.SetChildIndex(this, index);
            if (!ignoreIndex) Index = index;
        }

        public void Remove()
        {
            if (Parent == null) return;
            Parent.Controls.Remove(this);
        }

    }
}
