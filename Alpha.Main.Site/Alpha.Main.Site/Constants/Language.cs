namespace Alpha.Main.Site.Constants
{
    using System.Collections.Generic;

    public static class Language
    {
        public const string English = "en-US";
        public const string ChineseTraditional = "zh-TW";
        public const string ChineseSimplified = "zh-CN";
        public const string Japan = "ja-JP";
        public const string Thailand = "th-TH";
        public const string Korean = "ko-KR";
        public const string VietNam = "vi-VN";

        public const string Default = English;

        public static readonly Dictionary<string, string> CodeKeys = new Dictionary<string, string>
        {
            [English] = "E",
            [ChineseTraditional] = "CT",
            [ChineseSimplified] = "CS",
            [Japan] = "J",
            [Thailand] = "TH",
            [Korean] = "KO",
            [VietNam] = "VI"
        };

        public static readonly Dictionary<string, string> CodeNames = new Dictionary<string, string>
        {
            [English] = "English",
            [ChineseTraditional] = "繁體中文",
            [ChineseSimplified] = "简体中文",
            [Japan] = "日本語",
            [Thailand] = "ภาษาไทย",
            [Korean] = "한국어",
            [VietNam] = "Tiếng Việt"
        };
    }
}