using Joney.BasePro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Joney.BasePro.Controllers
{
    
    [MyFilter.ExceptionAttribut]//异常
    public class BaseController : Controller
    {
        

        /// <summary>
        /// 生成Token值
        /// </summary>
        /// <param name="code">安全密匙</param>
        /// <param name="param">参数键值</param>
        /// <param name="post">API请求的参数</param>
        /// <returns></returns>
        public string CreateToken(string code, Dictionary<string, string> param, string post)
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

            return (MyCommon.EncryptMD5(sbu.ToString()));
        }
    }


}