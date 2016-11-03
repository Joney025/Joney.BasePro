using Joney.BasePro.Models;
using Joney.MainConsoleTest.CommonClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Config;
using System.Data;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;

namespace Joney.MainConsoleTest
{
    class Program
    {
        public static int threadNum = 10;//线程总数
        public static int threadCount = 0;//线程累计
        public static double maxTime = 0;//最大耗时
        public static double minTime = 0;//最小耗时
        public static int successCount = 0;//任务成功数
        public static int errorCount = 0;//任务失败数
        public static int totalCount = 0;//任务总数
        public static readonly string url = "cloud5.sap360.com.cn";//web api 地址 sap360.6655.la
        public static readonly string port = "41001";//web api 端口 8081
        public static readonly string postJson = "{\"DocEntry\":0,\"OrderType\":12,\"FormID\":0,\"UpdateTime\":\"0001-01-01T00:00:00\",\"FormName\":null,\"Status\":0,\"Score\":0,\"NeedScore\":1,\"IsPublic\":1,\"CreateDate\":null,\"CancelDate\":null,\"ReplyCount\":0,\"PraiseCount\":0,\"LabelText\":\"\",\"DeviceType\":0,\"DeviceName\":\"\",\"Title\":null,\"TitleType\":0,\"TitleValue\":null,\"UserSign\":0,\"UserName\":null,\"FollowUser\":0,\"FollowUserName\":null,\"UserCount\":0,\"ExecUsers\":[\"2486\"],\"ExecUserNames\":null,\"RangeDesc\":\"全公司\",\"FinishDate\":\"0001-01-01T00:00:00\",\"Report2\":\"在5月13日召开的第七次全国人民防空会议上，国家国防动员委员会对65个“全国人民防空先进城市”予以通报表彰。人力资源社会保障部、中央军委国防动员部分别授予35个单位“全国人民防空先进集体”荣誉称号；授予45名同志“全国人民防空先进工作者”荣誉称号。国家人民防空办公室对180个“全国人民防空先进单位”和270名“全国人民防空先进个人”予以通报表彰。\",\"Report3\":\"全国人民防空先进城市\\n　　北京市、天津市、河北省邯郸市、河北省廊坊市、山西省太原市、山西省晋中市、内蒙古自治区呼和浩特市、内蒙古自治区赤峰市辽宁省沈阳市、辽宁省大连市、辽宁省鞍山市、吉林省长春市、吉林省辽源市、黑龙江省哈尔滨市、黑龙江省齐齐哈尔市、上海市、江苏省南京市、江苏省常州市、江苏省苏州市、江苏省淮安市、浙江省杭州市、浙江省宁波市、浙江省嘉兴市、浙江省台州市、安徽省合肥市、安徽省淮南市、安徽省滁州市、福建省厦门市、福建省泉州市、江西省九江市、江西省上饶市、山东省济南市、山东省青岛市、山东省威海市、山东省临沂市、河南省郑州市、河南省洛阳市、河南省信阳市、湖北省武汉市、湖北省襄阳市、湖北省荆门市、湖南省长沙市、湖南省株洲市、湖南省湘潭市、广东省广州市、广东省珠海市、广东省江门市、广东省惠州市、广西壮族自治区南宁市、广西壮族自治区桂林市、海南省海口市、重庆市、四川省成都市、四川省绵阳市、四川省泸州市、贵州省贵阳市、贵州省遵义市、云南省德宏傣族景颇族自治州芒市、西藏自治区日喀则市、陕西省西安市、陕西省宝鸡市、甘肃省张掖市、青海省格尔木市、宁夏回族自治区银川市、新疆维吾尔自治区阿克苏市。\",\"Content\":\"第七次全国人民防空会议13日在京举行，国家主席习近平强调，人民防空事关人民群众生命安危、事关改革开放和现代化建设成果。要坚持人民防空为人民，把这项工作摆到战略位置、纳入“十三五”规划，与其他工作同步抓好，团结一心开创人民防空事业新局面。图为习近平主席等党和国家领导在北京会见第七次全国人民防空会议代表。\",\"RemindTime\":\"0001-01-01T00:00:00\",\"Notify\":0,\"listReplyUsers\":[],\"ReplyUsers\":[],\"listCCUsers\":[],\"CCUsers\":[],\"MePraise\":0,\"NeedMeReply\":0,\"MeReplyStatus\":0,\"ReceiptCount\":0,\"ReceiptedCount\":0,\"EndDate\":\"0001-01-01T00:00:00\",\"FlowID\":0,\"MeConcern\":0,\"BaseEntry\":0,\"DataSource\":\"\",\"IsRead\":0,\"Attachments\":[],\"ShortComments\":[],\"Comments\":null,\"BaseType\":0,\"BaseName\":null,\"Target\":null,\"SixEvents\":null}";

