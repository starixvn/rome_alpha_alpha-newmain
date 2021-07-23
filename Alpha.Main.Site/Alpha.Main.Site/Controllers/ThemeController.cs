namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;
    using Alpha.Web;
    using Sunplus.Agent.Model.Entities;

    public class ThemeController : BaseController
    {
        private readonly IUserSession userSession;

        public ThemeController(IUserSession userSession)
        {
            this.userSession = userSession;
        }

        [HttpPost]
        public JsonResult Change(bool isDarkMode)
        {
            userSession.DarkModeName = isDarkMode ? "-dark" : string.Empty;

            return Json(true);
        }
    }
}