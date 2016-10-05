namespace Alpha.Main.Site.Models.Request.SecurityCode
{
    using Main.Models;
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;

    public class VerifySecurityCodeCommand : IRequest<Result>
    {
        private readonly IUserSession userSession;

        public VerifySecurityCodeCommand() : this(null)
        {
        }

        public VerifySecurityCodeCommand(IUserSession userSession)
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

        public string SecurityCode { get; set; }

        public string SecurityCodeToken
        {
            get
            {
                return this.userSession.SecurityCodeToken;
            }
        }

        public string Language
        {
            get
            {
                return this.userSession.Language;
            }
        }

        public string IP
        {
            get
            {
                return this.userSession.UserLoginIp;
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
                return !this.userSession.SubAccId.Equals(0);
            }
        }
    }
}