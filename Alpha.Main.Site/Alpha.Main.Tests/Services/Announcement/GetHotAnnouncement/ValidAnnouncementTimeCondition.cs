namespace Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement
{
    using Alpha.Main.Helpers;
    using NSubstitute;
    using System;

    public class ValidAnnouncementTimeCondition
    {
        private DateTime validAnnouncementDay = new DateTime(2016, 4, 13);

        public ValidAnnouncementTimeCondition()
        {
            DateTimeWrapper = Substitute.For<IDateTime>();
            DateTimeWrapper.Now.Returns(validAnnouncementDay);
        }

        public IDateTime DateTimeWrapper { get; set; }
    }
}
