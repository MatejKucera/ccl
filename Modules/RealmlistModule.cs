using CustomClientLauncher.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomClientLauncher.Modules
{
    class RealmlistModule : IModule
    {
        public bool run(App app)
        {
            Printer.info("Checking realmlist.wtf");

            string realmlistPath = "Data\\" + app.language + "\\realmlist.wtf";
            string realmlistValue = "set realmlist " + app.source.SelectSingleNode("/source/server/realmlist").InnerText;

            try {
                if (!File.Exists(realmlistPath))
                {
                    // Create new realmlist
                    Printer.resultNotice("missing");
                    Printer.info("Creating realmlist.wtf");

                    File.WriteAllText(realmlistPath, realmlistValue);

                    Printer.resultOk();
                    return true;
                }
                else if (File.ReadAllText(realmlistPath) != realmlistValue)
                {
                    // Delete and create a correct one
                    Printer.resultNotice("invalid");
                    Printer.info("Creating valid realmlist.wtf");

                    File.Delete(realmlistPath);
                    File.WriteAllText(realmlistPath, realmlistValue);

                    Printer.resultOk();
                    return true;
                }
                else
                {
                    Printer.resultOk();
                    return true;
                }
            }
            catch (UnauthorizedAccessException exception)
            {
                Printer.resultError("error, could not modify realmlist.wtf file");
                app.logger.Error("Realmlist file is probably read only. Dump: " + exception.ToString());
                return false;
            }
        }
    }
}
