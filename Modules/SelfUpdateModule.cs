using CustomClientLauncher.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CustomClientLauncher.Modules
{
    class SelfUpdateModule : IModule
    {
        private static string CCL_EXE = "CustomClientLauncher.exe";
        private static string CCL_EXE_TEMP = "CustomClientLauncher.exe_temp";

        private static string BATCH_UPDATER = "cclupdate.bat";

        public Boolean run(App app)
        {
            Printer.info("Checking for CCL update");

            double currentVersion = App.VERSION;
            double availableVersion = Convert.ToDouble(app.source.SelectSingleNode("/source/launcher/currentVersion").InnerText);

            // if there is no new version or allowUpdate in source file is set to false
            if (availableVersion == currentVersion || app.source.SelectSingleNode("/source/launcher/allowUpdate").InnerText.Equals("false"))
            {
                Printer.resultOk();
                return true;
            }

            // new version available
            Printer.resultNotice("new version available");

            Printer.info("Downloading new version");
            using (WebClient client = new WebClient())
            {
                try { 
                    client.DownloadFile(app.source.SelectSingleNode("/source/launcher/url").InnerText, SelfUpdateModule.CCL_EXE_TEMP);
                }
                catch (WebException exception)
                {
                    app.logger.Error("New version of the CCL was not found. Dump: " + exception.ToString());
                    return false;
                }
            }

            Printer.info("Creating update script");
            int pid = Process.GetCurrentProcess().Id;

            // delete script file if exists
            if (File.Exists(SelfUpdateModule.BATCH_UPDATER))
            {
                File.Delete(SelfUpdateModule.BATCH_UPDATER);
            }

            // create script file
            String script = "";
            script += "taskkill /F /PID "+pid.ToString() + "\r\n";
            script += "move "+SelfUpdateModule.CCL_EXE_TEMP+" "+SelfUpdateModule.CCL_EXE+"\r\n";
            script += SelfUpdateModule.CCL_EXE+"\r\n";
            script += "del /F "+SelfUpdateModule.BATCH_UPDATER+"\r\n";
            File.WriteAllText(SelfUpdateModule.BATCH_UPDATER,script);

            // run script
            Printer.info("This program will be terminated");
            Process.Start(SelfUpdateModule.BATCH_UPDATER);

            return false;
        }
    }
}
