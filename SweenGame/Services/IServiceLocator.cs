using System;

namespace SweenGame.Services
{
    public interface IServiceLocator
    {
        bool PrintDebug { get; set; }

        void AddService<T>(Type implementation, params object[] arguments);
        void AddService<T>(object instance, string name = null);
        void RemoveService<T>(object instance);
        void RemoveService<T>(string name);
        T GetService<T>(string name = null);
        void TryRemoveService<T>(string name);
        void Print(string message);
    }
}