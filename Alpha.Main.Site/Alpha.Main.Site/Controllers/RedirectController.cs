#pragma warning disable 618, 612

namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;
    using Configurations;
    using Main.Helpers;
    using Web;

    public class RedirectController : BaseController
    {
        private ActionResult Index(string siteAppPath, string url)
        {
            var absolutPath = string.Format("{0}{1}", AppPathHelper.GetAppPath(siteAppPath, "~"), url);
            return Redirect(absolutPath);
        }

        public ActionResult SiteMemberInfo(string url)
        {
            return Index(AppSettings.MemberInfoAppPath, url);
        }
    }
}