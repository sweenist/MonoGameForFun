using System;
using System.Collections.Generic;
using System.Linq;

namespace TestGame.Services
{
    public class ServiceLocator : IServiceLocator
    {
        private IDictionary<Type, object> _services;
        private readonly static object _lock = new Object();
        private static IServiceLocator _instance;

        public ServiceLocator()
        {
            _services = new Dictionary<Type, object>();
        }

        public static IServiceLocator Instance
        {
            get
            {
                lock (_lock)
                    return _instance ?? (_instance = new ServiceLocator());
            }
        }

        public void AddService<T>(Type implementation, params object[] arguments)
        {
            var constructor = implementation.GetConstructor(arguments.Select(arguments => arguments.GetType()).ToArray());
            var instance = constructor.Invoke(arguments);

            _services.Add(typeof(T), instance);
        }

        public void AddService<T>(object instance)
        {
            _services.Add(typeof(T), instance);
        }

        public T GetService<T>()
        {
            try
            {
                return (T)_services[typeof(T)];
            }
            catch (KeyNotFoundException)
            {
                throw new ApplicationException($"Service of type {typeof(T).FullName} was not registered");
            }
        }
    }
}