namespace Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement
{
    using System.Collections.Generic;
    using Alpha.Main.Services;
    using NSubstitute;
    using Params.Announcements;

    public class ShowedMessageCondition
    {
        public ShowedMessageCondition(bool isForceUseSecurityCode)
        {
            ForceUseSecurityCode = isForceUseSecurityCode;
        }

        public bool ForceUseSecurityCode { get; set; }

        public SecurityCodeService SecurityCodeService { get; set; }

        public void SetUpCondition(GetHotAnnouncementsParams query)
        {
            SecurityCodeService = Substitute.For<SecurityCodeService>();

            query.AnnouncementEnableStatus = new Dictionary<int, bool>();
        }
    }
}
