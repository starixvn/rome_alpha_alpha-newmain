namespace Alpha.Main.Site.Handlers
{
    using System;
    using System.Web;
    using Alpha.Main.Site.Models.Requests.CustomerProfile;
    using AppResources;
    using Fanex.Common;
    using Main.Models.CustomerProfiles;
    using MediatR;
    using Services;
    using ViewModels.CustomerProfile;
    using Web.Helpers;

    public class CustomerProfileHandler :
         IRequestHandler<BriefProfileQuery, BriefProfileViewModel>
    {
        private readonly CustomerProfileService customerProfileService;
        private readonly NicknameChecker nicknameChecker;

        public CustomerProfileHandler(
            CustomerProfileService customerProfileService,
            NicknameChecker nicknameChecker)
        {
            this.customerProfileService = customerProfileService;
            this.nicknameChecker = nicknameChecker;
        }

        public BriefProfileViewModel Handle(BriefProfileQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            var profile = customerProfileService.GetProfilePicture(message.CustId);

            var viewModel = new BriefProfileViewModel
            {
                IsDefaultAvatar = profile.UseDefaultAvatar,
                EmbedImage = GenerateImage(profile),
                CustId = message.CustId,
                Username = HttpUtility.HtmlEncode(message.UserName),
                IsInternal = message.IsInternal,
                NickName = BuildNickName(message),
                IntroduceNickNameTooltipCookieName = Cryptography.CfsEnCode($"ToolTip_{message.CustId}"),
                IsEnableOtp = message.IsEnabledOtp,
                IsHideNicknameLink = nicknameChecker.IsOfferForceLoginAndOverDueDate,
                SubAccountTooltip = message.IsSubAcc ? $"{Resources.subAcc} {HttpUtility.HtmlEncode(message.SubUserName)}" : string.Empty
        };

            return viewModel;
        }

        private string BuildNickName(BriefProfileQuery message)
            => message.IsInternal
                ? $"{Resources.admin} {HttpUtility.HtmlEncode(message.AdminName)}"
                : HttpUtility.HtmlEncode(message.NickName);

        private string GenerateImage(ProfilePicture profilePicture)
        {
            if (profilePicture.ImageData != null)
            {
                return $"data:image/jpeg;base64,{Convert.ToBase64String(profilePicture.ImageData)}";
            }

            return string.Empty;
        }
    }
}