namespace Alpha.Main.Site.Helpers
{
    using System.Globalization;
    using System.Resources;

    public static class ResourceHelper
    {
        public static string GetResource(ResourceManager resourceManager, string key, string language)
            => resourceManager.GetString(key, CultureInfo.GetCultureInfo(language));
    }
}