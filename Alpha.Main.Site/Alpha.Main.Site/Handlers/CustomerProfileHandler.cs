namespace Alpha.Main.Site.Handlers
{
    using Alpha.Main.ProfilePicture.Services;
    using Alpha.Main.Site.Models.Requests.CustomerProfile;
    using AppResources;
    using Fanex.Common;
    using MediatR;
    using Sunplus.Agent.Model.Entities;
    using System;
    using System.Web;
    using ViewModels.CustomerProfile;
    using Web.Helpers;

    public class CustomerProfileHandler :
         IRequestHandler<BriefProfileQuery, BriefProfileViewModel>
    {
        private readonly ProfilePictureServices customerProfileService;
        private readonly NicknameChecker nicknameChecker;
        private readonly IUserSession userSession;

        public CustomerProfileHandler(
            ProfilePictureServices customerProfileService,
            IUserSession userSession,
            NicknameChecker nicknameChecker)
        {
            this.customerProfileService = customerProfileService;
            this.nicknameChecker = nicknameChecker;
            this.userSession = userSession;
        }

        public BriefProfileViewModel Handle(BriefProfileQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            var profile = customerProfileService.GetActorProfilePicture(userSession.CustId);
            var viewModel = new BriefProfileViewModel
            {
                IsDefaultAvatar = string.IsNullOrEmpty(profile.ImageBase64String),
                EmbedImage = profile.ImageBase64String,
                ProfileImageId = profile.ImageId,
                CustId = message.CustId,
                Username = HttpUtility.HtmlEncode(message.UserName),
                IsInternal = message.IsInternal,
                NickName = BuildNickName(message),
                IntroduceNickNameTooltipCookieName = Cryptography.CfsEnCode($"ToolTip_{message.CustId}"),
                IsEnableOtp = message.IsEnabledOtp,
                IsHideNicknameLink = nicknameChecker.IsOfferForceLoginAndOverDueDate,
                SubAccountTooltip = message.IsSubAcc ? $"{Resources.subAcc} {HttpUtility.HtmlEncode(message.SubUserName)}" : string.Empty,
                IsShowProfilePictureTooltip = !profile.Modified.HasValue,
        };

            return viewModel;
        }

        private string BuildNickName(BriefProfileQuery message)
            => message.IsInternal
                ? $"{Resources.admin} {HttpUtility.HtmlEncode(message.AdminName)}"
                : HttpUtility.HtmlEncode(message.NickName);
    }
}