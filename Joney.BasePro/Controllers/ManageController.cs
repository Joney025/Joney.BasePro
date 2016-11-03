using Joney.BasePro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Joney.BasePro.Controllers
{
    //[MyFilter.ActionAttribut(Role = "admin", Code = "1")]//过滤器
    public class ManageController : BaseController
    {
        // GET: Manage
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FlowWork()
        {
            dynamic my = Request.QueryString["xx"];
            return View();
        }

        public ActionResult FlowDesign()
        {
            return View();
        }
    }
}