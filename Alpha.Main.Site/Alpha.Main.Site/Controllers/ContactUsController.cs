namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;
    using Alpha.Main.Site.ViewModels.ContactUs;
    using Alpha.Web;

    public class ContactUsController : BaseController
    {
        public ActionResult Index()
            => View(new ContactUsViewModel(Request.Headers["Host"]));
    }
}