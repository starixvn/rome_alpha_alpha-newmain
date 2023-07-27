namespace Alpha.Main.Site.Requests.ApiKey
{
    using Alpha.Main.Site.ViewModels.ApiKey;
    using Alpha.Web.Models;
    using AlphaPlus.User.Models;
    using MediatR;

    public class GetCustomerApiKeyQuery : IAsyncRequest<ApiKeyManagementViewModel>
    {
        public GetCustomerApiKeyQuery() : this(null)
        {
        }

        public GetCustomerApiKeyQuery(IUserSession userSession)
        {
            userSession = userSession ?? new UserSessionExt();
            CustId = userSession.CustId;
            CustLevel = Enumeration.FromValue<UserLevel>(userSession.RoleId);
            Language = userSession.Language;
        }

        public int CustId { get; }

        public UserLevel CustLevel { get; }

        public string Language { get; }
    }
}