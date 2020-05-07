namespace Alpha.Main.Site.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Alpha.Main.Site.Requests.Banner;
    using Alpha.Web;
    using MediatR;

    public class BannerController : BaseController
    {
        private readonly IMediator mediator;

        public BannerController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<ActionResult> Get(BannerQuery request)
            => Json(await mediator.SendAsync(request), JsonRequestBehavior.AllowGet);
    }
}