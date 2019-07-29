using DreamLeague.Controllers;
using DreamLeague.Models;
using DreamLeague.Models.Api;
using DreamLeague.Services;
using DreamLeague.Tests.DAL.Mock;
using DreamLeague.ViewModels;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DreamLeague.Tests.Controllers
{
    [TestFixture]
    public class DataControllerTests
    {
        MockDreamLeagueContext context;
        Mock<IGameWeekSerializer<GameWeekSummary>> gameWeekSerializer;
        Mock<IGameWeekService> gameWeekService;
        DataController controller;

        [SetUp]
        public void SetUp()
        {
            context = new MockDreamLeagueContext();
            gameWeekSerializer = new Mock<IGameWeekSerializer<GameWeekSummary>>();
            gameWeekService = new Mock<IGameWeekService>();
            controller = new DataController(context.MockContext.Object, gameWeekSerializer.Object, gameWeekService.Object);
        }

        [Test]
        public void Test_Winners_Returns_Winners()
        {
            var gameWeekSummary1 = new Mock<GameWeekSummary>();
            gameWeekSummary1.Setup(x => x.Winners).Returns(new List<string> { "Billy", "Tommy" });

            var gameWeekSummary2 = new Mock<GameWeekSummary>();
            gameWeekSummary2.Setup(x => x.Winners).Returns(new List<string> { "David" });

            gameWeekSerializer.Setup(x => x.DeSerialize(It.Is<int>(i => i == 1), It.Is<string>(s => s == "GameWeek")))
                .Returns(gameWeekSummary1.Object);

            gameWeekSerializer.Setup(x => x.DeSerialize(It.Is<int>(i => i == 2), It.Is<string>(s => s == "GameWeek")))
                .Returns(gameWeekSummary2.Object);

            var result = controller.Winners();

            Assert.AreEqual(3, result.Count);
            Assert.IsNotNull(result.Where(x=>x.GameWeek == 1 && x.Name == "Billy").FirstOrDefault());
            Assert.IsNotNull(result.Where(x => x.GameWeek == 1 && x.Name == "Tommy").FirstOrDefault());
            Assert.IsNotNull(result.Where(x => x.GameWeek == 2 && x.Name == "David").FirstOrDefault());
        }

        [Test]
        public void Test_Table_Returns_Table()
        {
            
        }

        [Test]
        public void Test_Meetings_Returns_Meetings()
        {
            
        }
    }
}
