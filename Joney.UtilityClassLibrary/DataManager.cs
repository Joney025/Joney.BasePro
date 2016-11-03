using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace Joney.UtilityClassLibrary
{
    public static class DataManager
    {
        /// <summary>
        ///  因为DataReader独占连接，本程序需要两个Connection,并且连接的数据库是master,所以要初始化新的连接字符串
        /// </summary>
        public static string connectionString = System.Configuration.ConfigurationManager.AppSettings["DBmaster"].ToString();
        public static string DBpath = System.Configuration.ConfigurationManager.AppSettings["DBpath"].ToString();
        public static string DBname = System.Configuration.ConfigurationManager.AppSettings["DBName"].ToString();
        public static string ErrorlogPath = System.Configuration.ConfigurationManager.AppSettings["ErrorLogPath"].ToString();
        public static SqlConnection sc = new SqlConnection(connectionString);

        #region 备份整个数据库
        /// <summary>
        /// 备份数据库
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static string BackUp(System.Web.UI.Page page)
        {

            string newname = DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒") + ".bak";
            StringBuilder sb = new StringBuilder();
            sb.Append(page.Server.MapPath("") + @"\");
            sb.Append(DBpath);
            if (!Directory.Exists(sb.ToString()))
            {
                Directory.CreateDirectory(sb.ToString());
            }
            sb.Append(@"\");
            sb.Append(newname);
            string sql = "BACKUP DATABASE @DBName to DISK =@DISK";
            try
            {
                SqlParameter[] sp ={
                new SqlParameter("@DBName", SqlDbType.NVarChar,50),
                new SqlParameter("@DISK", SqlDbType.NVarChar,200)};
                sp[0].Value = DBname;
                sp[1].Value = sb.ToString();
                Joney.UtilityClassLibrary.DBHelper.ExecuteNonQuery(sc, CommandType.Text, sql, sp);
                return newname;
            }
            catch (Exception ex)
            {
                ArrayList al = new ArrayList();
                al.Add("--------------------------数据备份错误---------------------------------");
                al.Add(ex.ToString());
                WriteLog(page, al);///写入日志
                return null;
            }
        }
        #endregion
        #region 根据时间自动备份整个数据库
        /// <summary>
        /// 根据时间自动备份整个数据库
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static string BackUpByday(string path)
        {
            ///string date = Yunpeng.DataAccess.Yutility.DBHelperSQL.GetSingle(" select getdate()").ToString();
            DateTime date = DateTime.Now;
            string newname = Convert.ToDateTime(date).ToString("yyyy年MM月dd日") + ".bak";
            StringBuilder sb = new StringBuilder();
            sb.Append(path + @"\");
            sb.Append(DBpath);
            if (!Directory.Exists(sb.ToString()))
            {
                Directory.CreateDirectory(sb.ToString());
            }
            sb.Append(@"\");
            sb.Append(newname);
            if (File.Exists(sb.ToString()))
            {
                return "";
            }
            string sql = "BACKUP DATABASE @DBName to DISK =@DISK";
            try
            {
                SqlParameter[] sp ={
                new SqlParameter("@DBName", SqlDbType.NVarChar,50),
                new SqlParameter("@DISK", SqlDbType.NVarChar,200)};
                sp[0].Value = DBname;
                sp[1].Value = sb.ToString();
                Joney.UtilityClassLibrary.DBHelper.ExecuteNonQuery(sc, CommandType.Text, sql, sp);
                return newname;
            }
            catch (Exception ex)
            {
                ArrayList al = new ArrayList();
                al.Add("--------------------------数据备份错误---------------------------------");
                al.Add(ex.ToString());
                ///WriteLog(page, al);
                return null;
            }
        }
        #endregion
        #region 还原整个数据库
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathBack">还原文件的路径</param>
        /// <param name="strDBName">数据库名</param>
        /// <returns></returns>
        public static bool SQLBack(System.Web.UI.Page page, string pathBack)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            KillSPID(page, DBname);
            bool IsBack = false;
            try
            {

                if (!pathBack.EndsWith(".bak"))
                {
                    throw new Exception("请选择正确的备份设备！");
                }
                string logicalName = pathBack.Substring(pathBack.LastIndexOf('\\') + 1).TrimEnd(new char[] { 'b', 'a', 'k', '.' });
                string cmdText = "restore database " + DBname + " from disk = '" + pathBack + "' WITH   REPLACE ";
                SqlParameter[] sp = { };
                Joney.UtilityClassLibrary.DBHelper.ExecuteNonQuery(sc, CommandType.Text, cmdText, sp);
                IsBack = true;
            }
            catch (Exception ex)
            {
                ArrayList al = new ArrayList();
                al.Add("--------------------------数据库还原错误---------------------------------");
                al.Add(ex.ToString());
                WriteLog(page, al);
            }
            return IsBack;
        }

        public static void KillConnect(System.Web.UI.Page page, string dbName)
        {
            SqlConnection con = new SqlConnection("server=localhost;database=master;uid=sa;pwd=;");
            SqlCommand cmd = new SqlCommand("killConnect", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter par = new SqlParameter("@dbName", SqlDbType.VarChar, 20);
            par.Value = dbName;
            cmd.Parameters.Add(par);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                page.ClientScript.RegisterStartupScript(page.GetType(),"message","<script language='javascript' defer>alert("+ ex.ToString()+");</script>");
            }
        }
        /// <summary>
        /// 关闭当前连接数据库的连接进程
        /// </summary>
        /// <param name="DBName">要关闭进程的数据库名称</param>
        public static void KillSPID(System.Web.UI.Page page, string DBName)
        {
            string strDBName = DBName;
            string strSQL = String.Empty, strSQLKill = String.Empty;
            try
            {

                //读取连接当前数据库的进程
                strSQL = "select spid from master..sysprocesses where dbid=db_id('" + strDBName + "')";
                SqlParameter[] sp = { };
                using (SqlDataReader mydr = Joney.UtilityClassLibrary.DBHelper.ExecuteReader(connectionString, CommandType.Text, strSQL, sp))
                {
                    //开取杀进程的数据连接
                    while (mydr.Read())
                    {
                        strSQLKill = "kill " + mydr["spid"].ToString();
                        SqlParameter[] sps = { };
                        Joney.UtilityClassLibrary.DBHelper.ExecuteNonQuery(connectionString, CommandType.Text, strSQLKill, sps);
                    }
                    mydr.Close();
                }
            }
            catch (Exception ex)
            {
                ArrayList al = new ArrayList();
                al.Add("--------------------------清除数据库" + DBName + "进程错误错误---------------------------------");
                al.Add(ex.ToString());
                WriteLog(page, al);
            }
        }
        #endregion
        #region 获得备份文件列表
        /// <summary>
        /// 获得备份文件列表
        /// </summary>
        /// <returns></returns>
        public static List<BackModel> BackList(string dbpath)
        {
            if (!Directory.Exists(dbpath))
            {
                Directory.CreateDirectory(dbpath);
            }
            FileInfo[] fi = new DirectoryInfo(dbpath).GetFiles();
            List<BackModel> blist = new List<BackModel>();
            foreach (FileInfo fiTemp in fi)
            {
                if (fiTemp.Name.Contains(".bak"))
                {
                    BackModel bm = new BackModel();
                    bm.CreationTime = fiTemp.CreationTime;
                    bm.fileName = fiTemp.Name;
                    blist.Add(bm);
                }
            }
            return blist;
        }
        #endregion
        #region 删除备份文件
        public static bool DelBackUp(System.Web.UI.Page page, string Xpath)
        {
            try
            {
                File.Delete(Xpath);
                return true;
            }
            catch (Exception ex)
            {
                ArrayList al = new ArrayList();
                al.Add("-------------------------删除数据库备份错误---------------------------------");
                al.Add(ex.ToString());
                WriteLog(page, al);
                return false;
            }
        }
        #endregion
        public static void WriteLog(System.Web.UI.Page page, ArrayList strLog)
        {
            string strPath;
            ErrorlogPath = page.Server.MapPath("") + "\\" + ErrorlogPath;
            strPath = string.Format("{0}\\{1}.log", ErrorlogPath, DateTime.Now.ToString("yyyy年MM月dd日"));
            using (FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (StreamWriter m_streamWriter = new StreamWriter(fs))
                {
                    m_streamWriter.BaseStream.Seek(0, SeekOrigin.End);
                    foreach (string log in strLog)
                    {
                        m_streamWriter.WriteLine(log);
                    }
                    m_streamWriter.Flush();
                    m_streamWriter.Close();
                }
                fs.Close();
            }
        }
    }
    public class BackModel
    {
        private string _fileName;
        private DateTime _CreationTime;
        public string fileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }
        public DateTime CreationTime
        {
            get { return _CreationTime; }
            set { _CreationTime = value; }
        }
    }
}
