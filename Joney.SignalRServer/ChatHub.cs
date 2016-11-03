using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Threading;

namespace Joney.SignalRServer
{
    [HubName("MyHub")]
    public class ChatHub : Hub
    {
        private static List<ConnectionPool> connections = new List<ConnectionPool>();

        private static readonly Dictionary<string, string> _users = new Dictionary<string, string>();//存放客户端发送过来的用户名

        private static readonly Dictionary<string, string> _clients = new Dictionary<string, string>();//存放用户ConnectionID

        public static ChatContext dbContext = new ChatContext();

        public ChatHub()
        {
            System.Timers.Timer timer = new System.Timers.Timer(10000);
            timer.AutoReset = true;
            timer.Enabled = false;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(DodoDataRecord);
            timer.Start();
        }

        /// <summary>
        /// 定时作业：demo
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="e"></param>
        protected void DodoDataRecord(object obj, System.Timers.ElapsedEventArgs e)
        {
            string timestamp = DateTime.Now.ToString("yyyyMMddsshhmm");
            MessageInfo model = new MessageInfo();
            model.Addressee = "张三" + timestamp;
            model.Addressor = "李四";
            model.Attachments = new[] { "--" };
            model.Cctos = "admin,test";
            model.Subject = "不要妨碍我表达我的感情" + timestamp;
            model.Body = "上课了，老师让用 ‘不约而同’ 造个句子，点到了小明。小明站起来说：有一天我见到一个美女，说：约吗？ 美女回答：对不起，我“不约儿童”！ 老师：滚……滚粗去。";
            string msg = JsonConvert.SerializeObject(model);
            ChatHub ch = new ChatHub();
            ch.SysBroadcastMessage(msg);//通讯广播
        }

        public void Hello()
        {
            Clients.All.hello();
        }

        /// <summary>
        /// 连接登录
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="userID">编号</param>
        /// <param name="pwd">密码</param>
        public void Connect(string url, string userID, string pwd)
        {
            try
            {
                object ip = string.Empty;
                Context.Request.Environment.TryGetValue("server.RemoteIpAddress", out ip);
                if (ip != null)
                {
                    url = ip.ToString();
                }
                var userlist = GetDataUser(userID, pwd);
                if (userlist.Count > 0)
                {
                    var id = Context.ConnectionId;
                    if (connections.Count(x => x.ConnectionID == id && x.UserAgent == userID) == 0)
                    {
                        connections.Add(new ConnectionPool
                        {
                            UserAgent = userID,
                            ConnectionID = id,
                            ConnectUrl = url,
                            ConnectTime = DateTime.Now
                        });
                        _users[userID] = id;
                        _clients[id] = userID;

                        Clients.Caller.onConnected(id, userID, url);
                        Clients.Client(id).onNewUserConnected(id, userID);
                        //Clients.AllExcept(id).onNewUserConnected(id,userID);
                    }
                    else
                    {
                        Clients.Caller.onConnected(id, userID, url);
                        Clients.Client(id).onExistUserConnected(id, userID);
                        //Clients.AllExcept(id).onExistUserConnected(id,userID);
                    }
                    //Clients.All.addMessage(Newtonsoft.Json.JsonConvert.SerializeObject(conUser));//输出当前所有连接.
                    ServiceSR ss = new ServiceSR();
                    ss.WriteLog(Newtonsoft.Json.JsonConvert.SerializeObject(connections));
                }
                else
                {
                    Clients.All.ErrorMessage("错误用户信息.");
                }
            }
            catch (Exception ex)
            {
                Clients.All.ErrorMessage(ex.Message.ToString());
            }
        }


        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="userID"></param>
        public void Exist(string userID)
        {
            try
            {
                var id = Context.ConnectionId;
                OnDisconnected(true);

                Clients.Caller.onConnected(id, userID, "");
                Clients.Client(id).onExist(id, userID);
            }
            catch (Exception ex)
            {
                Clients.All.ErrorMessage(ex.Message.ToString());
            }
        }

