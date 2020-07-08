using SweenGame.Enums;

namespace SweenGame.Maps
{
    public interface IMapManager
    {
        IMap CurrentMap { get; }
        bool IsInTransition {get;}
        void Transition(Direction direction);
    }
}