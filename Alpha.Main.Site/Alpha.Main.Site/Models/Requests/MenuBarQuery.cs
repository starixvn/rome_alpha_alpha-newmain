namespace Alpha.Main.Site.Models.Requests
{
    using Enumerations;
    using MediatR;
    using ViewModels;

    public class MenuBarQuery : IRequest<MenuBarViewModel>
    {
        private byte roleId;

        public MenuBarQuery(byte roleId)
        {
            this.roleId = roleId;
        }

        public IUser User
        {
            get
            {
                var user = new User();
                user.Level = Enumeration.FromValue<UserLevel>(this.roleId);

                return user;
            }
        }
    }
}