        public static ILog logger;

        static void Main(string[] args)
        {
            InitLog4Net();//初始化配置信息
            logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            logger.Info("Test Begin：" + DateTime.Now.ToString());

            #region Less Demo

            //var str = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
            //var strArry = str.Split(',');

            //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            //sw.Start();
            //for (int i = 0; i < strArry.Length; i++)
            //{
            //    if (strArry[i].Contains("j"))
            //    {
            //        sw.Stop();
            //        var ts = sw.ElapsedMilliseconds;
            //        Console.WriteLine(ts);
            //    }
            //}

            //System.Diagnostics.Stopwatch sw2 = new System.Diagnostics.Stopwatch();
            //sw2.Start();
            //for (int i = 0; i < strArry.Length; i++)
            //{
            //    if (strArry[i].Equals("j"))
            //    {
            //        sw2.Stop();
            //        var ts = sw2.ElapsedMilliseconds;
            //        Console.WriteLine(ts);
            //    }
            //}

            //System.Diagnostics.Stopwatch sw3 = new System.Diagnostics.Stopwatch();
            //sw3.Start();
            //for (int i = 0; i < strArry.Length; i++)
            //{
            //    if (strArry[i] == "j")
            //    {
            //        sw3.Stop();
            //        var ts = sw3.ElapsedMilliseconds;
            //        Console.WriteLine(ts);
            //    }
            //}

            //DataTable dt1 = new DataTable();
            //dt1.Columns.Add("Name", typeof(string));
            //dt1.Columns.Add("ID", typeof(string));
            //for (int i = 0; i < 5; i++)
            //{
            //    DataRow row0 = dt1.NewRow();
            //    row0["Name"] = "Francis"+i;
            //    row0["ID"] = "10001"+i;
            //    dt1.Rows.Add(row0);
            //}
            //DataRow row1 = dt1.NewRow();
            //row1["Name"] = "Joney-5";
            //row1["ID"] = "A105";
            //dt1.Rows.Add(row1);

            //DataTable dt2 = new DataTable();
            //dt2.Columns.Add("Name", typeof(string));
            //dt2.Columns.Add("ID", typeof(string));
            //for (int i = 0; i < 10; i++)
            //{
            //    DataRow row2 = dt2.NewRow();
            //    row2[0] = "Joney-"+i;
            //    row2[1] = "A10"+i;
            //    dt2.Rows.Add(row2);
            //}

            //dt1.Merge(dt2,true);

            //Console.WriteLine(dt1.Columns[0].ColumnName + " " + dt1.Columns[1].ColumnName);
            //Console.WriteLine("--------------------------------");
            //for (int i = 0; i < dt1.Rows.Count; i++)
            //{
            //    Console.WriteLine(dt1.Rows[i][0]+" "+dt1.Rows[i][1]);
            //}
            //Console.ReadKey();

            //var p1 = "F:\\WorkPlace\\百度云同步盘\\项目\\JoneyBaseProFrame\\Joney.BasePro\\Joney.BasePro\\UploadFiles\\WordFiles\\20160201052348.pdf";
            //var p2 = "";
            //var res = IOHelper.GetRelativePath(p1,p2);

            //Result=F:\WorkPlace\百度云同步盘\项目\JoneyBaseProFrame\Joney.BasePro\Joney.MainConsoleTest\bin\Debug\

            #endregion
            
            #region Demo A

            //ThreadStart tsDemo = new ThreadStart(TaskPostDataReg);
            //Stopwatch watch = new Stopwatch();
            //watch.Start();
            //var threadCount = 10;
            //for (int i = 0; i < threadCount; i++)
            //{
            //    Console.WriteLine("线程" + i + "开启"+watch.Elapsed.Milliseconds);
            //    Thread tdDemo = new Thread(tsDemo);
            //    watch.Start();
            //    logger.Info("开始时间：" + watch.Elapsed.TotalMilliseconds.ToString());//日志存档。
            //    tdDemo.Start();
            //    if (tdDemo.ThreadState == System.Threading.ThreadState.Stopped)
            //    {
            //        watch.Stop();
            //        logger.Info("中断时间：" + watch.Elapsed.TotalMilliseconds.ToString());//日志存档。
            //        break;
            //    }
            //}
            //while (true)
            //{
            //    if (tdDemo.ThreadState == System.Threading.ThreadState.Stopped)
            //    {
            //        watch.Stop();
            //        logger.Info("中断时间：" + watch.Elapsed.TotalMilliseconds.ToString());//日志存档。
            //        break;
            //    }
            //}
            #endregion

            #region API测试


            //for (int i = 1; i <= threadNum; i++)
            //{
            //    Task.Factory.StartNew(() =>
            //    {
            //        TaskPostData();
            //    });

            //    //Thread tt = new Thread(tsDemo);
            //    //tt.Start();

            //    logger.Info("线程开始(" + i + "）");
            //}

            //TimeSpan ts1 = new TimeSpan(DateTime.Now.Ticks);
            //while (true)
            //{
            //    if (threadCount == threadNum)
            //    {
            //        TimeSpan ts2 = new TimeSpan(DateTime.Now.Ticks);
            //        TimeSpan sp = ts2.Subtract(ts1).Duration();
            //        var sss = "H:" + sp.Hours + "-M:" + sp.Minutes + "-S:" + sp.Seconds + "-ML:" + sp.Milliseconds;
            //        watch.Stop();
            //        var pj = watch.Elapsed.TotalMilliseconds / totalCount;
            //        logger.Info("API调用(总耗时：" + watch.Elapsed.TotalMilliseconds.ToString() + "），执行任务：成功=" + successCount + "，失败=" + (totalCount - successCount) + ",错误=" + errorCount + ",总次=" + totalCount + ",平均=" + pj);
            //        logger.Info("Test End(" + DateTime.Now.ToString() + ")：" + sss);//日志存档。
            //        break;
            //    }
            //}

            //Console.WriteLine();
            //Console.WriteLine("Test Fineshi...");
            //Console.ReadKey();

            #endregion

            var res =Environment.CurrentDirectory;
            var res2 = AppDomain.CurrentDomain.BaseDirectory;
            var res3 = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

            Console.WriteLine(res);
            Console.WriteLine(res3);

            #region 反射

            try
            {
                {
                    //Assembly ass = Assembly.Load("itextsharp.dll");//itextsharp.dll
                    //foreach (var item in ass.GetModules())
                    //{
                    //    Console.WriteLine(item.FullyQualifiedName);
                    //}
                    //Type type = ass.GetType("itextsharp.dll");
                    //object oObj = Activator.CreateInstance(type);
                    //string idll=(string)oObj;
                    //idll.GetType();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }

            #endregion

            #region Demo B

            //LambdaShow.Show();

            //Console.WriteLine("+++++++++++++++++++++++++++++++");

            //const int num = 10000000;
            //string s1 = "junjieyu@joney";
            //string s2 = "junjieyu@joney";
            //int end;
            ////Compare:
            //int start = Environment.TickCount;
            //for (int i = 0; i < num; i++)
            //{
            //    string.Compare(s1, s2);
            //}
            //end = Environment.TickCount;
            //Console.WriteLine("Compare:"+(end-start));

            ////CompareTo:
            //start = Environment.TickCount;
            //for (int i = 0; i < num; i++)
            //{
            //    s1.CompareTo(s2);
            //}
            //end = Environment.TickCount;
            //Console.WriteLine("CompareTo:"+(end-start));

            ////CompareOrdinal:
            //start = Environment.TickCount;
            //for (int i = 0; i < num; i++)
            //{
            //    string.CompareOrdinal(s1,s2);
            //}
            //end = Environment.TickCount;
            //Console.WriteLine("CompareOrdinal:" + (end - start));

            ////静态Equals:
            //start = Environment.TickCount;
            //for (int i = 0; i < num; i++)
            //{
            //    string.Equals(s1, s2);
            //}
            //end = Environment.TickCount;
            //Console.WriteLine("静态Equals:" + (end - start));

            ////实例Equals:
            //start = Environment.TickCount;
            //for (int i = 0; i < num; i++)
            //{
            //    s1.Equals(s2);
            //}
            //end = Environment.TickCount;
            //Console.WriteLine("实例Equals:" + (end - start));

            ////==
            //start = Environment.TickCount;
            //for (int i = 0; i < num; i++)
            //{
            //    if (s1==s2)
            //    {
            //        continue;
            //    };
            //}
            //end = Environment.TickCount;
            //Console.WriteLine("==:" + (end - start));

            #endregion

            #region 数据监听

            //{
            //    string conStr = ConfigurationManager.ConnectionStrings["conStr"].ToString();
            //    string filePath = ConfigurationManager.AppSettings["fileRoot"].ToString();
            //    while (true)
            //    {
            //        string[] fileList = Directory.GetFiles(filePath);
            //        foreach (var item in fileList)
            //        {
            //            string strSql = "";
            //            using (StreamReader sr=new StreamReader(item))
            //            {
            //                string line;
            //                while ((line=sr.ReadLine())!=null)
            //                {
            //                    strSql += line + " ";
            //                }
            //                sr.Close();
            //            }
            //            try
            //            {
            //                using (SqlConnection connection=new SqlConnection(conStr))
            //                {
            //                    //connection.Open();
            //                    connection.OpenAsync();
            //                    SqlCommand cmd = new SqlCommand(strSql,connection);
            //                    cmd.CommandType = CommandType.Text;
            //                    //cmd.ExecuteNonQuery();
            //                    cmd.ExecuteNonQueryAsync();
            //                    connection.Close();
            //                }
            //            }
            //            catch (Exception ex)
            //            {
            //                Console.WriteLine(ex.Message.ToString());
            //            }
            //            File.Delete(item);
            //        }
            //        Thread.Sleep(5*1000);//每5秒执行一次
            //    }
            //}

            #endregion


            Console.ReadKey();
            
        }

        /// <summary>
        /// 发表日志
        /// </summary>
        public static void TaskPostData()
        {
            var count = 100;
            var tSuccess = 0;
            var tError = 0;
            var tTotal = 0;
            Stopwatch watch = new Stopwatch();
            watch.Start();

            while (count > 0)
            {
                totalCount++;
                //logger.Info("执行次数:"+totalCount);
                Stopwatch watch2 = new Stopwatch();
                watch2.Start();
                var sessionKey = "874b7279ab994c409bea78e76c3dda3d";//92e3674874674261a4d3297c1342a873
                var secretCode = "1";//4c20214a83504a87b4d3aff86e27302d8189144c5bb84208ba91a51698746565
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("sessionKey", sessionKey);
                string address = string.Format("api/OA/V1/Create/{0}/{1}", sessionKey, CreateToken(secretCode, dic, postJson));
                Program pg = new Program();
                var TaskRes = pg._PostHRGzip(address, postJson);
                if (TaskRes.Flag)
                {
                    successCount++;
                    tSuccess++;
                    logger.Info("成功次数:" + successCount);
                }
                else
                {
                    logger.Error("调用失败（" + TaskRes.Result + "）,耗时:" + watch2.Elapsed.TotalMilliseconds);
                    errorCount++;
                    tError++;
                }
                if (minTime == 0)
                {
                    minTime = watch2.Elapsed.TotalMilliseconds;
                }
                else
                {
                    if (minTime > watch2.Elapsed.TotalMilliseconds)
                    {
                        minTime = watch2.Elapsed.TotalMilliseconds;
                    }
                }
                if (maxTime == 0)
                {
                    maxTime = watch2.Elapsed.TotalMilliseconds;
                }
                else
                {
                    if (maxTime < watch2.Elapsed.TotalMilliseconds)
                    {
                        maxTime = watch2.Elapsed.TotalMilliseconds;
                    }
                }
                tTotal++;
                count--;
            }
            threadCount++;
            var pj = watch.Elapsed.TotalMilliseconds / tTotal;
            logger.Info("单线程调用完成（成功=" + tSuccess + "，失败=" + tError + ",总数=" + tTotal + "），单线程总耗时：" + watch.Elapsed.TotalMilliseconds + " 毫秒,最块：" + minTime + "毫秒,最慢：" + maxTime + "毫秒,平均：" + pj + "毫秒/笔");
        }

        /// <summary>
        /// 注册用户
        /// </summary>
        public static void TaskPostDataReg()
        {

            var count = 100;
            var tSuccess = 0;
            var tError = 0;
            var tTotal = 0;
            Stopwatch watch = new Stopwatch();
            watch.Start();

            while (count > 0)
            {
                totalCount++;
                //logger.Info("执行次数:"+totalCount);
                Stopwatch watch2 = new Stopwatch();
                watch2.Start();
                var guid = Guid.NewGuid().ToString("N");
                var timestamp = DateTime.Now.ToString("yyyyMMddhhmmss");
                guid = guid.Substring(0, 10);
                var postData = "{\"UserCode\":\"T" + guid + "\",\"UserName\":\"T" + timestamp + "\",\"Password\":\"827ccb0eea8a706c4c34a16891f84e7b\",\"Sex\":\"1\",\"ExtNum\":\"5421\",\"MobilePhone\":\"13896325415\",\"QQ\":\"4562583\",\"WeChat\":\"4562312@qq.com\",\"EMail\":\"\",\"PopServer\":\"\",\"SmtpServer\":\"\",\"MailPwd\":\"\",\"DftDeptName\":\"业务部\",\"Departments\":[\"总经办\",\"业务部\"],\"Roles\":[\"业务员\"],\"LeaderName\":\"Admin\",\"SuperUser\":0,\"Locked\":0,\"ReceiveDel\":0,\"Position\":\"测试\",\"Tel\":\"075565821354\",\"AssistantName\":\"\",\"PropertyItems\":[]}";
                var sessionKey = "874b7279ab994c409bea78e76c3dda3d";//92e3674874674261a4d3297c1342a873
                var secretCode = "1";//4c20214a83504a87b4d3aff86e27302d8189144c5bb84208ba91a51698746565
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("sessionKey", sessionKey);
                string address = string.Format("api/Manage/UserOpr/AddUser/{0}/{1}", sessionKey, CreateToken(secretCode, dic, postData));
                Program pg = new Program();
                var TaskRes = pg._PostHRGzip(address, postData);
                if (TaskRes.Flag)
                {
                    successCount++;
                    tSuccess++;
                    logger.Info("成功次数:" + successCount);
                }
                else
                {
                    logger.Error("调用失败（" + TaskRes.Result + "）,耗时:" + watch2.Elapsed.TotalMilliseconds);
                    errorCount++;
                    tError++;
                }
                if (minTime == 0)
                {
                    minTime = watch2.Elapsed.TotalMilliseconds;
                }
                else
                {
                    if (minTime > watch2.Elapsed.TotalMilliseconds)
                    {
                        minTime = watch2.Elapsed.TotalMilliseconds;
                    }
                }
                if (maxTime == 0)
                {
                    maxTime = watch2.Elapsed.TotalMilliseconds;
                }
                else
                {
                    if (maxTime < watch2.Elapsed.TotalMilliseconds)
                    {
                        maxTime = watch2.Elapsed.TotalMilliseconds;
                    }
                }
                tTotal++;
                count--;
            }
            threadCount++;
            var pj = watch.Elapsed.TotalMilliseconds / tTotal;
            logger.Info("单线程调用完成（成功=" + tSuccess + "，失败=" + (tTotal - tSuccess) + ",错误=" + tError + ",总数=" + tTotal + "），单线程总耗时：" + watch.Elapsed.TotalMilliseconds + " 毫秒,最块：" + minTime + "毫秒,最慢：" + maxTime + "毫秒,平均：" + pj + "毫秒/笔");
        }

        #region API接口
        public HttpResult _PostHR(string address, string param)
        {
            var hr = new HttpResult();
            try
            {
                address = string.Format("{0}/{1}", GetApiServerPath(), address);
                Uri serviceReq = new Uri(address);
                HttpContent content = new StringContent(param);

                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");//application/json

                HttpClient httpclient = new HttpClient();
                try
                {
                    HttpResponseMessage response = httpclient.PostAsync(serviceReq, content).Result;
                    if (response.StatusCode.ToString() == "OK" || response.IsSuccessStatusCode)
                    {
                        hr.Flag = true;
                        hr.Result = response.Content.ReadAsStringAsync().Result;
                    }
                    else
                    {
                        hr.Flag = false;
                        hr.StatusCode = Convert.ToInt32(response.StatusCode).ToString();
                        hr.Result = response.Content.ReadAsStringAsync().Result;
                    }
                }
                catch (AggregateException ex)
                {
                    hr.Flag = false;
                    hr.StatusCode = "Try";
                    hr.Result = ex.InnerException.InnerException.InnerException.Message.ToString();
                }
                return hr;
            }
            catch (WebException ex)
            {
                Stream receiveStream = ex.Response.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, System.Text.Encoding.UTF8);
                Char[] read = new Char[256];
                int count = readStream.Read(read, 0, 256);
                StringBuilder invokeResult = new StringBuilder();
                while (count > 0)
                {
                    invokeResult.Append(new String(read, 0, count));
                    count = readStream.Read(read, 0, 256);
                }
                readStream.Close();
                hr.Flag = false;
                hr.StatusCode = "Try";
                hr.Result = invokeResult.ToString();
                return hr;
            }
        }

        public HttpResult _GetHR(string address, string param)
        {
            var hr = new HttpResult();
            try
            {
                var result = string.Empty;
                HttpClient httpclient = new HttpClient();
                address = string.Format("{0}/{1}", GetApiServerPath(), address);
                Uri serviceReq = new Uri(address);
                try
                {
                    HttpResponseMessage response = httpclient.GetAsync(serviceReq).Result;
                    if (response.StatusCode.ToString() == "OK" || response.IsSuccessStatusCode)
                    {
                        hr.Flag = true;
                        hr.Result = response.Content.ReadAsStringAsync().Result;
                    }
                    else
                    {
                        hr.Flag = false;
                        hr.StatusCode = Convert.ToInt32(response.StatusCode).ToString();
                        hr.Result = response.Content.ReadAsStringAsync().Result;
                    }
                }
                catch (AggregateException ex)
                {
                    hr.Flag = false;
                    hr.StatusCode = "Try";
                    hr.Result = ex.InnerException.InnerException.InnerException.Message.ToString();
                }
            }
            catch (WebException ex)
            {
                Stream receiveStream = ex.Response.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, System.Text.Encoding.UTF8);
                Char[] read = new Char[256];
                int count = readStream.Read(read, 0, 256);
                StringBuilder invokeResult = new StringBuilder();
                while (count > 0)
                {
                    invokeResult.Append(new String(read, 0, count));
                    count = readStream.Read(read, 0, 256);
                }
                readStream.Close();
                hr.Flag = false;
                hr.StatusCode = "Try";
                hr.Result = invokeResult.ToString();
            }
            return hr;
        }

