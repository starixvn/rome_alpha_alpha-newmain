namespace Alpha.NewMain.Site.Extensions
{
    using System.Web;

    public class SessionExtension
    {
        private const string HYPHEN = "_";
        private const string IsDefaultAvatarKey = "IsDefaultAvatar";
        private const string BinaryAvatarKey = "BinaryAvatar";
        private const string LanguageKey = "Language";
        private const string DefaultLanguage = "en-US";
        
        public static bool IsDefaultAvatar
        {
            get
            {
                object value = GetProperty(IsDefaultAvatarKey);
                return (null == value) ? false : (bool)value;
            }

            set
            {
                SetProperty(IsDefaultAvatarKey, value);
            }
        }

        public static byte[] BinaryAvatar
        {
            get
            {
                object value = GetProperty(BinaryAvatarKey);
                return (null == value) ? null : (byte[])value;
            }

            set
            {
                SetProperty(BinaryAvatarKey, value);
            }
        }

        public static string Language
        {
            get
            {
                object value = GetProperty(LanguageKey);
                return (null == value) ? DefaultLanguage : value as string;
            }
        }

        private static object GetProperty(string key)
        {
            return HttpContext.Current.Session[HttpContext.Current.Session.SessionID + HYPHEN + key];
        }

        private static void SetProperty(string key, object value)
        {
            HttpContext.Current.Session[HttpContext.Current.Session.SessionID + HYPHEN + key] = value;
        }
    }
}