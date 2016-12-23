namespace Alpha.Main.Site.Controllers
{
    using System;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Constants;
    using MediatR;
    using Requests.Message;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.Model.Entities;
    using Web;

    [SessionState(SessionStateBehavior.ReadOnly)]
    public class MessageController : BaseController
    {
        private readonly IMediator mediator;
        private readonly IUserSession userSession;

        public MessageController(IMediator mediator, IUserSession userSession)
        {
            this.mediator = mediator;
            this.userSession = userSession;
        }

        public ActionResult Index(int messageType = MessageType.Private)
        {
            var viewModel = mediator.Send(new MessageQuery(
                userSession,
                messageType,
                DateTime.Today.AddDays(-3),
                DateTime.Today,
                AppSettings.LocalTest));

            return View(viewModel);
        }

        public ActionResult Messages(
            DateTime? startDate,
            DateTime? endDate,
            int messageType = MessageType.Public)
        {
            var viewModel = mediator.Send(new MessageQuery(
                userSession,
                messageType,
                startDate ?? DateTime.Today.AddDays(-3),
                endDate ?? DateTime.Today,
                AppSettings.LocalTest));

            return View("Index", viewModel);
        }
    }
}