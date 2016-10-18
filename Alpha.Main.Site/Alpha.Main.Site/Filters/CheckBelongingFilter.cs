namespace Alpha.Main.Site.Filters
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using Auth.Services;
    using Auth.Services.Impl;
    using Main.Helpers;

    /// <summary>
    /// Ricky: should review more in here, we temporary keep this code.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public class CheckBelongingFilter : AuthorizeAttribute
    {
        private readonly IAuthorizationService authorizationService;

        public CheckBelongingFilter() : this(null)
        {
        }

        internal CheckBelongingFilter(IAuthorizationService authorizationService)
        {
            this.authorizationService = authorizationService ?? new AuthorizationService();
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (!authorizationService.IsBelonging())
            {
                UnAuthorization(filterContext);
            }
        }

        private void UnAuthorization(AuthorizationContext filterContext)
        {
            HttpResponseBase response = filterContext.HttpContext.Response;

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                response.Clear();
                response.Write("{ \"message\": \"You are not authorized on this function!\" }");
                filterContext.Result = new HttpUnauthorizedResult();
                filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);
                response.TrySkipIisCustomErrors = true;
                response.End();
            }
            else
            {
                filterContext.Result = new RedirectResult(MicroSites.OldMainUrl(FormsAuthentication.LoginUrl));
            }

            var session = filterContext.HttpContext.Session;
            session.Clear();
            session.Abandon();
            if (!session.IsCookieless)
            {
                FormsAuthentication.SignOut();
            }
        }
    }
}