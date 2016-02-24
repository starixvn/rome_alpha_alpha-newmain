namespace Alpha.NewMain.Site.Models.Views.Report
{
    using System.Collections.Generic;

    public class AnnouncementGroupViewModel
    {
        public bool ShowTitle { get; set; }

        public string GroupId { get; set; }

        public List<AnnouncementItemViewModel> AnnouncementItems { get; set; }
    }
}