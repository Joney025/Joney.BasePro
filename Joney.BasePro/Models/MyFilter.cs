using Joney.UtilityClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace Joney.BasePro.Models
{
    /// <summary>
    /// 自定义过滤器
    /// </summary>
    public class MyFilter
    {
        public class ActionAttribut : ActionFilterAttribute
        {
            //自定义条件？

            /// <summary>
            /// 角色权限编号？
            /// </summary>
            public int Code { get; set; }
            /// <summary>
            /// 角色权限名称？
            /// </summary>
            public string Role { get; set; }
            
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                var cName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                var action = filterContext.ActionDescriptor.ActionName;
                
                if (cName != "Account" && action != "Index")//cName != "Account" && action != "Login"
                {
                    var returnUlr = filterContext.HttpContext.Request.Url.AbsolutePath;
                    var redirectUrl = string.Format("?ReturnUrl={0}",returnUlr);
                    var loginUrl = FormsAuthentication.LoginUrl + redirectUrl;
                    
                    if (!HttpContext.Current.User.Identity.IsAuthenticated)
                    {
                        Logon(filterContext);
                    }
                    else
                    {
                        //角色校验：
                        if (Code!=0)
                        {
                            var emType =Enum.Parse(typeof(SysRoleEnum.RoleEnum),Code.ToString());//通关角色编号获取角色名称
                            bool isAuthenticated = filterContext.HttpContext.User.IsInRole(emType.ToString());
                            
                            if (!isAuthenticated)
                            {
                                ContentResult ctRes = new ContentResult();
                                var errorUrl = "~/Shared/Error";
                                ctRes.Content = string.Format("<script type='text/javascript'>alert('你无权限执行该操作!');window.location.href='{0}';</script>", errorUrl);//返回错误页
                                filterContext.Result = ctRes;
                                //throw new UnauthorizedAccessException("You have no right to view this page!");
                            }
                        }
                        else
                        {
                            //没配置权限？按需进行逻辑操作：
                            //throw new InvalidOperationException("No Role Specified!");
                        }
                    }
                }
                base.OnActionExecuting(filterContext);
            }

            public override void OnActionExecuted(ActionExecutedContext filterContext)
            {
                if (filterContext.Exception != null)
                {
                    LogHelper.WriteLogs(" 异常信息：" + filterContext.Exception.Message + "\r\n 堆栈信息：" + filterContext.Exception.StackTrace);
                    JavaScriptResult _js = new JavaScriptResult();
                    _js.Script = string.Format("<script type='text/javascript'>alert('提示:{0}');window.parent.location.href='{1}';</script>", filterContext.Exception.Message, FormsAuthentication.LoginUrl);//返回登录页
                    filterContext.Result = _js;
                }

                base.OnActionExecuted(filterContext);
            }

            public override void OnResultExecuted(ResultExecutedContext filterContext)
            {
                base.OnResultExecuted(filterContext);
            }

            public override void OnResultExecuting(ResultExecutingContext filterContext)
            {
                base.OnResultExecuting(filterContext);
            }

            private void Logon(ActionExecutingContext filterContext)
            {
                RouteValueDictionary dictionary = new RouteValueDictionary
                (new
                {
                    controller = "Account",
                    action = "Login",
                    returnUrl = filterContext.HttpContext.Request.RawUrl
                });
                filterContext.Result = new RedirectToRouteResult(dictionary);
            }
        }
        
        /// <summary>
        /// 异常处理
        /// </summary>
        public class ExceptionAttribut : ActionFilterAttribute, IExceptionFilter
        {
            public void OnException(ExceptionContext filterContext)
            {
                Exception ex = filterContext.Exception;
                ContentResult conRes = new ContentResult();
                UrlHelper _Url = new UrlHelper(filterContext.RequestContext);

                string errorUrl = HttpContext.Current.Request.RawUrl;//错误发生地址
                string msg = ex.Message; //错误信息
                LogHelper.WriteLogs(" 异常信息：" + msg + "\r\n 堆栈信息：" + ex.StackTrace);
                conRes.Content = HttpContext.Current.Server.UrlEncode(string.Format("错误请求：{0},错误信息：{1}", errorUrl, msg));
                filterContext.ExceptionHandled = true;
                filterContext.Result = new RedirectResult(_Url.Action("Error", "Account", conRes));//跳转到错误提示页面
            }
        }

        public class MyAuthorizeAttribute : AuthorizeAttribute
        {
            protected override bool AuthorizeCore(HttpContextBase httpContext)
            {
                //bool state = false;
                //if (!false)//checklogin
                //{
                //    httpContext.Response.StatusCode = 401;
                //    state = false;
                //}else
                //{
                //    state = true;
                //}
                //return state;
                return base.AuthorizeCore(httpContext);
            }

            protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
            {
                base.HandleUnauthorizedRequest(filterContext);
                if (filterContext.HttpContext.Response.StatusCode == 401)
                {
                    filterContext.Result = new RedirectResult("/");
                }
                if (filterContext == null)
                {
                    throw new ArgumentNullException("filterContext");
                }
                else
                {
                    string path = filterContext.HttpContext.Request.Url.AbsolutePath;
                    var loginUrl = FormsAuthentication.LoginUrl+"?ReturnUrl={0}";
                    filterContext.HttpContext.Response.Redirect(string.Format(loginUrl,HttpUtility.UrlEncode(path)),true);
                }
            }
        }
    }
    
}