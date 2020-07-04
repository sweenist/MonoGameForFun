using System;

namespace TestGame.Services
{
    public interface IServiceLocator
    {
        void AddService<T>(Type implementation, params object[] arguments);
        void AddService<T>(object instance, string name = null);
        void RemoveService<T>(object instance);
        void RemoveService<T>(string name);
        T GetService<T>(string name = null);
    }
}