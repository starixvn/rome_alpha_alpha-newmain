namespace Alpha.Main.Site.Models.Requests.Announcement
{
    using Dtos.Announcement;
    using Fanex.Razorbill.Security.Configurations;
    using Fanex.Razorbill.Security.Entities;
    using Fanex.Razorbill.Security.Sessions;
    using MediatR;

    public class HotAnnouncementQuery : IRequest<HotAnnouncement>
    {
        public string MessageIds { get; set; }

        public string AnnouncementResourcePath { get; set; }

        public string SuperPrivateMessagePath { get; set; }

        public string MasterPrivateMessagePath { get; set; }

        public string AgentPrivateMessagePath { get; set; }

        public string CommonAnnouncementPath { get; set; }

        public bool IsSecurityCodeActive
        {
            get
            {
                return SecurityCodeConfig.IsActive;
            }
        }

        public bool IsRequireSecurityCode
        {
            get
            {
                return SecurityCodeSession.IsRequireSecurityCode;
            }
        }

        public bool IsInternalSecurityCode
        {
            get
            {
                return SecurityCodeSession.IsInternal;
            }
        }

        public SecurityCodeStatus SecurityCodeResult
        {
            get
            {
                return (SecurityCodeStatus)SecurityCodeSession.SecurityCodeResult;
            }
        }

        public ISecurityCodeSession SecurityCodeSession
        {
            get
            {
                return CookieLessSessionFactory.SecurityCodeSession;
            }
        }
    }
}