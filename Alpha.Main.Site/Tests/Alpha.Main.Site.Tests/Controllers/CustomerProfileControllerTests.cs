using Alpha.Main.Site.Controllers;
using Alpha.Main.Site.Models.Requests.CustomerProfile;
using MediatR;
using NSubstitute;
using NUnit.Framework;
using Sunplus.Agent.Model.Entities;

namespace Alpha.Main.Site.Tests.Controllers
{
    [TestFixture]
    public class CustomerProfileControllerTests
    {
        private CustomerProfileController customerProfileController;
        private IMediator mediator;
        private IUserSession userSession;

        [SetUp]
        public void SetUp()
        {
            mediator = Substitute.For<IMediator>();
            userSession = Substitute.For<IUserSession>();
            customerProfileController = new CustomerProfileController(userSession, mediator);
        }

        [Test]
        public void BriefProfile_Always_CallsMediatorSendBriefProfileQuery()
        {
            // Act
            customerProfileController.BriefProfile();
            var query = new BriefProfileQuery(userSession);

            // Assert
            mediator.Received().Send(
                Arg.Is<BriefProfileQuery>(
                    param => param.CustId == query.CustId
                        && param.UserName == query.UserName
                        && param.NickName == query.NickName
                        && param.IsSubAcc == query.IsSubAcc
                        && param.IsInternal == query.IsInternal
                        && param.SubUserName == query.SubUserName
                        && param.AdminName == query.AdminName));
        }
    }
}
