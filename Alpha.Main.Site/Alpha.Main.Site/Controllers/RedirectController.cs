#pragma warning disable 618, 612

namespace Alpha.Main.Site.Controllers
{
    using System;
    using System.Web.Mvc;
    using Configurations;
    using Main.Helpers;
    using Web;

    public class RedirectController : BaseController
    {
        private ActionResult Index(string siteAppPath, string url)
        {
            var absolutPath = $"{AppPathHelper.GetAppPath(siteAppPath, "~")}{url}";
            return Redirect(absolutPath);
        }

        public ActionResult SiteMemberInfo(Uri url) => Index(AppSettings.MemberInfoAppPath, url.ToString());
    }
}