using System.Web.Mvc;
using DotNetOpenAuth.OAuth2;
using Google.Apis.Authentication.OAuth2;
using Google.Apis.Moderator.v1;
using JaxDug.Models;


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
