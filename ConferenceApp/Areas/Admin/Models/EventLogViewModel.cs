using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.Models;
using MvcContrib.UI.Grid;

namespace ConferenceApp.Areas.Admin.Models
{
    public class EventLogViewModel
    {
        public IEnumerable<EventLog> Logs { get; set; }
        public GridSortOptions SortOptions { get; set; }
        public int? Page { get; set; }
    }
}