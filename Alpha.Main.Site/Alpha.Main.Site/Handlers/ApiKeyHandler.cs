namespace Alpha.Main.Site.Handlers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Alpha.Main.Constants;
    using Alpha.Main.Models.ApiKey;
    using Alpha.Main.Services;
    using Alpha.Main.Site.Requests.ApiKey;
    using Alpha.Main.Site.ViewModels.ApiKey;
    using MediatR;

    public class ApiKeyHandler :
        IAsyncRequestHandler<CreateApiKeyCommand, CreateApiKeyResult>,
        IAsyncRequestHandler<GetApiKeyQuery, EditApiKeyViewModel>,
        IAsyncRequestHandler<GetCustomerApiKeyQuery, ApiKeyManagementViewModel>,
        IAsyncRequestHandler<RevokeApiKeyCommand, Result>,
        IAsyncRequestHandler<UpdateApiKeyCommand, Result>
    {
        private readonly ApiKeyService apiKeyService;

        public ApiKeyHandler(ApiKeyService apiKeyService)
        {
            this.apiKeyService = apiKeyService;
        }

        public async Task<CreateApiKeyResult> Handle(CreateApiKeyCommand message)
        {
            var result = await apiKeyService.CreateApiKey(message.CustId, message.ApiName, message.PassPhrase, message.Scopes, message.IP, message.Language, message.ExpirationDate);
            return result;
        }

        public async Task<EditApiKeyViewModel> Handle(GetApiKeyQuery message)
        {
            var result = await apiKeyService.GetApiKeyByKey(message.Key);
            var scopes = new List<string> { ApiKeyScope.WinLossDetail, ApiKeyScope.Outstanding };
            return new EditApiKeyViewModel(result, scopes, message.Language);
        }

        public async Task<ApiKeyManagementViewModel> Handle(GetCustomerApiKeyQuery message)
        {
            var result = await apiKeyService.GetApiKeyByCustId(message.CustId);
            return new ApiKeyManagementViewModel(message.CustLevel.DisplayName, result, message.Language);
        }

        public async Task<Result> Handle(RevokeApiKeyCommand message)
        {
            await apiKeyService.RevokeApiKey(message.Key, message.IP);
            return new Result(0, "Success");
        }

        public async Task<Result> Handle(UpdateApiKeyCommand message)
        {
            return await apiKeyService.UpdateApiKey(message.ApiKey, message.CustId, message.ApiName, message.Scopes, message.IP, message.Language, message.ExpirationDate);
        }
    }
}