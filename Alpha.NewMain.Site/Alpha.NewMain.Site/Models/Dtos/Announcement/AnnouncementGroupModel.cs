namespace Alpha.NewMain.Site.Models.Dtos.Announcement
{
    using System.Collections.Generic;

    public class AnnouncementGroupModel
    {
        public bool ShowTitle { get; set; }

        public string GroupId { get; set; }

        public List<AnnouncementModel> AnnouncementItems { get; set; }
    }
}