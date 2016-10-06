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
            int targetId,
            string targetId2,
            DateTime lastPostTime,
            int totalMessage,
            bool isTest)
        {
            var criteria = new GetNoticeMessagesCriteria(targetId, targetId2, lastPostTime, totalMessage, isTest);

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