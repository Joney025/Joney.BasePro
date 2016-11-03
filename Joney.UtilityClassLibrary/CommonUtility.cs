using System;
using System.Collections.Generic;
using System.Text;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Json;
using System.Web;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Net;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Drawing;
using System.Data;
using Microsoft.Win32;

namespace Joney.UtilityClassLibrary
{
    public static class CommonUtility
        {
            /// <summary>
            /// 序列化表头-DataTable
            /// </summary>
            /// <param name="dt">当前表</param>
            /// <param name="dmCode">当前的表头</param>
            /// <param name="dmName">转换后表头</param>
            /// <returns></returns>
            public static DataTable SerializeData(DataTable dt, string[] dmCode, string[] dmName)
            {
                DataTable dtNew = new DataTable();
                dtNew = dt.Copy();
                //去除非数组存在的字段
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    int inCol = 0;
                    string colName = dt.Columns[i].ColumnName.ToString().ToLower();
                    for (int j = 0; j < dmCode.Length; j++)
                    {
                        string codName = dmCode[j].ToString().ToLower();
                        if (colName == codName)
                        {
                            inCol += 1;
                            break;
                        }
                    }
                    if (inCol <= 0)
                    {
                        dtNew.Columns.Remove(dt.Columns[i].ColumnName);
                    }
                }
                //表头标题中英文对换
                for (int i = 0; i < dtNew.Columns.Count; i++)
                {
                    string colName = dtNew.Columns[i].ColumnName.ToString().ToLower();
                    for (int j = 0; j < dmCode.Length; j++)
                    {
                        string codName = dmCode[j].ToString().ToLower();
                        if (colName == codName)
                        {
                            dtNew.Columns[i].ColumnName = dmName[j].ToString();
                            break;
                        }
                    }
                }
                //按照数组字段顺序重新调整数据表列顺序
                for (int i = 0; i < dtNew.Columns.Count; i++)
                {
                    dtNew.Columns[dmName[i].ToString()].SetOrdinal(i);
                }
                return dtNew;
            }

