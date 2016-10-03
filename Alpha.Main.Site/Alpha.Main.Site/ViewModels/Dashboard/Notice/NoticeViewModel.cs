namespace Alpha.Main.Site.ViewModels.Dashboard.Notice
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class NoticeViewModel
    {
        public NoticeViewModel(List<NoticeMessageWrapper> noticeMessages)
        {
            NoticeMessages = noticeMessages.OrderByDescending(m => m.CurrentMessage.PostTime).ToList();
        }

        public List<NoticeMessageWrapper> NoticeMessages { get; private set; }

        public DateTime MaxDateTime { get; } = DateTime.MaxValue;

        public DateTime LastMessagePostTime
        {
            get
            {
                if (!NoticeMessages.Any())
                {
                    return DateTime.MinValue;
                }

                return NoticeMessages.Last().PostTime;
            }
        }
    }
}