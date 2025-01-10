using Alpha.Main.AppResources;
using Alpha.Main.Site.Requests.WhiteLabel;
using Alpha.Web;
using Alpha.Web.Models;
using AlphaPlus.User.Models;
using Fanex.Logging;
using MediatR;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.SessionState;

namespace Alpha.Main.Site.Controllers
{
    [SessionState(SessionStateBehavior.Required)]
    public class WhiteLabelController : BaseController
    {
        private readonly IUserSession userSession;
        private readonly IMediator mediator;
        private readonly ILogger logger;

        public WhiteLabelController(
            IUserSession userSession,
            IMediator mediator,
            ILogger logger)
        {
            this.userSession = userSession;
            this.mediator = mediator;
            this.logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult> SSO()
        {
            if (userSession.IsSubAcc
                || userSession.IsInternal
                || userSession.IsIndirectLoggedIn
                || !Enumeration.FromValue<UserLevel>(userSession.RoleId).IsAgentLevel())
            {
                return Json(Main.Models.Result.CreateErrorResult(Security.NotAuthorized));
            }

            var response = await mediator.SendAsync(new WhiteLabelSSORequest(userSession.CustId.ToString(), Request.UserAgent, userSession.UserLoginIp));
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public async Task<JsonResult> BannerChat()
        {
            var subUsername = string.IsNullOrEmpty(userSession.SubUserName) ? "-" : userSession.SubUserName;
            var message = string.Format(Resources.WhiteLabelBannerChat, userSession.UserName, subUsername);
            await logger.InfoAsync(message);
            return Json(true);
        }
    }
}