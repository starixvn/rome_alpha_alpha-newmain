namespace Alpha.Main.Site.ViewModels.Dashboard.Notice
{
    using System;
    using Constants;
    using Fanex.Age.Utility;
    using Main.Models.Notice;

    public class NoticeMessageWrapper
    {
        private const string DefaultContentPropName = "ContentEn";
        private const int DefaultMaxContentLength = 100;
        private const string Ellipsis = "...";

        private string currentLang = Language.English;

        public NoticeMessageWrapper(NoticeMessage noticeMessage, string currentLang)
        {
            this.currentLang = currentLang;
            CurrentMessage = noticeMessage;
            PostTime = CurrentMessage.PostTime;
        }

        public NoticeMessage CurrentMessage { get; private set; }

        public DateTime PostTime { get; private set; }

        public string MessageType
        {
            get
            {
                var msgType = CurrentMessage.MessageType;
                var isSticky = CurrentMessage.IsSticky;

                if (msgType == 1 && isSticky != true)
                {
                    return "private";
                }
                else if ((msgType == 1 || msgType == 3) && isSticky == true)
                {
                    return "special";
                }
                else if (msgType == 3 && isSticky == false)
                {
                    return "public";
                }

                return string.Empty;
            }
        }

        public string ShortContent
        {
            get
            {
                int length = Math.Min(DefaultMaxContentLength, Content.Length);

                return Content.Substring(0, length) + Ellipsis;
            }
        }

        public string Content
        {
            get
            {
                var contentPropName = GetCurrentContentPropertyNameByLanguage(currentLang);
                var content = ReflectionUtil.GetValue(CurrentMessage, contentPropName);

                if (string.IsNullOrEmpty(content?.ToString()))
                {
                    content = ReflectionUtil.GetValue(CurrentMessage, DefaultContentPropName) ?? string.Empty;
                }

                var result = content.ToString().Replace("@@", CurrentMessage.BetTransId);

                return result;
            }
        }

        private string GetCurrentContentPropertyNameByLanguage(string lang)
        {
            var contentLangPropName = "Content";

            switch (lang)
            {
                case Language.ChineseSimplified:
                    contentLangPropName += "CS";
                    break;

                case Language.ChineseTraditional:
                    contentLangPropName += "CH";
                    break;

                case Language.Thai:
                    contentLangPropName += "TH";
                    break;

                case Language.Japanese:
                    contentLangPropName += "JP";
                    break;

                case Language.Korean:
                    contentLangPropName += "KO";
                    break;

                default:
                    contentLangPropName += "EN";
                    break;
            }

            return contentLangPropName;
        }
    }
}