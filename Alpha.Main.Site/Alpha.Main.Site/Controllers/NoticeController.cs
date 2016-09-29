namespace Alpha.Main.Site.Controllers
{
    using System;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using MediatR;
    using Requests.Notice;
    using Sunplus.Agent.Model.Entities;

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
            return View();
        }

        [HttpGet]
        public ActionResult NoticeMessages(DateTime lastPostDate, int totalMessage = 10)
        {
            var viewModel = mediator.Send(
                new NoticeQuery(userSession, lastPostDate, totalMessage, false));

            return PartialView("NoticeMessages", viewModel);
        }

        [HttpPost]
        public bool MarkMessageAsRead(string messageIds)
        {
            var isSuccess = mediator.Send(new MarkMessageAsReadQuery(this.userSession.CustId, messageIds));

            return isSuccess;
        }
    }
}