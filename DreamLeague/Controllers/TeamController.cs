using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DreamLeague.DAL;
using DreamLeague.Models;

namespace DreamLeague.Controllers
{
    public class TeamController : Controller
    {
        private DreamLeagueContext db;

        public TeamController()
        {
            this.db = new DreamLeagueContext();
        }

        public TeamController(DreamLeagueContext db)
        {
            this.db = db;
        }

        // GET: Team
        public async Task<ActionResult> Index()
        {
            var teams = db.Teams.AsNoTracking()
                .Include(t => t.League)
                .OrderBy(x=>x.League.Rank)
                .ThenBy(x=>x.Name);

            return View(await teams.ToListAsync());
        }

        // GET: Team/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = await db.Teams.AsNoTracking().Include(x => x.Conceded).Include(x => x.ManagerGoalKeepers.Select(m => m.Manager)).Where(x => x.TeamId == id).FirstOrDefaultAsync();
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            ViewBag.LeagueId = new SelectList(db.Leagues.AsNoTracking(), "LeagueId", "Name");
            return View();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "TeamId,LeagueId,Name,Alias")] Team team)
        {
            if (ModelState.IsValid)
            {
                db.Teams.Add(team);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.LeagueId = new SelectList(db.Leagues, "LeagueId", "Name", team.LeagueId);
            return View(team);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = await db.Teams.FindAsync(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            ViewBag.LeagueId = new SelectList(db.Leagues, "LeagueId", "Name", team.LeagueId);
            return View(team);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "TeamId,LeagueId,Name,Alias")] Team team)
        {
            if (ModelState.IsValid)
            {
                db.Entry(team).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.LeagueId = new SelectList(db.Leagues, "LeagueId", "Name", team.LeagueId);
            return View(team);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = await db.Teams.FindAsync(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Team team = await db.Teams.FindAsync(id);
            db.Teams.Remove(team);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult AutoComplete(string prefix)
        {            
            return Json(db.Teams.AsNoTracking().Where(x => x.Name.StartsWith(prefix)).Select(x => new { label = x.Name, val = x.TeamId }).ToList(), JsonRequestBehavior.AllowGet);
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
