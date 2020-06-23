using System;

namespace TestGame.Services
{
    public interface IServiceLocator
    {
        void AddService<T>(Type implementation, params object[] arguments);
        T GetService<T>();
    }
}