namespace TestGame.Services
{
    public interface IServiceLocator
    {
        void AddService<T>(object implementation, params object[] arguments);
        T GetService<T>();
    }
}