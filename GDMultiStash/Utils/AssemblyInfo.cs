﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Utils
{
    internal class AssemblyInfo
    {
        public string AppName { get; private set; }
        public string Copyright { get; private set; }
        public string Version { get; private set; }
        public string Description { get; private set; }
        public string Location { get; private set; }

        public AssemblyInfo()
        {
            AppName = Assembly.GetExecutingAssembly().GetName().Name;
            Assembly asm = Assembly.GetExecutingAssembly();
            Location = asm.Location;
            Copyright = ((AssemblyCopyrightAttribute)asm.GetCustomAttribute(typeof(AssemblyCopyrightAttribute))).Copyright;
            Version = ((AssemblyFileVersionAttribute)asm.GetCustomAttribute(typeof(AssemblyFileVersionAttribute))).Version;
            Description = ((AssemblyDescriptionAttribute)asm.GetCustomAttribute(typeof(AssemblyDescriptionAttribute))).Description;
        }


    }
}
