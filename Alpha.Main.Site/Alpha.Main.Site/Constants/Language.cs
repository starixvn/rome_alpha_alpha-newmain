namespace Alpha.Main.Site.Constants
{
    using System.Collections.Generic;

    public static class Language
    {
        private const string English = "en-US";
        public const string Default = English;
        public static readonly List<string> Keys = new List<string> { "E", "CT", "J", "TH", "CS", "KO", "VI" };
        public static readonly List<string> Codes = new List<string> { English, "zh-TW", "ja-JP", "th-TH", "zh-CN", "ko-KR", "vi-VN" };
    }
}