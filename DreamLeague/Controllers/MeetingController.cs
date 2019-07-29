using DreamLeague.DAL;
using DreamLeague.Models;
using DreamLeague.Services;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DreamLeague.Controllers
{
    public class MeetingController : Controller
    {
        readonly IDreamLeagueContext db;
        readonly IMeetingService meetingService;

        public MeetingController()
        {
            this.db = new DreamLeagueContext();
            this.meetingService = new MeetingService(db);
        }

        public MeetingController(IDreamLeagueContext db, IMeetingService meetingService)
        {
            this.db = db;
            this.meetingService = meetingService;
        }

        // GET: Meeting
        public async Task<ActionResult> Index()
        {
            return View(await db.Meetings.AsNoTracking().OrderBy(x => x.Date).ToListAsync());
        }

        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Meeting meeting = await db.Meetings.FindAsync(id);
            if (meeting == null)
            {
                return HttpNotFound();
            }
            return View(meeting);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Meeting/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MeetingId,Date,Location,Longitude,Latitude")] Meeting meeting)
        {
            if (ModelState.IsValid)
            {
                db.Meetings.Add(meeting);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(meeting);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Meeting meeting = await db.Meetings.FindAsync(id);
            if (meeting == null)
            {
                return HttpNotFound();
            }
            return View(meeting);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MeetingId,Date,Location,Longitude,Latitude")] Meeting meeting)
        {
            if (ModelState.IsValid)
            {
                db.SetModified(meeting);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(meeting);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Meeting meeting = await db.Meetings.FindAsync(id);
            if (meeting == null)
            {
                return HttpNotFound();
            }
            return View(meeting);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Meeting meeting = await db.Meetings.FindAsync(id);
            db.Meetings.Remove(meeting);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public ActionResult _NextMeeting()
        {
            return PartialView(meetingService.Next());
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
