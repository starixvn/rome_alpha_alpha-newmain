namespace Alpha.Main.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using Criteria.Message;
    using DataAccesses;
    using Models.Message;

    public class MessageServiceImpl : MessageService
    {
        private readonly MessageDataAccess messageDataAccess;

        public MessageServiceImpl(MessageDataAccess messageDataAccess)
        {
            this.messageDataAccess = messageDataAccess;
        }

        public IList<Message> GetMessages(
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

            return messageDataAccess.GetMessages(criteria, messageType);
        }
    }
}