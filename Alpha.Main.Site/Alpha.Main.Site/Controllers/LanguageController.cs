namespace Alpha.Main.Site.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Constants;
    using Sunplus.Agent.Model.Entities;
    using ViewModels;

    public class LanguageController : BaseController
    {
        private readonly IUserSession userSession;

        public LanguageController(IUserSession userSession)
        {
            this.userSession = userSession ?? UserSession.Current;
        }

        [ChildActionOnly]
        public ActionResult List()
        {
            var viewModel = new LanguageViewModel(Language.CodeNames, userSession.Language);

            return PartialView(viewModel);
        }

        [HttpPost]
        public JsonResult Change(string languageCode)
        {
            if (string.IsNullOrWhiteSpace(languageCode)
                || !Language.CodeNames.Keys.Contains(languageCode))
            {
                languageCode = Language.Default;
            }

            userSession.Language = languageCode;
            userSession.LangKey = Language.CodeKeys[languageCode];            

            return Json(languageCode);
        }
    }
}