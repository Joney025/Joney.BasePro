using System;
using System.Text;
using System.CodeDom;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Xml.Serialization;
using System.Reflection;
using System.Web;
using System.Net;
using System.IO;
using System.Web.Services.Description;

namespace Joney.UtilityClassLibrary
{
    public static class SMSService
    {

        public static readonly string smsCID = System.Configuration.ConfigurationManager.AppSettings["smsCID"];//机构编码
        public static readonly string smsAccount = System.Configuration.ConfigurationManager.AppSettings["smsID"];//账户名称
        public static readonly string smsPassword = System.Configuration.ConfigurationManager.AppSettings["smsPwd"];//账户密码
        public static readonly string smsWSDL = System.Configuration.ConfigurationManager.AppSettings["smsWSDL"];//接口地址
        public static readonly string smsCL = System.Configuration.ConfigurationManager.AppSettings["smsName"];//接口类名
        public static readonly string smsFC = System.Configuration.ConfigurationManager.AppSettings["smsFuntion"];//接口方法
        /// <summary>
        /// 短信单发
        /// </summary>
        public static string PostSingle(string phone, string msg)
        {
            object[] args = new object[7];
            args[0] = smsCID + ":" + smsAccount;//登陆标识：企业编号:账号
            args[1] = smsPassword;//密码
            args[2] = "13252012594";//发送短信号码
            args[3] = phone;//接收短信号码
            args[4] = msg;//短信内容
            args[5] = "";//发送时间
            args[6] = "";//是否语音
            var res = InvokeWebService(smsWSDL, smsCL, smsFC, args).ToString();//此方法①：即时生成&调用服务DLL
            //var res = InvokeWebServiceNew(smsWSDL, smsCL, smsFC, args).ToString();//次方法②：调用程序启动时已生成的服务DLL
            return res;
        }

        /// <summary>
        /// 返回结果处理
        /// </summary>
        public static string ResultCode(string response)
        {
            string result = string.Empty;
            switch (response)
            {
                case "1000":
                    result = "成功";
                    break;
                case "1001":
                    result = "用户不存在或密码出错";
                    break;
                case "1002":
                    result = "用户被停用";
                    break;
                case "1003":
                    result = "余额不足";
                    break;
                case "1004":
                    result = "请求频繁";
                    break;
                case "1005":
                    result = "内容超长";
                    break;
                case "1006":
                    result = "非法手机号码";
                    break;
                case "1007":
                    result = "关键字过滤";
                    break;
                case "1008":
                    result = "接收号码数量过多";
                    break;
                case "1009":
                    result = "帐户过期";
                    break;
                case "1010":
                    result = "参数格式错误";
                    break;
                case "1011":
                    result = "其它错误";
                    break;
                case "1012":
                    result = "数据库繁忙";
                    break;
                case "1013":
                    result = "非法发送时间";
                    break;
                default:
                    result = "成功?";
                    break;
            }
            return result;
        }

        #region 每次动态生成一个代理调用webservice

        /// <summary>   
        /// 动态调用WebService   
        /// </summary>   
        /// <param name="url">WebService地址</param>   
        /// <param name="classname">类名</param>   
        /// <param name="methodname">方法名(模块名)</param>   
        /// <param name="args">参数列表</param>   
        /// <returns>object</returns>   
        public static object InvokeWebService(string url, string classname, string methodname, object[] args)
        {
            string @namespace = "ServiceBase.WebService.DynamicWebLoad";
            if (classname == null || classname == "")
            {
                classname = GetClassName(url);
            }
            url = url.Contains("?WSDL") ? url : (url.Contains("?wsdl") ? url : url + "?WSDL");
            //获取服务描述语言(WSDL)   
            WebClient wc = new WebClient();
            Stream stream = wc.OpenRead(url);
            ServiceDescription sd = ServiceDescription.Read(stream);
            ServiceDescriptionImporter sdi = new ServiceDescriptionImporter();
            sdi.AddServiceDescription(sd, "", "");
            CodeNamespace cn = new CodeNamespace(@namespace);
            //生成客户端代理类代码   
            CodeCompileUnit ccu = new CodeCompileUnit();
            ccu.Namespaces.Add(cn);
            sdi.Import(cn, ccu);
            CSharpCodeProvider csc = new CSharpCodeProvider();
            ICodeCompiler icc = csc.CreateCompiler();
            //设定编译器的参数   
            CompilerParameters cplist = new CompilerParameters();
            cplist.GenerateExecutable = false;
            cplist.GenerateInMemory = true;
            cplist.ReferencedAssemblies.Add("System.dll");
            cplist.ReferencedAssemblies.Add("System.XML.dll");
            cplist.ReferencedAssemblies.Add("System.Web.Services.dll");
            cplist.ReferencedAssemblies.Add("System.Data.dll");
            //编译代理类   
            CompilerResults cr = icc.CompileAssemblyFromDom(cplist, ccu);
            if (true == cr.Errors.HasErrors)
            {
                System.Text.StringBuilder sb = new StringBuilder();
                foreach (CompilerError ce in cr.Errors)
                {
                    sb.Append(ce.ToString());
                    sb.Append(System.Environment.NewLine);
                }
                throw new Exception(sb.ToString());
            }
            //生成代理实例,并调用方法   
            System.Reflection.Assembly assembly = cr.CompiledAssembly;
            Type t = assembly.GetType(@namespace + "." + classname, true, true);//未能从程序集“i0uox4fy, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null”中加载类型“ServiceBase.WebService.DynamicWebLoad.SmsInterfaceClient”。 
            object obj = Activator.CreateInstance(t);
            System.Reflection.MethodInfo mi = t.GetMethod(methodname);
            return mi.Invoke(obj, args);
        }

