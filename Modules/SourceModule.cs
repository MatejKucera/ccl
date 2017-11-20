using CustomClientLauncher.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CustomClientLauncher.Modules
{
    class SourceModule : IModule
    {
        public Boolean run(App app)
        {
            // gets source file url from config
            string sourceFile = app.config.Sections.GetSectionData("main").Keys.GetKeyData("url").Value;

            // download source xml
            WebClient client = new WebClient();
            Stream stream = null;
            try
            {
                stream = client.OpenRead(sourceFile);
            }
            catch (WebException exception)
            {
                app.logger.Error("Could not load sources file. Dump: " + exception.ToString());
                return false;
            }
            StreamReader reader = new StreamReader(stream);
            String xmlContent = reader.ReadToEnd();

            // load xml source
            XmlDocument source = new XmlDocument();
            source.LoadXml(xmlContent);

            // Set source to app
            app.source = source;

            return true;
        }
    }
}
