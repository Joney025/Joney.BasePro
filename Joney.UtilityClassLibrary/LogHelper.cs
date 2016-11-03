using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using log4net;
using System.Reflection;
using log4net.Config;

namespace Joney.UtilityClassLibrary
{
    public class LogHelper
    {
        public static ILog logger;

        /// <summary>
        /// 登记日志文件
        /// </summary>
        /// <param name="name">用户</param>
        /// <param name="path">路径</param>
        /// <param name="ips">IP</param>
        /// <param name="action">动作</param>
        public static void ShowSettingsElement(string name, string action, string ips, string path)
        {
            //ReadWriteConfig config = new ReadWriteConfig();
            //SearchedValue = config.readConfigDoc("FileUploadSize");
            #region 日志操作

            string loger = name;
            string strNow = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            string ip = "Local_IP:" + ips + " Service_IP:" + CommonUtility._GetClientIPAddress();
            FileInfo fiXML = new FileInfo(path);
            if (!(fiXML.Exists))
            {
                XDocument xelLog = new XDocument(
                    new XDeclaration("1.0", "utf-8", "no"),
                    new XElement("ipmsg",
                        new XElement("login_log",
                        new XElement("ipAdd", ip),
                        new XElement("user", loger),
                        new XElement("logdate", strNow),
                        new XElement("message", action)
                        ))
                    );
                xelLog.Save(path);
            }
            else
            {
                Dictionary<string, string> dicLog = new Dictionary<string, string>();
                dicLog.Add("ipAdd", ip);
                dicLog.Add("user", name);
                dicLog.Add("logdate", strNow);
                dicLog.Add("message", action);

                XElement xelLog = XElement.Load(path);
                XElement newLog = new XElement("login_log",
                    new XElement("ipAdd", (string)dicLog["ipAdd"]),
                    new XElement("user", (string)dicLog["user"]),
                    new XElement("logdate", (string)dicLog["logdate"]),
                    new XElement("message", (string)dicLog["message"])
                    );
                xelLog.Add(newLog);
                xelLog.Save(path);
            }
            #endregion
        }

        /// <summary>
        /// 修改日志
        /// </summary>
        /// <param name="name"></param>
        /// <param name="path"></param>
        public static void ModifyLog(string name, string path)
        {
            FileInfo fiXML = new FileInfo(path);
            if (fiXML.Exists)
            {
                XElement xelem = XElement.Load(path);
                var queryXml = from LoginLog in xelem.Descendants("msg_log")
                               where LoginLog.Element("user").Value == "Admin"
                               select LoginLog;
                foreach (XElement item in queryXml)
                {
                    item.Element("user").Value = name;
                }
            }
        }

