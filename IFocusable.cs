using Microsoft.Xna.Framework;

namespace TestGame
{
    public interface IFocusable 
    {
        Vector2 Position {get;}

        void Add(ICamera2D camera);
    }
}