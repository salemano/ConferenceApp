using ConferenceApp.Models;
using Core.Services.Sessions;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConferenceApp.Controllers
{
    public class SessionController : Controller
    {
        //
        // GET: /Session/
        private ISessionService _sessionService;

        public SessionController(ISessionService service)
        {
            _sessionService = service;
        }

        [AllowAnonymous]
        public ActionResult CreateSession()
        {
            var model = new CreateNewSession();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSession(CreateNewSession model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var session = new Session
            {
                Title = model.Title,
                Overview=model.Overview,
                CreatedAt=DateTime.Now,
                IsAccepted=false,
                City=model.City,
                Start=model.Start,
                End=model.End,
                RegistrationClosedAt=model.RegistrationClosedAt
            };
            _sessionService.Create(session);
            return RedirectToAction("Index","Home");
        }

    }
}
