using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using ConferenceApp.Filters;

namespace ConferenceApp.Controllers
{
    [Localization]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //using (var db = new ConferenceContext())
            //{
            //    var users = db.Users;
            //    db.SaveChanges();
            //}
 

            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult ChangeCulture()
        {
            if (Session["culture"] == null)
                Session["culture"] = "en";
            else
                Session["culture"] = (Session["culture"].ToString() == "ru")
                    ? "en" : "ru";
            var p = Session["culture"].ToString();
            return RedirectToAction("Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
