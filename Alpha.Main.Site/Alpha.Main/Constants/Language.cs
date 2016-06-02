namespace Alpha.Main.Constants
{
    using System.Collections.Generic;

    public static class Language
    {
        public const string English = "en-US";
        public const string ChineseTraditional = "zh-TW";
        public const string ChineseSimplified = "zh-CN";
        public const string Japanese = "ja-JP";
        public const string Thai = "th-TH";
        public const string Korean = "ko-KR";
        public const string Vietnamese = "vi-VN";

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
    }
}