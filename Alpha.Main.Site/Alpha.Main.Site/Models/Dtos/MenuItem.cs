namespace Alpha.Main.Site.Models.Dtos
{
    using System.Collections.Generic;

    public class MenuItem
    {
        public string Name { get; set; }

        public string Title { get; set; }        

        public string Url { get; set; }

        public bool ShowSubMenus { get; set; }

        public int Order { get; set; }

        public bool IsImportant { get; set; }

        public List<MenuItem> SubMenus { get; set; }

        public MenuItem ParentMenu { get; set; }
    }
}