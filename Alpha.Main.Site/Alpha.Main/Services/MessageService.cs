namespace Alpha.Main.Services
{
    using System;
    using System.Collections.Generic;
    using Models.Message;

    public interface MessageService
    {
        List<Message> GetMessages(
            int custId,
            string parentSites,
            int specialType,
            int totalMessage,
            DateTime startDate,
            DateTime endDate,
            bool isTest,
            int messageType);

        List<Message> GetMessagesFromXml(string xmlFilePath, int messageType, byte userRoleId);
    }
}