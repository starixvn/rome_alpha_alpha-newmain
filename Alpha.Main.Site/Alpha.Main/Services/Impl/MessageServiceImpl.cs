namespace Alpha.Main.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Criteria.Message;
    using Fanex.Data.Repository;
    using Models.Message;

    public class MessageServiceImpl : MessageService
    {
        private readonly IDynamicRepository dynamicRepository;

        public MessageServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public List<Message> GetMessages(
            int custId,
            string parentSites,
            int specialType,
            int totalMessage,
            DateTime startDate,
            DateTime endDate,
            bool isTest,
            int messageType)
        {
            var criteria = new GetMessageCriteria(
                custId,
                parentSites,
                specialType,
                totalMessage,
                startDate,
                endDate,
                isTest,
                messageType);

            var messages = dynamicRepository.Fetch<Message>(criteria);

            return messages.ToList();
        }
    }
}