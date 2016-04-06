namespace Alpha.Main.Site.Models.Requests.Announcement
{
    using System.Collections.Generic;
    using System.Web;
    using Announcements;
    using Constants;
    using Enumerations;
    using Fanex.Razorbill.Security.Configurations;
    using Fanex.Razorbill.Security.Entities;
    using Fanex.Razorbill.Security.Sessions;
    using MediatR;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Model.Entities.Permission;

    public class HotAnnouncementQuery : IRequest<HotAnnouncement>
    {
        private readonly IDictionary<int, bool> productsStatus;
        private readonly bool isSecurityCodeActive;
        private readonly ISecurityCodeSession securityCodeSession;
        private readonly IUserSession userSession;
        private readonly string commonAnnouncementFilePath;
        private readonly string privateMessageFilePath;

        public HotAnnouncementQuery() : this(null, null, null, null, null, null)
        {
        }

        internal HotAnnouncementQuery(
            IDictionary<int, bool> productsStatus,
            ISecurityCodeSession securityCodeSession,
            IUserSession userSession,
            bool? isSecurityCodeActive,
            string commonAnnouncementFilePath,
            string privateMessageFilePath)
        {
            this.productsStatus = productsStatus ?? new Dictionary<int, bool>
                {
                    { MessageId.NotOfferedGoldDeluxe,  UserPermission.IsShowGoldDeluxeSettingForCredit && !UserPermission.IsShowLiveCasino },
                    { MessageId.OfferedGoldDeluxe,  UserPermission.IsShowGoldDeluxeSettingForCredit && UserPermission.IsShowLiveCasino },
                    { MessageId.OfferedSuperLive, UserPermission.IsShowSuperLive },
                    { MessageId.OfferedColossusBets,  UserPermission.IsShowColossusBets }
                };
            this.securityCodeSession = securityCodeSession ?? CookieLessSessionFactory.SecurityCodeSession;
            this.userSession = userSession ?? UserSession.Current;
            this.isSecurityCodeActive = isSecurityCodeActive.HasValue ? isSecurityCodeActive.Value : SecurityCodeConfig.IsActive;
            this.commonAnnouncementFilePath = commonAnnouncementFilePath ?? HttpContext.Current.Server.MapPath($"~/App_Data/Announcements/PopupAdCommon.xml");
            var userLevelName = Enumeration.FromValue<UserLevel>(this.userSession.RoleId);
            this.privateMessageFilePath = privateMessageFilePath ?? HttpContext.Current.Server.MapPath($"~/App_Data/Announcements/PopupAd{userLevelName}Neutral.xml");
        }

        public string MessageIds { get; set; }

        public string AnnouncementResourcePath { get; set; }

        public string PrivateMessagePath
        {
            get
            {
                return privateMessageFilePath;
            }
        }

        public string CommonAnnouncementPath
        {
            get
            {
                return commonAnnouncementFilePath;
            }
        }

        public bool IsSecurityCodeActive
        {
            get
            {
                return isSecurityCodeActive;
            }
        }

        public bool IsRequireSecurityCode
        {
            get
            {
                return securityCodeSession.IsRequireSecurityCode;
            }
        }

        public bool IsInternalSecurityCode
        {
            get
            {
                return securityCodeSession.IsInternal;
            }
        }

        public SecurityCodeStatus SecurityCodeResult
        {
            get
            {
                return (SecurityCodeStatus)securityCodeSession.SecurityCodeResult;
            }
        }

        public ISecurityCodeSession SecurityCodeSession
        {
            get
            {
                return securityCodeSession;
            }
        }

        public IDictionary<int, bool> ProductsStatus
        {
            get
            {
                return productsStatus;
            }
        }

        public byte RoleId
        {
            get
            {
                return userSession.RoleId;
            }
        }

        public int AdminId
        {
            get
            {
                return userSession.AdminId;
            }
        }

        public int CustId
        {
            get
            {
                return userSession.CustId;
            }
        }

        public string Language
        {
            get
            {
                return userSession.Language;
            }
        }

        public int UserId
        {
            get
            {
                return userSession.UserId;
            }
        }

        public string UserName
        {
            get
            {
                return userSession.UserName;
            }
        }

        public string Site
        {
            get
            {
                return userSession.Site;
            }
        }
    }
}