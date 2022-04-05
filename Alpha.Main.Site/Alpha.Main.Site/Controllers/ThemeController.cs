namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;
    using Alpha.Main.Site.Requests.Theme;
    using Alpha.Web;
    using Alpha.Web.Filters;
    using MediatR;

    public class ThemeController : BaseController
    {
        private readonly IMediator mediator;

        public ThemeController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        [AllowProtectedMode]
        public JsonResult Change(ChangeThemeQuery query)
            => Json(mediator.Send(query));

        [HttpPost]
        [AllowProtectedMode]
        public JsonResult ResetAssetAppPath(ResetAssetAppPathQuery query)
            => Json(mediator.Send(query));
    }
}