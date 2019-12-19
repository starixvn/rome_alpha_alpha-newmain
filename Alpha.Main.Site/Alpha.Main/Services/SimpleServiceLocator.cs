namespace Alpha.Main.Services
{
    using System;
    using System.Collections.Generic;

    public sealed class SimpleServiceLocator
    {
        private static readonly Dictionary<string, object> container
            = new Dictionary<string, object>();

        private SimpleServiceLocator()
        {
        }

        public static SimpleServiceLocator Instance { get; } = new SimpleServiceLocator();

        public SimpleServiceLocator Register<T>(T service)
        {
            container[typeof(T).ToString()] = service;

            return this;
        }

        public static T GetService<T>()
        {
            if (container.ContainsKey(typeof(T).ToString()))
            {
                return (T)container[typeof(T).ToString()];
            }

            throw new InvalidOperationException($"cannot resolve type {typeof(T).FullName}");
        }
    }
}