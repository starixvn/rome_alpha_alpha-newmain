namespace Alpha.Main.Constants
{
    using System.Collections.Generic;
    using AppResources;

    public static class SelfdestructionPeriod
    {
        public const int FifteenMinutes = 15;
        public const int ThirtyMinutes = 30;
        public const int OneHour = 60;
        public const int ThreeHours = 180;
        public const int SixHours = 360;
        public const int TwelveHours = 720;
        public const int OneDay = 1440;
        public const int ThreeDays = 4320;
        public const int SevenDays = 10080;
        public const int FifteenDays = 21600;
        public const int ThirtyDays = 43200;

        public static IDictionary<int, string> Options { get; } = new Dictionary<int, string>
        {
            [FifteenMinutes] = ChatBot.FifteenMinutes,
            [ThirtyMinutes] = ChatBot.ThirtyMinutes,
            [OneHour] = ChatBot.OneHour,
            [ThreeHours] = ChatBot.ThreeHours,
            [SixHours] = ChatBot.SixHours,
            [TwelveHours] = ChatBot.TwelveHours,
            [OneDay] = ChatBot.OneDay,
            [ThreeDays] = ChatBot.ThreeDays,
            [SevenDays] = ChatBot.SevenDays,
            [FifteenDays] = ChatBot.FifteenDays,
            [ThirtyDays] = ChatBot.ThirtyDays
        };
    }
}