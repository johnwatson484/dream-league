using DreamLeague.Controllers;
using DreamLeague.Models;
using DreamLeague.Tests.DAL.Mock;
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
    }
}
