using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using IAGrim.Parser.Arc;
using System.Drawing;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ConsoleApp1
{
    internal class Program
    {
        private static List<string> argsList;

        private static string GetNextArg()
        {
            if (argsList.Count == 0) return null;
            string ret = argsList[0];
            argsList.RemoveAt(0);
            return ret;
        }

        private static string CWD = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\working";

        private static void RunProcess(string FileName, string Arguments)
        {
            try
            {
                using (Process p = new Process()
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = FileName,
                        Arguments = Arguments,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    }
                })
                {
                    p.Start();
                    while (!p.StandardOutput.EndOfStream)
                        Console.WriteLine(p.StandardOutput.ReadLine());
                    
                    p.WaitForExit();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        struct ItemInfo
        {
            public Size Size;
            public int Level;
        }

        struct RecordInfo
        {
            public string bitmap;
            public string levelRequirement;
        }


        static void Main(string[] args)
        {
            
            if (args.Length == 0) return;
            argsList = new List<string>(args);
            string GDPATH = GetNextArg();
            

            //string GDPATH = "Z:\\Games\\Steam\\steamapps\\common\\Grim Dawn";
            if (!Directory.Exists(GDPATH))
            {
                Console.WriteLine("Directory not found: " + GDPATH);
                return;
            }

            //if (Directory.Exists(CWD)) Directory.Delete(CWD, true);
            if (!Directory.Exists(CWD))
            {
                Directory.CreateDirectory(CWD);

                string database0 = GDPATH + "\\database\\database.arz";
                string database1 = GDPATH + "\\gdx1\\database\\GDX1.arz";
                string database2 = GDPATH + "\\gdx2\\database\\GDX2.arz";
                RunProcess(GDPATH + "\\ArchiveTool.exe", "\"" + database0 + "\" -database \"" + CWD + "\"");
                RunProcess(GDPATH + "\\ArchiveTool.exe", "\"" + database1 + "\" -database \"" + CWD + "\"");
                RunProcess(GDPATH + "\\ArchiveTool.exe", "\"" + database2 + "\" -database \"" + CWD + "\"");

                string resources0 = GDPATH + "\\resources\\";
                string resources1 = GDPATH + "\\gdx1\\resources\\";
                string resources2 = GDPATH + "\\gdx2\\resources\\";

                foreach (string resPath in new string[] { resources0, resources1, resources2 })
                {
                    foreach (string arcFile in Directory.GetFiles(resPath, "*.arc"))
                    {
                        if (arcFile.ToLower().EndsWith("Conversations.arc")) continue;
                        if (arcFile.ToLower().EndsWith("Sound.arc")) continue;
                        if (arcFile.ToLower().EndsWith("Fonts.arc")) continue;

                        RunProcess(GDPATH + "\\ArchiveTool.exe", "\"" + arcFile + "\" -extract \"" + CWD + "\"");
                    }
                }
            }

            Dictionary<string, Regex> regex = new Dictionary<string, Regex>();
            regex["bitmap"] = new Regex(@"^[a-z]*bitmap[a-z]*,([^,]+),$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            regex["levelRequirement"] = new Regex(@"^levelRequirement,(\d+),$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            regex["itemLevel"] = new Regex(@"^itemLevel,(\d+),$", RegexOptions.Multiline | RegexOptions.IgnoreCase);

            int cwdLen = CWD.Length + 1;

            Dictionary<string, RecordInfo> recordInfos = new Dictionary<string, RecordInfo>();
            Dictionary<string, bool> textureFilesList = new Dictionary<string, bool>();
            Dictionary<string, Size> bitmapSizes = new Dictionary<string, Size>();
            SortedDictionary<string, ItemInfo> itemInfos = new SortedDictionary<string, ItemInfo>();

            Console.WriteLine("Reading records...");
            foreach (string dbrFile in Directory.GetFiles(CWD + "\\records", "*.dbr", SearchOption.AllDirectories))
            {
                string record = dbrFile.Substring(cwdLen, dbrFile.Length - cwdLen - 4).Replace("\\", "/");
                Console.WriteLine(record);

                string dbrText = File.ReadAllText(dbrFile);

                Dictionary<string, Match> m = new Dictionary<string, Match>();
                foreach (var kvp in regex)
                    m[kvp.Key] = kvp.Value.Match(dbrText);

                if (!m["bitmap"].Success) continue;
                if (!m["bitmap"].Groups[1].Value.EndsWith(".tex")) continue;

                RecordInfo recordInfo = new RecordInfo()
                {
                    bitmap = m["bitmap"].Groups[1].Value,
                    levelRequirement = m["levelRequirement"].Success
                    ? m["levelRequirement"].Groups[1].Value
                    : m["itemLevel"].Success
                    ? m["itemLevel"].Groups[1].Value
                    : "0",
                };

                recordInfos[record] = recordInfo;
                textureFilesList[recordInfo.bitmap] = true;
            }
            Console.WriteLine("... Done");

            Console.WriteLine("Reading sizes...");
            foreach (string texRecord in textureFilesList.Keys)
            {
                string texFile = string.Format("{0}\\{1}", CWD, texRecord);
                if (!File.Exists(texFile)) continue;

                string texFileClean = texFile.Substring(cwdLen, texFile.Length - cwdLen).Replace("\\", "/");
                Console.WriteLine(texFileClean);

                try
                {
                    Image img = DDSImageReader.ExtractImage(File.ReadAllBytes(texFile));
                    if (img != null)
                    {
                        if (img.Width % 32 != 0) continue;
                        if (img.Height % 32 != 0) continue;
                        int width = img.Width / 32;
                        int height = img.Height / 32;
                        if (width > 8 || height > 8) continue;
                        bitmapSizes[texFileClean] = new Size(width, height);
                        img.Dispose();
                    }
                    else
                    {
                        Console.WriteLine("Error in image: " + texFile);
                        Console.ReadLine();
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Error in image: " + texFile);
                    Console.ReadLine();
                }
            }
            Console.WriteLine("... Done");








            Console.WriteLine("...");
            foreach (KeyValuePair<string, RecordInfo> kvp in recordInfos)
            {
                string record = kvp.Key;
                if (!bitmapSizes.TryGetValue(kvp.Value.bitmap, out Size size)) continue;
                ItemInfo iteminfo = new ItemInfo() {
                    Size = size,
                    Level = int.Parse(kvp.Value.levelRequirement),
                };
                itemInfos[record] = iteminfo;
                Console.WriteLine($"{record} = size:{iteminfo.Size.Width}/{iteminfo.Size.Height} level:{iteminfo.Level}");
            }








            string listFile = Environment.CurrentDirectory + "\\iteminfos.txt";
            if (File.Exists(listFile)) File.Delete(listFile);
            File.WriteAllText(listFile, "");
            using (StreamWriter sw = File.AppendText(listFile))
            {
                sw.WriteLine($"//record:width:height:level");
                foreach (KeyValuePair<string, ItemInfo> kvp in itemInfos)
                {
                    sw.WriteLine($"{kvp.Key}:{kvp.Value.Size.Width}:{kvp.Value.Size.Height}:{kvp.Value.Level}");
                }
            }



            //Directory.Delete(CWD, true);
            Console.WriteLine("DONE -  Press enter to exit");
            Console.ReadLine();
        }
    }
}
