#pragma warning disable 618, 612

namespace Alpha.Main.Site.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Alpha.Main.Configurations;
    using Alpha.Web.Filters;
    using MediatR;
    using Requests.Dashboard.BalanceInfo;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Model.Entities.Permission;
    using TelegramBot.Client.Services;
    using ViewModels.Dashboard;
    using Web;

    [SessionState(SessionStateBehavior.ReadOnly)]
    public class DashboardController : BaseController
    {
        private readonly IMediator mediator;
        private readonly IUserSession userSession;
        private readonly IBotService botService;

        public DashboardController(IMediator mediator, IUserSession userSession, IBotService botService)
        {
            this.mediator = mediator;
            this.userSession = userSession;
            this.botService = botService;
        }

        [HttpGet]
        [AllowProtectedMode]
        public async Task<ActionResult> Index()
        {
            if (userSession.IsProtectedMode)
            {
                return View("~/Views/Dashboard/IndexProtectedMode.cshtml");
            }

            if (AppSettings.TelegramBotIsShowBanner)
            {
                var bot = await botService.GetBySystem(AppSettings.SiteGroup);
                return View($"~/Views/Dashboard/ChatBotBanner{(AppSettings.IsMBCSite ? "i1" : "i7")}.cshtml", (object)bot?.DisplayName);
            }

            if (AppSettings.IsShowEmptyDashboard)
            {
                string botName;

                if (AppSettings.SiteGroup.Equals("IBCBET", StringComparison.OrdinalIgnoreCase))
                {
                    botName = "Eva";
                }
                else if (AppSettings.SiteGroup.Equals("IBCIndo", StringComparison.OrdinalIgnoreCase))
                {
                    botName = "Eden";
                }
                else
                {
                    botName = "Elsa";
                }

                return View($"~/Views/Dashboard/ChatBotBanner{(AppSettings.IsMBCSite ? "i1" : "i7")}.cshtml", (object)botName);
            }

            return Index2();
        }

        [HttpGet]
        public ActionResult Index2()
        {
            var viewModel = new DashboardViewModel(userSession.RoleId);

            return View(viewModel);
        }

        [HttpGet]
        [ChildActionOnly]
        public ActionResult BalanceInfo()
        {
            var viewModel = mediator.Send(new BalanceInfoQuery(userSession, UserPermission.HasPermissionOnProduct));

            return PartialView(viewModel);
        }

        [HttpGet]
        [AllowProtectedMode]
        public ActionResult GetServerTime(int onUserId = -1)
        {
            if ((onUserId != -1 && onUserId != userSession.OnUserId) || userSession.OnUserId == 0)
            {
                return Json(new
                {
                    signOut = true
                }, JsonRequestBehavior.AllowGet);
            }

            var currentTime = DateTime.Now;

            return Json(new
            {
                currentTime.Year,
                currentTime.Month,
                currentTime.Day,
                currentTime.Hour,
                currentTime.Minute,
                currentTime.Second
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
