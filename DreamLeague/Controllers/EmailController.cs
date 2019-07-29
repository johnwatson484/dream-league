using DreamLeague.DAL;
using DreamLeague.Models;
using DreamLeague.Services;
using DreamLeague.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DreamLeague.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class EmailController : Controller
    {
        readonly IDreamLeagueContext db;
        readonly IGameWeekSerializer<GameWeekSummary> gameWeekSerializer;
        readonly IGameWeekSerializer<CupWeekSummary> cupWeekSerializer;
        readonly IEmailService emailService;
        readonly IAuditService auditService;

        public EmailController()
        {
            this.db = new DreamLeagueContext();
            this.gameWeekSerializer = new XMLGameWeekSerializer<GameWeekSummary>();
            this.cupWeekSerializer = new XMLGameWeekSerializer<CupWeekSummary>();
            this.emailService = new Services.EmailService(db);
            this.auditService = new AuditService(db);
        }

        public EmailController(IDreamLeagueContext db, IGameWeekSerializer<GameWeekSummary> gameWeekSerializer, IGameWeekSerializer<CupWeekSummary> cupWeekSerializer, IEmailService emailService,
            IAuditService auditService)
        {
            this.db = db;
            this.gameWeekSerializer = gameWeekSerializer;
            this.cupWeekSerializer = cupWeekSerializer;
            this.emailService = emailService;
            this.auditService = auditService;
        }

        public ActionResult Email(int? gameWeekId)
        {
            if (gameWeekId == null)
            {
                gameWeekId = db.GameWeeks.AsNoTracking().Where(x => x.Complete).OrderByDescending(x => x.Number).Select(x => x.GameWeekId).FirstOrDefault();
            }

            return View(gameWeekSerializer.DeSerialize(gameWeekId.Value, "GameWeek"));
        }

        public ActionResult _EmailCup(int gameWeekId)
        {
            List<CupWeekSummary> cupWeekSummaries = new List<CupWeekSummary>();

            var cups = db.Cups.AsNoTracking();

            foreach (var cup in cups)
            {
                var fixtureGameWeekId = db.Fixtures.AsNoTracking().Where(x => x.GameWeekId == gameWeekId && x.CupId == cup.CupId).Select(x => x.GameWeekId).FirstOrDefault();

                if (fixtureGameWeekId != 0)
                {
                    cupWeekSummaries.Add(cupWeekSerializer.DeSerialize(gameWeekId, string.Format("CupWeek_{0}", cup.CupId)));
                }
            }

            return PartialView(cupWeekSummaries);
        }

        public ActionResult _EmailChangeLog(int gameWeekId)
        {
            var previous = db.Audit.Where(x => x.GameWeekId == gameWeekId && x.Action == "Game Week Email Sent").OrderByDescending(x => x.Date).ToList();

            if (previous.Count > 0)
            {
                ViewBag.Version = previous.Count + 1;
                var previousDate = previous.FirstOrDefault()?.Date;
                return PartialView(db.Audit.Where(x => x.GameWeekId == gameWeekId && (x.Area == "Goal" || x.Area == "Concede") && x.Date > previousDate).ToList());
            }

            return PartialView(new List<Audit>());
        }

        public ActionResult Send(int gameWeekId)
        {
            emailService.Send(gameWeekSerializer.DeSerialize(gameWeekId, "GameWeek"), ControllerContext);
            auditService.Log("Email", "Game Week Email Sent", User.Identity.Name, "Game Week Email Sent", gameWeekId);
            db.SaveChanges();

            return Content("Ok");
        }
    }
}