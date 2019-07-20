using DreamLeague.DAL;
using DreamLeague.Models;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DreamLeague.Controllers
{
    public class GameWeekController : Controller
    {
        private DreamLeagueContext db;

        public GameWeekController()
        {
            this.db = new DreamLeagueContext();
        }

        public GameWeekController(DreamLeagueContext db)
        {
            this.db = db;
        }

        // GET: GameWeek
        public async Task<ActionResult> Index()
        {
            return View(await db.GameWeeks.AsNoTracking().OrderBy(x => x.Number).ToListAsync());
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "GameWeekId,Number,Start,Complete")] GameWeek gameWeek)
        {
            if (ModelState.IsValid)
            {
                db.GameWeeks.Add(gameWeek);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(gameWeek);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GameWeek gameWeek = await db.GameWeeks.FindAsync(id);
            if (gameWeek == null)
            {
                return HttpNotFound();
            }
            return View(gameWeek);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "GameWeekId,Number,Start,Complete")] GameWeek gameWeek)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gameWeek).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(gameWeek);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GameWeek gameWeek = await db.GameWeeks.FindAsync(id);
            if (gameWeek == null)
            {
                return HttpNotFound();
            }
            return View(gameWeek);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            GameWeek gameWeek = await db.GameWeeks.FindAsync(id);
            db.GameWeeks.Remove(gameWeek);
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
