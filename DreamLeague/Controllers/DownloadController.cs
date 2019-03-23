using DreamLeague.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DreamLeague.Controllers
{
    public class DownloadController : Controller
    {
        IFileService fileService;

        public DownloadController()
        {
            this.fileService = new FileService();
        }

        public DownloadController(IFileService fileService)
        {
            this.fileService = fileService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public FileResult Download(string fileName)
        {
            var fileBytes = fileService.GetBytesFromFile(fileName);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
    }
}