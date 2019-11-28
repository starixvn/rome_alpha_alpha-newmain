namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;
    using Alpha.Main.AppResources;
    using Alpha.Main.Configurations;
    using Alpha.Web;
    using Alpha.Web.Helpers;
    using CaptchaMvc.HtmlHelpers;
    using Sunplus.Agent.Model.Entities;

    public class RobotCaptchaController : BaseController
    {
        private readonly IUserSession userSession;

        public RobotCaptchaController(IUserSession userSession)
        {
            this.userSession = userSession;
        }

        public ActionResult Index()
        {
            if (CanAccessCaptchaPage())
            {
                if (AppSettings.RobotIsIgnoreInputCaptcha)
                {
                    userSession.IsRobot = false;

                    return ToDefaultPage();
                }

                if (userSession.RobotCaptchaFailedCount > AppSettings.RobotMaxCaptchaFailedCount)
                {
                    return ToSignOut();
                }

                userSession.RobotCaptchaFailedCount++;

                return View();
            }

            return ToDefaultPage();
        }

        [HttpPost]
        public ActionResult Check()
        {
            if (this.IsCaptchaValid(null))
            {
                userSession.IsRobot = false;

                return RedirectToAction(nameof(InputCaptchaSuccessfully));
            }

            TempData["ErrorMessage"] = Resources.invalidCaptcha;

            return RedirectToAction(nameof(Index));
        }

        public ActionResult InputCaptchaSuccessfully()
            => ToDefaultPage();

        private bool CanAccessCaptchaPage()
            => !userSession.IsInternal && userSession.IsRobot;

        private ActionResult ToDefaultPage()
            => RedirectToAction("Index", "Default");

        private ActionResult ToSignOut()
            => Redirect(AppPathHelper.GetAppPath(AppSettings.OldMainAppPath, "~/_Authorization/Handlers/SignOut.ashx"));
    }
}