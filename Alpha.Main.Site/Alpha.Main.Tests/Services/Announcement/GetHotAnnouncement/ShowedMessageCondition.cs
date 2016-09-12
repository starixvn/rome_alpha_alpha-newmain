namespace Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement
{
    using Alpha.Main.Services;
    using Main.Models.SecurityCode;
    using NSubstitute;
    using Params.Announcements;
    using Sessions;
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
