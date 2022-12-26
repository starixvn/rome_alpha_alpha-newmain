namespace Alpha.Main.Site.Controllers
{
    using Alpha.Main.Constants;
    using Alpha.Main.Services;
    using Alpha.Main.Site.Requests.ApiKey;
    using Alpha.Main.Site.ViewModels.ApiKey;
    using Alpha.Web;
    using Fanex.ActivityTracking.Models;
    using MediatR;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;

    public class ApiKeyController : BaseController
    {
        private readonly IMediator mediator;

        public ApiKeyController(IMediator mediator, ApiKeyService apiKeyService)
        {
            this.mediator = mediator;
        }

        //public async Task<ActionResult> CreateApiKey()
        //{
        //    var query = new CreateApiKeyQuery() { ApiName = "name", PassPhrase = "pass", Scopes = "winloss", ExpirationDate = DateTime.Now };
        //    var viewModel = await mediator.SendAsync(query);
        //    return View(viewModel);
        //}

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

        [HttpPost]
        public async Task<ActionResult> CreateApiKey(CreateApiKeyQuery query)
        {
            return Json(await mediator.SendAsync(query));
        }

        [HttpPost]
        public async Task<ActionResult> RevokeApiKey(RevokeApiKeyQuery query)
        {
            return Json(await mediator.SendAsync(query));
        }
    }
}