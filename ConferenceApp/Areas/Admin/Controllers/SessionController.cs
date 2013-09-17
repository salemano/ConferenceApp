using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ConferenceApp.Areas.Admin.Models;
using MvcContrib.UI.Grid;
using Model.Models;
using ConferenceApp.Infrastructure;
using Core.Services;
using MvcContrib.Pagination;
using MvcContrib.Sorting;
using ConferenceApp.Models;


namespace ConferenceApp.Areas.Admin.Controllers
{
    [Authorize]
    public class SessionController : Controller
    {
        private ISessionService _sessionService;
        private IUserService _userService;
        public const int ListPageSize = 10;

        public SessionController(ISessionService sessionService, IUserService userService)
        {
            _sessionService = sessionService;
            _userService = userService;
        }

        public ActionResult List(SessionFilterViewModel filter, int? page, GridSortOptions sortOptions)
        {
            var user = _userService.CurrentUser;
            if (filter == null)
                filter = new SessionFilterViewModel();

            if (sortOptions.Column == null)
                sortOptions = new GridSortOptions
                {
                    Column = "End",
                    Direction = SortDirection.Ascending
                };

            page = page ?? 1;

            var sessions = _sessionService.GetAll().ToList().Where(s => (int)s.Status != (int)SessionStatus.InProgress);

            // Filtering
            if (!string.IsNullOrWhiteSpace(filter.FilterText))
            {
                sessions = from c in sessions
                           where c.Title.Contains(filter.FilterText)
                           select c;
            }

            if (!filter.ShowClosed)
            {
                sessions = from c in sessions
                           where c.Start > DateTime.Now
                           select c;
            }

            if (filter.From != null)
            {
                sessions = from c in sessions
                           where c.Start >= filter.From
                           select c;
            }

            if (filter.To != null)
            {
                sessions = from c in sessions
                           where c.End <= filter.To
                           select c;
            }

            if (filter.SelectedTypeId != null)
            {
                sessions = from c in sessions
                           where (int)c.Type == filter.SelectedTypeId.Value
                           select c;
            }

            var list = from s in sessions.ToList()
                       select new SessionDescriptionModel
                       {
                           SessionId = s.Id,
                           Title = s.Title,
                           End = s.End,
                           Start = s.Start,
                           RegistrationCloseAt = s.RegistrationClosedAt,
                           NumberOfRegistrants = s.Users.Count(),
                           StatusId = (int)s.Status,
                           TypeId = (int)s.Type,
                           IsAccepted = s.IsAccepted,
                           Email = s.User.Email,
                           RejectionReason = s.RejectionReason
                       };

            list = list.OrderBy(sortOptions.Column, sortOptions.Direction);
            var types = (from SessionType s in SessionType.GetValues(typeof(SessionType))
                         select new SelectListItem { Text = s.ToString(), Value = ((int)s).ToString() }).ToList();
            types.Insert(0, new SelectListItem { Text = "All", Value = "" });
            filter.Types = types;

            var vm = new AdminSessionListViewModel
            {
                Sessions = list.AsPagination(page.Value, ListPageSize),
                SortOptions = sortOptions,
                Filter = filter,
                Page = page.Value
            };

            return View(vm);
        }   

        public ActionResult Reject(int id)
        {
            var session = _sessionService.GetById(id);
            var vm = new RejectSessionViewModel { SessionId = id, SessionTitle = session.Title };

            return PartialView("_RejectSession", vm);
        }

        [HttpPost]
        public ActionResult Reject(RejectSessionViewModel vm)
        {
            if (!ModelState.IsValid)
                return PartialView("_RejectSession", vm);

            _sessionService.Reject(vm.SessionId, vm.RejectionReason);

            return new EmptyResult()
                .WithSuccessMessage(string.Format("Session {0} has been successfully rejected", vm.SessionTitle));
        }

        public ActionResult Accept(int id)
        {
            var session = _sessionService.GetById(id);
            _sessionService.Accept(id);

            return new EmptyResult()
                .WithSuccessMessage(string.Format("Session {0} has been successfully accepted", session.Title));
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var session = _sessionService.GetById(id);

            var model = new EditSessionModel
            {
                Id = session.Id,
                City = session.City,
                CreateEnd = session.End,
                Overview = session.Overview,
                CreateRegistrationClosedAt = session.RegistrationClosedAt,
                SelectedTypeId = (int)session.Type,
                CreateStart = session.Start,
                Title = session.Title,
                Permissions = _sessionService.GetPermissionModel(session, _userService.CurrentUser),
                Users = session.Users.ToList()
            };

            PopulateEditSessionModel(model);

            if (!model.Permissions.CanView)
                return RedirectToAction("List", "Session").WithErrorMessage("You are not authorized to access this page.");

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit(EditSessionModel model, int id)
        {
            var session = _sessionService.GetById(id);

            PopulateEditSessionModel(model);
            model.Permissions = _sessionService.GetPermissionModel(session, _userService.CurrentUser);
            model.Users = session.Users.ToList();

            if (!model.Permissions.CanView)
                return RedirectToAction("List", "Session").WithErrorMessage("You are not authorized to access this page.");

            if (!ModelState.IsValid)
                return View(model);

            session.City = model.City;
            session.End = model.CreateEnd.Value;
            session.Overview = model.Overview;
            session.RegistrationClosedAt = model.CreateRegistrationClosedAt.Value;
            session.Start = model.CreateStart.Value;
            session.Title = model.Title;

            _sessionService.Update(session);

            return RedirectToAction("List", "Session").WithSuccessMessage(string.Format("Session '{0}' has been successfully saved.", session.Title));
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var session = _sessionService.GetById(id);
            // check permission
            if (session == null)
                return RedirectToAction("List", "Session").WithErrorMessage(string.Format("Could not find session by id {0}.", id));

            _sessionService.Delete(id);

            return RedirectToAction("List", "Session").WithSuccessMessage(string.Format("Session {0} has been successfully deleted.", session.Title));
        }

        void PopulateEditSessionModel(EditSessionModel model)
        {
            var statuses = from SessionType s in SessionType.GetValues(typeof(SessionType))
                           select new SelectListItem { Text = s.ToString(), Value = ((int)s).ToString() };

            model.Types = statuses;
        }

        public ActionResult Requests(int? page, GridSortOptions sortOptions)
        {
            var user = _userService.CurrentUser;

            if (sortOptions.Column == null)
                sortOptions = new GridSortOptions
                {
                    Column = "End",
                    Direction = SortDirection.Ascending
                };

            page = page ?? 1;

            var sessions = _sessionService.GetAll().ToList().Where(s => (int)s.Status == (int)SessionStatus.InProgress);

            var list = from s in sessions.ToList()
                       select new SessionDescriptionModel
                       {
                           SessionId = s.Id,
                           Title = s.Title,
                           End = s.End,
                           Start = s.Start,
                           RegistrationCloseAt = s.RegistrationClosedAt,
                           StatusId = (int)s.Status,
                           TypeId = (int)s.Type,
                           Email = s.User.Email,
                       };

            list = list.OrderBy(sortOptions.Column, sortOptions.Direction);

            var vm = new SessionRequestsViewModel
            {
                Sessions = list.AsPagination(page.Value, ListPageSize),
                SortOptions = sortOptions,
                Page = page.Value
            };

            return View(vm);
        }
    }
}
