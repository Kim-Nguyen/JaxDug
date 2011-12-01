using System.Web.Mvc;
using JaxDug.Models;

namespace JaxDug.Controllers
{
    public class ModeratorController : Controller
    {
        private GoogleServiceModel _google = new GoogleServiceModel("1544f6");
 
        //
        // GET: /Moderator/
        [Authorize]
        public ActionResult Index()
        {
            return View(_google.FetchTopiclists());
        }

        public ActionResult Submissions(string topic)
        {
            return View(_google.FetchSubmissionList(topic));
        }

    }
}
