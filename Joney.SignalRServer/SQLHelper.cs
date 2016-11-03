using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joney.SignalRServer
{
    public class SQLHelper
    {
        private static readonly string hostUrl = ConfigurationManager.ConnectionStrings["conStr"].ToString();

    }
}
