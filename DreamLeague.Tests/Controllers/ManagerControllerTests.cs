using DreamLeague.Controllers;
using DreamLeague.Models;
using DreamLeague.Services;
using DreamLeague.Tests.DAL.Mock;
using DreamLeague.Tests.Mocks;
using DreamLeague.ViewModels;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DreamLeague.Tests.Controllers
{
    [TestFixture]
    public class ManagerControllerTests
    {
        MockDreamLeagueContext context;
        Mock<IGameWeekSerializer<GameWeekSummary>> gameWeekSerializer;
        Mock<IStatisticsService> statisticsService;
        Mock<IEmailService> emailService;
        ManagerController controller;

        [SetUp]
        public void SetUp()
        {
            context = new MockDreamLeagueContext();
            gameWeekSerializer = new Mock<IGameWeekSerializer<GameWeekSummary>>();
            statisticsService = new Mock<IStatisticsService>();
            emailService = new Mock<IEmailService>();
            controller = new ManagerController(context.MockContext.Object, gameWeekSerializer.Object, statisticsService.Object, emailService.Object);
        }

        [Test]
        public async Task Test_Index_Returns_Managers()
        {
            var result = await controller.Index();

            var model = ((ViewResult)result).Model as List<Manager>;

            Assert.AreEqual(2, model.Count);
        }

        [Test]
        public void Test_Details_Returns_Manager()
        {
            controller.ControllerContext = new MockControllerContext().ControllerContext.Object;

            var result = controller.Details(1);

            var model = ((ViewResult)result).Model as ManagerProfile;

            Assert.AreEqual("John", model.Manager.Name);
        }

        [Test]
        public async Task Test_Create_Creates_Manager()
        {
            var manager = new ManagerViewModel { Manager = new Manager { Name = "Scott" } };

            await controller.Create(manager);

            context.MockManagers.Verify(x => x.Add(It.Is<Manager>(t => t == manager.Manager)));
        }

        [Test]
        public async Task Test_Edit_Edits_Manager()
        {
            var manager = new ManagerViewModel { Manager = new Manager { Name = "John" } };

            await controller.Edit(manager);

            context.MockContext.Verify(x => x.SetModified(It.Is<object>(t => t == manager.Manager)));
        }

        [Test]
        public async Task Test_Delete_Deletes_Manager()
        {
            await controller.DeleteConfirmed(1);

            context.MockManagers.Verify(x => x.Remove(It.Is<Manager>(t => t.ManagerId == 1)));
        }
    }
}
