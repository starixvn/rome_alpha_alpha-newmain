namespace Alpha.NewMain.Site.Models.Dtos.Announcement
{
    using System;

    public class AnnouncementModel
    {
        public string Id { get; set; }

        public string ContentEn { get; set; }

        public string ContentCs { get; set; }

        public string ContentCh { get; set; }

        public string ContentJp { get; set; }

        public string ContentTh { get; set; }

        public string ContentIt { get; set; }

        public string ContentKo { get; set; }

        public string ContentEs { get; set; }

        public DateTime PostTime { get; set; }

        public string Type { get; set; }

        public bool IsSticky { get; set; }

        public string LastUpdateTime { get; set; }

        public string BetTransId { get; set; }

        public bool IsRead { get; set; }

        public string FullContent { get; set; }

        public string BriefContent { get; set; }
    }
}