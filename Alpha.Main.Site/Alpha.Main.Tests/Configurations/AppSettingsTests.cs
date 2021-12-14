namespace Alpha.Main.Tests.Configurations
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using Alpha.Main.Configurations;
    using NUnit.Framework;

    [TestFixture]
    public class AppSettingsTests
    {
        [Test]
        public void OldMainAppPath_HasConfig_ReturnsCorrectSetting()
        {
            // Arrange
            AppSettings.Settings = new NameValueCollection
            {
                ["OldMainAppPath"] = "/ex-main"
            };

            var expectedSetting = "/ex-main";

            // Act
            var actualSetting = AppSettings.OldMainAppPath;

            // Assert
            Assert.AreEqual(expectedSetting, actualSetting);
        }

        [Test]
        public void HowToUseAppPath_HasConfig_ReturnsCorrectSetting()
        {
            // Arrange
            AppSettings.Settings = new NameValueCollection
            {
                ["HowToUseAppPath"] = "/site-howtouse"
            };

            var expectedSetting = "/site-howtouse";

            // Act
            var actualSetting = AppSettings.HowToUseAppPath;

            // Assert
            Assert.AreEqual(expectedSetting, actualSetting);
        }

        [Test]
        public void ReportAppPath_HasConfig_ReturnsCorrectSetting()
        {
            // Arrange
            AppSettings.Settings = new NameValueCollection
            {
                ["ReportsAppPath"] = "/site-reports"
            };

            var expectedSetting = "/site-reports";

            // Act
            var actualSetting = AppSettings.ReportAppPath;

            // Assert
            Assert.AreEqual(expectedSetting, actualSetting);
        }

        [Test]
        public void ExTotalBetsForecastAppPath_HasConfig_ReturnsCorrectSetting()
        {
            // Arrange
            AppSettings.Settings = new NameValueCollection
            {
                ["ExTotalBetsForecastAppPath"] = "/ex-totalbetsforecast"
            };

            var expectedSetting = "/ex-totalbetsforecast";

            // Act
            var actualSetting = AppSettings.ExTotalBetsForecastAppPath;

            // Assert
            Assert.AreEqual(expectedSetting, actualSetting);
        }

        [Test]
        public void MemberInfoAppPath_HasConfig_ReturnsCorrectSetting()
        {
            // Arrange
            AppSettings.Settings = new NameValueCollection
            {
                ["MemberInfoAppPath"] = "/site-memberinfo"
            };

            var expectedSetting = "/site-memberinfo";

            // Act
            var actualSetting = AppSettings.MemberInfoAppPath;

            // Assert
            Assert.AreEqual(expectedSetting, actualSetting);
        }

        [Test]
        public void UsersAlwaysAllowAccessBetsListMonitoring_HasConfig_ReturnsCorrectSetting()
        {
            // Arrange
            AppSettings.Settings = new NameValueCollection
            {
                ["UsersAlwaysAllowAccessBetsListMonitoring"] = "1176086"
            };

            var expectedSetting = "1176086";

            // Act
            var actualSetting = AppSettings.UsersAlwaysAllowAccessBetsListMonitoring;

            // Assert
            Assert.AreEqual(expectedSetting, actualSetting);
        }

        [Test]
        public void EnableBetsListMonitoring_HasConfig_ReturnsCorrectSetting()
        {
            // Arrange
            AppSettings.Settings = new NameValueCollection
            {
                ["EnableBetsListMonitoring"] = "true"
            };

            // Act
            var actualSetting = AppSettings.EnableBetsListMonitoring;

            // Assert
            Assert.True(actualSetting);
        }

        [Test]
        public void VirtualDirectory_HasConfig_ReturnsCorrectSetting()
        {
            // Arrange
            AppSettings.Settings = new NameValueCollection
            {
                ["VirtualDirectory"] = "site-main"
            };

            var expectedSetting = "site-main";

            // Act
            var actualSetting = AppSettings.VirtualDirectory;

            // Assert
            Assert.AreEqual(expectedSetting, actualSetting);
        }

        [Test]
        public void NonUseSecurityCodeSites_HasConfig_ReturnsCorrectSetting()
        {
            // Arrange
            AppSettings.Settings = new NameValueCollection
            {
                ["NonUseSecurityCodeSites"] = "789y"
            };

            var expectedSetting = "789y";

            // Act
            var actualSetting = AppSettings.NonUseSecurityCodeSites;

            // Assert
            Assert.AreEqual(expectedSetting, actualSetting);
        }

        [Test]
        public void StaticResourcesVersion_HasConfig_ReturnsCorrectSetting()
        {
            // Arrange
            AppSettings.Settings = new NameValueCollection
            {
                ["AssetsApp:StaticResourcesVersion"] = "4114434"
            };

            var expectedSetting = "4114434";

            // Act
            var actualSetting = AppSettings.StaticResourcesVersion;

            // Assert
            Assert.AreEqual(expectedSetting, actualSetting);
        }

        [Test]
        public void AllowedSitesToViewTBF_HasConfig_ReturnsCorrectSetting()
        {
            // Arrange
            AppSettings.Settings = new NameValueCollection
            {
                ["TotalBetsForecast:AllowedSitesToViewTBF"] = "site"
            };

            var expectedSetting = "site";

            // Act
            var actualSetting = AppSettings.AllowedSitesToViewTBF;

            // Assert
            Assert.AreEqual(expectedSetting, actualSetting);
        }

        [Test]
        public void StoreProcedurePath_HasConfig_ReturnsCorrectSetting()
        {
            // Arrange
            AppSettings.Settings = new NameValueCollection
            {
                ["StoredProcedurePath"] = "~/App_Data/SPNames/LOCAL"
            };

            var expectedSetting = "~/App_Data/SPNames/LOCAL";

            // Act
            var actualSetting = AppSettings.StoreProcedurePath;

            // Assert
            Assert.AreEqual(expectedSetting, actualSetting);
        }

        [Test]
        public void IsDisabledContactUsLink_ConfigValueIsEqualOne_ReturnsTrue()
        {
            // Arrange
            AppSettings.Settings = new NameValueCollection
            {
                ["IsDisabledContactUsLink"] = "1"
            };

            // Act
            var actualSetting = AppSettings.IsDisabledContactUsLink;

            // Assert
            Assert.True(actualSetting);
        }

        [Test]
        public void IsDisabledContactUsLink_ConfigValueIsNotEqualOne_ReturnsTrue()
        {
            // Arrange
            AppSettings.Settings = new NameValueCollection
            {
                ["IsDisabledContactUsLink"] = "0"
            };

            // Act
            var actualSetting = AppSettings.IsDisabledContactUsLink;

            // Assert
            Assert.False(actualSetting);
        }

        [Test]
        public void GoogleAnalyticId_HasConfig_ReturnsCorrectSetting()
        {
            // Arrange
            AppSettings.Settings = new NameValueCollection
            {
                ["GoogleAnalyticId"] = "UA-93265717-1"
            };

            var expectedSetting = "UA-93265717-1";

            // Act
            var actualSetting = AppSettings.GoogleAnalyticId;

            // Assert
            Assert.AreEqual(expectedSetting, actualSetting);
        }

        [Test]
        public void IsMBCSite_HasConfigAndValueEqualIBCBET_ReturnsTrue()
        {
            // Arrange
            AppSettings.Settings = new NameValueCollection
            {
                ["SiteGroup"] = "IBCBET"
            };

            // Act
            var actualSetting = AppSettings.IsMBCSite;

            // Assert
            Assert.True(actualSetting);
        }

        [Test]
        public void IsMBCSite_HasConfigAndValueNotEqual789Y_ReturnsTrue()
        {
            // Arrange
            AppSettings.Settings = new NameValueCollection
            {
                ["SiteGroup"] = "ABC"
            };

            // Act
            var actualSetting = AppSettings.IsMBCSite;

            // Assert
            Assert.True(actualSetting);
        }

        [Test]
        public void IsMBCSite_NotHasConfig_ReturnsFalse()
        {
            // Arrange
            AppSettings.Settings = new NameValueCollection();

            // Act
            var actualSetting = AppSettings.IsMBCSite;

            // Assert
            Assert.False(actualSetting);
        }
       

        [Test]
        public void NoticeTypes_HasConfig_ReturnsCorrectSetting()
        {
            // Arrange
            AppSettings.Settings = new NameValueCollection
            {
                ["NoticeTypes"] = "1_2,1,2,3"
            };

            var expectedSetting = new[] { "1_2", "1", "2", "3" };

            // Act
            var actualSetting = AppSettings.NoticeTypes;

            // Assert
            Assert.AreEqual(expectedSetting, actualSetting);
        }

        [Test]
        public void MessageAppPath_HasConfig_ReturnsCorrectSetting()
        {
            // Arrange
            AppSettings.Settings = new NameValueCollection
            {
                ["MessageAppPath"] = "/site-messages"
            };

            var expectedSetting = "/site-messages";

            // Act
            var actualSetting = AppSettings.MessageAppPath;

            // Assert
            Assert.AreEqual(expectedSetting, actualSetting);
        }

        [Test]
        public void ShowHelpLink_IsMBCSite_ReturnsTrue()
        {
            // Arrange
            AppSettings.Settings = new NameValueCollection
            {
                ["SiteGroup"] = "IBCBET"
            };

            // Act
            var actualSetting = AppSettings.ShowHelpLink;

            // Assert
            Assert.IsTrue(actualSetting);
        }

        [Test]
        public void LocalTest_IsLocalTest_ReturnsTrue()
        {
            // Arrange
            AppSettings.Settings = new NameValueCollection
            {
                ["LocalTest"] = "1"
            };

            // Act
            var actualSetting = AppSettings.LocalTest;

            // Assert
            Assert.IsTrue(actualSetting);
        }

        [Test]
        public void LocalTest_IsNotLocalTest_ReturnsFalse()
        {
            // Arrange
            AppSettings.Settings = new NameValueCollection
            {
                ["LocalTest"] = "0"
            };

            // Act
            var actualSetting = AppSettings.LocalTest;

            // Assert
            Assert.IsFalse(actualSetting);
        }

        [Test]
        public void MessageSuper_HasConfig_ReturnsCorrectSetting()
        {
            // Arrange
            var expectedSetting = "3000,3100";
            AppSettings.Settings = new NameValueCollection
            {
                ["Message:Super"] = expectedSetting
            };

            // Act
            var actualSetting = AppSettings.MessageSuper;

            // Assert
            Assert.AreEqual(expectedSetting, actualSetting);
        }

        [Test]
        public void MessageMaster_HasConfig_ReturnsCorrectSetting()
        {
            // Arrange
            var expectedSetting = "3000,3200";
            AppSettings.Settings = new NameValueCollection
            {
                ["Message:Master"] = expectedSetting
            };

            // Act
            var actualSetting = AppSettings.MessageMaster;

            // Assert
            Assert.AreEqual(expectedSetting, actualSetting);
        }

        [Test]
        public void MessageAgent_HasConfig_ReturnsCorrectSetting()
        {
            // Arrange
            var expectedSetting = "3000,3300";
            AppSettings.Settings = new NameValueCollection
            {
                ["Message:Agent"] = expectedSetting
            };

            // Act
            var actualSetting = AppSettings.MessageAgent;

            // Assert
            Assert.AreEqual(expectedSetting, actualSetting);
        }

        [Test]
        public void ChatBotBaseUrl_HasConfig_ReturnCorrectSetting()
        {
            // Arrange
            var expectedSetting = "Url";
            AppSettings.Settings = new NameValueCollection
            {
                ["Chatbot:BaseUrl"] = expectedSetting
            };

            // Act
            var actualSetting = AppSettings.ChatBotBaseUrl;

            // Assert
            Assert.AreEqual(expectedSetting, actualSetting);
        }

        [Test]
        public void ChatBotApiKey_HasConfig_ReturnCorrectSetting()
        {
            // Arrange
            var expectedSetting = "ApiKey";
            AppSettings.Settings = new NameValueCollection
            {
                ["Chatbot:ApiKey"] = expectedSetting
            };

            // Act
            var actualSetting = AppSettings.ChatBotApiKey;

            // Assert
            Assert.AreEqual(expectedSetting, actualSetting);
        }

        [Test]
        public void OfferChatbotSites_HasConfig_ReturnCorrectSetting()
        {
            // Arrange

            AppSettings.Settings = new NameValueCollection
            {
                ["OfferChatbotSites"] = "BEST-ODDS,733333"
            };
            var expectedSetting = new[] { "BEST-ODDS", "733333" };

            // Act
            var actualSetting = AppSettings.OfferChatbotSites;

            // Assert
            Assert.AreEqual(expectedSetting, actualSetting);
        }

        [Test]
        public void RemoteRefURL_HasConfig_ReturnCorrectSetting()
        {
            // Arrange

            AppSettings.Settings = new NameValueCollection
            {
                ["RemoteRefURL"] = "RemoteRefURL"
            };
            var expectedSetting = "RemoteRefURL";

            // Act
            var actualSetting = AppSettings.RemoteRefURL;

            // Assert
            Assert.AreEqual(expectedSetting, actualSetting);
        }

        [Test]
        public void OfferClearGivenCredit_HasConfig_ReturnCorrectSetting()
        {
            // Arrange

            AppSettings.Settings = new NameValueCollection
            {
                ["OfferClearGivenCredit:Sites"] = "KORIBC,INDO"
            };
            var expectedSetting = new[] { "KORIBC", "INDO" };

            // Act
            var actualSetting = AppSettings.Settings["OfferClearGivenCredit:Sites"].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            // Assert
            Assert.AreEqual(expectedSetting, actualSetting);
        }

        [Test]
        public void IndirectLoginAllowSuperIds_HasConfig_ReturnCorrectValues()
        {
            // Arrange

            AppSettings.Settings = new NameValueCollection
            {
                ["IndirectLogin:AllowSuperIds"] = "1,2"
            };
            var expectedSetting = new List<int> { 1, 2 };

            // Act
            var actualSetting = AppSettings.IndirectLoginSuperIds;

            // Assert
            Assert.AreEqual(expectedSetting, actualSetting);
        }

        [Test]
        public void IndirectLoginAllowSuperIds_HasConfig_ReturnCorrectSetting()
        {
            // Arrange

            AppSettings.Settings = new NameValueCollection
            {
                ["IndirectLogin:AllowSuperIds"] = "1,2"
            };
            var expectedSetting = new[] { "1", "2" };

            // Act
            var actualSetting = AppSettings.Settings["IndirectLogin:AllowSuperIds"].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            // Assert
            Assert.AreEqual(expectedSetting, actualSetting);
        }

        [TestCase(1, true)]
        [TestCase(2, true)]
        [TestCase(3, false)]
        [TestCase(4, false)]
        public void IndirectLoginIsAllowSuperIds_HasConfig_ReturnCorrectValues(int srecommend, bool expected)
        {
            // Arrange

            AppSettings.Settings = new NameValueCollection
            {
                ["IndirectLogin:AllowSuperIds"] = "1,2"
            };

            // Act
            var actualSetting = AppSettings.IsAllowedIndirectLogin(srecommend);

            // Assert
            Assert.AreEqual(expected, actualSetting);
        }

        [TestCase(1, false)]
        [TestCase(2, false)]
        [TestCase(3, false)]
        [TestCase(4, false)]
        public void IndirectLoginIsAllowSuperIds_HasNotConfig_ReturnCorrectValues(int srecommend, bool expected)
        {
            // Arrange

            AppSettings.Settings = new NameValueCollection
            {
                ["IndirectLogin:AllowSuperIds"] = ""
            };

            // Act
            var actualSetting = AppSettings.IsAllowedIndirectLogin(srecommend);

            // Assert
            Assert.AreEqual(expected, actualSetting);
        }

        [TestCase(1, true)]
        [TestCase(2, true)]
        [TestCase(3, true)]
        [TestCase(4, true)]
        public void IndirectLoginIsAllowSuperIds_HasAsteriskConfig_ReturnCorrectValues(int srecommend, bool expected)
        {
            // Arrange

            AppSettings.Settings = new NameValueCollection
            {
                ["IndirectLogin:AllowSuperIds"] = "*"
            };

            // Act
            var actualSetting = AppSettings.IsAllowedIndirectLogin(srecommend);

            // Assert
            Assert.AreEqual(expected, actualSetting);
        }

        //[Test]
        //public void AllowedSitesToViewBonusDetail_HasConfig_ReturnsExpectedValue()
        //{
        //    // Arrange
        //    AppSettings.Settings = new NameValueCollection
        //    {
        //        ["AllowedSitesToViewBonusDetail"] = "abc, def,ghi,"
        //    };

        //    // Act
        //    var actualSetting = AppSettings.AllowedSitesToViewBonusDetail;

        //    // Assert
        //    Assert.True(actualSetting.Length == 3
        //        && actualSetting[0] == "ABC"
        //        && actualSetting[1] == "DEF"
        //        && actualSetting[2] == "GHI");
        //}

        [Test]
        public void AllowedSitesToViewBonusDetail_HasNotConfig_ReturnsEmptyArray()
        {
            // Arrange & Act
            var actualSetting = AppSettings.AllowedSitesToViewBonusDetail;

            // Assert
            Assert.IsEmpty(actualSetting);
        }

        [Test]
        public void AllowedSitesToViewBonusDetail_ConfigIsEmpty_ReturnsEmptyArray()
        {
            // Arrange
            AppSettings.Settings = new NameValueCollection
            {
                ["AllowedSitesToViewBonusDetail"] = ""
            };

            // Act
            var actualSetting = AppSettings.AllowedSitesToViewBonusDetail;

            // Assert
            Assert.IsEmpty(actualSetting);
        }

        [Test]
        public void AllowedSitesToViewBonusDetail_ConfigIsWhiteSpace_ReturnsEmptyArray()
        {
            // Arrange
            AppSettings.Settings = new NameValueCollection
            {
                ["AllowedSitesToViewBonusDetail"] = "     "
            };

            // Act
            var actualSetting = AppSettings.AllowedSitesToViewBonusDetail;

            // Assert
            Assert.IsEmpty(actualSetting);
        }

        [TestCase("", "", false)]
        [TestCase(" ,", " ,", false)]
        [TestCase("*", "", true)]
        [TestCase("*", "*", false)]
        [TestCase("", "*", false)]
        [TestCase("", "1176071, 1176072", false)]
        [TestCase("", "1176070, 1176071, 1176072", false)]
        [TestCase("*", "1176071, 1176072", true)]
        [TestCase("*", "1176070, 1176071, 1176072", false)]
        [TestCase("733333, 733334, 733335", "", true)]
        [TestCase("733333, 733334, 733335", "1176071, 1176072", true)]
        [TestCase("733333, 733334, 733335", "1176070, 1176071, 1176072", false)]
        [TestCase("733334, 733335", "", false)]
        [TestCase("733334, 733335", "1176070, 1176071, 1176072", false)]
        [TestCase("733334, 733335", "1176071, 1176072", false)]
        public void IsUsedNewNewReports_HasConfig_ReturnCorrectValues(string configSites, string configSuperIds, bool expected)
        {
            // Arrange
            AppSettings.Settings = new NameValueCollection
            {
                ["NewReports.OfferSites"] = configSites,
                ["NewReports.IgnoreSuperIds"] = configSuperIds
            };

            // Act
            var actualSetting = AppSettings.IsUsedNewReports("733333", 1176070);

            // Assert
            Assert.AreEqual(expected, actualSetting);
        }

        [TestCase("", false)]
        [TestCase("*", true)]
        [TestCase("733331, 733332", false)]
        [TestCase("733331, 733332, 733333", true)]
        public void IsAllowedTransferToDownlineHistorySite_HasConfig_ReturnCorrectValues(string configSites, bool expected)
        {
            // Arrange
            AppSettings.Settings = new NameValueCollection
            {
                ["TransferToDownlineHistory.OfferSites"] = configSites
            };

            // Act
            var actualSetting = AppSettings.IsAllowedTransferToDownlineHistorySite("733333");

            // Assert
            Assert.AreEqual(expected, actualSetting);
        }
    }
}