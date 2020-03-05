namespace Alpha.Main.Site.Controllers
{
    using Alpha.Captcha.Models;
    using Alpha.Captcha.Mvc;
    using Alpha.Main.AppResources;
    using Alpha.Main.Configurations;
    using Alpha.Main.Models;
    using Alpha.Web;
    using System;
    using System.Web.Mvc;

    public class NewCustomerCaptchaController : BaseController
    {
        private readonly IUser user;

        public NewCustomerCaptchaController(IUser user)
        {
            this.user = user;
        }

        public ActionResult Index(string returnUrl)
        {
            ViewData[nameof(returnUrl)] = returnUrl;
            var captchaLevel = GetCaptchaLevel();
            return View(captchaLevel);
        }

        [HttpPost]
        [CaptchaValidation]
        public ActionResult Check(string returnUrl)
        {
            if (ModelState.IsValid)
            {
                user.RemoveNewCaptchaByCurrency();

                return string.IsNullOrWhiteSpace(returnUrl)
                    ? ToDefaultPage()
                    : Redirect(returnUrl);
            }

            TempData["ErrorMessage"] = Resources.invalidCaptcha;

            return View("~/Views/NewCustomerCaptcha/Index.cshtml");
        }

        private CaptchaLevels GetCaptchaLevel()
        {
            var createdSettingLastDateObject = Session[AppSettings.CreatedSettingLastDateSessionKey];
            if (createdSettingLastDateObject == null)
            {
                return CaptchaLevels.Medium;
            }

            var createdSettingLastDate = (DateTime)createdSettingLastDateObject;
            return DateTime.Now.Subtract(createdSettingLastDate).TotalMinutes < 1 ? CaptchaLevels.Extreme : CaptchaLevels.Medium;
        }

        private ActionResult ToDefaultPage()
            => RedirectToAction("Index", "Default");
    }
}