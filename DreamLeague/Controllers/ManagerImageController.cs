using DreamLeague.DAL;
using DreamLeague.Models;
using System.Web;
using System.Web.Mvc;

namespace DreamLeague.Controllers
{
    [Authorize(Roles = "User")]
    public class ManagerImageController : Controller
    {
        DreamLeagueContext db;

        public ManagerImageController()
        {
            this.db = new DreamLeagueContext();
        }

        public ManagerImageController(DreamLeagueContext db)
        {
            this.db = db;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(HttpPostedFileBase file, int managerId)
        {
            if (file != null)
            {
                var image = new byte[file.ContentLength];
                file.InputStream.Read(image, 0, image.Length);

                Manager manager = db.Managers.Find(managerId);
                manager.SetImage(image);

                db.SaveChanges();
            }

            return RedirectToAction("Details", "Manager", new { id = managerId });
        }

        [HttpPost]
        public ActionResult Delete(int managerId)
        {
            var image = db.ManagerImages.Find(managerId);

            db.ManagerImages.Remove(image);
            db.SaveChanges();

            return Content("Ok");
        }
    }
}