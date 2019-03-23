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
using DreamLeague.Services;
using PagedList;
using DreamLeague.ViewModels;

namespace DreamLeague.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class GoalController : Controller
    {
        private DreamLeagueContext db;
        IGameWeekService gameWeekService;
        IGameWeekSummaryService gameWeekSummaryService;
        IGameWeekSummaryService cupWeekSummaryService;
        IAuditService auditService;

        public GoalController()
        {
            this.db = new DreamLeagueContext();
            this.gameWeekSummaryService = new GameWeekSummaryService(db, new XMLGameWeekSerializer<GameWeekSummary>());
            this.cupWeekSummaryService = new CupWeekSummaryService(db, new XMLGameWeekSerializer<CupWeekSummary>());
            this.gameWeekService = new GameWeekService(db);
            this.auditService = new AuditService(db);
        }

        public GoalController(DreamLeagueContext db, IGameWeekSummaryService gameWeekSummaryService, IGameWeekSummaryService cupWeekSummaryService, IGameWeekService gameWeekService,
            IAuditService auditService)
        {
            this.db = db;
            this.gameWeekSummaryService = gameWeekSummaryService;
            this.cupWeekSummaryService = cupWeekSummaryService;
            this.gameWeekService = gameWeekService;
            this.auditService = auditService;
        }

        // GET: Goal
        public ActionResult Index(int? page)
        {
            var goals = db.Goals.AsNoTracking().Include(g => g.GameWeek).Include(g => g.Manager).Include(g => g.Player).OrderByDescending(x => x.GameWeek.Number).ThenByDescending(x => x.Player.Team.League.Rank).ThenBy(x => x.Player.LastName);

            int pageSize = 50;
            int pageNumber = (page ?? 1);

            return View(goals.ToPagedList(pageNumber, pageSize));
        }

        // GET: Goal/Create
        public ActionResult Create()
        {
            ViewBag.GameWeekId = new SelectList(db.GameWeeks.AsNoTracking().OrderBy(x => x.Number), "GameWeekId", "Details", gameWeekService.GetCurrent()?.GameWeekId);
            ViewBag.PlayerId = new SelectList(db.ManagerPlayers.AsNoTracking().Include(x => x.Player).Where(x => !x.Substitute).OrderBy(x => x.Player.LastName).ThenBy(x => x.Player.FirstName), "PlayerId", "Details");
            return View();
        }

        // POST: Goal/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "GoalId,PlayerId,GameWeekId")] Goal goal)
        {
            if (ModelState.IsValid)
            {
                var managerId = db.ManagerPlayers.AsNoTracking().Where(x => x.PlayerId == goal.PlayerId).Select(x => x.ManagerId).FirstOrDefault();
                goal.ManagerId = managerId;

                db.Goals.Add(goal);
                var player = db.Players.Where(x => x.PlayerId == goal.PlayerId).FirstOrDefault();

                if (!goal.Cup)
                {
                    auditService.Log("Goal", "Goal Added", User.Identity.Name, string.Format("Goal added for {0} ({1})", player?.FullName, player?.ManagerPlayers?.FirstOrDefault()?.Manager?.Name ?? "Unattached"), goal.GameWeekId);
                }
                else
                {
                    auditService.Log("Goal", "Goal Added", User.Identity.Name, string.Format("Cup goal added for {0} ({1})", player?.FullName, player?.ManagerPlayers?.FirstOrDefault()?.Manager?.Name ?? "Unattached"), goal.GameWeekId);
                }
                await db.SaveChangesAsync();

                gameWeekSummaryService.Create(goal.GameWeekId);
                cupWeekSummaryService.Create(goal.GameWeekId);

                return RedirectToAction("Index");
            }

            ViewBag.GameWeekId = new SelectList(db.GameWeeks.AsNoTracking().OrderBy(x => x.Number), "GameWeekId", "Details", gameWeekService.GetCurrent()?.GameWeekId);
            ViewBag.PlayerId = new SelectList(db.ManagerPlayers.AsNoTracking().Include(x => x.Player).Where(x => !x.Substitute).OrderBy(x => x.Player.LastName).ThenBy(x => x.Player.FirstName), "PlayerId", "Details");
            return View(goal);
        }

        // GET: Goal/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Goal goal = await db.Goals.FindAsync(id);
            if (goal == null)
            {
                return HttpNotFound();
            }
            ViewBag.GameWeekId = new SelectList(db.GameWeeks.AsNoTracking().OrderBy(x => x.Number), "GameWeekId", "Details", gameWeekService.GetCurrent()?.GameWeekId);
            ViewBag.PlayerId = new SelectList(db.ManagerPlayers.AsNoTracking().Include(x => x.Player).Where(x => !x.Substitute).OrderBy(x => x.Player.LastName).ThenBy(x => x.Player.FirstName), "PlayerId", "Details");
            return View(goal);
        }

        // POST: Goal/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "GoalId,PlayerId,GameWeekId")] Goal goal)
        {
            if (ModelState.IsValid)
            {
                var managerId = db.ManagerPlayers.AsNoTracking().Where(x => x.PlayerId == goal.PlayerId).Select(x => x.ManagerId).FirstOrDefault();
                goal.ManagerId = managerId;

                db.Entry(goal).State = EntityState.Modified;
                var player = db.Players.Where(x => x.PlayerId == goal.PlayerId).FirstOrDefault();

                if (!goal.Cup)
                {
                    auditService.Log("Goal", "Goal Updated", User.Identity.Name, string.Format("Goal updated for {0} ({1})", player?.FullName, player?.ManagerPlayers?.FirstOrDefault()?.Manager?.Name ?? "Unattached"), goal.GameWeekId);
                }
                else
                {
                    auditService.Log("Goal", "Goal Updated", User.Identity.Name, string.Format("Cup goal updated for {0} ({1})", player?.FullName, player?.ManagerPlayers?.FirstOrDefault()?.Manager?.Name ?? "Unattached"), goal.GameWeekId);
                }
                await db.SaveChangesAsync();

                gameWeekSummaryService.Create(goal.GameWeekId);
                cupWeekSummaryService.Create(goal.GameWeekId);

                return RedirectToAction("Index");
            }
            ViewBag.GameWeekId = new SelectList(db.GameWeeks.AsNoTracking().OrderBy(x => x.Number), "GameWeekId", "Details", gameWeekService.GetCurrent()?.GameWeekId);
            ViewBag.PlayerId = new SelectList(db.ManagerPlayers.AsNoTracking().Include(x => x.Player).Where(x => !x.Substitute).OrderBy(x => x.Player.LastName).ThenBy(x => x.Player.FirstName), "PlayerId", "Details");
            return View(goal);
        }

        // GET: Goal/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Goal goal = await db.Goals.FindAsync(id);
            if (goal == null)
            {
                return HttpNotFound();
            }
            return View(goal);
        }

        // POST: Goal/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Goal goal = await db.Goals.FindAsync(id);
            db.Goals.Remove(goal);
            var player = db.Players.Where(x => x.PlayerId == goal.PlayerId).FirstOrDefault();

            if (!goal.Cup)
            {
                auditService.Log("Goal", "Goal Deleted", User.Identity.Name, string.Format("Goal removed for {0} ({1})", player?.FullName, player?.ManagerPlayers?.FirstOrDefault()?.Manager?.Name ?? "Unattached"), goal.GameWeekId);
            }
            else
            {
                auditService.Log("Goal", "Goal Deleted", User.Identity.Name, string.Format("Cup goal removed for {0} ({1})", player?.FullName, player?.ManagerPlayers?.FirstOrDefault()?.Manager?.Name ?? "Unattached"), goal.GameWeekId);
            }
            await db.SaveChangesAsync();

            gameWeekSummaryService.Create(goal.GameWeekId);
            cupWeekSummaryService.Create(goal.GameWeekId);

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
