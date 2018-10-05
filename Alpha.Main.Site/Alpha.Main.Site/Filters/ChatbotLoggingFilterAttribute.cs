namespace Alpha.Main.Site.Filters
{
    using System;
    using System.Web.Mvc;
    using Configurations;
    using Fanex.Logging.Extensions;
    using Newtonsoft.Json;
    using Web.Helpers;
    using static Sunplus.Agent.Model.Entities.UserSession;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ChatbotLoggingFilterAttribute : ActionFilterAttribute
    {
        private const string ChatbotUserIdSessionKey = "Alpha.Main.Site.Handlers.ChatBotHandler.UserId";
        private const string LetstalkIdSessionKey = "Alpha.Main.Site.Handlers.ChatBotHandler.LetstalkId";
        private readonly string actionName;
        private readonly SessionManager sessionManager;

        public ChatbotLoggingFilterAttribute(string actionName)
        {
            this.actionName = actionName;
            sessionManager = DependencyResolver.Current.GetService<SessionManager>();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var logMessage = new LogMessage(
                string.Join(
                    "\r\n",
                     $"CustId: {Current.CustId}",
                    $"SubAccountId: {Current.SubAccId}",
                    $"Username: {Current.UserName}",
                    $"UserId: {sessionManager.Get<int>(ChatbotUserIdSessionKey)}",
                    $"LetstalkId: {sessionManager.Get<string>(LetstalkIdSessionKey)}",
                    $"Action Name: {actionName}",
                    $"Params: {JsonConvert.SerializeObject(filterContext.ActionParameters["command"], Formatting.Indented)}",
                    $"Action Id: {filterContext.ActionDescriptor.UniqueId}"))
                    .ToString();

            Fanex.Logging.LogService.Info(logMessage, AppSettings.ChatbotLogCategory);

            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var result = filterContext.Result as JsonResult;
            if (result != null)
            {
                var logMessage = new LogMessage(
                string.Join(
                    "\r\n",
                    $"CustId: {Current.CustId}",
                    $"SubAccountId: {Current.SubAccId}",
                    $"Username: {Current.UserName}",
                    $"UserId: {sessionManager.Get<int>(ChatbotUserIdSessionKey)}",
                    $"LetstalkId: {sessionManager.Get<string>(LetstalkIdSessionKey)}",
                    $"Action Name: {actionName}",
                    $"Result: {JsonConvert.SerializeObject(result.Data, Formatting.Indented)}",
                    $"Action Id: {filterContext.ActionDescriptor.UniqueId}"))
                    .ToString();

                Fanex.Logging.LogService.Info(logMessage, AppSettings.ChatbotLogCategory);
            }

            base.OnActionExecuted(filterContext);
        }
    }
}