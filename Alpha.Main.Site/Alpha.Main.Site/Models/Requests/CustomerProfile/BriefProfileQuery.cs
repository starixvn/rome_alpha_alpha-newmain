namespace Alpha.Main.Site.Models.Requests.CustomerProfile
{
    using MediatR;
    using ViewModels.CustomerProfile;

    public class BriefProfileQuery : IRequest<BriefProfileViewModel>
    {
        public int CustId { get; set; }
    }
}