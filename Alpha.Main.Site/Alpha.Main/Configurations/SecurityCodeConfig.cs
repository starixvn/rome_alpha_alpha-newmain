//namespace Alpha.Main.Configurations
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Configuration;

//    /// <summary>
//    /// TODO: Update summary.
//    /// </summary>
//    public static class SecurityCodeConfig
//    {
//        public static readonly bool IsActive;
//        private static readonly Dictionary<string, bool> IgnoreSecurityCode;

//        static SecurityCodeConfig()
//        {
//            IsActive = false;
//            //SecurityCodeSection.Instance.Active;
//            IgnoreSecurityCode = InitializeSecurityCodeSection();
//        }

//        /// <summary>
//        /// Determines whether [is ignore security code] [the specified URL].
//        /// </summary>
//        /// <param name="url">The URL that need to be validated.</param>
//        /// <returns>
//        ///   <c>true</c> If [is ignore security code] [the specified URL]; otherwise, <c>false</c>.
//        /// </returns>
//        public static bool IsIgnoreSecurityCode(string url)
//        {
//            return IgnoreSecurityCode.ContainsKey(url);
//        }

//        /// <summary>
//        /// Gets the ignore security codes.
//        /// </summary>
//        /// <returns>The hash table of Url.</returns>
//        private static Dictionary<string, bool> InitializeSecurityCodeSection()
//        {
//            Dictionary<string, bool> ignoreHash = new Dictionary<string, bool>();
//            if (IsActive)
//            {
//                string virtualDirectory = ConfigurationManager.AppSettings["VirtualDirectory"];
//                virtualDirectory = string.IsNullOrEmpty(virtualDirectory)
//                    ? string.Empty
//                    : "/" + virtualDirectory.ToLower();

//                foreach (var ignoreUrl in SecurityCodeSection.Instance.IgnoreUrls)
//                {
//                    IgnoreUrlElement urlElement = (IgnoreUrlElement)ignoreUrl;
//                    string url = urlElement.Url;
//                    if (!string.IsNullOrEmpty(url))
//                    {
//                        string[] urlArray = url.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
//                        if (urlArray.Length > 0)
//                        {
//                            foreach (string item in urlArray)
//                            {
//                                ignoreHash.Add(virtualDirectory + item.Trim(), urlElement.IsFullUrl);
//                            }
//                        }
//                    }
//                }
//            }

//            return ignoreHash;
//        }
//    }
//}
