namespace Alpha.Main.Site.Controllers
{
    using MediatR;
    using Models.Enumerations;
    using Models.Requests.Announcement;
    using Sunplus.Agent.Model.Entities;
    using System.Web.Mvc;

    public class AnnouncementController : BaseController
    {
        private IMediator _mediator;

        public AnnouncementController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public ActionResult HotAnnouncement(HotAnnouncementQuery message)
        {
            var userLevel = Enumeration.FromValue<UserLevel>(UserSession.Current.RoleId);

            var annoucementDataPath = HttpContext.Server.MapPath("~/App_Data/Announcements");
            message.PrivateMessagePath = HttpContext.Server.MapPath(annoucementDataPath + "PopupAd" + userLevel.DisplayName + "Neutral.xml");
            message.CommonAnnouncementPath = HttpContext.Server.MapPath(annoucementDataPath + "PopupAdCommon.xml");

            var viewModel = _mediator.Send(message);
            return View(viewModel);
        }
    }
}