using DreamLeague.DAL;
using DreamLeague.Models;
using DreamLeague.ViewModels;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DreamLeague.Controllers
{
    public class GroupController : Controller
    {
        private DreamLeagueContext db;

        public GroupController()
        {
            this.db = new DreamLeagueContext();
        }

        public GroupController(DreamLeagueContext db)
        {
            this.db = db;
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = await db.Groups.FindAsync(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Create(int cupId)
        {
            var managers = db.Managers.AsNoTracking().OrderBy(x => x.Name).ToList();
            var cup = db.Cups.Find(cupId);

            return View(new GroupViewModel(cup, managers));
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(GroupViewModel groupViewModel)
        {
            if (ModelState.IsValid)
            {
                foreach (var manager in groupViewModel.Managers.Where(x => x.Selected))
                {
                    db.Managers.Attach(manager.Manager);
                    groupViewModel.Group.Managers.Add(manager.Manager);
                }

                db.Groups.Add(groupViewModel.Group);
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "Cup", new { id = groupViewModel.Cup.CupId });
            }

            return View(groupViewModel);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = await db.Groups.AsNoTracking().Include(x => x.Managers).Where(x => x.GroupId == id).FirstOrDefaultAsync();
            if (group == null)
            {
                return HttpNotFound();
            }

            var managers = db.Managers.AsNoTracking().OrderBy(x => x.Name).ToList();
            var cup = db.Cups.Find(group.CupId);

            GroupViewModel groupViewModel = new GroupViewModel(cup, managers, group);

            return View(groupViewModel);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(GroupViewModel groupViewModel)
        {
            if (ModelState.IsValid)
            {
                Group existing = db.Groups.Include(x => x.Managers).Where(x => x.GroupId == groupViewModel.Group.GroupId).FirstOrDefault();

                foreach (var manager in db.Managers)
                {
                    if (groupViewModel.Managers.Exists(x => x.Manager.ManagerId == manager.ManagerId && x.Selected))
                    {
                        if (!existing.Managers.Exists(x => x.ManagerId == manager.ManagerId))
                        {
                            existing.Managers.Add(manager);
                        }
                    }
                    else if (existing.Managers.Exists(x => x.ManagerId == manager.ManagerId))
                    {
                        existing.Managers.Remove(manager);
                    }
                }

                existing.Name = groupViewModel.Group.Name;
                existing.GroupLegs = groupViewModel.Group.GroupLegs;
                existing.TeamsAdvancing = groupViewModel.Group.TeamsAdvancing;

                db.Entry(existing).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "Cup", new { id = groupViewModel.Cup.CupId });
            }
            return View(groupViewModel);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = await db.Groups.FindAsync(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Group group = await db.Groups.FindAsync(id);
            db.Groups.Remove(group);
            await db.SaveChangesAsync();
            return RedirectToAction("Details", "Cup", new { id = group.CupId });
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
