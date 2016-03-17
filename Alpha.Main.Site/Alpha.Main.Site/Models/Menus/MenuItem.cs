namespace Alpha.Main.Site.Models.Menus
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq.Expressions;
    using System.Text.RegularExpressions;
    using System.Web;
    using App_GlobalResources;
    using Fanex.Common.Helper;
    using Services;

    public class MenuItem : IMenuItem
    {
        private string url;

        public MenuItem()
        {           
        }

        public string Key
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }

        public string AppName
        {
            get; set;
        }

        public string DisplayName
        {
            get
            {
                return Alpha_sitemap.ResourceManager.GetString(this.Key);
            }
        }
        
        public virtual string Url
        {
            get
            {
                var appVirtualPath = ConfigurationManager.AppSettings[AppName];
                if (!string.IsNullOrWhiteSpace(appVirtualPath))
                {
                    var menuItemUrl = HttpContext.Current.Response.ApplyAppPathModifier(this.url);
                    return "/" + Regex.Replace(menuItemUrl, HttpRuntime.AppDomainAppVirtualPath, appVirtualPath, RegexOptions.IgnoreCase);
                }

                return this.url;
            }

            set
            {
                this.url = value;
            }
        }

        public string Function
        {
            get; set;
        }

        public bool ShowSubMenus
        {
            get; set;
        }

        public virtual bool IsVisible
        {
            get
            {
                return true;
            }
        }              

        public List<IMenuItem> SubMenus
        {
            get;
            set;
        }       
    }
}