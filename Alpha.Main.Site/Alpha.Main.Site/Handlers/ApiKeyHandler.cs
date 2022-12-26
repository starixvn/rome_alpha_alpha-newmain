
namespace Alpha.Main.Site.Handlers
{
    using Alpha.Main.Constants;
    using Alpha.Main.Models.ApiKey;
    using Alpha.Main.Services;
    using Alpha.Main.Site.Requests.ApiKey;
    using Alpha.Main.Site.ViewModels.ApiKey;
    using MediatR;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;

    public class ApiKeyHandler :
        IAsyncRequestHandler<CreateApiKeyQuery, ApiKeyResult>,
        IAsyncRequestHandler<GetApiKeyQuery, EditApiKeyViewModel>,
        IAsyncRequestHandler<GetCustomerApiKeyQuery, ApiKeyManagementViewModel>,
        IAsyncRequestHandler<RevokeApiKeyQuery, Result>,
        IAsyncRequestHandler<UpdateApiKeyQuery, Result>
    {
        private readonly ApiKeyService apiKeyService;

        public ApiKeyHandler(ApiKeyService apiKeyService)
        {
            this.apiKeyService = apiKeyService;
        }

        public async Task<ApiKeyResult> Handle(CreateApiKeyQuery message)
        {
            var result = await apiKeyService.CreateApiKey(message.CustId, message.ApiName, message.PassPhrase, message.Scopes, message.ExpirationDate);
            return result;
        }

        public async Task<EditApiKeyViewModel> Handle(GetApiKeyQuery message)
        {
            var result = await apiKeyService.GetApiKeyByKey(message.Key);
            var scopes = new List<string> { ApiKeyScope.WinLossDetail, ApiKeyScope.Outstanding };
            return new EditApiKeyViewModel(result) { Scopes = scopes };
        }

        public async Task<ApiKeyManagementViewModel> Handle(GetCustomerApiKeyQuery message)
        {
            var result = await apiKeyService.GetApiKeyByCustId(message.CustId);
            return new ApiKeyManagementViewModel { ApiKeyInfos = result };
        }

        public async Task<Result> Handle(RevokeApiKeyQuery message)
        {
            await apiKeyService.RevokeApiKey(message.Key);
            return new Result(0, "Success");
        }

        public async Task<Result> Handle(UpdateApiKeyQuery message)
        {
            await apiKeyService.UpdateApiKey(message.Key, message.CustId, message.ApiName, message.Scopes, message.ExpirationDate);
            return new Result(0, "Success");
        }
    }
}