namespace Alpha.Main.Site.Models
{
    using System;

    [Serializable]
    public abstract class Entity<TId>
    {
        public TId Id { get; set; }
    }
}