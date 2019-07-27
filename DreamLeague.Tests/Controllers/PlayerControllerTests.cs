using DreamLeague.Controllers;
using DreamLeague.Models;
using DreamLeague.Tests.DAL.Mock;
using Moq;
using NUnit.Framework;
using PagedList;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DreamLeague.Tests.Controllers
{
    [TestFixture]
    public class PlayerControllerTests
    {
        MockDreamLeagueContext context;
        PlayerController controller;

        [SetUp]
        public void SetUp()
        {
            context = new MockDreamLeagueContext();
            controller = new PlayerController(context.MockContext.Object);
        }

        [Test]
        public void Test_Index_Returns_Players()
        {
            var result = controller.Index(string.Empty, string.Empty, string.Empty, string.Empty, null);

            var model = ((ViewResult)result).Model as PagedList<Player>;

            Assert.AreEqual(2, model.Count);
        }

        [Test]
        public async Task Test_Details_Returns_Player()
        {
            var result = await controller.Details(1);

            var model = ((ViewResult)result).Model as Player;

            Assert.AreEqual("Adebayo", model.FirstName);
            Assert.AreEqual("Akinfenwa", model.LastName);
        }

        [Test]
        public async Task Test_Create_Creates_Player()
        {
            var Player = new Player { FirstName = "Ian", LastName = "Henderson", Position = Position.Forward };

            await controller.Create(Player);

            context.MockPlayers.Verify(x => x.Add(It.Is<Player>(t => t == Player)));
        }

        [Test]
        public async Task Test_Delete_Deletes_Player()
        {
            await controller.DeleteConfirmed(1);

            context.MockPlayers.Verify(x => x.Remove(It.Is<Player>(t => t.PlayerId == 1)));
        }
    }
}
