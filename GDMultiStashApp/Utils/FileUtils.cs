using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;

namespace Utils
{

    internal static class FileUtils
    {

        public static string GetFileHash(string filepath)
        {
            try
            {

                byte[] hash;
                using (var stream = new BufferedStream(File.OpenRead(filepath), 1048576 * 1))
                {
                    hash = System.Security.Cryptography.HashAlgorithm.Create().ComputeHash(stream);
                }
                return string.Concat(hash.Select(x => x.ToString("X2")));
            }
            catch (IOException e)
            {
                return null;
            }
        }

        public static bool FileIsLocked(string filePath)
        {
            try
            {
                File.Open(filePath, FileMode.Open).Close();
                return false;
            }
            catch (IOException e)
            {
                int errorCode = Marshal.GetHRForException(e) & ((1 << 16) - 1);
                if (errorCode == 32 || errorCode == 33) return true;
                throw;
            }
        }

    }

}
