using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
using DotNetOpenAuth.OpenId.RelyingParty;
using JaxDug.Models;

namespace JaxDug.Controllers
{
    public class AccountController : BaseController
    {
        AccountViewModel ViewModel = new AccountViewModel();

        #region Generated
        //
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            return View();
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, true, null, out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }
        #endregion

        [AcceptVerbs(HttpVerbs.Post | HttpVerbs.Get), ValidateInput(false)]
        public ActionResult OpenIdLogOn(string returnUrl)
        {
            var openid = new OpenIdRelyingParty();
            var response = openid.GetResponse();
            if (response == null)  // Initial operation
            {
                // Step 1 - Send the request to the OpenId provider server
                Identifier id;
                if (Identifier.TryParse(Request.Form["openid_identifier"], out id))
                {
                    if (!Identifier.IsValid(id))
                    {
                        ModelState.AddModelError("loginIdentifier", "The specific login identifier is invalid.");
                        return View("OpenLogin");
                    }

                    try
                    {
                        var request = openid.CreateRequest(id);
                        var fetch = new FetchRequest();
                        fetch.Attributes.AddRequired(WellKnownAttributes.Name.First);
                        fetch.Attributes.AddRequired(WellKnownAttributes.Name.Last);
                        fetch.Attributes.AddRequired(WellKnownAttributes.Contact.Email);
                        request.AddExtension(fetch);

                        return request.RedirectingResponse.AsActionResult();
                    }
                    catch (ProtocolException ex)
                    {
                        // display error by showing original LogOn view
                        return View("OpenLogin");
                    }
                }
            }
            else  // OpenId redirection callback
            {
                // Step 2: OpenID Provider sending assertion response
                switch (response.Status)
                {
                    case AuthenticationStatus.Authenticated:
                        string identifier = response.ClaimedIdentifier;
                        //FormsAuthentication.RedirectFromLoginPage(identifier, false);
                        // OpenId lookup fails - Id doesn't exist for login - login first
                        // Capture user information for AuthTicket
                        // and issue Forms Auth token
                        var fetch = response.GetExtension<FetchResponse>();
                        var openId = "unknown";
                        var firstName = "unknown";
                        var lastName = "unknown";
                        var email = "unknown";

                        if (fetch != null)
                        {
                            openId = identifier;
                            firstName = fetch.GetAttributeValue(WellKnownAttributes.Name.First);
                            lastName = fetch.GetAttributeValue(WellKnownAttributes.Name.Last);
                            email = fetch.GetAttributeValue(WellKnownAttributes.Contact.Email);

                            UserState userState = new UserState()
                                                      {
                                                          OpenId = openId,
                                                          FirstName = firstName,
                                                          LastName = lastName,
                                                          Email = email
                                                      };
                            IssueAuthTicket(userState, true);
                        }

                        if (!string.IsNullOrEmpty(returnUrl))
                            return Redirect(returnUrl);

                        return RedirectToAction("Index", "Moderator");

                    case AuthenticationStatus.Canceled:
                        //Show error cancelled at provider
                        ModelState.AddModelError("loginIdentifier", "Login was cancelled at the provider.");
                        break;
                    case AuthenticationStatus.Failed:
                        ModelState.AddModelError("loginIdentifier", "Login failed using the provided OpenId identifier.");
                        break;
                }
            }
            return View("OpenLogin");
        }

        /// <summary>
        /// Issues an authentication issue from a userState instance
        /// </summary>
        /// <param name="userState"></param>
        /// <param name="rememberMe"></param>
        private void IssueAuthTicket(UserState userState, bool rememberMe)
        {
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, userState.FirstName + " " + userState.LastName,
                                                                 DateTime.Now, DateTime.Now.AddDays(10),
                                                                 rememberMe, userState.ToString());

            string ticketString = FormsAuthentication.Encrypt(ticket);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, ticketString);
            if (rememberMe)
                cookie.Expires = DateTime.Now.AddDays(10);

            HttpContext.Response.Cookies.Add(cookie);
        }

        [AcceptVerbs(HttpVerbs.Post | HttpVerbs.Get), ValidateInput(false)]
        public ActionResult OpenIdRegistrationLogOn(FormCollection formVars)
        {
            return OpenIdRegistrationLogOn(null, true);
        }

        private ActionResult OpenIdRegistrationLogOn(IAuthenticationResponse response, bool reserved)
        {
            ViewData["IsNew"] = true;

            var openid = new OpenIdRelyingParty();

            if (response == null)
                response = openid.GetResponse();

            if (response == null)
            {
                string userId = Request.Form["Id2"];  // have to track the user’s id

                // Check for unlink operation
                if (!string.IsNullOrEmpty(Request.Form["btnOpenIdUnlink"]))
                {
                    //TODO: Check if User Exits
                    //TODO: Display Error: "Couldn't find associated User: " + ErrorMessage
                    //TODO: Save User (busUser.Save();)
                    return RedirectToAction("Register", new { id = userId });
                }

                Identifier id;
                string openIdIdentifier = Request.Form["openid_identifier"];
                if (Identifier.TryParse(openIdIdentifier, out id))
                {
                    try
                    {
                        // We need to know which user we are working with 
                        // and so we pass the id thru session – (is there a better way?) 
                        Session["userId"] = userId;

                        var req = openid.CreateRequest(id);

                        var fields = new ClaimsRequest();
                        fields.Email = DemandLevel.Request;
                        fields.FullName = DemandLevel.Request;
                        req.AddExtension(fields);

                        return req.RedirectingResponse.AsActionResult();
                    }
                    catch (ProtocolException ex)
                    {
                        return View("Register", this.ViewModel);
                    }
                }
            }
            else
            {
                // Reestablish the user we’re dealing with
                string userId = Session["userId"] as string;
                if (string.IsNullOrEmpty(userId))
                    ViewData["IsNew"] = true;
                else
                    ViewData["IsNew"] = false;

                // Stage 3: OpenID Provider sending assertion response
                switch (response.Status)
                {
                    case AuthenticationStatus.Authenticated:
                        var claim = response.GetExtension<ClaimsResponse>();
                        string email = null, fullname = null;
                        if (claim != null)
                        {
                            email = claim.Email;
                            fullname = claim.FullName;
                        }
                        string identifier = response.ClaimedIdentifier;

                        //TODO: associate openid with the user account

                        UserState userState = new UserState()
                        {
                            //    Name = busUser.Entity.Name,
                            //    Email = busUser.Entity.Name,
                            //    UserId = busUser.Entity.Id,
                            //    IsAdmin = busUser.Entity.IsAdmin
                        };
                        this.IssueAuthTicket(userState, true);

                        // and reload the page with the saved data
                        return this.RedirectToAction("Register", new { id = userId });
                    case AuthenticationStatus.Canceled:
                        //Show Error cancelled at provider
                        return View("Register", this.ViewModel);
                    case AuthenticationStatus.Failed:
                        //Show exemption
                        return View("Register", this.ViewModel);
                }
            }
            return new EmptyResult();
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
