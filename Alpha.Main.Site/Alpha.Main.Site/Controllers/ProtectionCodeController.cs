namespace Alpha.Main.Site.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Alpha.Main.AppResources;
    using Alpha.Main.Models;
    using Alpha.Main.Site.Requests.ProtectionCode;
    using Alpha.Web;
    using Alpha.Web.Filters;
    using MediatR;
    using Sunplus.Agent.Model.Entities;

    public class ProtectionCodeController : BaseController
    {
        private readonly IMediator mediator;
        private readonly IUserSession userSession;

        public ProtectionCodeController(IMediator mediator, IUserSession userSession)
        {
            this.mediator = mediator;
            this.userSession = userSession;
        }

        public async Task <ActionResult> Setup(ProtectionCodeQuery query)
        {
            if (query.IsOfferProtectedMode)
            {
                return View(await mediator.SendAsync(query));
            }

            return RedirectToAction("Index", "Default");
        }

        [HttpPost]
        [AntiXsrfFilter]
        public async Task<ActionResult> Setup(UpdateProtectionCodeCommand query)
        {
            var result = query.IsInternal || query.IsNotOfferProtectedMode
                ? Result.CreateErrorResult(Security.NotAuthorized)
                : await mediator.SendAsync(query);

            return Json(result, JsonRequestBehavior.DenyGet);
        }
    }
}