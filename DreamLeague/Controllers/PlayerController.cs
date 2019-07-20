using DreamLeague.DAL;
using DreamLeague.Models;
using PagedList;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DreamLeague.Controllers
{
    public class PlayerController : Controller
    {
        private DreamLeagueContext db;

        public PlayerController()
        {
            this.db = new DreamLeagueContext();
        }

        public PlayerController(DreamLeagueContext db)
        {
            this.db = db;
        }

        // GET: Player
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, string searchPosition, int? page)
        {
            ViewBag.CurrentSort = sortOrder;

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            ViewBag.SearchPosition = searchPosition;

            var players = db.Players.AsNoTracking().Include(p => p.Team).Include(p => p.ManagerPlayers.Select(x => x.Manager));

            if (!string.IsNullOrEmpty(searchString))
            {
                players = players.Where(s => s.LastName.ToUpper().Contains(searchString.ToUpper())
                                       || s.FirstName.ToUpper().Contains(searchString.ToUpper())
                                       || s.Team.Name.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (searchPosition)
            {
                case "Defenders":
                    players = players.Where(x => x.Position == Position.Defender);
                    break;
                case "Midfielders":
                    players = players.Where(x => x.Position == Position.Midfielder);
                    break;
                case "Forwards":
                    players = players.Where(x => x.Position == Position.Forward);
                    break;
                default:
                    break;
            }

            switch (sortOrder)
            {
                case "Name":
                    players = players.OrderBy(x => x.LastName).ThenBy(x => x.FirstName);
                    break;
                case "Team":
                    players = players.OrderBy(x => x.Team.Name).ThenBy(x => x.Position).ThenBy(x => x.LastName).ThenBy(x => x.FirstName);
                    break;
                case "Position":
                    players = players.OrderBy(x => x.Position).ThenBy(x => x.LastName).ThenBy(x => x.FirstName);
                    break;
                default:
                    players = players.OrderBy(x => x.Team.Name).ThenBy(x => x.Position).ThenBy(x => x.LastName).ThenBy(x => x.FirstName);
                    break;
            }

            int pageSize = 50;
            int pageNumber = (page ?? 1);

            return View(players.ToPagedList(pageNumber, pageSize));
        }

        // GET: Player/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Player player = await db.Players.AsNoTracking().Include(x => x.Goals).Include(x => x.ManagerPlayers.Select(m => m.Manager)).Where(x => x.PlayerId == id).FirstOrDefaultAsync();
            if (player == null)
            {
                return HttpNotFound();
            }
            return View(player);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            ViewBag.TeamId = new SelectList(db.Teams.AsNoTracking().OrderBy(x => x.Name), "TeamId", "Name");
            return View();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "PlayerId,TeamId,FirstName,LastName,Position")] Player player)
        {
            if (ModelState.IsValid)
            {
                db.Players.Add(player);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.TeamId = new SelectList(db.Teams.AsNoTracking().OrderBy(x => x.Name), "TeamId", "Name", player.TeamId);
            return View(player);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Player player = await db.Players.FindAsync(id);
            if (player == null)
            {
                return HttpNotFound();
            }
            ViewBag.TeamId = new SelectList(db.Teams.AsNoTracking().OrderBy(x => x.Name), "TeamId", "Name", player.TeamId);
            return View(player);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "PlayerId,TeamId,FirstName,LastName,Position")] Player player)
        {
            if (ModelState.IsValid)
            {
                db.Entry(player).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.TeamId = new SelectList(db.Teams.AsNoTracking().OrderBy(x => x.Name), "TeamId", "Name", player.TeamId);
            return View(player);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Player player = await db.Players.FindAsync(id);
            if (player == null)
            {
                return HttpNotFound();
            }
            return View(player);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Player player = await db.Players.FindAsync(id);
            db.Players.Remove(player);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult AutoComplete(string prefix)
        {
            var players = db.Players.AsNoTracking().Include(x => x.Team).Where(x => x.LastName.StartsWith(prefix)).ToList();

            var response = players.Select(x => new { label = x.Details, val = x.PlayerId }).ToList();

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
