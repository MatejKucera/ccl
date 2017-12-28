using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomClientLauncher.Tools
{
    class BatchFactory
    {
        public static string WAIT_TIME = "1";

        public static void moverBatch(String pid, String moveFrom, String moveTo, String batchName)
        {
            // delete script file if exists
            if (File.Exists(batchName))
            {
                File.Delete(batchName);
            }

            // create script file
            String script = "";
            script += "taskkill /F /PID " + pid + "\r\n";
            script += "timeout "+BatchFactory.WAIT_TIME+" >nul\r\n";
            script += "move " + moveFrom + " " + moveTo + "\r\n";
            script += "timeout " + BatchFactory.WAIT_TIME + " >nul\r\n";
            script += "Start \"\" " + moveTo + "\r\n";
            script += "del /F " + batchName + "\r\n";
            script += "exit \r\n";
            File.WriteAllText(batchName, script);

            // run script
            Printer.info("This program will be terminated");
            Process.Start(batchName);
        }
    }
}
