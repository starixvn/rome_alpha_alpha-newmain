namespace Alpha.NewMain.Site.Models.Dtos.Announcement
{
    using System;
    using System.Collections.Generic;

    public class AnnouncementReportModel
    {
        public List<AnnouncementGroupModel> AnnouncementGroups { get; set; }

        public DateTime LastPostTime { get; set; }
    }
}