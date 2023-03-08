namespace Alpha.Main.Site.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Alpha.Main.Configurations;
    using Alpha.Main.Site.Requests.ApiKey;
    using Alpha.Main.Site.ViewModels.ApiKey;
    using Alpha.Web;
    using Alpha.Web.Filters;
    using Alpha.Web.Models;
    using MediatR;
    using Sunplus.Agent.Model.Entities;
    using Alpha.Main.Configurations;

    public class ApiKeyController : BaseController
    {
        private readonly IMediator mediator;
        private readonly IUserSession userSession;

        public ApiKeyController(IMediator mediator, IUserSession userSession)
        {
            this.mediator = mediator;
            this.userSession = userSession;
        }

        public ActionResult CreateApiKey()
        {
            var viewModel = new CreateApiKeyViewModel(Enumeration.FromValue<UserLevel>(userSession.RoleId).DisplayName, userSession.Language);
            return View(viewModel);
        }

        public async Task<ActionResult> ApiKeyManagement(GetCustomerApiKeyQuery query)
        {
            var viewModel = await mediator.SendAsync(query);
            return View(viewModel);
        }

        public async Task<ActionResult> EditApiKey(GetApiKeyQuery query)
        {
            var viewModel = await mediator.SendAsync(query);
            return View(viewModel);
        }

        public ActionResult CreateApiKeyResult(string apiName, string key, string secret)
        {
            var viewModel = new CreateApiKeyResultViewModel(apiName, key, secret);
            return View(viewModel);
        }

        [HttpPost, AntiXsrfFilter]
        public async Task<ActionResult> CreateApiKey(CreateApiKeyCommand query)
        {
            return Json(await mediator.SendAsync(query));
        }

        [HttpPost, AntiXsrfFilter]
        public async Task<ActionResult> RevokeApiKey(RevokeApiKeyCommand query)
        {
            return Json(await mediator.SendAsync(query));
        }

        [HttpPost, AntiXsrfFilter]
        public async Task<ActionResult> UpdateApiKey(UpdateApiKeyCommand query)
        {
            return Json(await mediator.SendAsync(query));
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (userSession.IsSubAcc
                || userSession.IsProtectedMode
                || (AppSettings.APISuperIds.Any() && !AppSettings.APISuperIds.Any(s => string.Equals(s, userSession.SRecommendId.ToString()))))
            {
                UnAuthentication(filterContext);
            }
        }
    }
}