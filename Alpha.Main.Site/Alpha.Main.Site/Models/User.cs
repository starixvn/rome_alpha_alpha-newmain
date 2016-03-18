namespace Alpha.Main.Site.Models
{
    using System;
    using System.Diagnostics;
    using Dtos;
    using Enumerations;
    using Sunplus.Agent.Model.Entities;
    [Serializable]
    public class User : Entity<int>, IUser
    {
        private int id;
        private string name;
        private int subAccountId;
        private UserLevel level;        

        public User()
        {            
        }        

        public new int Id
        {
            [DebuggerStepThrough]
            get
            {
                return this.id;
            }

            [DebuggerStepThrough]
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
            [DebuggerStepThrough]
            get
            {
                return this.name;
            }

            [DebuggerStepThrough]
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
            [DebuggerStepThrough]
            get
            {
                return this.level;
            }

            [DebuggerStepThrough]
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
            [DebuggerStepThrough]
            get
            {
                return this.subAccountId;
            }

            [DebuggerStepThrough]
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
                return SubAccountId > 0;
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