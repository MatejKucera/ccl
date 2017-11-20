using CustomClientLauncher.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomClientLauncher.Modules
{
    class CheckModule : IModule
    {
        public bool run(App app)
        {
            // Get language version (TODO - only prototype, needs to be done better - more languages support)
            Printer.info("Detecting language version");
            if (Directory.Exists("Data\\enGB"))
            {
                app.language = "enGB";
            }
            else if (Directory.Exists("Data\\enUS"))
            {
                app.language = "enUS";
            }

            // If language is not set, throw a error
            if (app.language == null)
            {
                app.logger.Error("No language detected, can't continue. Probably other version than enGB or enUS which are not supported yet.");
                Printer.resultError("no language detected");
                return false;
            }

            Printer.resultOk(app.language);

            // List of files and directories that have to be present to proceed
            List<String> mandatoryFiles = new List<String>();
            mandatoryFiles.Add(app.config.Sections.GetSectionData("main").Keys.GetKeyData("exe").Value);
            mandatoryFiles.Add("Data");
            mandatoryFiles.Add("Data\\"+app.language);
            /*mandatoryFiles.Add("Data\\common.MPQ");
            mandatoryFiles.Add("Data\\common-2.MPQ");
            mandatoryFiles.Add("Data\\expansion.MPQ");
            mandatoryFiles.Add("Data\\lichking.MPQ");
            mandatoryFiles.Add("Data\\patch.MPQ");
            mandatoryFiles.Add("Data\\patch-2.MPQ");
            mandatoryFiles.Add("Data\\patch-3.MPQ");*/

            // Check those files
            Printer.info("Checking files");
            foreach (String file in mandatoryFiles) {
                if (!File.Exists(file) && !Directory.Exists(file))
                {
                    app.logger.Error("Missing one of mandatory game files - '"+file+"'. Can't proceed.");
                    Printer.resultError("missing "+file);
                    return false;
                }
            }

            Printer.resultOk();

            return true;
        }
    }
}
