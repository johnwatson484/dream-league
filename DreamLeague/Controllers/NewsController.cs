using DreamLeague.Models;
using DreamLeague.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace DreamLeague.Controllers
{
    public class NewsController : Controller
    {
        INewsService newsService;

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
            var news = newsService.Get();

            return PartialView(news);
        }
    }
}