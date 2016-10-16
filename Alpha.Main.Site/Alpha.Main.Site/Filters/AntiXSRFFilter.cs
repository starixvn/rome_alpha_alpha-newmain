namespace Alpha.Main.Site.Filters
{
    using System;
    using System.Web.Mvc;
    using Alpha.Auth.Web.Validators;
    using Main.Models;

    /// <summary>
    /// Ricky: should review more in here, we temporary keep this code.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class AntiXsrfFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var antiXSRFResult = AntiForgeryValidator.ValidateRequest();
            if (antiXSRFResult.ErrCode != 0)
            {
                ActionDenied(filterContext, antiXSRFResult);
            }

            base.OnActionExecuting(filterContext);
        }

        private static void ActionDenied(ActionExecutingContext filterContext, Alpha.Auth.Models.Result antiXSRFResult)
        {
            var request = filterContext.HttpContext.Request;
            if (request.IsAjaxRequest())
            {
                filterContext.Result = new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new Result
                    {
                        Code = antiXSRFResult.ErrCode,
                        Message = antiXSRFResult.ErrMsg
                    }
                };
            }
            else
            {
                filterContext.Result = new ViewResult
                {
                    ViewName = "~/Areas/CustomerSetting/Views/Security/AccessDenied.cshtml"
                };
            }
        }
    }
}