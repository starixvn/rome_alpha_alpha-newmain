namespace Alpha.Main.Site.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Alpha.Main.Site.Requests.UMMessage;
    using Alpha.Web;
    using MediatR;

    public class UMMessageController : BaseController
    {
        private readonly IMediator mediator;

        public UMMessageController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<ActionResult> Get(UMMessageQuery request)
            => Json(await mediator.SendAsync(request), JsonRequestBehavior.AllowGet);
    }
}