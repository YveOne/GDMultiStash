using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace D3DHook.Overlay.Utils.Extensions
{
    public static class Extensions
    {
        public static List<T> RemoveAndGetRange<T>(this List<T> list, int index, int count)
        {
            lock (list)
            {
                List<T> result = new List<T>();
                for (int i = 1; i <= count; i += 1)
                {
                    if (list.Count <= index) return result;
                    result.Add(list[index]);
                    list.RemoveAt(index);
                }
                return result;
            }
        }

    }
}
