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
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Utilities;
    using ViewModels.CustomerProfile;

    public class CustomerProfileHandler :
         IRequestHandler<BriefProfileQuery, BriefProfileViewModel>
    {
        private const int MaxCountNickNameTooltip = 4;

        private readonly CustomerProfileService customerProfileService;
        private readonly IUserSession userSession;
        private readonly string oldMainRootApp;

        internal CustomerProfileHandler(
            CustomerProfileService customerProfileService,
            IUserSession userSession,
            string oldMainRootApp)
        {
            this.customerProfileService = customerProfileService;
            this.userSession = userSession;
            this.oldMainRootApp = oldMainRootApp ?? AppPathHelper.GetAppPath(AppSettings.OldMainAppPath, "~/");
        }

        public CustomerProfileHandler(
            CustomerProfileService customerProfileService,
            IUserSession userSession) : this(customerProfileService, userSession, null)
        {
        }

        private string CookiesNameToolTip
        {
            get
            {
                return SecretExtension.CfsEnCode(string.Format("ToolTip{0}", userSession.CustId), 6);
            }
        }

        private int CountNickNameToolTip
        {
            get
            {
                var cookiesNameToolTip = HttpContext.Current.Request.Cookies[CookiesNameToolTip];

                return cookiesNameToolTip == null
                    ? 0
                    : int.Parse(cookiesNameToolTip.Value);
            }

            set
            {
                var toolTipCookie = new HttpCookie(CookiesNameToolTip)
                {
                    HttpOnly = true,
                    Value = value.ToString(),
                    Expires = DateTime.Now.AddDays(30)
                };

                HttpContext.Current.Response.Cookies.Clear();
                HttpContext.Current.Response.AppendCookie(toolTipCookie);
            }
        }

        public BriefProfileViewModel Handle(BriefProfileQuery message)
        {
            message.CustId = userSession.CustId;

            var profile = customerProfileService.GetProfilePicture(message.CustId);
            var isShowEditProfilePictureTooltip = profile.IsDefaultAvatar
                && CountNickNameToolTip < MaxCountNickNameTooltip
                && !string.IsNullOrEmpty(userSession.NickName);

            var viewModel = new BriefProfileViewModel
            {
                IsDefaultAvatar = profile.IsDefaultAvatar,
                EmbedImage = GenerateImage(profile),
                IsShowEditProfilePictureTooltip = isShowEditProfilePictureTooltip,
                ChangeNickNameUrl = string.Format("{0}LoginOption?custId={1}", this.oldMainRootApp, message.CustId),
                ViewProfileUrl = string.Format("{0}ProfilePicture?custId={1}", this.oldMainRootApp, message.CustId),
                Username = HttpUtility.HtmlEncode(userSession.UserName),
                IsNotMainAccount = userSession.IsSubAcc || userSession.IsInternal
            };

            if (CountNickNameToolTip < MaxCountNickNameTooltip)
            {
                CountNickNameToolTip++;
            }
            else
            {
                viewModel.IsShowIntroduceNickName = true;
            }

            viewModel.NickName = BuildNickName();

            return viewModel;
        }

        private string BuildNickName()
        {
            if (userSession.IsSubAcc)
            {
                return string.Format("{0} {1}", Resources.subAcc, HttpUtility.HtmlEncode(userSession.SubUserName));
            }
            else if (userSession.IsInternal)
            {
                return string.Format("{0} {1}", Resources.admin, HttpUtility.HtmlEncode(userSession.AdminName));
            }
            else
            {
                return HttpUtility.HtmlEncode(userSession.NickName);
            }
        }

        private string GenerateImage(ProfilePicture profilePicture)
        {
            try
            {
                if (profilePicture.BinaryAvatar != null)
                {
                    return "data:image/jpeg;base64," + Convert.ToBase64String(profilePicture.BinaryAvatar);
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