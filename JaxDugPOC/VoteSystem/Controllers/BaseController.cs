using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VoteSystem.Models;

namespace VoteSystem.Controllers
{
    public class BaseController : Controller
    {
        public UserInfo UserInfo { get; set; }
    }
}
