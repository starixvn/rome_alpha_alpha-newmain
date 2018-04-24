namespace Alpha.Main.Site.Handlers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Alpha.Main.Site.Requests.NetworkSpeed;
    using Alpha.Main.Site.ViewModels.NetworkSpeed;
    using Configurations;
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Web;
    using MediatR;
    using Sunplus.Agent.Model.Entities;

    public class NetworkSpeedHandler :
        IRequestHandler<NetworkSpeedQuery, NetworkSpeedViewModel>,
        IRequestHandler<SaveNetworkSpeedCommand, Unit>
    {
        private static readonly IEnumerable<int> TrackingConnectionCustIds = AppSettings.TrackingConnectionCustIds;
        private readonly ILogger logger;
        private readonly IUserSession userSession;

        public NetworkSpeedHandler(ILogger logger, IUserSession userSession)
        {
            this.logger = logger;
            this.userSession = userSession;
        }

        public Unit Handle(SaveNetworkSpeedCommand message)
        {
            if (message.SpeedKbps > 0)
            {
                LogUserInformation(message);
            }

            return default(Unit);
        }

        public NetworkSpeedViewModel Handle(NetworkSpeedQuery message)
            => new NetworkSpeedViewModel(IsNeedToCalculateUserConnectionSpeed(message.CustId));

        private static bool IsNeedToCalculateUserConnectionSpeed(int custId)
        {
            if (!TrackingConnectionCustIds.Any())
            {
                return false;
            }

            return TrackingConnectionCustIds.Contains(custId);
        }

        internal virtual void LogUserInformation(SaveNetworkSpeedCommand command)
        {
            // not unittestable here
            var request = HttpContext.Current.Request;
            var logMessage = new LogMessage(string.Join(
                                    "\r\n",
                                    "Customer connection information:",
                                    $"Connection Speed: {command.SpeedKbps} kbps",
                                    $"Cust Id: {userSession.CustId}",
                                    $"Username: {userSession.UserName}",
                                    $"Sub Account Id: {userSession.SubAccId}",
                                    $"Sub Account Name: {userSession.SubUserName}"))
                                .AddSection("request info", () => WebInfoGetters.GetRequestInfo(request))
                                .AddSection("browser info", () => WebInfoGetters.GetBrowserInfo(request.Browser))
                                .AddSection("server info", () => WebInfoGetters.GetRequestHeader(request.Headers))
                                .AddSection("session info", () => WebInfoGetters.GetSessionInfo(HttpContext.Current.Session))
                                .ToString();

            logger.Info(logMessage);
        }
    }
}