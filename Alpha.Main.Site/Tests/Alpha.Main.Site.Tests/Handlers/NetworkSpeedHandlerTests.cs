namespace Alpha.Main.Site.Tests.Handlers
{
    using System.Collections.Specialized;
    using System.Web;
    using Configurations;
    using Fanex.Logging;
    using NSubstitute;
    using NUnit.Framework;
    using Site.Handlers;
    using Site.Requests.NetworkSpeed;
    using Sunplus.Agent.Model.Entities;

    [TestFixture]
    public class NetworkSpeedHandlerTests
    {
        private ILogger logger;
        private IUserSession userSession;
        private NetworkSpeedHandler handler;

        [SetUp]
        public void SetUp()
        {
            HttpContext.Current = new HttpContext(new HttpRequest(null, "http://tempuri.org", null), new HttpResponse(null));

            logger = Substitute.For<ILogger>();
            userSession = Substitute.For<IUserSession>();
            handler = Substitute.ForPartsOf<NetworkSpeedHandler>(logger, userSession);
        }

        [Test]
        public void HandleSaveNetworkSpeedCommand_SpeedKbpsGreaterThanZero_CallLogUserInformation()
        {
            // Arrange
            var message = new SaveNetworkSpeedCommand { SpeedKbps = 1 };

            // Act
            handler.Handle(message);

            // Assert
            handler.Received().LogUserInformation(Arg.Is<SaveNetworkSpeedCommand>(m => ReferenceEquals(message, m)));
        }

        [Test]
        public void HandlerNetworkSpeedQuery_NotHaveAnyTrackingConnectionCustIds_ReturnNetworkSpeedViewModelWithRequiredCalculationIsFalse()
        {
            // Arrange
            AppSettings.Settings = new NameValueCollection();

            // Act
            var viewModel = handler.Handle(new NetworkSpeedQuery(custId: 1));

            // Assert
            Assert.False(viewModel.RequiredCalculation);
        }

        [Test]
        public void HandlerNetworkSpeedQuery_TrackingConnectionCustIdsIsNotContainCustId_ReturnNetworkSpeedViewModelWithRequiredCalculationIsFalse()
        {
            // Arrange
            AppSettings.Settings = new NameValueCollection
            {
                ["TrackingConnectionCustIds"] = "2"
            };

            // Act
            var viewModel = handler.Handle(new NetworkSpeedQuery(custId: 1));

            // Assert
            Assert.False(viewModel.RequiredCalculation);
        }

        [Test]
        public void HandlerNetworkSpeedQuery_TrackingConnectionCustIdsIsContainCustId_ReturnNetworkSpeedViewModelWithRequiredCalculationIsTrue()
        {
            // Arrange
            AppSettings.Settings = new NameValueCollection
            {
                ["TrackingConnectionCustIds"] = "1"
            };

            // Act
            var viewModel = handler.Handle(new NetworkSpeedQuery(custId: 1));

            // Assert
            Assert.False(viewModel.RequiredCalculation);
        }
    }
}
