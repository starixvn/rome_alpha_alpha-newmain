namespace Alpha.Main.Site.Models.Requests.Home
{
    using Alpha.Main.Site.Models.Views;
    using MediatR;
    using Sunplus.Agent.Model.Entities;

    public class NavigationQuery : IRequest<NavigationViewModel>
    {
        public PageAccess SubPageAccess { get; set; }

        public bool IsSubAcc { get; set; }

        public byte RoleId { get; set; }
    }
}