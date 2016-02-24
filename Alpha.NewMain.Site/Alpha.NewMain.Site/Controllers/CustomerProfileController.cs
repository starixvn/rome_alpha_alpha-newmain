namespace Alpha.NewMain.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Models.Requests.CustomerProfile;
    using Sunplus.Agent.Model.Entities;
    using Utils;

    public class CustomerProfileController : Controller
    {
        private IMediator _mediator;

        public CustomerProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public ActionResult ViewProfile(ViewProfileQuery query)
        {
            LanguageUtil.SetLanguage();
            query.CustId = UserSession.Current.CustId;
            var viewModel = _mediator.Send(query);
            return View(viewModel);
        }
    }
}