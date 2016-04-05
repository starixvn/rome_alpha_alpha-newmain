namespace Alpha.Main.Site.Services.Impl
{
    using System;
    using System.Web;
    using System.Web.SessionState;

    public class SessionManagerImpl : SessionManager
    {
        private readonly HttpSessionState httpSession;
        
        public SessionManagerImpl(HttpSessionState httpSession)
        {
            this.httpSession = httpSession ?? HttpContext.Current.Session;
        }
         
        public void Add<T>(string key, T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            ValidateKey(key);            

            httpSession.Add(key, item);
        }

        public T Get<T>(string key)
        {
            ValidateKey(key);

            var item = httpSession[key];

            if (item is T)
            {
                return (T)item;
            }

            if (item is Lazy<T>)
            {
                return ((Lazy<T>)item).Value;
            }

            return default(T);
        }

        public T GetOrAdd<T>(string key, Func<T> addItemFactory)
        {
            ValidateKey(key);

            var newLazyItem = new Lazy<T>(addItemFactory);

            var existingItem = httpSession[key];

            if (existingItem != null)
            {
                if (existingItem is T)
                {
                    return (T)existingItem;
                }

                if (existingItem is Lazy<T>)
                {
                    return ((Lazy<T>)existingItem).Value;
                }

                return default(T);
            }

            try
            {
                return newLazyItem.Value;
            }
            catch
            {
                // addItemFactory errored so do not cache the exception
                httpSession.Remove(key);
                throw;
            }
        }

        public void Remove(string key)
        {
            httpSession.Remove(key);
        }

        private void ValidateKey(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentOutOfRangeException("key", @"Cache keys cannot be empty or whitespace");
            }
        }
    }
}