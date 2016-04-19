namespace Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement
{
    using Alpha.Main.Services;
    using Fanex.Razorbill.Security.Entities;
    using Fanex.Razorbill.Security.Sessions;
    using NSubstitute;
    using Params.Announcements;
    using System.Collections.Generic;

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
            SecurityCodeService.IsForceUseSecurityCode(Arg.Any<string>(), Arg.Any<SecurityCodeStatus>(), Arg.Any<ISecurityCodeSession>())
                .Returns(ForceUseSecurityCode);

            query.AnnouncementEnableStatus = new Dictionary<int, bool>();
        }
    }
}
