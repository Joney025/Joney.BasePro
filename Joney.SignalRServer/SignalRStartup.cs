using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joney.SignalRServer
{
    public class SignalRStartup
    {
        public static IAppBuilder _app = null;
        public void Configuration(IAppBuilder app)
        {
            app.Map("/signalr", map => { 
                map.UseCors(CorsOptions.AllowAll);
                var hubConfiguration = new HubConfiguration
                {
                    //EnableCrossDomain=true,
                    EnableJSONP=true,
                    EnableDetailedErrors = true
                };
                map.RunSignalR(hubConfiguration);
                //app.MapHubs(hubConfiguration);
            });
        }
    }
}
