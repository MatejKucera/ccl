using CustomClientLauncher.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Xml;

namespace CustomClientLauncher.Modules
{
    class PatchesModule : IModule
    {
        // private attributes for download management
        private Boolean downloadLock = true;
        private int downloadStatus = 0;

        private App app;

        public bool run(App app)
        {
            this.app = app;

            Printer.info("Checking for patch updates");

            // Get all patches from source to the list
            XmlNodeList sourcePatchNodes = app.source.SelectNodes("/source/patches/patch");

            // Iterate through the patches
            foreach (XmlNode sourcePatch in sourcePatchNodes)
            {
                check(sourcePatch);                
            }

            return true;
        }

        // returns 
        private void check(XmlNode sourcePatch)
        {
            string filename = sourcePatch.SelectSingleNode("file").InnerText;
            string optionalString = sourcePatch.SelectSingleNode("optional").InnerText;
            string url = sourcePatch.SelectSingleNode("url").InnerText;
            string sourceChecksum = sourcePatch.SelectSingleNode("md5").InnerText;
            Printer.patchesInfo("Checking " + filename);

            // If the patch is optional and user doesn't want optional patches (config)
            if (app.config.Sections.GetSectionData("main").Keys.GetKeyData("download-optional-patches").Value == "false" && optionalString == "true")
            {
                Printer.resultNotice("Optional, won't be downloaded.");
                Printer.patchesInfo("Note: Update launcher configuration to download optional patches.", ConsoleColor.DarkGray);
                return;
            }

            if (!File.Exists("Data\\" + filename))
            {
                Printer.resultNotice("missing");
                this.download(filename, url);
            }
            else
            {
                string currentChecksum = md5("Data\\" + filename);

                // Current version of patch, no further activity
                if (sourceChecksum == currentChecksum)
                {
                    Printer.resultOk();
                    return;
                }
                else
                {
                    Printer.resultNotice(currentChecksum + "needs update");
                    this.download(filename, url);
                }
            }

            this.check(sourcePatch);
        }

        // download procedure
        private void download(string filename, string url)
        {
            Printer.patchesInfo("Downloading " + filename);

            Thread thread = new Thread(() =>
            {
                WebClient client = new WebClient();
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(downloadProgressChanged);
                client.DownloadFileCompleted += new AsyncCompletedEventHandler(downloadCompleted);
                Printer.resultNotice("0 %");
                client.DownloadFileAsync(new Uri(url), "Data\\" + filename);
            });

            thread.Start();

            // TODO async download with await instead of this mess
            this.downloadLock = true;
            while (this.downloadLock == true)
            {
                Thread.Sleep(500);
            }


        }

        // fired when download progress changed
        private void downloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            int status = e.ProgressPercentage;
            if (status > this.downloadStatus)
            {
                // TODO better progress bar
                string backspaces = "";
                if (this.downloadStatus < 10)
                {
                    backspaces = "\b\b\b\b";
                }
                else if (this.downloadStatus < 100)
                {
                    backspaces = "\b\b\b\b\b";
                }
                

                Printer.resultNotice(backspaces + " " + status.ToString() + " %");
            }
            this.downloadStatus = status;
        }

        // fired when download completed
        private void downloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Printer.resultNotice("\b\b\b\b\b\b\b 100 %");
            this.downloadLock = false;
        }

        // taken from stack overflow as it is, needs review
        private string md5(string filename)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                //File.ReadAllBytes(filename)
                var file = File.Open(filename, FileMode.Open);
                var buffer = md5.ComputeHash(file);
                var sb = new StringBuilder();
                for (int i = 0; i < buffer.Length; i++)
                {
                    sb.Append(buffer[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}