        private static string GetClassName(string url)
        {
            string[] parts = url.Split('/');
            string[] pps = parts[parts.Length - 1].Split('.');
            return pps[0];
        }

        #endregion

        #region 生成一个本地代理DLL，通过本地代理DLL调用webservice

        /// <summary>
        /// 创建一个调用webservice的本地代理DLL
        /// </summary>
        /// <param name="url"></param>
        public static void CreateWebServiceDLL()
        {
            string smsfile = HttpContext.Current.Server.MapPath("~") + "dll" + "\\SMSWebservice.dll";
            if (!File.Exists(smsfile))
            {
                return;
            }
            try
            {
                string url = smsWSDL;
                url = url.ToUpper().Contains("?WSDL") ? url : (url + "?WSDL");
                // 1. 使用 WebClient 下载 WSDL 信息。
                WebClient web = new WebClient();
                Stream stream = web.OpenRead(url);
                // 2. 创建和格式化 WSDL 文档。
                ServiceDescription description = ServiceDescription.Read(stream);
                // 3. 创建客户端代理代理类。
                ServiceDescriptionImporter importer = new ServiceDescriptionImporter();
                importer.ProtocolName = "Soap"; // 指定访问协议。
                importer.Style = ServiceDescriptionImportStyle.Client; // 生成客户端代理。
                importer.CodeGenerationOptions = CodeGenerationOptions.GenerateProperties | CodeGenerationOptions.GenerateNewAsync;
                importer.AddServiceDescription(description, null, null); // 添加 WSDL 文档。
                // 4. 使用 CodeDom 编译客户端代理类。
                CodeNamespace nmspace = new CodeNamespace();        // 为代理类添加命名空间，缺省为全局空间。
                CodeCompileUnit unit = new CodeCompileUnit();
                unit.Namespaces.Add(nmspace);
                ServiceDescriptionImportWarnings warning = importer.Import(nmspace, unit);
                CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
                CompilerParameters parameter = new CompilerParameters();
                parameter.GenerateExecutable = false;
                string file = HttpContext.Current.Server.MapPath("~") + "dll";
                if (!Directory.Exists(file))
                {
                    Directory.CreateDirectory(file);
                }
                parameter.OutputAssembly = file + "\\SMSWebservice.dll";  // 可以指定你所需的任何文件名。
                parameter.ReferencedAssemblies.Add("System.dll");
                parameter.ReferencedAssemblies.Add("System.XML.dll");
                parameter.ReferencedAssemblies.Add("System.Web.Services.dll");
                parameter.ReferencedAssemblies.Add("System.Data.dll");
                CompilerResults result = provider.CompileAssemblyFromDom(parameter, unit);
                if (result.Errors.HasErrors)
                {
                    // 显示编译错误信息
                    System.Text.StringBuilder sb = new StringBuilder();
                    foreach (CompilerError ce in result.Errors)
                    {
                        sb.Append(ce.ToString());
                        sb.Append(System.Environment.NewLine);
                    }
                    throw new Exception(sb.ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 调用webservice
        /// </summary>
        /// <param name="methodname"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static object InvokeWebServiceNew(string url, string className, string methodname, object[] args)
        {
            string classname = className;
            if (string.IsNullOrEmpty(classname)) classname = GetClassName(url);
            string file = HttpContext.Current.Server.MapPath("~") + "dll\\SMSWebservice.dll";
            Assembly asm = Assembly.LoadFrom(file);
            Type t = asm.GetType(classname);
            object o = Activator.CreateInstance(t);
            MethodInfo method = t.GetMethod(methodname);
            return method.Invoke(o, args);
        }

        #endregion
    }
}
