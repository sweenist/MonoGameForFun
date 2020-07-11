using SweenGame.Enums;

namespace SweenGame.Maps
{
    public interface IMapManager
    {
        IMap CurrentMap { get; }
        bool IsInTransition {get;}
        void SlideTransition(Direction direction);

        void AreaTransition();
    }
}