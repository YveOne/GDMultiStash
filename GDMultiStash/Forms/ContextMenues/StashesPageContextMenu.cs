using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

using GDMultiStash.Common;
using GDMultiStash.Common.Objects;

using GDMultiStash.Forms.ContextMenues.SortStashes;

using BrightIdeasSoftware;

namespace GDMultiStash.Forms.ContextMenues
{

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
            string colorButtonText = X(Global.L.ColorButton());
            ToolStripMenuItem colorButton = (ToolStripMenuItem)Items.Add(colorButtonText);
            foreach (Common.Config.ConfigColor col in Global.Configuration.Colors)
            {
                string optionText = X(col.Name != "" ? col.Name : col.Value);
                ToolStripMenuItem option = new ToolStripMenuItem(optionText, null, delegate
                {
                    foreach (StashObject st in selectedStashes)
                        st.TextColor = col.Value;
                    Global.Configuration.Save();
                    Global.Runtime.InvokeStashesInfoChanged(selectedStashes);
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
                colorButton.DropDownItems.Insert(0, new ToolStripMenuItem(X(Global.L.EmptyButton()))
                { ForeColor = Color.Gray });
            }
        }

        public void AddLockOption()
        {
            if (selectedStashesCount == 1)
            {
                Items.Add(X(clickedStash.Locked
                    ? Global.L.UnlockButton()
                    : Global.L.LockButton()
                ), Properties.Resources.LockBlackIcon, delegate {
                    clickedStash.Locked = !clickedStash.Locked;
                    Global.Configuration.Save();
                    Global.Runtime.InvokeStashesInfoChanged(clickedStash);
                });
            }
            else
            {
                Items.Add(X(Global.L.LockButton()), Properties.Resources.LockBlackIcon, delegate {
                    foreach (StashObject selStash in selectedStashes) selStash.Locked = true;
                    Global.Configuration.Save();
                    Global.Runtime.InvokeStashesInfoChanged(selectedStashes);
                });
                Items.Add(X(Global.L.UnlockButton()), Properties.Resources.LockBlackIcon, delegate {
                    foreach (StashObject selStash in selectedStashes) selStash.Locked = false;
                    Global.Configuration.Save();
                    Global.Runtime.InvokeStashesInfoChanged(selectedStashes);
                });
            }
        }

        public void AddEditNameOption()
        {
            if (selectedStashesCount > 1) return;

            Items.Add(X(Global.L.RenameButton()), null, delegate (object s, EventArgs e) {
                page.ActivateNameEditing(rowIndex);
            });
        }

        public void AddRestoreBackupOption()
        {
            if (selectedStashesCount > 1) return;

            var restoreButtonText = X(Global.L.RestoreBackupButton());
            var restoreButton = (ToolStripMenuItem)Items.Add(restoreButtonText);
            Global.Stashes.GetBackupFiles(clickedStash.ID)
                .ToList().ForEach(file => {
                    string fileName = System.IO.Path.GetFileName(file);
                    string fileDate = System.IO.File.GetLastWriteTime(file).ToString();
                    if (TransferFile.FromFile(file, out TransferFile transferFile))
                    {
                        string itemText = $"{fileName} - {fileDate} - {transferFile.TotalUsageText}";

                        restoreButton.DropDownItems.Add(X(itemText), null, delegate (object s, EventArgs e) {
                            Global.Stashes.RestoreTransferFile(clickedStash.ID, file);
                            clickedStash.LoadTransferFile();
                            Global.Runtime.InvokeStashesContentChanged(clickedStash, true);
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
                restoreButton.DropDownItems.Insert(0, new ToolStripMenuItem(X(Global.L.EmptyButton()))
                { ForeColor = Color.Gray });
            }
        }

        public void AddOverwriteOption()
        {
            if (selectedStashesCount > 1) return;

            Items.Add(X(Global.L.OverwriteButton()), null, delegate {
                DialogResult result = GrimDawnLib.GrimDawn.ShowSelectTransferFilesDialog(out string[] files, false, true);
                if (result == DialogResult.OK)
                {
                    if (Global.Stashes.ImportOverwriteStash(files[0], clickedStash))
                    {
                        Global.Runtime.InvokeStashesContentChanged(clickedStash, true);
                    }
                }
            });
        }

        public void AddExportButton()
        {
            Items.Add(X(Global.L.ExportButton()), null, delegate {

                StashesZipFile zipFile = new StashesZipFile();
                foreach (StashObject selStash in selectedStashes) zipFile.AddStash(selStash);
                using (var dialog = new SaveFileDialog()
                {
                    Filter = $"{Global.L.ZipArchive()}|*.zip",
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
            Items.Add(X(Global.L.EditTabsButton()), null, delegate {
                foreach (StashObject s in selectedStashes)
                    Global.Windows.ShowStashTabsEditorWindow(s);
            });
        }

        public void AddChangeExpansionOption()
        {
            if (Global.Runtime.ShownExpansion == GrimDawnLib.GrimDawn.LatestExpansion) return;

            ToolStripMenuItem copyToExpButton = (ToolStripMenuItem)Items.Add(X(Global.L.CopyToExpansionButton()));
            for (int i = (int)Global.Runtime.ShownExpansion + 1; i <= (int)GrimDawnLib.GrimDawn.LatestExpansion; i += 1)
            {
                GrimDawnLib.GrimDawnGameExpansion exp = (GrimDawnLib.GrimDawnGameExpansion)i;
                copyToExpButton.DropDownItems.Add(X(GrimDawnLib.GrimDawn.ExpansionNames[exp]), null, delegate {

                    var addedStashes = new List<StashObject>();
                    var removedStashes = new List<StashObject>();
                    foreach (var st in selectedStashes)
                    {
                        StashObject copied = Global.Stashes.CreateStashCopy(st);
                        copied.Expansion = exp;
                        addedStashes.Add(copied);
                    }
                    if (Console.Confirm(Global.L.ConfirmDeleteOldStashesMessage()))
                    {
                        //todo: if any stash is selectedStashes, switch to first stash in grp, if grp empty: to main stash

                        removedStashes = Global.Stashes.DeleteStashes(selectedStashes);
                    }
                    Global.Configuration.Save();
                    Global.Runtime.InvokeStashesRemoved(removedStashes);
                    Global.Runtime.InvokeStashesAdded(addedStashes);
                });
            }
        }

        public void AddAutoFillOption()
        {
            var fillButtonText = X(Global.L.AutoFillButton());
            var fillButton = (ToolStripMenuItem)Items.Add(fillButtonText);
            fillButton.DropDownItems.Add(X(Global.L.AutoFillRandomSeedsButton()), null, delegate {
                foreach (var s in selectedStashes)
                {
                    Global.FileSystem.BackupStashTransferFile(s.ID);
                    s.AutoFill();
                    s.SaveTransferFile();
                    s.LoadTransferFile();
                    Global.Runtime.InvokeStashesContentChanged(s, true);
                }
            });
        }

        public void AddAutoSortOption()
        {
            var sortButtonText = X(Global.L.SortItemsButton());
            var sortButton = (ToolStripMenuItem)Items.Add(sortButtonText);
            
            foreach (Common.Config.ConfigSortPattern sortPat in Global.Configuration.SortPatterns)
            {
                sortButton.DropDownItems.Add(X(sortPat.Name), null, delegate {
                    var sorthandler = new SortHandler(selectedStashes);

                    var sortPatternLines = Utils.Funcs.StringLines(sortPat.Value);
                    foreach (string l in sortPatternLines)
                        sorthandler.Sort(l.Trim());

                    Console.Alert(Global.L.SortingFinishedMessage());
                    page.StashEnsureVisible(clickedStash);
                    page.SelectStashes(selectedStashes.ToList());
                });
            }
        }
        
        public void AddDeleteOption()
        {
            var deleteButtonText = X(Global.L.DeleteButton());
            var deleteButton = (ToolStripMenuItem)Items.Add(deleteButtonText);
            deleteButton.DropDownItems.Add(X(Global.L.DeleteSelectedStashesButton()), null, delegate {
                if (Global.Configuration.Settings.ConfirmStashDelete
                && !Console.Confirm(Global.L.ConfirmDeleteStashesMessage()))
                    return;
                //todo: if any stash is selectedStashes, switch to first stash in grp, if grp empty: to main stash


                List<StashObject> deletedItems = Global.Stashes.DeleteStashes(selectedStashes);
                Global.Configuration.Save();
                Global.Runtime.InvokeStashesRemoved(deletedItems);
            });
            deleteButton.DropDownItems.Add(X(Global.L.DeleteEmptyStashesButton()), null, delegate {
                //todo: if any stash is selectedStashes, switch to first stash in grp, if grp empty: to main stash


                List<StashObject> deletedItems = Global.Stashes.DeleteStashes(selectedStashes, true);
                Global.Configuration.Save();
                Global.Runtime.InvokeStashesRemoved(deletedItems);
            });
        }

    }
}
