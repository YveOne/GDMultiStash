using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Reflection;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;
using System.Threading;
using System.Web.Script.Serialization;

namespace GDMultiStashUpdater
{
    public static class UpdaterAPI
    {



        private static readonly string latestUrl = @"https://api.github.com/repos/YveOne/GDMultiStash/releases/latest";

        private static string _newVersionUrl = null;
        private static string _newVersionName = null;

        public static string NewVersionName => _newVersionName;

        public class LatestReleaseDataAsset
        {
            public string browser_download_url { get; set; }
            public string name { get; set; }
        }

        public class LatestReleaseData
        {
            public string tag_name { get; set; }
            public string name { get; set; }
            public string published_at { get; set; }
            public IList<LatestReleaseDataAsset> assets { get; set; }
        }

        internal static void RunUpdate()
        {
            Console.WriteLine("Welcome to GDMultiStash Updater <3");
            Console.WriteLine("Getting data...");
            LatestReleaseData data = GetUpdateData();
            if (data == null)
            {
                Console.WriteLine("FAILED GETTING DATA FROM GITHUB!");
                Console.WriteLine("Updater will exit in 5 Seconds...");
                System.Threading.Thread.Sleep(5000);
                return;
            }
            string releaseName = data.name;
            string releaseLink = data.assets[0].browser_download_url;
            string releaseFilename = data.assets[0].name;
            Console.WriteLine(string.Format("Release Name: {0}", releaseName));
            Console.WriteLine(string.Format("Release Download Url: {0}", releaseLink));
            Console.WriteLine("Downloading...");
            if (File.Exists(releaseFilename)) File.Delete(releaseFilename);
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add("Accept", "*/*");
                wc.Headers.Add("User-Agent", "GDMultiStash");
                try
                {
                    wc.DownloadFile(releaseLink, releaseFilename);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("PRESS ANY KEY TO EXIT");
                    Console.Read();
                    return;
                }

            }
            Console.WriteLine("Extracting...");
            using (FileStream zipToOpen = new FileStream(releaseFilename, FileMode.Open))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        if (entry.Name == "") continue; // folder
                        if (entry.Name == "GDMultiStashUpdater.exe") continue; // because its currently running
                        Console.WriteLine("   " + entry.Name);
                        if (File.Exists(entry.Name)) File.Delete(entry.Name);
                        entry.ExtractToFile(entry.Name);
                    }
                }
            }
            File.Delete(releaseFilename);
            Console.WriteLine("Starting GDMultiStash.exe");
            new Thread(new ThreadStart(() => {
                var p = new Process();
                p.StartInfo = new ProcessStartInfo("GDMultiStash.exe")
                {
                    UseShellExecute = true
                };
                p.Start();
            })).Start();
            
        }

        public static LatestReleaseData GetUpdateData()
        {
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add("Accept", "application/vnd.github.v3+json");
                wc.Headers.Add("User-Agent", "GDMultiStash");
                string json = null;
                try
                {
                    json = wc.DownloadString(latestUrl);
                }
                catch (Exception)
                {
                    // TODO: show warning?
                    return null;
                }
                if (json == null)
                {
                    // TODO: show warning?
                    return null;
                }

                // fuck this JsonSerializer
                // it needs too much references
                // so much dll's just to deserialize one stupid thing
                //LatestReleaseData data = JsonSerializer.Deserialize<LatestReleaseData>(json);

                var deserializer = new JavaScriptSerializer();
                LatestReleaseData data = deserializer.Deserialize<LatestReleaseData>(json);

                if (data == null)
                {
                    // TODO: show warning?
                    return null;
                }
                return data;
            }
        }

        public static bool NewVersionAvailable()
        {
            LatestReleaseData data = GetUpdateData();
            if (data == null) return false;

            Match vMath = Regex.Match(data.tag_name, @"^v([\d\.]+)(.*?)$");
            if (!vMath.Success)
            {
                // TODO: show warning?
                return false;
            }

            string latestVersion = vMath.Groups[1].Value.Trim();
            string latestVersionSpecial = vMath.Groups[2].Value.Trim();

            Assembly asm = Assembly.LoadFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "GDMultiStash.exe")); //Assembly.GetExecutingAssembly();
            string currentVersion = ((AssemblyFileVersionAttribute)asm.GetCustomAttribute(typeof(AssemblyFileVersionAttribute))).Version;

            // cleanup latest version
            string[] verDig = new string[] { "0", "0", "0", "0" };
            latestVersion.Split('.').CopyTo(verDig, 0);
            latestVersion = string.Join(".", verDig);

            var result = new Version(currentVersion).CompareTo(new Version(latestVersion));

            // this was just for testing
            //Console.WriteLine(currentVersion);
            //Console.WriteLine(latestVersion);
            //_newVersionUrl = data.assets[0].browser_download_url;
            //_newVersionName = data.name;
            //return true;

            if (result > 0)
            {
                // current version is greater
                // user must be super special awesome
                return false;
            }
            else if (result < 0)
            {
                // new version available
                _newVersionUrl = data.assets[0].browser_download_url;
                _newVersionName = data.name;
                return true;
            }
            else
            {
                // this is latest version
                return false;
            }
        }





    }
}
