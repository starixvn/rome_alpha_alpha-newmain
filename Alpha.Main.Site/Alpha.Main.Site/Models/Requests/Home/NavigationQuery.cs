namespace Alpha.Main.Site.Models.Requests.Home
{
    using Alpha.Main.Site.Models.Views;
    using Enumerations;
    using MediatR;
    using Sunplus.Agent.Model.Entities;

    public class NavigationQuery : IRequest<NavigationViewModel>
    {
        public PageAccess SubPageAccess { get; set; }

        public bool IsSubAcc { get; set; }

        public byte RoleId { get; set; }

        public IUser GetUser()
        {
            var user = new User();
            user.Id = UserSession.Current.CustId;
            user.SubAccountId = UserSession.Current.SubAccId;
            user.Level = Enumeration.FromValue<UserLevel>(RoleId);

            return user;
        }
    }
}