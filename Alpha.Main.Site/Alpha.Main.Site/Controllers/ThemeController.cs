namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;
    using Alpha.Main.Site.Requests.Theme;
    using Alpha.Web;
    using MediatR;

    public class ThemeController : BaseController
    {
        private readonly IMediator mediator;

        public ThemeController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public JsonResult Change(ChangeThemeQuery query)
            => Json(mediator.Send(query));
    }
}