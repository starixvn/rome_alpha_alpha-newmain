namespace Alpha.Main.Site.ViewModels
{
    using System.Collections.Generic;
    using Alpha.Main.Constants;

    public class LanguageViewModel
    {
        public LanguageViewModel(
            IReadOnlyDictionary<string, string> data,
            string currentLanguage = Language.Default)
        {
            Data = data ?? Language.CodeNames;
            CurrentLanguage = currentLanguage;
        }

        public IReadOnlyDictionary<string, string> Data { get; }

        public string CurrentLanguage { get; }
    }
}
