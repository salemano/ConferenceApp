using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Net;
using RazorEngine;
using System.Configuration;
using System.Net.Mail;
using Core.Services.Email;

namespace Core.Services
{
    public class EmailDescription
    {
        public string From { get; set; }
        public string To { get; set; }
        public string CC { get; set; }
        public string Subject { get; set; }
        public string ReplyTo { get; set; }
    }

    public enum Mail
    {
        RequestResetPassword,
        RequestApproveTimesheet,
        RequestApproveRatings,
        RequestAdminApproveRatings,
    }

    public class EmailService: IEmailService
    {
        static string physicalApplicationPath = HostingEnvironment.ApplicationPhysicalPath;

        public void SendEmail(Mail emailName, object model, EmailDescription desc)
        {
            var templatePath = physicalApplicationPath + @"Service\Email\EmailTemplates\" + emailName + ".cshtml";
            var template = System.IO.File.ReadAllText(templatePath, System.Text.Encoding.Default);
            var body = Razor.Parse(template, model);

            SendMessage(desc.To, desc.CC, desc.Subject, body);
        }

        void SendMessage(string to, string cc, string subject, string message)
        {
            // following values come from Web.Config file.
            string username = ConfigurationManager.AppSettings["emailUserName"].ToString();
            string password = ConfigurationManager.AppSettings["emailPassword"].ToString();
            string from = ConfigurationManager.AppSettings["emailFrom"].ToString();
            string host = ConfigurationManager.AppSettings["smtpHost"].ToString();
            int port = 0;
            bool havePort = int.TryParse(ConfigurationManager.AppSettings["smtpPort"] ?? "".ToString(), out port);
            bool enableSsl = false;
            bool haveSSL = bool.TryParse(ConfigurationManager.AppSettings["smtpSSL"] ?? "".ToString(), out enableSsl);

            MailMessage mm = new MailMessage(from, to, subject, message);

            mm.IsBodyHtml = true;

            if (!String.IsNullOrEmpty(cc))
            {
                mm.CC.Add(cc);
            }

            NetworkCredential credentials = new NetworkCredential(username, password);
            SmtpClient sc = havePort ? new SmtpClient(host) : new SmtpClient(host, port);
            sc.EnableSsl = haveSSL ? enableSsl : false;
            sc.Credentials = credentials;

            try
            {
                sc.Send(mm);
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("Failed to send email", ex);
            }
        }

        /// <summary>
        /// gets host address prefix from web.config file.
        /// such as "http://www.vbrs.co.nz"
        /// </summary>
        /// <returns></returns>
        public string GetHostAddress()
        {
            return ConfigurationManager.AppSettings["hostAddress"].ToString();
        }
    }
}
