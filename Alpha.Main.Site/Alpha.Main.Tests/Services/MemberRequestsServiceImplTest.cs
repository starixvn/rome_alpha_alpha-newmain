using Alpha.Main.Models.MemberRequests;
using Alpha.Main.Services.Impl;
using Alpha.Main.Tests.Constants;
using Alpha.Main.Tests.TestData.Helper;
using Fanex.Data.Repository;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Alpha.Main.Tests.Services
{
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
            var expectedProductRequestData = FakeDataHelper.CreateMany<ProductRequest>().ToList();
            dynamicRepository.Fetch<ProductRequest>(Arg.Any<ICriteria>()).Returns(expectedProductRequestData);

            // Act
            var productRequestData = memberRequestService.GetProductRequestData(CustId.ValidId);

            // Assert
            Assert.AreEqual(expectedProductRequestData, productRequestData);
        }
    }
}
