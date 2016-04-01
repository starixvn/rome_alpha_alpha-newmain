namespace Alpha.Main.Site.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Constants;
    using Fanex.CustomerSetting.Module.Core.Web;
    using Sunplus.Agent.Model.Entities;
    using ViewModels;

    public class LanguageController : BaseController
    {
        [ChildActionOnly]
        public ActionResult List()
        {
            var viewModel = new LanguageViewModel();

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

            UserSession.Current.Language = languageCode;
            UserSession.Current.LangKey = Language.CodeKeys[languageCode];
            AppSession.Language = languageCode;

            return Json(languageCode);
        }
    }
}