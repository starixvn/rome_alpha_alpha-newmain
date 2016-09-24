namespace Alpha.Main.Site.Models.Request.SecurityCode
{
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;
    using ViewModels.Security;

    public class LoginSecurityCodeQuery : IRequest<LoginSecurityCodeViewModel>
    {
        private readonly IUserSession userSession;

        public LoginSecurityCodeQuery() : this(null)
        {
        }


        public LoginSecurityCodeQuery(IUserSession userSession)
        {
            this.userSession = userSession ?? new UserSessionExt();
        }

        public int CustId
        {
            get
            {
                return this.userSession.CustId;
            }
        }

        public int SubAccId
        {
            get
            {
                return this.userSession.SubAccId;
            }
        }


        public bool IsSubAcc
        {
            get
            {
                return this.userSession.IsSubAcc;
            }
        }
        public string UserName
        {
            get
            {
                return this.userSession.UserName;
            }
        }

        public string NickName
        {
            get
            {
                return this.userSession.NickName;
            }
        }

        public string Language
        {
            get
            {
                return this.userSession.Language;
            }
        }

        public string SubUserName
        {
            get
            {
                return this.userSession.SubUserName;
            }
        }
    }
}