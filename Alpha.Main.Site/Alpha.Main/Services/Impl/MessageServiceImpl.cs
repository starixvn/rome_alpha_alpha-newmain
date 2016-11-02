namespace Alpha.Main.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Xml;
    using Configurations;
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

        public List<Message> GetMessagesFromXml(string xmlFileName, int messageType, byte userRoleId)
        {
            var messages = new List<Message>();
            var xmlMessages = new XmlDocument();
            var xmlFilePath = $"{HttpContext.Current.Server.MapPath(AppSettings.GetMessagesDataPath)}/{xmlFileName}";

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
                        Convert.ToInt32(xmlMessage.Attributes["id"].Value),
                        xmlMessage.SelectNodes("en-US")?[0]?.InnerText,
                        xmlMessage.SelectNodes("zh-CN")?[0]?.InnerText,
                        xmlMessage.SelectNodes("zh-TW")?[0]?.InnerText,
                        xmlMessage.SelectNodes("ja-JP")?[0]?.InnerText,
                        xmlMessage.SelectNodes("th-TH")?[0]?.InnerText,
                        string.Empty,
                        xmlMessage.SelectNodes("ko-KR")?[0]?.InnerText,
                        string.Empty,
                        xmlMessage.SelectNodes("vi-VN")?[0]?.InnerText,
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        Convert.ToDateTime(xmlMessage.Attributes["posttime"].Value),
                        Convert.ToDateTime(xmlMessage.Attributes["posttime"].Value),
                        Convert.ToDateTime(xmlMessage.Attributes["posttime"].Value),
                        (byte)messageType,
                        false,
                        false,
                        0,
                        false,
                        0,
                        0,
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        0,
                        0);

            return message;
        }
    }
}