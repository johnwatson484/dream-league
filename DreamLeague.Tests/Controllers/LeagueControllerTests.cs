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
    public class LeagueControllerTests
    {
        MockDreamLeagueContext context;
        LeagueController controller;

        [SetUp]
        public void SetUp()
        {
            context = new MockDreamLeagueContext();
            controller = new LeagueController(context.MockContext.Object);
        }

        [Test]
        public async Task Test_Index_Returns_Leagues()
        {
            var result = await controller.Index();

            var model = ((ViewResult)result).Model as List<League>;

            Assert.AreEqual(3, model.Count);
        }

        [Test]
        public async Task Test_Details_Returns_League()
        {
            var result = await controller.Details(1);

            var model = ((ViewResult)result).Model as League;

            Assert.AreEqual("Championship", model.Name);
        }

        [Test]
        public async Task Test_Create_Creates_League()
        {
            var League = new League { Name = "National League" };

            await controller.Create(League);

            context.MockLeagues.Verify(x => x.Add(It.Is<League>(t => t == League)));
        }

        [Test]
        public async Task Test_Delete_Deletes_League()
        {
            await controller.DeleteConfirmed(1);

            context.MockLeagues.Verify(x => x.Remove(It.Is<League>(t => t.LeagueId == 1)));
        }
    }
}
