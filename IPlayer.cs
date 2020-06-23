using TestGame.Enums;
using Microsoft.Xna.Framework;

namespace TestGame
{
    public interface IPlayer : IGameComponent
    {
        Direction CurrentDirection { get; set; }
        bool IsMoving { get; set; }
        Vector2 MoveVector { get; set; }
        Rectangle Destination { get; }
        int Width { get; }
        int Height { get; }
    }
}