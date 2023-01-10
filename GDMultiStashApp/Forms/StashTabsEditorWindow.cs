using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

using GDMultiStash.Common;
using GDMultiStash.Common.Objects;

namespace GDMultiStash.Forms
{
    internal partial class StashTabsEditorWindow : BaseForm
    {

        private readonly StashObject stashObject;
        private uint maxTabs = 0;
        private StashTabBasePanel AddButtonPanel;

        private DateTime lastAddedTime = DateTime.Now;

        public StashTabsEditorWindow(StashObject stash) : base()
        {
            InitializeComponent();

            backgroundPanel.BackColor = Constants.FormBackColor;
            tabsListPanel.BackColor = Constants.FormBackColor;

            stashObject = stash;
            maxTabs = GrimDawnLib.GrimDawn.Stashes.GetStashInfoForExpansion(stash.Expansion).MaxTabs;

            AddButtonPanel = new StashTabAddPanel(stash);
            AddButtonPanel.Click += delegate {
                if ((DateTime.Now - lastAddedTime).TotalMilliseconds < 500) return;
                lastAddedTime = DateTime.Now;
                stashObject.AddTab();
                stashObject.SaveTransferFile();
                stashObject.LoadTransferFile();
                Global.Runtime.NotifyStashesContentChanged(stashObject);
                UpdateAddButton();
            };

            tabsListPanel.AllowDrop = true;
            tabsListPanel.DragOver += new DragEventHandler(TabsPanel_DragOver);
            tabsListPanel.DragLeave += new EventHandler(TabsPanel_DragLeave);
            tabsListPanel.DragDrop += new DragEventHandler(TabsPanel_DragDrop);

            Global.Runtime.StashesRemoved += delegate (object sender, GlobalHandlers.RuntimeHandler.ListEventArgs<StashObject> e)
            {
                if (e.Items.Contains(stash)) Close();
            };

            Global.Runtime.StashesContentChanged += delegate (object sender, GlobalHandlers.RuntimeHandler.ListEventArgs<StashObject> e)
            {
                if (e.Items.Contains(stash)) CreateTabs();
            };

            Global.Runtime.StashesInfoChanged += delegate (object sender, GlobalHandlers.RuntimeHandler.ListEventArgs<StashObject> e)
            {
                if (e.Items.Contains(stash)) UpdateInfo();
            };

            tabsListPanel.ControlAdded += delegate { UpdateAddButton(); };
            tabsListPanel.ControlRemoved += delegate { UpdateAddButton(); };

            Load += delegate {
                UpdateInfo();
                CreateTabs();
            };
            FormClosing += delegate {
                Global.Windows.ShowMainWindow();
            };
        }

        protected override void Localize(GlobalHandlers.LocalizationHandler.StringsHolder L)
        {

        }

        private void UpdateInfo()
        {
            Text = $"#{stashObject.ID} {stashObject.Name}";
        }

        private const int tabsMargin = 5;

        public class StashTabBasePanel : Panel
        {
            public Image HoverImage { get; set; }
            private bool hover = false;

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                if (hover) 
                    e.Graphics.DrawImage(HoverImage, 0, 0, Width, Height);
            }


            public StashTabBasePanel(StashObject stashObject)
            {
                var stashInfo = GrimDawnLib.GrimDawn.Stashes.GetStashInfoForExpansion(stashObject.Expansion);
                switch (stashInfo.Width)
                {
                    case 8:
                        HoverImage = Properties.Resources.caravanWindow8x16hover;
                        break;
                    default:
                        HoverImage = Properties.Resources.caravanWindow10x18hover;
                        break;
                }
                Width = HoverImage.Width;
                Height = HoverImage.Height;
                BackgroundImageLayout = ImageLayout.Stretch;

                MouseEnter  += delegate { hover = true; Invalidate(); };
                MouseLeave += delegate { hover = false; Invalidate(); };

                Margin = new Padding(tabsMargin, tabsMargin, tabsMargin, tabsMargin);
                Cursor = Cursors.Hand;
            }
        }

        public class StashTabAddPanel : StashTabBasePanel
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

        public class StashTabPanel : StashTabBasePanel
        {
            public GDIALib.Parser.Stash.StashTab StashTab { get; private set; }
            public StashObject StashObject { get; private set; }
            public int Index { get; private set; }

            public StashTabPanel(StashObject stashObject, GDIALib.Parser.Stash.StashTab tab) : base(stashObject)
            {
                this.StashTab = tab;
                this.StashObject = stashObject;
                BackgroundImage = Global.Database.CreateTabImage(tab, stashObject.Expansion);
                ContextMenu cm = new ContextMenu();
                cm.MenuItems.Add(Global.L.DeleteButton(), delegate {
                    if (tab.Items.Count != 0
                        && Global.Configuration.Settings.ConfirmStashDelete
                        && !Console.Confirm(Global.L.ConfirmDeleteStashTabMessage())) return;

                    Global.FileSystem.BackupStashTransferFile(stashObject.ID);
                    stashObject.RemoveTabAt(stashObject.Tabs.IndexOf(tab));
                    stashObject.SaveTransferFile();
                    stashObject.LoadTransferFile();
                    Global.Runtime.NotifyStashesContentChanged(stashObject);
                });
                Click += delegate (object sender, EventArgs e)
                {
                    // dont delete last tab! it would crash gd!
                    if (StashObject.Tabs.Count == 1) return;
                    MouseEventArgs me = (MouseEventArgs)e;
                    if (me.Button != MouseButtons.Right) return;
                    cm.Show(this, me.Location);
                };
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

        public void UpdateAddButton()
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
                tabsListPanel.Controls.Add(panel);
            }

