using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Utils
{
    internal class AssemblyInfo
    {
        private string _appname;
        private string _copyright;
        private string _version;

        public AssemblyInfo()
        {
            _appname = Assembly.GetExecutingAssembly().GetName().Name;
            Assembly asm = Assembly.GetExecutingAssembly();
            _copyright = ((AssemblyCopyrightAttribute)asm.GetCustomAttribute(typeof(AssemblyCopyrightAttribute))).Copyright;
            _version = ((AssemblyFileVersionAttribute)asm.GetCustomAttribute(typeof(AssemblyFileVersionAttribute))).Version;
        }

        public string AppName => _appname;
        public string Copyright => _copyright;
        public string Version => _version;

    }
}
