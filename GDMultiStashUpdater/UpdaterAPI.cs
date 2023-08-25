﻿using System;
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
            if (!NewVersionAvailable())
            {
                Console.WriteLine("Your GDMultiStash is up to date - you rock!");
                Console.WriteLine("Updater will exit in 5 Seconds...");
                Thread.Sleep(5000);
                return;
            }

            Console.WriteLine("Welcome to GDMultiStash Updater <3");
            Console.WriteLine("Getting data...");
            LatestReleaseData data = GetUpdateData();
            if (data == null)
            {
                Console.WriteLine("FAILED GETTING DATA FROM GITHUB!");
                Console.WriteLine("Updater will exit in 5 Seconds...");
                Thread.Sleep(5000);
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
                    Directory.CreateDirectory("Update");
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        if (entry.Name == "") continue; // folder
                        try
                        {
                            entry.ExtractToFile(Path.Combine("Update", entry.Name));
                        }
                        catch(Exception)
                        {
                            // file locked
                        }
                    }
                }
            }
            File.Delete(releaseFilename);

            Console.WriteLine("Running Update.bat");
            File.WriteAllText("Update.bat", Properties.Resources.Update);

            using (Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Normal,
                    FileName = "Update.bat",
                    UseShellExecute = false,
                }
            })
            {
                process.Start();
            }

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

        public static int CompareCurrentVersion(string v)
        {

            Assembly asm = Assembly.LoadFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "GDMultiStash.exe"));
            string currentVersion = ((AssemblyFileVersionAttribute)asm.GetCustomAttribute(typeof(AssemblyFileVersionAttribute))).Version;

            // cleanup latest version
            string[] verDig = new string[] { "0", "0", "0", "0" };
            v.Split('.').CopyTo(verDig, 0);
            v = string.Join(".", verDig);

            return new Version(currentVersion).CompareTo(new Version(v));
        }

        public static bool NewVersionAvailable()
        {

            LatestReleaseData data = GetUpdateData();
            if (data == null) return false;

            Match vMatch = Regex.Match(data.tag_name, @"^v([\d\.]+)(.*?)$");
            if (!vMatch.Success)
            {
                // TODO: show warning?
                return false;
            }

            string latestVersion = vMatch.Groups[1].Value.Trim();
            string latestVersionSpecial = vMatch.Groups[2].Value.Trim();
            int result = CompareCurrentVersion(latestVersion);

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
