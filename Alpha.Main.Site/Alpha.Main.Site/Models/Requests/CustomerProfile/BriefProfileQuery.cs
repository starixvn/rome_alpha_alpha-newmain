namespace Alpha.Main.Site.Models.Requests.CustomerProfile
{
    using MediatR;
    using ViewModels.CustomerProfile;

    public class BriefProfileQuery : IRequest<BriefProfileViewModel>
    {
        public int CustId { get; set; }

        public string UserName { get; set; }

        public string NickName { get; set; }

        public bool IsSubAcc { get; set; }

        public bool IsInternal { get; set; }

        public string SubUserName { get; set; }

        public string AdminName { get; set; }
    }
}