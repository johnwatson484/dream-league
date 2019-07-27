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
    public class TeamControllerTests
    {
        MockDreamLeagueContext context;
        TeamController controller;

        [SetUp]
        public void SetUp()
        {
            context = new MockDreamLeagueContext();
            controller = new TeamController(context.MockContext.Object);
        }

        [Test]
        public async Task Test_Index_Returns_Teams()
        {
            var result = await controller.Index();

            var model = ((ViewResult)result).Model as List<Team>;

            Assert.AreEqual(2, model.Count);
        }

        [Test]
        public async Task Test_Details_Returns_Team()
        {
            var result = await controller.Details(1);

            var model = ((ViewResult)result).Model as Team;

            Assert.AreEqual("Newcastle United", model.Name);
        }

        [Test]
        public async Task Test_Create_Creates_Team()
        {
            var team = new Team { Name = "Middlesbrough" };

            await controller.Create(team);

            context.MockTeams.Verify(x => x.Add(It.Is<Team>(t => t == team)));
        }

        [Test]
        public async Task Test_Delete_Deletes_Team()
        {
            await controller.DeleteConfirmed(1);

            context.MockTeams.Verify(x => x.Remove(It.Is<Team>(t => t.TeamId == 1)));
        }
    }
}
