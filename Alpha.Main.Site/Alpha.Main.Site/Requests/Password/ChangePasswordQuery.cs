namespace Alpha.Main.Site.Requests.Password
{
    using AlphaPlus.User.Models;
    using MediatR;
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

        public bool IsOfferProtectedMode => userSession.IsOfferProtectedMode;
    }
}