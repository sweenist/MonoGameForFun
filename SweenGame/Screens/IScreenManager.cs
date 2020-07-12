using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SweenGame.Screens
{
    public interface IScreenManager
    {
        bool TraceEnabled { get; set; }
        SpriteBatch SpriteBatch { get; }
        SpriteFont SpriteFont { get; }
        GraphicsDeviceManager GraphicsManager { get; }
        ContentManager Content { get; }

        void AddScreen(GameScreen screen);
        void RemoveScreen(GameScreen screen);
        void FadeBackBufferToBlack(int alpha);
        GameScreen[] GetScreens();
        Game GetGame();
        GraphicsDevice GetGraphicsDevice();
    }
}