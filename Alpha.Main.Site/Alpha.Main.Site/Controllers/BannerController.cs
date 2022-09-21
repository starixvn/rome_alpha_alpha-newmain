namespace Alpha.Main.Site.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Alpha.Main.Site.Requests.Banner;
    using Alpha.Main.Site.ViewModels.Banner;
    using Alpha.Web;
    using Alpha.Web.Filters;
    using MediatR;

    public class BannerController : BaseController
    {
        private readonly IMediator mediator;

        public BannerController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [AllowProtectedMode]
        public async Task<ActionResult> Index(BannerQuery request)
        {
            var viewModel = request.IsHideBanner
                ? null
                : await mediator.SendAsync(request);

            return PartialView(viewModel);
        }
    }
}