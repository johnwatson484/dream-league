using DreamLeague.DAL;
using DreamLeague.Models;
using DreamLeague.Services;
using DreamLeague.ViewModels;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DreamLeague.Controllers
{
    public class ManagerController : Controller
    {
        readonly IDreamLeagueContext db;
        readonly IGameWeekSerializer<GameWeekSummary> gameWeekSerializer;
        readonly IStatisticsService statisticsService;
        readonly IEmailService emailService;

        public ManagerController()
        {
            this.db = new DreamLeagueContext();
            this.gameWeekSerializer = new XMLGameWeekSerializer<GameWeekSummary>();
            this.statisticsService = new StatisticsService(db);
            this.emailService = new Services.EmailService(db);
        }

        public ManagerController(IDreamLeagueContext db, IGameWeekSerializer<GameWeekSummary> gameWeekSerializer, IStatisticsService statisticsService, IEmailService emailService)
        {
            this.db = db;
            this.gameWeekSerializer = gameWeekSerializer;
            this.statisticsService = statisticsService;
            this.emailService = emailService;
        }

        // GET: Manager
        public async Task<ActionResult> Index()
        {
            ViewBag.Emails = emailService.GetAll();

            return View(await db.Managers.AsNoTracking().Include(x => x.Image).OrderBy(x => x.Name).ToListAsync());
        }

        // GET: Manager/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Manager manager = db.Managers
                .AsNoTracking()
                .Include(x => x.Emails)
                .Include(x => x.GoalKeepers.Select(t => t.Team))
                .Include(x => x.Players.Select(p => p.Player))
                .Include(x => x.Image)
                .Where(x => x.ManagerId == id)
                .FirstOrDefault();

            if (User.IsInRole("User") && manager == null)
            {
                return RedirectToAction("Index", "Manage");
            }

            var gameWeekId = db.GameWeeks.AsNoTracking().Where(x => x.Complete).OrderByDescending(x => x.Number).Select(x => x.GameWeekId).FirstOrDefault();
            GameWeekSummary gameWeekSummary = gameWeekSerializer.DeSerialize(gameWeekId, "GameWeek");
            Form form = statisticsService.GetForm(id.Value, 40);

            ViewBag.AverageGoals = statisticsService.GetAverageGoals(id.Value);
            ViewBag.AverageConceded = statisticsService.GetAverageConceded(id.Value);

            return View(new ManagerProfile(manager, gameWeekSummary, form));
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ManagerViewModel managerViewModel)
        {
            if (ModelState.IsValid)
            {
                Manager manager = managerViewModel.Manager;

                if (!string.IsNullOrEmpty(managerViewModel.Email1.Address))
                {
                    manager.AddEmail(managerViewModel.Email1);
                }
                if (!string.IsNullOrEmpty(managerViewModel.Email2.Address))
                {
                    manager.AddEmail(managerViewModel.Email2);
                }

                db.Managers.Add(manager);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(managerViewModel);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manager manager = await db.Managers.AsNoTracking().Include(x => x.Emails).Where(x => x.ManagerId == id).FirstOrDefaultAsync();
            if (manager == null)
            {
                return HttpNotFound();
            }

            ManagerViewModel managerViewModel = new ManagerViewModel(manager);

            return View(managerViewModel);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ManagerViewModel managerViewModel)
        {
            ModelState.Remove("Email1.EmailId");
            ModelState.Remove("Email2.EmailId");
            ModelState.Remove("Email1.ManagerId");
            ModelState.Remove("Email2.ManagerId");

            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(managerViewModel.Email1.Address))
                {
                    if (managerViewModel.Email1.EmailId == 0)
                    {
                        managerViewModel.Email1.ManagerId = managerViewModel.Manager.ManagerId;
                        db.Emails.Add(managerViewModel.Email1);
                    }
                    else
                    {
                        db.SetModified(managerViewModel.Email1);
                    }
                }
                if (!string.IsNullOrEmpty(managerViewModel.Email2.Address))
                {
                    if (managerViewModel.Email2.EmailId == 0)
                    {
                        managerViewModel.Email2.ManagerId = managerViewModel.Manager.ManagerId;
                        db.Emails.Add(managerViewModel.Email2);
                    }
                    else
                    {
                        db.SetModified(managerViewModel.Email2);
                    }
                }

                if (string.IsNullOrEmpty(managerViewModel.Email1.Address) && managerViewModel.Email1.EmailId > 0)
                {
                    var email = new Email { EmailId = managerViewModel.Email1.EmailId };
                    db.SetModified(email);
                }
                if (string.IsNullOrEmpty(managerViewModel.Email2.Address) && managerViewModel.Email2.EmailId > 0)
                {
                    var email = new Email { EmailId = managerViewModel.Email2.EmailId };
                    db.SetModified(email);
                }

                db.SetModified(managerViewModel.Manager);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(managerViewModel);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manager manager = await db.Managers.FindAsync(id);
            if (manager == null)
            {
                return HttpNotFound();
            }
            return View(manager);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Manager manager = await db.Managers.FindAsync(id);
            db.Managers.Remove(manager);
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
