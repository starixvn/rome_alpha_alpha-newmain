namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Models.Requests.Announcement;

    public class AnnouncementController : BaseController
    {
        private IMediator _mediator;

        public AnnouncementController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public ActionResult HotAnnouncement(HotAnnouncementQuery message)
        {
            var announcementResourcePath = "../Content/Announcement/Resources/";
            message.SuperPrivateMessagePath = HttpContext.Server.MapPath(announcementResourcePath + "PopupAdSuperNeutral.xml");
            message.MasterPrivateMessagePath = HttpContext.Server.MapPath(announcementResourcePath + "PopupAdMasterNeutral.xml");
            message.AgentPrivateMessagePath = HttpContext.Server.MapPath(announcementResourcePath + "PopupAdAgentNeutral.xml");
            message.CommonAnnouncementPath = HttpContext.Server.MapPath(announcementResourcePath + "PopupAdCommon.xml");

            var viewModel = _mediator.Send(message);
            return View(viewModel);
        }
    }
}