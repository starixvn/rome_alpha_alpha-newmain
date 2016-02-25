namespace Alpha.NewMain.Site.Handlers
{
    using System;
    using System.Web;
    using Alpha.NewMain.Site.Models.Requests.CustomerProfile;
    using Extensions;
    using MediatR;
    using Models.Dtos;
    using Models.Views.CustomerProfile;
    using Services;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Utilities;

    public class CustomerProfileHandler :
         IRequestHandler<ViewProfileQuery, ViewProfileViewModel>
    {
        private readonly IDashboardService _dashboardService;

        public CustomerProfileHandler(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        private string CookiesNameToolTip
        {
            get
            {
                return SecretExtension.CfsEnCode(string.Format("ToolTip{0}", UserSession.Current.CustId), 6);
            }
        }

        private int CountNickNameToolTip
        {
            get
            {
                return HttpContext.Current.Request.Cookies[CookiesNameToolTip] == null ?
                    0 :
                    int.Parse(HttpContext.Current.Request.Cookies[CookiesNameToolTip].Value);
            }

            set
            {
                HttpCookie objCookie = new HttpCookie(CookiesNameToolTip)
                {
                    HttpOnly = true,
                    Value = value.ToString()
                };
                HttpContext.Current.Response.Cookies.Clear();
                HttpContext.Current.Response.AppendCookie(objCookie);
                var timeExpiry = DateTime.Now.AddDays(30);
                HttpContext.Current.Response.Cookies[CookiesNameToolTip].Expires = timeExpiry;
            }
        }

        public ViewProfileViewModel Handle(ViewProfileQuery message)
        {
            var profile = _dashboardService.GetProfile(message);
            var result = new ViewProfileViewModel
            {
                IsDefaultAvatar = profile.IsDefaultAvatar,
                EmbedImage = GenerateImage(profile),
                CountNicknameTooltip = CountNickNameToolTip
            };

            if (CountNickNameToolTip < 4)
            {
                CountNickNameToolTip++;
            }

            return result;
        }

        private string GenerateImage(ProfilePictureModel avatar)
        {
            try
            {
                byte[] binaryAvatar = avatar.BinaryAvatar;
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