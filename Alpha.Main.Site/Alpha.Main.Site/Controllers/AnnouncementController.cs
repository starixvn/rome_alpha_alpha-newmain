namespace Alpha.Main.Site.Controllers
{
    using Fanex.Razorbill.Security.Configurations;
    using Fanex.Razorbill.Security.Sessions;
    using MediatR;
    using Models.Enumerations;
    using Models.Requests.Announcement;
    using Sunplus.Agent.Model.Entities;
    using System.Web.Mvc;

    public class AnnouncementController : BaseController
    {
        private readonly IUserSession userSession;
        private IMediator mediator;

        public AnnouncementController(IUserSession userSession, IMediator mediator)
        {
            this.userSession = userSession;
            this.mediator = mediator;
        }

        public ActionResult HotAnnouncement(HotAnnouncementQuery message)
        {
            message.UserSession = this.userSession;
            message.SecurityCodeSession = CookieLessSessionFactory.SecurityCodeSession;
            message.IsSecurityCodeActive = SecurityCodeConfig.IsActive;

            var userLevel = Enumeration.FromValue<UserLevel>(userSession.RoleId);

            var annoucementDataPath = "~/App_Data/Announcements";
            message.PrivateMessagePath = HttpContext.Server.MapPath($"{annoucementDataPath}/PopupAd{userLevel.DisplayName}Neutral.xml");
            message.CommonAnnouncementPath = HttpContext.Server.MapPath($"{annoucementDataPath}/PopupAdCommon.xml");

            var viewModel = mediator.Send(message);
            return View(viewModel);
        }
    }
}