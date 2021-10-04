namespace Alpha.Main.Site.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Web.Mvc;
    using Alpha.Captcha.Models;
    using Alpha.Captcha.Mvc;
    using Alpha.Captcha.Services;
    using Alpha.Main.Configurations;
    using Alpha.Main.Models;
    using Alpha.Web;
    using Fanex.Logging.Extensions.Web;
    using Sunplus.Agent.Model.Entities;

    public class NewCustomerCaptchaController : BaseController
    {
        private readonly string CaptchaInputName = "c_v";
        private readonly string CaptchaFunctionName = "c_f";
        private readonly int EmptyCaptchaErrorCode = 1;
        private const string Separator = "; ";

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
            AssignDarkModeViewData(userSession);
            return View(GetCaptchaSetting(returnUrl));
        }

        [HttpPost, ActionName("Index"), CaptchaValidation]
        public ActionResult IndexPost(string returnUrl)
        {
            if (ModelState.IsValid)
            {
                user.RemoveNewCaptchaByCurrency();

                return string.IsNullOrWhiteSpace(returnUrl)
                    ? ToDefaultPage()
                    : Redirect(returnUrl);
            }

            var key = ModelState.Keys.FirstOrDefault(k => k.StartsWith(CaptchaInputName));
            var errorCode = Convert.ToInt32(key.Split('_').Last());
            var errorMessage = string.Join(Separator, ModelState[key].Errors.Select(x => x.ErrorMessage));
            if (errorCode != EmptyCaptchaErrorCode)
            {
                WriteLog(errorMessage);
            }

            AssignDarkModeViewData(userSession);
            TempData["ErrorMessage"] = errorMessage;
            return View("~/Views/NewCustomerCaptcha/Index.cshtml", GetCaptchaSetting(returnUrl));
        }

        private void WriteLog(string message)
        {
            var captchaFunction = Request.QueryString[CaptchaFunctionName] ?? Request.Form[CaptchaFunctionName];
            var writeLogEnabled = AppSettings.NewCaptchaLogCurrencies.Any(c => c == userSession.Currency || c == "*");
            if (writeLogEnabled)
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
                Level = captchaLevel,
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