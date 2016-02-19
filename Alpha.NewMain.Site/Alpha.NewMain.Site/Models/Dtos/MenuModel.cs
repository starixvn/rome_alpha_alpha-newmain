namespace Alpha.NewMain.Site.Models.Dtos
{
    using System.Collections.Generic;

    public class MenuModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public bool ShowSubMenu { get; set; }

        public int Order { get; set; }

        public bool IsImportant { get; set; }

        public List<MenuModel> SubMenu { get; set; }
    }
}