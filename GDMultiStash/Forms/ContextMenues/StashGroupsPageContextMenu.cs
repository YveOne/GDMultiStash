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
using System.ComponentModel;

namespace GDMultiStash.Forms.ContextMenues
{

    [DesignerCategory("code")]
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
            Items.Add(X(G.L.DeleteButton()), null, delegate (object s, EventArgs e) {
                if (G.Configuration.Settings.ConfirmStashDelete && MessageBox.Show(G.L.ConfirmDeleteStashGroupsMessage(), "", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK) return;

                List<StashGroupObject> deletedItems = G.StashGroups.DeleteGroups(selectedGroups);
                G.Configuration.Save();
                G.StashGroups.InvokeStashGroupsRemoved(deletedItems);
                //Global.Ingame.InvokeStashesRebuild();
            });
        }

    }
}
