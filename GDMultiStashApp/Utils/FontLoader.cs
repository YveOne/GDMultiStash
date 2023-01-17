using System;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Windows.Forms;

namespace Utils
{

    public class FontLoader
    {

        private static PrivateFontCollection fontCollection = new PrivateFontCollection();

        public static void LoadFromResource(byte[] fontBytes)
        {
            int index = fontCollection.Families.Length;
            var fontData = Marshal.AllocCoTaskMem(fontBytes.Length);
            Marshal.Copy(fontBytes, 0, fontData, fontBytes.Length);
            fontCollection.AddMemoryFont(fontData, fontBytes.Length);
        }

        public static Font GetFont(string familyName, int fontSize, FontStyle fontStyle)
        {
            foreach (FontFamily ff in fontCollection.Families)
            {
                if (ff.Name == familyName)
                    return new Font(ff, fontSize, fontStyle, GraphicsUnit.Pixel);
            }
            return null;
        }

    }

}
