namespace Alpha.NewMain.Site.Models.Requests.CustomerProfile
{
    using MediatR;
    using Views.CustomerProfile;
    public class ViewProfileQuery : IRequest<ViewProfileViewModel>
    {
        public int CustId { get; set; }
    }
}