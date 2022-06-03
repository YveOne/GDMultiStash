using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Utils
{


    internal static class FileUtils
    {

        public static string GetFileHash(string filepath)
        {
            try
            {
                if (!File.Exists(filepath)) return null;
                byte[] hash;
                using (var inputStream = File.Open(filepath, FileMode.Open))
                {
                    var md5 = System.Security.Cryptography.MD5.Create();
                    hash = md5.ComputeHash(inputStream);
                }
                return string.Concat(hash.Select(x => x.ToString("X2")));
            }
            catch(Exception)
            {
                return null;
            }
        }

        public static long GetLastWriteTicks(string filePath)
        {
            if (!File.Exists(filePath)) return 0;
            return (new FileInfo(filePath)).LastWriteTime.Ticks;
        }

    }

}