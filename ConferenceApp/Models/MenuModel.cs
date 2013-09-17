using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ConferenceApp.Resourses.Shared;

namespace ConferenceApp.Models
{
    public class MenuItemViewModel
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string NavigateUrl { get; set; }
        public string Role { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Area { get; set; }
        public bool IsSelected { get; set; }
        public string Url { get; set; }
        public bool Visible { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsAuthenticated { get; set; }

        public List<MenuItemViewModel> Nodes { get; set; }
    }
}