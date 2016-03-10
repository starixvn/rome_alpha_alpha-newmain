namespace Alpha.Main.Site.Services.Impl.Tests
{
    using System;
    using Models;
    using NUnit.Framework;

    [TestFixture]
    public class SiteMenuServiceImplTests
    {
        [Test]
        public void GetMenuByUser_UserIsNull_ArgumentNullExceptionThrown()
        {
            //// Arrange
            var siteMenuService = new SiteMenuServiceImpl();
            User user = null;

            //// Act & Assert
            Assert.Throws<ArgumentNullException>(() => siteMenuService.GetMenuByUser(user));
        }
    }
}