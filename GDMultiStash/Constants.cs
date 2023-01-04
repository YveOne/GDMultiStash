using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace GDMultiStash
{
    internal static class Constants
    {

        internal static int WindowCaptionDragHeight { get; } = 60;
        internal static int WindowResizeBorderSize { get; } = 8;

        internal static int ListViewColumnsHeight { get; } = 30;
        internal static int ListViewColumnsFontHeight { get; } = 11;
        internal static int ListViewRowHeight { get; } = 25;
        internal static int ListViewGroupHeaderHeight { get; } = 25;
        internal static int ListViewGroupSpaceBetween { get; } = 5;

        internal static Padding FormPadding { get; } = new Padding(20, 20, 20, 20);
        internal static Padding PagesPadding { get; } = new Padding(5, 5, 5, 5);
        internal static Padding ListViewBorderPadding { get; } = new Padding(5, 5, 5, 5);

        internal static Color FormBackColor { get; } = Color.FromArgb(28, 28, 28);

        internal static Color InteractiveForeColor { get; } = Color.FromArgb(200, 200, 200);
        internal static Color InteractiveForeColorHighlight { get; } = Color.FromArgb(250, 250, 250);
        internal static Color PassiveForeColor { get; } = Color.FromArgb(120, 120, 120);

        internal static Color CaptionButtonBackColor { get; } = Color.FromArgb(28, 28, 28);
        internal static Color CaptionButtonBackColorHover { get; } = Color.FromArgb(50, 50, 50);
        internal static Color CaptionButtonBackColorPressed { get; } = Color.FromArgb(70, 70, 70);

        internal static Color PageButtonBackColor { get; } = Color.FromArgb(35, 35, 35);
        internal static Color PageButtonBackColorActive { get; } = Color.FromArgb(45, 45, 45);
        internal static Color PageBackColor { get; } = Color.FromArgb(45, 45, 45);

        internal static Color ToolStripBackColor { get; } = Color.FromArgb(45, 45, 45);
        internal static Color ToolStripButtonBackColor { get; } = Color.FromArgb(45, 45, 45);
        internal static Color ToolStripButtonBackColorHover { get; } = Color.FromArgb(45, 45, 45);

        internal static Color ListViewBackColor { get; } = Color.FromArgb(37, 37, 37);
        internal static Color ListViewItemBackColor { get; } = Color.FromArgb(50, 50, 50);
        internal static Color ListViewCellBorderColor { get; } = Color.FromArgb(37, 37, 37);
        internal static Color ListViewGroupHeaderBackColor { get; } = Color.FromArgb(60, 60, 60);
        internal static Color ListViewGroupHeaderCountForeColor { get; } = Color.FromArgb(180, 180, 180);
        internal static Color ListViewGroupHeaderForeColor { get; } = Color.FromArgb(245, 245, 245);
        internal static Color ListViewGroupHeaderSeparatorColor { get; } = Color.FromArgb(50, 50, 50);

        internal static Color ScrollBarColor { get; } = Color.FromArgb(100, 100, 100);











    }
}
