using Joney.UtilityClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
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
                        HttpContext.Current.Session.Abandon();
                        HttpContext.Current.Session.Clear();//清空Session
                        HttpContext.Current.Request.Cookies.Clear();
                        FormsAuthentication.SignOut();
                        ContentResult contentRes = new ContentResult();
                        contentRes.Content = string.Format("<script type='text/javascript'>alert('用户凭证失效,请重新登录!');window.parent.location.href='{0}';</script>", loginUrl);
                        filterContext.Result = contentRes;
                        //filterContext.HttpContext.Response.Redirect(loginUrl, true);
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
            }

            public override void OnActionExecuted(ActionExecutedContext filterContext)
            {
                if (filterContext.Exception != null)
                {
                    //var mc = MyCommon.GetCookie();
                    var acName = filterContext.ActionDescriptor.ActionName;
                    var exMsg = filterContext.Exception == null ? "" : filterContext.Exception.Message.ToString();
                    var rqUrl = filterContext.HttpContext.Request.RawUrl;
                    var acUrl = filterContext.HttpContext.Request.UrlReferrer;
                    //LogHelper.WriteLogs("用户ID:" + mc.UserSign + " 用户名:" + mc.UserName + " 操作请求:" + rqUrl + " 异常信息：" + exMsg, "OnActionExecuted");
                    ContentResult ctRes = new ContentResult();
                    ctRes.Content = string.Format("<script type='text/javascript'>alert('提示:{0}');window.parent.location.href='{1}';</script>", exMsg, FormsAuthentication.LoginUrl);//返回登录页
                    filterContext.Result = ctRes;
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
        }
        
        /// <summary>
        /// 异常处理
        /// </summary>
        public class ExceptionAttribut : ActionFilterAttribute, IExceptionFilter
        {
            public void OnException(ExceptionContext filterContext)
            {
                #region Ather Exception.

                //if (filterContext.ExceptionHandled==true)
                //{
                //    HttpException httpExce = filterContext.Exception as HttpException;
                //    if (httpExce.GetHttpCode()!=500)
                //    {
                //        return;
                //    }
                //}
                //HttpException httpException = filterContext.Exception as HttpException;
                //if (httpException!=null)
                //{
                //    filterContext.Controller.ViewBag.UrlRefer = filterContext.HttpContext.Request.UrlReferrer;
                //    Exception ex = filterContext.Exception;
                //    string msg = ex.Message; //错误信息
                //    string url = HttpContext.Current.Request.RawUrl;//错误发生地址
                //    filterContext.ExceptionHandled = true;
                //    if (httpException.GetHttpCode()==404)
                //    {
                //        //filterContext.HttpContext.Response.Redirect("~/Home/Error");
                //        filterContext.Result = new RedirectResult("~/Home/Error");//跳转页面
                //    }
                //    else if (httpException.GetHttpCode()==500)
                //    {
                //        //filterContext.HttpContext.Response.Redirect("~/Account/LogOff");
                //        filterContext.Result = new RedirectResult("~/Account/LogOff");//跳转页面
                //    }
                //}

                #endregion

                Exception ex = filterContext.Exception;
                ContentResult ctRes = new ContentResult();
                string msg = ex.Message; //错误信息
                string url = HttpContext.Current.Request.Url.ToString();//错误发生地址
                LogHelper.Jlogger(MethodBase.GetCurrentMethod().DeclaringType, "error",url+" —— "+msg);//log4net日志。
                var errorUrl = string.Format("../Home/Error");
                var msgHtml = @"<div style='width:200px;height:100px;text-align:center;z-index:999999;position:fixed;top:10%;left:45%;border:5px solid #000;background:#000;border-radius:6px;opacity:0.3;filter: alpha(opacity=30);
'><div style='text-align:center;'>异常错误：" + msg + "</div></div>";//window.document.body.appendChild('{0}');//document.createElement('{0}');
                ctRes.Content = string.Format("<script type='text/javascript'>alert('{0}');window.location.href='{1}';</script>", msg, errorUrl);

                filterContext.ExceptionHandled = true;
                filterContext.Result = ctRes;// new RedirectResult("Home/Error");//跳转到错误提示页面
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