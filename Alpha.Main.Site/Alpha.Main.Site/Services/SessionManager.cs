namespace Alpha.Main.Site.Services
{
    using System;

    public interface SessionManager
    {
        void Add<T>(string key, T item);        

        T Get<T>(string key);

        T GetOrAdd<T>(string key, Func<T> addItemFactory);

        void Remove(string key);
    }
}