using TestGame.Enums;

namespace TestGame.Maps
{
    public interface IMapManager
    {
        IMap CurrentMap { get; }
        void Transition(Direction direction);
    }
}