using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

using GDMultiStash.Common;
using GDMultiStash.Common.Objects;

using BrightIdeasSoftware;

namespace GDMultiStash.Forms.Main
{

    internal class StashGroupsPageContextMenu : ContextMenuStrip
    {
        private readonly StashGroupsPage page;
        private readonly int rowIndex;
        private readonly StashGroupObject clickedGroup;
        private readonly IEnumerable<StashGroupObject> selectedGroups;
        private readonly int selectedGroupsCount;

        private bool LastWasSeparator => Items.Count == 0 ? false : Items[Items.Count - 1].GetType() == typeof(ToolStripSeparator);

        public StashGroupsPageContextMenu(StashGroupsPage page, CellRightClickEventArgs args)
        {
            this.page = page;
            this.rowIndex = args.RowIndex;
            this.clickedGroup = (StashGroupObject)args.Model;
            this.selectedGroups = page.GetSelectedObjects();
            this.selectedGroupsCount = selectedGroups.Count();

            AddComment(selectedGroupsCount == 1
                ? $"#{clickedGroup.ID} {clickedGroup.Name}"
                : $"({selectedGroupsCount})");
            AddSeparator();
        }

        private string T(string s)
        {
            // used to escape & sign for toolstrip item text
            return s.Replace("&", "&&");
        }

        public void AddComment(string text)
        {
            Items.Add(new ToolStripLabel(T(text))
            { ForeColor = Color.Gray });
        }

        public void AddSeparator()
        {
            if (LastWasSeparator) return;
            Items.Add(new ToolStripSeparator());
        }

        public void AddDeleteOption()
        {
            Items.Add(T(Global.L.DeleteButton()), null, delegate (object s, EventArgs e) {
                if (Global.Configuration.Settings.ConfirmStashDelete && MessageBox.Show(Global.L.ConfirmDeleteStashGroupsMessage(), "", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK) return;

                List<StashGroupObject> deletedItems = Global.Stashes.DeleteStashGroups(selectedGroups);
                Global.Configuration.Save();
                Global.Runtime.NotifyStashGroupsRemoved(deletedItems);
                Global.Runtime.NotifyStashesRebuild();
            });
        }

    }
}
