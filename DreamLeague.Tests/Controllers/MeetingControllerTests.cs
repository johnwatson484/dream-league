using DreamLeague.Controllers;
using DreamLeague.Models;
using DreamLeague.Services;
using DreamLeague.Tests.DAL.Mock;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DreamLeague.Tests.Controllers
{
    [TestFixture]
    public class MeetingControllerTests
    {
        MockDreamLeagueContext context;
        Mock<IMeetingService> meetingService;
        MeetingController controller;

        [SetUp]
        public void SetUp()
        {
            context = new MockDreamLeagueContext();
            meetingService = new Mock<IMeetingService>();
            controller = new MeetingController(context.MockContext.Object, meetingService.Object);
        }

        [Test]
        public async Task Test_Index_Returns_Meetings()
        {
            var result = await controller.Index();

            var model = ((ViewResult)result).Model as List<Meeting>;

            Assert.AreEqual(2, model.Count);
        }

        [Test]
        public async Task Test_Details_Returns_Meeting()
        {
            var result = await controller.Details(1);

            var model = ((ViewResult)result).Model as Meeting;

            Assert.AreEqual(new DateTime(2019, 8, 1), model.Date);
        }

        [Test]
        public async Task Test_Create_Creates_Meeting()
        {
            var Meeting = new Meeting { Date = new DateTime(2019, 10, 1) };

            await controller.Create(Meeting);

            context.MockMeetings.Verify(x => x.Add(It.Is<Meeting>(t => t == Meeting)));
        }

        [Test]
        public async Task Test_Delete_Deletes_Meeting()
        {
            await controller.DeleteConfirmed(1);

            context.MockMeetings.Verify(x => x.Remove(It.Is<Meeting>(t => t.MeetingId == 1)));
        }
    }
}