        /// <summary>
        /// 连接
        /// </summary>
        /// <returns></returns>
        public override Task OnConnected()
        {
            //校验当前用户
            var user = dbContext.UsersList.SingleOrDefault(u => u.UID == Context.ConnectionId);
            if (user == null)
            {
                user = new UserInfo()
                {
                    UID = Context.ConnectionId
                };
                dbContext.UsersList.Add(user);
            }
            //获取聊天室列表
            var itme = from a in dbContext.RoomsList select new { a.RoomName };
            Clients.Client(this.Context.ConnectionId).getRoomlist(JsonConvert.SerializeObject(itme.ToList()));
            return base.OnConnected();
        }

        /// <summary>
        /// 更新所有用户的聊天室列表
        /// </summary>
        private void GetRoomList()
        {
            var itme = from a in dbContext.RoomsList select new { a.RoomName };
            var jsondata = JsonConvert.SerializeObject(itme.ToList());
            Clients.All.getRoomlist(jsondata);
        }

        /// <summary>
        /// 断开
        /// </summary>
        /// <param name="stopCalled"></param>
        /// <returns></returns>
        public override Task OnDisconnected(bool stopCalled)
        {
            try
            {
                //var item = connections.FirstOrDefault(x => x.UserAgent == Context.ConnectionId);
                //if (item != null)
                //{
                //    connections.Remove(item);
                //    var id = Context.ConnectionId;
                //    Clients.All.onUserDisconnected(id, item.UserAgent);
                //}
                var user = dbContext.UsersList.Where(u => u.UserName == Context.ConnectionId).FirstOrDefault();

                //判断用户是否存在,存在则删除
                if (user != null)
                {
                    //删除用户
                    dbContext.UsersList.Remove(user);
                    // 循环用户的房间,删除用户
                    foreach (var item in user.Rooms)
                    {
                        RemoveFromRoom(item.RoomName);

                    }
                }
            }
            catch (Exception ex)
            {
                Clients.All.ErrorMessage(ex.Message.ToString());
            }

            return base.OnDisconnected(stopCalled);
        }

        /// <summary>
        /// 退出聊天室
        /// </summary>
        /// <param name="roomName"></param>
        public void RemoveFromRoom(string roomName)
        {

            //查找房间是否存在
            var room = dbContext.RoomsList.Find(a => a.RoomName == roomName);
            //存在则进入删除
            if (room != null)
            {
                //查找要删除的用户
                var user = room.Users.Where(a => a.UserName == Context.ConnectionId).FirstOrDefault();
                //移除此用户
                room.Users.Remove(user);
                //如果房间人数为0,则删除房间
                if (room.Users.Count <= 0)
                {
                    dbContext.RoomsList.Remove(room);

                }
                Groups.Remove(Context.ConnectionId, roomName);
                //提示客户端
                Clients.Client(Context.ConnectionId).removeRoom("退出成功!");
            }

        }

        /// <summary>
        /// 重新连接
        /// </summary>
        /// <returns></returns>
        public override Task OnReconnected()
        {

            //离线重新连接
            return base.OnReconnected();
        }

        /// <summary>
        /// 获取数据库用户
        /// </summary>
        /// <param name="condition">筛选条件</param>
        /// <returns></returns>
        private List<UserInfo> GetDataUser(string code, string password)
        {
            List<UserInfo> userList = new List<UserInfo>();
            try
            {
                if (true)
                {
                    List<UserInfo> defaultList = new List<UserInfo>();
                    var user1 = new UserInfo { UID = "1", UserName = "ADMIN", Password = "123456" };
                    var user2 = new UserInfo { UID = "2", UserName = "JONEY", Password = "123456" };
                    var user3 = new UserInfo { UID = "3", UserName = "TESTER", Password = "123456" };
                    defaultList.Add(user1);
                    defaultList.Add(user2);
                    defaultList.Add(user3);
                    var tt = defaultList.FirstOrDefault(x => x.UserName == code && x.Password == password);
                    if (tt != null)
                    {
                        userList.Add(tt);
                    }
                }
                else
                {
                    //List<object> userList = new List<object>();
                    //var user1 = new { id = 1, name = "ADMIN", password = "123456" };
                    //var user2 = new { id = 2, name = "JONEY", password = "123456" };
                    //var user3 = new { id = 3, name = "TESTER", password = "123456" };
                    //userList.Add(user1);
                    //userList.Add(user2);
                    //userList.Add(user3);
                }
            }
            catch (Exception)
            {

                throw;
            }
            return userList;
        }

