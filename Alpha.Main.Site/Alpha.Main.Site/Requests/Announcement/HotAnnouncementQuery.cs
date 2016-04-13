namespace Alpha.Main.Site.Models.Requests.Announcement
{
    using Constants;
    using Main.Models.Announcements;
    using Main.Models.Enumerations;
    using MediatR;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Model.Entities.Permission;
    using System.Collections.Generic;
    using System.Web;

    public class HotAnnouncementQuery : IRequest<HotAnnouncement>
    {
        private readonly string commonAnnouncementFilePath;
        private readonly string privateMessageFilePath;
        private readonly IUserSession userSession;
        private readonly IDictionary<int, bool> productsStatus;

        public HotAnnouncementQuery() : this(null, null, null, null)
        {
        }

        internal HotAnnouncementQuery(IDictionary<int, bool> productsStatus, IUserSession userSession, string privateMessageFilePath, string commonAnnouncementFilePath)
        {
            this.productsStatus = productsStatus ?? new Dictionary<int, bool>
                {
                    { AnnouncementId.NotOfferedGoldDeluxe,  UserPermission.IsShowGoldDeluxeSettingForCredit && !UserPermission.IsShowLiveCasino },
                    { AnnouncementId.OfferedGoldDeluxe,  UserPermission.IsShowGoldDeluxeSettingForCredit && UserPermission.IsShowLiveCasino },
                    { AnnouncementId.OfferedSuperLive, UserPermission.IsShowSuperLive },
                    { AnnouncementId.OfferedColossusBets,  UserPermission.IsShowColossusBets }
                };

            this.userSession = userSession ?? UserSession.Current;

            var userLevelName = Enumeration.FromValue<UserLevel>(this.userSession.RoleId);
            this.commonAnnouncementFilePath = commonAnnouncementFilePath ?? HttpContext.Current.Server.MapPath($"~/App_Data/Announcements/PopupAdCommon.xml");
            this.privateMessageFilePath = privateMessageFilePath ?? HttpContext.Current.Server.MapPath($"~/App_Data/Announcements/PopupAd{userLevelName}Neutral.xml");
        }

        public string MessageIds { get; set; }

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

        public int UserId
        {
            get
            {
                return userSession.UserId;
            }
        }

        public int SubAccId
        {
            get
            {
                return userSession.SubAccId;
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