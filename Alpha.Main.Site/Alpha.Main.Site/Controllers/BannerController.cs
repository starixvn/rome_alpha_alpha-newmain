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
        public async Task<ActionResult> Get(BannerQuery request)
        {
            var viewModel = request.IsProtectedMode
                ? new BannerViewModel()
                : await mediator.SendAsync(request);

            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }
    }
}