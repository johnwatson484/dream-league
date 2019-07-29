using DreamLeague.Controllers;
using DreamLeague.Models;
using DreamLeague.Services;
using DreamLeague.Tests.DAL.Mock;
using DreamLeague.Tests.Data.Mock;
using DreamLeague.ViewModels;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DreamLeague.Tests.Controllers
{
    [TestFixture]
    public class CupControllerTests
    {
        MockDreamLeagueContext context;
        Mock<ICupService> cupService;
        CupController controller;

        [SetUp]
        public void SetUp()
        {
            context = new MockDreamLeagueContext();
            cupService = new Mock<ICupService>();
            cupService.Setup(x => x.GetData(It.IsAny<int>())).Returns<int>(id => new CupViewModel(CupData.Data().FirstOrDefault(c => c.CupId == id)));
            controller = new CupController(context.MockContext.Object, cupService.Object);
        }

        [Test]
        public async Task Test_Index_Returns_Cups()
        {
            var result = await controller.Index();

            var model = ((ViewResult)result).Model as List<Cup>;

            Assert.AreEqual(2, model.Count);
        }

        [Test]
        public void Test_Details_Returns_Cup()
        {
            var result = controller.Details(1);

            var model = ((ViewResult)result).Model as CupViewModel;

            Assert.AreEqual("Cup", model.Cup.Name);
        }

        [Test]
        public async Task Test_Create_Creates_Cup()
        {
            var cup = new Cup { Name = "Plate" };

            await controller.Create(cup);

            context.MockCups.Verify(x => x.Add(It.Is<Cup>(t => t == cup)));
        }

        [Test]
        public async Task Test_Edit_Edits_Cup()
        {
            var cup = new Cup { Name = "Cup" };

            await controller.Edit(cup);

            context.MockContext.Verify(x => x.SetModified(It.Is<object>(t => t == cup)));
        }

        [Test]
        public async Task Test_Delete_Deletes_Cup()
        {
            await controller.DeleteConfirmed(1);

            context.MockCups.Verify(x => x.Remove(It.Is<Cup>(t => t.CupId == 1)));
        }
    }
}