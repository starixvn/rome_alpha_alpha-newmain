namespace Alpha.Main.Site.Handlers
{
    using System;
    using System.Web;
    using System.Web.Http.Routing;
    using Alpha.Main.Site.Models.Requests.CustomerProfile;
    using App_GlobalResources;
    using Extensions;
    using MediatR;
    using Models.CustomerProfiles;
    using Models.Dtos;
    using Models.Views.CustomerProfile;
    using Services;
    using Services.Impl;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Utilities;

    public class CustomerProfileHandler :
         IRequestHandler<BriefProfileQuery, BriefProfileViewModel>
    {
        private const int MaxCountNickNameTooltip = 4;

        private readonly CustomerProfileService _customerProfileService;
        private readonly AppPathService _appPathService;
        private readonly IUserSession _userSession;

        public CustomerProfileHandler(
            CustomerProfileService customerProfileService,
            AppPathService appPathService,
            IUserSession userSession)
        {
            _customerProfileService = customerProfileService;
            _appPathService = appPathService;
            _userSession = userSession;
        }

        private string CookiesNameToolTip
        {
            get
            {
                return SecretExtension.CfsEnCode(string.Format("ToolTip{0}", _userSession.CustId), 6);
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
            var baseOldMainApp = _appPathService.GetAppPath(AppSettings.OldMainApp, "~");
            var profile = _customerProfileService.GetProfilePicture(message.CustId);
            var isShowEditProfilePictureTooltip = profile.IsDefaultAvatar
                && CountNickNameToolTip < MaxCountNickNameTooltip
                && !string.IsNullOrEmpty(_userSession.NickName);

            var viewModel = new BriefProfileViewModel
            {
                IsDefaultAvatar = profile.IsDefaultAvatar,
                EmbedImage = GenerateImage(profile),
                IsShowEditProfilePictureTooltip = isShowEditProfilePictureTooltip,
                ChangeNickNameUrl = string.Format("/{0}LoginOption?custId={1}", baseOldMainApp, _userSession.CustId),
                ViewProfileUrl = string.Format("/{0}ProfilePicture?custId={1}", baseOldMainApp, _userSession.CustId),
                Username = HttpUtility.HtmlEncode(_userSession.UserName),
                IsNotMainAccount = _userSession.IsSubAcc || _userSession.IsInternal
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
            if (_userSession.IsSubAcc)
            {
                return string.Format("{0} {1}", Resources.subAcc, HttpUtility.HtmlEncode(_userSession.SubUserName));
            }
            else if (_userSession.IsInternal)
            {
                return string.Format("{0} {1}", Resources.admin, HttpUtility.HtmlEncode(_userSession.AdminName));
            }
            else
            {
                return HttpUtility.HtmlEncode(_userSession.NickName);
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