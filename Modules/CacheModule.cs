using CustomClientLauncher.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomClientLauncher.Modules
{
    class CacheModule : IModule
    {
        public bool run(App app)
        {
            Printer.info("Deleting cache");
            if (Directory.Exists("Cache"))
            {
                try
                {
                    Directory.Delete("Cache", true);
                }
                catch (UnauthorizedAccessException exception)
                {
                    Printer.resultError("error, could not delete cache");
                    app.logger.Error("Cache directory is probably read only. Dump: " + exception.ToString());
                    return false;
                }
            }

            Printer.resultOk();
            return true;
        }
    }
}
