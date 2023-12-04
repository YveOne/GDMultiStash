using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace GDMultiStash
{
    internal class C
    {

        //TODO: search for unused constants

        internal static string AppName { get; } = "GDMultiStash";
        internal static int WM_SHOWME { get; } = Native.RegisterWindowMessage("GDMS_SHOW");

        internal static string AppDataPath { get; } = Path.Combine(Application.StartupPath, "Data");
        internal static string StashesPath { get; } = Path.Combine(AppDataPath, "Stashes");
        internal static string LocalesPath { get; } = Path.Combine(AppDataPath, "Locales");
        internal static string ConfigFile { get; } = Path.Combine(AppDataPath, "Config.xml");
        internal static string ConfigFileBackup { get; } = $"{ConfigFile}.backup";

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
        internal static Color FormTitleBackColor { get; } = Color.FromArgb(31, 31, 31);

        internal static Color InteractiveForeColor { get; } = Color.FromArgb(200, 200, 200);
        internal static Color InteractiveForeColorHighlight { get; } = Color.FromArgb(250, 250, 250);
        internal static Color PassiveForeColor { get; } = Color.FromArgb(120, 120, 120);

        internal static Color ControlBoxButtonBackColor { get; } = FormTitleBackColor;
        internal static Color ControlBoxButtonBackColorHover { get; } = Color.FromArgb(50, 50, 50);
        internal static Color ControlBoxButtonBackColorPressed { get; } = Color.FromArgb(70, 70, 70);

        internal static Color PageButtonBackColor { get; } = Color.FromArgb(35, 35, 35);
        internal static Color PageButtonBackColorActive { get; } = Color.FromArgb(45, 45, 45);
        internal static Color PageBackColor { get; } = Color.FromArgb(45, 45, 45);

        internal static Color ToolStripBackColor { get; } = Color.FromArgb(45, 45, 45);
        internal static Color ToolStripButtonBackColor { get; } = Color.FromArgb(45, 45, 45);
        internal static Color ToolStripButtonBackColorHover { get; } = Color.FromArgb(45, 45, 45);

        internal static Color ListViewBackColor { get; } = Color.FromArgb(37, 37, 37);
        internal static Color ListViewItemBackColor { get; } = Color.FromArgb(50, 50, 50);
        internal static Color ListViewItemBackColorSelected { get; } = Color.Teal;
        internal static Color ListViewCellBorderColor { get; } = Color.FromArgb(37, 37, 37);
        internal static Color ListViewGroupHeaderBackColor { get; } = Color.FromArgb(60, 60, 60);
        internal static Color ListViewGroupHeaderBackColorEmpty { get; } = Color.FromArgb(45, 45, 45);
        internal static Color ListViewGroupHeaderBackColorSelected { get; } = SystemColors.Highlight;
        internal static Color ListViewGroupHeaderCountForeColor { get; } = Color.FromArgb(180, 180, 180);
        internal static Color ListViewGroupHeaderForeColor { get; } = Color.FromArgb(245, 245, 245);
        internal static Color ListViewGroupHeaderForeColorEmpty { get; } = Color.FromArgb(150, 150, 150);
        internal static Color ListViewGroupHeaderSeparatorColor { get; } = Color.FromArgb(50, 50, 50);

        internal static Color ScrollBarColor { get; } = Color.FromArgb(100, 100, 100);

        internal static Color ComboBoxBorderColor { get; } = FormBackColor;
        internal static Color ComboBoxButtonColor { get; } = PassiveForeColor;
        internal static Color ComboBoxButtonColorHighlight { get; } = InteractiveForeColor;
        internal static Color ComboBoxBackColor { get; } = ListViewBackColor;
        internal static Color ComboBoxForeColor { get; } = InteractiveForeColor;
        internal static Color ComboBoxBackColorHighlight { get; } = ListViewGroupHeaderBackColor;
        internal static Color ComboBoxForeColorHighlight { get; } = InteractiveForeColorHighlight;
        internal static Color ComboBoxBorderListBorderColor { get; } = ListViewGroupHeaderBackColor;

        internal static Color ContextBorderColor { get; } = FormTitleBackColor;
        internal static Color ContextForeColor { get; } = InteractiveForeColor;
        internal static Color ContextForeColorHover { get; } = InteractiveForeColorHighlight;
        internal static Color ContextFirstBackColor { get; } = FormTitleBackColor;
        internal static Color ContextFirstBackColorHover { get; } = ControlBoxButtonBackColorHover;
        internal static Color ContextFirstBackColorSelected { get; } = ControlBoxButtonBackColorPressed;
        internal static Color ContextBackColor { get; } = ControlBoxButtonBackColorHover;
        internal static Color ContextBackColorHover { get; } = ControlBoxButtonBackColorPressed;


    }
}
