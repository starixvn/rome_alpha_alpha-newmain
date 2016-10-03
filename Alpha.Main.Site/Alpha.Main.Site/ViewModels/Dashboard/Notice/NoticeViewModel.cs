namespace Alpha.Main.Site.ViewModels.Dashboard.Notice
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Constants;

    public class NoticeViewModel
    {
        public NoticeViewModel(IEnumerable<NoticeMessageWrapper> noticeMessages)
        {
            this.NoticeMessages = noticeMessages.OrderByDescending(m => m.CurrentMessage.PostTime);
        }

        public IEnumerable<NoticeMessageWrapper> NoticeMessages { get; private set; }

        public IOrderedEnumerable<DateTime> DateGroup
        {
            get
            {
                return this.NoticeMessages
                    .Select(m => m.CurrentMessage.PostTime.Date)
                    .Distinct()
                    .OrderByDescending(d => d);
            }
        }

        public DateTime MaxDateTime { get; } = DateTimeDefault.MaxDateTime;

        public DateTime LastMessagePostTime
        {
            get
            {
                if (this.DateGroup?.Count() == 0)
                {
                    return DateTimeDefault.MinDateTime;
                }

                return this.DateGroup.Last();
            }
        }
    }
}