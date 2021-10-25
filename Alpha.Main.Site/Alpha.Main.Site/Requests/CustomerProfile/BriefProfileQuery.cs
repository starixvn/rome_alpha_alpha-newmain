#pragma warning disable 618, 612
namespace Alpha.Main.Site.Models.Requests.CustomerProfile
{
    using MediatR;
    using SecurityCode.Web;
    using Sunplus.Agent.Model.Entities;
    using ViewModels.CustomerProfile;

    public class BriefProfileQuery : IRequest<BriefProfileViewModel>
    {
        private readonly IUserSession userSession;

        public BriefProfileQuery(IUserSession userSession)
        {
            this.userSession = userSession;
        }

        public int CustId
        {
            get
            {
                return this.userSession.CustId;
            }
        }

        public string UserName
        {
            get
            {
                return this.userSession.UserName;
            }
        }

        public string NickName
        {
            get
            {
                return this.userSession.NickName;
            }
        }

        public bool IsSubAcc
        {
            get
            {
                return this.userSession.IsSubAcc;
            }
        }

        public bool IsInternal
        {
            get
            {
                return this.userSession.IsInternal;
            }
        }

        public string SubUserName
        {
            get
            {
                return this.userSession.SubUserName;
            }
        }

        public string AdminName
        {
            get
            {
                return this.userSession.AdminName;
            }
        }

        public bool IsEnabledOtp
        {
            get
            {
                return SecurityCodeSessionManager.SecurityCodeSession.IsEnabledOtp;
            }
        }

        public bool IsProtectedMode => userSession.IsProtectedMode;
    }
}