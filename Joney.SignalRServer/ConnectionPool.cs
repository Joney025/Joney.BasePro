using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joney.SignalRServer
{
    public class ConnectionPool
    {
        public ConnectionPool() { }

        /// <summary>
        /// 用户代理
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// ConnectionID
        /// </summary>
        public string ConnectionID { get; set; }

        /// <summary>
        /// URL（IP地址）
        /// </summary>
        public string ConnectUrl { get; set; }

        /// <summary>
        /// 连接时间
        /// </summary>
        public DateTime ConnectTime { get; set; }

        /// <summary>
        /// 连接状态
        /// </summary>
        public bool ConnectStatus { get; set; }
    }
}
