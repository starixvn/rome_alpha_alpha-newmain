namespace Alpha.Main.Site.ViewModels
{
    using System.Collections.Generic;
    using Constants;

    public class LanguageViewModel
    {
        private Dictionary<string, string> data;
        private string currentLanguage;        
        
        public LanguageViewModel(Dictionary<string, string> data, string currentLanguage = Language.Default)
        {
            this.data = data ?? Language.CodeNames;
            this.currentLanguage = currentLanguage;
        }

        public LanguageViewModel() : this (null, string.Empty)
        {            
        }

        public Dictionary<string, string> Data
        {
            get
            {                
                return this.data;
            }
        }

        public string CurrentLanguage
        {
            get
            {
                return this.currentLanguage;
            }
        }
    }
}