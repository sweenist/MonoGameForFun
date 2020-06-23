using System;

namespace TestGame.Services
{
    public interface IServiceLocator
    {
        void AddService<T>(Type implementation, params object[] arguments);
        void AddService<T>(object instance);
        T GetService<T>();
    }
}