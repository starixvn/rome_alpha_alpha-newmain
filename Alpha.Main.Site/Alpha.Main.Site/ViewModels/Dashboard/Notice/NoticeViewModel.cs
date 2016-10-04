namespace Alpha.Main.Site.ViewModels.Dashboard.Notice
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class NoticeViewModel
    {
        public NoticeViewModel(List<NoticeMessageWrapper> noticeMessages)
        {
            NoticeMessages = noticeMessages
                .OrderByDescending(m => m.CurrentMessage.PostTime)
                .ToList();

            LastMessagePostTime = NoticeMessages.Any() ? NoticeMessages.Last().PostTime : DateTime.MinValue;
        }

        public List<NoticeMessageWrapper> NoticeMessages { get; private set; }

        public DateTime LastMessagePostTime { get; private set; }
    }
}