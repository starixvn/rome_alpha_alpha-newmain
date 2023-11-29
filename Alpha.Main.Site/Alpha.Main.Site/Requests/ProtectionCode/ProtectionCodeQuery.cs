namespace Alpha.Main.Site.Requests.ProtectionCode
{
    using Alpha.Main.Site.ViewModels.ProtectionCode;
    using AlphaPlus.User.Models;
    using MediatR;

    public class ProtectionCodeQuery : IAsyncRequest<ProtectionCodeViewModel>
    {
        public ProtectionCodeQuery() : this(null)
        {
        }

        public ProtectionCodeQuery(IUserSession userSession)
        {
            userSession = userSession ?? new UserSessionExt();
            CustId = userSession.CustId;
            SubAccId = userSession.SubAccId;
            RoleId = userSession.RoleId;
            Language = userSession.Language;
            IsOfferProtectedMode = userSession.IsOfferProtectedMode;
        }

        public int CustId { get; }

        public int SubAccId { get; }

        public byte RoleId { get; }

        public string Language { get; }

        public bool IsOfferProtectedMode { get; }
    }
}