namespace Alpha.Main.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Criteria.Notice;
    using Fanex.Data.Repository;
    using Models.Notice;

    public class NoticeServiceImpl : NoticeService
    {
        private readonly IDynamicRepository dynamicRepository;

        public NoticeServiceImpl(
            IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public List<NoticeMessage> GetNoticeMessages(
            int custId,
            string parentSites,
            DateTime lastPostTime,
            int totalMessage,
            bool isTest)
        {
            var criteria = new GetNoticeMessagesCriteria(custId, parentSites, lastPostTime, totalMessage, isTest);

            var message = dynamicRepository.Fetch<NoticeMessage>(criteria).ToList();

            return message;
        }

        public bool MarkMessageAsRead(int custId, int messageId)
        {
            var criteria = new MarkMessageAsReadCriteria(custId, messageId.ToString());

            if (!criteria.IsValid())
            {
                return false;
            }

            dynamicRepository.Execute(criteria);

            return true;
        }
    }
}