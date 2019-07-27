using DreamLeague.Controllers;
using DreamLeague.Services;
using DreamLeague.Tests.DAL.Mock;
using DreamLeague.ViewModels;
using Moq;
using NUnit.Framework;
using System.Web.Mvc;

namespace DreamLeague.Tests.Controllers
{
    [TestFixture]
    public class HomeControllerTests
    {
        MockDreamLeagueContext context;
        Mock<IGameWeekSerializer<GameWeekSummary>> gameWeekSerializer;
        Mock<ISearchService> searchService;
        HomeController controller;

        [SetUp]
        public void SetUp()
        {
            context = new MockDreamLeagueContext();
            gameWeekSerializer = new Mock<IGameWeekSerializer<GameWeekSummary>>();
            searchService = new Mock<ISearchService>();
            controller = new HomeController(context.MockContext.Object, gameWeekSerializer.Object, searchService.Object);
        }

        [Test]
        public void Test_Index_Returns_Home_View()
        {
            var result = controller.About() as ViewResult;

            Assert.AreEqual(string.Empty, result.ViewName);
        }

        [Test]
        public void Test_About_Returns_About_View()
        {
            var result = controller.About() as ViewResult;

            Assert.AreEqual(string.Empty, result.ViewName);
        }
    }
}
