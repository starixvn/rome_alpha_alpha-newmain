namespace Alpha.Main.Site.Controllers
{
    using System.Globalization;
    using System.Text;
    using System.Threading;
    using System.Web.Mvc;
    using System.Web.Mvc.Filters;
    using System.Web.Routing;
    using System.Web.Security;
    using Constants;
    using Services;
    using Services.Impl;
    using Sunplus.Agent.Model.Entities;

    public class BaseController : Controller
    {
        private CultureInfo _defaultCulture = CultureInfo.CreateSpecificCulture(LanguageConstant.DefaultLanguage);

        private AppPathService _appPathService = new AppPathServiceImpl();

        protected override void OnAuthentication(AuthenticationContext filterContext)
        {
            base.OnAuthentication(filterContext);

            if (UserSession.Current.CustId == 0)
            {
                var logInUrl = "/" + _appPathService.GetAppPath(AppSettings.OldMainApp, FormsAuthentication.LoginUrl);
                Response.Redirect(logInUrl, true);
                return;
            }
        }

        protected override JsonResult Json(
            object data,
            string contentType,
            Encoding contentEncoding,
            JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = int.MaxValue
            };
        }

        protected override void Initialize(RequestContext requestContext)
        {
            var cultureName = UserSession.Current.Language;

            if (string.IsNullOrEmpty(cultureName))
            {
                cultureName = LanguageConstant.DefaultLanguage;
                UserSession.Current.Language = cultureName;
            }

            // Modify current thread's cultures
            Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            Thread.CurrentThread.CurrentCulture.DateTimeFormat = _defaultCulture.DateTimeFormat;
            Thread.CurrentThread.CurrentCulture.NumberFormat = _defaultCulture.NumberFormat;

            base.Initialize(requestContext);
        }
    }
}