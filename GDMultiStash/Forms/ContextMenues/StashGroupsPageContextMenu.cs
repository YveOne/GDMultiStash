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

namespace GDMultiStash.Forms.ContextMenues
{

    internal class StashGroupsPageContextMenu : BaseContextMenu
    {
        private readonly MainWindow.StashGroupsPage page;
        private readonly int rowIndex;
        private readonly StashGroupObject clickedGroup;
        private readonly IEnumerable<StashGroupObject> selectedGroups;
        private readonly int selectedGroupsCount;

        public StashGroupsPageContextMenu(MainWindow.StashGroupsPage page, CellRightClickEventArgs args)
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

        public void AddDeleteOption()
        {
            Items.Add(X(Global.L.DeleteButton()), null, delegate (object s, EventArgs e) {
                if (Global.Configuration.Settings.ConfirmStashDelete && MessageBox.Show(Global.L.ConfirmDeleteStashGroupsMessage(), "", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK) return;

                List<StashGroupObject> deletedItems = Global.Groups.DeleteGroups(selectedGroups);
                Global.Configuration.Save();
                Global.Runtime.InvokeStashGroupsRemoved(deletedItems);
                //Global.Ingame.InvokeStashesRebuild();
            });
        }

    }
}
