using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ConferenceApp.Areas.Admin.Models;
using Core.Services;
using Model.Models;
using MvcContrib.UI.Grid;
using MvcContrib.Pagination;
using MvcContrib.Sorting;

namespace ConferenceApp.Areas.Admin.Controllers
{
    public class EventLogController : Controller
    {
        private ILogger _logger;

        public EventLogController(ILogger logger)
        {
            _logger = logger;
        }
        public ActionResult Index(int? page, GridSortOptions sortOptions)
        {
            if (sortOptions.Column == null)
                sortOptions = new GridSortOptions
                {
                    Column = "CreateDate",
                    Direction = SortDirection.Descending
                };

            page = page ?? 1;

            var log = _logger.GetAll();

            log = log.OrderBy(sortOptions.Column, sortOptions.Direction);

            var vm = new EventLogViewModel
            {
                Logs = log.AsPagination(page.Value, 10),
                SortOptions = sortOptions,
                Page = page.Value
            };

            return View(vm);
        }
    }
}
