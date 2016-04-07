namespace Alpha.Main.Site.Handlers
{
    using System;
    using System.Web;
    using Alpha.Main.Site.Models.Requests.CustomerProfile;
    using App_GlobalResources;
    using Configurations;
    using Fanex.Common;
    using Helpers;
    using MediatR;
    using Models.CustomerProfiles;
    using Services;
    using Sunplus.Agent.Utilities;
    using ViewModels.CustomerProfile;

    public class CustomerProfileHandler :
         IRequestHandler<BriefProfileQuery, BriefProfileViewModel>
    {
        private readonly CustomerProfileService customerProfileService;
        private readonly string oldMainRootApp;

        private int custId;

        internal CustomerProfileHandler(
            CustomerProfileService customerProfileService,
            string oldMainRootApp)
        {
            this.customerProfileService = customerProfileService;
            this.oldMainRootApp = oldMainRootApp ?? AppPathHelper.GetAppPath(AppSettings.OldMainAppPath, "~/");
        }

        public CustomerProfileHandler(
            CustomerProfileService customerProfileService) : this(customerProfileService, null)
        {
        }

        public BriefProfileViewModel Handle(BriefProfileQuery message)
        {
            this.custId = message.CustId;

            var profile = customerProfileService.GetProfilePicture(message.CustId);

            var viewModel = new BriefProfileViewModel
            {
                IsDefaultAvatar = profile.IsDefaultAvatar,
                EmbedImage = GenerateImage(profile),
                CustId = message.CustId,
                Username = HttpUtility.HtmlEncode(message.UserName),
                LoginUserIsNotMainAccount = message.IsSubAcc || message.IsInternal,
                OldMainRootApp = this.oldMainRootApp,
                NickName = BuildNickName(message),
                IntroduceNickNameTooltipCookieName = Cryptography.CfsEnCode($"ToolTip_{custId}")
            };

            return viewModel;
        }

        private string BuildNickName(BriefProfileQuery message)
        {
            if (message.IsSubAcc)
            {
                return $"{Resources.subAcc} {HttpUtility.HtmlEncode(message.SubUserName)}";
            }
            else if (message.IsInternal)
            {
                return $"{Resources.admin} {HttpUtility.HtmlEncode(message.AdminName)}";
            }
            else
            {
                return HttpUtility.HtmlEncode(message.NickName);
            }
        }

        private string GenerateImage(ProfilePicture profilePicture)
        {
            try
            {
                if (profilePicture.BinaryAvatar != null)
                {
                    return $"data:image/jpeg;base64,{Convert.ToBase64String(profilePicture.BinaryAvatar)}";
                }
            }
            catch (Exception ex)
            {
                Logs.Write(ex);
            }

            return string.Empty;
        }
    }
}