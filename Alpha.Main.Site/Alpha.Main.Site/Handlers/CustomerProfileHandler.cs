namespace Alpha.Main.Site.Handlers
{
    using System;
    using System.Web;
    using Alpha.Main.Site.Models.Requests.CustomerProfile;
    using App_GlobalResources;
    using Configurations;
    using Extensions;
    using Helpers;
    using MediatR;
    using Models.CustomerProfiles;
    using Services;
    using Sunplus.Agent.Utilities;
    using ViewModels.CustomerProfile;

    public class CustomerProfileHandler :
         IRequestHandler<BriefProfileQuery, BriefProfileViewModel>
    {
        private const int MaxCountNickNameTooltip = 4;

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

        private int CountNickNameToolTip
        {
            get
            {
                var cookiesNameToolTip = HttpContext.Current.Request.Cookies[GetCookiesNameToolTip(this.custId)];

                return cookiesNameToolTip == null
                    ? 0
                    : int.Parse(cookiesNameToolTip.Value);
            }

            set
            {
                var toolTipCookie = new HttpCookie(GetCookiesNameToolTip(this.custId))
                {
                    HttpOnly = true,
                    Value = value.ToString(),
                    Expires = DateTime.Now.AddDays(30)
                };

                HttpContext.Current.Response.Cookies.Clear();
                HttpContext.Current.Response.AppendCookie(toolTipCookie);
            }
        }

        private string GetCookiesNameToolTip(int custId)
        {
            return SecretExtension.CfsEnCode($"ToolTip{custId}", 6);
        }

        public BriefProfileViewModel Handle(BriefProfileQuery message)
        {
            this.custId = message.CustId;

            var profile = customerProfileService.GetProfilePicture(message.CustId);
            var isShowEditProfilePictureTooltip = profile.IsDefaultAvatar
                && CountNickNameToolTip < MaxCountNickNameTooltip
                && !string.IsNullOrEmpty(message.NickName);

            var viewModel = new BriefProfileViewModel
            {
                IsDefaultAvatar = profile.IsDefaultAvatar,
                EmbedImage = GenerateImage(profile),
                IsShowEditProfilePictureTooltip = isShowEditProfilePictureTooltip,
                ChangeNickNameUrl = $"{this.oldMainRootApp}LoginOption?custId={message.CustId}",
                ViewProfileUrl = $"{this.oldMainRootApp}ProfilePicture?custId={message.CustId}",
                Username = HttpUtility.HtmlEncode(message.UserName),
                IsNotMainAccount = message.IsSubAcc || message.IsInternal
            };

            if (CountNickNameToolTip < MaxCountNickNameTooltip)
            {
                CountNickNameToolTip++;
            }
            else
            {
                viewModel.IsShowIntroduceNickName = true;
            }

            viewModel.NickName = BuildNickName(message);

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