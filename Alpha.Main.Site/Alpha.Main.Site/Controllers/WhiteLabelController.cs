using Alpha.Main.AppResources;
using Alpha.Main.Site.Requests.WhiteLabel;
using Alpha.Web;
using Alpha.Web.Models;
using AlphaPlus.User.Models;
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

        public WhiteLabelController(IUserSession userSession, IMediator mediator)
        {
            this.userSession = userSession;
            this.mediator = mediator;
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
    }
}