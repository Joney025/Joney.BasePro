using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Collections;
using System.Xml;
using System.Web;
using System.Web.UI.WebControls;

namespace Joney.UtilityClassLibrary
{
    public static class DBHelper
    {
        private static SqlConnection con;
        private static OleDbConnection ocon;
        private static OdbcConnection orcon;

        //<connectionStrings>
        //  <add name="SqlConStr" connectionString="Data Source=localhost;Initial Catalog=myData;Persist Security Info=True;User ID=sa;Password=123456;pooling=true;min pool size=5;max pool size=10"/>／／／sqlServer数据库
        //  <add name="OledbConStr" connectionString="Provider=Microsoft.Jet.OLEDB.4.0;Data Source=E:\db1.mdb"/>／／／Acces数据库连接
        //  <add name="OracleConStr" connectionString="Password=sa;User ID=sa;Data Source=MyDB"/>
        //</connectionStrings>
        /// <summary>
        /// 连接数据库？
        /// </summary>
        public static SqlConnection Con
        {
            get
            {
                string conString = ConfigurationManager.ConnectionStrings["conStr"].ConnectionString.ToString();
                if (con == null)
                {
                    con = new SqlConnection(conString);
                    con.Open();
                }
                else if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }
                else if (con.State == System.Data.ConnectionState.Broken)
                {
                    con.Close();
                    con.Open();
                }
                return DBHelper.con;
            }
        }
        /// <summary>
        /// 连接ＡＣＣＥＳＳ数据库？
        /// </summary>
        public static OleDbConnection Ocon
        {
            get
            {
                string conString = ConfigurationManager.ConnectionStrings["oconStr"].ConnectionString.ToString();
                if (ocon == null)
                {
                    ocon = new OleDbConnection(conString);
                    ocon.Open();
                }
                else if (ocon.State == System.Data.ConnectionState.Closed)
                {
                    ocon.Open();
                }
                else if (ocon.State == System.Data.ConnectionState.Broken)
                {
                    ocon.Close();
                    ocon.Open();
                }
                return DBHelper.ocon;
            }
        }
        /// <summary>
        /// Oracle数据库链接
        /// </summary>
        public static OdbcConnection Orcon
        {
            get
            {
                string OracleConStr = ConfigurationManager.ConnectionStrings["OracleConStr"].ConnectionString.ToString();
                if (orcon == null)
                {
                    orcon = new OdbcConnection(OracleConStr);
                }
                else if (orcon.State == System.Data.ConnectionState.Closed)
                {
                    orcon.Open();
                }
                else if (orcon.State == System.Data.ConnectionState.Broken)
                {
                    orcon.Close();
                    orcon.Open();
                }
                return DBHelper.Orcon;
            }
        }

        public static XmlDataDocument GetXMLData(string str)
        {
            XmlDataDocument xd = new XmlDataDocument();
            DataSet ds = new DataSet();
            OdbcDataAdapter od = new OdbcDataAdapter(str, Orcon);

            od.Fill(ds, "MyTB");
            XmlDataDocument doc = new XmlDataDocument(ds);
            doc.Save(Console.Out);
            if (true)
            {
                XmlNode root1 = doc.DocumentElement;
                XmlNodeList roots = root1.SelectNodes("list");
                foreach (XmlNode item in roots)
                {
                    XmlElement link = xd.CreateElement("Sn");
                    link.InnerText = ConfigurationSettings.AppSettings["Sn"].ToString();
                    item.AppendChild(link);
                }
                return doc;
            }
            else
            {
                return null;
            }
        }

        #region 文本符号替换
        public static string GetHtmlEditReplace(string html)
        {
            string ss = "";
            bool ff = HasDangerousContents(html);
            if (!ff)
            {
                ss = html.Replace("'", "''").Replace("&nbsp", " ").Replace(",", ", ").Replace("%", "％").Replace("script", "").Replace(".js", "");
            }
            return ss;
        }
        #endregion
        /// <summary>
        /// 检测是否含有危险字符（防止Sql注入）？
        /// </summary>
        /// <param name="contents">预检测的内容</param>
        /// <returns>返回True或false</returns>
        public static bool HasDangerousContents(string contents)
        {
            bool bReturnValue = false;
            if (contents.Length > 0)
            {
                //convert to lower
                string sLowerStr = contents.ToLower();
                //RegularExpressions
                string sRxStr = @"(\sand\s)|(\sand\s)|(\slike\s)|(select\s)|(insert\s)|(delete\s)|(update\s[\s\S].*\sset)|(create\s)|(\stable)|(<[iframe|/iframe|script|/script])|(')|(\sexec)|(\sdeclare)|(\struncate)|(\smaster)|(\sbackup)|(\smid)|(\scount)";
                //Match
                bool bIsMatch = false;
                System.Text.RegularExpressions.Regex sRx = new System.Text.RegularExpressions.Regex(sRxStr);
                bIsMatch = sRx.IsMatch(sLowerStr, 0);
                if (bIsMatch)
                {
                    bReturnValue = true;
                }
            }
            return bReturnValue;
        }
        /// <summary>
        /// 数字密码加密？
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string Encrypt(string password)
        {
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            string password_md5 = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(password, "MD5");
            return password_md5;
        }