        /// <summary>
        /// 日志记录
        /// </summary>
        /// <param name="strmessage"></param>
        public static void WriteLogs(string strmessage)
        {
            try
            {
                string strFilePathName = AppDomain.CurrentDomain.SetupInformation.ApplicationBase.ToString() + "\\log";
                if (!Directory.Exists(strFilePathName))
                {
                    Directory.CreateDirectory(strFilePathName);
                }
                string strFileName = strFilePathName + @"\record.log";
                FileStream objFS;
                if (File.Exists(strFileName))
                {
                    FileInfo objFileInfo = new FileInfo(strFileName);
                    objFS = new FileStream(strFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                    objFS.Seek(0, SeekOrigin.End);
                }
                else
                {
                    objFS = new FileStream(strFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                    objFS.Seek(0, SeekOrigin.End);
                }
                StreamWriter objSW = new StreamWriter(objFS);
                objSW.WriteLine(objSW.NewLine);
                objSW.WriteLine(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.") +
                                System.DateTime.Now.Millisecond.ToString());
                objSW.Write(strmessage);
                objSW.WriteLine();
                objSW.Close();
                objFS.Close();
            }
            catch (Exception ex)
            {
                //string err = ex.Message;
                //EventLog.WriteEntry("PiWatchInterFace WriteLog Error:" + ex.Message.ToString());
            }
        }

        /// <summary>
        /// 日志记录
        /// </summary>
        /// <param name="strmessage">日志信息</param>
        /// <param name="pathname">日志名称</param>
        public static void WriteLogs(string strmessage, string pathname)
        {
            try
            {
                string strFilePathName = AppDomain.CurrentDomain.SetupInformation.ApplicationBase.ToString() + "\\log";
                if (!Directory.Exists(strFilePathName))
                {
                    Directory.CreateDirectory(strFilePathName);
                }
                string strFileName = strFilePathName + @"\" + pathname + ".log";
                FileStream objFS;
                if (File.Exists(strFileName))
                {
                    FileInfo objFileInfo = new FileInfo(strFileName);
                    objFS = new FileStream(strFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                    objFS.Seek(0, SeekOrigin.End);
                }
                else
                {
                    objFS = new FileStream(strFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                    objFS.Seek(0, SeekOrigin.End);
                }
                StreamWriter objSW = new StreamWriter(objFS);
                objSW.WriteLine(objSW.NewLine);
                objSW.WriteLine(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.") +
                                System.DateTime.Now.Millisecond.ToString());
                objSW.Write(strmessage);
                objSW.WriteLine();
                objSW.Close();
                objFS.Close();
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                //EventLog.WriteEntry("PiWatchInterFace WriteLog Error:" + ex.Message.ToString());
            }
        }

        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="path">HttpContext.Current.Server.MapPath(path)</param>
        /// <param name="fileName">自定义文件名</param>
        /// <param name="content">文件内容</param>
        /// <returns>返回写入结果：true、false</returns>
        public static bool WriteFile(string path,string fileName,string content)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string fname = path + "/" + fileName;
                FileStream FS = null;
                if (File.Exists(fname))
                {
                    FileInfo objFileInfo = new FileInfo(fname);
                    FS = new FileStream(fname, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                    FS.Seek(0, SeekOrigin.End);
                }
                else
                {
                    //FS = File.Create(fname);
                    //FS.Close();
                    FS = new FileStream(fname, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                    FS.Seek(0, SeekOrigin.End);
                }
                StreamWriter SW = new StreamWriter(FS,System.Text.Encoding.GetEncoding("UTF-8")); //new StreamWriter(fname,false,System.Text.Encoding.GetEncoding("UTF-8"));
                SW.WriteLineAsync(content);
                SW.Close();
                SW.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 读文件操作
        /// </summary>
        /// <returns></returns>
        public static string ReadLogs(string FilePath)
        {
            StreamReader SR;
            string TempStr, RtnStr = "";
            if (File.Exists(FilePath))
            {
                SR = File.OpenText(FilePath);
                TempStr = SR.ReadLine();
                while (TempStr != null)
                {
                    RtnStr += TempStr + "\r\n";
                    TempStr = SR.ReadLine();

                }
                SR.Close();
            }
            return RtnStr;
        }

        /// <summary>
        /// 读取文件内容：
        /// </summary>
        /// <param name="path">HttpContent.Current.Server.MapPath(path)</param>
        /// <returns>读取内容字符串</returns>
        public static string ReadFile(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    return string.Empty;
                }
                StreamReader SR = new StreamReader(path,System.Text.Encoding.GetEncoding("UTF-8"));
                string content = SR.ReadToEndAsync().ToString();
                SR.Close();
                return content;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        /// <summary>
        /// log4net loger
        /// </summary>
        /// <param name="tp"></param>
        /// <param name="type"></param>
        /// <param name="msg"></param>
        public static void Jlogger(Type tp, string type,string msg)
        {
            //Type=MethodBase.GetCurrentMethod().DeclaringType
            InitLog4Net();//初始化配置信息
            logger = LogManager.GetLogger(tp);
            type = string.IsNullOrEmpty(type) ? "" : type.ToUpper();
            msg =CommonUtility._GetClientIPAddress()+ "——"+msg;
            switch (type)
            {
                case "ERROR":
                    logger.Error(msg);
                    break;
                case "DEBUG":
                    logger.Debug(msg);
                    break;
                case "FATAL":
                    logger.Fatal(msg);
                    break;
                case "WARN":
                    logger.Warn(msg);
                    break;
                default:
                    logger.Info(msg);
                    break;
            }
        }

        /// <summary>
        /// 实例化日志配置信息:注意配置文件属性必须为 复制到输出目录：始终复制
        /// </summary>
        private static void InitLog4Net()
        {
            var logCfg = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
            XmlConfigurator.ConfigureAndWatch(logCfg);
        }
    }
}
