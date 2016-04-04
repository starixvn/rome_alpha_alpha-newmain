namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Models.Requests.CustomerProfile;

    public class CustomerProfileController : BaseController
    {
        private IMediator _mediator;

        public CustomerProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public ActionResult BriefProfile(BriefProfileQuery query)
        {
            var viewModel = _mediator.Send(query);
            return View(viewModel);
        }
    }
}