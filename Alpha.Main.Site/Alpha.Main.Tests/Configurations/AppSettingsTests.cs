namespace Alpha.Main.Tests.Configurations
{
    using System.Collections.Specialized;
    using Alpha.Main.Configurations;
    using NUnit.Framework;

    [TestFixture]
    public class AppSettingsTests
    {
        [Test]
        public void AssetAppPath_HasConfig_ReturnsCorrectSetting()
        {
            // Arrange
            AppSettings.Settings = new NameValueCollection
            {
                ["AssetAppPath"] = "/assets"
            };

            var expectedSetting = "/assets";

            // Act
            var actualSetting = AppSettings.AssetAppPath;

            // Assert
            Assert.AreEqual(expectedSetting, actualSetting);
        }

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
        public void IsMBCSite_HasConfigAndValueNotEqualIBCBET_ReturnsFalse()
        {
            // Arrange
            AppSettings.Settings = new NameValueCollection
            {
                ["SiteGroup"] = "ABC"
            };

            // Act
            var actualSetting = AppSettings.IsMBCSite;

            // Assert
            Assert.False(actualSetting);
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
        public void ShowGoldDeluxeAnnouncement_IsMBCSiteIsFalse_ReturnsFalse()
        {
            // Arrange
            AppSettings.Settings = new NameValueCollection();

            // Act
            var actualSetting = AppSettings.ShowGoldDeluxeAnnouncement;

            // Assert
            Assert.False(actualSetting);
        }

        [Test]
        public void ShowGoldDeluxeAnnouncement_IsMBCSiteIsTrue_ReturnsTrue()
        {
            // Arrange
            AppSettings.Settings = new NameValueCollection
            {
                ["SiteGroup"] = "IBCBET"
            };

            // Act
            var actualSetting = AppSettings.ShowGoldDeluxeAnnouncement;

            // Assert
            Assert.True(actualSetting);
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
            AppSettings.Settings = new NameValueCollection
            {
                ["Message:Super"] = "3000,3100"
            };

            var expectedSetting = "3000,3100";

            // Act
            var actualSetting = AppSettings.MessageSuper;

            // Assert
            Assert.AreEqual(expectedSetting, actualSetting);
        }

        [Test]
        public void MessageMaster_HasConfig_ReturnsCorrectSetting()
        {
            // Arrange
            AppSettings.Settings = new NameValueCollection
            {
                ["Message:Master"] = "3000,3200"
            };

            var expectedSetting = "3000,3200";

            // Act
            var actualSetting = AppSettings.MessageMaster;

            // Assert
            Assert.AreEqual(expectedSetting, actualSetting);
        }

        [Test]
        public void MessageAgent_HasConfig_ReturnsCorrectSetting()
        {
            // Arrange
            AppSettings.Settings = new NameValueCollection
            {
                ["Message:Agent"] = "3000,3300"
            };

            var expectedSetting = "3000,3300";

            // Act
            var actualSetting = AppSettings.MessageAgent;

            // Assert
            Assert.AreEqual(expectedSetting, actualSetting);
        }
    }
}
