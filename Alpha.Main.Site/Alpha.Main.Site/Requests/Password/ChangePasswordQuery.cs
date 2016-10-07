namespace Alpha.Main.Site.Requests.Password
{
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;
    using ViewModels.Security;

    public class ChangePasswordQuery : IRequest<PasswordViewModel>
    {
        private readonly IUserSession userSession;

        public ChangePasswordQuery() : this(null)
        {
        }

        internal ChangePasswordQuery(IUserSession userSession)
        {
            this.userSession = userSession ?? new UserSessionExt();
        }

        public byte CustRoleId
        {
            get
            {
                return this.userSession.RoleId;
            }
        }
    }
}