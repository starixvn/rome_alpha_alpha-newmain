namespace Alpha.Main.Site.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Web.Mvc;
    using Alpha.Captcha.Models;
    using Alpha.Captcha.Services;
    using Alpha.Main.AppResources;
    using Alpha.Main.Configurations;
    using Alpha.Main.Models;
    using Alpha.Web;
    using Fanex.Logging.Extensions.Web;
    using Sunplus.Agent.Model.Entities;

    public class NewCustomerCaptchaController : BaseController
    {
        private readonly string CaptchaInputName = "c_v";
        private readonly string CaptchaFunctionName = "c_f";

        private readonly IUser user;
        private readonly IUserSession userSession;
        private readonly ISessionService sessionService;

        public NewCustomerCaptchaController(IUser user, IUserSession userSession, ISessionService sessionService)
        {
            this.user = user;
            this.userSession = userSession;
            this.sessionService = sessionService;
        }

        [HttpGet]
        public ActionResult Index(string returnUrl)
        {
            return View(GetCaptchaSetting(returnUrl));
        }

        [HttpPost, ActionName("Index")]
        public ActionResult IndexPost(string returnUrl)
        {
            var validateMessage = ValidateCaptcha();
            if (string.IsNullOrEmpty(validateMessage))
            {
                user.RemoveNewCaptchaByCurrency();

                return string.IsNullOrWhiteSpace(returnUrl)
                    ? ToDefaultPage()
                    : Redirect(returnUrl);
            }

            TempData["ErrorMessage"] = validateMessage;
            return View("~/Views/NewCustomerCaptcha/Index.cshtml", GetCaptchaSetting(returnUrl));
        }

        private string ValidateCaptcha()
        {
            var inputCaptcha = Request.Form[CaptchaInputName];
            if (string.IsNullOrEmpty(inputCaptcha))
            {
                return Resources.CaptchaIsEmptyErrorMessage;
            }

            var captchaSession = sessionService.Take();
            if (captchaSession == null)
            {
                var message = Resources.CaptchaIsNullErrorMessage;
                WriteLog(message);
                return message;
            }

            if (!inputCaptcha.Equals(captchaSession.Text, StringComparison.OrdinalIgnoreCase))
            {
                var message = string.Format(Resources.invalidCaptcha, inputCaptcha.ToUpperInvariant(), captchaSession.Text);
                WriteLog(message);
                return message;
            }

            return null;
        }

        private void WriteLog(string message)
        {
            var captchaFunction = Request.QueryString[CaptchaFunctionName] ?? Request.Form[CaptchaFunctionName];
            if (AppSettings.NewCaptchaLogCurrencies.Any(c => c == userSession.Currency))
            {
                var username = string.IsNullOrEmpty(userSession.SubUserName) ? userSession.UserName : userSession.SubUserName;
                ExceptionHandler.Handle(new Exception($"Captcha validate result: customer={username}, function={captchaFunction}, message={message}"), TraceEventType.Warning);
            }
        }

        private CaptchaSetting GetCaptchaSetting(string returnUrl)
        {
            var captchaLevel = CaptchaLevels.Low;
            var createdSettingLastDateObject = Session[AppSettings.CreatedSettingLastDateSessionKey];
            if (createdSettingLastDateObject != null)
            {
                var createdSettingLastDate = (DateTime)createdSettingLastDateObject;
                if (DateTime.Now.Subtract(createdSettingLastDate).TotalMinutes < 1)
                {
                    captchaLevel = CaptchaLevels.High;
                }
            }

            return new CaptchaSetting
            {
                FunctionName = "newcustomer",
                CaptchaLevel = captchaLevel,
                ButtonClassname = "btn-submit",
                HiddenProps = new Dictionary<string, string>
                {
                    [nameof(returnUrl)] = returnUrl
                }
            };
        }

        private ActionResult ToDefaultPage()
            => RedirectToAction("Index", "Default");
    }
}