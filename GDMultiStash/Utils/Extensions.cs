using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GDMultiStash
{
    public static class Extensions
    {


        public static string Format(this string str, params string[] args)
        {
            return string.Format(str, args);
        }



    }
}
