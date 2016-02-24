namespace Alpha.NewMain.Site.Models.Views.Report
{
    using System;
    using System.Collections.Generic;
    using Dtos.Announcement;

    public class AnnouncementViewModel
    {
        public bool IsShowAnnouncementTitle { get; set; }

        public bool IsShowMoreLink { get; set; }

        public DateTime LastPostTime { get; set; }

        public List<AnnouncementGroupModel> AnnouncementGroup { get; set; }
    }
}