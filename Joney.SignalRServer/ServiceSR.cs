
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Owin;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Configuration;
using log4net;
using System.Reflection;
using log4net.Config;
using System.ComponentModel.DataAnnotations;

[assembly: OwinStartup(typeof(Joney.SignalRServer.Startup))]
namespace Joney.SignalRServer
{
    public partial class ServiceSR : ServiceBase
    {
        private static readonly string hostUrl =ConfigurationManager.AppSettings["HostUrl"].ToString();
        private static readonly string conStr = ConfigurationManager.ConnectionStrings["conStr"].ToString();

        public static ILog logger;
        IDisposable SignalR { get; set; }
        
        public ServiceSR()
        {
            InitLog4Net();//初始化配置信息
            logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            InitializeComponent();
        }

        /// <summary>
        /// 实例化日志配置信息:注意配置文件属性必须为 复制到输出目录：始终复制
        /// </summary>
        private static void InitLog4Net()
        {
            var logCfg = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
            XmlConfigurator.ConfigureAndWatch(logCfg);
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            if (!System.Diagnostics.EventLog.SourceExists("SKIMService"))
            {
                System.Diagnostics.EventLog.CreateEventSource("SKIMService", "Application");
            }
            eventLog.Source = "SKIMService";
            eventLog.Log = "Application";
            eventLog.WriteEntry("In OnStart.");

            string url = string.IsNullOrEmpty(hostUrl) ? "http://localhost:8080" : hostUrl;
            WebApp.Start(url);
            //WebApp.Start<Startup>(url);//此方式 服务启动失败
            logger.Info("SKIMService Start At:"+ DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            DosomeImportant();
            //StartSignalRService();
        }

        protected override void OnStop()
        {
            eventLog.WriteEntry("In OnStop.");
            SignalR.Dispose();
            logger.Info("ServiceSR Stop At:" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
        }

        protected override void OnPause()
        {
            base.OnPause();
        }

        protected override void OnContinue()
        {
                base.OnContinue();
        }

        protected override void OnShutdown()
        {
            //base.OnShutdown();
            logger.Info("System Shutdown SKIMService Exit At:" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
        }

        /// <summary>
        /// 开启SignalR服务:此方法与默认冲突，不可用
        /// </summary>
        private void StartSignalRService()
        {
            try
            {
                string url = string.IsNullOrEmpty(hostUrl) ? "http://localhost:888" : hostUrl;
                using (WebApp.Start(url))
                {
                    logger.Info("SignalR Service is Runing At:" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 定时器任务
        /// </summary>
        private void DosomeImportant()
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
        protected void DodoDataRecord(object obj,System.Timers.ElapsedEventArgs e)
        {
            string timestamp = DateTime.Now.ToString("yyyyMMddsshhmm");
            MessageInfo model = new MessageInfo();
            model.Addressee = "张三"+timestamp;
            model.Addressor = "李四";
            model.Attachments =new[]{"--"};
            model.Cctos = "admin,test";
            model.Subject = "不要妨碍我表达我的感情"+timestamp;
            model.Body = "上课了，老师让用 ‘不约而同’ 造个句子，点到了小明。小明站起来说：有一天我见到一个美女，说：约吗？ 美女回答：对不起，我“不约儿童”！ 老师：滚……滚粗去。";
            string msg = JsonConvert.SerializeObject(model);
            ChatHub ch = new ChatHub();
            ch.SysBroadcastMessage(msg);//通讯广播
            WriteLog(msg);//写日志
            WriteIntoDataBase(msg);//写数据库
        }
        
        /// <summary>
        /// 信息寫入數據庫
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public int WriteIntoDataBase(string msg)
        {
            DataTable dt = new DataTable();
            SqlTransaction tran = null;
            Random ran = new Random();
            var result = -1;
            using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(conStr))
            {
                try
                {
                    conn.Open();
                    tran = conn.BeginTransaction();
                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Connection = conn;
                    cmd.CommandText = "AutoSendMessage";
                    MessageInfo mi=JsonConvert.DeserializeObject<MessageInfo>(msg);
                    cmd.Parameters.Add("@TypeID", SqlDbType.Int).Value =mi.TypeID;
                    cmd.Parameters.Add("@Addressor", SqlDbType.NVarChar).Value =mi.Addressor;
                    cmd.Parameters.Add("@Addressee", SqlDbType.NVarChar).Value =mi.Addressee;
                    cmd.Parameters.Add("@Cctos", SqlDbType.NVarChar).Value =mi.Cctos;
                    cmd.Parameters.Add("@Subject", SqlDbType.NVarChar).Value =mi.Subject;
                    cmd.Parameters.Add("@Body", SqlDbType.NVarChar).Value =mi.Body;
                    cmd.Parameters.Add("@Attachments", SqlDbType.NVarChar).Value =mi.Attachments;
                    cmd.Transaction = tran;
                    //cmd.ExecuteNonQueryAsync();
                    result=cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message.ToString());
                    tran.Rollback();
                }
                finally
                {
                    conn.Dispose();
                }
            }
            return result;
        }

        /// <summary>
        /// 獲取數據庫數據
        /// </summary>
        /// <param name="condition">查詢條件</param>
        /// <returns></returns>
        public DataTable GetDataSourceInfo(string condition)
        {
            DataTable dt = new DataTable();
            try
            {
                using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(conStr))
                {
                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Connection = conn;
                    conn.Open();
                    cmd.CommandText = "GetRecordMessage";
                    PageContainer conditions =JsonConvert.DeserializeObject<PageContainer>(condition);
                    MessageInfo msg = conditions.MessageInfo;
                    int pageIndex = conditions.pageIndex;
                    int pageSize = conditions.pageSize;

                    SqlParameter[] parameters ={
                              new SqlParameter("@PageIndex",pageIndex),
                              new SqlParameter("@PageSize",pageSize),
                              new SqlParameter("@MessageID",msg.MessageID),
                              new SqlParameter("@TypeID",msg.TypeID),
                              new SqlParameter("@State",msg.State),
                              new SqlParameter("@Addressor",msg.Addressor),
                              new SqlParameter("@Addressee",msg.Addressee),
                              new SqlParameter("@Cctos",msg.Cctos),
                              new SqlParameter("@Subject",msg.Subject),
                              new SqlParameter("@Body",msg.Body),
                              new SqlParameter("@Attachments",msg.Attachments)
                              };
                    //params SqlParameter[] cmdParms
                    foreach (SqlParameter parameter in parameters)
                    {
                        if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) && (parameter.Value == null))
                        {
                            parameter.Value = DBNull.Value;
                        }
                        cmd.Parameters.Add(parameter);
                    }

                    //cmd.Parameters.Add("@SendDate", SqlDbType.Int).Value = 11;
                    //cmd.ExecuteReaderAsync();
                    SqlDataAdapter ada = new SqlDataAdapter(cmd);
                    ada.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex.Message.ToString());
            }
            return dt;
        }

        /// <summary>
        /// 输出执行日志
        /// </summary>
        /// <param name="str"></param>
        public void WriteLog(string str)
        {
            try
            {
                logger.Info(str+" At:" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                //using (System.IO.StreamWriter sw = new System.IO.StreamWriter("D:\\SignalRServer\\Dodo_log.txt", true))
                //{
                //    sw.BaseStream.Seek(0, System.IO.SeekOrigin.End);
                //    sw.WriteLineAsync(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss ") + str);
                //    sw.Flush();
                //    sw.Close();
                //}
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex.Message.ToString());
            }
        }

        /// <summary>
        /// 输出错误日志
        /// </summary>
        /// <param name="str"></param>
        public void WriteErrorLog(string str)
        {
            try
            {
                logger.Error(str + " At:" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }
    }
    
    public class MyConnection : PersistentConnection
    {
        protected override Task OnReceived(IRequest request, string connectionId, string data)
        {
            data = string.Format("Time:{0},Send Info:{1}", DateTime.Now.ToString(), data);
            return Connection.Broadcast(data);
            //return base.OnReceived(request, connectionId, data);
        }
    }
    
}
