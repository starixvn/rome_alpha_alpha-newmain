namespace Alpha.Main.Site.Helpers
{
    using System.Globalization;
    using System.Resources;
    using AppResources;

    public static class ResourceHelper
    {
        public static string GetResource(string key, string language)
            => GetResource(Resources.ResourceManager, key, language);

        public static string GetChatbotResource(string key, string language)
            => GetResource(ChatBot.ResourceManager, key, language);

        private static string GetResource(ResourceManager resourceManager, string key, string language)
            => resourceManager.GetString(key, CultureInfo.GetCultureInfo(language));
    }
}