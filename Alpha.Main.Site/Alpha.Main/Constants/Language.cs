namespace Alpha.Main.Constants
{
    using System.Collections.Generic;
    using System.Linq;

    public static class Language
    {
        public const string English = "en-US";
        public const string ChineseTraditional = "zh-TW";
        public const string ChineseSimplified = "zh-CN";
        public const string Japanese = "ja-JP";
        public const string Thai = "th-TH";
        public const string Korean = "ko-KR";
        public const string Vietnamese = "vi-VN";

        public const string NewChineseTraditional = "zh-Hant";
        public const string NewChineseSimplified = "zh-Hans";

        public const string Default = English;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Reviewed!")]
        public static readonly Dictionary<string, string> CodeKeys = new Dictionary<string, string>
        {
            [English] = "E",
            [ChineseTraditional] = "CT",
            [ChineseSimplified] = "CS",
            [Japanese] = "J",
            [Thai] = "TH",
            [Korean] = "KO",
            [Vietnamese] = "VI"
        };

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Reviewed!")]
        public static readonly Dictionary<string, string> CodeNames = new Dictionary<string, string>
        {
            [English] = "English",
            [ChineseTraditional] = "繁體中文",
            [ChineseSimplified] = "简体中文",
            [Japanese] = "日本語",
            [Thai] = "ภาษาไทย",
            [Korean] = "한국어",
            [Vietnamese] = "Tiếng Việt"
        };

        public static readonly Dictionary<string, string> TelegramChatBotCodeNames = new Dictionary<string, string>
        {
            [English] = "English",
            [NewChineseTraditional] = "繁體中文",
            [NewChineseSimplified] = "简体中文",
            [Thai] = "ภาษาไทย",
            [Vietnamese] = "Tiếng Việt"
        };

        public static readonly Dictionary<string, string> TelegramChatBotCodeNamesWithoutVietNamese = TelegramChatBotCodeNames
.Where(langauge => langauge.Key != Vietnamese)
            .ToDictionary(language => language.Key, language => language.Value);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Reviewed!")]
        public static readonly Dictionary<string, string> CodeNamesWithoutVietNamese = CodeNames
            .Where(langauge => langauge.Key != Vietnamese)
            .ToDictionary(language => language.Key, language => language.Value);
    }
}