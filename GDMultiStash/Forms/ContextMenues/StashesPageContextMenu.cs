using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

using GDMultiStash.Common;
using GDMultiStash.Common.Objects;
using GDMultiStash.Common.Objects.Sorting.Handler;

using BrightIdeasSoftware;
using System.ComponentModel;

namespace GDMultiStash.Forms.ContextMenues
{

    [DesignerCategory("code")]
    internal class StashesPageContextMenu : BaseContextMenu
    {
        private readonly MainWindow.StashesPage page;
        private readonly int rowIndex;
        private readonly StashObject clickedStash;
        private readonly IEnumerable<StashObject> selectedStashes;
        private readonly int selectedStashesCount;

        public StashesPageContextMenu(MainWindow.StashesPage page, CellRightClickEventArgs args)
        {
            this.page = page;
            this.rowIndex = args.RowIndex;
            this.clickedStash = (StashObject)args.Model;
            this.selectedStashes = page.GetSelectedObjects();
            this.selectedStashesCount = selectedStashes.Count();

            AddComment(selectedStashesCount == 1
                ? $"#{clickedStash.ID} {clickedStash.Name}"
                : $"({selectedStashesCount})");
            AddSeparator();
        }

        public void AddColorOption()
        {
            string colorButtonText = X(G.L.ColorButton());
            ToolStripMenuItem colorButton = (ToolStripMenuItem)Items.Add(colorButtonText);
            foreach (Common.Config.ConfigColor col in G.Configuration.Colors)
            {
                string optionText = X(col.Name != "" ? col.Name : col.Value);
                ToolStripMenuItem option = new ToolStripMenuItem(optionText, null, delegate
                {
                    foreach (StashObject st in selectedStashes)
                        st.TextColor = col.Value;
                    G.Configuration.Save();
                    G.Stashes.InvokeStashesInfoChanged(selectedStashes);
                })
                {
                    BackColor = Color.FromArgb(0, 0, 0)
                };
                try
                {
                    Color cFore = ColorTranslator.FromHtml(col.Value);
                    option.ForeColor = cFore;
                    option.MouseEnter += delegate { option.ForeColor = Color.Black; };
                    option.MouseLeave += delegate { option.ForeColor = cFore; };
                    colorButton.DropDownItems.Add(option);
                }
                catch (Exception)
                {
                }
            }
            if (colorButton.DropDownItems.Count == 0)
            {
                colorButton.DropDownItems.Insert(0, new ToolStripMenuItem(X(G.L.EmptyButton()))
                { ForeColor = Color.Gray });
            }
        }

        public void AddLockOption()
        {
            if (selectedStashesCount == 1)
            {
                Items.Add(X(clickedStash.Locked
                    ? G.L.UnlockButton()
                    : G.L.LockButton()
                ), Properties.Resources.LockBlackIcon, delegate {
                    clickedStash.Locked = !clickedStash.Locked;
                    G.Configuration.Save();
                    G.Stashes.InvokeStashesInfoChanged(clickedStash);
                });
            }
            else
            {
                Items.Add(X(G.L.LockButton()), Properties.Resources.LockBlackIcon, delegate {
                    foreach (StashObject selStash in selectedStashes) selStash.Locked = true;
                    G.Configuration.Save();
                    G.Stashes.InvokeStashesInfoChanged(selectedStashes);
                });
                Items.Add(X(G.L.UnlockButton()), Properties.Resources.LockBlackIcon, delegate {
                    foreach (StashObject selStash in selectedStashes) selStash.Locked = false;
                    G.Configuration.Save();
                    G.Stashes.InvokeStashesInfoChanged(selectedStashes);
                });
            }
        }

        public void AddEditNameOption()
        {
            if (selectedStashesCount > 1) return;

            Items.Add(X(G.L.RenameButton()), null, delegate (object s, EventArgs e) {
                page.ActivateNameEditing(rowIndex);
            });
        }

        public void AddRestoreBackupOption()
        {
            if (selectedStashesCount > 1) return;

            var restoreButtonText = X(G.L.RestoreBackupButton());
            var restoreButton = (ToolStripMenuItem)Items.Add(restoreButtonText);
            G.Stashes.GetBackupFiles(clickedStash.ID)
                .ToList().ForEach(file => {
                    string fileName = System.IO.Path.GetFileName(file);
                    string fileDate = System.IO.File.GetLastWriteTime(file).ToString();
                    if (TransferFile.FromFile(file, out TransferFile transferFile))
                    {
                        string itemText = $"{fileName} - {fileDate} - {transferFile.TotalUsageText}";

                        restoreButton.DropDownItems.Add(X(itemText), null, delegate (object s, EventArgs e) {
                            G.Stashes.RestoreTransferFile(clickedStash.ID, file);
                            clickedStash.LoadTransferFile();
                            G.Stashes.InvokeStashesContentChanged(clickedStash, true);
                        });
                    }
                    else
                    {
                        restoreButton.DropDownItems.Add(X($"{fileName} - UNABLE TO LOAD"));
                    }
                }
            );
            if (restoreButton.DropDownItems.Count == 0)
            {
                restoreButton.DropDownItems.Insert(0, new ToolStripMenuItem(X(G.L.EmptyButton()))
                { ForeColor = Color.Gray });
            }
        }

