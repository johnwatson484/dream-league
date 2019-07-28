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
    public class ConcedeControllerTests
    {
        MockDreamLeagueContext context;
        Mock<IGameWeekSummaryService> gameWeekSummaryService;
        Mock<IGameWeekSummaryService> cupWeekSummaryService;
        Mock<IGameWeekService> gameWeekService;
        Mock<IAuditService> auditService;
        ConcedeController controller;

        [SetUp]
        public void SetUp()
        {
            context = new MockDreamLeagueContext();
            gameWeekSummaryService = new Mock<IGameWeekSummaryService>();
            cupWeekSummaryService = new Mock<IGameWeekSummaryService>();
            gameWeekService = new Mock<IGameWeekService>();
            auditService = new Mock<IAuditService>();
            controller = new ConcedeController(context.MockContext.Object, gameWeekSummaryService.Object, cupWeekSummaryService.Object, gameWeekService.Object, auditService.Object);
        }

        [Test]
        public void Test_Index_Returns_Concedes()
        {
            var result = controller.Index(null);

            var model = ((ViewResult)result).Model as PagedList<Concede>;

            Assert.AreEqual(2, model.Count);
        }

        [Test]
        public async Task Test_Create_Creates_Concede()
        {
            controller.ControllerContext = new MockControllerContext().ControllerContext.Object;

            var concede = new Concede { TeamId = 1, ManagerId = 1, GameWeekId = 1 };

            await controller.Create(concede);

            context.MockConceded.Verify(x => x.Add(It.Is<Concede>(t => t == concede)));
        }

        [Test]
        public async Task Test_Delete_Deletes_Concede()
        {
            controller.ControllerContext = new MockControllerContext().ControllerContext.Object;

            await controller.DeleteConfirmed(1);

            context.MockConceded.Verify(x => x.Remove(It.Is<Concede>(t => t.ConcedeId == 1)));
        }
    }
}
