using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Web.Mvc.Controls;
using MvcContrib.Pagination;
using MvcContrib.UI.Grid;
using System.Web.Mvc.Html;
using MvcContrib.Sorting;

namespace ConferenceApp.Infrastructure
{
    public static class SortHtmlHelper
    {
        public static MvcHtmlString SortingOptions(this HtmlHelper helper, GridSortOptions sortOptions, object columnName, RouteValueDictionary getParams = null)
        {
            //copy existing parameters
            if (getParams == null)
                getParams = new RouteValueDictionary();
            var linkParams = new RouteValueDictionary();

            foreach (string key in helper.ViewContext.HttpContext.Request.QueryString)
            {
                getParams.Add(key, helper.ViewContext.HttpContext.Request.QueryString[key]);

                if (key != "Column" && key != "Direction")
                {
                    if (key == "Filter.ShowClosed")
                    {
                        if (helper.ViewContext.HttpContext.Request.QueryString[key].Contains("true"))
                            linkParams.Add(key, "true");
                        else linkParams.Add(key, "false");
                    }
                    else
                        linkParams.Add(key, helper.ViewContext.HttpContext.Request.QueryString[key]);
                }

            }

            if ((string)columnName == sortOptions.Column)
            {
                var direction = sortOptions.Direction;
                var newDirection = direction == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
                linkParams.Add("Direction", newDirection);
                linkParams.Add("Column", columnName.ToString());
            }
            else
            {
                linkParams.Add("Direction", SortDirection.Ascending);
                linkParams.Add("Column", columnName.ToString());
            }

            var link = helper.ActionLink((string)columnName, "List", "Session", linkParams, null);
            return link;
        }

        private static RouteValueDictionary Clone(this RouteValueDictionary original)
        {
            RouteValueDictionary newDict = new RouteValueDictionary();
            foreach (KeyValuePair<string, object> kvp in original)
                newDict.Add(kvp.Key, kvp.Value);
            return newDict;
        }
    }
}