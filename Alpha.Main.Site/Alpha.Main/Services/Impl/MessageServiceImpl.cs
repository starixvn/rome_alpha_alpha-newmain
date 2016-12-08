namespace Alpha.Main.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;
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

            var messages = messageDataAccess.GetMessages(criteria, messageType);

            return messages;
        }

        public List<Message> GetMessagesFromXml(string xmlFilePath, int messageType, byte userRoleId)
        {
            var messages = new List<Message>();
            var xmlMessages = new XmlDocument();

            if (!File.Exists(xmlFilePath))
            {
                return messages;
            }

            try
            {
                xmlMessages.Load(xmlFilePath);

                foreach (XmlNode xmlMessage in xmlMessages?.DocumentElement?.ChildNodes)
                {
                    if (XmlMessageIsValid(xmlMessage))
                    {
                        var showingDate = Convert.ToDateTime(xmlMessage.Attributes["showdate"].Value);
                        var effectedLevel = Convert.ToByte(xmlMessage.Attributes["level"].Value);

                        if (showingDate <= DateTime.Today &&
                            (effectedLevel == 0 || effectedLevel == userRoleId))
                        {
                            messages.Add(InitMessageObject(xmlMessage, messageType));
                        }
                    }
                }
            }
            catch (XmlException)
            {
                return messages;
            }

            return messages;
        }

        private bool XmlMessageIsValid(XmlNode xmlMessage)
          => xmlMessage?.ChildNodes?.Count > 0 &&
              !string.IsNullOrEmpty(xmlMessage?.Attributes?["level"]?.Value) &&
              !string.IsNullOrEmpty(xmlMessage?.Attributes?["id"]?.Value) &&
              !string.IsNullOrEmpty(xmlMessage?.Attributes?["posttime"]?.Value) &&
              !string.IsNullOrEmpty(xmlMessage?.Attributes?["showdate"]?.Value);

        private Message InitMessageObject(XmlNode xmlMessage, int messageType)
        {
            var message = new Message(
                        id: Convert.ToInt32(xmlMessage.Attributes["id"].Value),
                        contentEN: xmlMessage.SelectNodes("en-US")?[0]?.InnerText,
                        contentCS: xmlMessage.SelectNodes("zh-CN")?[0]?.InnerText,
                        contentCH: xmlMessage.SelectNodes("zh-TW")?[0]?.InnerText,
                        contentJP: xmlMessage.SelectNodes("ja-JP")?[0]?.InnerText,
                        contentTH: xmlMessage.SelectNodes("th-TH")?[0]?.InnerText,
                        contentIT: string.Empty,
                        contentKO: xmlMessage.SelectNodes("ko-KR")?[0]?.InnerText,
                        contentES: string.Empty,
                        contentVN: xmlMessage.SelectNodes("vi-VN")?[0]?.InnerText,
                        postTime: Convert.ToDateTime(xmlMessage.Attributes["posttime"].Value),
                        createTime: Convert.ToDateTime(xmlMessage.Attributes["posttime"].Value),
                        lastUpdateTime: Convert.ToDateTime(xmlMessage.Attributes["posttime"].Value),
                        messageType: (byte)messageType,
                        isSticky: false,
                        isTest: false,
                        matchId: 0,
                        isBatch: false,
                        creator: 0,
                        lastUpdator: 0,
                        creatorName: string.Empty,
                        lastUpdateName: string.Empty,
                        userName: string.Empty,
                        bettransId: string.Empty,
                        userId: 0,
                        rowNum: 0);

            return message;
        }
    }
}