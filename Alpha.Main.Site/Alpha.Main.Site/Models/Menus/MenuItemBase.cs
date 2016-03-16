namespace Alpha.Main.Site.Models.Menus
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using App_GlobalResources;
    using Fanex.Common.Helper;

    public class MenuItemBase : IMenuItem
    {
        protected string key;
        protected string displayName;
        protected string url;
        protected string function;
        protected bool showSubMenus;
        protected bool isVisible;

        public MenuItemBase(string key, string url, string function, bool isVisible, bool showSubMenus)
        {            
            this.key = key;
            this.url = url;
            this.function = function;
            this.isVisible = isVisible;
            this.showSubMenus = showSubMenus;
        }

        public string Key
        {
            get
            {
                return key;
            }
        }

        public string DisplayName
        {
            get
            {
                return Alpha_sitemap.ResourceManager.GetString(this.Key);
            }
        }

        public string Url
        {
            get
            {
                return url;
            }
        }

        public string Function
        {
            get
            {
                return function;
            }
        }

        public bool ShowSubMenus
        {
            get
            {
                return showSubMenus;
            }
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