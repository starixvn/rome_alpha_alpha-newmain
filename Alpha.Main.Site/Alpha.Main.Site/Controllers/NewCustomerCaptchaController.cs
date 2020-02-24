namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;
    using Alpha.Main.AppResources;
    using Alpha.Main.Models;
    using Alpha.Web;
    using CaptchaMvc.HtmlHelpers;

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

            return View();
        }

        [HttpPost]
        public ActionResult Check(string returnUrl)
        {
            if (this.IsCaptchaValid(null))
            {
                user.SetNewCaptchaByCurrency();

                return string.IsNullOrWhiteSpace(returnUrl)
                    ? ToDefaultPage()
                    : Redirect(returnUrl);
            }

            TempData["ErrorMessage"] = Resources.invalidCaptcha;

            return RedirectToAction(nameof(Index), new { returnUrl });
        }

        private ActionResult ToDefaultPage()
            => RedirectToAction("Index", "Default");
    }
}