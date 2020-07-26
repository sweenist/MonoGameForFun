using System;
using System.Collections.Generic;
using System.Linq;

namespace SweenGame.Services
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

        public bool PrintDebug { get; set; }
        public void Print(string message)
        {
            if (PrintDebug)
                Console.WriteLine(message);
        }

        public TImpl AddService<T, TImpl>(string name = null, params object[] arguments) where TImpl : class
        {
            TImpl instance = Construct<TImpl>(arguments);

            var t = typeof(T);
            if (_services.ContainsKey(t) && _services[t].ContainsKey(name ?? string.Empty))
                throw new InvalidOperationException($"Cannot construct an object for injection with {name ?? "<null>"}. Object already exists.");

            _services.Add(t, new Dictionary<string, object> { { string.Empty, instance } });
            return instance;
        }

        public void AddService<T, TImpl>(TImpl instance, string name = null) where TImpl : class
        {
            var t = typeof(T);
            if (_services.ContainsKey(t))
            {
                _services[t].Add(name ?? string.Empty, instance);
            }
            else
            {
                _services.Add(t, new Dictionary<string, object> { { name ?? string.Empty, instance } });
            }
        }

        public void RemoveService<T>(string name)
        {
            try
            {
                _services[typeof(T)].Remove(name);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void RemoveService<T>(object instance)
        {
            var t = typeof(T);
            var existing = GetService<T>(string.Empty);
            if (existing.Equals(instance) && _services[t].Count == 1)
            {
                _services.Remove(t);
            }
            else if (_services[t].Count != 1)
            {
                throw new InvalidOperationException(
                    "More than one instance of this type is registered. "
                    + "There are likely named versions of this type registered."
                    + "Consider using <cref RemoveService<T>(string name) /> instead.");
            }
            else
            {
                throw new InvalidOperationException("Cannot remove a different instance than what was registered.");
            }
        }

        public void TryRemoveService<T>(string name)
        {
            if (_services[typeof(T)].ContainsKey(name))
                RemoveService<T>(name);
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

        private static T Construct<T>(object[] arguments) where T : class
        {
            var constructor = typeof(T).GetConstructor(arguments.Select(arguments => arguments.GetType()).ToArray());
            var instance = constructor.Invoke(arguments);
            return instance as T;
        }
    }
}