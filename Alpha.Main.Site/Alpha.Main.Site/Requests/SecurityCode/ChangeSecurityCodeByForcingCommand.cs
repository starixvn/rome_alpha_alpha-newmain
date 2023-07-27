namespace Alpha.Main.Site.Models.Request.SecurityCode
{
    using AlphaPlus.User.Models;
    using Main.Models;
    using MediatR;

    public class ChangeSecurityCodeByForcingCommand : IRequest<Result>
    {
        private readonly IUserSession userSession;

        public ChangeSecurityCodeByForcingCommand() : this(null)
        {
        }

        public ChangeSecurityCodeByForcingCommand(IUserSession userSession)
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

        public int UserId
        {
            get
            {
                return this.userSession.UserId;
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

        public string OldSecurityCode { get; set; }

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
    }
}