namespace Alpha.Main.Site.Models.Menus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class MenuStatus
    {                
        public MenuStatus(bool isVisible = true, bool isDisabled = false)
        {
            this.IsVisible = isVisible;
            this.IsDisabled = isDisabled;
        }

        public MenuStatus(Func<bool> getDisableFunc = null) : this(true, getDisableFunc.Invoke())
        {
        }

        public bool IsVisible { get; }

        public bool IsDisabled { get; }

    }
}