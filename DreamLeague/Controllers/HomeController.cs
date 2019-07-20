using DreamLeague.DAL;
using DreamLeague.Services;
using DreamLeague.ViewModels;
using System.Linq;
using System.Web.Mvc;

namespace DreamLeague.Controllers
{
    public class HomeController : Controller
    {
        DreamLeagueContext db;
        IGameWeekSerializer<GameWeekSummary> gameWeekSerializer;
        ISearchService searchService;

        public HomeController()
        {
            this.db = new DreamLeagueContext();
            this.gameWeekSerializer = new XMLGameWeekSerializer<GameWeekSummary>();
            this.searchService = new SearchService(db);
        }

        public HomeController(DreamLeagueContext db, IGameWeekSerializer<GameWeekSummary> gameWeekSerializer, ISearchService searchService)
        {
            this.db = db;
            this.gameWeekSerializer = gameWeekSerializer;
            this.searchService = searchService;
        }

        public ActionResult Index()
        {
            var gameWeekId = db.GameWeeks.AsNoTracking().Where(x => x.Complete).OrderByDescending(x => x.Number).Select(x => x.GameWeekId).FirstOrDefault();

            GameWeekSummary gameWeekSummary = gameWeekSerializer.DeSerialize(gameWeekId, "GameWeek");

            return View(gameWeekSummary);
        }

        public ActionResult About()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AutoComplete(string prefix)
        {
            var response = searchService.Search(prefix).Select(x => new { label = x.Label, val = x.Url }).ToList();

            return Json(response, JsonRequestBehavior.AllowGet);
        }

    }
}