using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joney.SignalRServer
{
    public class MessageInfo
    {

        /// <summary>
        /// 信息編號
        /// </summary>
        public int? MessageID { get; set; }
        /// <summary>
        /// 類型編號
        /// </summary>
        public int? TypeID { get; set; }
        /// <summary>
        /// 狀態
        /// </summary>
        public byte? State { get; set; }
        /// <summary>
        /// 发信人
        /// </summary>
        public string Addressor { get; set; }
        /// <summary>
        /// 收信人
        /// </summary>
        public string Addressee { get; set; }
        /// <summary>
        /// 抄送對象
        /// </summary>
        public string Cctos { get; set; }
        /// <summary>
        /// 主題
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// 內容
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        public string[] Attachments { get; set; }
        /// <summary>
        /// 保存時間
        /// </summary>
        public DateTime? SaveDate { get; set; }
        /// <summary>
        /// 發送時間
        /// </summary>
        public DateTime? SendDate { get; set; }

        public MessageInfo()
        {

        }
    }
}
