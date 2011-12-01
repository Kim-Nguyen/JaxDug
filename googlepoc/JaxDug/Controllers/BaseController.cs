using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using JaxDug.Models;

namespace JaxDug.Controllers
{
    public class BaseController : Controller
    {
        private UserState UserState { get; set; }
        protected ErrorDisplay ErrorDisplay { get; set; }


        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            // Grab the user's login information from FormsAuth
            //var userState = new UserState();
            //if (User.Identity is FormsIdentity)
            //    UserState.FromString(((FormsIdentity)User.Identity).Ticket.UserData);

            //// have to explicitly add this so Master can see untyped value
            //ViewData["UserState"] = UserState;
            //ViewData["ErrorDisplay"] = ErrorDisplay;
        }

        /// <summary>
        /// Allow external initialization of this controller by explicitly
        /// passing in a request context
        /// </summary>
        /// <param name="requestContext"></param>
        public void InitializeForced(RequestContext requestContext)
        {
            Initialize(requestContext);
        }


        /// <summary>
        /// Displays a self contained error page without redirecting.
        /// Depends on ErrorController.ShowError() to exist
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="redirectTo"></param>
        /// <returns></returns>
        protected internal ActionResult DisplayErrorPage(string title, string message, string redirectTo)
        {
            return null;
        }
        

    }
}
