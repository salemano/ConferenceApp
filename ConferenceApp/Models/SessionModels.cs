using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Services;
using Model.Models;
using MvcContrib.UI.Grid;

namespace ConferenceApp.Models
{
    public class EditSessionModel
    {
        public EditSessionModel()
        {
            Users = new List<UsersInSessions>();
        }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Overview")]
        public string Overview { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "Session type")]
        public int? SelectedTypeId { get; set; }
        public IEnumerable<SelectListItem> Types { get; set; }

        [Required]
        [Display(Name = "Start")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CreateStart { get; set; }

        [Required]
        [Display(Name = "End")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CreateEnd { get; set; }

        [Required]
        [Display(Name = "Registration closed at")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode=true)]
        public DateTime? CreateRegistrationClosedAt { get; set; }

        public int? Id { get; set; }
        public SessionPermissionModel Permissions { get; set; }

        public List<UsersInSessions> Users { get; set; }
    }

    public class SessionListViewModel
    {
        public GridSortOptions SortOptions { get; set; }

        public SessionFilterViewModel Filter { get; set; }

        public int Page { get; set; }

        public IEnumerable<SesionListItemViewModel> List { get; set; }
    }

    public class SesionListItemViewModel
    {
        public DateTime Start { get; set; }
        public IEnumerable<SessionRowItem> Sessions { get; set; }
    }

    public class MySessionListViewModel
    {
        public int Page { get; set; }

        public GridSortOptions SortOptions { get; set; }

        public IEnumerable<SessionRowItem> Sessions { get; set; }
    }

    public class SessionFilterViewModel
    {
        [DisplayName("Search")]
        public string FilterText { get; set; }
        [DisplayName("Show Closed")]
        public bool ShowClosed { get; set; }
        [DisplayName("From")]
        public DateTime? From { get; set; }
        [DisplayName("To")]
        public DateTime? To { get; set; }
        [Display(Name = "Session type")]
        public int? SelectedTypeId { get; set; }
        public IEnumerable<SelectListItem> Types { get; set; }
    }

    public class SessionViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public DateTime RegistrationCloseAt { get; set; }
        [DisplayName("Users")]
        public int NumberOfRegistrants { get; set; }
        public string Address { get; set; }
        public string Overview { get; set; }
        public int TypeId { get; set; }
        public SessionType Type { get { return (SessionType)TypeId; } }
        public SessionStatus Status { get; set; }
        public bool? IsAccepted { get; set; }
        public string RejectionReason { get; set; }
        public DateTime? AdminSubmittedAt { get; set; }
    }

    public class SessionRowItem : SessionViewModel
    {
        public bool IsRegistered { get; set; }
        public bool IsAuthor { get; set; }
    }

    public class RegisterInSessionViewModel
    {
        public int SessionId { get; set; }
        public bool IsRegistered { get; set; }
        public SessionViewModel SessionDescription { get; set; }
    }
}