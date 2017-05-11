namespace Alpha.Main.Site.ViewModels.Message
{
    public class MessageLevel
    {
        public enum Range
        {
            /// <summary>
            /// Normal messages is from 21 and later
            /// </summary>
            Normal = 21,

            /// <summary>
            /// Highlight messages is from 1 to 20
            /// </summary>
            Highlight = 20
        }
    }
}