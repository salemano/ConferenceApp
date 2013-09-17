using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.WebPages.Html;
using ConferenceApp.Models;
using Model.Models;
using MvcContrib.UI.Grid;

namespace ConferenceApp.Areas.Admin.Models
{
    public class SessionRequestsViewModel
    {
        public GridSortOptions SortOptions { get; set; }

        public int Page { get; set; }
        public IEnumerable<SessionDescriptionModel> Sessions { get; set; }
    }

    public class AdminSessionListViewModel
    {
        public IEnumerable<SessionDescriptionModel> Sessions { get; set; }
        public SessionFilterViewModel Filter { get; set; }
        public GridSortOptions SortOptions { get; set; }

        public int Page { get; set; }
    }

    public class SessionDescriptionModel
    {
        public int CreatedByUserId { get; set; }
        public string Email { get; set; }
        public int SessionId { get; set; }
        public string Title { get; set; }
        [DisplayName("Users")]
        public int NumberOfRegistrants { get; set; }
        public string Date
        {
            get
            {
                return string.Format("{0} - {1}", Start.ToShortDateString(), End.ToShortDateString());
            }
        }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public DateTime RegistrationCloseAt { get; set; }
        public int TypeId { get; set; }
        public int StatusId { get; set; }
        public SessionType Type { get { return (SessionType)TypeId; } }
        public SessionStatus Status { get { return (SessionStatus)StatusId; } }
        public bool? IsAccepted { get; set; }
        public string RejectionReason { get; set; }
        public DateTime? AdminSubmittedAt { get; set; }
    }

    public class RejectSessionViewModel
    {
        public int SessionId { get; set; }
        [DisplayName("Session Name")]
        public string SessionTitle { get; set; }
        public int UserId { get; set; }

        [Required]
        [DisplayName("Reject reason")]
        [MaxLength(255, ErrorMessage = "Message is too long")]
        public string RejectionReason { get; set; }
    }
}