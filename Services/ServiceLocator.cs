using System;
using System.Collections.Generic;
using System.Linq;

namespace TestGame.Services
{
    public class ServiceLocator : IServiceLocator
    {
        private static IServiceLocator _instance;
        private IDictionary<Type, IDictionary<string, object>> _services;
        private readonly static object _lock = new Object();

        public ServiceLocator()
        {
            _services = new Dictionary<Type, IDictionary<string, object>>();
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

            if (_services.ContainsKey(typeof(T)))
                throw new InvalidOperationException("Cannot construct and object for injection in nameless context. Object already exists.");

            _services.Add(typeof(T), new Dictionary<string, object> { { string.Empty, instance } });
        }

        public void AddService<T>(object instance, string name = null)
        {
            if (_services.ContainsKey(typeof(T)))
            {
                _services[typeof(T)].Add(name, instance);
            }
            else
            {
                _services.Add(typeof(T), new Dictionary<string, object> { { name ?? string.Empty, instance } });
            }
        }

        public void RemoveService<T>(object instance, string name)
        {
            var existing = GetService<T>(name);
            if (existing.Equals(instance))
            {
                _services.Remove(typeof(T));
            }
            else
            {
                throw new InvalidOperationException("Cannot remove a different instance than what was registered.");
            }
        }

        public T GetService<T>(string name = null)
        {
            try
            {
                return (T)_services[typeof(T)][name ?? string.Empty];
            }
            catch (KeyNotFoundException)
            {
                throw new ApplicationException($"Service of type {typeof(T).FullName} was not registered");
            }
        }
    }
}