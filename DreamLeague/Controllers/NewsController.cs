using DreamLeague.Services;
using System.Web.Mvc;

namespace DreamLeague.Controllers
{
    public class NewsController : Controller
    {
        readonly INewsService newsService;

        public NewsController()
        {
            this.newsService = new SkySportsNewsService();
        }

        public NewsController(INewsService newsService)
        {
            this.newsService = newsService;
        }

        [OutputCache(Duration = 3600, VaryByParam = "none")]
        public ActionResult _News()
        {
            return PartialView(newsService.Get());
        }
    }
}