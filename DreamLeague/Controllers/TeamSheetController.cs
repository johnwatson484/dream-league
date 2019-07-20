using DreamLeague.DAL;
using DreamLeague.Inputs;
using DreamLeague.Models;
using DreamLeague.ViewModels;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DreamLeague.Controllers
{
    public class TeamSheetController : Controller
    {
        readonly DreamLeagueContext db;
        readonly ITeamSheetService teamSheetService;

        public TeamSheetController()
        {
            this.db = new DreamLeagueContext();
            this.teamSheetService = new TeamSheetService(new XLSXTeamSheetReader());
        }

        public TeamSheetController(DreamLeagueContext db, ITeamSheetService teamSheetService)
        {
            this.db = db;
            this.teamSheetService = teamSheetService;
        }

        public async Task<ActionResult> Index()
        {
            return View(await db.Managers.AsNoTracking().Include(x => x.Players.Select(p => p.Player)).Include(x => x.GoalKeepers.Select(t => t.Team)).OrderBy(x => x.Name).ToListAsync());
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Edit()
        {
            TeamSheet teamSheet = teamSheetService.Get();
            var managers = db.Managers.AsNoTracking()
                .Include(x => x.Players.Select(p => p.Player))
                .Include(x => x.GoalKeepers.Select(t => t.Team))
                .OrderBy(x => x.Name)
                .ToList();

            ViewBag.LastUpload = teamSheetService.LastUpload();
            ViewBag.NoNews = true;

            return View(new ManagerTeamSheet(managers, teamSheet));
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult EditPlayer(int managerId, int[] playerIds, int[] playerSubs)
        {
            if (playerSubs == null)
            {
                playerSubs = new int[0];
            }

            var manager = db.Managers.Include(x => x.Players).Where(x => x.ManagerId == managerId).FirstOrDefault();

            if (manager != null)
            {
                var selectedPlayers = new HashSet<int>(playerIds.Where(x => x != 0));
                var currentPlayers = new HashSet<int>(manager.Players.Select(x => x.PlayerId));

                foreach (var currentPlayer in currentPlayers)
                {
                    if (!selectedPlayers.Contains(currentPlayer))
                    {
                        db.ManagerPlayers.Remove(manager.Players.Where(x => x.PlayerId == currentPlayer).FirstOrDefault());
                    }
                }

                foreach (var selectedPlayer in selectedPlayers)
                {
                    if (!currentPlayers.Contains(selectedPlayer))
                    {
                        ManagerPlayer managerPlayer = new ManagerPlayer(selectedPlayer, managerId);
                        db.ManagerPlayers.Add(managerPlayer);
                    }
                }

                var selectedSubs = new HashSet<int>(playerSubs.Where(x => x != 0));
                var currentSubs = new HashSet<int>(manager.Players.Where(x => x.Substitute).Select(x => x.PlayerId));

                foreach (var currentSub in currentSubs)
                {
                    if (!selectedSubs.Contains(currentSub))
                    {
                        ManagerPlayer managerPlayer = manager.Players.Where(x => x.PlayerId == currentSub).FirstOrDefault();
                        managerPlayer.ToggleSubstitute();
                    }
                }

                foreach (var selectedSub in selectedSubs)
                {
                    if (!currentSubs.Contains(selectedSub))
                    {
                        ManagerPlayer managerPlayer = manager.Players.Where(x => x.PlayerId == selectedSub).FirstOrDefault();
                        managerPlayer.ToggleSubstitute();
                    }
                }
            }

            db.SaveChanges();

            return Content("Ok");
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult EditGoalKeeper(int managerId, int[] teamIds, int[] teamSubs)
        {
            if (teamSubs == null)
            {
                teamSubs = new int[0];
            }

            var manager = db.Managers.Include(x => x.GoalKeepers).Where(x => x.ManagerId == managerId).FirstOrDefault();

            if (manager != null)
            {
                var selectedTeams = new HashSet<int>(teamIds.Where(x => x != 0));
                var currentTeams = new HashSet<int>(manager.GoalKeepers.Select(x => x.TeamId));

                foreach (var currentTeam in currentTeams)
                {
                    if (!selectedTeams.Contains(currentTeam))
                    {
                        db.ManagerGoalKeepers.Remove(manager.GoalKeepers.Where(x => x.TeamId == currentTeam).FirstOrDefault());
                    }
                }

                foreach (var selectedTeam in selectedTeams)
                {
                    if (!currentTeams.Contains(selectedTeam))
                    {
                        ManagerGoalKeeper managerGoalKeeper = new ManagerGoalKeeper(selectedTeam, managerId);
                        db.ManagerGoalKeepers.Add(managerGoalKeeper);
                    }
                }

                var selectedSubs = new HashSet<int>(teamSubs.Where(x => x != 0));
                var currentSubs = new HashSet<int>(manager.GoalKeepers.Where(x => x.Substitute).Select(x => x.TeamId));

                foreach (var currentSub in currentSubs)
                {
                    if (!selectedSubs.Contains(currentSub))
                    {
                        ManagerGoalKeeper managerGoalKeeper = manager.GoalKeepers.Where(x => x.TeamId == currentSub).FirstOrDefault();
                        managerGoalKeeper.ToggleSubstitute();
                    }
                }

                foreach (var selectedSub in selectedSubs)
                {
                    if (!currentSubs.Contains(selectedSub))
                    {
                        ManagerGoalKeeper managerGoalKeeper = manager.GoalKeepers.Where(x => x.TeamId == selectedSub).FirstOrDefault();
                        managerGoalKeeper.ToggleSubstitute();
                    }
                }
            }

            db.SaveChanges();

            return Content("Ok");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            if (Path.GetExtension(file.FileName) == ".xlsx")
            {
                teamSheetService.Upload(file);
            }

            return RedirectToAction("Edit");
        }
    }
}