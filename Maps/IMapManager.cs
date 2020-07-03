using TestGame.Enums;

namespace TestGame.Maps
{
    public interface IMapManager
    {
        IMap CurrentMap { get; }
        bool IsInTransition {get;}
        void Transition(Direction direction);
    }
}