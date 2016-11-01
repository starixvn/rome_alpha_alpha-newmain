namespace Alpha.Main.Site.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Xml;
    using Configurations;
    using Constants;
    using Main.Models.Message;
    using MediatR;
    using Requests.Message;
    using Services;
    using ViewModels.Message;

    public class MessageHandler : IRequestHandler<MessageQuery, MessagesViewModel>
    {
        private readonly MessageService messageService;

        private static readonly Dictionary<int, string> MessageFileNameByType = new Dictionary<int, string>()
        {
            [MessageType.Public] = "NormalMessage.xml",
            [MessageType.Special] = "SpecialMessage.xml",
            [MessageType.Private] = "PersonalMessage.xml"
        };

        public MessageHandler(MessageService messageService)
        {
            this.messageService = messageService;
        }

        public MessagesViewModel Handle(MessageQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            var messages = messageService.GetMessages(
                                message.CustId,
                                message.ParentSites,
                                message.SpecialType,
                                message.TotalMessage,
                                message.StartDate,
                                message.EndDate,
                                message.IsTest,
                                message.MessageType);

            messages.AddRange(LoadMessagesFromXml(message));

            var messageWrappers = new List<MessageWrapper>(messages.Select(m => new MessageWrapper(m, message.CurrentLang)));

            var viewModel = new MessagesViewModel(
                messageWrappers,
                message.StartDate,
                message.EndDate,
                message.MessageType,
                message.RoleId);

            return viewModel;
        }

        private List<Message> LoadMessagesFromXml(MessageQuery messageQuery)
        {
            var messages = new List<Message>();
            var messageFilePath =
                HttpContext.Current.Server.MapPath($"{AppSettings.GetMessagesDataPath}/{MessageFileNameByType[messageQuery.MessageType]}");

            if (!File.Exists(messageFilePath))
            {
                return messages;
            }

            var xmlMessages = new XmlDocument();

            try
            {
                xmlMessages.Load(messageFilePath);

                foreach (XmlNode xmlMessage in xmlMessages?.DocumentElement?.ChildNodes)
                {
                    if (XmlMessageIsValid(xmlMessage))
                    {
                        var showingDate = Convert.ToDateTime(xmlMessage.Attributes["showdate"].Value);
                        var effectedLevel = Convert.ToByte(xmlMessage.Attributes["level"].Value);

                        if (showingDate <= DateTime.Today &&
                            (effectedLevel == 0 || effectedLevel == messageQuery.RoleId))
                        {
                            messages.Add(InitMessageObject(xmlMessage, messageQuery));
                        }
                    }
                }
            }
            catch
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

        private Message InitMessageObject(XmlNode xmlMessage, MessageQuery messageQuery)
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
                        (byte)messageQuery.MessageType,
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