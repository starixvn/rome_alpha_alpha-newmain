namespace Alpha.Main.Site.Models.Request.SecurityCode
{
    using Alpha.Main.Models;
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;

    public class UpdateSecurityCodeFunctionCommand : IRequest<Result>
    {
        private readonly IUserSession userSession;

        public UpdateSecurityCodeFunctionCommand() : this(null)
        {
        }

        public UpdateSecurityCodeFunctionCommand(IUserSession userSession)
        {
            this.userSession = userSession ?? new UserSessionExt();
        }

        public bool IsOn { get; set; }

        public string Function { get; set; }

        public int CustId => userSession.CustId;

        public int SubAccId => userSession.SubAccId;

        public string LangKey => userSession.LangKey;

        public string IP => userSession.UserLoginIp;
    }
}