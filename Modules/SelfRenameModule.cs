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
    class SelfRenameModule : IModule
    {

        private static string BATCH_RENAMER = "cclrename.bat";

        public Boolean run(App app)
        {
            // This module has no printable output

            String exeNameActual = AppDomain.CurrentDomain.FriendlyName;
            String exeNameDesired = app.getSourceValue("/source/launcher/exe");

            // if there is no new version or allowUpdate in source file is set to false
            if (exeNameActual.Equals(exeNameDesired))
            {
                return true;
            }

            // new version available
            Printer.info("Launcher needs to be renamed");
            Printer.info("Creating rename script");
            int pid = Process.GetCurrentProcess().Id;

            BatchFactory.moverBatch(pid.ToString(), exeNameActual, exeNameDesired, SelfRenameModule.BATCH_RENAMER);

            Console.ReadLine();
            return false;
        }
    }
}