        /// <summary>
        /// 根据ConnectionID获取用户名
        /// </summary>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        private string GetUser(string connectionId)
        {
            string user = string.Empty;
            if (!_users.TryGetValue(connectionId, out user))
            {
                return connectionId;
            }
            return user;
        }

        /// <summary>
        /// 根据用户名获取用户ConnectionID
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private string GetClient(string user)
        {
            string connectionId = string.Empty;
            if (_users.TryGetValue(user, out connectionId))
            {
                return connectionId;
            }
            return connectionId;
        }

        /// <summary>
        /// 默认广播Send Method
        /// </summary>
        /// <param name="msg"></param>
        public void SysBroadcastMessage(string msg)
        {
            try
            {
                msg = string.Format("ServerTime：{0}，Context：{1},ConnectedNum:{2}", DateTime.Now.ToString(), msg, connections.Count);
                Clients.All.BroadcastMessage(msg);
            }
            catch (Exception ex)
            {
                Clients.All.ErrorMessage(ex.Message.ToString());
            }
        }


        /// <summary>
        /// 入组连接
        /// </summary>
        /// <param name="key">标识ID</param>
        /// <param name="group">群组标识</param>
        public void AddGroup(string key, string from, string group, string gname)
        {
            try
            {
                var id = Context.ConnectionId;
                Groups.Add(id, group);
                var dTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                from = from + " " + dTime;
                Clients.Group(group).ReceiveGroupMessage(key, from, group, key + "已进入" + gname + "通话组!");
            }
            catch (Exception ex)
            {
                Clients.All.ErrorMessage(ex.Message.ToString());
            }

        }

