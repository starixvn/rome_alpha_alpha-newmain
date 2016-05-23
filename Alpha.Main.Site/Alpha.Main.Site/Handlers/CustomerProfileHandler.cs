namespace Alpha.Main.Site.Handlers
{
    using Alpha.Main.Site.Models.Requests.CustomerProfile;
    using AppResources;
    using Fanex.Common;
    using Main.Models.CustomerProfiles;
    using MediatR;
    using Services;
    using System;
    using System.Web;
    using ViewModels.CustomerProfile;

    public class CustomerProfileHandler :
         IRequestHandler<BriefProfileQuery, BriefProfileViewModel>
    {
        private readonly CustomerProfileService customerProfileService;

        public CustomerProfileHandler(CustomerProfileService customerProfileService)
        {
            this.customerProfileService = customerProfileService;
        }

        public BriefProfileViewModel Handle(BriefProfileQuery message)
        {
            var profile = customerProfileService.GetProfilePicture(message.CustId);

            var viewModel = new BriefProfileViewModel
            {
                IsDefaultAvatar = profile.IsDefaultAvatar,
                EmbedImage = GenerateImage(profile),
                CustId = message.CustId,
                Username = HttpUtility.HtmlEncode(message.UserName),
                LoginUserIsNotMainAccount = message.IsSubAcc || message.IsInternal,
                NickName = BuildNickName(message),
                IntroduceNickNameTooltipCookieName = Cryptography.CfsEnCode($"ToolTip_{message.CustId}")
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
            if (profilePicture.BinaryAvatar != null)
            {
                return $"data:image/jpeg;base64,{Convert.ToBase64String(profilePicture.BinaryAvatar)}";
            }

            return string.Empty;
        }
    }
}