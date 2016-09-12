namespace Alpha.Main.Params.Announcements
{
    using Configurations;
    using Models.SecurityCode;
    using Sessions;
    using System.Collections.Generic;

    public class GetHotAnnouncementsParams
    {
        private readonly ISecurityCodeSession securityCodeSession;
        private bool isSecurityCodeActive;

        public GetHotAnnouncementsParams() : this(null, null)
        {
        }

        internal GetHotAnnouncementsParams(
            ISecurityCodeSession securityCodeSession = null,
            bool? isSecurityCodeActive = null)
        {
            this.securityCodeSession = securityCodeSession ?? CookieLessSessionFactory.SecurityCodeSession;
            this.isSecurityCodeActive = isSecurityCodeActive.HasValue ? isSecurityCodeActive.Value : SecurityCodeConfig.IsActive;
        }

        public string MessageIds { get; set; }

        public string PrivateMessagePath { get; set; }

        public string CommonAnnouncementPath { get; set; }

        public int UserId { get; set; }

        public int SubAccId { get; set; }

        public IDictionary<int, bool> AnnouncementEnableStatus { get; set; }

        public byte RoleId { get; set; }

        public int AdminId { get; set; }

        public int CustId { get; set; }

        public string Language { get; set; }

        public string UserName { get; set; }

        public string Site { get; set; }

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
    }
}