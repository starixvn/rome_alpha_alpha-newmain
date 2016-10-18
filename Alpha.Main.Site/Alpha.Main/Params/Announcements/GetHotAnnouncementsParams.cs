namespace Alpha.Main.Params.Announcements
{
    using System.Collections.Generic;

    public class GetHotAnnouncementsParams
    {
        public string MessageIds { get; set; }

        public string PrivateMessagePath { get; set; }

        public string CommonAnnouncementPath { get; set; }

        public int UserId { get; set; }

        public int SubAccId { get; set; }

        public IDictionary<int, bool> AnnouncementEnableStatus { get; set; }

        public byte RoleId { get; set; }

        public int AdminId { get; set; }

        public int CustId { get; set; }

        public string Language { get; set; }

        public string UserName { get; set; }

        public string Site { get; set; }
    }
}