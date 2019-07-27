using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DreamLeague.Tests.Mocks
{
    public static class MockControllerContext
    {
        public static Mock<ControllerContext> GetContext()
        {
            Mock<ControllerContext> controllerContext = new Mock<ControllerContext>();
            controllerContext.Setup(x => x.HttpContext.User.IsInRole(It.IsAny<string>())).Returns(true);
            return controllerContext;
        }
    }
}
