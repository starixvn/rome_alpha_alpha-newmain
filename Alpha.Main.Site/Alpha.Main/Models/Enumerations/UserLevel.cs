namespace Alpha.Main.Models.Enumerations
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    [Serializable]
    public class UserLevel : Enumeration
    {
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Reviewed!")]
        public static readonly UserLevel Super = new UserLevel(4, "Super");
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Reviewed!")]
        public static readonly UserLevel Master = new UserLevel(3, "Master");
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Reviewed!")]
        public static readonly UserLevel Agent = new UserLevel(2, "Agent");
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Reviewed!")]
        public static readonly UserLevel Member = new UserLevel(1, "Member");

        public UserLevel()
        {
        }

        private UserLevel(byte value, string displayName)
            : base(value, displayName)
        {
        }       

        public bool IsAgentLevel()
        {
            return this == Agent;
        }

        public bool IsNotAgentLevel()
        {
            return !IsAgentLevel();
        }

        public bool IsMasterLevel()
        {
            return this == Master;
        }

        public bool IsNotMasterLevel()
        {
            return !IsMasterLevel();
        }

        public bool IsSuperLevel()
        {
            return this == Super;
        }

        public bool IsNotSuperLevel()
        {
            return !IsSuperLevel();
        }

        public bool IsMemberLevel()
        {
            return this == Member;
        }

        public bool IsDirectUplineOf(byte downlineLevelValue)
        {
            return GapBetweenTwoLevels(this.Value, downlineLevelValue) == 1;
        }

        public bool IsNotDirectUplineOf(byte downlineLevelValue)
        {
            return !IsDirectUplineOf(downlineLevelValue);
        }

        public bool IsDirectUplineOf(Enumeration customerLevel)
        {
            if (customerLevel == null)
            {
                return false;
            }

            return IsDirectUplineOf(customerLevel.Value);
        }

        public bool IsNotDirectUplineOf(Enumeration customerLevel)
        {
            return !IsDirectUplineOf(customerLevel);
        }

        public bool IsDirectDownlineOf(Enumeration uplineLevel)
        {
            if (uplineLevel == null)
            {
                return false;
            }

            return GapBetweenTwoLevels(uplineLevel.Value, this.Value) == 1;
        }

        public bool IsNotDirectDownlineOf(Enumeration uplineLevel)
        {
            return !IsDirectDownlineOf(uplineLevel);
        }

        public bool Is(Enumeration level)
        {
            if (level != null)
            {
                return this == level;
            }

            return false;
        }

        public bool IsNot(Enumeration level)
        {
            return !Is(level);
        }

        private static int GapBetweenTwoLevels(byte uplineLevelValue, byte downlineLevelValue)
        {
            return uplineLevelValue - downlineLevelValue;
        }

        public UserLevel GetDirectDownLineLevel() => FromValue<UserLevel>((byte)(Value - 1));
    }
}