using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Joney.BasePro.Controllers
{
    public class ClubIMController : Controller
    {
        // GET: ClubIM
        public ActionResult Index()
        {
            return View("ClubIM");
        }

        public ActionResult SignalRIM()
        {
            return View();
        }
    }
}