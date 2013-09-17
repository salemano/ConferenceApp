using ConferenceApp.Models;
using Core.Services;
using Model;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ConferenceApp.Infrastructure;
using MvcContrib.Pagination;
using MvcContrib.Sorting;
using MvcContrib.UI.Grid;
using System.Web.Security;

namespace ConferenceApp.Controllers
{
    [Authorize]
    public class SessionController : Controller
    {
        //
        // GET: /Session/
        private ISessionService _sessionService;
        private IUserService _userService;
        public const int ListPageSize = 10;

        public SessionController(ISessionService sessionService, IUserService userService)
        {
            _sessionService = sessionService;
            _userService = userService;
        }

        public ActionResult Index()
        {
            return RedirectToAction("List", "Session");
        }

        [HttpGet]
        public ActionResult Create()
        {
            var model = new EditSessionModel { Permissions = new SessionPermissionModel { CanEdit = true, CanView = true } };

            PopulateEditSessionModel(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EditSessionModel model, string btnSubmit)
        {
            PopulateEditSessionModel(model);
            model.Permissions = new SessionPermissionModel { CanEdit = true, CanView = true };

            if (!ModelState.IsValid)
                return View(model);

            var user = _userService.GetByUsername(User.Identity.Name);

            var session = new Session
            {
                UserId = user.Id,
                Title = model.Title,
                Overview = model.Overview,
                CreatedAt = DateTime.Now,
                City = model.City,
                Start = model.CreateStart.Value,
                End = model.CreateEnd.Value,
                RegistrationClosedAt = model.CreateRegistrationClosedAt.Value
            };

            _sessionService.Create(session);

            if(btnSubmit == "Submit")
            {
                _sessionService.UserSubmit(session.Id);
                return RedirectToAction("List", "Session").WithSuccessMessage(string.Format("Session '{0}' has been successfully submitted for Admin approval.", session.Title));
            }

            return RedirectToAction("List", "Session").WithSuccessMessage(string.Format("Session '{0}' has been successfully created.", session.Title));
        }

        [Authorize]
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

            var sessions = _sessionService.GetAll().Where(s => s.IsAccepted == true && s.UserId != user.Id);

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
                           where c.RegistrationClosedAt > DateTime.Now
                           select c;
            }

            if (filter.From != null)
            {
                sessions = from c in sessions
                           where c.Start >= filter.From
                           select c;
            }

            if (filter.To != null) {
                sessions = from c in sessions
                           where c.End <= filter.To
                           select c;
            }

            if (filter.SelectedTypeId != null) {
                sessions = from c in sessions
                           where (int)c.Type == filter.SelectedTypeId.Value
                           select c;         
            }

            var list = from s in sessions.ToList()
                       select new SessionRowItem
                          {
                              Id = s.Id,
                              Title = s.Title,
                              End = s.End,
                              Start = s.Start,
                              RegistrationCloseAt = s.RegistrationClosedAt,
                              NumberOfRegistrants = s.Users.Count(),
                              IsAuthor = s.UserId == user.Id,
                              IsRegistered = s.Users.Any(u => u.UserId == user.Id) ,
                              TypeId = (int)s.Type
                          };

            list = list.OrderBy(sortOptions.Column, sortOptions.Direction);
            var types = (from SessionType s in SessionType.GetValues(typeof(SessionType))
                           select new SelectListItem { Text = s.ToString(), Value = ((int)s).ToString() }).ToList();
            types.Insert(0, new SelectListItem { Text = "", Value = "" });
            filter.Types = types;
            var goups = list.ToList().GroupBy(s => s.Start.Date).Select(p =>
            {
                return new SesionListItemViewModel
                {
                    Start = p.Key,
                    Sessions = p
                };
            });

            var vm = new SessionListViewModel
            {
                List = goups.ToList().AsPagination(page.Value, ListPageSize),
                SortOptions = sortOptions,
                Filter = filter,
                Page = page.Value
            };

            return View(vm);
        }