        public HttpResult _DeleteHR(string address, string param)
        {
            var hr = new HttpResult();
            try
            {
                address = string.Format("{0}/{1}", GetApiServerPath(), address);
                Uri serviceReq = new Uri(address);
                //HttpContent content = new StringContent(param);
                //content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpClient httpclient = new HttpClient();
                try
                {
                    //result = httpclient.DeleteAsync(serviceReq).Result.Content.ReadAsStringAsync().Result;
                    HttpResponseMessage response = httpclient.DeleteAsync(serviceReq).Result;
                    if (response.StatusCode.ToString() == "OK" || response.IsSuccessStatusCode)
                    {
                        hr.Flag = true;
                        hr.Result = response.Content.ReadAsStringAsync().Result;
                    }
                    else
                    {
                        hr.Flag = false;
                        hr.StatusCode = Convert.ToInt32(response.StatusCode).ToString();
                        hr.Result = response.Content.ReadAsStringAsync().Result;
                    }
                }
                catch (AggregateException ex)
                {
                    hr.Flag = false;
                    hr.StatusCode = "Try";
                    hr.Result = ex.InnerException.InnerException.InnerException.Message.ToString();
                }
            }
            catch (WebException ex)
            {
                Stream receiveStream = ex.Response.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, System.Text.Encoding.UTF8);
                Char[] read = new Char[256];
                int count = readStream.Read(read, 0, 256);
                StringBuilder invokeResult = new StringBuilder();
                while (count > 0)
                {
                    invokeResult.Append(new String(read, 0, count));
                    count = readStream.Read(read, 0, 256);
                }
                readStream.Close();
                hr.Flag = false;
                hr.StatusCode = "Try";
                hr.Result = invokeResult.ToString();
            }
            return hr;
        }

