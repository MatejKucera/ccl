using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using NLog;
using IniParser;
using IniParser.Exceptions;
using IniParser.Model;
using CustomClientLauncher.Modules;
using CustomClientLauncher.Tools;
using NLog.Config;
using NLog.Targets;

namespace CustomClientLauncher
{
    class App
    {
        public static double VERSION = 3;

        // startup method, which starts the main thread and displays final error message
        static void Main(string[] args)
        {

            App app = new App();
            Boolean result = app.start();
            if (!result) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("The application ended with an error which prevents updating & starting your game.");
                Console.WriteLine("Please check CustomClientLauncher.log file for more information.");
                Console.WriteLine();
                Console.WriteLine("Press ENTER to exit the launcher.");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

        public Logger logger { get; private set; }
        public IniData config { get; private set; } 
        public XmlDocument source { get; set; } // is set in GetSourceModule
        public String language { get; set; } // is set in CheckModule

        // main class for whole project, runs the sequence of logics
        public Boolean start() {
            logger = LogManager.GetCurrentClassLogger();
            LogManager.Configuration = loggerConfiguration();
            logger.Info("Application started.");

            FileIniDataParser parser = new FileIniDataParser();
            try
            {
                config = parser.ReadFile("launcher.ini");
                logger.Info("Config successfully loaded.");
            }
            catch (ParsingException exception) {
                logger.Error("Configuration was not successfully loaded. Missing launcher.ini file? Dump: "+exception.ToString());
                return false;
            }

            // create list of modules in the execution order
            List<IModule> modules = new List<IModule>();
            modules.Add(new SourceModule());
            modules.Add(new WelcomeModule());
            modules.Add(new SelfRenameModule());
            modules.Add(new SelfUpdateModule());
            modules.Add(new CheckModule());
            modules.Add(new RealmlistModule());
            modules.Add(new CacheModule());
            modules.Add(new PatchesModule());
            modules.Add(new StartModule());

            foreach (IModule module in modules) {
                logger.Info("Starting " + module.ToString() +" module.");
                Boolean result = module.run(this);
                // if the result is false, launcher should be exited
                if (!result)
                {
                    logger.Error("Application ended at "+module.ToString()+" module.");
                    return false;
                }
            }

            return true;
        }

        public string getSourceValue(string xpath)
        {
            return this.source.SelectSingleNode(xpath).InnerText;
        }

        public LoggingConfiguration loggerConfiguration()
        {
            var config = new LoggingConfiguration();
            var fileTarget = new FileTarget();
            config.AddTarget("file", fileTarget);
            fileTarget.FileName = "${basedir}/launcher.log";
            fileTarget.Layout = "${message}";
            fileTarget.DeleteOldFileOnStartup = true;
            var rule = new LoggingRule("*", LogLevel.Debug, fileTarget);
            config.LoggingRules.Add(rule);
            return config;
        }
    }
}
