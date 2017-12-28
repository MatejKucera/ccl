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

        private static string BATCH_UPDATER = "cclupdate.bat";

        public Boolean run(App app)
        {
            Printer.info("Checking for CCL update");

            double currentVersion = App.VERSION;
            double availableVersion = Convert.ToDouble(app.source.SelectSingleNode("/source/launcher/currentVersion").InnerText);

            String exeName = app.source.SelectSingleNode("/source/launcher/exe").InnerText;
            String exeNameTemp = exeName + "_temp";

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
                    client.DownloadFile(app.source.SelectSingleNode("/source/launcher/url").InnerText, exeNameTemp);
                }
                catch (WebException exception)
                {
                    app.logger.Error("New version of the CCL was not found. Dump: " + exception.ToString());
                    return false;
                }
            }

            Printer.info("Creating update script");
            int pid = Process.GetCurrentProcess().Id;

            BatchFactory.moverBatch(pid.ToString(), exeNameTemp, exeName, SelfUpdateModule.BATCH_UPDATER);

            Console.ReadLine();
            return false;
        }
    }
}
