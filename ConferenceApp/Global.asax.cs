using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Globalization;
using System.Threading;
using Core.Services;

namespace ConferenceApp
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            ModelBinders.Binders.Add(typeof(DateTime), new DateTimeBinder());
            ModelBinders.Binders.Add(typeof(DateTime?), new DateTimeBinder());
        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            //It's important to check whether session object is ready
            if (HttpContext.Current.Session != null)
            {
                if (this.Session["culture"] == null)
                    Session["culture"] = "en-GB";

                var ci = new CultureInfo((string)Session["culture"]);
                //Finally setting culture for each request
                Thread.CurrentThread.CurrentUICulture = ci;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            var baseException = Server.GetLastError().GetBaseException();
            string detail = "Error Caught in Application_Error event\n" +
                    "Error in: " + Request.Url.ToString() +
                    "\nError Message:" + exception.Message.ToString() +
                    "\nStack Trace:" + exception.StackTrace.ToString();

            var service = DependencyResolver.Current.GetService<ILogger>();
            try
            {
                service.Log(LogLevel.ERROR, LogType.SYSTEM, User.Identity.Name, Request.UserHostAddress, "Uncaught Application Error", detail, null);
            }
            catch { }

            Response.Clear();

            HttpException httpException = exception as HttpException;

            if (httpException != null)
            {
                string action;

                switch (httpException.GetHttpCode())
                {
                    case 404:
                        // page not found
                        action = "HttpError404";
                        break;
                    case 500:
                        // server error
                        action = "HttpError500";
                        break;
                    default:
                        action = "General";
                        break;
                }

                // clear error on server
                Server.ClearError();
                Response.Redirect(String.Format("~/Error/{0}", action));
            }
        }
    }

    public class DateTimeBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            try
            {
                ValueProviderResult vpr = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
                bindingContext.ModelState.SetModelValue(bindingContext.ModelName, bindingContext.ValueProvider.GetValue(bindingContext.ModelName));
                object date = vpr.ConvertTo(typeof(DateTime), CultureInfo.CurrentCulture);
                return date;
            }
            catch (Exception)
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Please enter a valid date");
                return null;
            }
        }
    }
}