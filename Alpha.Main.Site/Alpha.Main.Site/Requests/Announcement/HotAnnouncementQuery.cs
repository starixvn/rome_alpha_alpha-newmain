namespace Alpha.Main.Site.Models.Requests.Announcement
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using Fanex.Razorbill.Security.Configurations;
    using Main.Models.Announcements;
    using Main.Models.Enumerations;
    using MediatR;
    using Params.Announcements;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Model.Entities.Permission;

    public class HotAnnouncementQuery : IRequest<HotAnnouncement>
    {
        private readonly string commonAnnouncementFilePath;
        private readonly string privateMessageFilePath;
        private readonly IUserSession userSession;
        private readonly IDictionary<int, bool> productsStatus;

        public const int OfferedGoldDeluxe = 3543501;
        public const int NotOfferedGoldDeluxe = 3543502;
        public const int OfferedSuperLive = 3577701;
        public const int OfferedColossusBets = 4523901;
        public const int OfferedAllbet = 53088003;
        public const int NotOfferedAllbet = 53088004;

        private bool isSecurityCodeActive;

        public HotAnnouncementQuery() : this(null, null, null, null, null)
        {
        }

        internal HotAnnouncementQuery(
            IDictionary<int, bool> productsStatus,
            IUserSession userSession,
            string privateMessageFilePath,
            string commonAnnouncementFilePath,
            bool? isSecurityCodeActive = null)
        {
            this.productsStatus = productsStatus;
            this.userSession = userSession ?? new UserSessionExt();
            this.commonAnnouncementFilePath = commonAnnouncementFilePath;
            this.privateMessageFilePath = privateMessageFilePath;
            this.isSecurityCodeActive = isSecurityCodeActive.HasValue ? isSecurityCodeActive.Value : SecurityCodeConfig.IsActive;
        }

        public string MessageIds { get; set; }

        public string PrivateMessagePath
        {
            get
            {
                return privateMessageFilePath 
                    ?? HttpContext.Current.Server.MapPath($"~/App_Data/Announcements/PopupAd{Enumeration.FromValue<UserLevel>(this.userSession.RoleId)}Neutral.xml");
            }
        }

        public string CommonAnnouncementPath
        {
            get
            {
                return commonAnnouncementFilePath ?? HttpContext.Current.Server.MapPath($"~/App_Data/Announcements/PopupAdCommon.xml");
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
                return productsStatus ?? new Dictionary<int, bool>
                {
                    { NotOfferedGoldDeluxe,  UserPermission.IsShowGoldDeluxeSettingForCredit && !UserPermission.IsShowLiveCasino },
                    { OfferedGoldDeluxe,  UserPermission.IsShowGoldDeluxeSettingForCredit && UserPermission.IsShowLiveCasino },
                    { OfferedSuperLive, UserPermission.IsShowSuperLive },
                    { OfferedColossusBets,  UserPermission.IsShowColossusBets },
                    { OfferedAllbet,  UserPermission.IsSyncAllBet && UserPermission.IsShowLiveCasino },
                    { NotOfferedAllbet,  UserPermission.IsSyncAllBet && !UserPermission.IsShowLiveCasino }
                };
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

        public bool IsSecurityCodeActive
        {
            get
            {
                return isSecurityCodeActive;
            }
        }

        public void AssignValue(GetHotAnnouncementsParams hotAnnouncementsParams)
        {
            if (hotAnnouncementsParams == null)
            {
                throw new ArgumentNullException(nameof(hotAnnouncementsParams), "Argument cannot be null");
            }

            hotAnnouncementsParams.MessageIds = MessageIds;
            hotAnnouncementsParams.PrivateMessagePath = PrivateMessagePath;
            hotAnnouncementsParams.CommonAnnouncementPath = CommonAnnouncementPath;
            hotAnnouncementsParams.AdminId = AdminId;
            hotAnnouncementsParams.CustId = CustId;
            hotAnnouncementsParams.UserId = UserId;
            hotAnnouncementsParams.Language = Language;
            hotAnnouncementsParams.AnnouncementEnableStatus = ProductsStatus;
            hotAnnouncementsParams.RoleId = RoleId;
            hotAnnouncementsParams.Site = Site;
            hotAnnouncementsParams.SubAccId = SubAccId;
            hotAnnouncementsParams.UserName = UserName;
        }
    }
}