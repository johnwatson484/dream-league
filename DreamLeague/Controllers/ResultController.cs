using DreamLeague.DAL;
using DreamLeague.Services;
using DreamLeague.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;

namespace DreamLeague.Controllers
{
    public class ResultController : Controller
    {
        DreamLeagueContext db;
        IGameWeekSerializer<GameWeekSummary> gameWeekSerializer;
        IGameWeekSerializer<CupWeekSummary> cupWeekSerializer;

        public ResultController()
        {
            this.db = new DreamLeagueContext();
            this.gameWeekSerializer = new XMLGameWeekSerializer<GameWeekSummary>();
            this.cupWeekSerializer = new XMLGameWeekSerializer<CupWeekSummary>();
        }

        public ResultController(DreamLeagueContext db, IGameWeekSerializer<GameWeekSummary> gameWeekSerializer, IGameWeekSerializer<CupWeekSummary> cupWeekSerializer)
        {
            this.db = db;
            this.gameWeekSerializer = gameWeekSerializer;
            this.cupWeekSerializer = cupWeekSerializer;
        }

        // GET: Result
        public ActionResult Index(int? gameWeekId)
        {
            if (gameWeekId == null)
            {
                gameWeekId = db.GameWeeks.AsNoTracking().Where(x => x.Complete).OrderByDescending(x => x.Number).Select(x => x.GameWeekId).FirstOrDefault();
            }

            GameWeekSummary gameWeekSummary = gameWeekSerializer.DeSerialize(gameWeekId.Value, "GameWeek");

            ViewBag.GameWeekId = new SelectList(db.GameWeeks.AsNoTracking().Where(x => x.Complete).OrderBy(x => x.Number), "GameWeekId", "Title", gameWeekId);

            return View(gameWeekSummary);
        }

        public ActionResult _CupResults(int gameWeekId)
        {
            List<CupWeekSummary> cupWeekSummaries = new List<CupWeekSummary>();

            var cups = db.Cups.AsNoTracking();

            foreach (var cup in cups)
            {
                var fixtureGameWeekId = db.Fixtures.AsNoTracking().Where(x => x.GameWeekId == gameWeekId && x.CupId == cup.CupId).Select(x => x.GameWeekId).FirstOrDefault();

                if (fixtureGameWeekId != 0)
                {
                    if (db.GameWeeks.Find(fixtureGameWeekId).Complete)
                    {
                        CupWeekSummary cupWeekSummary = cupWeekSerializer.DeSerialize(gameWeekId, string.Format("CupWeek_{0}", cup.CupId));

                        if (cupWeekSummary != null)
                        {
                            cupWeekSummaries.Add(cupWeekSummary);
                        }
                    }
                }
            }

            return PartialView(cupWeekSummaries);
        }
    }
}