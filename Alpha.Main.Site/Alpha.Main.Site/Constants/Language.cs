namespace Alpha.Main.Site.Constants
{
    using System.Collections.Generic;

    public static class Language
    {
        private const string English = "en-US";
        public const string Default = English;        

        public static readonly Dictionary<string, string> CodeKeys = new Dictionary<string, string>
        {
            [English] = "E",
            ["zh-TW"] = "CT",
            ["zh-CN"] = "CS",
            ["ja-JP"] = "J",
            ["th-TH"] = "TH",
            ["ko-KR"] = "KO",
            ["vi-VN"] = "VI"
        };

        public static readonly Dictionary<string, string> CodeNames = new Dictionary<string, string>
        {
            [English] = "English",
            ["zh-TW"] = "繁體中文",
            ["zh-CN"] = "简体中文",
            ["ja-JP"] = "日本語",
            ["th-TH"] = "ภาษาไทย",            
            ["ko-KR"] = "한국어",
            ["vi-VN"] = "Tiếng Việt"
        };
    }
}