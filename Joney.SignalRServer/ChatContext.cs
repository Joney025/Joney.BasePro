using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joney.SignalRServer
{
    public class ChatContext
    {
        public List<UserInfo> UsersList { get; set; }

        public List<ConnectionPool> ConnectionPoolList { get; set; }

        public List<ConversationRoom> RoomsList { get; set; }

        public ChatContext()
        {
            UsersList = new List<UserInfo>();
            ConnectionPoolList = new List<ConnectionPool>();
            RoomsList = new List<ConversationRoom>();
        }
    }
}
