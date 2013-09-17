using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConferenceApp
{
    public static class ViewContextExtensions
    {
        public static string GetViewContextValue(ControllerBase context, string valueKey)
        {
            var value = context.ValueProvider.GetValue(valueKey);

            if (value == null)
                return null;

            return value.RawValue as string;
        }

        public static string GetControllerName(this ControllerBase context)
        {
            return GetViewContextValue(context, "controller");
        }

        public static string GetActionName(this ControllerBase context)
        {
            return GetViewContextValue(context, "action");
        }

        public static string GetAreaName(this ControllerBase context)
        {
            if (System.Web.HttpContext.Current.Request.Url.ToString().Contains("/Admin"))
                return "Admin";

            return GetViewContextValue(context, "area");
        }
    }
}