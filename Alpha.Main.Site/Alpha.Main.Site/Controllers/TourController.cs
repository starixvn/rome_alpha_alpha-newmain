using System.Web.Mvc;
using Alpha.Main.Site.ViewModels.Tour;
using Alpha.Web;
using Alpha.Web.Models;
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

        public ActionResult Index()
        {
            // Create Direct Member Tour
            var actorLevel = Enumeration.FromValue<UserLevel>(userSession.RoleId);

            if ((actorLevel.IsSuperLevel() || actorLevel.IsMasterLevel()) && userSession.SubPageAccess.CreateAccount)
            {
                return View(new TourViewModel(userSession.Language, userSession.RoleId));
            }

            return new EmptyResult();
        }

        public ActionResult ProfilePicture()
        {
            var viewModel = new TourViewModel(userSession.Language, userSession.RoleId);

            return View(viewModel);
        }
    }
}