using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMultiStash
{
    internal class Cleanup
    {
        private static void DeleteFile(string file)
        {
            file = System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, file);
            if (System.IO.File.Exists(file)) System.IO.File.Delete(file);
        }

        public static void DoCleanup()
        {
        }

    }
}
