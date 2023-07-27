#pragma warning disable 618, 612

namespace Alpha.Main.Site.Controllers
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using Alpha.Web;
    using Alpha.Web.Filters;
    using AlphaPlus.User.Models;
    using Configurations;
    using MediatR;
    using Models.Requests;
    using Requests;

    public class LanguageController : BaseController
    {
        private readonly IUserSession userSession;
        private readonly IMediator mediator;

        public LanguageController(IMediator mediator, IUserSession userSession)
        {
            if (userSession == null)
            {
                throw new ArgumentNullException(nameof(userSession));
            }

            this.userSession = userSession;
            this.mediator = mediator;
        }

        [ChildActionOnly]
        [AllowProtectedMode]
        public ActionResult List()
        {
            var viewModel = mediator.Send(new AllLanguagesQuery(userSession.Language, AppSettings.DisabledVietnamese));

            return PartialView(viewModel);
        }

        [HttpPost]
        [AllowProtectedMode]
        public JsonResult Change(string languageCode, bool? displayLanguage)
        {
            if (displayLanguage.HasValue && displayLanguage.Value && userSession.IsInternal)
            {
                return Json(null);
            }

            var viewModel = mediator.Send(
                new ChangeLanguageCommand(userSession, languageCode, AppSettings.DisabledVietnamese, displayLanguage ?? false));

            var domain = Request.Headers["Host"];
            var subDomain = domain.Split('.')[0];
            var mainDomain = domain.Replace(subDomain, string.Empty);
            Response.AppendCookie(new HttpCookie("LANGUAGE", viewModel) { Expires = DateTime.Now.AddDays(365), Path = "/", Domain = mainDomain, HttpOnly = false });
            Response.AppendCookie(new HttpCookie("hidLanguage", viewModel) { Expires = DateTime.Now.AddDays(365), Path = "/", Domain = mainDomain, HttpOnly = false });

            return Json(viewModel);
        }
    }
}