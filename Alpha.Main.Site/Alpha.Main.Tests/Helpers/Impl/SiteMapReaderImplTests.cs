namespace Alpha.Main.Tests.Helpers.Impl
{
    using Main.Helpers.Impl;
    using NUnit.Framework;

    [TestFixture]
    public class SiteMapReaderImplTests
    {
        private SiteMapReaderImpl siteMapReaderImpl;

        [SetUp]
        public void SetUp()
        {
            siteMapReaderImpl = new SiteMapReaderImpl();
        }

        ////[Test]
        ////public void GetRootNode_NotProvideRootNode_ThrowsNullReferenceException()
        ////{
        ////    // Act & Assert
        ////    Assert.Throws<NullReferenceException>(new TestDelegate(() => siteMapReaderImpl.GetRootNode()));
        ////}

        [Test]
        public void GetRootNode_ProvideRootNode_ReturnsInputRootNode()
        {
            // Arrange
            var rootNode = CustomSiteMapProvider.CreateRootNodeStub(@"\TestData\Menus\MenuItemBuilder.sitemap");
            var expectedSiteMapNode = rootNode;

            // Act
            var actualSiteMapNode = siteMapReaderImpl.GetRootNode(rootNode);

            // Assert
            Assert.AreEqual(expectedSiteMapNode, actualSiteMapNode);
        }
    }
}
