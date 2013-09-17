using MvcContrib.UI.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace ConferenceApp.Areas.Admin.Models
{
    public class UserListViewModel
    {
        public IEnumerable<UserDescriptionModel> Users { get; set; }

        public GridSortOptions SortOptions { get; set; }

        [DisplayName("Search")]
        public string FilterText { get; set; }

        public int Page { get; set; }
    }

    public class UserDescriptionModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public Guid? ActivationToken { get; set; }
        public DateTime? ActivatedAt { get; set; }
        public bool IsAdministrator { get; set; }
        public string FirstName { get; set; }
        public Nullable<DateTime> DateOfBirth { get; set; }
    }
}