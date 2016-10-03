namespace Alpha.Main.Site.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Constants;

    public class LanguageViewModel
    {
        public LanguageViewModel(Dictionary<string, string> data, string currentLanguage = Language.Default)
        {
            Data = data ?? Language.CodeNames;
            CurrentLanguage = currentLanguage;
        }

        public Dictionary<string, string> Data { get; private set; }

        public string CurrentLanguage { get; private set; }

        /// <summary>
        /// Equals: used for compare two object (Unit test)
        /// </summary>
        public override bool Equals(object obj)
        {
            var compareViewModel = (LanguageViewModel)obj;
            if ((Data == null && compareViewModel.Data != null) ||
                (Data != null && compareViewModel.Data == null) ||
                (Data.Count != compareViewModel.Data.Count))
            {
                return false;
            }

            var isEqual = !Data.Except(compareViewModel.Data).Any() &&
                string.Equals(CurrentLanguage, compareViewModel.CurrentLanguage, StringComparison.Ordinal);

            return isEqual;
        }

        public override int GetHashCode()
        {
            return Data.GetHashCode() ^ CurrentLanguage.GetHashCode();
        }

        /// <summary>
        /// ToString: use for displaying compare result two object (Unit test)
        /// </summary>
        public override string ToString()
        {
            var result = new StringBuilder();

            result.AppendLine("Data:");
            foreach (var datum in Data)
            {
                result.AppendLine(datum.Key + " " + datum.Value);
            }

            result.AppendLine();
            result.AppendLine("CurrentLanguage: " + CurrentLanguage);

            result.AppendLine(base.ToString());
            return result.ToString();
        }
    }
}