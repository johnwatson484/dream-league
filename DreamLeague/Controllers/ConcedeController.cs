using DreamLeague.DAL;
using DreamLeague.Models;
using DreamLeague.Services;
using DreamLeague.ViewModels;
using PagedList;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DreamLeague.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ConcedeController : Controller
    {
        private DreamLeagueContext db;
        IGameWeekSummaryService gameWeekSummaryService;
        IGameWeekSummaryService cupWeekSummaryService;
        IGameWeekService gameWeekService;
        IAuditService auditService;

        public ConcedeController()
        {
            this.db = new DreamLeagueContext();
            this.gameWeekSummaryService = new GameWeekSummaryService(db, new XMLGameWeekSerializer<GameWeekSummary>());
            this.cupWeekSummaryService = new CupWeekSummaryService(db, new XMLGameWeekSerializer<CupWeekSummary>());
            this.gameWeekService = new GameWeekService(db);
            this.auditService = new AuditService(db);
        }
        public ConcedeController(DreamLeagueContext db, IGameWeekSummaryService gameWeekSummaryService, IGameWeekSummaryService cupWeekSummaryService, IGameWeekService gameWeekService, IAuditService auditService)
        {
            this.db = db;
            this.gameWeekSummaryService = gameWeekSummaryService;
            this.cupWeekSummaryService = cupWeekSummaryService;
            this.gameWeekService = gameWeekService;
            this.auditService = auditService;
        }

        // GET: Concede
        public ActionResult Index(int? page)
        {
            var conceded = db.Conceded.AsNoTracking()
                .Include(c => c.GameWeek)
                .Include(c => c.Manager)
                .Include(c => c.Team)
                .OrderByDescending(x => x.GameWeek.Number)
                .ThenByDescending(x => x.Team.League.Rank)
                .ThenBy(x => x.Team.Name);

            return View(conceded.ToPagedList((page ?? 1), 50));
        }

        // GET: Concede/Create
        public ActionResult Create()
        {
            ViewBag.GameWeekId = new SelectList(db.GameWeeks.AsNoTracking().OrderBy(x => x.Number), "GameWeekId", "Details", gameWeekService.GetCurrent()?.GameWeekId);
            ViewBag.TeamId = new SelectList(db.ManagerGoalKeepers.AsNoTracking().Include(x => x.Team).OrderBy(x => x.Team.Name), "TeamId", "Details");
            return View();
        }

        // POST: Concede/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ConcedeId,TeamId,GameWeekId,ManagerId,Substitute")] Concede concede)
        {
            if (ModelState.IsValid)
            {
                var managerGoalKeeper = db.ManagerGoalKeepers.AsNoTracking().Where(x => x.TeamId == concede.TeamId).FirstOrDefault();
                if (managerGoalKeeper != null)
                {
                    concede.ManagerId = managerGoalKeeper.ManagerId;
                    concede.Substitute = managerGoalKeeper.Substitute;
                }

                db.Conceded.Add(concede);

                var team = db.Teams.Where(x => x.TeamId == concede.TeamId).FirstOrDefault();
                auditService.Log("Concede", "Concede Added", User.Identity.Name, string.Format("{0} conceded for {1} ({2})", !concede.Cup ? "Goal" : "Cup goal", team?.Name, team?.ManagerGoalKeepers?.FirstOrDefault()?.Manager?.Name ?? "Unattached"), concede.GameWeekId);

                await db.SaveChangesAsync();

                gameWeekSummaryService.Create(concede.GameWeekId);
                cupWeekSummaryService.Create(concede.GameWeekId);

                return RedirectToAction("Index");
            }

            ViewBag.GameWeekId = new SelectList(db.GameWeeks.AsNoTracking().OrderBy(x => x.Number), "GameWeekId", "Details", gameWeekService.GetCurrent()?.GameWeekId);
            ViewBag.TeamId = new SelectList(db.ManagerGoalKeepers.AsNoTracking().Include(x => x.Team).OrderBy(x => x.Team.Name), "TeamId", "Details");
            return View(concede);
        }

        // GET: Concede/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Concede concede = await db.Conceded.FindAsync(id);
            if (concede == null)
            {
                return HttpNotFound();
            }
            ViewBag.GameWeekId = new SelectList(db.GameWeeks.AsNoTracking().OrderBy(x => x.Number), "GameWeekId", "Details", gameWeekService.GetCurrent()?.GameWeekId);
            ViewBag.TeamId = new SelectList(db.ManagerGoalKeepers.AsNoTracking().Include(x => x.Team).OrderBy(x => x.Team.Name), "TeamId", "Details");
            return View(concede);
        }

        // POST: Concede/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ConcedeId,TeamId,GameWeekId,ManagerId,Substitute")] Concede concede)
        {
            if (ModelState.IsValid)
            {
                var managerGoalKeeper = db.ManagerGoalKeepers.AsNoTracking().Where(x => x.TeamId == concede.TeamId).FirstOrDefault();
                if (managerGoalKeeper != null)
                {
                    concede.ManagerId = managerGoalKeeper.ManagerId;
                    concede.Substitute = managerGoalKeeper.Substitute;
                }

                db.Entry(concede).State = EntityState.Modified;

                var team = db.Teams.Where(x => x.TeamId == concede.TeamId).FirstOrDefault();
                auditService.Log("Concede", "Concede Updated", User.Identity.Name, string.Format("{0} conceded updated for {1} ({2})", !concede.Cup ? "Goal" : "Cup goal", team?.Name, team?.ManagerGoalKeepers?.FirstOrDefault()?.Manager?.Name ?? "Unattached"), concede.GameWeekId);
                await db.SaveChangesAsync();

                gameWeekSummaryService.Create(concede.GameWeekId);
                cupWeekSummaryService.Create(concede.GameWeekId);

                return RedirectToAction("Index");
            }
            ViewBag.GameWeekId = new SelectList(db.GameWeeks.AsNoTracking().OrderBy(x => x.Number), "GameWeekId", "Details", gameWeekService.GetCurrent()?.GameWeekId);
            ViewBag.TeamId = new SelectList(db.ManagerGoalKeepers.AsNoTracking().Include(x => x.Team).OrderBy(x => x.Team.Name), "TeamId", "Details");
            return View(concede);
        }

        // GET: Concede/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Concede concede = await db.Conceded.FindAsync(id);
            if (concede == null)
            {
                return HttpNotFound();
            }
            return View(concede);
        }

        // POST: Concede/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Concede concede = await db.Conceded.FindAsync(id);
            db.Conceded.Remove(concede);

            var team = db.Teams.Where(x => x.TeamId == concede.TeamId).FirstOrDefault();
            auditService.Log("Concede", "Concede Deleted", User.Identity.Name, string.Format("{0} conceded removed for {1} ({2})", !concede.Cup ? "Goal" : "Cup goal", team?.Name, team?.ManagerGoalKeepers?.FirstOrDefault()?.Manager?.Name ?? "Unattached"), concede.GameWeekId);
            await db.SaveChangesAsync();

            gameWeekSummaryService.Create(concede.GameWeekId);
            cupWeekSummaryService.Create(concede.GameWeekId);

            return RedirectToAction("Index");
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
