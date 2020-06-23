using Microsoft.Xna.Framework;
using TestGame.Camera;

namespace TestGame
{
    public interface IFocusable 
    {
        Vector2 Position {get;}

        void Add(ICamera2D camera);
    }
}