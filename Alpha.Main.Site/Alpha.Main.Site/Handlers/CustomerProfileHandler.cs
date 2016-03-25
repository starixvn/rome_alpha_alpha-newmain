namespace Alpha.Main.Site.Handlers
{
    using System;
    using System.Web;
    using Alpha.Main.Site.Models.Requests.CustomerProfile;
    using App_GlobalResources;
    using Extensions;
    using MediatR;
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

        private readonly IDashboardService _dashboardService;
        private readonly AppPathService _appPathService;
        private readonly IUserSession _userSession;

        public CustomerProfileHandler(
            IDashboardService dashboardService,
            AppPathService appPathService,
            IUserSession userSession)
        {
            _dashboardService = dashboardService;
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
            var profile = _dashboardService.GetProfile(message);
            var isShowEditProfilePictureTooltip = profile.IsDefaultAvatar
                && CountNickNameToolTip < MaxCountNickNameTooltip
                && !string.IsNullOrEmpty(_userSession.NickName);

            var viewModel = new BriefProfileViewModel
            {
                IsDefaultAvatar = profile.IsDefaultAvatar,
                EmbedImage = GenerateImage(profile.BinaryAvatar),
                IsShowEditProfilePictureTooltip = isShowEditProfilePictureTooltip,
                ChangeNickNameUrl = string.Format("/{0}LoginOption?custId={1}", baseOldMainApp, _userSession.CustId),
                ViewProfileUrl = string.Format("/{0}ProfilePicture?custId={1}", baseOldMainApp, _userSession.CustId),
                Username = HttpUtility.HtmlEncode(_userSession.UserName),
                IsNotMainAccount = _userSession.IsSubAcc || _userSession.IsInternal,
                IsShowIntroduceNickName = CountNickNameToolTip++ < MaxCountNickNameTooltip
            };

            BuildNickName(viewModel);

            return viewModel;
        }

        private void BuildNickName(BriefProfileViewModel viewModel)
        {
            if (_userSession.IsSubAcc)
            {
                viewModel.NickName = string.Format("{0} {1}", Resources.subAcc, HttpUtility.HtmlEncode(_userSession.SubUserName));
            }
            else if (_userSession.IsInternal)
            {
                viewModel.NickName = string.Format("{0} {1}", Resources.admin, HttpUtility.HtmlEncode(_userSession.AdminName));
            }
            else
            {
                viewModel.NickName = HttpUtility.HtmlEncode(_userSession.NickName);
            }
        }

        private string GenerateImage(byte[] binaryAvatar)
        {
            try
            {
                if (binaryAvatar != null)
                {
                    return "data:image/jpeg;base64," + Convert.ToBase64String(binaryAvatar);
                }
            }
            catch (Exception ex)
            {
                Logs.Write(ex);
            }

            return "data:image/jpeg;base64," + string.Empty;
        }
    }
}