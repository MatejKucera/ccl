using CustomClientLauncher.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomClientLauncher.Modules
{
    class WelcomeModule : IModule
    {
        public bool run(App app)
        {
            // print welcome text
            //   source is already loaded, so it can be used
            Console.Title = app.getSourceValue("/source/server/title");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(app.getSourceValue("/source/server/title"));
            Console.WriteLine(" - Custom Client Launcher, v" + App.VERSION);
            Console.WriteLine(" - Web:  " + app.getSourceValue("/source/server/web"));
            Console.WriteLine(" - Launcher:  " + app.getSourceValue("/source/server/help"));
            Console.WriteLine("");

            return true;
        }
    }
}
