#pragma warning disable 618, 612

namespace Alpha.Main.Site.Controllers
{
    using System;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Alpha.Main.Configurations;
    using Alpha.Web;
    using Alpha.Web.Filters;
    using AlphaPlus.User.Models;
    using MediatR;
    using Requests.Dashboard.BalanceInfo;
    using TelegramBot.Client.Services;
    using ViewModels.Dashboard;

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
        public ActionResult Index()
        {
            if (userSession.IsProtectedMode)
            {
                return View("~/Views/Dashboard/IndexProtectedMode.cshtml");
            }

            var viewModel = new DashboardBannerViewModel(userSession);

            if (viewModel.IsShow)
            {
                return View("~/Views/Dashboard/Banner.cshtml", viewModel);
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
            if (AppSettings.IsEnabledRechekin && ((onUserId > 0 && onUserId != userSession.OnUserId) || userSession.OnUserId == 0))
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