            /// <summary>
            /// 去除DataTable中的空行
            /// </summary>
            /// <param name="dt"></param>
            /// <returns></returns>
            public static DataTable removeEmptyTableRow(DataTable dt)
            {
                DataTable dtNew = new DataTable();
                List<DataRow> removelist = new List<DataRow>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    bool rowdataisnull = true;
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {

                        if (!string.IsNullOrEmpty(dt.Rows[i][j].ToString().Trim()))
                        {

                            rowdataisnull = false;
                        }

                    }
                    if (rowdataisnull)
                    {
                        removelist.Add(dt.Rows[i]);
                    }

                }
                for (int i = 0; i < removelist.Count; i++)
                {
                    dt.Rows.Remove(removelist[i]);
                }
                dtNew = dt.Copy();
                return dtNew;
            }

            /// <summary>
            /// JSON字符串转装Object对象。
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="jsons"></param>
            /// <returns></returns>
            public static T JsonToObject<T>(string jsons)
            {
                try
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                    using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsons)))
                    {
                        var obj = (T)serializer.ReadObject(stream);
                        stream.Close();
                        return obj;
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }

            /// <summary>
            /// JSON字符串转装Object对象。
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="jsons"></param>
            /// <returns></returns>
            public static T JsonToObj<T>(string jsons)
            {
                try
                {
                    var obj = JsonConvert.DeserializeObject<T>(jsons);
                    return obj;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// object转装json
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public static string ObjectToJson(object obj)
            {
                string res = string.Empty;
                try
                {
                    res = JsonConvert.SerializeObject(obj);
                }
                catch (Exception ex)
                {
                    return ex.Message.ToString();
                }
                return res;
            }

            /// <summary>
            /// DataTable转装Json
            /// </summary>
            /// <param name="dt"></param>
            /// <returns></returns>
            public static string DataTableToJson(DataTable dt)
            {
                string res = string.Empty;
                try
                {
                    res = JsonConvert.SerializeObject(dt, new DataTableConverter());
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return res;
            }

            /// <summary>
            /// Json转装DataTable
            /// </summary>
            /// <param name="jsons"></param>
            /// <returns></returns>
            public static DataTable JsonConvertToDataTable(string jsons)
            {
                try
                {
                    DataTable dt = new DataTable();
                    dt = (DataTable)JsonConvert.DeserializeObject<DataTable>(jsons);
                    return dt;
                }
                catch (Exception)
                {

                    throw;
                }

            }

            /// <summary>
            /// 将json转换为DataTable
            /// </summary>
            /// <param name="strJson">得到的json</param>
            /// <returns></returns>
            public static DataTable JsonToDataTable(string strJson)
            {
                //转换json格式
                strJson = strJson.Replace(",\"", "*\"").Replace("\":", "\"#").ToString();
                //取出表名   
                var rg = new Regex(@"(?<={)[^:]+(?=:\[)", RegexOptions.IgnoreCase);
                string strName = rg.Match(strJson).Value;
                DataTable tb = null;
                //去除表名   
                strJson = strJson.Substring(strJson.IndexOf("[") + 1);
                strJson = strJson.Substring(0, strJson.IndexOf("]"));

                //获取数据   
                rg = new Regex(@"(?<={)[^}]+(?=})");
                MatchCollection mc = rg.Matches(strJson);
                for (int i = 0; i < mc.Count; i++)
                {
                    string strRow = mc[i].Value;
                    string[] strRows = strRow.Split('*');

                    //创建表   
                    if (tb == null)
                    {
                        tb = new DataTable();
                        tb.TableName = strName;
                        foreach (string str in strRows)
                        {
                            var dc = new DataColumn();
                            string[] strCell = str.Split('#');

                            if (strCell[0].Substring(0, 1) == "\"")
                            {
                                int a = strCell[0].Length;
                                dc.ColumnName = strCell[0].Substring(1, a - 2);
                            }
                            else
                            {
                                dc.ColumnName = strCell[0];
                            }
                            tb.Columns.Add(dc);
                        }
                        tb.AcceptChanges();
                    }

                    //增加内容   
                    DataRow dr = tb.NewRow();
                    for (int r = 0; r < strRows.Length; r++)
                    {
                        dr[r] = strRows[r].Split('#')[1].Trim().Replace("，", ",").Replace("：", ":").Replace("\"", "");
                    }
                    tb.Rows.Add(dr);
                    tb.AcceptChanges();
                }

                return tb;
            }

            /// <summary>
            /// json转List<T>对象集合
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="jsons"></param>
            /// <returns></returns>
            public static List<T> JsonStrToObjectList<T>(string jsons)
            {
                List<T> list = new List<T>();
                try
                {
                    list = JsonConvert.DeserializeObject<List<T>>(jsons);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return list;
            }


        #region 跨平台加密算法


            // 密匙
            private const string DefaultKey = "P)(%&#*~<>diuej8ApU!Wm,#@:3TuoiQ";  //KeySize = 256

            // 向量（CBC 模式用）
            private const string DefaultIV = "0001020304050607"; //16个字节，默认BlockSize=128


            /// <summary>
            /// 跨平台加密（CBC模式）
            /// </summary>
            /// <param name="strEncrypt">待加密内容</param>
            /// <param name="strKey">密匙</param>
            /// <param name="strIV">向量</param>
            /// <returns></returns>
            public static string PlatformEncrypt(string strEncrypt, string strKey = DefaultKey, string strIV = DefaultIV)
            {
                strKey = strKey ?? DefaultKey;
                if (strKey.Length != 32 && strKey.Length != 24 && strKey.Length != 16)
                    throw new ArgumentOutOfRangeException("密匙长度异常，非128/192/256位！");
                strIV = strIV ?? DefaultIV;
                if (strIV.Length != 16)
                    throw new ArgumentOutOfRangeException("向量长度异常，非128位");

                using (RijndaelManaged rDel = new RijndaelManaged())
                {
                    rDel.Key = UTF8Encoding.UTF8.GetBytes(strKey);
                    rDel.IV = UTF8Encoding.UTF8.GetBytes(strIV);
                    rDel.Padding = PaddingMode.PKCS7;
                    rDel.Mode = CipherMode.CBC;

                    byte[] arrEncrypt = UTF8Encoding.UTF8.GetBytes(strEncrypt);
                    ICryptoTransform cTransform = rDel.CreateEncryptor();
                    byte[] resultArray = cTransform.TransformFinalBlock(arrEncrypt, 0, arrEncrypt.Length);

                    return Convert.ToBase64String(resultArray, 0, resultArray.Length);
                }
            }

            /// <summary>
            /// 跨平台解密
            /// </summary>
            /// <param name="strDecrypt">待解密的内容，强制Base64String 字符串</param>
            /// <param name="strKey">密匙</param>
            /// <param name="strIV">向量（CBC 模式用）</param>
            /// <returns></returns>
            public static string PlatformDecrypt(string strDecrypt, string strKey = DefaultKey, string strIV = DefaultIV)
            {
                strKey = strKey ?? DefaultKey;
                if (strKey.Length != 32 && strKey.Length != 24 && strKey.Length != 16)
                    throw new ArgumentOutOfRangeException("密匙长度异常，非128/192/256位！");
                strIV = strIV ?? DefaultIV;
                if (strIV.Length != 16)
                    throw new ArgumentOutOfRangeException("向量长度异常，非128位");

                using (RijndaelManaged rDel = new RijndaelManaged())
                {
                    rDel.Key = UTF8Encoding.UTF8.GetBytes(strKey);
                    rDel.IV = UTF8Encoding.UTF8.GetBytes(strIV);
                    rDel.Padding = PaddingMode.PKCS7;
                    rDel.Mode = CipherMode.CBC;

                    byte[] arrDecrypt = Convert.FromBase64String(strDecrypt);   //TODO. strDecrypt 非64位格式，转换失效的异常【是否这里需要强制验证】
                    ICryptoTransform cTransform = rDel.CreateDecryptor();
                    byte[] resultArray = cTransform.TransformFinalBlock(arrDecrypt, 0, arrDecrypt.Length);

                    return UTF8Encoding.UTF8.GetString(resultArray);
                }
            }

            /// <summary>
            /// MD5加密
            /// </summary>
            /// <param name="password"></param>
            /// <returns></returns>
            public static string Encrypt(string password)
            {
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                string password_md5 = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(password, "MD5");
                return password_md5;
            }

            /// <summary>
            /// 获取文件MD5码
            /// </summary>
            /// <param name="fileName"></param>
            /// <returns></returns>
            public static string GetMD5HashFormFile(string fileName)
            {
                try
                {
                    FileStream file = new FileStream(fileName, System.IO.FileMode.Open);
                    MD5 md5 = new MD5CryptoServiceProvider();
                    byte[] retVal = md5.ComputeHash(file);
                    file.Close();
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < retVal.Length; i++)
                    {
                        sb.Append(retVal[i].ToString("x2"));
                    }
                    return sb.ToString();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error:" + ex.Message.ToString());
                }
            }


            /// <summary>
            /// 有密码的AES加密 
            /// </summary>
            /// <param name="text">加密字符</param>
            /// <param name="password">加密的密码</param>
            /// <param name="iv">密钥</param>
            /// <returns></returns>
            public static string AESEncrypt(string text, string password, string iv)
            {
                RijndaelManaged rijndaelCipher = new RijndaelManaged();

                rijndaelCipher.Mode = CipherMode.CBC;

                rijndaelCipher.Padding = PaddingMode.PKCS7;

                rijndaelCipher.KeySize = 128;

                rijndaelCipher.BlockSize = 128;

                byte[] pwdBytes = System.Text.Encoding.UTF8.GetBytes(password);

                byte[] keyBytes = new byte[16];

                int len = pwdBytes.Length;

                if (len > keyBytes.Length) len = keyBytes.Length;

                System.Array.Copy(pwdBytes, keyBytes, len);

                rijndaelCipher.Key = keyBytes;


                byte[] ivBytes = System.Text.Encoding.UTF8.GetBytes(iv);
                rijndaelCipher.IV = ivBytes;

                ICryptoTransform transform = rijndaelCipher.CreateEncryptor();

                byte[] plainText = Encoding.UTF8.GetBytes(text);

                byte[] cipherBytes = transform.TransformFinalBlock(plainText, 0, plainText.Length);

                return Convert.ToBase64String(cipherBytes);

            }

            /// <summary>
            /// 随机生成密钥
            /// </summary>
            /// <returns></returns>
            public static string GetIv(int n)
            {
                char[] arrChar = new char[]{
           'a','b','d','c','e','f','g','h','i','j','k','l','m','n','p','r','q','s','t','u','v','w','z','y','x',
           '0','1','2','3','4','5','6','7','8','9',
           'A','B','C','D','E','F','G','H','I','J','K','L','M','N','Q','P','R','T','S','V','U','W','X','Y','Z'
          };

                StringBuilder num = new StringBuilder();

                Random rnd = new Random(DateTime.Now.Millisecond);
                for (int i = 0; i < n; i++)
                {
                    num.Append(arrChar[rnd.Next(0, arrChar.Length)].ToString());

                }

                return num.ToString();
            }

            /// <summary>
            /// AES解密
            /// </summary>
            /// <param name="text"></param>
            /// <param name="password"></param>
            /// <param name="iv"></param>
            /// <returns></returns>
            public static string AESDecrypt(string text, string password, string iv)
            {
                RijndaelManaged rijndaelCipher = new RijndaelManaged();

                rijndaelCipher.Mode = CipherMode.CBC;

                rijndaelCipher.Padding = PaddingMode.PKCS7;

                rijndaelCipher.KeySize = 128;

                rijndaelCipher.BlockSize = 128;

                byte[] encryptedData = Convert.FromBase64String(text);

                byte[] pwdBytes = System.Text.Encoding.UTF8.GetBytes(password);

                byte[] keyBytes = new byte[16];

                int len = pwdBytes.Length;

                if (len > keyBytes.Length) len = keyBytes.Length;

                System.Array.Copy(pwdBytes, keyBytes, len);

                rijndaelCipher.Key = keyBytes;

                byte[] ivBytes = System.Text.Encoding.UTF8.GetBytes(iv);
                rijndaelCipher.IV = ivBytes;

                ICryptoTransform transform = rijndaelCipher.CreateDecryptor();

                byte[] plainText = transform.TransformFinalBlock(encryptedData, 0, encryptedData.Length);

                return Encoding.UTF8.GetString(plainText);

            }


            /// <summary>
            /// UTF-8转Base64编码
            /// </summary>
            /// <param name="code"></param>
            /// <returns></returns>
            public static string EncryptBase64(string code)
            {
                string res = string.Empty;
                byte[] b = System.Text.Encoding.UTF8.GetBytes(code);
                res = Convert.ToBase64String(b);
                return res;
            }

            public static string ReplaceEncodeBase64(string code)
            {
                if (string.IsNullOrEmpty(code)) return string.Empty;
                string base64string = Convert.ToBase64String(Encoding.UTF8.GetBytes(code));
                return base64string.Replace("/", "_a").Replace("+", "_b").Replace("=", "_c");
            }

            public static string ReplaceDecodeBase64(string code)
            {
                if (string.IsNullOrEmpty(code)) return string.Empty;
                string temp = code.Replace("_a", "/").Replace("_b", "+").Replace("_c", "=");
                return Encoding.UTF8.GetString(Convert.FromBase64String(temp));
            }

            /// <summary>
            /// Base64解码
            /// </summary>
            /// <param name="code"></param>
            /// <returns></returns>
            public static string DecodeBase64(string code)
            {
                string res = string.Empty;
                byte[] c = Convert.FromBase64String(code);
                res = System.Text.Encoding.UTF8.GetString(c);
                return res;
            }

            /// <summary>
            /// Base64编码
            /// </summary>
            /// <param name="code_type">文本编码格式：utf-8？defualt</param>
            /// <param name="code"></param>
            /// <returns></returns>
            public static string EncodeBase64(string code_type, string code)
            {
                string encode = "";
                byte[] bytes = Encoding.GetEncoding(code_type).GetBytes(code);
                try
                {
                    encode = Convert.ToBase64String(bytes);
                }
                catch
                {
                    encode = code;
                }
                return encode;
            }

            /// <summary>
            /// Base64解码
            /// </summary>
            /// <param name="code_type"></param>
            /// <param name="code"></param>
            /// <returns></returns>
            public static string DecodeBase64(string code_type, string code)
            {
                string decode = "";
                byte[] bytes = Convert.FromBase64String(code);
                try
                {
                    decode = Encoding.GetEncoding(code_type).GetString(bytes);
                }
                catch
                {
                    decode = code;
                }
                return decode;
            }


            #endregion



            /// <summary>
            /// 返回一个随机数
            /// </summary>
            /// <param name="num">随机数位数</param>
            /// <returns></returns>
            public static string RandomNum(int num)
            {
                string random = string.Empty;
                Random rd = new Random();
                for (int i = 0; i < num; i++)
                {
                    random += rd.Next(0, 10).ToString();
                }
                return random;
            }

            /// <summary>
            /// 解析获取XML某节点内容
            /// </summary>
            /// <param name="xml"></param>
            /// <param name="nodeName"></param>
            /// <returns></returns>
            public static string _GetXMLNode(string xml, string nodeName)
            {
                string res = string.Empty;
                var xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(xml);
                var node = xmlDoc.SelectSingleNode(nodeName);
                res = node.InnerText;
                return res;
            }

            /// <summary>
            /// 获取IP地址
            /// </summary>
            /// <returns></returns>
            public static string _GetClientIPAddress()
            {
                //string serverIP = Request.UserHostAddress;
                //string localIP = Request.ServerVariables[0];
                string ip = string.Empty;
                string address = string.Empty;
                string mac = string.Empty;
                //IPHostEntry ipHE = Dns.GetHostByName(Dns.GetHostName());
                //IPAddress Ad = new IPAddress(ipHE.AddressList[0].Address);
                //ip = address.ToString();


                System.Net.IPAddress[] ad = Dns.GetHostByName(Dns.GetHostName()).AddressList;
                if (ad.Length > 1)
                {
                    IPAddress Ad = new IPAddress(ad[0].Address);
                    ip = Ad.ToString();
                    address = ad[1].ToString();
                }
                else
                {
                    IPAddress Ad = new IPAddress(ad[0].Address);
                    ip = Ad.ToString();
                    ip = ad[0].ToString();
                    address = "Break The Line...";
                }

                return ip + "_" + address;
            }

            /// <summary>
            /// 获取本机IP地址
            /// </summary>
            /// <returns></returns>
            public static string _GetClientIP()
            {
                string ip = string.Empty;
                if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
                {
                    ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(new char[] { ',' })[0];
                }
                else
                {
                    ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                if (string.IsNullOrEmpty(ip))
                {
                    ip = HttpContext.Current.Request.UserHostAddress;
                }
                return ip;
            }

            [DllImport("Iphlpapi.dll")]
            private static extern int SendARP(Int32 dest, Int32 host, ref Int64 mac, ref Int32 length);
            [DllImport("Ws2_32.dll")]
            private static extern Int32 inet_addr(string ip);
            /// <summary>
            /// 获取MAC地址1(访问该服务的客户端MAC)
            /// </summary>
            /// <returns></returns>
            public static string _GetMacAddress()
            {
                string mac = string.Empty;
                try
                {
                    string userip = HttpContext.Current.Request.UserHostAddress;
                    string strClientIP = userip.ToString().Trim();
                    Int32 ldest = inet_addr(strClientIP);
                    Int32 lhost = inet_addr("");
                    Int64 macinfo = new Int64();
                    Int32 len = 6;
                    int res = SendARP(ldest, 0, ref macinfo, ref len);
                    string mac_src = macinfo.ToString("X");
                    if (mac_src == "0")
                    {
                        if (userip == "127.0.0.1")
                        {
                            mac = "";
                        }
                        else
                        {
                            mac = userip;
                        }
                        return mac;
                    }
                    while (mac_src.Length < 12)
                    {
                        mac_src = mac_src.Insert(0, "0");
                    }
                    string mac_dest = "";
                    for (int i = 0; i < 11; i++)
                    {
                        if ((i % 2) == 0)
                        {
                            if (i == 10)
                            {
                                mac_dest = mac_dest.Insert(0, mac_src.Substring(i, 2));
                            }
                            else
                            {
                                mac_dest = "-" + mac_dest.Insert(0, mac_src.Substring(i, 2));
                            }
                        }
                    }

                    return mac = mac_dest;
                }
                catch (Exception)
                {

                    throw;
                }
            }

            /// <summary>
            /// 获取MAC地址2(本程序所运行的服务器MAC)
            /// </summary>
            /// <returns></returns>
            public static string GetMacAddressByNetworkInformation()
            {
                string key = "SYSTEM\\CurrentControlSet\\Control\\Network\\{4D36E972-E325-11CE-BFC1-08002BE10318}\\";
                string macAddress = string.Empty;
                try
                {
                    NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                    foreach (NetworkInterface adapter in nics)
                    {
                        if (adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet
                            && adapter.GetPhysicalAddress().ToString().Length != 0)
                        {
                            string fRegistryKey = key + adapter.Id + "\\Connection";
                            RegistryKey rk = Registry.LocalMachine.OpenSubKey(fRegistryKey, false);
                            if (rk != null)
                            {
                                string fPnpInstanceID = rk.GetValue("PnpInstanceID", "").ToString();
                                int fMediaSubType = Convert.ToInt32(rk.GetValue("MediaSubType", 0));
                                if (fPnpInstanceID.Length > 3 &&
                                    fPnpInstanceID.Substring(0, 3) == "PCI")
                                {
                                    macAddress = adapter.GetPhysicalAddress().ToString();
                                    for (int i = 1; i < 6; i++)
                                    {
                                        macAddress = macAddress.Insert(3 * i - 1, ":");
                                    }
                                    break;
                                }
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    //这里写异常的处理
                }
                return macAddress;
            }

            /// <summary>
            /// 获取图片字节流数据
            /// </summary>
            /// <param name="imgPath"></param>
            /// <returns></returns>
            public static byte[] GetPictureData(string imgPath)
            {
                FileStream fs = new FileStream(imgPath, FileMode.Open);
                byte[] byteData = new byte[fs.Length];
                fs.Read(byteData, 0, byteData.Length);
                fs.Close();
                return byteData;
            }

            /// <summary>
            /// 图片文件转字节流数据
            /// </summary>
            /// <param name="img"></param>
            /// <returns></returns>
            public static byte[] PictureConverter(System.Drawing.Image img)
            {
                MemoryStream ms = new MemoryStream();
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                byte[] byteData = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(byteData, 0, byteData.Length);
                ms.Close();
                return byteData;
            }

            /// <summary>
            /// 图片字节流转换成图片
            /// </summary>
            /// <param name="data"></param>
            /// <param name="savePath"></param>
            /// <returns></returns>
            public static Image ByteConvertImage(byte[] data, string savePath)
            {
                MemoryStream ms = new MemoryStream(data);
                Image image = Image.FromStream(ms);
                string result = string.Empty;
                try
                {
                    image.Save(savePath);
                }
                catch (Exception e)
                {
                    result = e.Message.ToString();
                }
                finally
                {
                    image.Dispose();
                }
                return image;
            }

            #region 说明

            //设置Cookie
            //var randPWD = MyUtil.RandomNum(6);
            //Response.Cookies["UPWD"].Value = randPWD;
            //Response.Cookies["UPWD"].Expires = DateTime.Now.AddHours(1);

            //HttpCookie myCookie = new HttpCookie("UPWD");
            //myCookie.Value = randPWD;
            //myCookie.Expires = DateTime.Now.AddHours(1);
            //Response.Cookies.Add(myCookie);


            #endregion

        }
    }