            tabsListPanel.Controls.Add(AddButtonPanel);
            UpdateAddButton();

            Panel firstTab = (Panel)tabsListPanel.Controls[0];

            tabsListPanel.Width = (int)maxTabs * (firstTab.Width + tabsMargin * 2) + tabsListPanel.Padding.Horizontal;
            tabsListPanel.Height = firstTab.Height + tabsMargin * 2 + tabsListPanel.Padding.Vertical;

            Width = tabsListPanel.Width + tabsListPanel.Location.X * 2 + (Width - ClientSize.Width) + Padding.Horizontal;
            Height = tabsListPanel.Height + tabsListPanel.Location.Y * 2 + (Height - ClientSize.Height) + Padding.Vertical;
            Width = Math.Max(Width, MinimumSize.Width);
            Height = Math.Max(Height, MinimumSize.Height);
        }









        #region drag stuff

        private static DragImage draggingImage = null;

        private void ResetDragging()
        {
            draggingImage.Reset();
            draggingImage.Close();
            draggingImage.Dispose();
            draggingImage = null;
        }





        public class DragImage : Form
        {
            public StashTabPanel TabPanel { get; private set; }

            private Control draggingOriginalParentControl;
            private int draggingOriginalIndex;

            public DragImage(StashTabPanel tabPanel, Point offset)
            {
                draggingOriginalParentControl = tabPanel.Parent;
                draggingOriginalIndex = tabPanel.Parent.Controls.IndexOf(tabPanel);

                FormBorderStyle = FormBorderStyle.None;
                Padding = Padding.Empty;
                TopMost = true;
                BackgroundImage = new Bitmap(tabPanel.BackgroundImage);
                StartPosition = FormStartPosition.Manual;
                ShowInTaskbar = false;
                Location = new Point(
                    Cursor.Position.X + offset.X,
                    Cursor.Position.Y + offset.Y
                );
                Width = tabPanel.Width;
                Height = tabPanel.Height;

                TabPanel = tabPanel;

                bool loop = true;
                FormClosing += delegate { loop = false; };
                System.Threading.Thread t = null;

                Load += delegate {
                    t = new System.Threading.Thread(() => {
                        try
                        {
                            while (loop)
                            {
                                Invoke((MethodInvoker)delegate {
                                    Location = new Point(
                                        Cursor.Position.X + offset.X,
                                        Cursor.Position.Y + offset.Y
                                    );
                                });
                                System.Threading.Thread.Sleep(1);
                            }
                        }
                        catch (Exception)
                        {
                        }
                    });
                    t.Start();
                };

                Shown += delegate {
                    Native.SetWindowLong(Handle, Native.GWL_EXSTYLE, (IntPtr)(Native.GetWindowLong(Handle, Native.GWL_EXSTYLE) | Native.WS_EX_LAYERED | Native.WS_EX_TRANSPARENT));
                    Native.SetLayeredWindowAttributes(Handle, 0, 128, Native.LWA_ALPHA);
                };
            }

            public void AppendTo(Control parent)
            {
                if (parent == null) return;
                AppendTo(parent, parent.Controls.Count);
            }

            public void AppendTo(Control parent, int index)
            {
                if (parent == null) return;
                TabPanel.AppendTo(parent, index);
                Hide();
            }

            public void Remove()
            {
                Visible = true;
                TabPanel.Remove();
                Show();
            }

            public void Reset()
            {
                TabPanel.AppendTo(draggingOriginalParentControl, draggingOriginalIndex, true);
                Hide();
            }

        }

        private bool dragRequested = false;
        private DateTime dragStartTime = DateTime.Now;

        private void Tab_MouseDown(object sender, MouseEventArgs e)
        {
            if ((DateTime.Now - lastAddedTime).TotalMilliseconds < 500) return;
            if (e.Button != MouseButtons.Left) return;
            dragRequested = true;
            dragStartTime = DateTime.Now;
        }

        private void Tab_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if ((DateTime.Now - lastAddedTime).TotalMilliseconds < 500)
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
            StashTabPanel tabPanel = e.Data.GetData(e.Data.GetFormats()[0]) as StashTabPanel;
            if (tabPanel != null)
            {
                tabPanel.StashObject.RemoveTab(tabPanel.StashTab);
                stashObject.AddTab(tabPanel.StashTab, tabPanel.Index);

                Global.FileSystem.BackupStashTransferFile(tabPanel.StashObject.ID);
                tabPanel.StashObject.SaveTransferFile();
                tabPanel.StashObject.LoadTransferFile();
                Global.Runtime.NotifyStashesContentChanged(tabPanel.StashObject);

                if (stashObject != tabPanel.StashObject)
                {
                    Global.FileSystem.BackupStashTransferFile(stashObject.ID);
                    stashObject.SaveTransferFile();
                    stashObject.LoadTransferFile();
                    Global.Runtime.NotifyStashesContentChanged(stashObject);
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
                    b.Inflate(tabsMargin, tabsMargin);
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
