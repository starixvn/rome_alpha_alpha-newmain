namespace Alpha.NewMain.Site.Models.Dtos.Announcement
{
    public class HotAnnouncementModel
    {
        public long Id { get; set; }

        public string Content { get; set; }

        public int Top { get; set; }

        public int Left { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public string Source { get; set; }

        public string CookieId { get; set; }

        public string LanguagueCode { get; set; }
    }
}