        public HttpResult _GetHRGzip(string address, string param)
        {
            var hr = new HttpResult();
            try
            {
                var result = string.Empty;
                var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate };
                HttpClient httpclient = new HttpClient(handler);
                address = string.Format("{0}/{1}", GetApiServerPath(), address);
                Uri serviceReq = new Uri(address);
                try
                {
                    //result = httpclient.GetStringAsync(serviceReq).Result;
                    HttpResponseMessage response = httpclient.GetAsync(serviceReq).Result;
                    if (response.StatusCode.ToString() == "OK" || response.IsSuccessStatusCode)
                    {
                        hr.Flag = true;
                        hr.Result = response.Content.ReadAsStringAsync().Result;
                    }
                    else
                    {
                        hr.Flag = false;
                        hr.StatusCode = Convert.ToInt32(response.StatusCode).ToString();
                        hr.Result = response.Content.ReadAsStringAsync().Result;
                    }
                }
                catch (AggregateException ex)
                {
                    hr.Flag = false;
                    hr.StatusCode = "Try";
                    hr.Result = ex.InnerException.InnerException.InnerException.Message.ToString();
                }
            }
            catch (WebException ex)
            {
                Stream receiveStream = ex.Response.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, System.Text.Encoding.UTF8);
                Char[] read = new Char[256];
                int count = readStream.Read(read, 0, 256);
                StringBuilder invokeResult = new StringBuilder();
                while (count > 0)
                {
                    invokeResult.Append(new String(read, 0, count));
                    count = readStream.Read(read, 0, 256);
                }
                readStream.Close();
                hr.Flag = false;
                hr.StatusCode = "Try";
                hr.Result = invokeResult.ToString();
            }
            return hr;
        }

        public HttpResult _PostHRGzip(string address, string param)
        {
            var hr = new HttpResult();
            try
            {
                var result = string.Empty;
                address = string.Format("{0}/{1}", GetApiServerPath(), address);
                Uri serviceReq = new Uri(address);
                HttpContent content = new StringContent(param);

                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate };
                HttpClient httpclient = new HttpClient(handler);
                try
                {
                    HttpResponseMessage response = httpclient.PostAsync(serviceReq, content).Result;
                    if (response.StatusCode.ToString() == "OK" || response.IsSuccessStatusCode)
                    {
                        hr.Flag = true;
                        hr.Result = response.Content.ReadAsStringAsync().Result;
                    }
                    else
                    {
                        hr.Flag = false;
                        hr.StatusCode = Convert.ToInt32(response.StatusCode).ToString();
                        hr.Result = response.Content.ReadAsStringAsync().Result;
                    }
                }
                catch (AggregateException ex)
                {
                    var exMsg = "";
                    foreach (Exception e in ex.InnerExceptions)
                    {
                        exMsg += e.Message.ToString() + "\n";
                    }
                    hr.Flag = false;
                    hr.StatusCode = "Try";
                    hr.Result = exMsg;
                }
            }
            catch (WebException ex)
            {
                Stream receiveStream = ex.Response.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, System.Text.Encoding.UTF8);
                Char[] read = new Char[256];
                int count = readStream.Read(read, 0, 256);
                StringBuilder invokeResult = new StringBuilder();
                while (count > 0)
                {
                    invokeResult.Append(new String(read, 0, count));
                    count = readStream.Read(read, 0, 256);
                }
                readStream.Close();
                hr.Flag = false;
                hr.StatusCode = "Try";
                hr.Result = invokeResult.ToString();
            }
            return hr;
        }

        /// <summary>
        /// 获取API URL
        /// </summary>
        /// <returns></returns>
        public static string GetApiServerPath()
        {
            if (url.Contains("http:") || url.Contains("https:"))
            {
                return string.Format("{0}:{1}", url, port);
            }
            else
            {
                return string.Format("http://{0}:{1}", url, port);
            }
        }

        #endregion

        #region 辅助方法

        public static string CreateToken(string code, Dictionary<string, string> param, string post)
        {
            StringBuilder sbu = new StringBuilder();
            Dictionary<string, string> ParamToUpper = new Dictionary<string, string>();
            IList<string> acs = new List<string>();
            string temp = "";
            foreach (var item in param.OrderBy(o => o.Key))
            {
                acs.Add(item.Key.ToUpper());
                ParamToUpper.Add(item.Key.ToUpper(), param[item.Key]);
            }
            for (int i = 0; i < acs.Count - 1; i++)
            {
                for (int j = 0; j < acs.Count - 1 - i; j++)
                {
                    if (string.CompareOrdinal(acs[j], acs[j + 1]) > 0)
                    {
                        temp = acs[j + 1];
                        acs[j + 1] = acs[j];
                        acs[j] = temp;
                    }
                }
            }
            sbu.Append(code);
            foreach (var str in acs)
            {
                sbu.Append(str);
                sbu.Append(ParamToUpper[str]);
            }
            sbu.Append(post);
            sbu.Append(code);
            return (EncryptMD5(sbu.ToString()));
        }

        public static string EncryptMD5(string str)
        {
            string crypStr = string.Empty;
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(str));
            md5.Clear();
            for (int i = 0; i < result.Length; i++)
            {
                //crypStr += string.Format("{0:x}",result[i]);
                crypStr += result[i].ToString("x").PadLeft(2, '0');
            }
            return crypStr;
        }

        /// <summary>
        /// 实例化日志配置信息:注意配置文件属性必须为 复制到输出目录：始终复制
        /// </summary>
        private static void InitLog4Net()
        {
            var logCfg = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
            XmlConfigurator.ConfigureAndWatch(logCfg);
        }

        #endregion
    }

    public class HttpResult
    {
        /// <summary>
        /// api请求结果
        /// </summary>
        public bool Flag { get; set; }

        /// <summary>
        /// api请求状态码：string 类型
        /// </summary>
        public string StatusCode { get; set; }

        /// <summary>
        /// api请求返回值：string 类型
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// api请求返回值：byte[] 类型
        /// </summary>
        public byte[] ByteData { get; set; }
    }
}
