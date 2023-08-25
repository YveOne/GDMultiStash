using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

using GDMultiStash.Common;
using GDMultiStash.Common.Objects;
using GDMultiStash.Forms.StashTabsEditor;

using Utils.Extensions;

namespace GDMultiStash.Forms
{
    internal partial class StashTabsEditorWindow : BaseForm
    {

        private readonly StashObject stashObject;
        private uint maxTabs = 0;
        private StashTabBasePanel AddButtonPanel;

        private static DragImage draggingImage = null;

        private bool dragRequested = false;
        private DateTime dragStartTime = DateTime.Now;

        public StashTabsEditorWindow(StashObject stash) : base()
        {
            InitializeComponent();

            backgroundPanel.BackColor = Constants.FormBackColor;
            tabsListPanel.BackColor = Constants.FormBackColor;

            stashObject = stash;
            maxTabs = TransferFile.GetStashInfoForExpansion(stash.Expansion).MaxTabs;

            AddButtonPanel = new StashTabAddPanel(stash);
            AddButtonPanel.Click += AddButton_Click;

            tabsListPanel.AllowDrop = true;
            tabsListPanel.DragOver += new DragEventHandler(TabsPanel_DragOver);
            tabsListPanel.DragLeave += new EventHandler(TabsPanel_DragLeave);
            tabsListPanel.DragDrop += new DragEventHandler(TabsPanel_DragDrop);
            tabsListPanel.ControlAdded += delegate { UpdateAddButton(); };
            tabsListPanel.ControlRemoved += delegate { UpdateAddButton(); };

            Load += delegate {
                UpdateWindowTitle();
                CreateTabs();
            };
        }

        protected override void Localize(GlobalHandlers.LocalizationHandler.StringsHolder L)
        {

        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            Global.Runtime.StashesRemoved += Global_Ingame_StashesRemoved;
            Global.Runtime.StashesContentChanged += Global_Ingame_StashesContentChanged;
            Global.Runtime.StashesInfoChanged += Global_Ingame_StashesInfoChanged;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            Global.Runtime.StashesRemoved -= Global_Ingame_StashesRemoved;
            Global.Runtime.StashesContentChanged -= Global_Ingame_StashesContentChanged;
            Global.Runtime.StashesInfoChanged -= Global_Ingame_StashesInfoChanged;
        }

        private void Global_Ingame_StashesRemoved(object sender, GlobalHandlers.RuntimeHandler.ListEventArgs<StashObject> e)
        {
            if (e.Items.Contains(stashObject))
                Invoke(new Action(Close));
        }

        private void Global_Ingame_StashesContentChanged(object sender, GlobalHandlers.RuntimeHandler.StashesContentChangedEventArgs e)
        {
            if (e.Items.Contains(stashObject))
                Invoke(new Action(CreateTabs));
        }

        private void Global_Ingame_StashesInfoChanged(object sender, GlobalHandlers.RuntimeHandler.ListEventArgs<StashObject> e)
        {
            if (e.Items.Contains(stashObject))
                Invoke(new Action(UpdateWindowTitle));
        }

        private void UpdateWindowTitle()
        {
            Text = $"#{stashObject.ID} {stashObject.Name}";
        }

        private void UpdateAddButton()
        {
            AddButtonPanel.Visible = tabsListPanel.Controls.Count <= maxTabs;
        }