        /// <summary>
        /// 退出聊天室
        /// </summary>
        /// <param name="roomName"></param>
        public void RemoveGroup(string id, string gname)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    Groups.Remove(id, gname);
                }
                else
                {
                    Groups.Remove(Context.ConnectionId, gname);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 给分组内所有的用户发送消息
        /// </summary>
        /// <param name="Room">分组名</param>
        /// <param name="Message">信息</param>
        public void SendMessageToRoom(string room, string message)
        {
            try
            {
                Clients.Group(room, new string[0]).sendMessageToRoom(room, message + "-" + DateTime.Now.ToString());
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 分组通信
        /// </summary>
        /// <param name="key">标识ID</param>
        /// <param name="group">群组标识</param>
        /// <param name="message">信息</param>
        public void SendGroup(string key, string from, string group, string message)
        {
            try
            {
                //var id = Context.ConnectionId;
                //Groups.Add(id,group);//添加连接到指定的组
                //Groups.Remove(id,group);//从组中移除Connection连接

                //Clients.Group(group).sendMessage(string.Format(CultureInfo.InvariantCulture, message, DateTime.UtcNow, group + "/"));

                var dTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                from = from + " " + dTime;
                Clients.Group(group).ReceiveGroupMessage(key, from, group, message);
            }
            catch (Exception ex)
            {
                Clients.All.ErrorMessage(ex.Message.ToString());
            }

        }

        /// <summary>
        /// One to One
        /// </summary>
        /// <param name="key">标识ID</param>
        /// <param name="from">信息来自？</param>
        /// <param name="to">信息接收方？</param>
        /// <param name="message">信息内容</param>
        public void SendOne(string key, string from, string to, string message)
        {
            try
            {
                var dTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                from = from + " " + dTime;
                var cid = Context.ConnectionId;//当前用户的ConnectionID
                var tid = "";//接收信息方的ConnectionID
                if (connections.Count(x => x.UserAgent == to) > 0)
                {
                    tid = connections.FirstOrDefault(x => x.UserAgent == to).ConnectionID;
                    Clients.Client(tid).ReceiveOneMessage(key, from, to, message);//对方也能看到
                }
                Clients.Client(cid).ReceiveOneMessage(key, from, to, message);//我要也能看到
                //服务器调用newLog().
                IHubContext _hubContext = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                _hubContext.Clients.User(to).newLog();
            }
            catch (Exception ex)
            {
                Clients.All.ErrorMessage(ex.Message.ToString());
            }

        }

        /// <summary>
        /// 一对一通信
        /// </summary>
        /// <param name="msg">Json对象数据</param>
        public void SendOneToOne(string msg)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                try
                {
                    MessageInfo message = JsonConvert.DeserializeObject<MessageInfo>(msg);
                    var dTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                    var to = message.Addressee;
                    var cid = Context.ConnectionId;//当前用户的ConnectionID
                    var tid = "";//接收信息方的ConnectionID
                    if (connections.Count(x => x.UserAgent == to) > 0)
                    {
                        tid = connections.FirstOrDefault(x => x.UserAgent == to).ConnectionID;
                        Clients.Client(tid).ReceiveFromOneMessage(msg);//对方也能看到
                    }
                    Clients.Client(cid).ReceiveFromOneMessage(msg);//我要也能看到
                    //服务器调用newLog().
                    IHubContext _hubContext = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                    _hubContext.Clients.User(to).newLog();
                    ServiceSR ss = new ServiceSR();
                    ss.WriteIntoDataBase(msg);
                    ss.WriteLog(msg);
                }
                catch (Exception ex)
                {
                    Clients.All.ErrorMessage(ex.Message.ToString());
                }
            }
        }

        /// <summary>
        /// 群组通信
        /// </summary>
        /// <param name="msg">Json对象数据</param>
        public void SendOneToGroup(string msg)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                try
                {
                    MessageInfo message = JsonConvert.DeserializeObject<MessageInfo>(msg);
                    Clients.Group(message.TypeID.ToString()).ReceiveFromGroupMessage(message);
                    ServiceSR ss = new ServiceSR();
                    ss.WriteIntoDataBase(msg);
                    ss.WriteLog(msg);
                }
                catch (Exception ex)
                {
                    Clients.All.ErrorMessage(ex.Message.ToString());
                }
            }
        }

        #region 文件上传拷贝复制

        public void UploadFileOrImg(string msg)
        {
            if (string.IsNullOrEmpty(msg))
            {
                Clients.All.addMessage("文件内容不能为空");
                return;
            }
            int fileCount = 0;
            if (msg.Contains("|"))
            {
                fileCount = msg.Split('|').Length;
            }
            else
            {
                fileCount = 1;
            }
            string[] fileCollection = new string[fileCount];
            if (fileCount > 1)
            {
                fileCollection = msg.Split('|');
            }
            else
            {
                fileCollection[0] = msg;
            }
            string uploadPath = AppDomain.CurrentDomain.BaseDirectory;
            int fileFlag = 0;
            foreach (string filename in fileCollection)
            {
                if (File.Exists(filename))
                {
                    string newName = Path.Combine(uploadPath, "UploadFile", FileWithOutExtension(filename));
                    if (File.Exists(newName))
                    {
                        try
                        {
                            File.Delete(newName);
                        }
                        catch (Exception ex)
                        {
                            Clients.All.addMessage(ex.Message.ToString());
                        }
                        parameterCollection pc = new parameterCollection();
                        pc.filename = filename;
                        pc.newName = newName;
                        pc.eachLoopSize = 2048;
                        pc.fileFlag = fileFlag;

                        fileFlag++;
                    }
                }
            }
        }

        private void BeginCopyFile(object obj)
        {
            try
            {
                parameterCollection pc = (parameterCollection)obj;
                Clients.All.addMessage("Start to copy file:" + pc.filename + "...");
                Action<object> actionStart = new Action<object>(CopyFileAsync);
                actionStart.BeginInvoke(obj, new AsyncCallback(vok =>
                {
                    Action<object> actionEnd = (Action<object>)vok.AsyncState;
                    actionEnd.EndInvoke(vok);
                    Clients.All.addMessage("Copied " + pc.filename + " Ok...");
                }), actionStart);
            }
            catch (Exception ex)
            {
                Clients.All.addMessage(ex.Message.ToString());
            }
        }

        /// <summary>
        /// 复制拷贝文件
        /// </summary>
        /// <param name="fromFile">要复制的文件</param>
        /// <param name="toFile">要保存的位置</param>
        /// <param name="lengthEachTime">每次复制的长度</param>
        /// <param name="fileFlag">复制状态</param>
        private void CopyFile(string fromFile, string toFile, int lengthEachTime, int fileFlag)
        {
            FileStream fileToCopy = null;
            try
            {
                fileToCopy = new FileStream(fromFile, FileMode.Open, FileAccess.Read);
            }
            catch (Exception ex)
            {
                Clients.All.addMessage(ex.Message.ToString());
            }
            FileStream copyToFile = null;
            try
            {
                copyToFile = new FileStream(toFile, FileMode.Append, FileAccess.Write);
            }
            catch (Exception ex)
            {
                Clients.All.addMessage(ex.Message.ToString());
            }
            string fileFlagStr = fileFlag.ToString();
            int lenghtToCopy = 0;
            int pauseCount = 0;
            if (lengthEachTime < fileToCopy.Length)//分段拷贝
            {
                byte[] buffer = new byte[lengthEachTime];
                int copied = 0;
                while (copied <= (int)fileToCopy.Length - lengthEachTime)
                {
                    lenghtToCopy = fileToCopy.Read(buffer, 0, lengthEachTime);
                    fileToCopy.Flush();
                    copyToFile.Write(buffer, 0, lengthEachTime);
                    copyToFile.Flush();
                    copyToFile.Position = fileToCopy.Position;
                    copied += lenghtToCopy;

                    string sendSizeCurrent = ((double)copied / (double)fileToCopy.Length).ToString();
                    Clients.All.addMessage(fileFlagStr + "|" + sendSizeCurrent);
                    pauseCount++;
                    if (pauseCount % 3 == 0)
                    {
                        Thread.Sleep(1);
                    }
                }
                int left = (int)fileToCopy.Length - copied;
                lenghtToCopy = fileToCopy.Read(buffer, 0, left);
                fileToCopy.Flush();
                copyToFile.Write(buffer, 0, left);
                copyToFile.Flush();
                Clients.All.addMessage(fileFlagStr + "|" + 1);
            }
            else//整体拷贝
            {
                byte[] buffer = new byte[fileToCopy.Length];
                fileToCopy.Read(buffer, 0, (int)fileToCopy.Length);
                fileToCopy.Flush();
                copyToFile.Write(buffer, 0, (int)fileToCopy.Length);
                copyToFile.Flush();
                Clients.All.addMessage(fileFlagStr + "|" + 1);
            }
            fileToCopy.Close();
            copyToFile.Close();
            Thread.Sleep(10);
        }

        private void CopyFileAsync(object obj)
        {
            parameterCollection pc = (parameterCollection)obj;
            CopyFile(pc.filename, pc.newName, pc.eachLoopSize, pc.fileFlag);
        }

        private string FileWithOutExtension(string filePath)
        {
            if (filePath.Contains(@"\"))
            {
                return filePath.Substring(filePath.LastIndexOf(@"\") + 1);
            }
            if (filePath.Contains(@"/"))
            {
                return filePath.Substring(filePath.LastIndexOf(@"/") + 1);
            }
            return filePath;
        }

        private struct parameterCollection
        {
            public string filename;
            public string newName;
            public int eachLoopSize;
            public int fileFlag;
        }

        #endregion
    }
}
