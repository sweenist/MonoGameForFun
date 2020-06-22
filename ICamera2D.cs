using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame
{
    public interface ICamera2D
    {
        Vector2 Position { get; set; }
        Vector2 Origin { get; }
        Vector2 ScreenCenter { get; }
        Matrix Transform { get; }
        IFocusable FocalPoint {get; set;}
        float MoveSpeed { get; set; }
        float Rotation { get; set; }
        float Scale { get; set; }

        bool IsInView(Vector2 position, Texture2D texture);
    }
}