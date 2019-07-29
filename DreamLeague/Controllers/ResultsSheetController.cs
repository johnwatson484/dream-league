using DreamLeague.DAL;
using DreamLeague.Models;
using DreamLeague.Services;
using DreamLeague.ViewModels;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace DreamLeague.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ResultsSheetController : Controller
    {
        readonly IDreamLeagueContext db;
        readonly IGameWeekService gameWeekService;
        readonly IGameWeekSummaryService gameWeekSummaryService;
        readonly IGameWeekSummaryService cupWeekSummaryService;
        readonly IAuditService auditService;

        public ResultsSheetController()
        {
            this.db = new DreamLeagueContext();
            this.gameWeekService = new GameWeekService(db);
            this.gameWeekSummaryService = new GameWeekSummaryService(db, new XMLGameWeekSerializer<GameWeekSummary>());
            this.cupWeekSummaryService = new CupWeekSummaryService(db, new XMLGameWeekSerializer<CupWeekSummary>());
            this.auditService = new AuditService(db);
        }

        public ResultsSheetController(IDreamLeagueContext db, IGameWeekService gameWeekService, IGameWeekSummaryService gameWeekSummaryService, IGameWeekSummaryService cupWeekSummaryService, IAuditService auditService)
        {
            this.db = db;
            this.gameWeekService = gameWeekService;
            this.gameWeekSummaryService = gameWeekSummaryService;
            this.cupWeekSummaryService = cupWeekSummaryService;
            this.auditService = auditService;
        }

        public ActionResult Index()
        {
            var goalKeepers = db.ManagerGoalKeepers.AsNoTracking().Include(x => x.Team).Include(x => x.Manager).OrderBy(x => x.Substitute).ThenBy(x => x.Team.League.Rank).ThenBy(x => x.Team.Name).ToList();
            var players = db.ManagerPlayers.AsNoTracking().Include(x => x.Player).Include(x => x.Manager).Where(x => !x.Substitute).OrderBy(x => x.Player.Team.League.Rank).ThenBy(x => x.Player.Team.Name).ThenBy(x => x.Player.LastName).ThenBy(x => x.Player.FirstName).ToList();
            var managerCupWeeks = gameWeekService.ManagerCupWeeks();

            ViewBag.GameWeekId = new SelectList(db.GameWeeks.AsNoTracking().OrderBy(x => x.Start), "GameWeekId", "Details", gameWeekService.GetCurrent()?.GameWeekId);
            ViewBag.NoNews = true;

            return View(new ResultsSheet(goalKeepers, players, managerCupWeeks));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ResultsSheet resultsSheet)
        {
            var managerCupWeeks = gameWeekService.ManagerCupWeeks(resultsSheet.GameWeekId);

            foreach (var goalKeeper in resultsSheet.GoalKeepers)
            {
                bool substitute = false;

                if (goalKeeper.SubstitutePlayed)
                {
                    substitute = true;
                }

                if (goalKeeper.Conceded > 0)
                {
                    for (int i = 0; i < goalKeeper.Conceded; i++)
                    {
                        Concede concede = new Concede(goalKeeper.GoalKeeper.TeamId, resultsSheet.GameWeekId, goalKeeper.GoalKeeper.ManagerId, substitute);
                        db.Conceded.Add(concede);
                        var team = db.Teams.Where(x => x.TeamId == concede.TeamId).FirstOrDefault();
                        auditService.Log("Concede", "Concede Added", User.Identity.Name, string.Format("Goal conceded for {0} ({1})", team?.Name, team?.ManagerGoalKeepers?.FirstOrDefault()?.Manager?.Name ?? "Unattached"), resultsSheet.GameWeekId);
                    }
                }

                if (managerCupWeeks.Exists(x => x.ManagerId == goalKeeper.GoalKeeper.ManagerId))
                {
                    if (goalKeeper.CupConceded > 0)
                    {
                        for (int i = 0; i < goalKeeper.CupConceded; i++)
                        {
                            Concede concede = new Concede(goalKeeper.Substitute.TeamId, resultsSheet.GameWeekId, goalKeeper.Substitute.ManagerId, substitute, true);
                            db.Conceded.Add(concede);
                            var team = db.Teams.Where(x => x.TeamId == concede.TeamId).FirstOrDefault();
                            auditService.Log("Concede", "Concede Added", User.Identity.Name, string.Format("Cup goal conceded for {0} ({1})", team?.Name, team?.ManagerGoalKeepers?.FirstOrDefault()?.Manager?.Name ?? "Unattached"), resultsSheet.GameWeekId);
                        }
                    }
                }
            }

            foreach (var player in resultsSheet.Players)
            {
                if (player.Goals > 0)
                {
                    for (int i = 0; i < player.Goals; i++)
                    {
                        Goal goal = new Goal(player.Player.PlayerId, resultsSheet.GameWeekId, player.Player.ManagerId);
                        db.Goals.Add(goal);
                        var playerT = db.Players.Where(x => x.PlayerId == goal.PlayerId).FirstOrDefault();
                        auditService.Log("Goal", "Goal Added", User.Identity.Name, string.Format("Goal scored for {0} ({1})", playerT?.FullName, playerT?.ManagerPlayers.FirstOrDefault()?.Manager?.Name ?? "Unattached"), resultsSheet.GameWeekId);
                    }
                }

                if (managerCupWeeks.Exists(x => x.ManagerId == player.Player.ManagerId))
                {
                    if (player.CupGoals > 0)
                    {
                        for (int i = 0; i < player.CupGoals; i++)
                        {
                            Goal goal = new Goal(player.Player.PlayerId, resultsSheet.GameWeekId, player.Player.ManagerId, true);
                            db.Goals.Add(goal);
                            var playerT = db.Players.Where(x => x.PlayerId == goal.PlayerId).FirstOrDefault();
                            auditService.Log("Goal", "Goal Added", User.Identity.Name, string.Format("Cup goal scored for {0} ({1})", playerT?.FullName, playerT?.ManagerPlayers.FirstOrDefault()?.Manager?.Name ?? "Unattached"), resultsSheet.GameWeekId);
                        }
                    }
                }
            }

            var gameWeek = db.GameWeeks.Find(resultsSheet.GameWeekId);
            gameWeek.SetComplete();

            db.SaveChanges();

            gameWeekSummaryService.Create(resultsSheet.GameWeekId);
            cupWeekSummaryService.Create(resultsSheet.GameWeekId);

            return RedirectToAction("Index", "Result", new { gameWeekId = resultsSheet.GameWeekId });
        }

        [HttpPost]
        public ActionResult Refresh()
        {
            gameWeekSummaryService.Refresh();
            cupWeekSummaryService.Refresh();

            return RedirectToAction("Index", "Result");
        }
    }
}