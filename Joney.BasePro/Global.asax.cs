using Joney.ModelsFactory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace Joney.BasePro
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public MvcApplication()
        {
            AuthorizeRequest += new EventHandler(MvcApplication_AuthorizeRequest);
        }

        void MvcApplication_AuthorizeRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.User!=null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    //方式A:
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        //get current user identitied by forms
                        FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;
                        FormsAuthenticationTicket ticket = id.Ticket;
                        var userData =JsonConvert.DeserializeObject<UserInfo>(ticket.UserData);
                        var roles = userData.Roles;//.Split(',');//在票据里获取当前用户所拥有的操作权限:
                        HttpContext.Current.User = new GenericPrincipal(id,roles);
                    }
                    //方式B：
                    //{
                    //    IIdentity id = Context.User.Identity;
                    //    if (id.IsAuthenticated)
                    //    {
                    //        var roles = new UserRepository().GetRoles(id.Name);//读取数据源获取当前用户所拥有的操作权限:
                    //        Context.User = new GenericPrincipal(id, roles);
                    //    }
                    //}
                }
            }
            
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
            ViewEngines.Engines.Add(new WebFormViewEngine());

        }
    }
}
