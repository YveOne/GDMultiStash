using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;


namespace Utils
{
    namespace Extensions
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

            public static T DeepClone<T>(this T obj)
            {
                using (var ms = new MemoryStream())
                {
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(ms, obj);
                    ms.Position = 0;
                    return (T)formatter.Deserialize(ms);
                }
            }

            public static int Clamp(this int v, int min, int max)
            {
                return (v < min) ? min : (v > max) ? max : v;
            }

            public static bool InRange(this int v, int min, int max)
            {
                return v >= min && v <= max;
            }

        }

    }
}
