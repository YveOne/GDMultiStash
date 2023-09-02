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

        private static readonly string GDPATH = "Z:\\Games\\Steam\\steamapps\\common\\Grim Dawn";

        private static readonly string GD0_DB = Path.Combine(GDPATH, "database", "database.arz");
        private static readonly string GD1_DB = Path.Combine(GDPATH, "gdx1", "database", "GDX1.arz");
        private static readonly string GD2_DB = Path.Combine(GDPATH, "gdx2", "database", "GDX2.arz");
        private static readonly string GD0_RES = Path.Combine(GDPATH, "resources");
        private static readonly string GD1_RES = Path.Combine(GDPATH, "gdx1", "resources");
        private static readonly string GD2_RES = Path.Combine(GDPATH, "gdx2", "resources");

        private static readonly string CWD = Path.Combine(Environment.CurrentDirectory, "working");
        private static readonly int CWDLEN = CWD.Length + 1;
        private static readonly string CWD_RECORDS = Path.Combine(CWD, "records");
        private static readonly string CWD_TEXTURES = Path.Combine(CWD, "textures");

        private static readonly Dictionary<string, RegexPattern> RegexPatterns = new Dictionary<string, RegexPattern>()
        {
            // items
            ["bitmap"] = new RegexPattern {
                Pattern = new Regex(@"^[a-z]*bitmap[a-z]*,([^,]+\.tex),$", RegexOptions.Multiline | RegexOptions.IgnoreCase),
                Default = "", // has to be set in dbr
            },
            ["class"] = new RegexPattern {
                Pattern = new Regex(@"^class,([^,]+),$", RegexOptions.Multiline | RegexOptions.IgnoreCase),
                Default = "", // has to be set in dbr
            },
            ["levelRequirement"] = new RegexPattern {
                Pattern = new Regex(@"^levelRequirement,(\d+),$", RegexOptions.Multiline | RegexOptions.IgnoreCase),
                Default = "0"
            },
            ["itemClassification"] = new RegexPattern {
                Pattern = new Regex(@"^itemClassification,([^,]+),$", RegexOptions.Multiline | RegexOptions.IgnoreCase),
                Default = "None",
            },
            ["itemSetName"] = new RegexPattern
            {
                Pattern = new Regex(@"^itemSetName,([^,]+),$", RegexOptions.Multiline | RegexOptions.IgnoreCase),
                Default = "",
            },
            ["itemLevel"] = new RegexPattern {
                Pattern = new Regex(@"^itemLevel,(\d+),$", RegexOptions.Multiline | RegexOptions.IgnoreCase),
                Default = "0",
            },

            // itemsets
            ["setName"] = new RegexPattern {
                Pattern = new Regex(@"^setName,([^,]+),$", RegexOptions.Multiline | RegexOptions.IgnoreCase),
                Default = "", // has to be set in dbr
            },
            ["setMembers"] = new RegexPattern {
                Pattern = new Regex(@"^setMembers,([^,]+),$", RegexOptions.Multiline | RegexOptions.IgnoreCase),
                Default = "", // has to be set in dbr
            },
        };

        private static readonly Dictionary<string, string> CustomQualities = new Dictionary<string, string>() {
            ["records/endlessdungeon/items/a001_ring.dbr"] = "Legendary",
        };

        static void Main(string[] args)
        {
            if (!Directory.Exists(GDPATH))
            {
                Console.WriteLine("GDPATH not found: " + GDPATH);
                return;
            }
            ExtractFiles();

            var itemRecordInfos = new Dictionary<string, RecordInfo>();
            var foundBitmaps = new Dictionary<string, bool>();
            var bitmapInfos = new Dictionary<string, BitmapInfo>();
            var itemInfos = new SortedDictionary<string, ItemInfo>();
            var pngPaths = new Dictionary<string, string>();

            Console.WriteLine("Reading item records ...");
            foreach (string dbrFile in GetRecordFiles(CWD_RECORDS, true))
            {
                var record = GetRecordPath(dbrFile);
                if (record.StartsWith("records/level art/")) continue;
                if (record.StartsWith("records/proxies/")) continue;
                if (record.StartsWith("records/sandbox/")) continue;
                if (record.StartsWith("records/controllers/")) continue;
                if (record.StartsWith("records/sounds/")) continue;
                if (record.StartsWith("records/skills/")) continue;
                if (record.StartsWith("records/endlessdungeon/skills/")) continue;
                if (record.StartsWith("records/items/lootaffixes/")) continue;
                Console.WriteLine(record);

                var dbrData = DoRegex(File.ReadAllText(dbrFile), new string[] {
                    "bitmap", "class", "levelRequirement", "itemClassification", "itemSetName"
                });
                var _bitmap = dbrData["bitmap"];
                var _class = dbrData["class"];
                var _levelRequirement = dbrData["levelRequirement"];
                var _itemClassification = dbrData["itemClassification"];
                var _itemSetName = dbrData["itemSetName"];

                if (_bitmap == "") continue;
                if (_class == "") continue;

                if (CustomQualities.TryGetValue(record, out string q))
                    _itemClassification = q;

                foundBitmaps[_bitmap] = true;
                itemRecordInfos[record] = new RecordInfo()
                {
                    Bitmap = _bitmap,
                    Class = _class,
                    LevelRequirement = _levelRequirement,
                    ItemClassification = _itemClassification,
                    ItemSetName = _itemSetName,
                };
            }

            Console.WriteLine("Loading image sizes...");
            foreach (string bitmap in foundBitmaps.Keys)
            {
                Console.WriteLine(bitmap);
                var texName = $"{Path.GetFileName(bitmap)}.png";
                var texPath = Path.Combine(CWD_TEXTURES, texName);
                if (!File.Exists(texPath))
                {
                    Console.WriteLine("  - Skipped, not found");
                    continue;
                }
                using (Image png = Image.FromFile(texPath))
                {
                    if (png.Width % 32 != 0) continue;
                    if (png.Height % 32 != 0) continue;
                    var width = png.Width / 32;
                    var height = png.Height / 32;
                    if (width > 8 || height > 8) continue;
                    bitmapInfos[bitmap] = new BitmapInfo
                    {
                        Width = width,
                        Height = height,
                        File = texPath,
                    };
                }
            }

            Console.WriteLine("itemRecordInfo -> itemInfo");
            foreach (KeyValuePair<string, RecordInfo> kvp in itemRecordInfos)
            {
                var record = kvp.Key;
                var recordInfo = kvp.Value;
                var bitmap = recordInfo.Bitmap;

                if (!bitmapInfos.TryGetValue(bitmap, out BitmapInfo bitmapInfo)) continue;

                var iteminfo = new ItemInfo()
                {
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
                        System.Threading.Thread.Sleep(1);
                    }
                }
            }

            Console.WriteLine("Writing iteminfos.txt ...");
            string itemInfosFile = Environment.CurrentDirectory + "\\iteminfos.txt";
            if (File.Exists(itemInfosFile)) File.Delete(itemInfosFile);
            using (StreamWriter sw = File.AppendText(itemInfosFile))
            {
                sw.WriteLine($"//record|width|height|level|class|quality|bitmap|setrecord");
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
                        kvp.Value.RecordInfo.ItemClassification,
                        kvp.Value.BitmapName,
                        kvp.Value.RecordInfo.ItemSetName,
                    };
                    sw.WriteLine(string.Join("|", Array.ConvertAll(l, Convert.ToString)));
                    allClasses[kvp.Value.RecordInfo.Class] = true;
                    allQualities[kvp.Value.RecordInfo.ItemClassification] = true;
                }
                sw.WriteLine($"//Classes:");
                foreach (string s in allClasses.Keys)
                    sw.WriteLine($"// - {s}");
                sw.WriteLine($"//Qualities:");
                foreach (string s in allQualities.Keys)
                    sw.WriteLine($"// - {s}");
            }

            Console.WriteLine("Writing itemaffixes.txt ...");
            string itemAffixesFile = Environment.CurrentDirectory + "\\itemaffixes.txt";
            if (File.Exists(itemAffixesFile)) File.Delete(itemAffixesFile);
            using (StreamWriter sw = File.AppendText(itemAffixesFile))
            {
                sw.WriteLine($"//record|level|quality");
                var allQualities = new SortedDictionary<string, bool>();

                foreach (string dbrFile in GetRecordFiles(CWD_RECORDS, true))
                {
                    var record = GetRecordPath(dbrFile);
                    if (!(record.StartsWith("records/items/lootaffixes/")
                       || record.StartsWith("records/items/enchants/")
                       || record.StartsWith("records/items/materia/")
                       || record == "records/items/gearweapons/focus/b203c_focus.dbr"
                    )) continue;

                    var dbrData = DoRegex(File.ReadAllText(dbrFile), new string[] {
                        "levelRequirement", "itemClassification"
                    });
                    var _levelRequirement = dbrData["levelRequirement"];
                    var _itemClassification = dbrData["itemClassification"];

                    if (CustomQualities.TryGetValue(record, out string q))
                        _itemClassification = q;

                    allQualities[_itemClassification] = true;

                    sw.WriteLine(string.Join("|", new string[] {
                        record,
                        _levelRequirement,
                        _itemClassification,
                    }));
                }
                sw.WriteLine($"//Qualities:");
                foreach (string s in allQualities.Keys)
                    sw.WriteLine($"// - {s}");
            }

            Console.WriteLine("Writing itemsets.txt ...");
            string itemSetsFile = Environment.CurrentDirectory + "\\itemsets.txt";
            if (File.Exists(itemSetsFile)) File.Delete(itemSetsFile);
            using (StreamWriter sw = File.AppendText(itemSetsFile))
            {
                sw.WriteLine($"//record|setname|setitems");
                foreach (string dbrFile in GetRecordFiles(CWD_RECORDS, true))
                {
                    var record = GetRecordPath(dbrFile);
                    if (record.Contains("petbonus")
                    || !(record.StartsWith("records/items/lootsets/")
                       || record.StartsWith("records/storyelements/signs/")
                    )) continue;

                    var dbrData = DoRegex(File.ReadAllText(dbrFile), new string[] {
                        "setName", "setMembers"
                    });
                    var _setName = dbrData["setName"];
                    var _setMembers = dbrData["setMembers"];

                    if (_setName == "" || _setMembers == "")
                        continue;

                    sw.WriteLine(string.Join("|", new string[] {
                        record,
                        _setName,
                        _setMembers,
                    }));
                }
            }

            Console.WriteLine("DONE -  Press enter to exit");
            Console.ReadLine();
        }














        #region structs

        internal struct RecordInfo
        {
            public string Bitmap;
            public string Class;
            public string LevelRequirement;
            public string ItemClassification;
            public string ItemSetName;
        }

        internal struct BitmapInfo
        {
            public int Width;
            public int Height;
            public string File;
        }

        internal struct ItemInfo
        {
            public RecordInfo RecordInfo;
            public BitmapInfo BitmapInfo;
            public string BitmapName;
        }

        internal struct RegexPattern
        {
            public Regex Pattern;
            public string Default;
        }

        #endregion






        private static Dictionary<string, string> DoRegex(string text, string[] keys)
        {
            var p = new Dictionary<string, string>();
            foreach (string k in keys)
            {
                Match m = RegexPatterns[k].Pattern.Match(text);
                p[k] = m.Success
                    ? m.Groups[1].Value
                    : RegexPatterns[k].Default;
            }
            return p;
        }

        private static string GetRecordPath(string path) =>
            path.Substring(CWDLEN, path.Length - CWDLEN).Replace("\\", "/");

        private static string[] GetRecordFiles(string path, bool recursive = false)
        {
            SearchOption options = recursive
                ? SearchOption.AllDirectories
                : SearchOption.TopDirectoryOnly;
            return Directory.GetFiles(path, "*.dbr", options);
        }
















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

        private static void ExtractFiles()
        {
            if (!Directory.Exists(CWD))
            {
                Directory.CreateDirectory(CWD);
            }
            if (!Directory.Exists(CWD_RECORDS))
            {
                Console.WriteLine("Extracting records ...");
                RunProcess(GDPATH + "\\ArchiveTool.exe", "\"" + GD0_DB + "\" -database \"" + CWD + "\"");
                RunProcess(GDPATH + "\\ArchiveTool.exe", "\"" + GD1_DB + "\" -database \"" + CWD + "\"");
                RunProcess(GDPATH + "\\ArchiveTool.exe", "\"" + GD2_DB + "\" -database \"" + CWD + "\"");
            }
            if (!Directory.Exists(CWD_TEXTURES))
            {
                Directory.CreateDirectory(CWD_TEXTURES);
                Console.WriteLine("Extracting textures ...");
                foreach (string resPath in new string[] { GD0_RES, GD1_RES, GD2_RES })
                {
                    foreach (string arcFile in Directory.GetFiles(resPath, "*.arc"))
                    {
                        if (arcFile.EndsWith("Scripts.arc")
                            || arcFile.EndsWith("Shaders.arc"))
                            continue;
                        DDSImageReader.ExtractItemIcons(arcFile, CWD_TEXTURES);
                    }
                }
            }
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
