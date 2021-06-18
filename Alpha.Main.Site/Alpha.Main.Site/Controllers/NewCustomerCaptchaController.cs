namespace Alpha.Main.Site.Controllers
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Web.Mvc;
    using Alpha.Captcha.Models;
    using Alpha.Main.AppResources;
    using Alpha.Main.Configurations;
    using Alpha.Main.Models;
    using Alpha.Web;
    using Fanex.Logging.Extensions.Web;
    using Sunplus.Agent.Model.Entities;

    public class NewCustomerCaptchaController : BaseController
    {
        private readonly string CaptchaInputName = "Captcha";
        private readonly string CaptchaSessionValue = "Alpha.Captcha.Value";

        private readonly IUser user;
        private readonly IUserSession userSession;

        public NewCustomerCaptchaController(IUser user, IUserSession userSession)
        {
            this.user = user;
            this.userSession = userSession;
        }

        public ActionResult Index(string returnUrl)
        {
            ViewData[nameof(returnUrl)] = returnUrl;
            var captchaLevel = GetCaptchaLevel();
            return View(captchaLevel);
        }

        [HttpPost]
        public ActionResult Check(string returnUrl)
        {
            var validateMessage = ValidateCaptcha();
            if (string.IsNullOrEmpty(validateMessage))
            {
                user.RemoveNewCaptchaByCurrency();

                return string.IsNullOrWhiteSpace(returnUrl)
                    ? ToDefaultPage()
                    : Redirect(returnUrl);
            }

            TempData["ErrorMessage"] = $"{Resources.invalidCaptcha} ({validateMessage})";
            var captchaLevel = GetCaptchaLevel();
            return View("~/Views/NewCustomerCaptcha/Index.cshtml", captchaLevel);
        }

        private string ValidateCaptcha()
        {
            var inputCaptcha = Request.Form[CaptchaInputName];
            if (string.IsNullOrEmpty(inputCaptcha))
            {
                return "Captcha is empty";
            }

            var sessionCaptcha = HttpContext.Session[CaptchaSessionValue];
            if (sessionCaptcha == null)
            {
                var message = "Captcha is null";
                WriteLog(message);
                return message;
            }

            var captcha = sessionCaptcha.ToString();
            if (!inputCaptcha.Equals(captcha, StringComparison.OrdinalIgnoreCase))
            {
                var message = $"{inputCaptcha} <> {captcha}";
                WriteLog(message);
                return message;
            }

            return null;
        }

        private void WriteLog(string message)
        {
            if (AppSettings.TrackLogSuperIds.Any(id => id == userSession.SRecommendId))
            {
                var username = string.IsNullOrEmpty(userSession.SubUserName) ? userSession.UserName : userSession.SubUserName;
                ExceptionHandler.Handle(new Exception($"Captcha validate result for customer {username}: {message}"), TraceEventType.Warning);
            }
        }

        private CaptchaLevels GetCaptchaLevel()
        {
            var createdSettingLastDateObject = Session[AppSettings.CreatedSettingLastDateSessionKey];
            if (createdSettingLastDateObject == null)
            {
                return CaptchaLevels.Low;
            }

            var createdSettingLastDate = (DateTime)createdSettingLastDateObject;
            return DateTime.Now.Subtract(createdSettingLastDate).TotalMinutes < 1 ? CaptchaLevels.High : CaptchaLevels.Low;
        }

        private ActionResult ToDefaultPage()
            => RedirectToAction("Index", "Default");
    }
}