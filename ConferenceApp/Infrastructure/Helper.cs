using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using RazorEngine;

namespace ConferenceApp.Infrastructure
{
    public class Helper
    {
        static string physicalApplicationPath = HostingEnvironment.ApplicationPhysicalPath;

        public static string GetEmailBody(Mail emailName, object model)
        {
            var templatePath = physicalApplicationPath + @"Views\EmailTemplates\" + emailName + ".cshtml";
            var template = System.IO.File.ReadAllText(templatePath, System.Text.Encoding.Default);

            return Razor.Parse(template, model);
        }

        public static string GetHostAddress()
        {
            return ConfigurationManager.AppSettings["hostAddress"].ToString();
        }

        public static string GetCurrentUrl()
        {
            var request = HttpContext.Current.Request;
            var querystring = HttpUtility.ParseQueryString(request.UrlReferrer.Query);

            return request.UrlReferrer.AbsolutePath+ "?" + querystring.ToString();
        }
    }

    public enum Mail
    {
        RegistrationConfirmation,
        RequestResetPassword,
        AdminRegistrationConfirmation,
        AdminRequestResetPassword
    }

    public enum UserValidationResult
    {
        InvalidUser,
        NotActivated,
        PasswordMismatch
    }
}