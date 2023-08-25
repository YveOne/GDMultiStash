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
    internal class DragImage : Form
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
                            Invoke(new Action(() => {
                                Location = new Point(
                                    Cursor.Position.X + offset.X,
                                    Cursor.Position.Y + offset.Y
                                );
                            }));
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
}
