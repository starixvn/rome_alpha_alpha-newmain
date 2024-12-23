using System.Web.Mvc;
using Alpha.Main.Site.ViewModels.Tour;
using Alpha.Web;
using AlphaPlus.User.Models;

namespace Alpha.Main.Site.Controllers
{
    public class TourController : BaseController
    {
        private readonly IUserSession userSession;

        public TourController(IUserSession userSession)
        {
            this.userSession = userSession;
        }

        public ActionResult ProfilePicture()
        {
            var viewModel = new TourViewModel(userSession.Language, userSession.RoleId);

            return View(viewModel);
        }
    }
}