        #region SQL数据操作
        /// <summary>
        /// 执行sql语句/存储过程，返回影响的行数？
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <returns></returns>
        public static int ExecuteCommand(string sqlStr)
        {
            int result = 0;
            SqlCommand cmd = new SqlCommand(sqlStr, Con);
            try
            {
                result = cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (SqlException se)
            {
                throw se;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
            return result;
        }
        /// <summary>
        /// 执行sql语句，返回影响的行数？
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static int ExecuteCommandByParam(string sqlStr, params SqlParameter[] values)
        {
            int result = 0;
            SqlCommand cmd = new SqlCommand(sqlStr, Con);
            try
            {
                if (values != null)
                { //判断是否有参数 

                    foreach (SqlParameter para in values)//循环添加参数 
                    {
                        cmd.Parameters.Add(para);
                    }
                }
                result = cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (SqlException se)
            {
                throw se;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }

            return result;
        }
        /// <summary>
        /// 执行cmd,返回一个结果？
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static int ExecuteNonQueryByCmd(SqlCommand cmd)
        {
            int Result = 0;
            try
            {
                cmd.Connection = Con;
                Result = cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Result;
        }
        /// <summary>
        ///  执行sql语句，返回影响的行数？
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string sqlStr, string[] paramName, string[] paramValue)
        {
            int result = 0;

            SqlCommand cmd = new SqlCommand(sqlStr, Con);
            try
            {
                for (int i = 0; i < paramName.Length; i++)
                {
                    cmd.Parameters.AddWithValue(paramName[i], paramValue[i]);
                }
                result = cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (SqlException se)
            {
                throw se;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
            return result;
        }
        /// <summary>
        /// 执行sql语句/存储过程，返回第一行的第一列，比如插入新记录，返回自增的id？
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <returns></returns>
        public static int ExecuteScalar(string sqlStr)
        {
            int result = 0;
            SqlCommand cmd = new SqlCommand(sqlStr, Con);
            try
            {
                Object obj = cmd.ExecuteScalar();

                if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                {
                    result = 0;
                }
                else
                {
                    result = Convert.ToInt32(obj);
                }
                con.Close();
            }
            catch (SqlException se)
            {
                throw se;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
            return result;
        }
        /// <summary>
        /// 执行cmd，返回得到的结果--
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static int ExecuteSclarByCmd(SqlCommand cmd)
        {
            int flag = 0;
            try
            {
                cmd.Connection = Con;
                Object Result = cmd.ExecuteScalar();
                if ((Object.Equals(Result, null)) || (Object.Equals(Result, System.DBNull.Value)))
                {
                    return 0;
                }
                if (Result != null)
                {
                    flag = Convert.ToInt32(Result);
                }
                else
                {
                    flag = 0;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
            return flag;
        }
        /// <summary>
        ///  执行sql语句(带参数组)，返回影响的行数？
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static int GetScalarByParamArray(string sqlStr, params SqlParameter[] values)
        {
            int result = 0;
            SqlCommand cmd = new SqlCommand(sqlStr, Con);
            try
            {
                cmd.Parameters.AddRange(values);
                object obj = cmd.ExecuteScalar();
                if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                {
                    return 0;
                }
                else
                {
                    result = Convert.ToInt32(obj);
                }
                con.Close();
            }
            catch (SqlException se)
            {
                throw se;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
            return result;
        }
        /// <summary>
        ///  执行sql语句，返回一个Object？
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        /// <returns></returns>
        public static object ExecuteScalarByParam(string sqlStr, string[] paramName, string[] paramValue)
        {
            object obj = null;
            SqlCommand cmd = new SqlCommand(sqlStr, Con);
            try
            {
                for (int i = 0; i < paramName.Length; i++)
                {
                    cmd.Parameters.AddWithValue(paramName[i], paramValue[i]);
                }
                obj = cmd.ExecuteScalar();
                if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                {
                    return null;
                }
            }
            catch (SqlException se)
            {
                throw se;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
            return obj;
        }
        /// <summary>
        /// 返回一个Object？
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static object GetObject(string sql)
        {
            object obj = null;
            try
            {
                SqlCommand cmd = new SqlCommand(sql, Con);
                obj = cmd.ExecuteScalar();
            }
            catch (SqlException se)
            {
                throw se;
            }
            return obj;
        }
        /// <summary>
        /// 返回一个Object？
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static object GetObjectByParam(string sqlStr, params SqlParameter[] values)
        {
            object obj = null;
            try
            {
                SqlCommand cmd = new SqlCommand(sqlStr, Con);
                cmd.Parameters.AddRange(values);
                obj = cmd.ExecuteScalar();
            }
            catch (SqlException se)
            {
                throw se;
            }
            return obj;
        }
        /// <summary>
        /// 执行sql语句/存储过程，返回一个DataReader？
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <returns></returns>
        public static SqlDataReader GetReader(string sqlStr)
        {
            SqlDataReader reader = null;
            SqlCommand cmd = new SqlCommand(sqlStr, Con);
            try
            {
                reader = cmd.ExecuteReader();
                //reader.Close();
            }
            catch (SqlException se)
            {
                throw se;
            }
            finally
            {
                cmd.Dispose();
                //if (reader != null)
                //{
                //    reader.Close();
                //}
                //con.Close();
            }
            return reader;
        }
        /// <summary>
        /// 返回一个DataReader？
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static SqlDataReader GetReaderByParam(string sqlStr, params SqlParameter[] values)
        {
            SqlDataReader reader = null;
            SqlCommand cmd = new SqlCommand(sqlStr, Con);
            try
            {
                cmd.Parameters.AddRange(values);
                reader = cmd.ExecuteReader();
                cmd.Parameters.Clear();
                //con.Close();
            }
            catch (SqlException se)
            {
                throw se;
            }
            finally
            {
                cmd.Dispose();
                //if (reader != null) { reader.Close(); }
                //con.Close();
            }
            return reader;
        }
        /// <summary>
        /// 执行sql语句/存储过程，返回一个DataTable？
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static DataTable GetDataSet(string sqlStr)
        {
            SqlCommand cmd = new SqlCommand(sqlStr, Con);
            DataSet ds = new DataSet();
            SqlDataAdapter da = null;
            try
            {
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();
            }
            catch (SqlException se)
            {
                throw se;
            }
            finally
            {
                ///da.Dispose();
                cmd.Dispose();
                con.Close();
            }
            return ds.Tables[0];
        }
        /// <summary>
        /// 检测数据行是否存在
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static int CheckDataBySet(string sqlStr, params SqlParameter[] values)
        {
            int res = 0;
            SqlCommand cmd = new SqlCommand(sqlStr, Con);
            DataSet ds = new DataSet();
            SqlDataAdapter da = null;
            try
            {
                cmd.Parameters.AddRange(values);
                da = new SqlDataAdapter(cmd);
                da.Fill(ds.Tables["tb"]);
                con.Close();
            }
            catch (SqlException se)
            {
                throw se;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
            return Convert.ToInt32(ds.Tables[0].Rows[0][1]);
        }
        /// <summary>
        /// 返回一个DataTable？
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static DataTable GetDataSetByParam(string sqlStr, params SqlParameter[] values)
        {
            SqlCommand cmd = new SqlCommand(sqlStr, Con);
            DataSet ds = new DataSet();
            SqlDataAdapter da = null;
            try
            {
                cmd.Parameters.AddRange(values);
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();
            }
            catch (SqlException se)
            {
                throw se;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
            return ds.Tables[0];
        }
        /// <summary>
        /// 执行带参数SQL语句，返回一张表？
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        /// <returns></returns>
        public static DataTable GetDataTableByParam(string sqlStr, string[] paramName, string[] paramValue)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = null; ;
            SqlDataAdapter da = null;
            try
            {
                cmd = new SqlCommand(sqlStr, Con);
                for (int i = 0; i < paramName.Length; i++)
                {
                    cmd.Parameters.AddWithValue(paramName[i], paramValue[i]);
                }
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (SqlException se)
            {
                throw se;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
            return dt;
        }
        /// <summary>
        /// 执行SqlCommand命令，并返回一张表?
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static DataTable GetDataTableByCmd(SqlCommand cmd)
        {
            DataTable dt = new DataTable();
            cmd.Connection = con;
            SqlDataAdapter da = null;
            try
            {
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (SqlException se)
            {
                throw se;
            }
            finally
            {
                da.Dispose();
            }
            return dt;
        }
        /// <summary>
        /// 执行带参数的存储过程，返回一张表？--查
        /// </summary>
        /// <param name="produre"></param>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        /// <returns></returns>
        public static DataTable GetDataTableByProdure(string produre, string[] paramName, string[] paramValue)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter da = null;
            try
            {
                cmd = new SqlCommand(produre, Con);
                cmd.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < paramName.Length; i++)
                {
                    cmd.Parameters.AddWithValue(paramName[i], paramValue[i]);
                }
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (SqlException se)
            {
                throw se;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
            return dt;
        }
        /// <summary>
        /// 执行带参数的存储过程，并返回受影响的行数？--增、删、改
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        /// <returns></returns>
        public static int ExecuteProcedure(string procedureName, string[] paramName, string[] paramValue)
        {
            SqlCommand cmd = new SqlCommand(procedureName, Con);
            for (int i = 0; i < paramName.Length; i++)
            {
                cmd.Parameters.AddWithValue(paramName[i], paramValue[i]);
            }
            cmd.CommandType = CommandType.StoredProcedure;
            int result = cmd.ExecuteNonQuery();
            return result;
        }
        /// <summary>
        /// 执行存储过程-返回影响行数？--删、改、查
        /// </summary>
        /// <param name="procedureName"></param>
        /// <returns></returns>
        public static int ExecuteProcedureByName(string procedureName)
        {
            SqlCommand cmd = new SqlCommand(procedureName, Con);
            cmd.CommandType = CommandType.StoredProcedure;
            int result = cmd.ExecuteNonQuery();
            con.Close();
            return result;
        }

        /// <summary>
        /// 执行存储过程 返回影响的数据行数--添加/修改/删除
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static int Add_Modify_Del_ExecuteProcedureByPrams(string procedureName, params SqlParameter[] values)
        {
            int res = 0;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(procedureName, Con);
                cmd.CommandType = CommandType.StoredProcedure;
                foreach (SqlParameter parameter in values)
                {
                    cmd.Parameters.Add(parameter);
                }
                res = cmd.ExecuteNonQuery();
                Con.Close();
            }
            catch (SqlException se)
            {
                throw se;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
            return res;
        }
        /// <summary>
        /// 通过存储过程返回数据表
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static DataSet GetInfo_ExecuteProcedureByPrams(string procedureName, params SqlParameter[] values)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = null;
            try
            {
                da = new SqlDataAdapter(procedureName, Con);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.Add(values);
                da.Fill(ds, "table");
            }
            catch (SqlException se)
            {
                throw se;
            }
            finally
            {
                con.Close();
            }
            return ds;
        }

        /// <summary>
        /// 分页存储过程--万能通用
        /// </summary>
        /// <param name="tableName">要查询的表名</param>
        /// <param name="fieldStr">要查询的字段（*为全部）</param>
        /// <param name="orderStr">排序字段（只写字段即可，不用加order by）</param>
        /// <param name="whereStr">条件语句</param>
        /// <param name="pageSize">每页显示几条数据</param>
        /// <param name="pageIndex">当前第几页</param>
        /// <param name="count">返回的记录总数</param>
        /// <returns>返回DataTable数据集</returns>
        public static DataTable GetPager(string tableName, string fieldStr, string orderStr, string whereStr, int pageSize, int pageIndex, ref int count)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter da = null;
            try
            {
                cmd = new SqlCommand("GetPager", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TableName", tableName);
                cmd.Parameters.AddWithValue("@ReFieldsStr", fieldStr);
                cmd.Parameters.AddWithValue("@OrderString", orderStr);
                cmd.Parameters.AddWithValue("@WhereString", whereStr);
                cmd.Parameters.AddWithValue("@PageSize", pageSize);
                cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
                SqlParameter paramPageCount = cmd.Parameters.Add("@TotalRecord", SqlDbType.Int);
                paramPageCount.Direction = ParameterDirection.Output;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                count = Convert.ToInt32(paramPageCount.Value);
            }
            catch (SqlException se)
            {
                throw se;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
            return dt;
        }

        #endregion

        #region 操作ACCESS数据库
        /// 执行sql语句/存储过程，返回影响的行数？
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <returns></returns>
        public static int OExecuteCommand(string sqlStr)
        {
            int result = 0;
            OleDbCommand cmd = new OleDbCommand(sqlStr, Ocon);
            try
            {
                result = cmd.ExecuteNonQuery();
                ocon.Close();
            }
            catch (SqlException se)
            {
                throw se;
            }
            finally
            {
                cmd.Dispose();
                ocon.Close();
            }
            return result;
        }
        /// <summary>
        /// 执行sql语句，返回影响的行数？
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static int OExecuteCommandByParam(string sqlStr, params OleDbParameter[] values)
        {
            int result = 0;
            OleDbCommand cmd = new OleDbCommand(sqlStr, Ocon);
            try
            {
                if (values != null)
                { //判断是否有参数 

                    foreach (OleDbParameter para in values)//循环添加参数 
                    {

                        cmd.Parameters.Add(para);
                    }

                }
                //cmd.Parameters.AddRange(values);
                result = cmd.ExecuteNonQuery();
                ocon.Close();
            }
            catch (OleDbException se)
            {
                throw se;
            }
            finally
            {
                cmd.Dispose();
                ocon.Close();
            }

            return result;
        }
        /// <summary>
        /// 执行cmd,返回一个结果？
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static int OExecuteNonQueryByCmd(OleDbCommand cmd)
        {
            int Result = 0;
            try
            {
                cmd.Connection = Ocon;
                Result = cmd.ExecuteNonQuery();
            }
            catch (OleDbException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Result;
        }
        /// <summary>
        ///  执行sql语句，返回影响的行数？
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        /// <returns></returns>
        public static int OExecuteNonQuery(string sqlStr, string[] paramName, string[] paramValue)
        {
            int result = 0;

            OleDbCommand cmd = new OleDbCommand(sqlStr, Ocon);
            try
            {
                for (int i = 0; i < paramName.Length; i++)
                {
                    cmd.Parameters.AddWithValue(paramName[i], paramValue[i]);
                }
                result = cmd.ExecuteNonQuery();
                ocon.Close();
            }
            catch (OleDbException se)
            {
                throw se;
            }
            finally
            {
                cmd.Dispose();
                ocon.Close();
            }
            return result;
        }
        /// <summary>
        /// 执行sql语句/存储过程，返回第一行的第一列，比如插入新记录，返回自增的id？
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <returns></returns>
        public static int OExecuteScalar(string sqlStr)
        {
            int result = 0;
            OleDbCommand cmd = new OleDbCommand(sqlStr, Ocon);
            try
            {
                //result = Convert.ToInt32(cmd.ExecuteScalar());
                Object obj = cmd.ExecuteScalar();
                if (obj != null)
                {
                    result = Convert.ToInt32(obj);
                }
                else
                {
                    result = 0;
                }
                ocon.Close();
            }
            catch (OleDbException se)
            {
                throw se;
            }
            finally
            {
                cmd.Dispose();
                ocon.Close();
            }
            return result;
        }
        /// <summary>
        /// 执行cmd，返回得到的结果--
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static int OExecuteSclarByCmd(OleDbCommand cmd)
        {
            int flag = 0;
            try
            {
                cmd.Connection = Ocon;
                Object Result = cmd.ExecuteScalar();
                if (Result != null)
                {
                    flag = Convert.ToInt32(Result);
                }
                else
                {
                    flag = 0;
                }
            }
            catch (OleDbException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                cmd.Dispose();
                ocon.Close();
            }
            return flag;
        }
        /// <summary>
        ///  执行sql语句，返回影响的行数？
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static int OGetScalarByParamArray(string sqlStr, params OleDbParameter[] values)
        {
            int result = 0;
            OleDbCommand cmd = new OleDbCommand(sqlStr, Ocon);
            try
            {
                cmd.Parameters.AddRange(values);
                result = Convert.ToInt32(cmd.ExecuteScalar());
                ocon.Close();
            }
            catch (SqlException se)
            {
                throw se;
            }
            finally
            {
                cmd.Dispose();
                ocon.Close();
            }
            return result;
        }
        /// <summary>
        ///  执行sql语句，返回一个Object？
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        /// <returns></returns>
        public static object OExecuteScalarByParam(string sqlStr, string[] paramName, string[] paramValue)
        {
            object obj = null;
            OleDbCommand cmd = new OleDbCommand(sqlStr, Ocon);
            try
            {
                for (int i = 0; i < paramName.Length; i++)
                {
                    cmd.Parameters.AddWithValue(paramName[i], paramValue[i]);
                }
                obj = cmd.ExecuteScalar();
                ocon.Close();
            }
            catch (OleDbException se)
            {
                throw se;
            }
            finally
            {
                cmd.Dispose();
                ocon.Close();
            }
            return obj;
        }
        /// <summary>
        /// 返回一个Object？
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static object OGetObject(string sql)
        {
            object obj = null;
            try
            {
                OleDbCommand cmd = new OleDbCommand(sql, Ocon);
                obj = cmd.ExecuteScalar();
            }
            catch (OleDbException se)
            {
                throw se;
            }
            return obj;
        }
        /// <summary>
        /// 返回一个Object？
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static object OGetObjectByParam(string sqlStr, params OleDbParameter[] values)
        {
            object obj = null;
            try
            {
                OleDbCommand cmd = new OleDbCommand(sqlStr, Ocon);
                cmd.Parameters.AddRange(values);
                obj = cmd.ExecuteScalar();
            }
            catch (OleDbException se)
            {
                throw se;
            }
            return obj;
        }
        /// <summary>
        /// 执行sql语句/存储过程，返回一个DataReader？
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <returns></returns>
        public static OleDbDataReader OGetReader(string sqlStr)
        {
            OleDbDataReader reader = null;
            OleDbCommand cmd = new OleDbCommand(sqlStr, Ocon);
            try
            {
                reader = cmd.ExecuteReader();
                //reader.Close();
            }
            catch (OleDbException se)
            {
                throw se;
            }
            finally
            {
                cmd.Dispose();
                //if (reader != null)
                //{
                //    reader.Close();
                //}
                //con.Close();
            }
            return reader;
        }
        /// <summary>
        /// 返回一个DataReader？
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static OleDbDataReader OGetReaderByParam(string sqlStr, params OleDbParameter[] values)
        {
            OleDbDataReader reader = null;
            OleDbCommand cmd = new OleDbCommand(sqlStr, Ocon);
            try
            {
                cmd.Parameters.AddRange(values);
                reader = cmd.ExecuteReader();
                cmd.Parameters.Clear();
                //con.Close();
            }
            catch (OleDbException se)
            {
                throw se;
            }
            finally
            {
                cmd.Dispose();
                //if (reader != null) { reader.Close(); }
                //con.Close();
            }
            return reader;
        }
        /// <summary>
        /// 执行sql语句/存储过程，返回一个DataTable？
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static DataTable OGetDataSet(string sqlStr)
        {
            OleDbCommand cmd = new OleDbCommand(sqlStr, Ocon);
            DataSet ds = new DataSet();
            OleDbDataAdapter da = null;
            try
            {
                da = new OleDbDataAdapter(cmd);
                da.Fill(ds);
                ocon.Close();
            }
            catch (OleDbException se)
            {
                throw se;
            }
            finally
            {
                cmd.Dispose();
                ocon.Close();
            }
            return ds.Tables[0];
        }
        /// <summary>
        /// 返回一个DataTable？
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static DataTable OGetDataSetByParam(string sqlStr, params OleDbParameter[] values)
        {
            OleDbCommand cmd = new OleDbCommand(sqlStr, Ocon);
            DataSet ds = new DataSet();
            OleDbDataAdapter da = null;
            try
            {
                cmd.Parameters.AddRange(values);
                da = new OleDbDataAdapter(cmd);
                da.Fill(ds);
                ocon.Close();
            }
            catch (OleDbException se)
            {
                throw se;
            }
            finally
            {
                cmd.Dispose();
                ocon.Close();
            }
            return ds.Tables[0];
        }
        /// <summary>
        /// 执行带参数SQL语句，返回一张表？
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        /// <returns></returns>
        public static DataTable OGetDataTableByParam(string sqlStr, string[] paramName, string[] paramValue)
        {
            DataTable dt = new DataTable();
            OleDbCommand cmd = null; ;
            OleDbDataAdapter da = null;
            try
            {
                cmd = new OleDbCommand(sqlStr, Ocon);
                for (int i = 0; i < paramName.Length; i++)
                {
                    cmd.Parameters.AddWithValue(paramName[i], paramValue[i]);
                }
                da = new OleDbDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (SqlException se)
            {
                throw se;
            }
            finally
            {
                cmd.Dispose();
                ocon.Close();
            }
            return dt;
        }
        /// <summary>
        /// 执行带参数的存储过程，返回一张表？
        /// </summary>
        /// <param name="produre"></param>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        /// <returns></returns>
        public static DataTable OGetDataTableByProdure(string produre, string[] paramName, string[] paramValue)
        {
            DataTable dt = new DataTable();
            OleDbCommand cmd = null;
            OleDbDataAdapter da = null;
            try
            {
                cmd = new OleDbCommand(produre, Ocon);
                cmd.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < paramName.Length; i++)
                {
                    cmd.Parameters.AddWithValue(paramName[i], paramValue[i]);
                }
                da = new OleDbDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (OleDbException se)
            {
                throw se;
            }
            finally
            {
                cmd.Dispose();
                ocon.Close();
            }
            return dt;
        }
        /// <summary>
        /// 执行SqlCommand命令，并返回一张表?
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static DataTable OGetDataTableByCmd(OleDbCommand cmd)
        {
            DataTable dt = new DataTable();
            cmd.Connection = ocon;
            OleDbDataAdapter da = null;
            try
            {
                da = new OleDbDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (OleDbException se)
            {
                throw se;
            }
            finally
            {
                da.Dispose();
            }
            return dt;
        }
        /// <summary>
        /// 执行带参数的存储过程，并返回受影响的行数？
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        /// <returns></returns>
        public static int OExecuteProcedure(string procedureName, string[] paramName, string[] paramValue)
        {
            OleDbCommand cmd = new OleDbCommand(procedureName, Ocon);
            for (int i = 0; i < paramName.Length; i++)
            {
                cmd.Parameters.AddWithValue(paramName[i], paramValue[i]);
            }
            cmd.CommandType = CommandType.StoredProcedure;
            int result = cmd.ExecuteNonQuery();
            return result;
        }
        /// <summary>
        /// 返回影响行数？
        /// </summary>
        /// <param name="procedureName"></param>
        /// <returns></returns>
        public static int OExecuteProcedureByName(string procedureName)
        {
            OleDbCommand cmd = new OleDbCommand(procedureName, Ocon);
            cmd.CommandType = CommandType.StoredProcedure;
            int result = cmd.ExecuteNonQuery();
            ocon.Close();
            return result;
        }
        #endregion

        #region 数据还原--另类
        /// <summary>
        /// 高手恶徒
        /// </summary>
        private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());
        public static int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {

            SqlCommand cmd = new SqlCommand();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
        }
        public static int ExecuteNonQuery(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {

            SqlCommand cmd = new SqlCommand();

            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }
        public static int ExecuteNonQuery(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }
        public static SqlDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }
        public static object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
        }
        public static object ExecuteScalar(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {

            SqlCommand cmd = new SqlCommand();

            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return val;
        }
        public static void CacheParameters(string cacheKey, params SqlParameter[] commandParameters)
        {
            parmCache[cacheKey] = commandParameters;
        }
        public static SqlParameter[] GetCachedParameters(string cacheKey)
        {
            SqlParameter[] cachedParms = (SqlParameter[])parmCache[cacheKey];

            if (cachedParms == null)
                return null;

            SqlParameter[] clonedParms = new SqlParameter[cachedParms.Length];

            for (int i = 0, j = cachedParms.Length; i < j; i++)
                clonedParms[i] = (SqlParameter)((ICloneable)cachedParms[i]).Clone();

            return clonedParms;
        }
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }

        #endregion

        #region 数据转换类型

        /// <summary>
        /// 把IList 转换成DataTable
        /// </summary>
        /// <param name="ResList"></param>
        /// <returns></returns>
        public static DataSet ListToDataSet(IList ResList)
        {
            DataSet RDS = new DataSet();
            DataTable TempDT = new DataTable();

            //此处遍历IList的结构并建立同样的DataTable
            System.Reflection.PropertyInfo[] p = ResList[0].GetType().GetProperties();
            foreach (System.Reflection.PropertyInfo pi in p)
            {
                TempDT.Columns.Add(pi.Name, System.Type.GetType(pi.PropertyType.ToString()));
            }

            for (int i = 0; i < ResList.Count; i++)
            {
                IList TempList = new ArrayList();
                //将IList中的一条记录写入ArrayList
                foreach (System.Reflection.PropertyInfo pi in p)
                {
                    object oo = pi.GetValue(ResList[i], null);
                    TempList.Add(oo);
                }

                object[] itm = new object[p.Length];
                //遍历ArrayList向object[]里放数据
                for (int j = 0; j < TempList.Count; j++)
                {
                    itm.SetValue(TempList[j], j);
                }
                //将object[]的内容放入DataTable
                TempDT.LoadDataRow(itm, true);
            }
            //将DateTable放入DataSet
            RDS.Tables.Add(TempDT);
            //返回DataSet
            return RDS;
        }

        /// <summary>
        /// 把IList集合转换成DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="iWellOils"></param>
        /// <returns></returns>
        public static DataTable ParseToDataTable<T>(IList<T> iWellOils)
        {
            DataTable dt = new DataTable();
            if (iWellOils.Count > 0)
            {
                System.Reflection.PropertyInfo[] p = iWellOils[0].GetType().GetProperties();
                foreach (System.Reflection.PropertyInfo pi in p)
                {
                    // 处理指定类型为可空类型的情况
                    Type pt = pi.PropertyType;
                    if (pt.IsGenericType && pt.GetGenericTypeDefinition() == typeof(Nullable<>))
                        pt = Nullable.GetUnderlyingType(pt);
                    dt.Columns.Add(pi.Name, pt);
                }
                for (int i = 0; i < iWellOils.Count; i++)
                {
                    object[] values = new object[dt.Columns.Count];
                    int k = 0;
                    foreach (System.Reflection.PropertyInfo pi in p)
                    {
                        values.SetValue(pi.GetValue(iWellOils[i], null), k);
                        k++;
                    }
                    dt.LoadDataRow(values, true);
                }
            }
            return dt;
        }

        /// <summary>  
        /// 把一个一维数组转换为DataTable  
        /// ArrayToDataTable.Convert("haha", new string[] { "1", "2", "3", "4", "5", "6" });
        /// </summary>  
        public static DataTable ConvertToTable(string ColumnName, string[] Array)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(ColumnName, typeof(string));

            for (int i = 0; i < Array.Length; i++)
            {
                DataRow dr = dt.NewRow();
                dr[ColumnName] = Array[i].ToString();
                dt.Rows.Add(dr);
            }
            return dt;
        }

        /// <summary>
        /// 表转换一位数组
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string[] TableToArray(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return new string[0];
            }
            string[] sr = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (Convert.IsDBNull(dt.Rows[i][0]))
                {
                    sr[i] = "";
                }
                else
                {
                    sr[i] = dt.Rows[i][0] + "";
                }
            }
            return sr;
        }

        public static int[,] TableToArray2(DataTable dt)
        {
            int row = dt.Rows.Count;
            int col = dt.Columns.Count;
            int[,] tb = new int[row, col];
            for (int r = 0; r < row; r++)
            {
                for (int c = 0; c < col; c++)
                {
                    tb[r, c] = int.Parse(dt.Rows[r][c].ToString());
                }
            }
            return tb;
        }

        /// <summary>  
        /// 反一个M行N列的二维数组转换为DataTable  
        /// ArrayToDataTable.Convert(new string[] { "haha1", "haha2", "haha3" }, array3D);
        /// </summary>  
        public static DataTable ConvertToTable(string[] ColumnNames, string[,] Arrays)
        {
            DataTable dt = new DataTable();

            foreach (string ColumnName in ColumnNames)
            {
                dt.Columns.Add(ColumnName, typeof(string));
            }

            for (int i1 = 0; i1 < Arrays.GetLength(0); i1++)
            {
                DataRow dr = dt.NewRow();
                for (int i = 0; i < ColumnNames.Length; i++)
                {
                    dr[i] = Arrays[i1, i].ToString();
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        /// <summary>  
        /// 反一个M行N列的二维数组转换为DataTable  
        /// ArrayToDataTable.Convert(array3D);
        /// </summary>  
        public static DataTable ConvertToTable(string[,] Arrays)
        {
            DataTable dt = new DataTable();

            int a = Arrays.GetLength(0);
            for (int i = 0; i < Arrays.GetLength(1); i++)
            {
                dt.Columns.Add("col" + i.ToString(), typeof(string));
            }

            for (int i1 = 0; i1 < Arrays.GetLength(0); i1++)
            {
                DataRow dr = dt.NewRow();
                for (int i = 0; i < Arrays.GetLength(1); i++)
                {
                    dr[i] = Arrays[i1, i].ToString();
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }


        #endregion

        #region DataList分页
        /// <summary>
        /// DataList 分页
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="datalistname"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public static string GetPageNum(DataTable ds, DataList datalistname, int pagesize)
        {
            PagedDataSource objPds = new PagedDataSource();
            objPds.DataSource = ds.DefaultView;
            objPds.AllowPaging = true;
            int total = ds.Rows.Count;
            objPds.PageSize = pagesize;
            int page;
            if (HttpContext.Current.Request.QueryString["page"] != null)
                page = Convert.ToInt32(HttpContext.Current.Request.QueryString["page"]);
            else
                page = 1;
            objPds.CurrentPageIndex = page - 1;
            datalistname.DataSource = objPds;
            datalistname.DataBind();
            int allpage = 0;
            int next = 0;
            int pre = 0;
            int startcount = 0;
            int endcount = 0;
            string pagestr = "";

            if (page < 1) { page = 1; }
            //计算总页数
            if (pagesize != 0)
            {
                allpage = (total / pagesize);
                allpage = ((total % pagesize) != 0 ? allpage + 1 : allpage);
                allpage = (allpage == 0 ? 1 : allpage);
            }
            next = page + 1;
            pre = page - 1;
            startcount = (page + 5) > allpage ? allpage - 9 : page - 4;//中间页起始序号
            //中间页终止序号
            endcount = page < 5 ? 10 : page + 5;
            if (startcount < 1) { startcount = 1; } //为了避免输出的时候产生负数，设置如果小于1就从序号1开始
            if (allpage < endcount) { endcount = allpage; } //页码+5的可能性就会产生最终输出序号大于总页码，那么就要将其控制在页码数之内
            pagestr = "共" + allpage + "页&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";

            pagestr += page > 1 ? "<a href=\"" + HttpContext.Current.Request.CurrentExecutionFilePath + "?page=1\">首页</a>&nbsp;&nbsp;<a href=\"" + HttpContext.Current.Request.CurrentExecutionFilePath + "?page=" + pre + "\">上一页</a>" : "首页 上一页";
            //中间页处理，这个增加时间复杂度，减小空间复杂度
            for (int i = startcount; i <= endcount; i++)
            {
                pagestr += page == i ? "&nbsp;&nbsp;<font color=\"#ff0000\">" + i + "</font>" : "&nbsp;&nbsp;<a href=\"" + HttpContext.Current.Request.CurrentExecutionFilePath + "?page=" + i + "\">" + i + "</a>";
            }
            pagestr += page != allpage ? "&nbsp;&nbsp;<a href=\"" + HttpContext.Current.Request.CurrentExecutionFilePath + "?page=" + next + "\">下一页</a>&nbsp;&nbsp;<a href=\"" + HttpContext.Current.Request.CurrentExecutionFilePath + "?page=" + allpage + "\">末页</a>" : " 下一页 末页";

            return pagestr;
        }
        /// <summary>
        /// 获取传入数据表的数据量--总数量
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static int GetPageCount(int pageSize, string tableName)
        {
            int c = 0;
            ///int ps = 1;
            DataTable dt = new DataTable();
            SqlDataAdapter da = null;
            try
            {
                SqlCommand cmd = Con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[GetPageSize]";
                cmd.Parameters.AddWithValue("@pagesiz", pageSize);
                cmd.Parameters.AddWithValue("@tb", tableName);
                Con.Open();
                cmd.ExecuteNonQuery();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            finally
            {
                con.Close();
            }
            return c = dt.Rows.Count;
        }
        /// <summary>
        /// 数据分页--传表分页
        /// </summary>
        /// <param name="tableName">传入的数据库表名</param>
        /// <param name="pageSize">数据量</param>
        /// <param name="curentPage">当前页</param>
        /// <returns></returns>
        public static DataTable GetPageSelect(string tableName, int pageSize, int curentPage)
        {
            DataTable dt = null;
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                SqlCommand cmd = Con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[PageSelect]";
                cmd.Parameters.AddWithValue("@tb", tableName);
                cmd.Parameters.AddWithValue("@pagesize", pageSize);
                cmd.Parameters.AddWithValue("@curentpage", curentPage);
                Con.Open();
                cmd.ExecuteNonQuery();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
            return dt;
        }
        #endregion
    }
}
