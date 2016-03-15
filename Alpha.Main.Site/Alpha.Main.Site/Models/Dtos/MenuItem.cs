namespace Alpha.Main.Site.Models.Dtos
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using App_GlobalResources;

    public class MenuItem
    {
        public string ResourceKey { get; set; }

        public string Name { get; set; }

        public string DisplayName
        {
            get
            {
               return Alpha_sitemap.ResourceManager.GetString(this.ResourceKey);
            }
        }

        public string Url { get; set; }                

        public bool IsImportant { get; set; }

        public List<MenuItem> SubMenus { get; set; }        

        public Expression<Func<User, MenuItem, bool>> IsAccessibleTo
        {
            get; set;
        }
    }
}