        private void CreateTabs()
        {
            tabsListPanel.Controls.Clear();
            foreach (var tab in stashObject.Tabs)
            {
                var panel = new StashTabPanel(stashObject, tab);
                panel.MouseDown += new MouseEventHandler(Tab_MouseDown);
                panel.MouseMove += new MouseEventHandler(Tab_MouseMove);
                panel.MouseUp += new MouseEventHandler(Tab_MouseUp);
                panel.QueryContinueDrag += new QueryContinueDragEventHandler(Tab_QueryContinueDrag);
                panel.Click += StashTab_Click;
                tabsListPanel.Controls.Add(panel);
            }

            tabsListPanel.Controls.Add(AddButtonPanel);
            UpdateAddButton();

            Panel firstTab = (Panel)tabsListPanel.Controls[0];

            tabsListPanel.Width = (int)maxTabs * (firstTab.Width + StashTabBasePanel.TabsMargin * 2) + tabsListPanel.Padding.Horizontal;
            tabsListPanel.Height = firstTab.Height + StashTabBasePanel.TabsMargin * 2 + tabsListPanel.Padding.Vertical;

            Width = tabsListPanel.Width + tabsListPanel.Location.X * 2 + (Width - ClientSize.Width) + Padding.Horizontal;
            Height = tabsListPanel.Height + tabsListPanel.Location.Y * 2 + (Height - ClientSize.Height) + Padding.Vertical;
            Width = Math.Max(Width, MinimumSize.Width);
            Height = Math.Max(Height, MinimumSize.Height);
        }

        private void ResetDragging()
        {
            draggingImage.Reset();
            draggingImage.Close();
            draggingImage.Dispose();
            draggingImage = null;
        }

        #region delay next available action

        private long TimestampMiliseconds()
        {
            return (long)DateTime.Now.ToUniversalTime().Subtract(
                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            ).TotalMilliseconds;
        }
        private long _nextActionAvailabaleAfter = 0;
        private bool ActionIsAvailable()
        {
            return TimestampMiliseconds() - _nextActionAvailabaleAfter >= 0;
        }
        private void DelayNextAction(int ms)
        {
            _nextActionAvailabaleAfter = TimestampMiliseconds() + ms;
        }

        #endregion

        #region events 

        private void AddButton_Click(object sender, EventArgs e)
        {
            if (!ActionIsAvailable()) return;
            DelayNextAction(300);
            stashObject.AddTab();
            stashObject.SaveTransferFile();
            stashObject.LoadTransferFile();
            Global.Runtime.InvokeStashesContentChanged(stashObject, true);
            UpdateAddButton();
        }

        private void StashTab_Click(object sender, EventArgs e)
        {
            if (!(sender is StashTabPanel tabPanel)) return;
            var me = (MouseEventArgs)e;
            if (me.Button != MouseButtons.Right) return;

            ContextMenu cm = new ContextMenu();

            if (tabPanel.StashTab.Items.Count != 0)
            {
                cm.MenuItems.Add(Global.L.ClearButton(), delegate {
                    if (tabPanel.StashTab.Items.Count != 0
                        && Global.Configuration.Settings.ConfirmStashDelete
                        && !Console.Confirm(Global.L.ConfirmClearStashTabMessage())) return;

                    Global.FileSystem.BackupStashTransferFile(stashObject.ID);
                    tabPanel.StashTab.Items.Clear();
                    stashObject.SaveTransferFile();
                    stashObject.LoadTransferFile();
                    Global.Runtime.InvokeStashesContentChanged(stashObject, true);
                });
            }

            if (stashObject.Tabs.Count < stashObject.MaxTabsCount)
            {
                if (tabPanel.StashTab.Items.Count != 0)
                {
                    cm.MenuItems.Add(Global.L.DuplicateButton(), delegate {
                        var dup = new GDIALib.Parser.Stash.StashTab();
                        foreach (var item in tabPanel.StashTab.Items)
                            dup.Items.Add(item.DeepClone());
                        stashObject.AddTab(dup);
                        stashObject.SaveTransferFile();
                        stashObject.LoadTransferFile();
                        Global.Runtime.InvokeStashesContentChanged(stashObject, true);
                    });
                }
            }

            // dont delete last tab! it would crash gd!
            if (stashObject.Tabs.Count != 1)
            {
                cm.MenuItems.Add(Global.L.DeleteButton(), delegate {
                    if (tabPanel.StashTab.Items.Count != 0
                        && Global.Configuration.Settings.ConfirmStashDelete
                        && !Console.Confirm(Global.L.ConfirmDeleteStashTabMessage())) return;

                    Global.FileSystem.BackupStashTransferFile(stashObject.ID);
                    stashObject.RemoveTabAt(stashObject.Tabs.IndexOf(tabPanel.StashTab));
                    stashObject.SaveTransferFile();
                    stashObject.LoadTransferFile();
                    Global.Runtime.InvokeStashesContentChanged(stashObject, true);
                });
            }

            cm.Show(tabPanel, me.Location);
        }

