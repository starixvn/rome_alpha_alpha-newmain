namespace Alpha.Main.Site.ViewModels.Message
{
    using Constants;
    using Fanex.Age.Utility;
    using Main.Models.Message;

    public class MessageWrapper
    {
        private const string ContentFontStartMarkup = "<font color=red>";
        private const string ContentFontEndMarkup = "</font>";
        private const string DefaultContentPropName = "ContentEN";

        private readonly string currentLang = Language.English;

        public MessageWrapper(Message message, string currentLang)
        {
            CurrentMessage = message;
            this.currentLang = currentLang;
        }

        public Message CurrentMessage { get; }

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

                content = content.ToString()
                    .Replace(ContentFontStartMarkup, string.Empty)
                    .Replace(ContentFontEndMarkup, string.Empty)
                    .Replace("@@", CurrentMessage.BetTransId);

                return content.ToString();
            }
        }

        public bool HasRedColor => CurrentMessage.ContentEN.Contains(ContentFontStartMarkup);

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