        public void AddOverwriteOption()
        {
            if (selectedStashesCount > 1) return;

            Items.Add(X(G.L.OverwriteButton()), null, delegate {
                DialogResult result = GrimDawnLib.GrimDawn.ShowSelectTransferFilesDialog(out string[] files, false, true);
                if (result == DialogResult.OK)
                {
                    if (G.Stashes.ImportOverwriteStash(files[0], clickedStash))
                    {
                        G.Stashes.InvokeStashesContentChanged(clickedStash, true);
                    }
                }
            });
        }

        public void AddExportButton()
        {
            Items.Add(X(G.L.ExportButton()), null, delegate {

                StashesZipFile zipFile = new StashesZipFile();
                foreach (StashObject selStash in selectedStashes) zipFile.AddStash(selStash);
                using (var dialog = new SaveFileDialog()
                {
                    Filter = $"{G.L.ZipArchive()}|*.zip",
                    FileName = "TransferFiles.zip",
                })
                {
                    DialogResult result = dialog.ShowDialog();
                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.FileName))
                    {
                        zipFile.SaveTo(dialog.FileName);
                    }
                }
            });
        }

        public void AddEditTabsButton()
        {
            Items.Add(X(G.L.EditTabsButton()), null, delegate {
                foreach (StashObject s in selectedStashes)
                    G.Windows.ShowStashTabsEditorWindow(s);
            });
        }

        public void AddChangeExpansionOption()
        {
            if (G.Runtime.ShownExpansion == GrimDawnLib.GrimDawn.LatestExpansion) return;

            ToolStripMenuItem copyToExpButton = (ToolStripMenuItem)Items.Add(X(G.L.CopyToExpansionButton()));
            for (int i = (int)G.Runtime.ShownExpansion + 1; i <= (int)GrimDawnLib.GrimDawn.LatestExpansion; i += 1)
            {
                GrimDawnLib.GrimDawnGameExpansion exp = (GrimDawnLib.GrimDawnGameExpansion)i;
                copyToExpButton.DropDownItems.Add(X(GrimDawnLib.GrimDawn.ExpansionNames[exp]), null, delegate {

                    var addedStashes = new List<StashObject>();
                    var removedStashes = new List<StashObject>();
                    foreach (var st in selectedStashes)
                    {
                        StashObject copied = G.Stashes.CreateStashCopy(st);
                        copied.Expansion = exp;
                        addedStashes.Add(copied);
                    }
                    if (Console.Confirm(G.L.ConfirmDeleteOldStashesMessage()))
                    {
                        //todo: if any stash is selectedStashes, switch to first stash in grp, if grp empty: to main stash

                        removedStashes = G.Stashes.DeleteStashes(selectedStashes);
                    }
                    G.Configuration.Save();
                    G.Stashes.InvokeStashesRemoved(removedStashes);
                    G.Stashes.InvokeStashesAdded(addedStashes);
                });
            }
        }

        public void AddAutoFillOption()
        {
            var fillButtonText = X(G.L.AutoFillButton());
            var fillButton = (ToolStripMenuItem)Items.Add(fillButtonText);
            fillButton.DropDownItems.Add(X(G.L.AutoFillRandomSeedsButton()), null, delegate {
                foreach (var s in selectedStashes)
                {
                    G.FileSystem.BackupStashTransferFile(s.ID);
                    s.AutoFill();
                    s.SaveTransferFile();
                    s.LoadTransferFile();
                    G.Stashes.InvokeStashesContentChanged(s, true);
                }
            });
        }

        public void AddAutoSortOption()
        {
            var sortButtonText = X(G.L.SortItemsButton());
            var sortButton = (ToolStripMenuItem)Items.Add(sortButtonText);
            
            foreach (Common.Config.ConfigSortPattern sortPat in G.Configuration.SortPatterns)
            {
                sortButton.DropDownItems.Add(X(sortPat.Name), null, delegate {
                    var sorthandler = new SortHandler(selectedStashes);

                    var sortPatternLines = Utils.Funcs.StringLines(sortPat.Value);
                    foreach (string l in sortPatternLines)
                        sorthandler.Sort(l.Trim());

                    Console.Alert(G.L.SortingFinishedMessage());
                    page.StashEnsureVisible(clickedStash);
                    page.SelectStashes(selectedStashes.ToList());
                });
            }
        }
        
        public void AddDeleteOption()
        {
            var deleteButtonText = X(G.L.DeleteButton());
            var deleteButton = (ToolStripMenuItem)Items.Add(deleteButtonText);
            deleteButton.DropDownItems.Add(X(G.L.DeleteSelectedStashesButton()), null, delegate {
                if (G.Configuration.Settings.ConfirmStashDelete
                && !Console.Confirm(G.L.ConfirmDeleteStashesMessage()))
                    return;
                //todo: if any stash is selectedStashes, switch to first stash in grp, if grp empty: to main stash


                List<StashObject> deletedItems = G.Stashes.DeleteStashes(selectedStashes);
                G.Configuration.Save();
                G.Stashes.InvokeStashesRemoved(deletedItems);
            });
            deleteButton.DropDownItems.Add(X(G.L.DeleteEmptyStashesButton()), null, delegate {
                //todo: if any stash is selectedStashes, switch to first stash in grp, if grp empty: to main stash


                List<StashObject> deletedItems = G.Stashes.DeleteStashes(selectedStashes, true);
                G.Configuration.Save();
                G.Stashes.InvokeStashesRemoved(deletedItems);
            });
        }

    }
}
