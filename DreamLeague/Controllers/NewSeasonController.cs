using DreamLeague.DAL;
using DreamLeague.Inputs;
using DreamLeague.Models;
using DreamLeague.Services;
using DreamLeague.ViewModels;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace DreamLeague.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class NewSeasonController : Controller
    {
        DreamLeagueContext db;
        IGameWeekSerializer<GameWeekSummary> gameWeekSerializer;
        IGameWeekSerializer<CupWeekSummary> cupWeekSerializer;
        IPlayerListService playerListService;
        ITeamSheetService teamSheetService;

        public NewSeasonController()
        {
            this.db = new DreamLeagueContext();
            this.gameWeekSerializer = new XMLGameWeekSerializer<GameWeekSummary>();
            this.cupWeekSerializer = new XMLGameWeekSerializer<CupWeekSummary>();
            this.playerListService = new PlayerListService(db, new XLSXPlayerListReader());
            this.teamSheetService = new TeamSheetService(new XLSXTeamSheetReader());
        }

        public NewSeasonController(DreamLeagueContext db, IGameWeekSerializer<GameWeekSummary> gameWeekSerializer, IGameWeekSerializer<CupWeekSummary> cupWeekSerializer, IPlayerListService playerListService, ITeamSheetService teamSheetService)
        {
            this.db = db;
            this.gameWeekSerializer = gameWeekSerializer;
            this.cupWeekSerializer = cupWeekSerializer;
            this.playerListService = playerListService;
            this.teamSheetService = teamSheetService;
        }

        public ActionResult Index(string message = null, string warning = null)
        {
            ViewBag.Message = message;
            ViewBag.Warning = warning;

            return View();
        }

        [HttpPost]
        public ActionResult Clear()
        {
            var goals = db.Goals;
            foreach (var goal in goals)
            {
                db.Goals.Remove(goal);
            }
            db.Database.ExecuteSqlCommand("DBCC CHECKIDENT('Results.Goals', RESEED, 0)");

            var conceded = db.Conceded;
            foreach (var concede in conceded)
            {
                db.Conceded.Remove(concede);
            }
            db.Database.ExecuteSqlCommand("DBCC CHECKIDENT('Results.Conceded', RESEED, 0)");

            var managerPlayers = db.ManagerPlayers;
            foreach (var managerPlayer in managerPlayers)
            {
                db.ManagerPlayers.Remove(managerPlayer);
            }
            db.Database.ExecuteSqlCommand("DBCC CHECKIDENT('DreamLeague.ManagerPlayers', RESEED, 0)");

            var managerGoalKeepers = db.ManagerGoalKeepers;
            foreach (var managerGoalKeeper in managerGoalKeepers)
            {
                db.ManagerGoalKeepers.Remove(managerGoalKeeper);
            }
            db.Database.ExecuteSqlCommand("DBCC CHECKIDENT('DreamLeague.ManagerGoalKeepers', RESEED, 0)");

            var fixtures = db.Fixtures;
            foreach (var fixture in fixtures)
            {
                db.Fixtures.Remove(fixture);
            }
            db.Database.ExecuteSqlCommand("DBCC CHECKIDENT('Cup.Fixtures', RESEED, 0)");

            var groups = db.Groups;
            foreach (var group in groups)
            {
                db.Groups.Remove(group);
            }
            db.Database.ExecuteSqlCommand("DBCC CHECKIDENT('Cup.Groups', RESEED, 0)");

            var audits = db.Audit;
            foreach (var audit in audits)
            {
                db.Audit.Remove(audit);
            }
            db.Database.ExecuteSqlCommand("DBCC CHECKIDENT('DreamLeague.Audit', RESEED, 0)");

            var gameWeeks = db.GameWeeks;
            foreach (var gameWeek in gameWeeks)
            {
                gameWeek.SetIncomplete();
            }

            db.SaveChanges();

            gameWeekSerializer.DeleteAll("GameWeek");
            cupWeekSerializer.DeleteAll("CupWeek");
            teamSheetService.DeleteAll();

            return Content("Ok");
        }

        [HttpPost]
        public ActionResult GameWeeks(DateTime startDate, int total)
        {
            var gameWeeks = db.GameWeeks;
            foreach (var gameWeek in gameWeeks)
            {
                db.GameWeeks.Remove(gameWeek);
            }
            db.Database.ExecuteSqlCommand("DBCC CHECKIDENT('DreamLeague.GameWeeks', RESEED, 0)");

            DateTime currentStart = startDate;

            for (int i = 0; i < total; i++)
            {
                GameWeek gameWeek = new GameWeek(i + 1, currentStart);
                db.GameWeeks.Add(gameWeek);

                currentStart = currentStart.AddDays(7);
            }

            db.SaveChanges();

            return Content("Ok");
        }

        [HttpPost]
        public ActionResult Meetings(DateTime startDate, int total, string location, double lon, double lat)
        {
            var meetings = db.Meetings;
            foreach (var meeting in meetings)
            {
                db.Meetings.Remove(meeting);
            }
            db.Database.ExecuteSqlCommand("DBCC CHECKIDENT('DreamLeague.Meetings', RESEED, 0)");

            Meeting firstMeeting = new Meeting(startDate.AddHours(18).AddMinutes(30), location, lon, lat);
            db.Meetings.Add(firstMeeting);

            DateTime currentStart = new DateTime(startDate.Year, startDate.Month, 1, 19, 30, 0);
            currentStart = currentStart.AddMonths(1);

            for (int i = 0; i < total - 1; i++)
            {
                currentStart = currentStart.AddMonths(1);

                DateTime temp = currentStart;

                while (temp.DayOfWeek != DayOfWeek.Wednesday)
                {
                    temp = temp.AddDays(1);
                }

                Meeting meeting = new Meeting(temp, location, lon, lat);
                db.Meetings.Add(meeting);
            }

            db.SaveChanges();

            return Content("Ok");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Players(HttpPostedFileBase file)
        {
            string result = "File format must be .xlsx";

            if (Path.GetExtension(file.FileName) == ".xlsx")
            {
                result = playerListService.Upload(file);
            }
            else if (Path.GetExtension(file.FileName) == ".csv")
            {
                playerListService = new PlayerListService(db, new CSVPlayerListReader());
                result = playerListService.Upload(file);
            }

            if (result == "Success")
            {
                return RedirectToAction("Index", new { message = "Players uploaded." });
            }

            return RedirectToAction("Index", new { warning = result });
        }
    }
}