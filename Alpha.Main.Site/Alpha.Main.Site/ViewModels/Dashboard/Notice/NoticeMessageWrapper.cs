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
            this.CurrentMessage = noticeMessage;
            this.currentLang = currentLang;
        }

        public NoticeMessage CurrentMessage
        {
            get; private set;
        }

        public DateTime PostTime
        {
            get
            {
                return this.CurrentMessage.PostTime;
            }
        }

        public string Type
        {
            get
            {
                var msgType = this.CurrentMessage.Type;
                var isSticky = this.CurrentMessage.IsSticky;

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
                var contentPropName = GetCurrentContentPropertyNameByLanguage(this.currentLang);
                var content = ReflectionUtil.GetValue(this.CurrentMessage, contentPropName);

                if (string.IsNullOrEmpty(content?.ToString()))
                {
                    content = ReflectionUtil.GetValue(this.CurrentMessage, DefaultContentPropName) ?? string.Empty;
                }

                var result = content.ToString().Replace("@@", this.CurrentMessage.BetTransId);

                return result;
            }
        }

        private string GetCurrentContentPropertyNameByLanguage(string lang)
        {
            var contentLangPropName = "Content";

            switch (lang)
            {
                case Language.ChineseSimplified:
                    contentLangPropName += "Cs";
                    break;

                case Language.ChineseTraditional:
                    contentLangPropName += "Ch";
                    break;

                case Language.Thai:
                    contentLangPropName += "Th";
                    break;

                case Language.Japanese:
                    contentLangPropName += "Jp";
                    break;

                case Language.Korean:
                    contentLangPropName += "Ko";
                    break;

                default:
                    contentLangPropName += "En";
                    break;
            }

            return contentLangPropName;
        }
    }
}