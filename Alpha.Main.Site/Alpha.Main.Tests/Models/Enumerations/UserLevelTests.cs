namespace Alpha.Main.Tests.Models.Enumerations
{
    using Main.Constants;
    using Main.Models.Enumerations;
    using NUnit.Framework;

    [TestFixture]
    public class UserLevelTests
    {
        private const string DirectUplineDataSourceName = "uplineUserLevelAndDownlineValueDataSources";
        private const string DirectDownlineDataSourceName = "downlineUserLevelAndUplineValueDataSources";
        private const string SameUserLevelDataSourceName = "sameUseLevelDataSource";

        private static object[] uplineUserLevelAndDownlineValueDataSources =
        {
            new object[] { UserLevel.Super,  UserLevel.Super.Value, false },
            new object[] { UserLevel.Super,  UserLevel.Master.Value, true },
            new object[] { UserLevel.Super,  UserLevel.Agent.Value, false },
            new object[] { UserLevel.Master, UserLevel.Super.Value, false },
            new object[] { UserLevel.Master, UserLevel.Master.Value, false },
            new object[] { UserLevel.Master, UserLevel.Agent.Value, true },
            new object[] { UserLevel.Agent,  UserLevel.Super.Value, false },
            new object[] { UserLevel.Agent,  UserLevel.Master.Value, false },
            new object[] { UserLevel.Agent,  UserLevel.Agent.Value, false }
        };
        
        private static object[] downlineUserLevelAndUplineValueDataSources =
        {
            new object[] { UserLevel.Super,  UserLevel.Super.Value, false },
            new object[] { UserLevel.Super,  UserLevel.Master.Value, false },
            new object[] { UserLevel.Super,  UserLevel.Agent.Value, false },
            new object[] { UserLevel.Master, UserLevel.Super.Value, true },
            new object[] { UserLevel.Master, UserLevel.Master.Value, false },
            new object[] { UserLevel.Master, UserLevel.Agent.Value, false },
            new object[] { UserLevel.Agent,  UserLevel.Super.Value, false },
            new object[] { UserLevel.Agent,  UserLevel.Master.Value, true },
            new object[] { UserLevel.Agent,  UserLevel.Agent.Value, false }
        };
        
        private static object[] sameUseLevelDataSource =
        {
            new object[] { UserLevel.Super,  UserLevel.Super, true },
            new object[] { UserLevel.Super,  UserLevel.Master, false },
            new object[] { UserLevel.Super,  UserLevel.Agent, false },
            new object[] { UserLevel.Master, UserLevel.Super, false },
            new object[] { UserLevel.Master, UserLevel.Master, true },
            new object[] { UserLevel.Master, UserLevel.Agent, false },
            new object[] { UserLevel.Agent,  UserLevel.Super, false },
            new object[] { UserLevel.Agent,  UserLevel.Master, false },
            new object[] { UserLevel.Agent,  UserLevel.Agent, true }
        };

        [Test]
        public void IsAgentLevel_UserLevelIsAgent_ReturnTrue()
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(UserLevel.Agent.Value);

            // Act
            var result = userLevel.IsAgentLevel();

            // Assert
            Assert.AreEqual(true, result);
        }

        [Test]
        public void IsNotAgentLevel_UserLevelIsAgent_ReturnFalse()
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(UserLevel.Agent.Value);

            // Act
            var result = userLevel.IsNotAgentLevel();

            // Assert
            Assert.AreEqual(false, result);
        }

        [Test]
        public void IsMasterLevel_UserLevelIsMaster_ReturnTrue()
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(UserLevel.Master.Value);

            // Act
            var result = userLevel.IsMasterLevel();

            // Assert
            Assert.AreEqual(true, result);
        }

        [Test]
        public void IsNotMasterLevel_UserLevelIsMaster_ReturnFalse()
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(UserLevel.Master.Value);

            // Act
            var result = userLevel.IsNotMasterLevel();

            // Assert
            Assert.AreEqual(false, result);
        }

        [Test]
        public void IsSuperLevel_UserLevelIsSuper_ReturnTrue()
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(UserLevel.Super.Value);

            // Act
            var result = userLevel.IsSuperLevel();

            // Assert
            Assert.AreEqual(true, result);
        }

        [Test]
        public void IsNotSuperLevel_UserLevelIsSuper_ReturnFalse()
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(UserLevel.Super.Value);

            // Act
            var result = userLevel.IsNotSuperLevel();

            // Assert
            Assert.AreEqual(false, result);
        }

        [Test]
        public void IsDirectUplineOf_CustomerLevelIsNull_ReturnFalse()
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(UserLevel.Super.Value);

            // Act
            var result = userLevel.IsDirectUplineOf(null);

            // Assert
            Assert.AreEqual(false, result);
        }

        [Test, TestCaseSource(DirectUplineDataSourceName)]
        public void IsDirectUplineOf_Always_ReturnValidResult(UserLevel uplineLevel, byte downlineLevelValue, bool result)
        {
            var actualResult = uplineLevel.IsDirectUplineOf(downlineLevelValue);

            Assert.AreEqual(result, actualResult);
        }

        [Test, TestCaseSource(DirectUplineDataSourceName)]
        public void IsDirectUplineOf_ValidCustomerLevel_ReturnValidValue(UserLevel uplineLevel, byte downlineLevelValue, bool result)
        {
            // Act
            var actualResult = uplineLevel
                .IsDirectUplineOf(Enumeration.FromValue<UserLevel>(downlineLevelValue));

            // Assert
            Assert.AreEqual(result, actualResult);
        }

        [Test, TestCaseSource(DirectUplineDataSourceName)]
        public void IsNotDirectUplineOf_ValidDownlineLevelValue_ReturnValidResult(UserLevel uplineLevel, byte downlineLevelValue, bool result)
        {
            var actualResult = uplineLevel.IsNotDirectUplineOf(downlineLevelValue);

            Assert.AreEqual(!result, actualResult);
        }

        [Test, TestCaseSource(DirectUplineDataSourceName)]
        public void IsNotDirectUplineOf_DownlineLevelIsNotNull_ReturnValidResult(UserLevel uplineLevel, byte downlineLevelValue, bool result)
        {
            // Arrange
            var downLineLevel = Enumeration.FromValue<UserLevel>(downlineLevelValue);

            // Act
            var actualResult = uplineLevel.IsNotDirectUplineOf(downLineLevel);

            // Assert
            Assert.AreEqual(!result, actualResult);
        }

        [Test]
        public void IsDirectDownlineOf_CustomerLevelIsNull_ReturnFalse()
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(UserLevel.Super.Value);

            // Act
            var result = userLevel.IsDirectDownlineOf(null);

            // Assert
            Assert.AreEqual(false, result);
        }

        [Test, TestCaseSource(DirectDownlineDataSourceName)]
        public void IsDirectDownlineOf_CustomerLevelIsNotNull_ReturnValidResult(UserLevel downlineLevel, byte uplineLevelValue, bool result)
        {
            // Arrange
            var uplineLevel = Enumeration.FromValue<UserLevel>(uplineLevelValue);

            // Act
            var actualResult = downlineLevel.IsDirectDownlineOf(uplineLevel);

            // Assert
            Assert.AreEqual(result, actualResult);
        }

        [Test, TestCaseSource(DirectDownlineDataSourceName)]
        public void IsNotDirectDownlineOf_CustomerLevelIsNotNull_ReturnValidResult(UserLevel downlineLevel, byte uplineLevelValue, bool result)
        {
            // Arrange
            var uplineLevel = Enumeration.FromValue<UserLevel>(uplineLevelValue);

            // Act
            var actualResult = downlineLevel.IsNotDirectDownlineOf(uplineLevel);

            // Assert
            Assert.AreEqual(!result, actualResult);
        }

        [TestCase(RoleId.Super)]
        [TestCase(RoleId.Master)]
        [TestCase(RoleId.Agent)]
        public void Is_UserLevelIsNull_ReturnFalse(byte userLevelValue)
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(userLevelValue);

            // Act
            var result = userLevel.Is(null);

            // Assert
            Assert.AreEqual(false, result);
        }

        [Test, TestCaseSource(SameUserLevelDataSourceName)]
        public void Is_UserLevelIsNotNull_ReturnExpectedResult(UserLevel userLevel1, UserLevel userLevel2, bool result)
        {
            var actualResult = userLevel1.Is(userLevel2);

            Assert.AreEqual(result, actualResult);
        }

        [Test, TestCaseSource(SameUserLevelDataSourceName)]
        public void IsNot_UserLevelIsNotNull_ReturnExpectedResult(UserLevel userLevel1, UserLevel userLevel2, bool result)
        {
            var actualResult = userLevel1.IsNot(userLevel2);

            Assert.AreEqual(!result, actualResult);
        }
    }
}
