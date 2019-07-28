using DreamLeague.Controllers;
using DreamLeague.Models;
using DreamLeague.Services;
using DreamLeague.Tests.DAL.Mock;
using DreamLeague.Tests.Mocks;
using Moq;
using NUnit.Framework;
using PagedList;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DreamLeague.Tests.Controllers
{
    [TestFixture]
    public class GoalControllerTests
    {
        MockDreamLeagueContext context;
        Mock<IGameWeekSummaryService> gameWeekSummaryService;
        Mock<IGameWeekSummaryService> cupWeekSummaryService;
        Mock<IGameWeekService> gameWeekService;
        Mock<IAuditService> auditService;
        GoalController controller;

        [SetUp]
        public void SetUp()
        {
            context = new MockDreamLeagueContext();
            gameWeekSummaryService = new Mock<IGameWeekSummaryService>();
            cupWeekSummaryService = new Mock<IGameWeekSummaryService>();
            gameWeekService = new Mock<IGameWeekService>();
            auditService = new Mock<IAuditService>();
            controller = new GoalController(context.MockContext.Object, gameWeekSummaryService.Object, cupWeekSummaryService.Object, gameWeekService.Object, auditService.Object);
        }

        [Test]
        public void Test_Index_Returns_Goals()
        {
            var result = controller.Index(null);

            var model = ((ViewResult)result).Model as PagedList<Goal>;

            Assert.AreEqual(2, model.Count);
        }

        [Test]
        public async Task Test_Create_Creates_Goal()
        {
            controller.ControllerContext = new MockControllerContext().ControllerContext.Object;

            var goal = new Goal { PlayerId = 1, ManagerId = 1, GameWeekId = 1 };

            await controller.Create(goal);

            context.MockGoals.Verify(x => x.Add(It.Is<Goal>(t => t == goal)));
        }

        [Test]
        public async Task Test_Delete_Deletes_Goal()
        {
            controller.ControllerContext = new MockControllerContext().ControllerContext.Object;

            await controller.DeleteConfirmed(1);

            context.MockGoals.Verify(x => x.Remove(It.Is<Goal>(t => t.GoalId == 1)));
        }
    }
}
