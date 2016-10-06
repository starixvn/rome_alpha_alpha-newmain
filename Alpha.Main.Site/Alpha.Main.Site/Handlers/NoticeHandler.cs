namespace Alpha.Main.Site.Handlers
{
    using System;
    using System.Collections.Generic;
    using Main.Models.Notice;
    using MediatR;
    using Requests.Dashboard.Notice;
    using Services;
    using ViewModels.Dashboard.Notice;

    public class NoticeHandler :
        IRequestHandler<NoticeQuery, NoticeMessagesViewModel>,
        IRequestHandler<MarkMessageAsReadQuery, bool>
    {
        private const string QueryNullExceptionMessage = "Query cannot be null";
        private readonly NoticeService noticeService;

        public NoticeHandler(NoticeService noticeService)
        {
            this.noticeService = noticeService;
        }

        public NoticeMessagesViewModel Handle(NoticeQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), QueryNullExceptionMessage);
            }

            var noticeMessages = noticeService.GetNoticeMessages(
                                    message.CustId,
                                    message.ParentSiteId,
                                    message.LastPostTime,
                                    message.TotalMessage,
                                    message.IsTest);

            var noticeMessageViewModels = CreateNoticeMessageViewModels(noticeMessages, message.CurrentLang);

            var viewModel = new NoticeMessagesViewModel(noticeMessageViewModels);

            return viewModel;
        }

        public bool Handle(MarkMessageAsReadQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), QueryNullExceptionMessage);
            }

            var isSuccess = noticeService.MarkMessageAsRead(message.CustId, message.MessageId);

            return isSuccess;
        }

        private List<NoticeMessageWrapper> CreateNoticeMessageViewModels(List<NoticeMessage> noticeMessages, string currentLang)
        {
            var noticeMessageViewWrappers = new List<NoticeMessageWrapper>();

            foreach (var noticeMessage in noticeMessages)
            {
                var noticeMessageWrapper = new NoticeMessageWrapper(noticeMessage, currentLang);
                noticeMessageViewWrappers.Add(noticeMessageWrapper);
            }

            return noticeMessageViewWrappers;
        }
    }
}