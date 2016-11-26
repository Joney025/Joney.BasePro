using Joney.BasePro.Models;
using System.Web.Mvc;

namespace Joney.BasePro.Controllers
{
    //[MyFilter.ActionAttribut(Role = "admin", Code = "1")]//过滤器
    public class AdminController : BaseController
    {
        public ActionResult Index()
        {
            return View("AdminIndex");
        }
        

        public ActionResult UserManage()
        {
            return View();
        }

        public ActionResult UserRegist()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        public ActionResult SaveUserInfo(FormCollection form)
        {
            var html = Server.HtmlEncode(form["htm"]);
            var htm = Server.HtmlDecode(form["htm"]);
            var uri = Server.UrlEncode(form["htm"]);
            var url = Server.UrlDecode(form["htm"]);
            
            return View();
        }
    }
}