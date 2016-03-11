namespace Alpha.Main.Site.Models
{
    using System;
    using Enumerations;
    [Serializable]
    public class User : Entity<int>
    {
        private int id;
        private string name;
        private int subAccountId;
        private UserLevel level;

        public new int Id
        {
            get
            {
                return this.id;
            }

            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("value", "User Id cannot be negative or zero.");
                }

                this.id = value;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentOutOfRangeException("value", "Username cannot be null or empty.");
                }

                this.name = value;
            }
        }

        public UserLevel Level
        {
            get
            {
                return this.level;
            }

            set
            {
                if (ReferenceEquals(value, null))
                {
                    throw new ArgumentNullException("value");
                }

                this.level = value;
            }
        }

        public int SubAccountId
        {
            get
            {
                return this.subAccountId;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value", "Sub account id cannot be negative.");
                }

                this.subAccountId = value;
            }
        }

        public bool IsSubAccount
        {
            get
            {
                return (SubAccountId > 0);
            }
        }

        public bool IsMainAccount
        {
            get
            {
                return !IsSubAccount;
            }
        }
    }
}