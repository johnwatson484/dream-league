using DreamLeague.DAL;
using DreamLeague.Models;
using DreamLeague.Services;
using DreamLeague.ViewModels;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DreamLeague.Controllers
{
    public class CupController : Controller
    {
        private DreamLeagueContext db;
        ICupService cupService;

        public CupController()
        {
            this.db = new DAL.DreamLeagueContext();
            this.cupService = new CupService(db, new XMLGameWeekSerializer<CupWeekSummary>());
        }

        public CupController(DreamLeagueContext db, ICupService cupService)
        {
            this.db = db;
            this.cupService = cupService;
        }

        public async Task<ActionResult> Index()
        {
            return View(await db.Cups.AsNoTracking().ToListAsync());
        }

        // GET: Cup/Details/5
        public ActionResult Details(int id)
        {
            CupViewModel cupViewModel = cupService.GetData(id);

            return View(cupViewModel);
        }

        [Authorize(Roles = ("Administrator"))]
        public ActionResult Create()
        {
            return View(new Cup());
        }

        [Authorize(Roles = ("Administrator"))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CupId,Name,HasGroupStage,KnockoutLegs,FinalLegs")] Cup cup)
        {
            if (ModelState.IsValid)
            {
                db.Cups.Add(cup);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(cup);
        }

        [Authorize(Roles = ("Administrator"))]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cup cup = await db.Cups.FindAsync(id);
            if (cup == null)
            {
                return HttpNotFound();
            }
            return View(cup);
        }

        [Authorize(Roles = ("Administrator"))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CupId,Name,HasGroupStage,KnockoutLegs,FinalLegs")] Cup cup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cup).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(cup);
        }

        [Authorize(Roles = ("Administrator"))]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cup cup = await db.Cups.FindAsync(id);
            if (cup == null)
            {
                return HttpNotFound();
            }
            return View(cup);
        }

        [Authorize(Roles = ("Administrator"))]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Cup cup = await db.Cups.FindAsync(id);
            db.Cups.Remove(cup);
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
