namespace Alpha.Main.Site.Models.Dtos
{
    using System.Collections.Generic;

    public class MenuItem
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string ResourceKey { get; set; }

        public string Url { get; set; }

        public bool ShowSubMenu { get; set; }

        public int Order { get; set; }

        public bool IsImportant { get; set; }

        public List<MenuItem> SubMenu { get; set; }
    }
}