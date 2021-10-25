namespace Alpha.Main.Site.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Alpha.Main.Models.UMMessage;
    using Alpha.Main.Site.Requests.UMMessage;
    using Alpha.Web;
    using Alpha.Web.Filters;
    using MediatR;

    [SessionState(SessionStateBehavior.ReadOnly)]
    public class UMMessageController : BaseController
    {
        private readonly IMediator mediator;

        public UMMessageController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [AllowProtectedMode]
        public async Task<ActionResult> Get(UMMessageQuery request)
        {
            var messages = request.EnableUMAnnouncementPopup
                ? await mediator.SendAsync(request)
                : Enumerable.Empty<UMMessageDTO>();

            return Json(messages, JsonRequestBehavior.AllowGet);
        }
    }
}