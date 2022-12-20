namespace Alpha.Main.Site.Requests.Otp
{
    using Alpha.Main.Configurations;
    using Alpha.Main.Site.ViewModels.Security;
    using Alpha.Web.Models;
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;
    using System;
    using System.Linq;

    public class SetUpOtpQuery : IAsyncRequest<OtpViewModel>
    {
        private readonly IUserSession userSession;

        public SetUpOtpQuery() : this(null)
        {
        }

        internal SetUpOtpQuery(IUserSession userSession)
        {
            this.userSession = userSession ?? new UserSessionExt();
            CustId = this.userSession.CustId;
            SubAccountId = this.userSession.SubAccId;
            CustLevel = Enumeration.FromValue<UserLevel>(this.userSession.RoleId);
            Language = this.userSession.Language;
            IsShowLetstalkChatBot = !this.userSession.IsSubAcc
                && (AppSettings.OfferChatbotSites.Contains("*")
                    || AppSettings.OfferChatbotSites.Contains(this.userSession.Site, StringComparer.OrdinalIgnoreCase));
        }

        public int CustId { get; }

        public int SubAccountId { get; }

        public string Username
        {
            get; set;
        }

        public UserLevel CustLevel { get; }

        public string Language { get; }

        public bool IsShowLetstalkChatBot { get; }

        public bool IsOfferProtectedMode => userSession.IsOfferProtectedMode;
    }
}