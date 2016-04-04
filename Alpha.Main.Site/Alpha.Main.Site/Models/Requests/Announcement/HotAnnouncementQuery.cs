namespace Alpha.Main.Site.Models.Requests.Announcement
{
    using System.Collections.Generic;
    using Announcements;
    using Constants;
    using Fanex.Razorbill.Security.Configurations;
    using Fanex.Razorbill.Security.Entities;
    using Fanex.Razorbill.Security.Sessions;
    using MediatR;
    using Sunplus.Agent.Model.Entities.Permission;

    public class HotAnnouncementQuery : IRequest<HotAnnouncement>
    {
        private IDictionary<int, bool> productsStatus;

        public HotAnnouncementQuery()
        {
        }

        internal HotAnnouncementQuery(IDictionary<int, bool> productsStatus)
        {
            this.productsStatus = productsStatus;
        }

        public string MessageIds { get; set; }

        public string AnnouncementResourcePath { get; set; }

        public string PrivateMessagePath { get; set; }

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

        public IDictionary<int, bool> ProductsStatus
        {
            get
            {
                var productsStatus = this.productsStatus ?? new Dictionary<int, bool>
                {
                    { MessageId.NotOfferedGoldDeluxe,  UserPermission.IsShowGoldDeluxeSettingForCredit && !UserPermission.IsShowLiveCasino },
                    { MessageId.OfferedGoldDeluxe,  UserPermission.IsShowGoldDeluxeSettingForCredit && UserPermission.IsShowLiveCasino },
                    { MessageId.OfferedSuperLive, UserPermission.IsShowSuperLive },
                    { MessageId.OfferedColossusBets,  UserPermission.IsShowColossusBets }
                };

                return productsStatus;
            }
        }
    }
}