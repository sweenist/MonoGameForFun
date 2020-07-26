using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace SweenGame.Screens
{
    public interface IScreenManager
    {
        bool TraceEnabled { get; set; }
        int ScreenCount { get; }
        SpriteBatch SpriteBatch { get; }
        SpriteFont SpriteFont { get; }
        ContentManager Content { get; }

        void AddScreen(GameScreen screen);
        void RemoveScreen(GameScreen screen);
        void FadeBackBufferToBlack(int alpha);
        void ClearBuffer();
        void RemoveAllScreens();

        Game GetGame();
        Viewport GetViewport();
    }
}