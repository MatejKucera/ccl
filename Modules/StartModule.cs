using CustomClientLauncher.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomClientLauncher.Modules
{
    class StartModule : IModule
    {

        public bool run(App app)
        {
            Printer.info("Starting WoW");

            // Get exe filename from config and execute it
            string wow = app.config.Sections.GetSectionData("main").Keys.GetKeyData("exe").Value;
            Process process = Process.Start(wow);
            if (this.isRunning(process))
            {
                Printer.resultOk();
                return true;
            }
            else
            {
                app.logger.Error("Process could not be started.");
                Printer.resultError("Process could not be started.");
                return false;
            }
        }

        private Boolean isRunning(Process process)
        {
            try
            {
                Process.GetProcessById(process.Id);
            }
            catch (ArgumentException exception)
            {
                return false;
            }
            return true;
        }
    }
}
