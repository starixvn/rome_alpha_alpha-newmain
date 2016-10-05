#pragma warning disable 612, 618
namespace Alpha.Main.Site.Models.Requests
{
    using MediatR;
    using SecurityCode.Services;
    using Sunplus.Agent.Model.Entities;
    using ViewModels;

    public class IndexQuery : IRequest<IndexViewModel>
    {
        private readonly bool isEnableSecurityCode;
        private readonly IUserSession userSession;

        public IndexQuery(IUserSession userSession, bool isEnableSecurityCode)
        {
            this.userSession = userSession;
            this.isEnableSecurityCode = isEnableSecurityCode;
        }

        public int CustId => userSession.CustId;        

        public int OnUserId => userSession.OnUserId;        

        public int SubAccId => userSession.SubAccId;       

        public byte RoleId => userSession.RoleId;       

        public string Language => userSession.Language;       

        public string LanguageKey => userSession.LangKey;

        public string SubPageAccess => userSession.SubPageAccess;        

        public bool IsSubAccount => userSession.IsSubAcc;               
        
        public bool IsInternalWebVpn => userSession.IsInternalWebVPN;

        public bool EnableSecurityCode => userSession.IsLogged &&
                    !userSession.IsPasswordExpired &&
                    this.isEnableSecurityCode;
    }
}