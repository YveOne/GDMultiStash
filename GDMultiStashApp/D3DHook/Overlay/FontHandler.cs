using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace D3DHook.Overlay
{
    public class FontHandler
    {
        private static string GetFontKey(Font font, int size) => $"{font.FontFamily}|{size}";
        private static readonly Dictionary<string, Font> _fontCache = new Dictionary<string, Font>();

        public static void ClearFontCache() => _fontCache.Clear();

        private readonly Font _font;

        public FontHandler(Font font)
        {
            _font = font;
        }

        public Font GetFont(float scale)
        {
            int fontPx = (int)(_font.Size * scale);
            string fontKey = GetFontKey(_font, fontPx);
            if (_fontCache.TryGetValue(fontKey, out Font scaledFont))
                return scaledFont;
            scaledFont = new Font(_font.FontFamily, fontPx, _font.Style, GraphicsUnit.Pixel);
            _fontCache.Add(fontKey, scaledFont);
            return scaledFont;
        }

    }
}
