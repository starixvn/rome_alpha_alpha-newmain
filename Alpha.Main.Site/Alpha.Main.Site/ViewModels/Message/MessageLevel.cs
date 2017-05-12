namespace Alpha.Main.Site.ViewModels.Message
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("Microsoft.Design", "CA1053:StaticHolderTypesShouldNotHaveConstructors", Justification = "Reviewed!")]
    public class MessageLevel
    {
        [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue", Justification = "Reviewed!")]
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