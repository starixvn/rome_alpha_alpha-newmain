namespace Alpha.Main.Site.Helpers
{
    using Alpha.Main.Site.Constants;

    public class LanguageHelper
    {
        public static string GetShortLanguageCode(string language)
        {
            var codeLength = 2;
            var correctedlanguage = string.IsNullOrEmpty(language) ? Language.Default : language;
            return language.Substring(0, codeLength);
        }
    }
}