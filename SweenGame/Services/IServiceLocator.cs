using System;

namespace SweenGame.Services
{
    public interface IServiceLocator
    {
        bool PrintDebug { get; set; }

        TImpl AddService<T, TImpl>(string name = null, params object[] arguments) where TImpl : class;
        void AddService<T, TImpl>(TImpl instance, string name = null) where TImpl : class;

        void RemoveService<T>(object instance);
        void RemoveService<T>(string name);

        T GetService<T>(string name = null);
        void TryRemoveService<T>(string name);
        void Print(string message);
    }
}