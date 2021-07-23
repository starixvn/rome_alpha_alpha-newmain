namespace Alpha.Main.Site.Tests.Controllers
{
    using System.Web.Mvc;
    using NSubstitute;
    using NUnit.Framework;
    using Site.Controllers;
    using Sunplus.Agent.Model.Entities;

    [TestFixture]
    public class ErrorControllerTests
    {
        private IUserSession userSession;
        private ErrorController errorController;

        [SetUp]
        public void SetUp()
        {
            userSession = Substitute.For<IUserSession>();
            errorController = new ErrorController(userSession);
        }

        [Test]
        public void Index_Always_ReturnViewResult()
        {
            // Act
            var result = errorController.Index();

            // Assert
            Assert.IsTrue(result is ViewResult);
        }
    }
}
