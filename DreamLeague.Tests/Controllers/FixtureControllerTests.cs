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
    public class FixtureControllerTests
    {
        MockDreamLeagueContext context;
        FixtureController controller;

        [SetUp]
        public void SetUp()
        {
            context = new MockDreamLeagueContext();
            controller = new FixtureController(context.MockContext.Object);
        }

        [Test]
        public async Task Test_Index_Returns_Fixtures()
        {
            var result = await controller.Index();

            var model = ((ViewResult)result).Model as List<Fixture>;

            Assert.AreEqual(2, model.Count);
        }

        [Test]
        public async Task Test_Details_Returns_Fixture()
        {
            var result = await controller.Details(1);

            var model = ((ViewResult)result).Model as Fixture;

            Assert.AreEqual(1, model.CupId);
        }

        [Test]
        public async Task Test_Create_Creates_Fixture()
        {
            var fixture = new Fixture { CupId = 1 };

            await controller.Create(fixture);

            context.MockFixtures.Verify(x => x.Add(It.Is<Fixture>(t => t == fixture)));
        }

        [Test]
        public async Task Test_Delete_Deletes_Fixture()
        {
            await controller.DeleteConfirmed(1);

            context.MockFixtures.Verify(x => x.Remove(It.Is<Fixture>(t => t.FixtureId == 1)));
        }
    }
}
