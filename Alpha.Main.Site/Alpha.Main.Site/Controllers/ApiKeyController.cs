namespace Alpha.Main.Site.Controllers
{
    using Alpha.Main.Constants;
    using Alpha.Main.Services;
    using Alpha.Main.Site.Requests.ApiKey;
    using Alpha.Main.Site.ViewModels.ApiKey;
    using Alpha.Web;
    using MediatR;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    public class ApiKeyController : BaseController
    {
        private readonly IMediator mediator;

        public ApiKeyController(IMediator mediator, ApiKeyService apiKeyService)
        {
            this.mediator = mediator;
        }

        public ActionResult CreateApiKey()
        {
            var viewModel = new CreateApiKeyViewModel
            {
                Scopes = new List<string>
                {
                    ApiKeyScope.WinLossDetail,
                    ApiKeyScope.Outstanding
                }
            };
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

        public async Task<ActionResult> CreateApiKeyResult(string apiName, string key, string secret)
        {
            var viewModel = new CreateApiKeyResultViewModel { ApiName = apiName, Key = key, Secret = secret };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> CreateApiKey(CreateApiKeyCommand query)
        {
            return Json(await mediator.SendAsync(query));
        }

        [HttpPost]
        public async Task<ActionResult> RevokeApiKey(RevokeApiKeyCommand query)
        {
            return Json(await mediator.SendAsync(query));
        }

        [HttpPost]
        public async Task<ActionResult> UpdateApiKey(UpdateApiKeyCommand query)
        {
            return Json(await mediator.SendAsync(query));
        }
    }
}