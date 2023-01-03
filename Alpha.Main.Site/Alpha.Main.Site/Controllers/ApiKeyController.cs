namespace Alpha.Main.Site.Controllers
{
    using Alpha.Main.Site.Requests.ApiKey;
    using Alpha.Main.Site.ViewModels.ApiKey;
    using Alpha.Web;
    using Alpha.Web.Models;
    using Alpha.Web.Filters;
    using MediatR;
    using Sunplus.Agent.Model.Entities;
    using System.Threading.Tasks;
    using System.Web.Mvc;

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
    }
}