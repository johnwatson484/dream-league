using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DreamLeague.Tests.Mocks
{
    public class MockControllerContext
    {
        public Mock<ControllerContext> ControllerContext { get; set; }

        public MockControllerContext()
        {
            ControllerContext = new Mock<ControllerContext>();
            ControllerContext.Setup(x => x.HttpContext.User.IsInRole(It.IsAny<string>())).Returns(true);
            ControllerContext.Setup(x => x.HttpContext.User.Identity.Name).Returns("Test User");
        }
    }
}
