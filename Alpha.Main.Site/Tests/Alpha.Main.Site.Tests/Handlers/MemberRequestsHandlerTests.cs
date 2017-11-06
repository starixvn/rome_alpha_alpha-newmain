namespace Alpha.Main.Site.Tests.Handlers
{
    using System;
    using System.Linq;
    using Constants;
    using KellermanSoftware.CompareNetObjects;
    using Main.Models.MemberRequests;
    using Main.Tests.TestData.Helper;
    using NSubstitute;
    using NUnit.Framework;
    using Services;
    using Site.Handlers;
    using Site.Requests.Dashboard.MemberRequests;
    using Site.ViewModels.Dashboard.MemberRequests;
    using Sunplus.Agent.Model.Entities;

    [TestFixture]
    public class MemberRequestsHandlerTests
    {
        private MemberRequestsHandler memberRequestsHandler;
        private MemberRequestsService memberRequestsService;
        private IUserSession userSession;

        [SetUp]
        public void SetUp()
        {
            userSession = Substitute.For<IUserSession>();
            userSession.CustId.Returns(CustId.ValidId);
            userSession.RoleId.Returns(RoleId.Agent);
            userSession.SubPageAccess.Returns(new PageAccess());

            memberRequestsService = Substitute.For<MemberRequestsService>();
            memberRequestsHandler = new MemberRequestsHandler(memberRequestsService, userSession);
        }

        [Test]
        public void HandlerMemberRequestsQuery_MessageIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            MemberRequestsQuery message = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => memberRequestsHandler.Handle(message));
        }

        [Test]
        public void HandlerMemberRequestsQuery_MessageIsNotNull_CallsGetProductRequestData()
        {
            // Arrange
            var message = new MemberRequestsQuery(userSession);

            // Act
            memberRequestsHandler.Handle(message);

            // Assert
            memberRequestsService.Received()
                .GetProductRequestData(Arg.Is<int>(custId => custId == message.CustId));
        }

        [Test]
        public void HandlerMemberRequestsQuery_MessageIsNotNull_ReturnsMemberRequestsViewModel()
        {
            // Arrange
            var productRequests = FakeDataHelper.CreateMany<ProductRequest>();
            memberRequestsService.GetProductRequestData(Arg.Any<int>()).Returns(productRequests);
            userSession.GetHideProduct(Arg.Any<int>()).Returns(false);
            var message = new MemberRequestsQuery(userSession);
            var compareLogic = new CompareLogic();

            var expectedViewModel = new MemberRequestsViewModel(
                productRequests.Where(product => memberRequestsHandler.IsShowProduct(product.ProductId)).Select(product => product),
                message.UserLevel,
                message.UserSession.SubPageAccess.ViewReports,
                message.UserSession.SubPageAccess.ViewMemberInfo);

            // Act
            var actualViewModel = memberRequestsHandler.Handle(message);

            // Assert
            Assert.True(compareLogic.Compare(expectedViewModel, actualViewModel).AreEqual);
        }
    }
}
