using DreamLeague.Controllers;
using DreamLeague.Models;
using DreamLeague.Tests.DAL.Mock;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DreamLeague.Tests.Controllers
{
    [TestFixture]
    public class GameWeekControllerTests
    {
        MockDreamLeagueContext context;
        GameWeekController controller;

        [SetUp]
        public void SetUp()
        {
            context = new MockDreamLeagueContext();
            controller = new GameWeekController(context.MockContext.Object);
        }

        [Test]
        public async Task Test_Index_Returns_GameWeeks()
        {
            var result = await controller.Index();

            var model = ((ViewResult)result).Model as List<GameWeek>;

            Assert.AreEqual(2, model.Count);
        }

        [Test]
        public async Task Test_Create_Creates_GameWeek()
        {
            var GameWeek = new GameWeek { Number = 3 };

            await controller.Create(GameWeek);

            context.MockGameWeeks.Verify(x => x.Add(It.Is<GameWeek>(t => t == GameWeek)));
        }

        [Test]
        public async Task Test_Delete_Deletes_GameWeek()
        {
            await controller.DeleteConfirmed(1);

            context.MockGameWeeks.Verify(x => x.Remove(It.Is<GameWeek>(t => t.GameWeekId == 1)));
        }
    }
}
