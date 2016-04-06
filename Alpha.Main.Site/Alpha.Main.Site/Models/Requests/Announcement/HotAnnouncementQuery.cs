namespace Alpha.Main.Site.Models.Requests.Announcement
{
    using Announcements;
    using Constants;
    using Fanex.Razorbill.Security.Entities;
    using Fanex.Razorbill.Security.Sessions;
    using MediatR;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Model.Entities.Permission;
    using System.Collections.Generic;

    public class HotAnnouncementQuery : IRequest<HotAnnouncement>
    {
        private IDictionary<int, bool> productsStatus;

        private bool isSecurityCodeActive;

        private ISecurityCodeSession securityCodeSession;

        private IUserSession userSession;

        public HotAnnouncementQuery()
        {
        }

        internal HotAnnouncementQuery(
            IDictionary<int, bool> productsStatus,
            ISecurityCodeSession securityCodeSession,
            IUserSession userSession,
            bool isSecurityCodeActive)
        {
            this.productsStatus = productsStatus;
            this.securityCodeSession = securityCodeSession;
            this.userSession = userSession;
            this.isSecurityCodeActive = isSecurityCodeActive;
        }

        public IUserSession UserSession
        {
            get
            {
                return userSession;
            }

            set
            {
                userSession = value;
            }
        }

        public string MessageIds { get; set; }

        public string AnnouncementResourcePath { get; set; }

        public string PrivateMessagePath { get; set; }

        public string CommonAnnouncementPath { get; set; }

        public bool IsSecurityCodeActive
        {
            get
            {
                return isSecurityCodeActive;
            }

            set
            {
                isSecurityCodeActive = value;
            }
        }

        public bool IsRequireSecurityCode
        {
            get
            {
                return SecurityCodeSession != null ? SecurityCodeSession.IsRequireSecurityCode : false;
            }
        }

        public bool IsInternalSecurityCode
        {
            get
            {
                return SecurityCodeSession != null ? SecurityCodeSession.IsInternal : false;
            }
        }

        public SecurityCodeStatus SecurityCodeResult
        {
            get
            {
                return SecurityCodeSession != null ? (SecurityCodeStatus)SecurityCodeSession.SecurityCodeResult : SecurityCodeStatus.NONE;
            }
        }

        public ISecurityCodeSession SecurityCodeSession
        {
            get
            {
                return securityCodeSession;
            }

            set
            {
                securityCodeSession = value;
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

        public byte RoleId
        {
            get
            {
                return userSession != null ? userSession.RoleId : (byte)0;
            }
        }

        public int AdminId
        {
            get
            {
                return userSession != null ? userSession.AdminId : 0;
            }
        }

        public int CustId
        {
            get
            {
                return userSession != null ? userSession.CustId : 0;
            }
        }

        public string Language
        {
            get
            {
                return userSession != null ? userSession.Language : string.Empty;
            }
        }

        public int UserId
        {
            get
            {
                return userSession != null ? userSession.UserId : 0;
            }
        }

        public string UserName
        {
            get
            {
                return userSession != null ? userSession.UserName : string.Empty;
            }
        }

        public string Site
        {
            get
            {
                return userSession != null ? userSession.Site : string.Empty;
            }
        }
    }
}