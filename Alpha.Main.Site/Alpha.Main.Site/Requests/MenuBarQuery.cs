namespace Alpha.Main.Site.Models.Requests
{
    using Main.Models.Enumerations;
    using MediatR;
    using ViewModels;

    public class MenuBarQuery : IRequest<MenuBarViewModel>
    {
        private byte roleId;

        public MenuBarQuery(byte roleId)
        {
            this.roleId = roleId;
        }

        public UserLevel UserLevel
        {
            get
            {
                return Enumeration.FromValue<UserLevel>(this.roleId);
            }
        }
    }
}