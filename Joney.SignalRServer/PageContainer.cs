using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joney.SignalRServer
{
    public class PageContainer
    {
        /// <summary>
        /// 条件
        /// </summary>
        public string conditions { get; set; }

        public MessageInfo MessageInfo { get; set; }

        public List<MessageInfo> MessageList { get; set; }

        public int pageIndex { get; set; }

        public int pageSize { get; set; }

        public PageContainer()
        {
            MessageList = new List<MessageInfo>();
        }
        
    }
}
