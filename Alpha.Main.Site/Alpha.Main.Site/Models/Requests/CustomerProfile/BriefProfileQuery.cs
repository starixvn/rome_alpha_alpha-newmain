namespace Alpha.Main.Site.Models.Requests.CustomerProfile
{
    using MediatR;
    using Views.CustomerProfile;

    public class BriefProfileQuery : IRequest<BriefProfileViewModel>
    {
        public int CustId { get; set; }
    }
}