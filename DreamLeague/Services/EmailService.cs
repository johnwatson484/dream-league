using DreamLeague.DAL;
using DreamLeague.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using PreMailer.Net;
using System.Data.Entity;

namespace DreamLeague.Services
{
    public class EmailService : IEmailService
    {
        DreamLeagueContext db;
        static string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content");

        public EmailService(DreamLeagueContext db)
        {
            this.db = db;
        }

        public void Send(GameWeekSummary gameWeekSummary, ControllerContext context)
        {
            SmtpClient smtpClient = new SmtpClient();            

            MailMessage message = new MailMessage();
            message.IsBodyHtml = true;
            message.Subject = string.Format("Dream League Results - Game Week {0}", gameWeekSummary.GameWeek.Number);

            message.Body = RenderViewToString(context, "Email", gameWeekSummary);
            message.Body = PreMailer.Net.PreMailer.MoveCssInline(message.Body, false, stripIdAndClassAttributes:true).Html;

            var managers = db.Managers.AsNoTracking().Include(x => x.Emails);

            foreach(var manager in managers)
            {
                foreach(var email in manager.Emails)
                {                    
                    message.To.Add(email.Address);
                }
            }

            smtpClient.Send(message);
        }

        private string RenderViewToString(ControllerContext context, string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = context.RouteData.GetRequiredString("action");
            }

            ViewDataDictionary viewData = new ViewDataDictionary(model);

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(context, viewName);
                ViewContext viewContext = new ViewContext(context, viewResult.View, viewData, new TempDataDictionary(), sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        public string GetAll()
        {
            var emails = db.Emails.AsNoTracking();

            StringBuilder sb = new StringBuilder();

            foreach (var email in emails)
            {
                sb.Append(email.Address);
                sb.Append(";");
            }

            return sb.ToString();
        }
    }
}