using DreamLeague.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using DreamLeague.Services;
using DreamLeague.ViewModels;

namespace DreamLeague.Controllers
{
    public class StatisticsController : Controller
    {
        DreamLeagueContext db;
        IStatisticsService statisticsService;

        public StatisticsController()
        {
            this.db = new DreamLeagueContext();
            this.statisticsService = new StatisticsService(db);
        }

        public StatisticsController(DreamLeagueContext db, IStatisticsService statisticsService)
        {
            this.db = db;
            this.statisticsService = statisticsService;
        }

        public ActionResult _TopGoalScorer()
        {
            var scorers = db.Players.AsNoTracking()
                .Include(x => x.Team)
                .Include(x => x.ManagerPlayers
                .Select(m => m.Manager))
                .OrderByDescending(x => x.Goals.Where(g => !g.Cup).Count())
                .ThenBy(x => x.LastName)
                .Take(10)
                .ToList();

            return PartialView(scorers);
        }

        public ActionResult _Form()
        {
            List<Form> formList = new List<Form>();

            var managers = db.Managers.AsNoTracking();

            foreach (var manager in managers)
            {
                Form form = statisticsService.GetForm(manager.ManagerId);
                formList.Add(form);
            }

            return PartialView(formList.OrderByDescending(x => x.Value).ThenBy(x => x.Manager).ToList());
        }
    }
}