        private void Tab_MouseDown(object sender, MouseEventArgs e)
        {
            if (!ActionIsAvailable()) return;
            if (e.Button != MouseButtons.Left) return;
            dragRequested = true;
            dragStartTime = DateTime.Now;
        }

        private void Tab_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (!ActionIsAvailable())
            {
                dragRequested = false;
            }
            if (dragRequested && (DateTime.Now - dragStartTime).TotalMilliseconds > 50)
            {
                dragRequested = false;
                StashTabPanel tabPanel = sender as StashTabPanel;
                // dont delete last tab! it would crash gd!
                if (tabPanel.StashObject.Tabs.Count == 1) return;
                draggingImage = new DragImage(tabPanel, new Point(-e.Location.X, -e.Location.Y));
                tabPanel.DoDragDrop(tabPanel, DragDropEffects.Move);
            }
        }

        private void Tab_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            dragRequested = false;
        }

        private void Tab_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            // this will get called before TabsPanel_DragDrop
            if (e.Action == DragAction.Drop)
            {
                StashTabPanel tabPanel = sender as StashTabPanel;
                if (tabPanel.Parent == null)
                {
                    ResetDragging();
                }
            }
        }

        private void TabsPanel_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(e.Data.GetFormats()[0]) is StashTabPanel tabPanel)
            {
                tabPanel.StashObject.RemoveTab(tabPanel.StashTab);
                stashObject.AddTab(tabPanel.StashTab, tabPanel.Index);

                Global.FileSystem.BackupStashTransferFile(tabPanel.StashObject.ID);
                tabPanel.StashObject.SaveTransferFile();
                tabPanel.StashObject.LoadTransferFile();
                Global.Runtime.InvokeStashesContentChanged(tabPanel.StashObject, true);

                if (stashObject != tabPanel.StashObject)
                {
                    Global.FileSystem.BackupStashTransferFile(stashObject.ID);
                    stashObject.SaveTransferFile();
                    stashObject.LoadTransferFile();
                    Global.Runtime.InvokeStashesContentChanged(stashObject, true);
                }
            }
        }

        private void TabsPanel_DragOver(object sender, DragEventArgs e)
        {
            var tabsListPanel = (FlowLayoutPanel)sender;
            if (tabsListPanel == null)
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            var canDrop = tabsListPanel.Controls.Count-1 < maxTabs || tabsListPanel.Controls.Contains(draggingImage.TabPanel);
            if (!canDrop)
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            e.Effect = DragDropEffects.Move;
            Panel overPanel = null;
            if (canDrop)
            {
                foreach (Panel p in tabsListPanel.Controls)
                {
                    if (p is StashTabAddPanel) continue;
                    Rectangle b = p.Bounds;
                    b.Inflate(StashTabBasePanel.TabsMargin, StashTabBasePanel.TabsMargin);
                    if (b.Contains(tabsListPanel.PointToClient(Cursor.Position)))
                    {
                        overPanel = p;
                    }
                }
            }
            if (overPanel != null)
            {
                draggingImage.AppendTo(tabsListPanel, tabsListPanel.Controls.IndexOf(overPanel));
                UpdateAddButton();
            }
            else
            {
                if (canDrop)
                {
                    draggingImage.AppendTo(tabsListPanel);
                }
            }
        }

        private void TabsPanel_DragLeave(object sender, EventArgs e)
        {
            if (draggingImage == null) return;
            draggingImage.Remove();
        }
        
        #endregion

    }
}
