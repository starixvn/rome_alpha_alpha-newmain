namespace Alpha.NewMain.Site.Controllers
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using Extensions;
    using MediatR;
    using Models.Requests.CustomerProfile;
    using Sunplus.Agent.Model.Entities;

    public class CustomerProfileController : Controller
    {
        private IMediator _mediator;
        private int _custId = 0;

        public CustomerProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        private string CookiesNameToolTip
        {
            get
            {
                return SecretExtension.CfsEnCode(string.Format("ToolTip{0}", _custId), 6);
            }
        }

        private int CountNickNameToolTip
        {
            get
            {
                return Request.Cookies[CookiesNameToolTip] == null ?
                    0 :
                    int.Parse(Request.Cookies[CookiesNameToolTip].Value);
            }

            set
            {
                HttpCookie objCookie = new HttpCookie(CookiesNameToolTip)
                {
                    HttpOnly = true,
                    Value = value.ToString()
                };
                Response.Cookies.Clear();
                Response.AppendCookie(objCookie);
                var timeExpiry = DateTime.Now.AddDays(30);
                Response.Cookies[CookiesNameToolTip].Expires = timeExpiry;
            }
        }

        public ActionResult ViewProfile(ViewProfileQuery query)
        {
            _custId = UserSession.Current.CustId;
            query.CustId = _custId;
            var result = _mediator.Send(query);
            result.CountNicknameTooltip = CountNickNameToolTip;
            if (CountNickNameToolTip < 4)
            {
                CountNickNameToolTip++;
            }

            return View(result);
        }
    }
}