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
    public class FixtureController : Controller
    {
        private readonly DreamLeagueContext db = new DreamLeagueContext();

        // GET: Fixture
        public async Task<ActionResult> Index()
        {
            return View(await db.Fixtures.AsNoTracking().Include(f => f.AwayManager).Include(f => f.Cup).Include(f => f.GameWeek).Include(f => f.HomeManager).ToListAsync());
        }

        // GET: Fixture/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fixture fixture = await db.Fixtures.FindAsync(id);
            if (fixture == null)
            {
                return HttpNotFound();
            }
            return View(fixture);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Create(int cupId)
        {
            var managers = db.Managers.AsNoTracking().OrderBy(x => x.Name);

            ViewBag.AwayManagerId = new SelectList(managers, "ManagerId", "Name");
            ViewBag.GameWeekId = new SelectList(db.GameWeeks.AsNoTracking().OrderBy(x => x.Number), "GameWeekId", "Details");
            ViewBag.HomeManagerId = new SelectList(managers, "ManagerId", "Name");

            int? round = db.Fixtures.AsNoTracking().Where(x => x.CupId == cupId).Max(x => (int?)x.Round);

            return View(new Fixture(cupId, round));
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "FixtureId,CupId,GameWeekId,HomeManagerId,AwayManagerId,Round")] Fixture fixture)
        {
            if (ModelState.IsValid)
            {
                db.Fixtures.Add(fixture);
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "Cup", new { id = fixture.CupId });
            }

            var managers = db.Managers.AsNoTracking().OrderBy(x => x.Name);

            ViewBag.AwayManagerId = new SelectList(managers, "ManagerId", "Name", fixture.AwayManagerId);
            ViewBag.GameWeekId = new SelectList(db.GameWeeks.AsNoTracking().OrderBy(x => x.Number), "GameWeekId", "Details", fixture.GameWeekId);
            ViewBag.HomeManagerId = new SelectList(managers, "ManagerId", "Name", fixture.HomeManagerId);

            return View(fixture);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fixture fixture = await db.Fixtures.FindAsync(id);
            if (fixture == null)
            {
                return HttpNotFound();
            }

            var managers = db.Managers.AsNoTracking().OrderBy(x => x.Name);

            ViewBag.AwayManagerId = new SelectList(managers, "ManagerId", "Name", fixture.AwayManagerId);
            ViewBag.GameWeekId = new SelectList(db.GameWeeks.AsNoTracking().OrderBy(x => x.Number), "GameWeekId", "Details", fixture.GameWeekId);
            ViewBag.HomeManagerId = new SelectList(managers, "ManagerId", "Name", fixture.HomeManagerId);

            return View(fixture);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "FixtureId,CupId,GameWeekId,HomeManagerId,AwayManagerId,Round")] Fixture fixture)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fixture).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "Cup", new { id = fixture.CupId });
            }

            var managers = db.Managers.AsNoTracking().OrderBy(x => x.Name);

            ViewBag.AwayManagerId = new SelectList(managers, "ManagerId", "Name", fixture.AwayManagerId);
            ViewBag.GameWeekId = new SelectList(db.GameWeeks.AsNoTracking().OrderBy(x => x.Number), "GameWeekId", "Details", fixture.GameWeekId);
            ViewBag.HomeManagerId = new SelectList(managers, "ManagerId", "Name", fixture.HomeManagerId);
            return View(fixture);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fixture fixture = await db.Fixtures.FindAsync(id);
            if (fixture == null)
            {
                return HttpNotFound();
            }
            return View(fixture);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Fixture fixture = await db.Fixtures.FindAsync(id);
            db.Fixtures.Remove(fixture);
            await db.SaveChangesAsync();
            return RedirectToAction("Details", "Cup", new { id = fixture.CupId });
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
