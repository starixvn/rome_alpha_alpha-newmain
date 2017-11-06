namespace Alpha.Main.Tests.Services
{
    using System.Collections.Generic;
    using Alpha.Main.Models.MemberRequests;
    using Alpha.Main.Services.Impl;
    using Alpha.Main.Tests.Constants;
    using Fanex.Data.Repository;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class MemberRequestsServiceImplTest
    {
        private IDynamicRepository dynamicRepository;
        private MemberRequestsServiceImpl memberRequestService;

        [SetUp]
        public void Setup()
        {
            dynamicRepository = Substitute.For<IDynamicRepository>();
            memberRequestService = new MemberRequestsServiceImpl(dynamicRepository);
        }

        [Test]
        public void GetProductRequestData_InValidCustId_ReturnsEmpty()
        {
            // Act
            var productRequestData = memberRequestService.GetProductRequestData(CustId.InvalidId);

            // Assert
            Assert.AreEqual(productRequestData, new List<ProductRequest>());
        }

        [Test]
        public void GetProductRequestData_ValidCustId_ReturnsProductRequestData()
        {
            // Arrange
            var expectedProductRequestData = new List<ProductRequest> { new ProductRequest(1, string.Empty, 0) };
            dynamicRepository.Fetch<ProductRequest>(Arg.Any<ICriteria>()).Returns(expectedProductRequestData);

            // Act
            var productRequestData = memberRequestService.GetProductRequestData(CustId.ValidId);

            // Assert
            Assert.AreEqual(expectedProductRequestData, productRequestData);
        }
    }
}
