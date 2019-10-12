using DreamLeague.DAL;
using DreamLeague.Services;
using DreamLeague.ViewModels;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace DreamLeague.Controllers
{
    public class StatisticsController : Controller
    {
        readonly IDreamLeagueContext db;
        readonly IStatisticsService statisticsService;

        public StatisticsController()
        {
            this.db = new DreamLeagueContext();
            this.statisticsService = new StatisticsService(db);
        }

        public StatisticsController(IDreamLeagueContext db, IStatisticsService statisticsService)
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
                .Take(db.Managers.AsNoTracking().Count())
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