        [Authorize]
        public ActionResult Register(int id)
        {
            // check if user already registered
            var session = _sessionService.GetById(id);
            var user = _userService.CurrentUser;

            if (session == null)
                return new EmptyResult().WithErrorMessage(string.Format("Could not find session by Id: {0}", id));

            var vm = new RegisterInSessionViewModel
            {
                // check permissions
                SessionId = session.Id,
                IsRegistered = session.Users.Any(u=>u.UserId == user.Id),
                SessionDescription = new SessionViewModel
                {
                    Id = session.Id,
                    Overview = session.Overview,
                    End = session.End,
                    RegistrationCloseAt = session.RegistrationClosedAt,
                    Start = session.Start,
                    Title = session.Title,
                    TypeId = (int)session.Type,
                    Address = session.City
                }

            };

            return PartialView("_Register", vm);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Register(RegisterInSessionViewModel vm)
        {
            var user = _userService.CurrentUser;
            var session = _sessionService.GetById(vm.SessionId);

            _sessionService.Register(user.Id, vm.SessionId);

            return new EmptyResult().WithSuccessMessage(string.Format("You have been successfully registered in session: {0}", session.Title));
        }

        [HttpGet]
        [Authorize]
        public ActionResult Manage(int? page, GridSortOptions sortOptions)
        {
            var username = User.Identity.Name;
            var id = _userService.GetByUsername(username).Id;
            var sessions = _sessionService.GetAllByUserId(id).ToList();

            if (sortOptions.Column == null)
                sortOptions = new GridSortOptions
                {
                    Column = "End",
                    Direction = SortDirection.Ascending
                };
            page = page ?? 1;
            var vm = new MySessionListViewModel { Page = page.Value, SortOptions = sortOptions };
            var list = from s in sessions
                       select new SessionRowItem
                       {
                           Id = s.Id,
                           Title = s.Title,
                           End = s.End,
                           Start = s.Start,
                           RegistrationCloseAt = s.RegistrationClosedAt,
                           NumberOfRegistrants = s.Users.Count(),
                           TypeId = (int)s.Type,
                           Status = s.Status,
                           IsAccepted = s.IsAccepted,
                           RejectionReason = s.RejectionReason,
                           AdminSubmittedAt = s.AdminSubmittedAt
                       };

            list = list.OrderBy(sortOptions.Column, sortOptions.Direction);
            vm.Sessions = list.AsPagination(page.Value, ListPageSize);

            return View(vm);
        }

        [HttpGet]
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

            if (session.Status == SessionStatus.InProgress)
                model.Permissions.CanEdit = true;

            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, EditSessionModel model, string btnSubmit)
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

            if (btnSubmit == "Submit")
            {
                _sessionService.UserSubmit(session.Id);
                return RedirectToAction("Manage", "Session").WithSuccessMessage(string.Format("Session '{0}' has been successfully submitted for Admin approval.", session.Title));
            }

            return RedirectToAction("Manage", "Session").WithSuccessMessage(string.Format("Session '{0}' has been successfully saved.", session.Title));
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUserFromSession(int userId, int sessionId)
        {
            var session = _sessionService.GetById(sessionId);
            var user = _userService.GetById(userId);

            if (session != null && user != null)
                _sessionService.RemoveUser(userId, sessionId);

            else
                return RedirectToAction("Edit", "Session", new { id = sessionId }).WithErrorMessage("Could not delete user from session.");

            return RedirectToAction("Edit", "Session", new { id = sessionId }).WithSuccessMessage(string.Format("User '{0}' has been successfully removed from session '{1}'.", user.FullName, session.Title));
        }

        void PopulateEditSessionModel(EditSessionModel model)
        {
            var statuses = from SessionType s in SessionType.GetValues(typeof(SessionType))
                           select new SelectListItem { Text = s.ToString(), Value = ((int)s).ToString() };

            model.Types = statuses;
        }

        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            // check permission
            _sessionService.Delete(id);

            return RedirectToAction("Manage", "Session");
        }
    }
}
