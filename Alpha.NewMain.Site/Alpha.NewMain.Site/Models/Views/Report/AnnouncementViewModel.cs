namespace Alpha.NewMain.Site.Models.Views.Report
{
    using System;
    using System.Collections.Generic;

    public class AnnouncementViewModel
    {
        public bool IsShowAnnouncementTitle { get; set; }

        public bool IsShowMoreLink { get; set; }

        public DateTime LastPostTime { get; set; }

        public List<AnnouncementGroupViewModel> AnnouncementGroup { get; set; }
    }
}