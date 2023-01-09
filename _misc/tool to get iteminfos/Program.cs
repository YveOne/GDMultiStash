using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using IAGrim.Parser.Arc;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
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

        private static string CWD = Path.Combine(Environment.CurrentDirectory, "working");

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

        struct RecordInfo
        {
            public string Bitmap;
            public string LevelRequirement;
            public string Class;
            public string Classification;
        }

        struct BitmapInfo
        {
            public int Width;
            public int Height;
            public string File;
        }

        struct ItemInfo
        {
            public RecordInfo RecordInfo;
            public BitmapInfo BitmapInfo;
            public string BitmapName;
        }

        static void Main(string[] args)
        {
            //if (args.Length == 0) return;
            argsList = new List<string>(args);
            string GDPATH = GetNextArg();


            GDPATH = "Z:\\Games\\Steam\\steamapps\\common\\Grim Dawn";
            if (!Directory.Exists(GDPATH))
            {
                Console.WriteLine("Directory not found: " + GDPATH);
                return;
            }

            int cwdLen = CWD.Length + 1;
            string recordsDir = Path.Combine(CWD, "records");
            string texturesDir = Path.Combine(CWD, "textures");

            string database0 = GDPATH + "\\database\\database.arz";
            string database1 = GDPATH + "\\gdx1\\database\\GDX1.arz";
            string database2 = GDPATH + "\\gdx2\\database\\GDX2.arz";
            string resources0 = GDPATH + "\\resources\\";
            string resources1 = GDPATH + "\\gdx1\\resources\\";
            string resources2 = GDPATH + "\\gdx2\\resources\\";


            //if (Directory.Exists(CWD)) Directory.Delete(CWD, true);
            if (!Directory.Exists(CWD))
            {
                Directory.CreateDirectory(CWD);
                Console.WriteLine("Extracting records ...");
                RunProcess(GDPATH + "\\ArchiveTool.exe", "\"" + database0 + "\" -database \"" + CWD + "\"");
                RunProcess(GDPATH + "\\ArchiveTool.exe", "\"" + database1 + "\" -database \"" + CWD + "\"");
                RunProcess(GDPATH + "\\ArchiveTool.exe", "\"" + database2 + "\" -database \"" + CWD + "\"");
                /*
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
                */
            }

            //if (Directory.Exists(texturesDir)) Directory.Delete(texturesDir, true);
            if (!Directory.Exists(texturesDir))
            {
                Directory.CreateDirectory(texturesDir);
                Console.WriteLine("Extracting textures ...");
                foreach (string resPath in new string[] { resources0, resources1, resources2 })
                {
                    foreach (string arcFile in Directory.GetFiles(resPath, "*.arc"))
                    {
                        DDSImageReader.ExtractItemIcons(arcFile, texturesDir);
                    }
                }
            }





            Dictionary<string, Regex> regex = new Dictionary<string, Regex>();
            regex["bitmap"] = new Regex(@"^[a-z]*bitmap[a-z]*,([^,]+\.tex),$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            regex["levelRequirement"] = new Regex(@"^levelRequirement,(\d+),$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            //regex["itemLevel"] = new Regex(@"^itemLevel,(\d+),$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            regex["itemClassification"] = new Regex(@"^itemClassification,([^,]+),$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            regex["class"] = new Regex(@"^class,([^,]+),$", RegexOptions.Multiline | RegexOptions.IgnoreCase);

            Dictionary<string, RecordInfo> recordInfos = new Dictionary<string, RecordInfo>();
            Dictionary<string, bool> foundBitmaps = new Dictionary<string, bool>();
            var bitmapInfos = new Dictionary<string, BitmapInfo>();
            SortedDictionary<string, ItemInfo> itemInfos = new SortedDictionary<string, ItemInfo>();
            var pngPaths = new Dictionary<string, string>();

            
            var customQualities = new Dictionary<string, string>();
            customQualities["records/endlessdungeon/items/a001_ring.dbr"] = "Legendary";


            Console.WriteLine("Reading item records ...");
            foreach (string dbrFile in Directory.GetFiles(recordsDir, "*.dbr", SearchOption.AllDirectories))
            {
                string record = dbrFile.Substring(cwdLen, dbrFile.Length - cwdLen).Replace("\\", "/");
                if (record.StartsWith("records/level art/")) continue;
                if (record.StartsWith("records/proxies/")) continue;
                if (record.StartsWith("records/sandbox/")) continue;
                if (record.StartsWith("records/controllers/")) continue;
                if (record.StartsWith("records/sounds/")) continue;
                if (record.StartsWith("records/skills/")) continue;
                if (record.StartsWith("records/endlessdungeon/skills/")) continue;
                if (record.StartsWith("records/items/lootaffixes/")) continue;
                Console.WriteLine(record);

                string dbrText = File.ReadAllText(dbrFile);
                Dictionary<string, Match> m = new Dictionary<string, Match>();
                foreach (var kvp in regex)
                    m[kvp.Key] = kvp.Value.Match(dbrText);

                if (!m["bitmap"].Success) continue;
                if (!m["class"].Success) continue;

                string recordBitmap = m["bitmap"].Groups[1].Value;
                string recordLevel = m["levelRequirement"].Success
                        ? m["levelRequirement"].Groups[1].Value
                        : "0";
                string recordClass = m["class"].Success
                        ? m["class"].Groups[1].Value
                        : "Unknown";
                string recordQuality = m["itemClassification"].Success
                        ? m["itemClassification"].Groups[1].Value
                        : "None";

                if (customQualities.TryGetValue(record, out string q))
                    recordQuality = q;

                RecordInfo recordInfo = new RecordInfo()
                {
                    Bitmap = recordBitmap,
                    LevelRequirement = recordLevel,
                    Class = recordClass,
                    Classification = recordQuality,
                };

                recordInfos[record] = recordInfo;
                foundBitmaps[recordInfo.Bitmap] = true;
            }





            Console.WriteLine("Loading image sizes...");
            foreach (string bitmap in foundBitmaps.Keys)
            {
                Console.WriteLine(bitmap);
                string texName = $"{Path.GetFileName(bitmap)}.png";
                string texPath = Path.Combine(texturesDir, texName);
                if (!File.Exists(texPath))
                {
                    Console.WriteLine("  - Skipped, not found");
                    continue;
                }
                using (Image png = Image.FromFile(texPath))
                {
                    if (png.Width % 32 != 0) continue;
                    if (png.Height % 32 != 0) continue;
                    int width = png.Width / 32;
                    int height = png.Height / 32;
                    if (width > 8 || height > 8) continue;
                    bitmapInfos[bitmap] = new BitmapInfo {
                        Width = width,
                        Height = height,
                        File = texPath,
                    };
                }
            }



            Console.WriteLine("...");
            foreach (KeyValuePair<string, RecordInfo> kvp in recordInfos)
            {
                string record = kvp.Key;
                RecordInfo recordInfo = kvp.Value;
                string bitmap = recordInfo.Bitmap;

                if (!bitmapInfos.TryGetValue(bitmap, out BitmapInfo bitmapInfo)) continue;

                ItemInfo iteminfo = new ItemInfo() {
                    RecordInfo = recordInfo,
                    BitmapInfo = bitmapInfo,
                    BitmapName = Path.ChangeExtension(Path.GetFileName(bitmap), ".png"),
                };
                itemInfos[record] = iteminfo;
                pngPaths[iteminfo.BitmapName] = bitmapInfo.File;
                Console.WriteLine($"{record} = size:{iteminfo.BitmapInfo.Width}/{iteminfo.BitmapInfo.Height} level:{iteminfo.RecordInfo.LevelRequirement}");
                System.Threading.Thread.Sleep(1);
            }



            Console.WriteLine("Creating itemtextures.zip ...");
            if (File.Exists("textures.zip")) File.Delete("itemtextures.zip");
            using (FileStream zipStream = new FileStream("itemtextures.zip", FileMode.Create))
            {
                using (ZipArchive zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Update))
                {
                    ImageConverter converter = new ImageConverter();
                    foreach (var entry in pngPaths)
                    {
                        string pngName = entry.Key;
                        string pngPath = entry.Value;
                        Console.WriteLine($"Zipping {pngName}");
                        ZipArchiveEntry zipEntry = zipArchive.CreateEntry(pngName);
                        
                        using (Image pngOriginal = Image.FromFile(pngPath))
                        {
                            if (pngOriginal != null)
                            {
                                using (Image pngResized = ResizeImage(pngOriginal, 0.5f))
                                {
                                    using (StreamWriter writer = new StreamWriter(zipEntry.Open()))
                                    {
                                        byte[] buffer = (byte[])converter.ConvertTo(pngResized, typeof(byte[]));
                                        writer.BaseStream.Write(buffer, 0, buffer.Length);
                                    }
                                }
                            }
                        }
                        //byte[] buffer = File.ReadAllBytes(entry.Key);
                        System.Threading.Thread.Sleep(1);
                    }
                }
            }




            string itemInfosFile = Environment.CurrentDirectory + "\\iteminfos.txt";
            if (File.Exists(itemInfosFile)) File.Delete(itemInfosFile);
            using (StreamWriter sw = File.AppendText(itemInfosFile))
            {
                sw.WriteLine($"//record|width|height|level|class|quality|bitmap");
                var allClasses = new SortedDictionary<string, bool>();
                var allQualities = new SortedDictionary<string, bool>();
                foreach (KeyValuePair<string, ItemInfo> kvp in itemInfos)
                {
                    object[] l = new object[] {
                        kvp.Key,
                        kvp.Value.BitmapInfo.Width,
                        kvp.Value.BitmapInfo.Height,
                        kvp.Value.RecordInfo.LevelRequirement,
                        kvp.Value.RecordInfo.Class,
                        kvp.Value.RecordInfo.Classification,
                        kvp.Value.BitmapName,
                    };
                    sw.WriteLine(string.Join("|", Array.ConvertAll(l, Convert.ToString)));
                    allClasses[kvp.Value.RecordInfo.Class] = true;
                    allQualities[kvp.Value.RecordInfo.Classification] = true;
                }
                sw.WriteLine($"//Classes:");
                foreach (string s in allClasses.Keys)
                    sw.WriteLine($"// - {s}");
                sw.WriteLine($"//Qualities:");
                foreach (string s in allQualities.Keys)
                    sw.WriteLine($"// - {s}");
            }





            Console.WriteLine("Reading affixes&enchants records ...");
            string itemAffixesFile = Environment.CurrentDirectory + "\\itemaffixes.txt";
            if (File.Exists(itemAffixesFile)) File.Delete(itemAffixesFile);
            using (StreamWriter sw = File.AppendText(itemAffixesFile))
            {
                sw.WriteLine($"//record|level|quality");
                var allQualities = new SortedDictionary<string, bool>();
                foreach (string dbrFile in Directory.GetFiles(recordsDir, "*.dbr", SearchOption.AllDirectories))
                {
                    string record = dbrFile.Substring(cwdLen, dbrFile.Length - cwdLen).Replace("\\", "/");
                    if (!(record.StartsWith("records/items/lootaffixes/")
                       || record.StartsWith("records/items/enchants/")
                       || record.StartsWith("records/items/materia/")
                    )) continue;
                    Console.WriteLine(record);
                    
                    string dbrText = File.ReadAllText(dbrFile);
                    Dictionary<string, Match> m = new Dictionary<string, Match>();
                    foreach (var kvp in regex)
                        m[kvp.Key] = kvp.Value.Match(dbrText);

                    string recordLevel = m["levelRequirement"].Success
                        ? m["levelRequirement"].Groups[1].Value
                        : "0";
                    string recordQuality = m["itemClassification"].Success
                            ? m["itemClassification"].Groups[1].Value
                            : "None";

                    if (customQualities.TryGetValue(record, out string q))
                        recordQuality = q;


                    object[] l = new object[] {
                        record,
                        recordLevel,
                        recordQuality,
                    };
                    sw.WriteLine(string.Join("|", Array.ConvertAll(l, Convert.ToString)));
                    allQualities[recordQuality] = true;
                }
                sw.WriteLine($"//Qualities:");
                foreach (string s in allQualities.Keys)
                    sw.WriteLine($"// - {s}");
            }
















            //Directory.Delete(CWD, true);
            Console.WriteLine("DONE -  Press enter to exit");
            Console.ReadLine();
        }

        public static Image ResizeImage(Image img, float f)
        {
            int width = Math.Max(1, (int)((float)img.Width * f));
            int height = Math.Max(1, (int)((float)img.Height * f));
            var newImg = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            Graphics g = null;
            try
            {
                g = Graphics.FromImage(newImg);
                g.SmoothingMode = SmoothingMode.None;
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.CompositingQuality = CompositingQuality.AssumeLinear;
                g.Clear(Color.Transparent);
                g.DrawImage(img, 0, 0, width, height);
            }
            finally
            {
                if (g != null)
                {
                    g.Dispose();
                }
            }
            return newImg;
        }

    }
}
