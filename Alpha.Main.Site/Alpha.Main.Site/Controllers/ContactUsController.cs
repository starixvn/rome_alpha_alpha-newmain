namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;
    using Alpha.Main.Site.ViewModels.ContactUs;
    using Alpha.Web;
    using AlphaPlus.User.Models;

    public class ContactUsController : BaseController
    {
        private readonly IUserSession userSession;

        public ContactUsController(IUserSession userSession)
        {
            this.userSession = userSession;
        }

        public ActionResult Index()
            => View(new ContactUsViewModel(Request.Headers["Host"], userSession));
    }
}