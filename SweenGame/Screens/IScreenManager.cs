using Microsoft.Xna.Framework;

namespace SweenGame.Screens
{
    public interface IScreenManager
    {
        bool TraceEnabled { get; set; }

        void AddScreen(GameScreen screen);
        void RemoveScreen(GameScreen screen);
        GameScreen[] GetScreens();

        Game GetGame();
    }
}