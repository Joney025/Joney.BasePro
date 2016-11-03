using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joney.SignalRServer
{
    public class UserInfo
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [Key]
        public string UID { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用户密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 用户关联的对话用户集合
        /// </summary>
        public List<ConnectionPool> ConnectionPool { get; set; }
        /// <summary>
        /// 用户关联的聊天室集合
        /// </summary>
        public virtual List<ConversationRoom> Rooms { get; set; }

        public UserInfo()
        {
            ConnectionPool = new List<ConnectionPool>();
            Rooms = new List<ConversationRoom>();
        }
    }
}
