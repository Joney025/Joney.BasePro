using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Xml.Linq;

namespace Joney.BasePro.Models
{
    public class MyCommon
    {
        /// <summary>
        ///  获取票证用户信息
        /// </summary>
        /// <returns></returns>
        public static MyTicket GetTicket()
        {
            
            MyTicket myticket = new MyTicket();
            if (HttpContext.Current.Request.IsAuthenticated)
            {
                HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                myticket = JsonConvert.DeserializeObject<MyTicket>(ticket.UserData);
            }
            else
            {
                FormsAuthentication.SignOut();
            }
            return myticket;
        }

        /// <summary>
        /// 获取票证用户信息
        /// </summary>
        /// <returns></returns>
        public static MyCookie GetCookie()
        {
            MyCookie Cookie = new MyCookie();
            var cook = HttpContext.Current.Request.Cookies["MyCookie"];
            if (HttpContext.Current.Request.IsAuthenticated && cook != null)
            {
                Cookie = JsonConvert.DeserializeObject<MyCookie>(HttpUtility.UrlDecode(cook.Value.ToString(), Encoding.GetEncoding("UTF-8")));
            }
            else
            {
                FormsAuthentication.SignOut();
            }
            return Cookie;
        }

        /// <summary>
        /// 获取角色配置
        /// </summary>
        public class GetRoles
        {
            public static string GetActionRoles(string action, string controller)
            {
                XElement rootElement = XElement.Load(HttpContext.Current.Server.MapPath("/") + "ActionRoles.xml");
                XElement controllerElement = findElementByAttribute(rootElement, "Controller", controller);
                if (controllerElement != null)
                {
                    XElement actionElement = findElementByAttribute(controllerElement, "Action", action);
                    if (actionElement != null)
                    {
                        return actionElement.Value;
                    }
                }
                return "";
            }

            public static XElement findElementByAttribute(XElement xElement, string tagName, string attribute)
            {
                return xElement.Elements(tagName).FirstOrDefault(x => x.Attribute("name").Value.Equals(attribute, StringComparison.OrdinalIgnoreCase));
            }
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
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
    }

    public class MyTicket {
        public string UserName { get; set; }
        public int UserSign { get; set; }
        public string SessionKey { get; set; }
        public string SecretCode { get; set; }
    }

    public class MyCookie
    {
        public string UserSign { get; set; }

        public string UserName { get; set; }

        public bool SuperUser { get; set; }
    }
}