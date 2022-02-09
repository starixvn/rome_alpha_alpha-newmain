namespace Alpha.Main.Site.Tests.Helpers
{
    using System;
    using NSubstitute;
    using Site.Helpers;

    public class UrlHelperTestSetUp
    {
        public static void SetUp()
        {
            UrlHelper.MapOldMainPath = Substitute.For<Func<string, string>>();
            UrlHelper.MapOldMainPath(Arg.Any<string>()).Returns("OldMainPath");

            UrlHelper.MapMainPath = Substitute.For<Func<string, string>>();
            UrlHelper.MapMainPath(Arg.Any<string>()).Returns("MainPath");

            UrlHelper.MapReportsPath = Substitute.For<Func<string, string>>();
            UrlHelper.MapReportsPath(Arg.Any<string>()).Returns("ReportsPath/");

            UrlHelper.MapHowToUsePath = Substitute.For<Func<string, string>>();
            UrlHelper.MapHowToUsePath(Arg.Any<string>()).Returns("HowToUsePath");

            UrlHelper.MapExTotalBetsForecastPath = Substitute.For<Func<string, string>>();
            UrlHelper.MapExTotalBetsForecastPath(Arg.Any<string>()).Returns("ExTotalBetsForecastPath");

            UrlHelper.MapMessagesPath = Substitute.For<Func<string, string>>();
            UrlHelper.MapMessagesPath(Arg.Any<string>()).Returns("MessagesPath");

            UrlHelper.MapMemberInfoPath = Substitute.For<Func<string, string>>();
            UrlHelper.MapMemberInfoPath(Arg.Any<string>()).Returns("MemberInfoPath/");
        }
    }
}
