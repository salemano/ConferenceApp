using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ConferenceApp.Models;
using System.Web.UI.WebControls;
using Core.Services;
using ConferenceApp.Resourses.Shared;

namespace ConferenceApp.Controllers
{
	public class MenuController : Controller
    {
        private IUserService _userService;

        public MenuController(IUserService userService)
        {
            _userService = userService;
        }

        public MenuItemViewModel Menu
        {
            get
            {
                var menu = new MenuItemViewModel()
                 {
                     Nodes = new List<MenuItemViewModel>(){
                        new MenuItemViewModel {Id = "nav-sess", Action="List", Controller = "Session", IsAuthenticated = true, Text = Localization.Sessions,  Nodes = new List<MenuItemViewModel>()},   
                        new MenuItemViewModel {Id = "nav-my-sess", Action="Manage", Controller = "Session", IsAuthenticated = true, Text = Localization.MySessions,  Nodes = new List<MenuItemViewModel>()},                                                     
                        new MenuItemViewModel {Id = "nav-about", Action="About", Controller = "Home", Text = Localization.About,  Nodes = new List<MenuItemViewModel>()},
                        new MenuItemViewModel {Id = "nav-contact", Action="Contact", Controller = "Home", Text = Localization.Contact,  Nodes = new List<MenuItemViewModel>()},
                        new MenuItemViewModel {Id = "nav-admin", Area="Admin", IsAuthenticated = true, Text = Localization.Admin, IsAdmin = true,  Nodes = new List<MenuItemViewModel>
                            {
                                new MenuItemViewModel {Id = "nav-admin-user", Action="List", Controller = "User", Area="Admin",  IsAuthenticated = true,  IsAdmin = true, Text = Localization.ManageUsers,  Nodes = new List<MenuItemViewModel>()},
                                new MenuItemViewModel {Id = "nav-admin-sess", Action="List", Controller = "Session", Area="Admin",  IsAuthenticated = true, IsAdmin = true,  Text = Localization.ManageSessions,  Nodes = new List<MenuItemViewModel>()},
                                new MenuItemViewModel {Id = "nav-admin-sess-req", Action="Requests", Controller = "Session", Area="Admin",  IsAuthenticated = true,  IsAdmin = true, Text = Localization.SessionRequests,  Nodes = new List<MenuItemViewModel>()},
                                new MenuItemViewModel {Id = "nav-admin-event-log", Action="Index", Controller = "EventLog", Area="Admin",  IsAuthenticated = true,  IsAdmin = true, Text = Localization.EventLog,  Nodes = new List<MenuItemViewModel>()}
                            }}
                     }
                 };

                return menu;
            }
        }

        public PartialViewResult MainMenu(string requestArea, string requestController, string requestAction, string requestUrl)
        {
            var siteMap = Menu;

            var viewModel = CreateMenuItemViewModels(siteMap.Nodes, requestArea, requestController,requestAction, requestUrl, true);

            return PartialView(viewModel);
        }

        public PartialViewResult SubMenu(string requestArea, string requestController, string requestAction, string requestUrl)
        {
            var siteMap = Menu;

            var areaNode = siteMap.Nodes.FirstOrDefault(n => n.Area == requestArea);

            if (areaNode == null)
                return null;

            var viewModel = CreateMenuItemViewModels(areaNode.Nodes, requestArea, requestController, requestAction, requestUrl, false);

            return PartialView(viewModel);
        }

        public IEnumerable<MenuItemViewModel> CreateMenuItemViewModels(IEnumerable<MenuItemViewModel> siteMapNodes, string requestArea, string requestController, string requestAction, string requestUrl, bool isMainMenu)
        {
            var currentUser = _userService.CurrentUser;
            var isAdmin = currentUser != null && currentUser.IsAdministrator;
            var isAuthenticated = currentUser != null;

            return from w in siteMapNodes
                   let hasAccess = (w.IsAdmin ? isAdmin : true) && (w.IsAuthenticated ? isAuthenticated : true)
                   select new MenuItemViewModel
                   {
                       Id = w.Id,
                       Text = w.Text,
                       Controller = w.Controller,
                       Action = w.Action,
                       Area = w.Area,
                       NavigateUrl = GetMenuItemUrl(w),
                       Role = w.Role,
                       IsSelected = IsSelected(requestArea, requestController, requestAction, requestUrl, w, isMainMenu),
                       Visible = hasAccess
                   };
        }

        private string GetMenuItemUrl(MenuItemViewModel node)
        {
            string area = (node.Area == null ? String.Empty : ("/" + node.Area));

            return String.Format("~{0}/{1}/{2}", area, node.Controller, node.Action);
        }

        private bool IsSelected(string area, string controller, string action, string requestUrl, MenuItemViewModel node, bool isMainMenu)
        {
            if (node == null)
                return false;

            area = area ?? String.Empty;
            action = action ?? "Index";

            string nodeArea = node.Area ?? String.Empty;
            string nodeController = node.Controller ?? String.Empty;
            string nodeAction = node.Action ?? String.Empty;

            if (!String.IsNullOrWhiteSpace(node.Url) && node.Url != requestUrl)
                return false;

            if (isMainMenu && !string.IsNullOrEmpty(node.Area)) {
                return nodeArea == area;
            }

            bool selected =
                nodeArea == area &&
                (nodeController == controller || String.IsNullOrWhiteSpace(nodeController)) &&
                (nodeAction == action || String.IsNullOrWhiteSpace(action));

            return selected;
        }
    }
}
