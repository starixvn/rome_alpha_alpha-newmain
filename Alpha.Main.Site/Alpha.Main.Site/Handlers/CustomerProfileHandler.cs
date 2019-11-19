namespace Alpha.Main.Site.Handlers
{
    using Alpha.Main.Site.Models.Requests.CustomerProfile;
    using Alpha.ProfilePicture.Services;
    using AppResources;
    using Fanex.Common;
    using MediatR;
    using System;
    using System.Web;
    using ViewModels.CustomerProfile;
    using Web.Helpers;

    public class CustomerProfileHandler :
         IRequestHandler<BriefProfileQuery, BriefProfileViewModel>
    {
        private readonly ProfilePictureServices customerProfileService;
        private readonly NicknameChecker nicknameChecker;

        public CustomerProfileHandler(
            ProfilePictureServices customerProfileService,
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

            var profile = customerProfileService.GetActorProfilePicture(message.CustId);

            var viewModel = new BriefProfileViewModel
            {
                IsDefaultAvatar = string.IsNullOrEmpty(profile.ImageBase64String),
                EmbedImage = profile.ImageBase64String,
                CustId = message.CustId,
                Username = HttpUtility.HtmlEncode(message.UserName),
                IsInternal = message.IsInternal,
                NickName = BuildNickName(message),
                IntroduceNickNameTooltipCookieName = Cryptography.CfsEnCode($"ToolTip_{message.CustId}"),
                IsEnableOtp = message.IsEnabledOtp,
                IsHideNicknameLink = nicknameChecker.IsOfferForceLoginAndOverDueDate,
                SubAccountTooltip = message.IsSubAcc ? $"{Resources.subAcc} {HttpUtility.HtmlEncode(message.SubUserName)}" : string.Empty,
                IsShowProfilePictureTooltip = !profile.Modified.HasValue
        };

            return viewModel;
        }

        private string BuildNickName(BriefProfileQuery message)
            => message.IsInternal
                ? $"{Resources.admin} {HttpUtility.HtmlEncode(message.AdminName)}"
                : HttpUtility.HtmlEncode(message.NickName);
    }
}