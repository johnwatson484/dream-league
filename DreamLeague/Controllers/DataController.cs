using DreamLeague.DAL;
using DreamLeague.Models;
using DreamLeague.Models.Api;
using DreamLeague.Services;
using DreamLeague.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace DreamLeague.Controllers
{
    public class DataController : ApiController
    {
        readonly IDreamLeagueContext db;
        readonly IGameWeekSerializer<GameWeekSummary> gameWeekSerializer;
        readonly IGameWeekService gameWeekService;

        public DataController()
        {
            this.db = new DreamLeagueContext();
            this.gameWeekSerializer = new XMLGameWeekSerializer<GameWeekSummary>();
            this.gameWeekService = new GameWeekService(db);
        }

        public DataController(IDreamLeagueContext db, IGameWeekSerializer<GameWeekSummary> gameWeekSerializer, IGameWeekService gameWeekService)
        {
            this.db = db;
            this.gameWeekSerializer = gameWeekSerializer;
            this.gameWeekService = gameWeekService;
        }

        [HttpGet]
        public List<Winner> Winners()
        {
            List<Winner> winners = new List<Winner>();

            var gameWeeks = db.GameWeeks.AsNoTracking().Where(x => x.Complete);

            foreach (var gameWeek in gameWeeks)
            {
                var gameWeekWinners = gameWeekSerializer.DeSerialize(gameWeek.Number, "GameWeek")?.Winners;

                foreach (var winner in gameWeekWinners)
                {
                    winners.Add(new Winner { GameWeek = gameWeek.Number, Name = winner });
                }
            }

            return winners;
        }

        [HttpGet]
        public List<TableRow> Table()
        {
            var gameWeek = gameWeekService.GetLatest();

            return gameWeek != null ? gameWeekSerializer.DeSerialize(gameWeek.Number, "GameWeek")?.Table?.TableRows : null;
        }

        [HttpGet]
        public List<Meeting> Meetings()
        {
            return db.Meetings.OrderBy(x => x.MeetingId).ToList();
        }
    }
}
