using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConferenceApp.Controllers
{
    public class ErrorController : Controller
    {
        // GET: /Error/General
        public ActionResult General()
        {
            return View("Index");
        }

        // GET: /Error/HttpError500
        public ActionResult HttpError500()
        {
            return View();
        }

        // GET: /Error/HttpError404
        public ActionResult HttpError404()
        {
            return View();
        }
    }
}
