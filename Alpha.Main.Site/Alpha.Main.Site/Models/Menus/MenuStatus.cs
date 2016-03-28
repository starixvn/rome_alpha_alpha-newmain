namespace Alpha.Main.Site.Models.Menus
{
    using System;

    public class MenuStatus
    {
        public MenuStatus(bool visible = true, bool disabled = false)
        {
            this.Visible = visible;
            this.Disabled = disabled;
        }

        public MenuStatus(Func<bool> getVisibleFunc = null, Func<bool> getDisableFunc = null)
            : this(getVisibleFunc != null ? getVisibleFunc.Invoke() : true,
                  getDisableFunc != null ? getDisableFunc.Invoke() : false)
        {
        }

        public bool Visible { get; }

        public bool Disabled { get; }
    }
}