#pragma warning disable 618, 612

namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Models.Requests;
    using Models.Requests.UserMessages;
    using Requests;
    using Requests.Otp;
    using SecurityCode.Services;
    using Sunplus.Agent.Model.Entities;
    using Web;

    public class DefaultController : BaseController
    {
        private const string ForceChangePasswordPath = "~/Password/ForceChangePassword";
        private const string SecurityCodePath = "~/SecurityCode/Index";
        private const string OtpVerifyDifferenceDevicePath = "~/otp/VerifyOtpDifferenceDevice";
        private const string UnSupportedBrowsers = "UnsupportedBrowsers";

        private readonly IUserSession userSession;
        private readonly IMediator mediator;
        private readonly SecurityCodeChecker securityCodeChecker;

        public DefaultController(
            IUserSession userSession,
            IMediator mediator,
            SecurityCodeChecker securityCodeChecker)
        {
            this.userSession = userSession;
            this.mediator = mediator;
            this.securityCodeChecker = securityCodeChecker;
        }

        public ActionResult Index()
        {
            var browser = mediator.Send(new BrowserQuery(userSession));

            if (browser.IsUnsupportedBrowser)
            {
                return View(UnSupportedBrowsers, browser);
            }

            if (!this.userSession.IsLogged
                || this.userSession.IsPasswordExpired)
            {
                return Redirect(ForceChangePasswordPath);
            }

            if (this.mediator.Send(new CheckDifferenceDeviceCommand(this.userSession)))
            {
                return Redirect(OtpVerifyDifferenceDevicePath);
            }

            if (this.securityCodeChecker.IsForcedChangeSecurityCode())
            {
                return Redirect(SecurityCodePath);
            }

            var query = new IndexQuery(userSession, this.securityCodeChecker.IsEnableSecurityCode());

            var viewModel = mediator.Send(query);

            return View(viewModel);
        }

        public ActionResult MenuBar()
        {
            var viewModel = mediator.Send(new MenuBarQuery(userSession.RoleId, Request.UserAgent));

            return PartialView(viewModel);
        }

        public ActionResult HeaderHotMsg()
        {
            var query = new HotMessagesQuery
            {
                Language = userSession.Language,
                RoleId = userSession.RoleId
            };

            var viewModel = mediator.Send(query);

            return View(viewModel);
        }
    }
}