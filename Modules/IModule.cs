using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomClientLauncher.Modules
{
    interface IModule
    {
        Boolean run(App app);
    }
}
