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
    public class HistoryController : Controller
    {
        private DreamLeagueContext db = new DreamLeagueContext();

        // GET: History
        public async Task<ActionResult> Index()
        {
            return View(await db.History.AsNoTracking().OrderByDescending(x => x.Year).ToListAsync());
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "HistoryId,Year,Teams,League1,League2,Cup,Plate,LeagueCup")] History history)
        {
            if (ModelState.IsValid)
            {
                db.History.Add(history);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(history);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            History history = await db.History.FindAsync(id);
            if (history == null)
            {
                return HttpNotFound();
            }
            return View(history);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "HistoryId,Year,Teams,League1,League2,Cup,Plate,LeagueCup")] History history)
        {
            if (ModelState.IsValid)
            {
                db.Entry(history).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(history);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            History history = await db.History.FindAsync(id);
            if (history == null)
            {
                return HttpNotFound();
            }
            return View(history);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            History history = await db.History.FindAsync(id);
            db.History.Remove(history);
            await db.SaveChangesAsync();
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
