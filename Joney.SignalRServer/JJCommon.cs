using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joney.SignalRServer
{
    public class JJCommon
    {
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
}
