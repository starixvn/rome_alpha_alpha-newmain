namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Models.Requests.CustomerProfile;
    using Sunplus.Agent.Model.Entities;

    public class CustomerProfileController : BaseController
    {
        private readonly IUserSession userSession;
        private readonly IMediator mediator;

        public CustomerProfileController(IUserSession userSession, IMediator mediator)
        {
            this.userSession = userSession;
            this.mediator = mediator;
        }

        public ActionResult BriefProfile(BriefProfileQuery query)
        {
            query.CustId = userSession.CustId;
            query.NickName = userSession.NickName;
            query.UserName = userSession.UserName;
            query.AdminName = userSession.AdminName;
            query.IsSubAcc = userSession.IsSubAcc;
            query.IsInternal = userSession.IsInternal;
            query.SubUserName = userSession.SubUserName;

            var viewModel = mediator.Send(query);

            return View(viewModel);
        }
    }
}