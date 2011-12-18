using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VoteSystem.Models;

namespace VoteSystem.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }

        public ActionResult About()
        {
            using (var db = new VoteSystemDataContext())
            {
                var topics = from t in db.Topic
                            select t;
   
                //foreach(var t in topics)
                //{
                //    var topic = t;
                //    var suggestions = from s in db.Suggestion
                //                     where s.TopicId == topic.TopicId
                //                     select s;
                //    t.Suggestions.AddRange(suggestions);

                //    foreach(var s in t.Suggestions)
                //    {
                //        var suggestion = s;
                //        var votes = from v in db.Vote
                //                   where v.SuggestionId == suggestion.SuggestionId
                //                   select v;
                //        s.Votes.AddRange(votes);
                //    }
                //}

                return View(topics.ToList());
            }
        }
    }
}
