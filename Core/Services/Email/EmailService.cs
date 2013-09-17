using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Net;
using System.Configuration;
using System.Net.Mail;
using System.IO;

namespace Core.Services
{
    public class EmailService: IEmailService
    {
        public void WriteMessageToFile(EmailDescription desc)
        {
            var path = @"c:\ConferenceAppEmails";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            var timeStamp = string.Format("{0:yyyy-MM-dd_hh-mm-ss-tt}.txt", DateTime.Now);
            using (var writetext = new StreamWriter(string.Format(@"{0}\{1}_{2}_{3}.txt", path, desc.Subject, desc.To, timeStamp)))
            {
                writetext.Write(desc.Body);
            }
        }

        public void SendMessage(EmailDescription desc)
        {
            WriteMessageToFile(desc);
            return;

            // following values come from Web.Config file.
            string username = ConfigurationManager.AppSettings["emailUserName"].ToString();
            string password = ConfigurationManager.AppSettings["emailPassword"].ToString();
            string from = ConfigurationManager.AppSettings["emailFrom"].ToString();
            string host = ConfigurationManager.AppSettings["smtpHost"].ToString();
            int port = 0;
            bool havePort = int.TryParse(ConfigurationManager.AppSettings["smtpPort"] ?? "".ToString(), out port);
            bool enableSsl = true;
            
            bool haveSSL = bool.TryParse(ConfigurationManager.AppSettings["smtpSSL"] ?? "".ToString(), out enableSsl);

            MailMessage mm = new MailMessage(from, desc.To, desc.Subject, desc.Body);

            mm.IsBodyHtml = true;

            if (!String.IsNullOrEmpty(desc.CC))
            {
                mm.CC.Add(desc.CC);
            }

            NetworkCredential credentials = new NetworkCredential(username, password);
            SmtpClient sc = havePort ? new SmtpClient(host) : new SmtpClient(host, port);
            sc.EnableSsl = true ;//sc.EnableSsl = haveSSL ? enableSsl : false;
            sc.UseDefaultCredentials = false;
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
        /// </summary>
        /// <returns></returns>
        public string GetHostAddress()
        {
            return ConfigurationManager.AppSettings["hostAddress"].ToString();
        }
    }

    public class EmailDescription
    {
        public string From { get; set; }
        public string To { get; set; }
        public string CC { get; set; }
        public string Subject { get; set; }
        public string ReplyTo { get; set; }
        public string Body { get; set; }
    }
}
