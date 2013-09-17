using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using ConferenceApp.Filters;
using ConferenceApp.Infrastructure;

namespace ConferenceApp.Controllers
{
    [Localization]
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("List", "Session");
        }

        [AllowAnonymous]
        public ActionResult ChangeCulture()
        {
            if (Session["culture"] == null)
                Session["culture"] = "en";
            else
                Session["culture"] = (Session["culture"].ToString() == "ru")
                    ? "en-GB" : "ru";
            var p = Session["culture"].ToString();

            var url = Helper.GetCurrentUrl();

            return Redirect(url); 
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
