using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JaxDug.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to Jacksonville's .NET Developer User Group";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
