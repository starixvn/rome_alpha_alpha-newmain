namespace Alpha.Main.Site.Models.Request.SecurityCode
{
    using Main.Models;
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;

    public class ResetSecurityCodeCommand : IRequest<Result>
    {
        private readonly IUserSession userSession;

        public ResetSecurityCodeCommand() : this(null)
        {
        }

        public ResetSecurityCodeCommand(IUserSession userSession)
        {
            this.userSession = userSession ?? new UserSessionExt();
        }

        public int? CustId { get; set; }

        public int UserId
        {
            get
            {
                return this.userSession.UserId;
            }
        }

        public int? SubAccId { get; set; }

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