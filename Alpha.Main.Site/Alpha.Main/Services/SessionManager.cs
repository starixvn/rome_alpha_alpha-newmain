namespace Alpha.Main.Services
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public interface SessionManager
    {
        void Add<T>(string key, T item);

        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Get", Justification = "Reviewed!")]
        T Get<T>(string key);

        T GetOrAdd<T>(string key, Func<T> addItemFactory);

        void Remove(string key);
    }
}