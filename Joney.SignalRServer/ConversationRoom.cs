using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joney.SignalRServer
{
    /// <summary>
    /// 聊天室类
    /// </summary>
    public class ConversationRoom
    {
        [Key]
        public string RoomName { get; set; }
        /// <summary>
        /// 创建者
        /// </summary>
        public string Creater { get; set; }
        /// <summary>
        /// 管理员
        /// </summary>
        public string Manager { get; set; }
        /// <summary>
        /// 用户集合
        /// </summary>
        public virtual List<UserInfo> Users { get; set; }
        /// <summary>
        /// 聊天室构造函数
        /// </summary>
        public ConversationRoom()
        {
            Users = new List<UserInfo>();
        }
    }
}
