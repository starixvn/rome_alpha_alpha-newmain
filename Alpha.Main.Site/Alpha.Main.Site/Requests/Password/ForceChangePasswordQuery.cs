namespace Alpha.Main.Site.Requests.Password
{
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;
    using ViewModels.Security;

    public class ForceChangePasswordQuery : IRequest<ForceChangePasswordViewModel>
    {
        private readonly IUserSession userSession;

        public ForceChangePasswordQuery() : this(null)
        {
        }

        public ForceChangePasswordQuery(IUserSession userSession)
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