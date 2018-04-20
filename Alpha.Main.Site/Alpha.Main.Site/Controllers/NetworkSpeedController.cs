namespace Alpha.Main.Site.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Configurations;
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Web;
    using Sunplus.Agent.Model.Entities;
    using Web;

    public class NetworkSpeedController : BaseController
    {
        private readonly IUserSession userSession;
        private static readonly IEnumerable<int> TrackingConnectionCustIds = AppSettings.TrackingConnectionCustIds;

        public NetworkSpeedController(
            IUserSession userSession)
        {
            this.userSession = userSession;
        }

        public ActionResult Index()
        {
            if (!IsNeedToCalculateUserConnectionSpeed())
            {
                return View("NoInfo");
            }

            return View();
        }

        public ActionResult SaveResult(float speedKbps)
        {
            if (speedKbps > 0 && IsNeedToCalculateUserConnectionSpeed())
            {
                LogUserInformation(speedKbps);
            }

            return View("NoInfo");
        }

        private bool IsNeedToCalculateUserConnectionSpeed()
        {
            if(!TrackingConnectionCustIds.Any())
            {
                return false;
            }

            var custId = userSession.CustId;
            foreach (var trackingCustId in TrackingConnectionCustIds)
            {
                if (trackingCustId > 0
                    && custId == trackingCustId)
                {
                    return true;
                }
            }

            return false;
        }

        private void LogUserInformation(float speedKbps)
        {
            var request = System.Web.HttpContext.Current.Request;
            var logMessage = new LogMessage(string.Join(
                                    "\r\n",
                                    "Customer connection information:",
                                    $"Connection Speed: {speedKbps} kbps",
                                    $"Cust Id: {userSession.CustId}",
                                    $"Username: {userSession.UserName}",
                                    $"Sub Account Id: {userSession.SubAccId}",
                                    $"Sub Account Name: {userSession.SubUserName}"))
                                .AddSection("request info", () => WebInfoGetters.GetRequestInfo(request))
                                .AddSection("browser info", () => WebInfoGetters.GetBrowserInfo(request.Browser))
                                .AddSection("server info", () => WebInfoGetters.GetRequestHeader(request.Headers))
                                .AddSection("session info", () => WebInfoGetters.GetSessionInfo(System.Web.HttpContext.Current.Session))
                                .ToString();

            LogService.Info(logMessage);
        }
    }
}