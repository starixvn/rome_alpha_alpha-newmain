namespace Alpha.Main.Site.Models.Request.SecurityCode
{
    using AlphaPlus.User.Models;
    using Main.Models;
    using MediatR;

    public class ChangeSecurityCodeFirstLoginCommand : IRequest<Result>
    {
        private readonly IUserSession userSession;

        public ChangeSecurityCodeFirstLoginCommand() : this(null)
        {
        }

        public ChangeSecurityCodeFirstLoginCommand(IUserSession userSession)
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

        public string SecurityCode { get; set; }

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