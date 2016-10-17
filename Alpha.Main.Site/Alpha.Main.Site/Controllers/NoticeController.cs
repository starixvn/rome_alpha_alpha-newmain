#pragma warning disable 618, 612

namespace Alpha.Main.Site.Controllers
{
    using System;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using MediatR;
    using Requests.Dashboard.Notice;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.Model.Entities;
    using ViewModels.Dashboard.Notice;

    [SessionState(SessionStateBehavior.ReadOnly)]
    public class NoticeController : BaseController
    {
        private readonly IMediator mediator;
        private readonly IUserSession userSession;

        public NoticeController(IMediator mediator, IUserSession userSession)
        {
            this.mediator = mediator;
            this.userSession = userSession;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var viewModel = new NoticeViewModel();

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult NoticeMessages(DateTime lastPostDate, int totalMessage = 10)
        {
            var viewModel = mediator.Send(
                new NoticeQuery(userSession, lastPostDate, totalMessage, AppSettings.LocalTest));

            return PartialView("NoticeMessages", viewModel);
        }

        [HttpPost]
        public bool MarkMessageAsRead(int messageId)
        {
            var isSuccess = mediator.Send(new MarkMessageAsReadQuery(userSession.CustId, messageId));

            return isSuccess;
        }

        [HttpGet]
        public ActionResult MoreMessages(int totalMessage = 10)
        {
            var viewModel = new NoticeViewModel(totalMessage);

            return View("Index", viewModel);
        }
    }
}