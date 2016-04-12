namespace Alpha.Main.Params.Announcements
{
    using System.Collections.Generic;
    using Fanex.Razorbill.Security.Configurations;
    using Fanex.Razorbill.Security.Entities;
    using Fanex.Razorbill.Security.Sessions;

    public class GetHotAnnouncementsParams
    {
        private readonly bool isSecurityCodeActive;
        private readonly ISecurityCodeSession securityCodeSession;

        public GetHotAnnouncementsParams() : this(null, null)
        {
        }

        internal GetHotAnnouncementsParams(
            ISecurityCodeSession securityCodeSession,
            bool? isSecurityCodeActive)
        {
            this.securityCodeSession = securityCodeSession ?? CookieLessSessionFactory.SecurityCodeSession;
            this.isSecurityCodeActive = isSecurityCodeActive.HasValue ? isSecurityCodeActive.Value : SecurityCodeConfig.IsActive;
        }

        public string MessageIds { get; set; }

        public string PrivateMessagePath { get; set; }

        public string CommonAnnouncementPath { get; set; }

        public int UserId { get; set; }

        public int SubAccId { get; set; }

        public IDictionary<int, bool> ProductsStatus { get; set; }

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