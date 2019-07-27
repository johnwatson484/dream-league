using DreamLeague.Controllers;
using DreamLeague.Services;
using Moq;
using NUnit.Framework;
using System.Web.Mvc;

namespace DreamLeague.Tests.Controllers
{
    [TestFixture]
    public class DownloadControllerTests
    {
        Mock<IFileService> fileService;
        DownloadController controller;

        [SetUp]
        public void SetUp()
        {
            fileService = new Mock<IFileService>();
            controller = new DownloadController(fileService.Object);
        }

        [Test]
        public void Test_Index_Returns_Downloads_View()
        {
            var result = controller.Index() as ViewResult;

            Assert.AreEqual(string.Empty, result.ViewName);
        }

        [Test]
        public void Test_Download_Returns_File()
        {
            var result = controller.Download("test.txt") as FileResult;

            Assert.AreEqual("test.txt", result.FileDownloadName);
        }
    }
}
