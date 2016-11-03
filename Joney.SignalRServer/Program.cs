using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using System.Configuration;
using System.ServiceProcess;

namespace Joney.SignalRServer
{
    class Program
    {
        private static readonly string hostUrl = ConfigurationManager.AppSettings["HostUrl"].ToString();
        static void Main(string[] args)
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new ServiceSR()
            };
            ServiceBase.Run(ServicesToRun);
        }

    }
}
