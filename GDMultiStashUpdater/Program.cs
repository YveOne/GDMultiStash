using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMultiStashUpdater
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length >= 1)
            {
                UpdaterAPI.RunUpdate(args[0]);
            }
            else
            {
                UpdaterAPI.RunUpdate();
            }
        }
    }
}
