namespace Alpha.Main.Site.Tests.Controllers
{
    using System.Web.Mvc;
    using NUnit.Framework;
    using Site.Controllers;

    [TestFixture]
    public class ErrorControllerTests
    {
        private ErrorController errorController;

        [SetUp]
        public void SetUp()
        {
            errorController = new ErrorController();
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
