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
    public class HistoryControllerTests
    {
        MockDreamLeagueContext context;
        HistoryController controller;

        [SetUp]
        public void SetUp()
        {
            context = new MockDreamLeagueContext();
            controller = new HistoryController(context.MockContext.Object);
        }

        [Test]
        public async Task Test_Index_Returns_Historys()
        {
            var result = await controller.Index();

            var model = ((ViewResult)result).Model as List<History>;

            Assert.AreEqual(2, model.Count);
        }

        [Test]
        public async Task Test_Create_Creates_History()
        {
            var history = new History { Year = 2019 };

            await controller.Create(history);

            context.MockHistory.Verify(x => x.Add(It.Is<History>(t => t == history)));
        }

        [Test]
        public async Task Test_Edit_Edits_History()
        {
            var history = new History { Year = 2017 };

            await controller.Edit(history);

            context.MockContext.Verify(x => x.SetModified(It.Is<object>(t => t == history)));
        }

        [Test]
        public async Task Test_Delete_Deletes_History()
        {
            await controller.DeleteConfirmed(1);

            context.MockHistory.Verify(x => x.Remove(It.Is<History>(t => t.HistoryId == 1)));
        }
    }
}
